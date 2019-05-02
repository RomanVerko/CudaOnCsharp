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
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void button2_Click(object sender, EventArgs e) // Matrix Multiply
        {
            SidePanel.Height = button2.Height;
            SidePanel.Top = button2.Top;
            imageProcessingControl1.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e) // Image processing
        {
            SidePanel.Height = button1.Height;
            SidePanel.Top = button1.Top;
            matrixMultControl1.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button3.Height;
            SidePanel.Top = button3.Top;
            additionControl1.BringToFront();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button4.Height;
            SidePanel.Top = button4.Top;
            infoControl1.BringToFront();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button5.Height;
            SidePanel.Top = button5.Top;
          
        }
    }
}
