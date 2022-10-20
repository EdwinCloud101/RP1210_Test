
// Type: RP1210_Test.HelpClasses.RP1210_MsgJ1939




using Peak.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RP1210_Test.HelpClasses
{
  public class RP1210_MsgJ1939 : 
    MessageDataHandler,
    RP1210_SerialisableMsg,
    IEquatable<RP1210_MsgJ1939>
  {
    public const int MaximumDataBytesCount = 1785;
    public const byte Tester_ID = 249;
    public const byte Global_ID = 255;
    public const byte Null_ID = 254;

    public RP1210_MsgJ1939()
      : this(0U, RP1210_MsgJ1939.TransportMode.BAM, (byte) 7, (byte) 249, byte.MaxValue, new byte[0])
    {
    }

    public RP1210_MsgJ1939(uint Pgn)
      : this(Pgn, RP1210_MsgJ1939.TransportMode.BAM, (byte) 7, (byte) 249, byte.MaxValue, new byte[0])
    {
    }

    public RP1210_MsgJ1939(uint Pgn, RP1210_MsgJ1939.TransportMode Mode)
      : this(Pgn, Mode, (byte) 7, (byte) 249, byte.MaxValue, new byte[0])
    {
    }

    public RP1210_MsgJ1939(uint Pgn, RP1210_MsgJ1939.TransportMode Mode, byte Priority)
      : this(Pgn, Mode, Priority, (byte) 249, byte.MaxValue, new byte[0])
    {
    }

    public RP1210_MsgJ1939(uint Pgn, RP1210_MsgJ1939.TransportMode Mode, byte Priority, byte Sa)
      : this(Pgn, Mode, Priority, Sa, byte.MaxValue, new byte[0])
    {
    }

    public RP1210_MsgJ1939(
      uint Pgn,
      RP1210_MsgJ1939.TransportMode Mode,
      byte Priority,
      byte Sa,
      byte Da)
      : this(Pgn, Mode, Priority, Sa, Da, new byte[0])
    {
    }

    public RP1210_MsgJ1939(byte[] rawData)
      : base(1785)
    {
      this.Parse(rawData);
    }

    public RP1210_MsgJ1939(
      uint Pgn,
      RP1210_MsgJ1939.TransportMode Mode,
      byte Priority,
      byte Sa,
      byte Da,
      byte[] data)
      : base(1785)
    {
      this.PGN = Pgn;
      this.SendMode = Mode;
      this.Priority = Priority;
      this.SourceAddress = Sa;
      this.DestinationAddress = Da;
      this.Data = data;
    }

    public static bool operator ==(RP1210_MsgJ1939 msg1, RP1210_MsgJ1939 msg2) => (object) msg2 != null && (int) msg1.PGN == (int) msg2.PGN && ((int) msg1.SourceAddress == (int) msg2.SourceAddress && (int) msg1.DestinationAddress == (int) msg2.DestinationAddress) && (int) msg1.Priority == (int) msg2.Priority;

    public static bool operator !=(RP1210_MsgJ1939 msg1, RP1210_MsgJ1939 msg2) => !(msg1 == msg2);

    public override bool Equals(object obj) => obj != null && (object) (obj as RP1210_MsgJ1939) != null && this.Equals(obj as RP1210_MsgJ1939);

    public override int GetHashCode() => base.GetHashCode();

    public bool Equals(RP1210_MsgJ1939 obj) => obj == this;

    public override string ToString()
    {
      string str = string.Format("PGN: {0} (0x{0:X4}) |" + " Send as: {1} |" + " Prior: {2} |" + " SA: {3} (0x{3:X2}) |" + " DA: {4} (0x{4:X2}) | LEN: {2} | Data: ", (object) this.PGN, (object) this.SendMode.ToString(), (object) this.Priority, (object) this.SourceAddress, (object) this.DestinationAddress, (object) this.Length);
      for (int index = 0; index < this.Length; ++index)
        str = str + this.m_Data[index].ToString("X2") + " ";
      return str;
    }

    public byte[] ToByteArray()
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) SubArray.GetBytes(BitConverter.GetBytes(this.PGN), 0, 3));
      byteList.Add(this.SendMode == RP1210_MsgJ1939.TransportMode.BAM || this.DestinationAddress == byte.MaxValue ? (byte) (128 | (int) this.Priority & 7) : (byte) ((int) this.Priority & 7));
      byteList.Add(this.SourceAddress);
      byteList.Add(this.DestinationAddress);
      for (int index = 0; index < this.Length; ++index)
        byteList.Add(this.m_Data[index]);
      return byteList.ToArray();
    }

    public bool Parse(byte[] rawData)
    {
      try
      {
        int num1 = 0;
        this.PGN = (uint) rawData[0] << 16 | (uint) BitConverter.ToUInt16(SubArray.GetBytes(rawData, 1, 2), 0);
        int index;
        this.SendMode = ((int) rawData[index = num1 + 3] & 128) != 0 ? RP1210_MsgJ1939.TransportMode.BAM : RP1210_MsgJ1939.TransportMode.RTS_CTS;
        this.Priority = (byte) ((uint) rawData[index] & 7U);
        int num2;
        this.SourceAddress = rawData[num2 = index + 1];
        int num3;
        this.DestinationAddress = rawData[num3 = num2 + 1];
        int num4;
        this.Data = SubArray.GetBytes(rawData, num4 = num3 + 1, rawData.Length - num4);
        return true;
      }
      catch
      {
        return false;
      }
    }

    protected short GetWriteBUfferLength() => (short) (6 + this.Length);

    public int CreateListViewEntry(ListView parent)
    {
      string text = "";
      ListViewItem listViewItem = parent.Items.Add(this.PGN.ToString());
      listViewItem.SubItems.Add(this.Priority.ToString());
      listViewItem.SubItems.Add(this.SourceAddress.ToString());
      listViewItem.SubItems.Add(this.DestinationAddress.ToString());
      listViewItem.SubItems.Add(this.Length.ToString());
      listViewItem.SubItems.Add("0");
      for (int index = 0; index < this.Length; ++index)
        text = text + this.Data[index].ToString("X2") + " ";
      listViewItem.SubItems.Add(text);
      listViewItem.Tag = (object) this;
      listViewItem.Selected = true;
      return listViewItem.Index;
    }

    public uint PGN { get; set; }

    public RP1210_MsgJ1939.TransportMode SendMode { get; set; }

    public byte Priority { get; set; }

    public byte SourceAddress { get; set; }

    public byte DestinationAddress { get; set; }

    public short WriteBufferLength => this.GetWriteBUfferLength();

    public enum TransportMode
    {
      BAM,
      RTS_CTS,
    }
  }
}
