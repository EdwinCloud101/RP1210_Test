
// Type: RP1210_Test.HelpClasses.RP1210ISO15765FlowControl




using Peak.RP1210C;
using System;
using System.Collections.Generic;

namespace RP1210_Test.HelpClasses
{
  public class RP1210ISO15765FlowControl
  {
    public RP1210ISO15765FlowControl(byte[] rawData) => this.Parse(rawData);

    public RP1210ISO15765FlowControl(
      RP1210CISO15765MsgType msgType = RP1210CISO15765MsgType.STANDARD_CAN,
      uint incomingID = 256,
      byte incomingExtendedID = 0,
      uint outgoingID = 257,
      byte outgoingExtendedID = 0,
      byte blockSize = 0,
      byte separationTime = 10,
      ushort separationTimeTx = 10)
    {
      this.MsgType = msgType;
      this.IncomingCanID = incomingID;
      this.IncomingExtendedCanID = incomingExtendedID;
      this.OutgoingCanID = outgoingID;
      this.OutgoingExtendedCanID = outgoingExtendedID;
      this.BlockSize = blockSize;
      this.SeparationTime = separationTime;
      this.SeparationTimeTx = separationTimeTx;
    }

    public static bool operator ==(RP1210ISO15765FlowControl flow1, RP1210ISO15765FlowControl flow2) => flow1.MsgType == flow2.MsgType && (int) flow1.IncomingCanID == (int) flow2.IncomingCanID && ((int) flow1.IncomingExtendedCanID == (int) flow2.IncomingExtendedCanID && (int) flow1.OutgoingCanID == (int) flow2.OutgoingCanID) && ((int) flow1.OutgoingExtendedCanID == (int) flow2.OutgoingExtendedCanID && (int) flow1.BlockSize == (int) flow2.BlockSize && ((int) flow1.SeparationTime == (int) flow2.SeparationTime && (int) flow1.SeparationTimeTx == (int) flow2.SeparationTimeTx));

    public static bool operator !=(RP1210ISO15765FlowControl flow1, RP1210ISO15765FlowControl flow2) => !(flow1 == flow2);

    public override bool Equals(object obj) => obj != null && (object) (obj as RP1210ISO15765FlowControl) != null && this.Equals(obj as RP1210ISO15765FlowControl);

    public override int GetHashCode() => base.GetHashCode();

    public bool Equals(RP1210ISO15765FlowControl obj) => obj == this;

    public byte[] ToByteArray()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) this.MsgType);
      byte[] bytes1 = BitConverter.GetBytes(this.IncomingCanID);
      Array.Reverse((Array) bytes1);
      byteList.AddRange((IEnumerable<byte>) bytes1);
      byteList.Add(this.IncomingExtendedCanID);
      byte[] bytes2 = BitConverter.GetBytes(this.OutgoingCanID);
      Array.Reverse((Array) bytes2);
      byteList.AddRange((IEnumerable<byte>) bytes2);
      byteList.Add(this.OutgoingExtendedCanID);
      byteList.Add(this.BlockSize);
      byteList.Add(this.SeparationTime);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.SeparationTimeTx));
      return byteList.ToArray();
    }

    public bool Parse(byte[] rawData)
    {
      try
      {
        this.MsgType = (RP1210CISO15765MsgType) rawData[0];
        byte[] numArray1 = new byte[4];
        Array.Copy((Array) rawData, 1, (Array) numArray1, 0, numArray1.Length);
        Array.Reverse((Array) numArray1);
        this.IncomingCanID = BitConverter.ToUInt32(numArray1, 0);
        this.IncomingExtendedCanID = rawData[5];
        Array.Copy((Array) rawData, 6, (Array) numArray1, 0, numArray1.Length);
        Array.Reverse((Array) numArray1);
        this.OutgoingCanID = BitConverter.ToUInt32(numArray1, 0);
        this.OutgoingExtendedCanID = rawData[10];
        this.BlockSize = rawData[11];
        this.SeparationTime = rawData[12];
        byte[] numArray2 = new byte[2];
        Array.Copy((Array) rawData, 13, (Array) numArray2, 0, numArray2.Length);
        this.SeparationTimeTx = BitConverter.ToUInt16(numArray2, 0);
        return true;
      }
      catch
      {
        return false;
      }
    }

    protected short GetWriteBufferLength() => 15;

    public byte[] ByteArray
    {
      get => this.ToByteArray();
      set => this.Parse(value);
    }

    public RP1210CISO15765MsgType MsgType { get; set; }

    public uint IncomingCanID { get; set; }

    public byte IncomingExtendedCanID { get; set; }

    public uint OutgoingCanID { get; set; }

    public byte OutgoingExtendedCanID { get; set; }

    public byte BlockSize { get; set; }

    public byte SeparationTime { get; set; }

    public ushort SeparationTimeTx { get; set; }
  }
}
