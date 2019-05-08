using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SidePanel.Height = button1.Height;
            matrixMultControl1.BringToFront();
            label1.Text = "         Instructions";
            instructionControl1.BringToFront();
            SidePanel.Visible = false;

        }
        /// <summary>
        /// closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// minimising
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void button2_Click(object sender, EventArgs e) // Matrix Multiply
        {
            SidePanel.Visible = true;
              label1.Text = "   Image processing";
            SidePanel.Height = button2.Height;
            SidePanel.Top = button2.Top;
            imageProcessingControl1.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e) // Image processing
        {
            SidePanel.Visible = true;
            label1.Text = "  Matrix multiplication";
            SidePanel.Height = button1.Height;
            SidePanel.Top = button1.Top;
            matrixMultControl1.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e) // Componentwise addition
        {
            SidePanel.Visible = true;
            label1.Text = "Componentwise addition";
            SidePanel.Height = button3.Height;
            SidePanel.Top = button3.Top;
            additionControl1.BringToFront();
            
        }

        private void button4_Click(object sender, EventArgs e) // GPU information
        {
            SidePanel.Visible = true;
            label1.Text = "GPU device information";
            SidePanel.Height = button4.Height;
            SidePanel.Top = button4.Top;
            infoControl1.BringToFront();
            
        }

        private void button5_Click(object sender, EventArgs e) // instructions
        {
            SidePanel.Visible = true;
            label1.Text = "             About";
            SidePanel.Height = button5.Height;
            SidePanel.Top = button5.Top;
            aboutControl1.BringToFront();
          
        }
    }
}
