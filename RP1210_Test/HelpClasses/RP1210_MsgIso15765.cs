
// Type: RP1210_Test.HelpClasses.RP1210_MsgIso15765




using Peak.RP1210C;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RP1210_Test.HelpClasses
{
  public class RP1210_MsgIso15765 : 
    MessageDataHandler,
    RP1210_SerialisableMsg,
    IEquatable<RP1210_MsgIso15765>
  {
    public const int MaximumDataBytesCount = 4096;

    public RP1210_MsgIso15765()
      : this(RP1210CISO15765MsgType.STANDARD_CAN, 2047U, (byte) 0, new byte[0])
    {
    }

    public RP1210_MsgIso15765(RP1210CISO15765MsgType msgType)
      : this(msgType, 2047U, (byte) 0, new byte[0])
    {
    }

    public RP1210_MsgIso15765(RP1210CISO15765MsgType msgType, uint id)
      : this(msgType, id, (byte) 0, new byte[0])
    {
    }

    public RP1210_MsgIso15765(RP1210CISO15765MsgType msgType, uint id, byte extendedAddress)
      : this(msgType, id, extendedAddress, new byte[0])
    {
    }

    public RP1210_MsgIso15765(byte[] rawData)
      : base(4096)
    {
      this.Parse(rawData);
    }

    public RP1210_MsgIso15765(
      RP1210CISO15765MsgType msgType,
      uint id,
      byte extendedAddress,
      byte[] data)
      : base(4096)
    {
      this.MessageType = msgType;
      this.ID = id;
      this.ExtendedAddress = extendedAddress;
      this.Data = data;
    }

    public static bool operator ==(RP1210_MsgIso15765 msg1, RP1210_MsgIso15765 msg2) => (object) msg2 != null && msg1.MessageType == msg2.MessageType && ((int) msg1.ID == (int) msg2.ID && (int) msg1.ExtendedAddress == (int) msg2.ExtendedAddress);

    public static bool operator !=(RP1210_MsgIso15765 msg1, RP1210_MsgIso15765 msg2) => !(msg1 == msg2);

    public override bool Equals(object obj) => obj != null && (object) (obj as RP1210_MsgIso15765) != null && this.Equals(obj as RP1210_MsgIso15765);

    public override int GetHashCode() => base.GetHashCode();

    public bool Equals(RP1210_MsgIso15765 obj) => obj == this;

    public override string ToString()
    {
      string str = string.Format("Type: {0} | ID: 0x{1:X8} Extended Address: {2} | Length: {3} | Data: ", (object) this.MessageType, (object) this.ID, (object) this.ExtendedAddress, (object) this.Length);
      for (int index = 0; index < this.Length; ++index)
        str = str + this.m_Data[index].ToString("X2") + " ";
      return str;
    }

    public byte[] ToByteArray()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) this.MessageType);
      byte[] bytes = BitConverter.GetBytes(this.ID);
      Array.Reverse((Array) bytes);
      byteList.AddRange((IEnumerable<byte>) bytes);
      byteList.Add(this.ExtendedAddress);
      for (int index = 0; index < this.Length; ++index)
        byteList.Add(this.m_Data[index]);
      return byteList.ToArray();
    }

    public bool Parse(byte[] rawData)
    {
      try
      {
        this.MessageType = (RP1210CISO15765MsgType) rawData[0];
        byte[] numArray = new byte[4];
        Array.Copy((Array) rawData, 1, (Array) numArray, 0, numArray.Length);
        Array.Reverse((Array) numArray);
        this.ID = BitConverter.ToUInt32(numArray, 0);
        this.ExtendedAddress = rawData[5];
        this.Length = rawData.Length - 6;
        Array.Copy((Array) rawData, 6, (Array) this.m_Data, 0, this.Length);
        return true;
      }
      catch
      {
        return false;
      }
    }

    protected short GetWriteBufferLength() => (short) (6 + this.Length);

    public int CreateListViewEntry(ListView parent)
    {
      string text = "";
      bool flag = this.MessageType == RP1210CISO15765MsgType.EXTENDED_CAN || this.MessageType == RP1210CISO15765MsgType.EXTENDED_CAN_ISO15765_EXTENDED;
      ListViewItem listViewItem = parent.Items.Add(this.MessageType.ToString());
      listViewItem.SubItems.Add(string.Format(flag ? "{0:X8}h" : "{0:X4}h", (object) this.ID));
      listViewItem.SubItems.Add(string.Format("{0}", (object) this.ExtendedAddress));
      listViewItem.SubItems.Add(this.Length.ToString());
      for (int index = 0; index < this.Length; ++index)
        text = text + this.Data[index].ToString("X2") + " ";
      listViewItem.SubItems.Add("0");
      listViewItem.SubItems.Add(text);
      listViewItem.Tag = (object) this;
      listViewItem.Selected = true;
      return listViewItem.Index;
    }

    public RP1210CISO15765MsgType MessageType { get; set; }

    public uint ID { get; set; }

    public byte ExtendedAddress { get; set; }

    public short WriteBufferLength => this.GetWriteBufferLength();
  }
}
