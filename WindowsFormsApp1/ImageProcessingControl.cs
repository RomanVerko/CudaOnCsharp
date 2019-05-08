using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alea;
using Alea.Parallel;
using ILGPU;
using ILGPU.Runtime;
using System.IO;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class ImageProcessingControl : UserControl
    {
        public ImageProcessingControl()
        {
            InitializeComponent();
            textBox1.Multiline = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.AcceptsReturn = true;
            textBox1.AcceptsTab = true;
            textBox1.WordWrap = true;
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt";

        }
        string outDir, srcDir;
        public static string textBoxData;
      

        /// <summary>
        /// images processing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (srcDir == null) {MessageBox.Show("No input data"); return; }
            var imagePaths = Directory.GetFiles(srcDir);
            progressBar1.Value = 0;
            int checkedNums = 0;
            if (checkBox1.Checked) checkedNums++;
            if (checkBox2.Checked) checkedNums++;
            if (checkBox3.Checked) checkedNums++;
            if (checkBox4.Checked) checkedNums++;
            progressBar1.Maximum = imagePaths.Length*checkedNums;
            if (checkBox3.Checked)
            {
                Inverse("CPU Parallel", imagePaths, outDir, image => TplImageFilter.Apply(image, TplImageFilter.Invert));
            }

            if (checkBox1.Checked)
            {
                Console.WriteLine("Warming up GPU...");
                Alea.Gpu.Default.For(0, 100, i => i++);
                Inverse("AleaGPU", imagePaths, outDir, image => AleaGPU.Apply(image, AleaGPU.Invert));
            }
            if (checkBox2.Checked)
            {
                using (var ilGpuFilter = new IlGpuFilter())
                {
                    Inverse("ILGPU", imagePaths, outDir, image => ilGpuFilter.Apply(image, IlGpuFilter.Invert));
                }
            }
            if (checkBox4.Checked)
            {
                Inverse("Cpu_Linear", imagePaths, outDir, image => CPUImageFilter.Apply(image, CPUImageFilter.Invert));
            }
            
        }

        private void Inverse(string technology, string[] Paths, string outDir, Func<Rgba32[], Rgba32[]> transform)
        {

            textBox1.Text += $"{Environment.NewLine}Testing {technology}{Environment.NewLine}";
            
            var stopwatch = new Stopwatch();
           
            
            foreach (string imagePath in Paths)
            {
                
                textBox1.Text += $"Processing {imagePath}...{Environment.NewLine}";

                Image<Rgba32> image = SixLabors.ImageSharp.Image.Load(imagePath);
                Rgba32[] pixelArray = new Rgba32[image.Height * image.Width];
                image.SavePixelData(pixelArray);

                progressBar1.Value++;

                string imageTitle = Path.GetFileName(imagePath);

                stopwatch.Start();
                Rgba32[] transformedPixels = transform(pixelArray);
                stopwatch.Stop();

                Image<Rgba32> res = SixLabors.ImageSharp.Image.LoadPixelData(
                    config: Configuration.Default,
                    data: transformedPixels,
                    width: image.Width,
                    height: image.Height);

                res.Save(Path.Combine(outDir, $"{imageTitle}.{technology}.bmp"));
            }

            textBox1.Text += $"{technology}:\t\t{stopwatch.Elapsed}{Environment.NewLine}";
        }
        /// <summary>
        /// Saving in txt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, textBox1.Text);
            MessageBox.Show("Saved successfully");
        }

        /// <summary>
        /// open folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        { 
            if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            srcDir = folderBrowserDialog1.SelectedPath;
            // сохраняем текст в файл
            label4.Visible = true;
            label4.Text = srcDir.Split('\\')[srcDir.Split('\\').Length - 1];
            Directory.CreateDirectory(srcDir + "\\ImageResult");
            outDir = srcDir + @"\ImageResult";
        }

       

       
    }


    public class AleaGPU
    {
        [GpuManaged]
        public static Rgba32[] Apply(Rgba32[] pixelArray, Func<Rgba32, Rgba32> filter)
        {
            Stopwatch sw = new Stopwatch();
            Gpu gpu = Gpu.Default;
            sw.Start();
            gpu.For(0, pixelArray.Length, i => pixelArray[i] = filter(pixelArray[i]));
            sw.Stop();
            Console.WriteLine("inside: " + sw.Elapsed);
            return pixelArray;
        }


        public static Rgba32 Invert(Rgba32 from)
        {
            var to = new Rgba32
            {
                A = (byte)~from.A,
                R = (byte)~from.R,
                G = (byte)~from.G,
                B = (byte)~from.B
            };

            return to;
        }
    }

    public class IlGpuFilter : IDisposable
    {
        private readonly Accelerator gpu;
        private readonly Action<Index, ArrayView<Rgba32>> kernel;

        public IlGpuFilter()
        {
            this.gpu = Accelerator.Create(new ILGPU.Context(), Accelerator.Accelerators.First(a => a.AcceleratorType == AcceleratorType.Cuda));
            this.kernel = this.gpu.LoadAutoGroupedStreamKernel<Index, ArrayView<Rgba32>>(ApplyKernel);
        }

        private static void ApplyKernel(
            Index index, /* The global thread index (1D in this case) */
            ArrayView<Rgba32> pixelArray /* A view to a chunk of memory (1D in this case)*/)
        {
            pixelArray[index] = Invert(pixelArray[index]);
        }

        public Rgba32[] Apply(Rgba32[] pixelArray, Func<Rgba32, Rgba32> filter)
        {
            using (MemoryBuffer<Rgba32> buffer = this.gpu.Allocate<Rgba32>(pixelArray.Length))
            {
                buffer.CopyFrom(pixelArray, 0, Index.Zero, pixelArray.Length);

                this.kernel(buffer.Length, buffer.View);

                // Wait for the kernel to finish...
                this.gpu.Synchronize();

                return buffer.GetAsArray();
            }
        }

        public static Rgba32 Invert(Rgba32 color)
        {
            return new Rgba32(
                r: (byte)~color.R,
                g: (byte)~color.G,
                b: (byte)~color.B,
                a: (byte)~color.A);
        }

        public void Dispose()
        {
            this.gpu?.Dispose();
        }
    }

    public class TplImageFilter
    {
        public static Rgba32[] Apply(Rgba32[] pixelArray, Func<Rgba32, Rgba32> filter)
        {
            Parallel.For(0, pixelArray.Length, i => pixelArray[i] = filter(pixelArray[i]));

            return pixelArray;
        }

        public static Rgba32 Invert(Rgba32 color)
        {
            return new Rgba32(
                r: (byte)~color.R,
                g: (byte)~color.G,
                b: (byte)~color.B,
                a: (byte)~color.A);
        }
    }

    public class CPUImageFilter
    {
        public static Rgba32[] Apply(Rgba32[] pixelArray, Func<Rgba32, Rgba32> filter)
        {

            for (int i = 0; i < pixelArray.Length; i++)
            {
                pixelArray[i] = filter(pixelArray[i]);
            }

            return pixelArray;
        }

        public static Rgba32 Invert(Rgba32 color)
        {
            return new Rgba32(
                r: (byte)~color.R,
                g: (byte)~color.G,
                b: (byte)~color.B,
                a: (byte)~color.A);
        }
    }
}
