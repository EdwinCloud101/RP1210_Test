
// Type: RP1210_Test.HelpClasses.RP1210_FilterJ1939




using Peak.RP1210C;
using System;

namespace RP1210_Test.HelpClasses
{
  public struct RP1210_FilterJ1939
  {
    private uint m_PGN;
    private byte m_Priority;
    private byte m_SA;
    private byte m_DA;

    public static bool operator ==(RP1210_FilterJ1939 filterA, RP1210_FilterJ1939 filterB) => filterA.Flags == filterB.Flags && ((filterA.Flags & RP1210CJ1939Filter.FILTER_PGN) != RP1210CJ1939Filter.FILTER_PGN || (int) filterA.PGN == (int) filterB.PGN) && (((filterA.Flags & RP1210CJ1939Filter.FILTER_PRIORITY) != RP1210CJ1939Filter.FILTER_PRIORITY || (int) filterA.Priority == (int) filterB.Priority) && ((filterA.Flags & RP1210CJ1939Filter.FILTER_SOURCE) != RP1210CJ1939Filter.FILTER_SOURCE || (int) filterA.SourceAddress == (int) filterB.SourceAddress)) && ((filterA.Flags & RP1210CJ1939Filter.FILTER_DESTINATION) != RP1210CJ1939Filter.FILTER_DESTINATION || (int) filterA.DestinationAddress == (int) filterB.DestinationAddress);

    public static bool operator !=(RP1210_FilterJ1939 filterA, RP1210_FilterJ1939 filterB) => !(filterA == filterB);

    public override bool Equals(object obj) => obj != null && obj is RP1210_FilterJ1939 rp1210FilterJ1939 && this == rp1210FilterJ1939;

    public bool Equals(RP1210_FilterJ1939 obj) => obj == this;

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString()
    {
      string str = "";
      if ((this.Flags & RP1210CJ1939Filter.FILTER_PGN) == RP1210CJ1939Filter.FILTER_PGN)
        str = "PGN: " + this.PGN.ToString();
      if ((this.Flags & RP1210CJ1939Filter.FILTER_SOURCE) == RP1210CJ1939Filter.FILTER_SOURCE)
      {
        if (str != "")
          str += " | ";
        str = str + "SA: " + this.SourceAddress.ToString();
      }
      if ((this.Flags & RP1210CJ1939Filter.FILTER_DESTINATION) == RP1210CJ1939Filter.FILTER_DESTINATION)
      {
        if (str != "")
          str += " | ";
        str = str + "DA: " + this.DestinationAddress.ToString();
      }
      if ((this.Flags & RP1210CJ1939Filter.FILTER_PRIORITY) == RP1210CJ1939Filter.FILTER_PRIORITY)
      {
        if (str != "")
          str += " | ";
        str = str + "Priority: " + this.Priority.ToString();
      }
      return str;
    }

    private uint GetPGN() => this.m_PGN;

    private byte GetPriority() => this.m_Priority;

    private byte GetSA() => this.m_SA;

    private byte GetDA() => this.m_DA;

    private void SetPGN(uint value) => this.m_PGN = value & 16777215U;

    private void SetPriority(byte value) => this.m_Priority = (byte) ((uint) value & 7U);

    private void SetSA(byte value)
    {
      if (value == byte.MaxValue)
        return;
      this.m_SA = value;
    }

    private void SetDA(byte value)
    {
      if (value == (byte) 254)
        return;
      this.m_DA = value;
    }

    private byte[] GetBytes()
    {
      byte[] numArray = new byte[7];
      numArray[0] = (byte) this.Flags;
      Array.Copy((Array) BitConverter.GetBytes(this.m_PGN), 1, (Array) numArray, 1, 3);
      numArray[4] = this.m_Priority;
      numArray[5] = this.m_SA;
      numArray[6] = this.m_DA;
      return numArray;
    }

    private void SetBytes(byte[] value)
    {
      byte[] numArray = new byte[4];
      if (value == null || value.Length < 7)
        return;
      this.Flags = (RP1210CJ1939Filter) value[0];
      Array.Copy((Array) value, 1, (Array) numArray, 1, 3);
      this.m_PGN = BitConverter.ToUInt32(numArray, 0);
      this.m_Priority = value[4];
      this.m_SA = value[5];
      this.m_DA = value[6];
    }

    public RP1210CJ1939Filter Flags { get; set; }

    public uint PGN
    {
      get => this.GetPGN();
      set => this.SetPGN(value);
    }

    public byte Priority
    {
      get => this.GetPriority();
      set => this.SetPriority(value);
    }

    public byte SourceAddress
    {
      get => this.GetSA();
      set => this.SetSA(value);
    }

    public byte DestinationAddress
    {
      get => this.GetDA();
      set => this.SetDA(value);
    }

    public byte[] ByteArray
    {
      get => this.GetBytes();
      set => this.SetBytes(value);
    }
  }
}
