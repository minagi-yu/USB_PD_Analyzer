using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USB_PD_Analyzer
{
	internal class PdDataObject
	{
		public readonly uint dataObject;

		public PdDataObject(uint d)
		{
			dataObject = d;
		}
	}

	internal class PdPowerDataObject : PdDataObject
	{
		public enum SourceTypes
		{
			FixedSupply,
			Battery,
			VariableSupply
		}
		public PdPowerDataObject(uint d) : base(d) { }

		public SourceTypes SupplyType => (SourceTypes)((dataObject >> 30) & 0x03);
	}

	internal class PdSourceFixedSupplyPdo : PdPowerDataObject
	{
		public PdSourceFixedSupplyPdo(uint d) : base(d) { }

		public bool DualRolePower => Convert.ToBoolean((dataObject >> 29) & 0x01);
		public bool UsbSuspendSupported => Convert.ToBoolean((dataObject >> 28) & 0x01);
		public bool UnconstrainedPower => Convert.ToBoolean((dataObject >> 27) & 0x01);
		public bool UsbCommunicationCapable => Convert.ToBoolean((dataObject >> 26) & 0x01);
		public bool DualRoleData => Convert.ToBoolean((dataObject >> 25) & 0x01);
		public int PeakCurrent => (int)((dataObject >> 20) & 0x03);
		public int Voltage => (int)((dataObject >> 10) & 0x3ff) * 50 / 1000;
		public int MaximumCurrent => (int)(dataObject & 0x3ff) * 10 / 1000;
	}

	internal class PdSourceVariableSupplyPdo : PdPowerDataObject
	{
		public PdSourceVariableSupplyPdo(uint d) : base(d) { }

		public int MaximumVoltage => (int)((dataObject >> 20) & 0x3ff) * 50 / 1000;
		public int MinimumVoltage => (int)((dataObject >> 10) & 0x3ff) * 50 / 1000;
		public int MaximumCurrent => (int)(dataObject & 0x3ff) * 10 / 1000;
	}

	internal class PdSourceBatterySupplyPdo : PdPowerDataObject
	{
		public PdSourceBatterySupplyPdo(uint d) : base(d) { }

		public int MaximumVoltage => (int)((dataObject >> 20) & 0x3ff) * 50 / 1000;
		public int MinimumVoltage => (int)((dataObject >> 10) & 0x3ff) * 50 / 1000;
		public int MaximumAllowablePower => (int)(dataObject & 0x3ff) * 250 / 1000;
	}

	internal class PdSinkFixedSupplyPdo : PdPowerDataObject
	{
		public PdSinkFixedSupplyPdo(uint d) : base(d) { }

		public bool DualRolePower => Convert.ToBoolean((dataObject >> 29) & 0x01);
		public bool HigherCapability => Convert.ToBoolean((dataObject >> 28) & 0x01);
		public bool UnconstrainedPower => Convert.ToBoolean((dataObject >> 27) & 0x01);
		public bool UsbCommunicationCapable => Convert.ToBoolean((dataObject >> 26) & 0x01);
		public bool DualRoleData => Convert.ToBoolean((dataObject >> 25) & 0x01);
		public int Voltage => (int)((dataObject >> 10) & 0x3ff) * 50 / 1000;
		public int OperationalCurrent => (int)(dataObject & 0x3ff) * 10 / 1000;
	}

	internal class PdSinkVariableSupplyPdo : PdPowerDataObject
	{
		public PdSinkVariableSupplyPdo(uint d) : base(d) { }

		public int MaximumVoltage => (int)((dataObject >> 20) & 0x3ff) * 50 / 1000;
		public int MinimumVoltage => (int)((dataObject >> 10) & 0x3ff) * 50 / 1000;
		public int OperationalCurrent => (int)(dataObject & 0x3ff) * 10 / 1000;
	}

	internal class PdSinkBatterySupplyPdo : PdPowerDataObject
	{
		public PdSinkBatterySupplyPdo(uint d) : base(d) { }

		public int MaximumVoltage => (int)((dataObject >> 20) & 0x3ff) * 50 / 1000;
		public int MinimumVoltage => (int)((dataObject >> 10) & 0x3ff) * 50 / 1000;
		public int OperationalPower => (int)(dataObject & 0x3ff) * 250 / 1000;
	}

	internal class PdRequestDataObject : PdDataObject
	{
		public PdRequestDataObject(uint d) : base(d) { }

		public int ObjectPosition => (int)((dataObject >> 28) & 0x7);
		public bool GiveBackFlag => Convert.ToBoolean((dataObject >> 27) & 0x01);
		public bool CapabilityMismatch => Convert.ToBoolean((dataObject >> 26) & 0x01);
		public bool UsbCommunicationsCapable => Convert.ToBoolean((dataObject >> 25) & 0x01);
		public bool NoUsbSuspended => Convert.ToBoolean((dataObject >> 24) & 0x01);
	}

	internal class PdFixedAndVariableRequestDataObject : PdRequestDataObject
	{
		public PdFixedAndVariableRequestDataObject(uint d) : base(d) { }

		public int OperatingCurrent => (int)((dataObject >> 10) & 0x3ff) * 10 / 1000;
		public int MaximumOperatingCurrent => (int)(dataObject & 0x3ff) * 10 / 1000;
	}

	internal class PdBatteryRequestdataObject : PdRequestDataObject
	{
		public PdBatteryRequestdataObject(uint d) : base(d) { }

		public int OperatingPower => (int)((dataObject >> 10) & 0x3ff) * 250 / 1000;
		public int MaximumOperatingPower => (int)(dataObject & 0x3ff) * 250 / 1000;
	}
}
