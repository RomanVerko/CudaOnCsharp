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
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class AdditionControl : UserControl
    {
        public AdditionControl()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt";
            textBox3.Multiline = true;
            textBox3.ScrollBars = ScrollBars.Vertical;
            textBox3.AcceptsReturn = true;
            textBox3.AcceptsTab = true;
            textBox3.WordWrap = true;
        }
        int dimention,loopCount;
        /// <summary>
        /// start processing 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out dimention) || dimention < 1 || dimention > 100000000)
            {
                MessageBox.Show(" 'size' value is outside the supported range ");
                return;
            }
            if (!int.TryParse(textBox2.Text, out loopCount) || loopCount < 1 || loopCount > 1000000)
            {
                MessageBox.Show("'loop' value is outside the supported range");
                return;
            }
            if(checkBox1.Checked || checkBox2.Checked || checkBox3.Checked ) textBox3.Text += $"{Environment.NewLine}----- size = {dimention} , {loopCount} loops -----{Environment.NewLine}";

            progressBar1.Value = 0;
            int checkedNums = 0;
            if (checkBox1.Checked) checkedNums++;
            if (checkBox2.Checked) checkedNums++;
            if (checkBox3.Checked) checkedNums++;
            progressBar1.Maximum = loopCount * checkedNums;
            progressBar1.Step = 1;
            if (checkBox1.Checked)
            {
                Gpu2(dimention, loopCount);
            }
            if (checkBox2.Checked)
            {
                CpuParallel(dimention, loopCount);
            }
            if (checkBox3.Checked)
            {
                 Cpu(dimention, loopCount);
            }
        }

        [GpuManaged]
        public void Gpu2(int n, int loops)
        {
           
            var arg1 = Enumerable.Range(0, n).ToArray();
            var arg2 = Enumerable.Range(0, n).ToArray();
            var result = new int[n];

            var gpu = Gpu.Default;
            Stopwatch sw = new Stopwatch();
            gpu.For(0, 100, i => i++);
            sw.Start();
            for (int j = 0; j < loops; j++)
            {
                gpu.For(0, result.Length, i => result[i] = arg1[i] + arg2[i]);
                progressBar1.PerformStep();
            }
            sw.Stop();
           textBox3.Text+= $"GPU Parallel: {sw.Elapsed}{Environment.NewLine}";
        }

        public void CpuParallel(int n, int loops)
        {
           
            var arg1 = Enumerable.Range(0, n).ToArray();
            var arg2 = Enumerable.Range(0, n).ToArray();
            var result = new int[n];
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int j = 0; j < loops; j++)
            {
                Parallel.For(0, result.Length, i => result[i] = arg1[i] + arg2[i]);
                progressBar1.PerformStep();
            }
            sw.Stop();
            textBox3.Text += $"CPU Parallel: {sw.Elapsed}{Environment.NewLine}";
        }

        

        public void Cpu(int n, int loops)
        {
           
            var arg1 = Enumerable.Range(0, n).ToArray();
            var arg2 = Enumerable.Range(0, n).ToArray();
            var result = new int[n];
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int j = 0; j < loops; j++)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = arg1[i] + arg2[i];
                }
                progressBar1.PerformStep();
            }
            sw.Stop();
            textBox3.Text += $"CPU: {sw.Elapsed}{Environment.NewLine}";
        }

        private void button2_Click(object sender, EventArgs e) // saving
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, textBox1.Text);
            MessageBox.Show("Saved successfully");
        }
    }


}
