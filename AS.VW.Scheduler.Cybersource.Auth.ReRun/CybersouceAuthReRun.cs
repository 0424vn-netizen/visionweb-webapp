using AS.VW.Repository;
using AS.VW.Task.Common;
using System;

namespace AS.VW.Scheduler.Cybersource.Auth.ReRun
{
    public class CybersouceAuthReRun : VisionWebTask
    {
        protected override string TaskName => "CybersouceAuthReRun";

        protected override void Run(string[] parameters = null)
        {
            if (parameters != null && parameters.Length > 0)
            {
                string[] args = { parameters[0], "day", parameters[2], "IsReRun" };
                CybersouceAuth task = new CybersouceAuth();
                task.Execute(args);
            }
            else 
            {
                string clients = AppConfigurations.GetStringAppSettings("Clients");
                var selectedClients = clients.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var client in selectedClients)
                {
                    string[] args = { client, "day", DateTime.Now.ToShortDateString(), "IsReRun" };
                    CybersouceAuth task = new CybersouceAuth();
                    task.Execute(args);
                }
            }

            TaskResult.IsSuccess = true;
        }

    }
}
