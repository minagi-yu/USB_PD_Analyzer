﻿using System;
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
	}
}
