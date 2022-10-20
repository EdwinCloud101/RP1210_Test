
// Type: RP1210_Test.HelpClasses.RP1210_DeviceInfo




using System;

namespace RP1210_Test.HelpClasses
{
  public struct RP1210_DeviceInfo
  {
    public bool Load(RP1210_ImplementationInformation implementation, int deviceIndex)
    {
      if (!implementation.IniFileFound || deviceIndex < 0)
        return false;
      this.ImplementationName = implementation.Name;
      this.Index = deviceIndex;
      IniFileReader iniFileReader = new IniFileReader(implementation.IniFilePath, string.Format("DeviceInformation{0}", (object) deviceIndex));
      string result;
      if (!iniFileReader.ReadString("DeviceID", out result))
        return false;
      this.DeviceID = Convert.ToInt32(result);
      if (!iniFileReader.ReadString("DeviceDescription", out result))
        return false;
      this.DeviceDescription = result;
      if (!iniFileReader.ReadString("DeviceName", out result))
        return false;
      this.DeviceName = result;
      if (!iniFileReader.ReadString("DeviceParams", out result))
        return false;
      this.DeviceParams = result;
      if (!iniFileReader.ReadString("MultiCANChannels", out result))
        return false;
      this.MultiCANChannels = Convert.ToInt32(result);
      if (!iniFileReader.ReadString("MultiJ1939Channels", out result))
        return false;
      this.MultiJ1939Channels = Convert.ToInt32(result);
      if (!iniFileReader.ReadString("MultiISO15765Channels", out result))
        return false;
      this.MultiISO15765Channels = Convert.ToInt32(result);
      return true;
    }

    public override string ToString() => string.Format("({0}) {1}", (object) this.DeviceID, (object) this.DeviceDescription);

    public int Index { get; private set; }

    public string ImplementationName { get; private set; }

    public int DeviceID { get; internal set; }

    public string DeviceDescription { get; private set; }

    public string DeviceName { get; private set; }

    public string DeviceParams { get; private set; }

    public int MultiCANChannels { get; private set; }

    public int MultiJ1939Channels { get; private set; }

    public int MultiISO15765Channels { get; private set; }
  }
}
