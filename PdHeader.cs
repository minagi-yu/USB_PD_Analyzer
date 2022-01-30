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
			Reject=4,
			Ping=5,
			PS_RDY=6,
			Get_Source_Cpa =7,
			Get_Sink_Cap=8,
			DR_Swap=9,
			PR_Swap=10,
			VCONN_SWAP=11,
			Wait=12,
			Soft_Reset=13,

			// Data Message Types
			Source_Capabilities=1,
			Request=2,
			BIST=3,
			Sink_Capabilities=4,
			Vendor_Defined=15
		}

		public readonly ushort header;

		public PdHeader(ushort d)
		{
			header = d;
		}

		public int NumberOfDataObjects
		{
			get => (header >> 12) & 0x07;
		}
		public int MessageID
		{
			get => (header >> 9) & 0x07;
		}
		public int SpecificationRevision
		{
			get => (header >> 6) & 0x03;
		}
		public MessageTypes MessageType
		{
			get => (MessageTypes)(header & 0x0f);
		}
	}

	internal class PdHeaderSop : PdHeader
	{
		public PdHeaderSop(ushort d) : base(d) { }

		public int PortPowerRole
		{
			get => (header >> 8) & 0x01;
		}
		public int PortDataRole
		{
			get => (header >> 5) & 0x01;
		}
	}

	internal class PdHeaderSopPrime : PdHeader
	{
		public PdHeaderSopPrime(ushort d) : base(d) { }

		public int CablePlug
		{
			get => (header >> 8) & 0x01;
		} 
	}
}
