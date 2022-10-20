
// Type: RP1210_Test.HelpClasses.RP1210_MsgCan




using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RP1210_Test.HelpClasses
{
  public class RP1210_MsgCan : MessageDataHandler, RP1210_SerialisableMsg, IEquatable<RP1210_MsgCan>
  {
    public const uint MaxStd_ID = 2047;
    public const uint MaxExt_ID = 536870911;

    public RP1210_MsgCan()
      : this(2047U, false, new byte[0])
    {
    }

    public RP1210_MsgCan(uint Id)
      : this(Id, false, new byte[0])
    {
    }

    public RP1210_MsgCan(uint Id, bool isExtended)
      : this(Id, isExtended, new byte[0])
    {
    }

    public RP1210_MsgCan(byte[] rawData)
      : base(8)
    {
      this.Parse(rawData);
    }

    public RP1210_MsgCan(uint Id, bool isExtended, byte[] data)
      : base(8)
    {
      this.Extended = isExtended;
      this.ID = Id;
      this.Data = data;
    }

    public static bool operator ==(RP1210_MsgCan msg1, RP1210_MsgCan msg2) => (object) msg2 != null && (int) msg1.ID == (int) msg2.ID && msg1.Extended == msg2.Extended;

    public static bool operator !=(RP1210_MsgCan msg1, RP1210_MsgCan msg2) => !(msg1 == msg2);

    public override bool Equals(object obj) => obj != null && (object) (obj as RP1210_MsgCan) != null && this.Equals(obj as RP1210_MsgCan);

    public override int GetHashCode() => base.GetHashCode();

    public bool Equals(RP1210_MsgCan obj) => obj == this;

    public override string ToString()
    {
      string str = string.Format("ID: 0x{0:" + (this.Extended ? "X8}" : "X4} |") + " Extended: {1} | LEN: {2} | Data: ", (object) this.ID, (object) this.Extended, (object) this.Length);
      for (int index = 0; index < this.Length; ++index)
        str = str + this.m_Data[index].ToString("X2") + " ";
      return str;
    }

    public byte[] ToByteArray()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add(this.Extended ? (byte) 1 : (byte) 0);
      byte[] numArray = this.Extended ? BitConverter.GetBytes(this.ID) : BitConverter.GetBytes((ushort) this.ID);
      Array.Reverse((Array) numArray);
      byteList.AddRange((IEnumerable<byte>) numArray);
      for (int index = 0; index < this.Length; ++index)
        byteList.Add(this.m_Data[index]);
      return byteList.ToArray();
    }

    public bool Parse(byte[] rawData)
    {
      try
      {
        this.Extended = rawData[0] > (byte) 0;
        byte[] numArray = new byte[this.Extended ? 4 : 2];
        Array.Copy((Array) rawData, 1, (Array) numArray, 0, numArray.Length);
        Array.Reverse((Array) numArray);
        this.ID = this.Extended ? BitConverter.ToUInt32(numArray, 0) : (uint) BitConverter.ToUInt16(numArray, 0);
        this.Length = rawData.Length - (this.Extended ? 5 : 3);
        Array.Copy((Array) rawData, this.Extended ? 5 : 3, (Array) this.m_Data, 0, this.Length);
        return true;
      }
      catch
      {
        return false;
      }
    }

    protected short GetWriteBufferLength() => (short) (1 + (this.Extended ? 4 : 2) + this.Length);

    public int CreateListViewEntry(ListView parent)
    {
      string text = "";
      ListViewItem listViewItem;
      if (this.Extended)
      {
        listViewItem = parent.Items.Add("EXTENDED");
        listViewItem.SubItems.Add(string.Format("{0:X8}h", (object) this.ID));
      }
      else
      {
        listViewItem = parent.Items.Add("STANDARD");
        listViewItem.SubItems.Add(string.Format("{0:X4}h", (object) this.ID));
      }
      listViewItem.SubItems.Add(this.Length.ToString());
      for (int index = 0; index < this.Length; ++index)
        text = text + this.Data[index].ToString("X2") + " ";
      listViewItem.SubItems.Add(text);
      listViewItem.SubItems.Add("0");
      listViewItem.Tag = (object) this;
      listViewItem.Selected = true;
      return listViewItem.Index;
    }

    public uint ID { get; set; }

    public bool Extended { get; set; }

    public short WriteBufferLength => this.GetWriteBufferLength();
  }
}
