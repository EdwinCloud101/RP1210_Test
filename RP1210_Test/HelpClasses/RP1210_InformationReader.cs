
// Type: RP1210_Test.HelpClasses.RP1210_InformationReader




using System;
using System.Collections.Generic;

namespace RP1210_Test.HelpClasses
{
  public class RP1210_InformationReader
  {
    private RP1210_ImplementationInformation m_FilesInfo;
    private IniFileReader m_Ini;

    public RP1210_InformationReader(string implementation)
    {
      this.m_FilesInfo = new RP1210_ImplementationInformation(implementation);
      this.m_Ini = new IniFileReader(this.m_FilesInfo.IniFilePath, "VendorInformation");
    }

    public string GetValue(RP1210_InformationReader.VendorInformation value)
    {
      string result;
      return !this.m_Ini.ReadString(value.ToString(), out result) ? string.Empty : result;
    }

    private string GetValue(RP1210_InformationReader.VendorInformation value, string defaultValue)
    {
      string result;
      this.m_Ini.ReadString(value.ToString(), out result, defaultValue);
      return result;
    }

    private RP1210_DeviceInfo[] GetDevices()
    {
      List<RP1210_DeviceInfo> rp1210DeviceInfoList = new List<RP1210_DeviceInfo>();
      string str1 = this.GetValue(RP1210_InformationReader.VendorInformation.Devices);
      char[] chArray = new char[2]{ ',', ' ' };
      foreach (string str2 in str1.Split(chArray))
        rp1210DeviceInfoList.Add(this.GetDevice(Convert.ToInt32(str2)));
      return rp1210DeviceInfoList.ToArray();
    }

    private RP1210_ProtocolInfo[] GetProtocols()
    {
      List<RP1210_ProtocolInfo> rp1210ProtocolInfoList = new List<RP1210_ProtocolInfo>();
      string str1 = this.GetValue(RP1210_InformationReader.VendorInformation.Protocols);
      char[] chArray = new char[2]{ ',', ' ' };
      foreach (string str2 in str1.Split(chArray))
        rp1210ProtocolInfoList.Add(this.GetProtocol(Convert.ToInt32(str2)));
      return rp1210ProtocolInfoList.ToArray();
    }

    private RP1210_DeviceInfo GetDevice(int Index)
    {
      RP1210_DeviceInfo rp1210DeviceInfo = new RP1210_DeviceInfo();
      rp1210DeviceInfo.Load(this.m_FilesInfo, Index);
      return rp1210DeviceInfo;
    }

    private RP1210_ProtocolInfo GetProtocol(int Index)
    {
      RP1210_ProtocolInfo rp1210ProtocolInfo = new RP1210_ProtocolInfo();
      rp1210ProtocolInfo.Load(this.m_FilesInfo, Index);
      return rp1210ProtocolInfo;
    }

    public RP1210_ImplementationInformation Implementation => this.m_FilesInfo;

    public string Name => this.GetValue(RP1210_InformationReader.VendorInformation.Name);

    public string Address1 => this.GetValue(RP1210_InformationReader.VendorInformation.Address1);

    public string Address2 => this.GetValue(RP1210_InformationReader.VendorInformation.Address2);

    public string City => this.GetValue(RP1210_InformationReader.VendorInformation.City);

    public string State => this.GetValue(RP1210_InformationReader.VendorInformation.State);

    public string Country => this.GetValue(RP1210_InformationReader.VendorInformation.Country);

    public string Postal => this.GetValue(RP1210_InformationReader.VendorInformation.Postal);

    public string Telephone => this.GetValue(RP1210_InformationReader.VendorInformation.Telephone);

    public string Fax => this.GetValue(RP1210_InformationReader.VendorInformation.Fax);

    public string VendorURL => this.GetValue(RP1210_InformationReader.VendorInformation.VendorURL);

    public string MessageString => this.GetValue(RP1210_InformationReader.VendorInformation.MessageString);

    public string ErrorString => this.GetValue(RP1210_InformationReader.VendorInformation.ErrorString);

    public int TimestampWeight => Convert.ToInt32(this.GetValue(RP1210_InformationReader.VendorInformation.TimestampWeight, "1000"));

    public string AutoDetectCapable => this.GetValue(RP1210_InformationReader.VendorInformation.AutoDetectCapable, "no");

    public string Version => this.GetValue(RP1210_InformationReader.VendorInformation.Version);

    public string RP1210 => this.GetValue(RP1210_InformationReader.VendorInformation.RP1210, "A");

    public int DebugLevel => Convert.ToInt32(this.GetValue(RP1210_InformationReader.VendorInformation.DebugLevel, "-1"));

    public string DebugFile => this.GetValue(RP1210_InformationReader.VendorInformation.DebugFile);

    public int DebugMode => Convert.ToInt32(this.GetValue(RP1210_InformationReader.VendorInformation.DebugMode, "0"));

    public int DebugFileSize => Convert.ToInt32(this.GetValue(RP1210_InformationReader.VendorInformation.DebugFileSize, "1024"));

    public int NumberOfRTSCTSSessions => Convert.ToInt32(this.GetValue(RP1210_InformationReader.VendorInformation.NumberOfRTSCTSSessions, "1"));

    public string CANFormatsSupported => this.GetValue(RP1210_InformationReader.VendorInformation.CANFormatsSupported, "4");

    public string J1939FormatsSupported => this.GetValue(RP1210_InformationReader.VendorInformation.J1939FormatsSupported, "2");

    public int J1939Addresses => Convert.ToInt32(this.GetValue(RP1210_InformationReader.VendorInformation.J1939Addresses, "1"));

    public bool CANAutoBaud => Convert.ToBoolean(this.GetValue(RP1210_InformationReader.VendorInformation.CANAutoBaud, "False"));

    public string J1708FormatsSupported => this.GetValue(RP1210_InformationReader.VendorInformation.J1708FormatsSupported, "2");

    public string ISO15765FormatsSupported => this.GetValue(RP1210_InformationReader.VendorInformation.ISO15765FormatsSupported, "2");

    public string Devices => this.GetValue(RP1210_InformationReader.VendorInformation.Devices);

    public RP1210_DeviceInfo[] DevicesList => this.GetDevices();

    public string Protocols => this.GetValue(RP1210_InformationReader.VendorInformation.Protocols);

    public RP1210_ProtocolInfo[] ProtocolsList => this.GetProtocols();

    public enum VendorInformation
    {
      Name,
      Address1,
      Address2,
      City,
      State,
      Country,
      Postal,
      Telephone,
      Fax,
      VendorURL,
      MessageString,
      ErrorString,
      TimestampWeight,
      AutoDetectCapable,
      Version,
      RP1210,
      DebugLevel,
      DebugFile,
      DebugMode,
      DebugFileSize,
      NumberOfRTSCTSSessions,
      CANFormatsSupported,
      J1939FormatsSupported,
      J1939Addresses,
      CANAutoBaud,
      J1708FormatsSupported,
      ISO15765FormatsSupported,
      Devices,
      Protocols,
    }
  }
}
