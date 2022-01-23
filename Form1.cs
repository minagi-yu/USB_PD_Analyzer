using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace USB_PD_Analyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ListComPort()
		{
            string[] ports = SerialPort.GetPortNames();
            comPortSelecter.Items.Clear();
            foreach (string port in ports)
            {
                comPortSelecter.Items.Add(port);
            }
            if (comPortSelecter.Items.Count > 0)
                comPortSelecter.SelectedIndex = 0;
        }

		private void Form1_Load(object sender, EventArgs e)
		{
            ListComPort();
        }

		private void ComPortSelecter_DropDown(object sender, EventArgs e)
		{
			ListComPort();
		}

		private void ComPortConnectButton_Click(object sender, EventArgs e)
		{
            serialPort.Close();
            serialPort.BaudRate = 460800;
            System.Diagnostics.Debug.WriteLine(comPortSelecter.SelectedItem.ToString());
            serialPort.PortName = comPortSelecter.SelectedItem.ToString();
			try
			{
                serialPort.Open();
			}
			catch (Exception)
			{
                statusLabel1.Text = String.Format("{0} Connect Error.", serialPort.PortName);
                return;
			}
            statusLabel1.Text = String.Format("{0} Connected.", serialPort.PortName);
        }

		private void ComPortDisonnectButton_Click(object sender, EventArgs e)
		{
            serialPort.Close();
            statusLabel1.Text = String.Format("{0} Disconnected.", serialPort.PortName);
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string line = serialPort.ReadLine();

                BeginInvoke(new Action(() => serialConsole.AppendText(line + "\r\n")));
            }
            catch (Exception)
            {
            }
        }

		private void ClearButton_Click(object sender, EventArgs e)
		{
            serialConsole.Clear();
		}
	}
}
