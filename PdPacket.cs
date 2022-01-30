using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USB_PD_Analyzer
{
	internal class PdPacket
	{
		static readonly byte[] symbol5b4b = new byte[]
		{
			0xff, 0xff, 0xff, 0xff,
			0xff, 0x02, 0xff, 0x0e,
			0xff, 0x08, 0x04, 0x0c,
			0xff, 0x0a, 0x06, 0x00,
			0xff, 0xff, 0x01, 0xff,
			0xff, 0x03, 0xff, 0x0f,
			0x00, 0x09, 0x05, 0x0d,
			0xff, 0x0b, 0x07, 0xff
		};

		public byte[] preamble;
		public PdSop sop;
		public PdHeader header;
		public List<PdDataObject> data;
		public uint crc;
		public byte eop;

		public PdPacket(string hex)
		{
			// Preamble:		64 bit
			// Start Of Packet:	 4 K-code * 5b
			// Message Header:	16 bit / 4b * 5b
			// Data Objects:	32 bit * 7 objs(Max) / 4b * 5b
			// CRC:				32 bit / 4b * 5b
			// End Of packet:	 1 K-code * 5b
			byte[] rawData;

			byte[] sopBinary = new byte[4];
			ushort headerBinary = 0;

			rawData = HexStringToBinary(hex).ToArray();
			int len = rawData.Length;

			if (len < 8)
				return;

			preamble = new byte[8];
			for (int i = 0; i < 8; i++)
			{
				preamble[i] = rawData[i];
			}

			int bitPosition = 8 * 8;

			for (int i = 0; i < 4; i++)
			{
				if (Get5b(rawData, ref bitPosition, ref sopBinary[i]))
					return;
			}
			sop = new PdSop(sopBinary);

			if (Convert5bTo4b(rawData, ref bitPosition, ref headerBinary))
				return;
			if (sop.Sop == PdSop.SopType.Sop)
			{
				header = new PdHeaderSop(headerBinary);
			}
			else if (sop.Sop == PdSop.SopType.SopPrime || sop.Sop == PdSop.SopType.SopDoublePrime)
			{
				header = new PdHeaderSopPrime(headerBinary);
			}
			else
			{
				return;
			}

			data = new List<PdDataObject>();
			for (int i = 0; i < header.NumberOfDataObjects; i++)
			{
				uint dataBinary = 0;

				if (Convert5bTo4b(rawData, ref bitPosition, ref dataBinary))
					return;

				if (header.MessageType == PdHeader.MessageTypes.Source_Capabilities || header.MessageType == PdHeader.MessageTypes.Sink_Capabilities)
				{
					PdPowerDataObject pdo = new PdPowerDataObject(dataBinary);
					if (pdo.SourceType == PdPowerDataObject.SourceTypes.FixedSupply)
					{
						if (header.MessageType == PdHeader.MessageTypes.Source_Capabilities)
							data.Add(new PdSourceFixedSupplyPdo(dataBinary));
						else
							data.Add(new PdSinkFixedSupplyPdo(dataBinary));
					}
					else if (pdo.SourceType == PdPowerDataObject.SourceTypes.VariableSupply)
					{
						if (header.MessageType == PdHeader.MessageTypes.Source_Capabilities)
							data.Add(new PdSourceVariableSupplyPdo(dataBinary));
						else
							data.Add(new PdSinkVariableSupplyPdo(dataBinary));
					} else if (pdo.SourceType == PdPowerDataObject.SourceTypes.Battery)
					{
						if (header.MessageType == PdHeader.MessageTypes.Source_Capabilities)
							data.Add(new PdSourceBatterySupplyPdo(dataBinary));
						else
							data.Add(new PdSinkBatterySupplyPdo(dataBinary));
					}
				}
				else if (header.MessageType == PdHeader.MessageTypes.Request)
				{
					data.Add(new PdFixedAndVariableRequestDataObject(dataBinary));
				}
				else
				{
					data.Add(new PdDataObject(dataBinary));
				}
			}

			if (Convert5bTo4b(rawData, ref bitPosition, ref crc))
				return;

			if (Get5b(rawData, ref bitPosition, ref eop))
				return;

		}

		private IEnumerable<byte> HexStringToBinary(string hex)
		{
			for (int i = 0; i < hex.Length - 1; i += 2)
			{
				yield return Convert.ToByte(hex.Substring(i, 2), 16);
			}
		}

		private bool Get5b(byte[] data4b, ref int position, ref byte symbol)
		{
			int len = data4b.Length;

			if (position % 8 > 3)
			{
				if ((position + 7) % 8 + 1 > len)
					return true;

				symbol = (byte)(((data4b[position / 8] << (position % 8 - 3)) | (data4b[position / 8 + 1] >> (11 - position % 8))) & 0x1f);
			}
			else
			{
				if ((position + 7) % 8 > len)
					return true;

				symbol = (byte)((data4b[position / 8] >> (3 - position % 8)) & 0x1f);
			}

			position += 5;

			return false;
		}

		private bool Convert5bTo4bNibble(byte[] data4b, ref int position, ref byte decoded)
		{
			byte nibble = 0;

			if (Get5b(data4b, ref position, ref nibble))
				return true;

			decoded = symbol5b4b[nibble];

			if (decoded > 0x0f)
				return true;

			return false;
		}

		private bool Convert5bTo4b(byte[] data4b, ref int position, ref byte decoded)
		{
			byte hi = 0, lo = 0;

			if (Convert5bTo4bNibble(data4b, ref position, ref lo))
				return true;
			if (Convert5bTo4bNibble(data4b, ref position, ref hi))
				return true;

			decoded = (byte)((hi << 4) | lo);

			return false;
		}

		private bool Convert5bTo4b(byte[] data, ref int position, ref ushort decoded)
		{
			decoded = 0;

			for (int i = 0; i < 2; i++)
			{
				byte d = 0;

				if (Convert5bTo4b(data, ref position, ref d))
					return true;

				decoded += (ushort)(d << (8 * i));
			}
			return false;
		}

		private bool Convert5bTo4b(byte[] data, ref int position, ref uint decoded)
		{
			for (int i = 0; i < 4; i++)
			{
				byte d = 0;

				if (Convert5bTo4b(data, ref position, ref d))
					return true;

				decoded += (uint)(d << (8 * i));
			}
			return false;
		}
	}
}
