using AS.Common.DBManager;
using AS.VW.Repository;
using AS.VW.Scheduler.Cybersource.Auth.Business;
using AS.VW.Scheduler.Cybersource.Auth.Model;
using AS.VW.Task.Common;
using AS.Web.Business;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;

namespace AS.VW.Scheduler.Cybersource.Auth
{
    public class CybersouceAuth : VisionWebTask
    {
        private readonly bool IsMock = AppConfigurations.GetBoolAppSettings("IsMock");
        private readonly bool IsDebug = AppConfigurations.GetBoolAppSettings("IsDebug");
        private readonly string ClientConfig = AppConfigurations.GetStringAppSettings("Clients", string.Empty);
        private readonly int NumberDayRun = AppConfigurations.GetIntAppSettings("NumberDayRun", 1);

        protected override string TaskName => "CybersouceAuth";

        protected override void Run(string[] parameters = null)
        {
            var allConfig = GetClientConfig();
            var clientconfigs = new List<ClientConfig>();

            if (parameters != null && parameters.Length > 0)
            {
                clientconfigs = GetClientConfigWithParam(parameters, allConfig);
            }
            else
            {
                if (!string.IsNullOrEmpty(ClientConfig))
                {
                    var selectedClients = ClientConfig.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
                    foreach (var client in selectedClients)
                    {
                        var item = allConfig.FirstOrDefault(x => x.ClientId == client);
                        if (item != null)
                        {
                            item.IsNormal = true;
                            clientconfigs.Add(item);
                        }
                    }
                }
            }

            foreach (var client in clientconfigs)
            {
                GetAuthData(client);
            }

            TaskResult.IsSuccess = true;
        }

        private List<ClientConfig> GetClientConfigWithParam(string[] parameters, List<ClientConfig> allConfig)
        {
            var clientconfigs = new List<ClientConfig>();
            var client = allConfig.FirstOrDefault(x => x.ClientId == int.Parse(parameters[0]));
            string methodRun = parameters[1].ToLower();

            if (client != null)
            {
                switch (methodRun)
                {
                    case "hour":
                        long startDate = DateTimeOffset.Parse(parameters[2]).ToUnixTimeMilliseconds();
                        long endDate = DateTimeOffset.Parse(parameters[3]).ToUnixTimeMilliseconds();

                        client.Query = $"submitTimeUtc:[{startDate} TO {endDate}]";
                        client.ReportDate = DateTime.Parse(parameters[2]);
                        client.ReportDateRun = DateTime.Parse(parameters[2]);
                        client.TransactionDate = DateTime.Parse(parameters[2]);
                        client.IsRunByHour = true;
                        clientconfigs.Add(client);
                        break;
                    case "day":
                        DateTime dateParam = DateTime.Parse(parameters[2]);
                        for (var i = 0; i < NumberDayRun; i++)
                        {
                            DateTime dateParamRun = dateParam.AddDays(-i);
                            List<HourConfig> hours = Get24Hours();
                            List<AuthLogModel> logs = new List<AuthLogModel>();
                            bool isReRun = parameters.Length > 3 && parameters[3].ToLower() == "isrerun";
                            if (isReRun)
                            {
                                logs = GetLogFromDB(int.Parse(parameters[0]), dateParamRun.ToShortDateString(), client.PrefixFileName);
                            }

                            foreach (var item in hours)
                            {
                                DateTime dateRun = dateParamRun;
                                if (item.IsPreviousDate)
                                    dateRun = dateRun.AddDays(-1);
                                DateTime startDateTemp = new DateTime(dateRun.Year, dateRun.Month, dateRun.Day, int.Parse(item.Hour), 0, 0);
                                DateTime endDateTemp = new DateTime(dateRun.Year, dateRun.Month, dateRun.Day, int.Parse(item.Hour), 59, 59);
                                startDate = DateTimeOffset.Parse(startDateTemp.ToString()).ToUnixTimeMilliseconds();
                                endDate = DateTimeOffset.Parse(endDateTemp.ToString()).ToUnixTimeMilliseconds();

                                client.Query = $"submitTimeUtc:[{startDate} TO {endDate}]";
                                client.ReportDate = dateParamRun;
                                client.ReportDateRun = startDateTemp;
                                client.TransactionDate = dateParamRun;
                                client.IsReRun = isReRun;
                                client.AuthLogs = logs;
                                clientconfigs.Add((ClientConfig)client.Clone());
                            }
                        }
                        break;
                }
            }

            return clientconfigs;
        }
        private List<HourConfig> Get24Hours()
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string conectionPath = "/App_Data/Config24Hours.xml";
            var filePath = Path.GetFullPath($"{rootPath}{conectionPath}");

            if (!File.Exists(filePath))
                return new List<HourConfig>();

