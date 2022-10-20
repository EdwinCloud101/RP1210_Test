
// Type: RP1210_Test.HelpClasses.RP1210_ImplementationInformation




using System;
using System.IO;

namespace RP1210_Test.HelpClasses
{
  public struct RP1210_ImplementationInformation
  {
    private string m_Implementation;
    private static string m_WindowsPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
    private static string m_System32Path = Environment.GetFolderPath(Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.SystemX86 : Environment.SpecialFolder.System);
    private static string m_System64Path = Environment.Is64BitOperatingSystem ? Environment.GetFolderPath(Environment.SpecialFolder.System) : string.Empty;

    public RP1210_ImplementationInformation(string implementation) => this.m_Implementation = implementation;

    private static string[] GetRP1210Implementations()
    {
      string result;
      if (!IniFileReader.ReadString(RP1210_ImplementationInformation.RP1210IniPath, "RP1210Support", "APIImplementations", out result))
        return new string[0];
      return result.Split(new char[2]{ ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public string Name => this.m_Implementation;

    public bool Exists
    {
      get
      {
        if (!this.IniFileFound)
          return false;
        return this.Library32Found || this.Library64Found;
      }
    }

    public string IniFilePath => RP1210_ImplementationInformation.m_WindowsPath + "\\" + this.m_Implementation + ".ini";

    public string Library32Path => RP1210_ImplementationInformation.m_System32Path + "\\" + this.m_Implementation + ".dll";

    public string Library64Path => RP1210_ImplementationInformation.m_System64Path + "\\" + this.m_Implementation + ".dll";

    public bool IniFileFound => File.Exists(this.IniFilePath);

    public bool Library32Found => File.Exists(this.Library32Path);

    public bool Library64Found => File.Exists(this.Library64Path);

    public static string[] Implementations => RP1210_ImplementationInformation.GetRP1210Implementations();

    public static string RP1210IniPath => Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\RP121032.ini";
  }
}
