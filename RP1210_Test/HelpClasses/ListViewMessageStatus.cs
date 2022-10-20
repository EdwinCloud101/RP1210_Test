
// Type: RP1210_Test.HelpClasses.ListViewMessageStatus




using Peak.RP1210C;
using System.Windows.Forms;

namespace RP1210_Test.HelpClasses
{
  public class ListViewMessageStatus
  {
    private int m_Index;
    private object m_Message;
    private RP1210CProtocol m_Protocol;
    private ListView m_Parent;

    public ListViewMessageStatus(ListView parent, object msg)
    {
      this.m_Message = msg;
      this.m_Protocol = (RP1210CProtocol) 0;
      this.m_Parent = parent;
      if (msg is RP1210_MsgCanRx)
      {
        this.m_Protocol = RP1210CProtocol.CAN;
        this.m_Index = (this.m_Message as RP1210_MsgCanRx).CreateListViewEntry(this.m_Parent);
      }
      if (msg is RP1210_MsgJ1939Rx)
      {
        this.m_Protocol = RP1210CProtocol.J1939;
        this.m_Index = (this.m_Message as RP1210_MsgJ1939Rx).CreateListViewEntry(this.m_Parent);
      }
      if (!(msg is RP1210_MsgIso15765Rx))
        return;
      this.m_Protocol = RP1210CProtocol.ISO15765;
      this.m_Index = (this.m_Message as RP1210_MsgIso15765Rx).CreateListViewEntry(this.m_Parent);
    }

    public bool SameMessage(object msg)
    {
      switch (this.m_Protocol)
      {
        case RP1210CProtocol.CAN:
          return this.CompareToCAN(msg as RP1210_MsgCanRx);
        case RP1210CProtocol.J1939:
          return this.CompareToJ1939(msg as RP1210_MsgJ1939Rx);
        case RP1210CProtocol.ISO15765:
          return this.CompareToISO15765(msg as RP1210_MsgIso15765Rx);
        default:
          return false;
      }
    }

    public void Update(object msg)
    {
      this.m_Message = msg;
      switch (this.m_Protocol)
      {
        case RP1210CProtocol.CAN:
          this.UpdateCAN();
          break;
        case RP1210CProtocol.J1939:
          this.UpdateJ1939();
          break;
        case RP1210CProtocol.ISO15765:
          this.UpdateISO15765();
          break;
      }
    }

    private void UpdateCAN()
    {
      if (!(this.m_Message is RP1210_MsgCanRx message))
        return;
      string str1 = "";
      ListViewItem listViewItem = this.m_Parent.Items[this.m_Index];
      ListViewItem.ListViewSubItem subItem1 = listViewItem.SubItems[2];
      int num = message.Msg.Length;
      string str2 = num.ToString();
      subItem1.Text = str2;
      for (int index = 0; index < message.Msg.Length; ++index)
        str1 = str1 + message.Msg.Data[index].ToString("X2") + " ";
      listViewItem.SubItems[3].Text = str1;
      ListViewItem.ListViewSubItem subItem2 = listViewItem.SubItems[4];
      num = int.Parse(listViewItem.SubItems[4].Text) + 1;
      string str3 = num.ToString();
      subItem2.Text = str3;
      listViewItem.SubItems[5].Text = message.Timestamp.ToString();
    }

    private void UpdateJ1939()
    {
      if (!(this.m_Message is RP1210_MsgJ1939Rx message))
        return;
      string str = "";
      ListViewItem listViewItem = this.m_Parent.Items[this.m_Index];
      listViewItem.SubItems[1].Text = message.Msg.Priority.ToString();
      listViewItem.SubItems[4].Text = message.Msg.Length.ToString();
      listViewItem.SubItems[5].Text = (int.Parse(listViewItem.SubItems[5].Text) + 1).ToString();
      listViewItem.SubItems[6].Text = message.Timestamp.ToString();
      for (int index = 0; index < message.Msg.Length; ++index)
        str = str + message.Msg.Data[index].ToString("X2") + " ";
      listViewItem.SubItems[7].Text = str;
    }

    private void UpdateISO15765()
    {
      if (!(this.m_Message is RP1210_MsgIso15765Rx message))
        return;
      string str = "";
      ListViewItem listViewItem = this.m_Parent.Items[this.m_Index];
      listViewItem.SubItems[4].Text = (int.Parse(listViewItem.SubItems[4].Text) + 1).ToString();
      listViewItem.SubItems[5].Text = message.Timestamp.ToString();
      listViewItem.SubItems[6].Text = message.Msg.Length.ToString();
      for (int index = 0; index < message.Msg.Length; ++index)
        str = str + message.Msg.Data[index].ToString("X2") + " ";
      listViewItem.SubItems[7].Text = str;
    }

    private bool CompareToCAN(RP1210_MsgCanRx msg)
    {
      if (msg == null)
        return false;
      RP1210_MsgCanRx canMsg = this.CANMsg;
      return (int) msg.Msg.ID == (int) canMsg.Msg.ID && msg.Msg.Extended == canMsg.Msg.Extended;
    }

    private bool CompareToJ1939(RP1210_MsgJ1939Rx msg)
    {
      if (msg == null)
        return false;
      RP1210_MsgJ1939Rx j1939Msg = this.J1939Msg;
      return (int) msg.Msg.PGN == (int) j1939Msg.Msg.PGN && (int) msg.Msg.SourceAddress == (int) j1939Msg.Msg.SourceAddress && (int) msg.Msg.DestinationAddress == (int) j1939Msg.Msg.DestinationAddress;
    }

    private bool CompareToISO15765(RP1210_MsgIso15765Rx msg)
    {
      if (msg == null)
        return false;
      RP1210_MsgIso15765Rx isO15765Msg = this.ISO15765Msg;
      return msg.Indication == isO15765Msg.Indication && msg.Msg.MessageType == isO15765Msg.Msg.MessageType && (int) msg.Msg.ID == (int) isO15765Msg.Msg.ID && (int) msg.Msg.ExtendedAddress == (int) isO15765Msg.Msg.ExtendedAddress;
    }

    public int Index => this.m_Index;

    public RP1210_MsgCanRx CANMsg => !(this.m_Message is RP1210_MsgCanRx) ? (RP1210_MsgCanRx) null : (RP1210_MsgCanRx) this.m_Message;

    public RP1210_MsgJ1939Rx J1939Msg => !(this.m_Message is RP1210_MsgJ1939Rx) ? (RP1210_MsgJ1939Rx) null : (RP1210_MsgJ1939Rx) this.m_Message;

    public RP1210_MsgIso15765Rx ISO15765Msg => !(this.m_Message is RP1210_MsgIso15765Rx) ? (RP1210_MsgIso15765Rx) null : (RP1210_MsgIso15765Rx) this.m_Message;

    public RP1210CProtocol Protocol => this.m_Protocol;
  }
}
