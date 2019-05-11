using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            InitializeComponent();
            label1.Text = $"    CUDA is a parallel computing platform and application programming interface (API) model created\n" +
                          $"by Nvidia. It allows software developers  and  software engineers to use a CUDA-enabled graphics \n" +
                          $"processing unit (GPU) for general purpose processing — an approach termed GPGPU (General-Purpose \n" +
                          $"computing on Graphics Processing Units). The CUDA platform is a software layer that gives direct \n" +
                          $"access to the GPU's virtual instruction set and parallel computational elements, for the execution \n" +
                          $"of compute kernels.\n\n" +
                          $"Unfortunately, he CUDA platform is designed to work only with programming languages such as C, C++. \n" +
                          $"In this program CUDA has used by Alea GPU and ILGPU open libraries, that give you oppotunity to use \n" +
                          $"full GPU device power with C#. All computing data could be saved and analysed.";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//Alea gpu link
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link.");
            }

            void VisitLink()
            {
                // Change the color of the link text by setting LinkVisited   
                // to true.  
                linkLabel1.LinkVisited = true;
                //Call the Process.Start method to open the default browser   
                //with a URL:  
                System.Diagnostics.Process.Start("http://www.quantalea.com/");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) // ilgpu link
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link.");
            }

            void VisitLink()
            {
                // Change the color of the link text by setting LinkVisited   
                // to true.  
                linkLabel2.LinkVisited = true;
                //Call the Process.Start method to open the default browser   
                //with a URL:  
                System.Diagnostics.Process.Start("http://www.ilgpu.net/");
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //Alea git link
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link.");
            }

            void VisitLink()
            {
                // Change the color of the link text by setting LinkVisited   
                // to true.  
                linkLabel3.LinkVisited = true;
                //Call the Process.Start method to open the default browser   
                //with a URL:  
                System.Diagnostics.Process.Start("https://github.com/quantalea");
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //ilgpu git link
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link.");
            }

            void VisitLink()
            {
                // Change the color of the link text by setting LinkVisited   
                // to true.  
                linkLabel4.LinkVisited = true;
                //Call the Process.Start method to open the default browser   
                //with a URL:  
                System.Diagnostics.Process.Start("https://github.com/m4rs-mt/ILGPU");
            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link.");
            }

            void VisitLink()
            {
                // Change the color of the link text by setting LinkVisited   
                // to true.  
                linkLabel5.LinkVisited = true;
                //Call the Process.Start method to open the default browser   
                //with a URL:  
                System.Diagnostics.Process.Start("https://github.com/RomanVerko/CudaOnCsharp");
            }
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link.");
            }

            void VisitLink()
            {
                // Change the color of the link text by setting LinkVisited   
                // to true.  
                linkLabel6.LinkVisited = true;
                //Call the Process.Start method to open the default browser   
                //with a URL:  
                System.Diagnostics.Process.Start("https://developer.nvidia.com/cuda-zone");
            }
        }
    }
}
