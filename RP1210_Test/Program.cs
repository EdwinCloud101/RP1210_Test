
// Type: RP1210_Test.Program




using System;
using System.Windows.Forms;

namespace RP1210_Test
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new Form1());
    }
  }
}
