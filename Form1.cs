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
		List<PdPacket> packets = new List<PdPacket>();

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
			string line;

			try
			{
				line = serialPort.ReadLine();

				BeginInvoke(new Action(() => serialConsole.AppendText(line + "\r\n")));
			}
			catch (Exception)
			{
				return;
			}
		}

		private void AnalyzeButton_Click(object sender, EventArgs e)
		{
			int i = 0;
			List<ListViewItem> listViewItem = new List<ListViewItem>();

			packets.Clear();
			foreach (string line in serialConsole.Lines)
			{
				if (line == "")
					continue;

				packets.Add(new PdPacket(line));
			}

			timeLine.Items.Clear();
			foreach (var packet in packets)
			{
				listViewItem.Add(AddTimeLine(packet, ++i));
			}

			timeLine.Items.AddRange(listViewItem.ToArray());
			timeLine.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		private void ExportButton_Click(object sender, EventArgs e)
		{
			string fileName;

			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter = "Text (*.txt)|*.txt";

				DialogResult dialogResult = saveFileDialog.ShowDialog(this);

				if (dialogResult == DialogResult.Cancel)
				{
					return;
				}

				fileName = saveFileDialog.FileName;
			}

			try
			{
				System.IO.File.WriteAllText(fileName, serialConsole.Text);
			}
			catch (Exception ex)
			{
				statusBar.Text = ex.Message;
			}
			
		}

		private void ImportButton_Click(object sender, EventArgs e)
		{
			string text;

			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "Text (*.txt)|*.txt";

				DialogResult dialogResult = openFileDialog.ShowDialog(this);

				if (dialogResult == DialogResult.Cancel)
				{
					return;
				}

				text = System.IO.File.ReadAllText(openFileDialog.FileName);
			}

			try
			{
				serialConsole.Text = text;
			}
			catch (Exception ex)
			{
				statusBar.Text = ex.Message;
			}
		}

		private void ClearButton_Click(object sender, EventArgs e)
		{
			serialConsole.Clear();
			packets.Clear();
			timeLine.Items.Clear();
		}

		private void TimeLine_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private ListViewItem AddTimeLine(PdPacket packet, int number)
		{
			List<string> row = new List<string>();
			ListViewItem listViewItem = new ListViewItem(number.ToString());

			if (packet.preamble == null)
			{
				row.Add("");
			}
			else
			{
				row.Add(BitConverter.ToString(packet.preamble));
			}

			if (packet.sop == null)
			{
				row.Add("");
				row.Add("");
			}
			else
			{
				row.Add(packet.sop.GetSopString());
				row.Add(packet.sop.GetSopTypeString());
			}

			if (packet.header == null)
			{
				for (int i = 0; i < 6; i++)
				{
					row.Add("");
				}
			}
			else
			{
				row.Add(packet.header.value.ToString("X4"));
				row.Add(packet.header.MessageID.ToString());
				if (packet.header is PdHeaderSop)
				{
					if (((PdHeaderSop)packet.header).PortPowerRole == 0)
					{
						row.Add("Sink");
						listViewItem.BackColor = Color.White;
					}
					else
					{
						row.Add("Source");
						listViewItem.BackColor = Color.LightCyan;
					}
					row.Add(((PdHeaderSop)packet.header).PortDataRole == 0 ? "UFP" : "DFP");
				} else if (packet.header is PdHeaderSopPrime)
				{
					row.Add(((PdHeaderSopPrime)packet.header).CablePlug == 0 ? "DFP or UFP" : "Cable Plug");
					row.Add("");
				}
				else
				{
					row.Add("");
					row.Add("");
				}
				row.Add(packet.header.GetMessageTypeString());
				row.Add(packet.header.NumberOfDataObjects.ToString());
			}

			if (packet.data == null)
			{
				row.Add("");
			}
			else
			{
				List<string> pds = new List<string>();
				foreach (var pdDataObject in packet.data)
				{
					pds.Add(pdDataObject.dataObject.ToString("X8"));
				}
				row.Add(string.Join(" ", pds));
			}

			row.Add(packet.eop == 0x16 ? "EOP" : "Error");

			listViewItem.SubItems.AddRange(row.ToArray());

			return listViewItem;
		}
	}
}
