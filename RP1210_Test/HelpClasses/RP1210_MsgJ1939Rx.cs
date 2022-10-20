
// Type: RP1210_Test.HelpClasses.RP1210_MsgJ1939Rx




using Peak.Classes;
using System;
using System.Windows.Forms;

namespace RP1210_Test.HelpClasses
{
  public class RP1210_MsgJ1939Rx
  {
    public RP1210_MsgJ1939Rx(byte[] rawData)
      : this(rawData, false)
    {
    }

    public RP1210_MsgJ1939Rx(byte[] rawData, bool usingEcho)
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
      this.Msg = new RP1210_MsgJ1939(SubArray.GetBytes(rawData, this.UsingEcho ? 5 : 4));
    }

    public int CreateListViewEntry(ListView parent)
    {
      string text = "";
      ListViewItem listViewItem = parent.Items.Add(this.Msg.PGN.ToString());
      listViewItem.SubItems.Add(this.Msg.Priority.ToString());
      listViewItem.SubItems.Add(this.Msg.SourceAddress.ToString());
      listViewItem.SubItems.Add(this.Msg.DestinationAddress.ToString());
      listViewItem.SubItems.Add(this.Msg.Length.ToString());
      listViewItem.SubItems.Add("1");
      listViewItem.SubItems.Add(this.Timestamp.ToString());
      for (int index = 0; index < this.Msg.Length; ++index)
        text = text + this.Msg.Data[index].ToString("X2") + " ";
      listViewItem.SubItems.Add(text);
      return listViewItem.Index;
    }

    public RP1210_MsgJ1939 Msg { get; private set; }

    public uint Timestamp { get; private set; }

    public bool UsingEcho { get; private set; }

    public bool IsEcho { get; private set; }
  }
}
