
// Type: RP1210_Test.HelpClasses.RP1210_ProtocolInfo




using System;
using System.Collections.Generic;

namespace RP1210_Test.HelpClasses
{
  public struct RP1210_ProtocolInfo
  {
    public bool Load(RP1210_ImplementationInformation implementation, int ProtocolIndex)
    {
      List<RP1210_DeviceInfo> rp1210DeviceInfoList = new List<RP1210_DeviceInfo>();
      if (!implementation.IniFileFound || ProtocolIndex < 0)
        return false;
      this.Index = ProtocolIndex;
      this.ImplementationName = implementation.Name;
      IniFileReader iniFileReader = new IniFileReader(implementation.IniFilePath, string.Format("ProtocolInformation{0}", (object) ProtocolIndex));
      string result;
      iniFileReader.ReadString("ProtocolDescription", out result, "");
      this.ProtocolDescription = result;
      iniFileReader.ReadString("ProtocolSpeed", out result, "");
      this.ProtocolSpeed = result;
      iniFileReader.ReadString("ProtocolString", out result, "");
      this.ProtocolString = result;
      iniFileReader.ReadString("ProtocolParams", out result, "");
      this.ProtocolParams = result;
      iniFileReader.ReadString("Devices", out result, "");
      this.Devices = result;
      if (result != string.Empty)
      {
        string str1 = result;
        char[] chArray = new char[2]{ ',', ' ' };
        foreach (string str2 in str1.Split(chArray))
        {
          try
          {
            rp1210DeviceInfoList.Add(this.GetDevice(implementation, Convert.ToInt32(str2)));
          }
          catch
          {
          }
        }
        this.DevicesList = rp1210DeviceInfoList.ToArray();
      }
      return true;
    }

    public override string ToString() => this.ProtocolDescription;

    private RP1210_DeviceInfo GetDevice(
      RP1210_ImplementationInformation implementation,
      int DeviceIndex)
    {
      RP1210_DeviceInfo rp1210DeviceInfo = new RP1210_DeviceInfo();
      rp1210DeviceInfo.Load(implementation, DeviceIndex);
      return rp1210DeviceInfo;
    }

    public int Index { get; private set; }

    public string ImplementationName { get; private set; }

    public string ProtocolDescription { get; private set; }

    public string ProtocolSpeed { get; private set; }

    public string ProtocolString { get; private set; }

    public string ProtocolParams { get; private set; }

    public string Devices { get; private set; }

    public RP1210_DeviceInfo[] DevicesList { get; private set; }
  }
}
