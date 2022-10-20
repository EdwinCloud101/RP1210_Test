
// Type: RP1210_Test.HelpClasses.RP1210_MsgIso15765Rx




using Peak.Classes;
using Peak.RP1210C;
using System;
using System.Windows.Forms;

namespace RP1210_Test.HelpClasses
{
  public class RP1210_MsgIso15765Rx
  {
    public RP1210_MsgIso15765Rx(byte[] rawData)
      : this(rawData, false)
    {
    }

    public RP1210_MsgIso15765Rx(byte[] rawData, bool usingEcho)
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
        ++index;
      }
      this.Indication = (RP1210CISO15765Indication) rawData[index];
      int num = index + 1;
      this.Msg = new RP1210_MsgIso15765(SubArray.GetBytes(rawData, this.UsingEcho ? 6 : 5));
    }

    public int CreateListViewEntry(ListView parent)
    {
      string str = "";
      ListViewItem listViewItem = parent.Items.Add(this.GetIndicationString());
      listViewItem.SubItems.Add(this.Msg.MessageType.ToString());
      listViewItem.SubItems.Add(string.Format("{0:X8}h", (object) this.Msg.ID));
      listViewItem.SubItems.Add(string.Format("{0:X}h", (object) this.Msg.ExtendedAddress));
      listViewItem.SubItems.Add("1");
      listViewItem.SubItems.Add(this.Timestamp.ToString());
      listViewItem.SubItems.Add(this.Msg.Length.ToString());
      if (this.Indication == RP1210CISO15765Indication.ISO15765_ACTUAL_MESSAGE)
      {
        for (int index = 0; index < this.Msg.Length; ++index)
          str = str + this.Msg.Data[index].ToString("X2") + " ";
      }
      listViewItem.SubItems.Add(this.GetDataField());
      return listViewItem.Index;
    }

    private string GetDataField()
    {
      string str1 = "";
      switch (this.Indication)
      {
        case RP1210CISO15765Indication.ISO15765_ACTUAL_MESSAGE:
        case RP1210CISO15765Indication.ISO15765_FF_INDICATION:
          for (int index = 0; index < this.Msg.Length; ++index)
            str1 = str1 + this.Msg.Data[index].ToString("X2") + " ";
          return str1;
        case RP1210CISO15765Indication.ISO15765_CONFIRM:
          string str2 = string.Format("Result: {0} - Data: ", (object) (RP1210CISO15765Error) this.Msg.Data[0]);
          for (int index = 1; index < this.Msg.Length; ++index)
            str2 = str2 + this.Msg.Data[index].ToString("X2") + " ";
          return str2;
        case RP1210CISO15765Indication.ISO15765_RX_ERROR_INDICATION:
          return ((RP1210CISO15765Error) this.Msg.Data[0]).ToString();
        default:
          return "";
      }
    }

    private string GetIndicationString()
    {
      switch (this.Indication)
      {
        case RP1210CISO15765Indication.ISO15765_ACTUAL_MESSAGE:
          return "Diagnostic";
        case RP1210CISO15765Indication.ISO15765_CONFIRM:
          return "Confirmation";
        case RP1210CISO15765Indication.ISO15765_FF_INDICATION:
          return "Indication";
        case RP1210CISO15765Indication.ISO15765_RX_ERROR_INDICATION:
          return "Error";
        default:
          return "";
      }
    }

    public RP1210_MsgIso15765 Msg { get; private set; }

    public uint Timestamp { get; private set; }

    public RP1210CISO15765Indication Indication { get; private set; }

    public bool UsingEcho { get; private set; }

    public bool IsEcho { get; private set; }
  }
}
