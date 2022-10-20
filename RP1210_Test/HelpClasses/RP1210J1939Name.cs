
// Type: RP1210_Test.HelpClasses.RP1210J1939Name




using System;

namespace RP1210_Test.HelpClasses
{
  public struct RP1210J1939Name
  {
    private byte[] m_Name;

    public RP1210J1939Name(ulong name) => this.m_Name = BitConverter.GetBytes(name);

    public RP1210J1939Name(byte[] name)
    {
      this.m_Name = new byte[8];
      this.SetAsArray(name);
    }

    public RP1210J1939Name(RP1210J1939Name obj)
    {
      this.m_Name = new byte[8];
      Array.Copy((Array) obj.m_Name, (Array) this.m_Name, 8);
    }

    private void SetAsArray(byte[] value) => this.m_Name = value != null && value.Length >= 8 ? value : throw new InvalidOperationException("J1939 Name array is null or incomplete");

    private bool GetIfAAC() => ((int) this.m_Name[7] & 128) > 0;

    private void SetAAC(bool value)
    {
      this.m_Name[7] &= (byte) 127;
      if (!value)
        return;
      this.m_Name[7] |= (byte) 128;
    }

    private byte GetIG() => (byte) ((int) this.m_Name[7] >> 4 & 7);

    private void SetIG(byte value)
    {
      this.m_Name[7] &= (byte) 143;
      this.m_Name[7] |= (byte) (((int) value & 7) << 4);
    }

    private byte GetVSI() => (byte) ((uint) this.m_Name[7] & 15U);

    private void SetVSI(byte value)
    {
      this.m_Name[7] &= (byte) 240;
      this.m_Name[7] |= (byte) ((uint) value & 15U);
    }

    private byte GetVS() => (byte) (((int) this.m_Name[6] & 254) >> 1);

    private void SetVS(byte value)
    {
      this.m_Name[6] &= (byte) 1;
      this.m_Name[6] |= (byte) (((int) value & (int) sbyte.MaxValue) << 1);
    }

    private byte GetFunction() => this.m_Name[5];

    private void SetFunction(byte value) => this.m_Name[5] = value;

    private byte GetFI() => (byte) (((int) this.m_Name[4] & 248) >> 3);

    private void SetFI(byte value)
    {
      this.m_Name[4] &= (byte) 7;
      this.m_Name[4] |= (byte) (((int) value & 31) << 3);
    }

    private byte GetEcu() => (byte) ((uint) this.m_Name[4] & 7U);

    private void SetEcu(byte value)
    {
      this.m_Name[4] &= (byte) 248;
      this.m_Name[4] |= (byte) ((uint) value & 7U);
    }

    private ushort GetMC() => (ushort) ((int) this.m_Name[3] << 3 | ((int) this.m_Name[2] & 224) >> 5);

    private void SetMC(ushort value)
    {
      value &= (ushort) 2047;
      this.m_Name[3] = (byte) (((int) value & 2040) >> 3);
      this.m_Name[2] &= (byte) 31;
      this.m_Name[2] |= (byte) (((int) value & 7) << 5);
    }

    private uint GetIN() => (uint) (((int) this.m_Name[2] & 31) << 16) | (uint) (ushort) ((uint) this.m_Name[1] << 8 | (uint) this.m_Name[0]);

    private void SetIN(uint value)
    {
      value &= 2097151U;
      this.m_Name[2] &= (byte) 224;
      this.m_Name[2] |= (byte) ((value & 16711680U) >> 16);
      this.m_Name[1] = (byte) ((value & 65280U) >> 8);
      this.m_Name[0] = (byte) (value & (uint) byte.MaxValue);
    }

    public ulong AsNumber
    {
      get
      {
        if (this.m_Name == null)
          this.m_Name = new byte[8];
        return BitConverter.ToUInt64(this.m_Name, 0);
      }
      set => this.m_Name = BitConverter.GetBytes(value);
    }

    public byte[] AsByteArray
    {
      get => this.m_Name;
      set => this.SetAsArray(value);
    }

    public bool ArbitraryAddressCapable
    {
      get => this.GetIfAAC();
      set => this.SetAAC(value);
    }

    public byte IndustryGroup
    {
      get => this.GetIG();
      set => this.SetIG(value);
    }

    public byte VehicleSystemInstance
    {
      get => this.GetVSI();
      set => this.SetVSI(value);
    }

    public byte VehicleSystem
    {
      get => this.GetVS();
      set => this.SetVS(value);
    }

    public byte Function
    {
      get => this.GetFunction();
      set => this.SetFunction(value);
    }

    public byte FunctionInstance
    {
      get => this.GetFI();
      set => this.SetFI(value);
    }

    public byte ECUInstance
    {
      get => this.GetEcu();
      set => this.SetEcu(value);
    }

    public ushort ManufactureCode
    {
      get => this.GetMC();
      set => this.SetMC(value);
    }

    public uint IdentifyNumber
    {
      get => this.GetIN();
      set => this.SetIN(value);
    }
  }
}
