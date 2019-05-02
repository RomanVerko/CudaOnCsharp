using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alea;
using Alea.Parallel;

namespace WindowsFormsApp1
{
    public partial class MatrixMultControl : UserControl
    {
        public MatrixMultControl()
        {
            InitializeComponent(); 
            textBox1.Multiline = true;
            textBox1.ScrollBars = ScrollBars.Vertical;     
            textBox1.AcceptsReturn = true;       
            textBox1.AcceptsTab = true;
            textBox1.WordWrap = true;
            trackBar1.Scroll += trackBar1_Scroll;
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|Csv(*.csv)|*.csv";
        }
        int n, step, StepNumber = 0;
        static Stopwatch gpuTime = new Stopwatch();
        string data, CsvData;
        

        /// <summary>
        /// Matrix multiplication from n=10 to inf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox2.Text, out step) || step<1|| step > 500 || !int.TryParse(textBox3.Text,out n) || n<1||n>1500)
            {
                MessageBox.Show("Wrong step or start dimention");
                return;
            }

            Stopwatch sw = new Stopwatch();
            CsvData = $"n;GPU;CPU TPL;CPU{Environment.NewLine}";
            label5.Visible = true;
            
            try
            {
                while (true)
                {
                    
                    double[][] ma = MatrixCreate(n, n);
                    double[][] mb = MatrixCreate(n, n);
                    double[][] prod, prod2, prod3;

                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            ma[i][j] = i + j;
                            mb[i][j] = i + j;
                        }
                    }

                    textBox1.Text += $"{Environment.NewLine}\t---- n={n} ----{Environment.NewLine}";

                    if (checkBox1.Checked)
                    {
                        prod3 = GpuParallelMatrixProduct(ma, mb);
                        textBox1.Text += $"GPU Parallel: {gpuTime.Elapsed}{Environment.NewLine}";
                        CsvData += $"{n};{gpuTime.ElapsedMilliseconds};";
                    }

                    if (checkBox3.Checked)
                    {
                        sw.Start();
                        prod = MatrixProduct(ma, mb);
                        sw.Stop();
                        textBox1.Text += $"CPU: {sw.Elapsed}{Environment.NewLine}";
                        CsvData += $"{sw.ElapsedMilliseconds};";
                        sw.Reset();
                    }
                    if (checkBox2.Checked)
                    {
                        sw.Start();
                        prod2 = CpuParallelMatrixProduct(ma, mb);
                        sw.Stop();
                        textBox1.Text += $"CPU Parallel: {sw.Elapsed}{Environment.NewLine}";
                        CsvData += $"{sw.ElapsedMilliseconds}{Environment.NewLine}";
                        sw.Reset();
                    }

                    StepNumber++;
                    if (StepNumber % 10 == 0)
                    {
                        DialogResult dialogResult = MessageBox.Show($"Number of steps is {StepNumber}\nCurrent dimention is {n}\nAre you want to continue?", "Step counter", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            break;
                        }
                    }
                    n += step;
                    //TODO make work twice
                }
            }
            catch (Exception ex)
            {
                textBox1.Text += $"{Environment.NewLine} Memory is over. This is more than your GPU capabilities. {Environment.NewLine}";
                label5.Visible = false;
               // Gpu.FreeAllImplicitMemory();
            }

            label5.Visible = false;
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = String.Format($"N = {trackBar1.Value}");
        }

        static double[][] MatrixCreate(int rows, int cols)
        {
            // do error checking here
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
            {
                result[i] = new double[cols];
            }
            return result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            Stopwatch sw = new Stopwatch();
            n = trackBar1.Value;
            double[][] ma = MatrixCreate(n, n);
            double[][] mb = MatrixCreate(n, n);
            double[][] prod, prod2, prod3;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    ma[i][j] = i + j;
                    mb[i][j] = i + j;
                }
            }

            textBox1.Text += $"{Environment.NewLine}\t---- n={n} ----{Environment.NewLine}";

            if (checkBox1.Checked)
            {
                prod3 = GpuParallelMatrixProduct(ma, mb);
                textBox1.Text += $"GPU Parallel: {gpuTime.Elapsed}{Environment.NewLine}";
                
            }

            if (checkBox3.Checked)
            {
                sw.Start();
                prod = MatrixProduct(ma, mb);
                sw.Stop();
                textBox1.Text += $"CPU: {sw.Elapsed}{Environment.NewLine}";
                
                sw.Reset();
            }
            if (checkBox2.Checked)
            {
                sw.Start();
                prod2 = CpuParallelMatrixProduct(ma, mb);
                sw.Stop();
                textBox1.Text += $"CPU Parallel: {sw.Elapsed}{Environment.NewLine}";
                
                sw.Reset();
            }
           

        }
        /// <summary>
        /// Save file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e) // saving
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, textBox1.Text);
            string filename2 = filename.Replace(".txt", ".csv");
            System.IO.File.WriteAllText(filename2, CsvData);
            MessageBox.Show("Saved successfully");
        }

        static double[][] MatrixProduct(double[][] matrixA, double[][] matrixB)
        {
            int aRows = matrixA.Length; int aCols = matrixA[0].Length;
            int bRows = matrixB.Length; int bCols = matrixB[0].Length;

            double[][] result = MatrixCreate(aRows, bCols);

            for (int i = 0; i < aRows; ++i)
            { // each row of A
                for (int j = 0; j < bCols; ++j)
                { // each col of B
                    for (int k = 0; k < aCols; ++k)
                    { // could use k < bRows
                        result[i][j] += matrixA[i][k] * matrixB[k][j];
                    }
                }
            }
            return result;
        }

        static double[][] CpuParallelMatrixProduct(double[][] matrixA, double[][] matrixB)
        {
            int aRows = matrixA.Length; int aCols = matrixA[0].Length;
            int bRows = matrixB.Length; int bCols = matrixB[0].Length;

            double[][] result = MatrixCreate(aRows, bCols);

            Parallel.For(0, aRows, i =>
            {
                for (int j = 0; j < bCols; ++j)
                { // each col of B
                    for (int k = 0; k < aCols; ++k)
                    { // could use k < bRows
                        result[i][j] += matrixA[i][k] * matrixB[k][j];
                    }
                }
            }
            );

            return result;
        }


        [GpuManaged]
        static double[][] GpuParallelMatrixProduct(double[][] matrixA, double[][] matrixB)
        {
            int aRows = matrixA.Length; int aCols = matrixA[0].Length;
            int bRows = matrixB.Length; int bCols = matrixB[0].Length;

            double[][] result = MatrixCreate(aRows, bCols);
            gpuTime.Reset();

            Alea.Gpu.Default.For(0, 10, i => i++);
            gpuTime.Start();
            Gpu.Default.For(0, aRows, i =>
            {
                for (int j = 0; j < bCols; ++j)
                { // each col of B
                    for (int k = 0; k < aCols; ++k)
                    { // could use k < bRows
                        result[i][j] += matrixA[i][k] * matrixB[k][j];
                    }
                }
            }
            );
            gpuTime.Stop();


            return result;
        }
    }
}
