using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.Framework.Services;
using AS.VW.Scheduler.Cybersource.Auth.Business;
using AS.VW.Scheduler.Cybersource.Auth.ReRun;

namespace AS.VW.Scheduler.Cybersource.Auth
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Running task with syntax: [clientId|methodRun(hour/day)|fromDateTime(yyyy-mm-dd hh:mm:ss)|toDataTime(yyyy-mm-dd hh:mm:ss)]");

                bool needToContinue = false;
                do
                {
                    Console.Write("Enter your parameters: ");
                    
                    string info = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(info))
                    {
                        args = info.Split('|');
                        bool isReRun = args.Length > 3 && args[3].ToLower() == "isrerun";
                        if (isReRun)
                        {
                            CybersouceAuthReRun task = new CybersouceAuthReRun();
                            task.Execute(args);
                        }
                        else
                        {
                            CybersouceAuth task = new CybersouceAuth();
                            task.Execute(args);
                        }
                        Console.Write("Successfully completed.");
                    }

                    Console.WriteLine();
                    Console.Write("Do you want to continue Y/N: ");
                    needToContinue = "Y".Equals(Console.ReadLine(), StringComparison.InvariantCultureIgnoreCase);
                }
                while (needToContinue);
            }
            else
            {
                CybersouceAuth task = new CybersouceAuth();
                task.Execute(args);
            }
        }
    }
}
