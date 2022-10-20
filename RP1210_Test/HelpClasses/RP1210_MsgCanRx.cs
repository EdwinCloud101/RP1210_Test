
// Type: RP1210_Test.HelpClasses.RP1210_MsgCanRx




using Peak.Classes;
using System;
using System.Windows.Forms;

namespace RP1210_Test.HelpClasses
{
  public class RP1210_MsgCanRx
  {
    public RP1210_MsgCanRx(byte[] rawData)
      : this(rawData, false)
    {
    }

    public RP1210_MsgCanRx(byte[] rawData, bool usingEcho)
    {
      this.UsingEcho = usingEcho;
      int startIndex = 0;
      byte[] bytes = SubArray.GetBytes(rawData, startIndex, 4);
      Array.Reverse((Array) bytes);
      this.Timestamp = BitConverter.ToUInt32(bytes, 0);
      int index = startIndex + 4;
      this.IsEcho = false;
      if (this.UsingEcho)
      {
        this.IsEcho = rawData[index] > (byte) 0;
        int num = index + 1;
      }
      this.Msg = new RP1210_MsgCan(SubArray.GetBytes(rawData, this.UsingEcho ? 5 : 4));
    }

    public int CreateListViewEntry(ListView parent)
    {
      string text = "";
      ListViewItem listViewItem;
      if (this.Msg.Extended)
      {
        listViewItem = parent.Items.Add("EXTENDED");
        listViewItem.SubItems.Add(string.Format("{0:X8}h", (object) this.Msg.ID));
      }
      else
      {
        listViewItem = parent.Items.Add("STANDARD");
        listViewItem.SubItems.Add(string.Format("{0:X4}h", (object) this.Msg.ID));
      }
      listViewItem.SubItems.Add(this.Msg.Length.ToString());
      for (int index = 0; index < this.Msg.Length; ++index)
        text = text + this.Msg.Data[index].ToString("X2") + " ";
      listViewItem.SubItems.Add(text);
      listViewItem.SubItems.Add("1");
      listViewItem.SubItems.Add(this.Timestamp.ToString());
      return listViewItem.Index;
    }

    public RP1210_MsgCan Msg { get; private set; }

    public uint Timestamp { get; private set; }

    public bool UsingEcho { get; private set; }

    public bool IsEcho { get; private set; }
  }
}
