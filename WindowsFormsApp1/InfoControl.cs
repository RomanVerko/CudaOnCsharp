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
    public partial class InfoControl : UserControl
    {
        public InfoControl()
        {
            InitializeComponent();
            textBox1.Multiline = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.AcceptsReturn = true;
            textBox1.AcceptsTab = true;
            textBox1.WordWrap = true;
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt";
        }
        /// <summary>
        /// displaying info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            var devices = Device.Devices;
            var numGpus = devices.Length;
            foreach (var device in devices)
            {
                textBox1.Text += $"Name : { device.Name}{Environment.NewLine}" +
                    $"Architecture : { device.Arch.ToString()}{Environment.NewLine}" +
                    $"Max threads per multiprocessor : {device.Attributes.MaxThreadsPerMultiprocessor}{Environment.NewLine}" +
                    $"Max threads per block : {device.Attributes.MaxThreadsPerBlock}{Environment.NewLine}" +
                    $"Warp size : {device.Attributes.WarpSize}{Environment.NewLine}" +
                    $"Memory clock rate : { device.Attributes.MemoryClockRate} Hz{Environment.NewLine}" +
                    $"Cores : { device.Cores.ToString()}{Environment.NewLine}" +
                    $"Max grid size : {device.Properties.MaxGridSize}{Environment.NewLine}" +
                    $"Max thread dimention : { device.Properties.MaxThreadsDim}{Environment.NewLine}" +
                    $"Shared memory per block : {device.Properties.SharedMemPerBlock}{Environment.NewLine}" +
                    $"Total memory : {device.TotalMemory.ToString()} bytes, {device.TotalMemory/1024/1024} MBytes{Environment.NewLine}" +
                    $"Device type : {device.Type}{Environment.NewLine}{Environment.NewLine}";
                
                var id = device.Id;
                var arch = device.Arch;
                var numMultiProc = device.Attributes.MultiprocessorCount;
            }

            // all device ids
            var deviceIds = devices.Select(device => device.Id);
        }
        /// <summary>
        /// saving data
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
    }
}
