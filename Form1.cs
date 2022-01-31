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
				listViewItem.Add(GetTimeLineItem(packet, ++i));
			}

			timeLine.BeginUpdate();
			timeLine.Items.AddRange(listViewItem.ToArray());
			timeLine.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			timeLine.EndUpdate();
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
			detailView.Items.Clear();
		}

		private void TimeLine_SelectedIndexChanged(object sender, EventArgs e)
		{
			int number = 0;

			if (timeLine.SelectedIndices.Count != 1)
				return;

			foreach (int i in timeLine.SelectedIndices)
				number = i;

			if (number > packets.Count)
				return;

			detailView.BeginUpdate();
			detailView.Items.Clear();
			detailView.Groups.Clear();
			ShowDetailView(packets[number]);
			detailView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			detailView.EndUpdate();
		}

		private ListViewItem GetTimeLineItem(PdPacket packet, int number)
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
				row.Add(string.Join(" ", packet.sop.GetSopString()));
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
				}
				else if (packet.header is PdHeaderSopPrime)
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

		private void ShowDetailView(PdPacket packet)
		{
			if (packet.preamble == null)
				return;
			ListViewGroup preambleGroup = new ListViewGroup("Preamble");
			detailView.Groups.Add(preambleGroup);
			detailView.Items.Add(new ListViewItem(new string[] { "Preamble", BitConverter.ToString(packet.preamble) }, preambleGroup));

			if (packet.sop == null)
				return;
			ListViewGroup sopGroup = new ListViewGroup("SOP");
			detailView.Groups.Add(sopGroup);
			detailView.Items.Add(new ListViewItem(new String[] { "SOP*", packet.sop.GetSopTypeString() }, sopGroup));
			foreach (var (v, i) in packet.sop.GetSopString().Select((v, i) => (v, i)))
			{
				detailView.Items.Add(new ListViewItem(new String[] { string.Format("K-code {0}", i + 1), v }, sopGroup));
			}

			if (packet.header == null)
				return;
			ListViewGroup headerGroup = new ListViewGroup(String.Format("Header - 0x{0}", packet.header.value.ToString("X4")));
			detailView.Groups.Add(headerGroup);
			detailView.Items.Add(new ListViewItem(new String[] { "Number of Data Objects (14:12)", packet.header.NumberOfDataObjects.ToString() }, headerGroup));
			detailView.Items.Add(new ListViewItem(new String[] { "Message ID (11:9)", packet.header.MessageID.ToString() }, headerGroup));
			if (packet.header is PdHeaderSop headerSop)
			{
				detailView.Items.Add(new ListViewItem(new String[] { "Port Power Role (8)", headerSop.PortPowerRole == 0 ? "Sink" : "Source" }, headerGroup));
				detailView.Items.Add(new ListViewItem(new String[] { "Specification Revision (7:6)", (headerSop.SpecificationRevision + 1).ToString() }, headerGroup));
				detailView.Items.Add(new ListViewItem(new String[] { "Port Data Role (5)", headerSop.PortDataRole == 0 ? "UFP" : "DFP" }, headerGroup));
			}
			else if (packet.header is PdHeaderSopPrime headerSopPrime)
			{
				detailView.Items.Add(new ListViewItem(new String[] { "Cable Plug (8)", headerSopPrime.CablePlug == 0 ? "DFP or UFP" : "Cable Plug" }, headerGroup));
				detailView.Items.Add(new ListViewItem(new String[] { "Specification Revision (7:6)", (headerSopPrime.SpecificationRevision + 1).ToString() }, headerGroup));
			}
			detailView.Items.Add(new ListViewItem(new String[] { "Message Type (4:0)", packet.header.GetMessageTypeString() }, headerGroup));

			if (packet.data == null)
				return;
			foreach (var (dataPacket, i) in packet.data.Select((v, i) => (v, i)))
			{
				ListViewGroup dataGroup = new ListViewGroup(String.Format("Data - 0x{0}", dataPacket.dataObject.ToString("X8")));
				detailView.Groups.Add(dataGroup);
				if (dataPacket is PdPowerDataObject pdo)
				{
					switch (pdo.SupplyType)
					{
						case PdPowerDataObject.SourceTypes.FixedSupply:
							detailView.Items.Add(new ListViewItem(new String[] { "Supply Type (31:30)", "Fixed Supply" }, dataGroup));
							break;
						case PdPowerDataObject.SourceTypes.Battery:
							detailView.Items.Add(new ListViewItem(new String[] { "Supply Type (31:30)", "Battery" }, dataGroup));
							break;
						case PdPowerDataObject.SourceTypes.VariableSupply:
							detailView.Items.Add(new ListViewItem(new String[] { "Supply Type (31:30)", "Variable Supply" }, dataGroup));
							break;
						default:
							detailView.Items.Add(new ListViewItem(new String[] { "Supply Type (31:30)", "Reserved" }, dataGroup));
							break;
					}

					switch(pdo)
					{
						case PdSourceFixedSupplyPdo pdo1:
							detailView.Items.Add(new ListViewItem(new String[] { "Dual-Role Power (29)", pdo1.DualRolePower.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "USB Suspend Supported (28)", pdo1.UsbSuspendSupported.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Unconstrained Power (27)", pdo1.UnconstrainedPower.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "USB Communications Capable (26)", pdo1.UsbCommunicationCapable.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Dual-Role Data (25)", pdo1.DualRoleData.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Peak Current(A) (21:20)", pdo1.PeakCurrent.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Voltage(V) (19:10)", pdo1.Voltage.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Maximum Current(A) (9:0)", pdo1.MaximumCurrent.ToString() }, dataGroup));
							break;
						case PdSourceVariableSupplyPdo pdo1:
							detailView.Items.Add(new ListViewItem(new String[] { "Maximum Voltage(V) (29:20)", pdo1.MaximumVoltage.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Minimum Voltage(V) (19:10)", pdo1.MinimumVoltage.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Maximum Current(A) (9:0)", pdo1.MaximumCurrent.ToString() }, dataGroup));
							break;
						case PdSourceBatterySupplyPdo pdo1:
							detailView.Items.Add(new ListViewItem(new String[] { "Maximum Voltage(V) (29:20)", pdo1.MaximumVoltage.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Minimum Voltage(V) (19:10)", pdo1.MinimumVoltage.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Maximum Allowable Power(W) (9:0)", pdo1.MaximumAllowablePower.ToString() }, dataGroup));
							break;
						case PdSinkFixedSupplyPdo pdo1:
							detailView.Items.Add(new ListViewItem(new String[] { "Dual-Role Power (29)", pdo1.DualRolePower.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Higher Capability (28)", pdo1.HigherCapability.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Unconstrained Power (27)", pdo1.UnconstrainedPower.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "USB Communications Capable (26)", pdo1.UsbCommunicationCapable.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Dual-Role Data (25)", pdo1.DualRoleData.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Voltage(V) (19:10)", pdo1.Voltage.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Operational Current(A) (9:0)", pdo1.OperationalCurrent.ToString() }, dataGroup));
							break;
						case PdSinkVariableSupplyPdo pdo1:
							detailView.Items.Add(new ListViewItem(new String[] { "Maximum Voltage(V) (29:20)", pdo1.MaximumVoltage.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Minimum Voltage(V) (19:10)", pdo1.MinimumVoltage.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Operational Current(A) (9:0)", pdo1.OperationalCurrent.ToString() }, dataGroup));
							break;
						case PdSinkBatterySupplyPdo pdo1:
							detailView.Items.Add(new ListViewItem(new String[] { "Maximum Voltage(V) (29:20)", pdo1.MaximumVoltage.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Minimum Voltage(V) (19:10)", pdo1.MinimumVoltage.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Operational Power(W) (9:0)", pdo1.OperationalPower.ToString() }, dataGroup));
							break;
						default:
							break;
					}
				}
				else if (dataPacket is PdRequestDataObject rdo)
				{
					switch (rdo)
					{
						case PdFixedAndVariableRequestDataObject pdo1:
							detailView.Items.Add(new ListViewItem(new String[] { "Object Position (30:28)", pdo1.ObjectPosition.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Giveback Flag (27)", pdo1.GiveBackFlag.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Capability Mismatch (26)", pdo1.CapabilityMismatch.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "USB Communications Capable (25)", pdo1.UsbCommunicationsCapable.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "No USB Suspend (24)", pdo1.NoUsbSuspended.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Operating Current(A) (19:10)", pdo1.OperatingCurrent.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Maximum Operating Current(A) (26)", pdo1.MaximumOperatingCurrent.ToString() }, dataGroup));
							break;
						case PdBatteryRequestdataObject pdo1:
							detailView.Items.Add(new ListViewItem(new String[] { "Object Position (30:28)", pdo1.ObjectPosition.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Giveback Flag (27)", pdo1.GiveBackFlag.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Capability Mismatch (26)", pdo1.CapabilityMismatch.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "USB Communications Capable (25)", pdo1.UsbCommunicationsCapable.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "No USB Suspend (24)", pdo1.NoUsbSuspended.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Operating Power(W) (19:10)", pdo1.OperatingPower.ToString() }, dataGroup));
							detailView.Items.Add(new ListViewItem(new String[] { "Maximum Operating Power(W) (26)", pdo1.MaximumOperatingPower.ToString() }, dataGroup));
							break;
						default:
							break;
					}
				}
				else
				{
					detailView.Items.Add(new ListViewItem(new String[] { "Data", String.Format("0x{0}", dataPacket.dataObject.ToString("X8")) }, dataGroup));
				}
			}

			ListViewGroup eopGroup = new ListViewGroup("EOP");
			detailView.Groups.Add(eopGroup);
			detailView.Items.Add(new ListViewItem(new String[] { "EOP", packet.eop == 0x16 ? "EOP" : "Error" }, eopGroup));
		}
	}
}
