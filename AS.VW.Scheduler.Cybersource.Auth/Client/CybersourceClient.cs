using AS.Core.Common.Log;
using AS.VW.Repository;
using AS.VW.Scheduler.Cybersource.Auth.Business;
using AS.VW.Scheduler.Cybersource.Auth.Model;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace AS.VW.Scheduler.Cybersource.Auth.Client
{
    public class CybersourceClient : RestClient
    {        
        private readonly bool serverCertificateValidation = AppConfigurations.GetBoolAppSettings("ServerCertificateValidation", true);
        private readonly bool LogRequest = AppConfigurations.GetBoolAppSettings("LogRequest");
        private readonly ILog LogManager;
        private ClientConfig ApiConfig { get; set; }        
        public CybersourceClient(ClientConfig config, ILog log)
        {            
            BaseUrl = new Uri(config.BaseApiUrl);
            ApiConfig = config;
            LogManager = log;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return serverCertificateValidation; };     
        }
        public IRestRequest CreateRequest(string resource, Method method)
        {
            IRestRequest request = new RestRequest(resource, method);
            request.AddHeader("Content-Type", "application/json");           
            return request;
        }       
        public override IRestResponse<T> Execute<T>(IRestRequest request)
        {
            BeforeRequest(request);
            IRestResponse<T> response = default;
            try
            {
                response = base.Execute<T>(request);
            }
            catch (Exception ex)
            {
                LogManager.Error("ERROR Cybersource Api", ex);
            }

            AfterRequest(response);
            return response;
        }
        public override IRestResponse Execute(IRestRequest request)
        {
            BeforeRequest(request);
            IRestResponse response = default;
            try
            {
                response = base.Execute(request);
            }
            catch (Exception ex)
            {
                LogManager.Error("ERROR Cybersource Api", ex);
            }

            AfterRequest(response);
            return response;
        }
        private void BeforeRequest(IRestRequest request)
        {
            var requestData = JsonConvert.SerializeObject(request);
            WriteRequest(Utils.MaskSensitiveData(requestData), true);
        }
        private void AfterRequest(IRestResponse response)
        {
            var responseData = Utils.MaskSensitiveData(response.Content);
            WriteRequest(Utils.MaskSensitiveData(responseData), false);
        }
        private void WriteRequest(string content, bool isRequest)
        {
            if (LogRequest)
            {
                var fileName = $"{(isRequest ? "Request_" : "Response_")}{DateTime.Now.ToString("yyyyMMddHHmmss")}_{Guid.NewGuid()}";
                var outputFolder = GetRequestFolder();
                var filePath = Path.Combine(outputFolder, fileName);
                File.WriteAllText(filePath, content);
            }           
        }
        private string GetRequestFolder()
        {            
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string outputName = "/App_Data/Output/Request";
            var outputFolder = Path.GetFullPath($"{rootPath}{outputName}");
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            return outputFolder;
        }
    }
}
