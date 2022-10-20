
// Type: RP1210_Test.HelpClasses.ListViewMessageSend




using Peak.RP1210C;
using System.Windows.Forms;

namespace RP1210_Test.HelpClasses
{
  public class ListViewMessageSend
  {
    private int m_Index;
    private object m_Message;
    private RP1210CProtocol m_Protocol;
    private ListView m_Parent;

    public ListViewMessageSend(ListView parent, object msg)
    {
      this.m_Message = msg;
      this.m_Protocol = (RP1210CProtocol) 0;
      this.m_Parent = parent;
      if ((object) (msg as RP1210_MsgCan) != null)
      {
        this.m_Protocol = RP1210CProtocol.CAN;
        this.m_Index = (this.m_Message as RP1210_MsgCan).CreateListViewEntry(this.m_Parent);
      }
      else if ((object) (msg as RP1210_MsgJ1939) != null)
      {
        this.m_Protocol = RP1210CProtocol.J1939;
        this.m_Index = (this.m_Message as RP1210_MsgJ1939).CreateListViewEntry(this.m_Parent);
      }
      else
      {
        if ((object) (msg as RP1210_MsgIso15765) == null)
          return;
        this.m_Protocol = RP1210CProtocol.ISO15765;
        this.m_Index = (this.m_Message as RP1210_MsgIso15765).CreateListViewEntry(this.m_Parent);
      }
    }

    public bool SameMessage(object msg)
    {
      switch (this.m_Protocol)
      {
        case RP1210CProtocol.CAN:
          return this.CompareToCAN(msg as RP1210_MsgCan);
        case RP1210CProtocol.J1939:
          return this.CompareToJ1939(msg as RP1210_MsgJ1939);
        case RP1210CProtocol.ISO15765:
          return this.CompareToISO15765(msg as RP1210_MsgIso15765);
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

    private bool CompareToCAN(RP1210_MsgCan msg)
    {
      if (msg == (RP1210_MsgCan) null)
        return false;
      RP1210_MsgCan canMsg = this.CANMsg;
      return (int) msg.ID == (int) canMsg.ID && msg.Extended == canMsg.Extended;
    }

    private void UpdateCAN()
    {
      RP1210_MsgCan message = this.m_Message as RP1210_MsgCan;
      if (message == (RP1210_MsgCan) null)
        return;
      ListViewItem listViewItem = this.m_Parent.Items[this.m_Index];
      string str = "";
      if (message.Extended)
      {
        listViewItem.SubItems[0].Text = "EXTENDED";
        listViewItem.SubItems[1].Text = string.Format("{0:X8}h", (object) message.ID);
      }
      else
      {
        listViewItem.SubItems[0].Text = "STANDARD";
        listViewItem.SubItems[1].Text = string.Format("{0:X4}h", (object) message.ID);
      }
      listViewItem.SubItems[2].Text = message.Length.ToString();
      for (int index = 0; index < message.Length; ++index)
        str = str + message.Data[index].ToString("X2") + " ";
      listViewItem.SubItems[3].Text = str;
    }

    internal void UpdateCount()
    {
      ListViewItem listViewItem = this.m_Parent.Items[this.m_Index];
      switch (this.m_Protocol)
      {
        case RP1210CProtocol.CAN:
          listViewItem.SubItems[4].Text = (int.Parse(listViewItem.SubItems[4].Text) + 1).ToString();
          break;
        case RP1210CProtocol.J1939:
          listViewItem.SubItems[5].Text = (int.Parse(listViewItem.SubItems[5].Text) + 1).ToString();
          break;
        case RP1210CProtocol.ISO15765:
          listViewItem.SubItems[4].Text = (int.Parse(listViewItem.SubItems[4].Text) + 1).ToString();
          break;
      }
    }

    private bool CompareToJ1939(RP1210_MsgJ1939 msg)
    {
      if (msg == (RP1210_MsgJ1939) null)
        return false;
      RP1210_MsgJ1939 j1939Msg = this.J1939Msg;
      return (int) msg.PGN == (int) j1939Msg.PGN && (int) msg.SourceAddress == (int) j1939Msg.SourceAddress && (int) msg.DestinationAddress == (int) j1939Msg.DestinationAddress;
    }

    private void UpdateJ1939()
    {
      RP1210_MsgJ1939 message = this.m_Message as RP1210_MsgJ1939;
      if (message == (RP1210_MsgJ1939) null)
        return;
      ListViewItem listViewItem = this.m_Parent.Items[this.m_Index];
      string str = "";
      listViewItem.SubItems[0].Text = message.PGN.ToString();
      listViewItem.SubItems[1].Text = message.Priority.ToString();
      listViewItem.SubItems[2].Text = message.SourceAddress.ToString();
      listViewItem.SubItems[3].Text = message.DestinationAddress.ToString();
      listViewItem.SubItems[4].Text = message.Length.ToString();
      for (int index = 0; index < message.Length; ++index)
        str = str + message.Data[index].ToString("X2") + " ";
      listViewItem.SubItems[6].Text = str;
    }

    private bool CompareToISO15765(RP1210_MsgIso15765 msg)
    {
      if (msg == (RP1210_MsgIso15765) null)
        return false;
      RP1210_MsgIso15765 isO15765Msg = this.ISO15765Msg;
      return msg.MessageType == isO15765Msg.MessageType && (int) msg.ID == (int) isO15765Msg.ID && (int) msg.ExtendedAddress == (int) isO15765Msg.ExtendedAddress;
    }

    private void UpdateISO15765()
    {
      RP1210_MsgIso15765 message = this.m_Message as RP1210_MsgIso15765;
      if (message == (RP1210_MsgIso15765) null)
        return;
      ListViewItem listViewItem = this.m_Parent.Items[this.m_Index];
      string str = "";
      bool flag = message.MessageType == RP1210CISO15765MsgType.EXTENDED_CAN || message.MessageType == RP1210CISO15765MsgType.EXTENDED_CAN_ISO15765_EXTENDED;
      listViewItem.SubItems[0].Text = message.MessageType.ToString();
      listViewItem.SubItems[1].Text = string.Format(flag ? "{0:X8}h" : "{0:X4}h", (object) message.ID);
      listViewItem.SubItems[2].Text = string.Format("{0}", (object) message.ExtendedAddress);
      listViewItem.SubItems[3].Text = message.Length.ToString();
      for (int index = 0; index < message.Length; ++index)
        str = str + message.Data[index].ToString("X2") + " ";
      listViewItem.SubItems[5].Text = str;
    }

    public int Index => this.m_Index;

    public RP1210_MsgCan CANMsg => (object) (this.m_Message as RP1210_MsgCan) == null ? (RP1210_MsgCan) null : (RP1210_MsgCan) this.m_Message;

    public RP1210_MsgJ1939 J1939Msg => (object) (this.m_Message as RP1210_MsgJ1939) == null ? (RP1210_MsgJ1939) null : (RP1210_MsgJ1939) this.m_Message;

    public RP1210_MsgIso15765 ISO15765Msg => (object) (this.m_Message as RP1210_MsgIso15765) == null ? (RP1210_MsgIso15765) null : (RP1210_MsgIso15765) this.m_Message;

    public RP1210CProtocol Protocol => this.m_Protocol;
  }
}
