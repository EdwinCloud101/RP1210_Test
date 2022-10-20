
// Type: RP1210_Test.HelpClasses.RP1210_FilterIso15765




using Peak.RP1210C;
using System;

namespace RP1210_Test.HelpClasses
{
  public struct RP1210_FilterIso15765
  {
    public static bool operator ==(RP1210_FilterIso15765 filterA, RP1210_FilterIso15765 filterB)
    {
      if (filterA.FilterType == filterB.FilterType)
      {
        switch (filterA.FilterType)
        {
          case RP1210CISO15765MsgType.STANDARD_CAN:
          case RP1210CISO15765MsgType.EXTENDED_CAN:
            return (int) filterA.Mask == (int) filterB.Mask && (int) filterA.Header == (int) filterB.Header;
          case RP1210CISO15765MsgType.STANDARD_CAN_ISO15765_EXTENDED:
          case RP1210CISO15765MsgType.EXTENDED_CAN_ISO15765_EXTENDED:
          case RP1210CISO15765MsgType.STANDARD_MIXED_CAN_ISO15765:
            return (int) filterA.Mask == (int) filterB.Mask && (int) filterA.Header == (int) filterB.Header && (int) filterA.ExtendedAddressMask == (int) filterB.ExtendedAddressMask && (int) filterA.ExtendedAddressHeader == (int) filterB.ExtendedAddressHeader;
        }
      }
      return false;
    }

    public static bool operator !=(RP1210_FilterIso15765 filterA, RP1210_FilterIso15765 filterB) => !(filterA == filterB);

    public override bool Equals(object obj) => obj != null && obj is RP1210_FilterIso15765 rp1210FilterIso15765 && this == rp1210FilterIso15765;

    public bool Equals(RP1210_FilterIso15765 obj) => this == obj;

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString() => string.Format("type: {0} | Mask 0x{1} | Header 0x{2} | Extended Address Mask 0x{3} | Extended Address Header 0x{4}", (object) this.FilterType, (object) this.Mask.ToString("X8"), (object) this.Header.ToString("X8"), (object) this.ExtendedAddressMask.ToString("X"), (object) this.ExtendedAddressHeader.ToString("X"));

    private void SetBytes(byte[] value)
    {
      if (value == null || value.Length < 11)
        return;
      this.FilterType = (RP1210CISO15765MsgType) value[0];
      Array.Reverse((Array) value, 1, 4);
      this.Mask = BitConverter.ToUInt32(value, 1);
      this.ExtendedAddressMask = value[5];
      Array.Reverse((Array) value, 6, 4);
      this.Header = BitConverter.ToUInt32(value, 6);
      this.ExtendedAddressHeader = value[10];
    }

    private byte[] GetBytes()
    {
      byte[] numArray = new byte[11];
      numArray[0] = (byte) this.FilterType;
      byte[] bytes1 = BitConverter.GetBytes(this.Mask);
      Array.Reverse((Array) bytes1);
      Array.Copy((Array) bytes1, 0, (Array) numArray, 1, 4);
      numArray[5] = this.ExtendedAddressMask;
      byte[] bytes2 = BitConverter.GetBytes(this.Header);
      Array.Reverse((Array) bytes2);
      Array.Copy((Array) bytes2, 0, (Array) numArray, 6, 4);
      numArray[10] = this.ExtendedAddressHeader;
      return numArray;
    }

    public RP1210CISO15765MsgType FilterType { get; set; }

    public uint Mask { get; set; }

    public byte ExtendedAddressMask { get; set; }

    public uint Header { get; set; }

    public byte ExtendedAddressHeader { get; set; }

    public byte[] ByteArray
    {
      get => this.GetBytes();
      set => this.SetBytes(value);
    }
  }
}
