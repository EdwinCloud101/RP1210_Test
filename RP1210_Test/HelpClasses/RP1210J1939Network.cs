
// Type: RP1210_Test.HelpClasses.RP1210J1939Network




using System.IO;

namespace RP1210_Test.HelpClasses
{
  public class RP1210J1939Network
  {
    private byte m_Addr;
    private RP1210J1939Name m_Name;

    public RP1210J1939Network()
      : this((byte) 0, new RP1210J1939Name())
    {
    }

    public RP1210J1939Network(byte address)
      : this(address, new RP1210J1939Name())
    {
    }

    public RP1210J1939Network(RP1210J1939Network obj)
    {
      this.m_Addr = obj.m_Addr;
      this.m_Name = obj.m_Name;
      this.Claimed = obj.Claimed;
    }

    public RP1210J1939Network(byte address, RP1210J1939Name name)
    {
      this.m_Addr = address;
      this.m_Name = name;
      this.Claimed = false;
    }

    private void SetAddress(byte value) => this.m_Addr = value <= (byte) 253 ? value : throw new InvalidDataException("Address cannot be NULL-Address or Global-Address");

    public byte Address
    {
      get => this.m_Addr;
      set => this.SetAddress(value);
    }

    public RP1210J1939Name Name
    {
      get => this.m_Name;
      set => this.m_Name = new RP1210J1939Name(value);
    }

    public bool Claimed { get; set; }
  }
}
