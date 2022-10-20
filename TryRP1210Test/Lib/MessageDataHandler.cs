
// Type: RP1210_Test.HelpClasses.MessageDataHandler




using System;

namespace RP1210_Test.HelpClasses
{
  public abstract class MessageDataHandler
  {
    protected byte[] m_Data;
    protected int m_Length;

    public MessageDataHandler(int maxDataLength) => this.m_Data = new byte[maxDataLength];

    private byte[] GetData()
    {
      byte[] numArray = new byte[this.Length];
      Array.Copy((Array) this.m_Data, (Array) numArray, this.Length);
      return numArray;
    }

    private void SetData(byte[] value)
    {
      this.Length = 0;
      if (value == null)
        return;
      this.Length = value.Length > this.m_Data.Length ? this.m_Data.Length : value.Length;
      Array.Copy((Array) value, (Array) this.m_Data, this.Length);
    }

    public int Length
    {
      get => this.m_Length;
      set => this.m_Length = value > this.m_Data.Length ? this.m_Data.Length : value;
    }

    public byte[] Data
    {
      get => this.GetData();
      set => this.SetData(value);
    }
  }
}
