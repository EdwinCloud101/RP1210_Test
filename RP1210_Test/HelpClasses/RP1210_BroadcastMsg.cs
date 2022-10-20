
// Type: RP1210_Test.HelpClasses.RP1210_BroadcastMsg




using Peak.Classes;
using Peak.RP1210C;
using System;

namespace RP1210_Test.HelpClasses
{
  public class RP1210_BroadcastMsg : IEquatable<RP1210_BroadcastMsg>
  {
    public RP1210_BroadcastMsg(RP1210_SerialisableMsg msg, ushort interval, byte entryNumber)
    {
      this.Interval = interval;
      this.Msg = msg;
      this.EntryNumber = entryNumber;
      this.Protocol = (RP1210CProtocol) 0;
      if ((object) (this.Msg as RP1210_MsgCan) != null)
        this.Protocol = RP1210CProtocol.CAN;
      if ((object) (this.Msg as RP1210_MsgJ1939) != null)
        this.Protocol = RP1210CProtocol.J1939;
      if ((object) (this.Msg as RP1210_MsgIso15765) == null)
        return;
      this.Protocol = RP1210CProtocol.ISO15765;
    }

    public RP1210_BroadcastMsg(byte[] rawData, RP1210CProtocol protocol, byte entryNumber)
    {
      this.Interval = BitConverter.ToUInt16(rawData, 0);
      this.Protocol = protocol;
      this.EntryNumber = entryNumber;
      switch (protocol)
      {
        case RP1210CProtocol.CAN:
          this.Msg = (RP1210_SerialisableMsg) new RP1210_MsgCan(SubArray.GetBytes(rawData, 2));
          break;
        case RP1210CProtocol.J1939:
          this.Msg = (RP1210_SerialisableMsg) new RP1210_MsgJ1939(SubArray.GetBytes(rawData, 2));
          break;
        case RP1210CProtocol.ISO15765:
          this.Msg = (RP1210_SerialisableMsg) new RP1210_MsgIso15765(SubArray.GetBytes(rawData, 2));
          break;
        default:
          this.Msg = (RP1210_SerialisableMsg) null;
          throw new ArgumentException("Unknow protocol. Cannot parse the data to a RP1210_SerialisableMsg object");
      }
    }

    public static bool operator ==(RP1210_BroadcastMsg broadcast1, RP1210_BroadcastMsg broadcast2)
    {
      if ((object) broadcast2 == null || (int) broadcast1.Interval != (int) broadcast2.Interval || broadcast1.Protocol != broadcast2.Protocol)
        return false;
      switch (broadcast1.Protocol)
      {
        case RP1210CProtocol.CAN:
          return broadcast1.Msg as RP1210_MsgCan == broadcast2.Msg as RP1210_MsgCan;
        case RP1210CProtocol.J1939:
          return broadcast1.Msg as RP1210_MsgJ1939 == broadcast2.Msg as RP1210_MsgJ1939;
        case RP1210CProtocol.ISO15765:
          return broadcast1.Msg as RP1210_MsgIso15765 == broadcast2.Msg as RP1210_MsgIso15765;
        default:
          return false;
      }
    }

    public static bool operator !=(RP1210_BroadcastMsg broadcast1, RP1210_BroadcastMsg broadcast2) => !(broadcast1 == broadcast2);

    public override bool Equals(object obj) => obj != null && (object) (obj as RP1210_BroadcastMsg) != null && this.Equals(obj as RP1210_BroadcastMsg);

    public override int GetHashCode() => base.GetHashCode();

    public bool Equals(RP1210_BroadcastMsg obj) => obj == this;

    public byte EntryNumber { get; private set; }

    public ushort Interval { get; private set; }

    public RP1210_SerialisableMsg Msg { get; private set; }

    public RP1210CProtocol Protocol { get; private set; }
  }
}