            string items = "//Items/ItemNormal/Item";
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList list = doc.SelectNodes(items);
            List<HourConfig> colList = new List<HourConfig>();
            foreach (XmlNode node in list)
            {
                string value = node.Attributes["Value"].Value;
                string isPreviousDate = node.Attributes["IsPreviousDate"] != null ? node.Attributes["IsPreviousDate"].Value : string.Empty;
                colList.Add(new HourConfig { Hour = value, IsPreviousDate = isPreviousDate == "true" });
            }

            return colList;
        }

        private void GetAuthData(ClientConfig client)
        {
            var service = GetService(client.ClientId);
            var credential = GetCredentials(service, client.ClientId, client.CredentialPath);

            if (credential == null)
            {
                var message = $"Cannot find credential for client: {client.ClientId}";
                throw new ArgumentNullException(message, new Exception(message));
            }

            client.AuthorizationInfo = credential;
            AuthBusiness business = new AuthBusiness(service, client, IsMock, IsDebug);
            business.ProcessAuthData();
        }

        private List<ClientConfig> GetClientConfig()
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string conectionPath = "/App_Data/ClientConfig.json";
            var filePath = Path.GetFullPath($"{rootPath}{conectionPath}");
            return TaskUtility.GetJsonConfiguration<ClientConfig>(filePath);
        }
        private AuthorizationInfo GetCredentials(ReportServices service, int clientId, string filePath)
        {
            var configs = new List<AuthorizationInfo>();

            if (string.IsNullOrEmpty(filePath))
            {
                var credentialString = GetCredentialsFromDatabase(service, clientId);

                if (!string.IsNullOrEmpty(credentialString))
                    return JsonConvert.DeserializeObject<AuthorizationInfo>(credentialString);
            }
            else
            {
                if (!File.Exists(filePath))
                    return null;

                configs = TaskUtility.GetJsonConfiguration<AuthorizationInfo>(filePath);
            }

            return configs.FirstOrDefault(x => x.ClientId == clientId);
        }
        private string GetCredentialsFromDatabase(ReportServices service, int clientId)
        {
            FilterParameterCollection inputParam = new FilterParameterCollection
            {
                new FilterParameter("@ASClientID", clientId, DbType.Int32),
            };

            var data = service.GetReports("spa_API_AUTH_Get_Credentials", inputParam);
            if (data != null && data.Rows.Count > 0)
            {
                var encryptedText = data.Rows[0]["CredentialString"].ToString();
                return service.DecryptText(encryptedText, clientId);
            }

            return string.Empty;
        }

        private List<AuthLogModel> GetLogFromDB(int clientId, string reportDate, string prefixFileName)
        {
            var service = GetService(clientId);
            List<AuthLogModel> authLog = new List<AuthLogModel>();
            FilterParameterCollection inputParam = new FilterParameterCollection
            {
                new FilterParameter("@ASClientID", clientId, DbType.Int32),
                new FilterParameter("@ReportDate", reportDate, DbType.Date),
            };

            var data = service.GetReports("spa_cs_GetLog_MerchantAuthorization_API", inputParam);
            if (data != null && data.Rows.Count > 0)
            {
                for (var i = 0; i < data.Rows.Count; i++)
                {
                    string originalFileName = data.Rows[i]["OriginalFileName"].ToString();
                    string fileName = originalFileName.Substring(0, originalFileName.Length - 2);
                    var itemIndex = authLog.FindIndex(x => x.OriginalFileName == fileName);
                    if (itemIndex != -1)
                    {
                        bool isPreviousValid = authLog[itemIndex].IsValid;
                        authLog[itemIndex].TransactionCount += int.Parse(data.Rows[i]["TransactionCount"].ToString());
                        authLog[itemIndex].IsValid = !isPreviousValid ? isPreviousValid :  bool.Parse(data.Rows[i]["IsValid"].ToString());

                    }
                    else
                    {
                        authLog.Add(new AuthLogModel
                        {
                            OriginalFileName = fileName,
                            TransactionDate = DateTime.Parse(data.Rows[i]["TransactionDate"].ToString()),
                            ReportDate = GetReportDateFromFileName(fileName, prefixFileName),
                            TransactionCount = int.Parse(data.Rows[i]["TransactionCount"].ToString()),
                            IsValid = bool.Parse(data.Rows[i]["IsValid"].ToString()),
                        });
                    }
                }
            }

            return authLog;
        }

        private DateTime GetReportDateFromFileName(string fileName, string prefixFileName)
        {
            string[] preName = prefixFileName.Split('_');
            string[] data = fileName.Split('_');
            string date = data[preName.Length - 1];
            return new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(4, 2))
                , int.Parse(date.Substring(6, 2)), int.Parse(date.Substring(8, 2)), 0, 0);
        }
    }
}
