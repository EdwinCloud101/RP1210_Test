
// Type: RP1210_Test.HelpClasses.RP1210_FilterCan




using System;

namespace RP1210_Test.HelpClasses
{
  public struct RP1210_FilterCan
  {
    public static bool operator ==(RP1210_FilterCan filterA, RP1210_FilterCan filterB) => (int) filterA.Mask == (int) filterB.Mask && (int) filterA.Header == (int) filterB.Header && filterA.Extended == filterB.Extended;

    public static bool operator !=(RP1210_FilterCan filterA, RP1210_FilterCan filterB) => !(filterA == filterB);

    public override bool Equals(object obj) => obj != null && obj is RP1210_FilterCan rp1210FilterCan && this == rp1210FilterCan;

    public bool Equals(RP1210_FilterCan obj) => obj == this;

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString() => string.Format(this.Extended ? "Type: EXT | Mask 0x{0:X8} | Header 0x{1:X8}" : "Type: STD | Mask 0x{0:X4} | Header 0x{1:X4}", (object) this.Mask, (object) this.Header);

    private void SetBytes(byte[] value)
    {
      if (value == null || value.Length < 9)
        return;
      this.Extended = value[0] > (byte) 0;
      Array.Reverse((Array) value, 1, 4);
      this.Mask = BitConverter.ToUInt32(value, 1);
      Array.Reverse((Array) value, 5, 4);
      this.Header = BitConverter.ToUInt32(value, 5);
    }

    private byte[] GetBytes()
    {
      byte[] numArray = new byte[9];
      numArray[0] = this.Extended ? (byte) 1 : (byte) 0;
      byte[] bytes1 = BitConverter.GetBytes(this.Mask);
      Array.Reverse((Array) bytes1);
      Array.Copy((Array) bytes1, 0, (Array) numArray, 1, 4);
      byte[] bytes2 = BitConverter.GetBytes(this.Header);
      Array.Reverse((Array) bytes2);
      Array.Copy((Array) bytes2, 0, (Array) numArray, 5, 4);
      return numArray;
    }

    public bool Extended { get; set; }

    public uint Mask { get; set; }

    public uint Header { get; set; }

    public byte[] ByteArray
    {
      get => this.GetBytes();
      set => this.SetBytes(value);
    }
  }
}
