
// Type: RP1210_Test.HelpClasses.IniFileReader




using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RP1210_Test.HelpClasses
{
    public class IniFileReader
    {
        [DllImport("kernel32", SetLastError = true)]
        private static extern int GetPrivateProfileString(
          string section,
          string key,
          string def,
          StringBuilder retVal,
          int size,
          string filePath);

        public IniFileReader()
          : this(string.Empty)
        {
        }

        public IniFileReader(string INIPath)
          : this(INIPath, string.Empty)
        {
        }

        public IniFileReader(string INIPath, string INISection)
        {
            this.Path = INIPath;
            this.Section = INISection;
        }

        public bool ReadString(string ValueKey, out string result) => this.ReadString(this.Section, ValueKey, out result);

        public bool ReadString(string INISection, string ValueKey, out string result)
        {
            this.Section = INISection;
            return IniFileReader.ReadString(this.Path, this.Section, ValueKey, out result);
        }

        public static bool ReadString(
          string INIPath,
          string INISection,
          string ValueKey,
          out string result)
        {
            StringBuilder retVal = new StringBuilder((int)byte.MaxValue);
            IniFileReader.GetPrivateProfileString(INISection, ValueKey, "", retVal, (int)byte.MaxValue, INIPath);
            result = retVal.ToString();
            return Marshal.GetLastWin32Error() == 0;
        }

        public void ReadString(string ValueKey, out string result, string defaultValue) => this.ReadString(this.Section, ValueKey, out result, defaultValue);

        public void ReadString(
          string INISection,
          string ValueKey,
          out string result,
          string defaultValue)
        {
            this.Section = INISection;
            IniFileReader.ReadString(this.Path, this.Section, ValueKey, out result, defaultValue);
        }

        public static void ReadString(
          string INIPath,
          string INISection,
          string ValueKey,
          out string result,
          string defaultValue)
        {
            StringBuilder retVal = new StringBuilder((int)byte.MaxValue);
            IniFileReader.GetPrivateProfileString(INISection, ValueKey, defaultValue, retVal, (int)byte.MaxValue, INIPath);
            result = retVal.ToString();
        }

        public string Path { get; set; }

        public string Section { get; set; }

        public bool Exists => File.Exists(this.Path);
    }
}
