using AS.Common.DBManager;
using AS.Common.Logger;
using AS.Framework.Services;
using AS.VW.Repository;
using AS.Web.Business;
using AS.WS.Business;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Task.Common
{
    public abstract class VisionWebTask: BaseTask
    {
        protected readonly ILog TaskLogger = LoggerManager.GetLogger("VisionWebTaskLogger");
        public TaskResult TaskResult { get; set; }
        protected abstract string TaskName { get; }
        protected abstract void Run(string[] parameters = null);
        public override void Execute()
        {
            TaskResult = new TaskResult();
            TaskLogger.Info(string.Format("Task [{0}] started!", TaskName));
            try
            {
                TaskResult.ElapsedTime = TaskUtility.ExecuteWithWatch(() => {
                    Run();
                });
            }
            catch (Exception ex)
            {
                TaskResult.Exception = ex;
                TaskLogger.Error(ex.Message, ex);
                EmailHelper.SendEmailAlert(TaskName, ex.StackTrace);
            }
            finally
            {
                TaskLogger.Info($"Task [{TaskName}] done in {TaskResult.ElapsedTime}: {(TaskResult.IsSuccess ? "success" : "fail")}. {TaskResult.Message ?? ""}{Environment.NewLine}");
            }
        }
        public virtual void Execute(string[] parameters)
        {
            TaskResult = new TaskResult();
            TaskLogger.Info(string.Format("Task [{0}] started! Params: {1}", TaskName, string.Join(" - ", parameters)));
            try
            {
                TaskResult.ElapsedTime = TaskUtility.ExecuteWithWatch(() => {
                    Run(parameters);
                });
            }
            catch (Exception ex)
            {
                TaskResult.Exception = ex;
                TaskLogger.Error(ex.Message, ex);
                EmailHelper.SendEmailAlert(TaskName, ex.StackTrace);
            }
            finally
            {
                TaskLogger.Info($"Task [{TaskName}] done in {TaskResult.ElapsedTime}: {(TaskResult.IsSuccess ? "success" : "fail")}. {TaskResult.Message ?? ""}{Environment.NewLine}");
            }
        }
        public ReportingBusiness GetDataAcess(int clientId)
        {
            var connection = TaskUtility.GetConnection(clientId);
            if (!string.IsNullOrEmpty(connection))
            {
                var timeout = AppConfigurations.GetIntAppSettings("SqlCommandTimeout", 0);

                if (timeout > 0)
                {
                    ASSqlDatabase.CommandTimeout = timeout;
                }

                ReportingBusiness reportingBusiness = new ReportingBusiness();
                reportingBusiness.InitializeForCS(connection);
                return reportingBusiness;
            }

            return null;
        }
        public ReportServices GetService(int clientId)
        {
            var url = AppConfigurations.GetStringAppSettings("WS_URL");
            var token1 = AppConfigurations.GetStringAppSettings("WS_Token1");
            var token2 = AppConfigurations.GetStringAppSettings("WS_Token2");
            var service = new ReportServices(url, token1, token2);
            service.AddRequestHeader("ClientId", clientId.ToString());
            return service;
        }
    }
}
