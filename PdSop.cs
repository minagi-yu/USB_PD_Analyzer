using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USB_PD_Analyzer
{
	internal class PdSop
	{
		public enum SopType
		{
			Sop,
			SopPrime,
			SopDoublePrime,
			SopPrimeDebug,
			SopDoublePrimeDebug,
			HardReset,
			CableReset,
			Error
		}
		public enum K_code
		{
			Sync_1 = 0x03,
			Sync_2 = 0x11,
			Sync_3 = 0x0c,
			RST_1 = 0x1c,
			RST_2 = 0x13
		}

		public readonly K_code[] value = new K_code[4];

		public PdSop(byte[] d)
		{
			if (d.Count() != 4)
				throw new ArgumentException();

			for (int i = 0; i < 4; i++)
			{
				value[i] = (K_code)d[i];
			}
		}

		public SopType Sop
		{
			get
			{
				if (value.SequenceEqual(new K_code[] { K_code.Sync_1, K_code.Sync_1, K_code.Sync_1, K_code.Sync_2 }))
				{
					return SopType.Sop;
				}
				else if (value.SequenceEqual(new K_code[] { K_code.Sync_1, K_code.Sync_1, K_code.Sync_3, K_code.Sync_3 }))
				{
					return SopType.SopPrime;
				}
				else if (value.SequenceEqual(new K_code[] { K_code.Sync_1, K_code.Sync_3, K_code.Sync_1, K_code.Sync_3 }))
				{
					return SopType.SopDoublePrime;
				}
				else if (value.SequenceEqual(new K_code[] { K_code.Sync_1, K_code.RST_2, K_code.RST_2, K_code.Sync_3 }))
				{
					return SopType.SopPrimeDebug;
				}
				else if (value.SequenceEqual(new K_code[] { K_code.Sync_1, K_code.RST_2, K_code.Sync_3, K_code.Sync_2 }))
				{
					return SopType.SopDoublePrimeDebug;
				}
				else if (value.SequenceEqual(new K_code[] { K_code.RST_1, K_code.RST_1, K_code.RST_1, K_code.RST_2 }))
				{
					return SopType.CableReset;
				}
				else if (value.SequenceEqual(new K_code[] { K_code.Sync_1, K_code.Sync_1, K_code.RST_1, K_code.Sync_3 }))
				{
					return SopType.HardReset;
				}
				return SopType.Error;
			}
		}

		public string GetSopTypeString()
		{
			switch (this.Sop)
			{
				case SopType.Sop:
					return "SOP";
				case SopType.SopPrime:
					return "SOP'";
				case SopType.SopDoublePrime:
					return "SOP''";
				case SopType.SopPrimeDebug:
					return "SOP'_Debug";
				case SopType.SopDoublePrimeDebug:
					return "SOP''_Debug";
				case SopType.CableReset:
					return "Cable Reset";
				case SopType.HardReset:
					return "Hard Reset";
				default:
					return "Error";
			}
		}

		public string GetSopString()
		{
			string[] ss = new string[4];

			for (int i = 0; i < 4; i++)
			{
				switch (this.value[i])
				{
					case K_code.Sync_1:
						ss[i] = "Sync-1";
						break;
					case K_code.Sync_2:
						ss[i] = "Sync-2";
						break;
					case K_code.Sync_3:
						ss[i] = "Sync-3";
						break;
					case K_code.RST_1:
						ss[i] = "RST-1";
						break;
					case K_code.RST_2:
						ss[i] = "RST-2";
						break;
					default:
						ss[i] = "Error";
						break;

				}
			}

			return string.Join(" ", ss);
		}
	}
}
