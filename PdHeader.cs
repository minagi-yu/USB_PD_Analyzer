using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USB_PD_Analyzer
{
	internal class PdHeader
	{
		public enum MessageTypes
		{
			// Control Message Types
			GoodCRC = 1,
			GotoMin = 2,
			Accept = 3,
			Reject = 4,
			Ping = 5,
			PS_RDY = 6,
			Get_Source_Cap = 7,
			Get_Sink_Cap = 8,
			DR_Swap = 9,
			PR_Swap = 10,
			VCONN_Swap = 11,
			Wait = 12,
			Soft_Reset = 13,

			// Data Message Types
			Source_Capabilities = 1,
			Request = 2,
			BIST = 3,
			Sink_Capabilities = 4,
			Vendor_Defined = 15
		}

		public readonly ushort value;

		public PdHeader(ushort d)
		{
			value = d;
		}

		public int NumberOfDataObjects => (value >> 12) & 0x07;
		public int MessageID => (value >> 9) & 0x07;
		public int SpecificationRevision => (value >> 6) & 0x03;
		public MessageTypes MessageType => (MessageTypes)(value & 0x0f);

		public string GetMessageTypeString()
		{
			if (this.NumberOfDataObjects == 0)
			{
				switch (this.MessageType)
				{
					case MessageTypes.GoodCRC:
						return "GoodCRC";
					case MessageTypes.GotoMin:
						return "GotoMin";
					case MessageTypes.Accept:
						return "Accept";
					case MessageTypes.Reject:
						return "Reject";
					case MessageTypes.Ping:
						return "Ping";
					case MessageTypes.PS_RDY:
						return "PS_RDY";
					case MessageTypes.Get_Source_Cap:
						return "Get_Source_Cap";
					case MessageTypes.Get_Sink_Cap:
						return "Get_Sink_Cap";
					case MessageTypes.DR_Swap:
						return "DR_Swap";
					case MessageTypes.PR_Swap:
						return "PR_Swap";
					case MessageTypes.VCONN_Swap:
						return "VCONN_Swap";
					case MessageTypes.Wait:
						return "Wait";
					case MessageTypes.Soft_Reset:
						return "Soft_Reset";
					default:
						return "Error;";
				}
			}
			else
			{
				switch (this.MessageType)
				{
					case MessageTypes.Source_Capabilities:
						return "Source_Capabilities";
					case MessageTypes.Request:
						return "Request";
					case MessageTypes.BIST:
						return "BIST";
					case MessageTypes.Sink_Capabilities:
						return "Sink_Capabilities";
					case MessageTypes.Vendor_Defined:
						return "Vendor_Defined";
					default:
						return "Error";
				}
			}
		}
	}

	internal class PdHeaderSop : PdHeader
	{
		public PdHeaderSop(ushort d) : base(d) { }

		public int PortPowerRole
		{
			get => (value >> 8) & 0x01;
		}
		public int PortDataRole
		{
			get => (value >> 5) & 0x01;
		}
	}

	internal class PdHeaderSopPrime : PdHeader
	{
		public PdHeaderSopPrime(ushort d) : base(d) { }

		public int CablePlug
		{
			get => (value >> 8) & 0x01;
		}
	}
}