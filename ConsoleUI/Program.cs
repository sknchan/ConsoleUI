using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ConsoleUI
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            ConsoleHelper.WriteDeafult();
            ConsoleHelper.WriteLog("1");
            ConsoleHelper.WriteLog("2");
            ConsoleHelper.WriteLog("3");
            ConsoleHelper.WriteLog("4");
            ConsoleHelper.WriteLog("5");
            Process.GetCurrentProcess().WaitForExit();
        }
        static int count = 0;
        static void a()
        {

        }
       static void b()
        {

        }
    }
}
