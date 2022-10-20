
// Type: RP1210_Test.HelpClasses.RP1210_SerialisableMsg




namespace RP1210_Test.HelpClasses
{
  public interface RP1210_SerialisableMsg
  {
    byte[] ToByteArray();

    bool Parse(byte[] rawData);

    short WriteBufferLength { get; }
  }
}
