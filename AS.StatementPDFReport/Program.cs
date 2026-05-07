using AS.Common.Logger;
using System;
using System.IO;
using System.Windows.Forms;

namespace AS.StatementPDFReport
{
    static class Program
    {
        const string NO_PASSWORD = "NoPassword";

        static void Main(string[] args)
        {
            System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            string tmpTable, password, exportPath = string.Empty;

            if (args.Length != 0)
            {
                tmpTable = args[0];
                if(args.Length >= 2)
                {
                    password = args[1].Equals(NO_PASSWORD) ? string.Empty : args[1];
                }
                else
                {
                    password = string.Empty;
                }
                exportPath = args.Length >= 3 ? args[2] : Path.GetDirectoryName(Application.ExecutablePath);
                     
                ProcessStatement statement = new ProcessStatement();
                statement.Start(tmpTable, password, exportPath);
            }
            else
            {
                LoggerManager.Debug("\n\n----- Exception while exporting! Arguments are empty -----");
            }

            Application.Exit();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LoggerManager.Debug("\n\n----- Exception while exporting: " + e.ExceptionObject.ToString());
            Application.Exit();
        }
    }
}
