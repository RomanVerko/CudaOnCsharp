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
    public partial class MatrixMultControl : UserControl
    {
        public MatrixMultControl()
        {
            InitializeComponent();
            // Set the Multiline property to true.
            textBox1.Multiline = true;
            // Add vertical scroll bars to the TextBox control.
            textBox1.ScrollBars = ScrollBars.Vertical;
            // Allow the TAB key to be entered in the TextBox control.
            textBox1.AcceptsReturn = true;
            // Allow the TAB key to be entered in the TextBox control.
            textBox1.AcceptsTab = true;
            // Set WordWrap to true to allow text to wrap to the next line.
            textBox1.WordWrap = true;

        }
        int dimention = 10;
        /// <summary>
        /// Matrix multiplication from n=10 to inf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            dimention = 10;
            
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
