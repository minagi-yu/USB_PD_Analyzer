using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USB_PD_Analyzer
{
	internal class pd
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

		private List<byte> rawData;

		public pd()
		{
		}

		public void Analyze(string hex)
		{
			rawData = new List<byte>();

			for (int i = 0; i < hex.Length - 1; i+=2)
			{
				rawData.Add(Convert.ToByte(hex.Substring(i, 2), 16));
			}
		}
	}
}
