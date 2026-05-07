using AS.VW.Scheduler.Tasks;

namespace AS.VWSheduler.Tasks.UnitTestConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
            ExportReports t = new ExportReports();
            t.DoGenerateFiles();
        }
    }
}
