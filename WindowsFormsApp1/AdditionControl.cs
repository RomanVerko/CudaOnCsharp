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


namespace WindowsFormsApp1
{
    public partial class AdditionControl : UserControl
    {
        public AdditionControl()
        {
            InitializeComponent();
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
                MessageBox.Show("'size' value is outside the supported range ");
                return;
            }
            if (!int.TryParse(textBox2.Text, out loopCount) || loopCount < 1 || loopCount > 1000000)
            {
                MessageBox.Show("'loop' value is outside the supported range");
                return;
            }

        }

        [GpuManaged]
        public static void Gpu2(int n)
        {
            var arg1 = Enumerable.Range(0, n).ToArray();
            var arg2 = Enumerable.Range(0, n).ToArray();
            var result = new int[n];

            var gpu = Gpu.Default;
            Stopwatch sw = new Stopwatch();
            gpu.For(0, 100, i => i++);
            sw.Start();
            for (int j = 0; j < 100000; j++)
            {
                gpu.For(0, result.Length, i => result[i] = arg1[i] + arg2[i]);

            }
            sw.Stop();
            System.Console.WriteLine("GPU:" + sw.Elapsed);
        }
    }


}
