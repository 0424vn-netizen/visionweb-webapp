using AS.Common.DataProtection;
using AS.Common.DBManager;
using AS.Security.WS.Entities.Utility;
using AS.Web.Business.Logger;
using AS.Web.Business.Shared.Constants;
using AS.Web.Business.Shared.Enums;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using PciFilterParameter = AS.Web.Business.PciWebServices.FilterParameter;

namespace AS.Web.Business.PCI
{
    public class PciServices : Common.WebUI.ASReportBusiness, IPciServices
    {
        #region ctor
        private readonly PciWebServices.PCIServices _pciWebServices;
        public PciServices(string url, string token1, string token2, bool isSecurityProtocol = true)
        {
            _pciWebServices = new PciWebServices.PCIServices();
            string userName = Cryptophy.DecryptText(token1);
            string passWord = Cryptophy.DecryptText(token2);
            UsernameToken token = new UsernameToken(userName, passWord, PasswordOption.SendPlainText);
            _pciWebServices.Url = url;
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings[PciConstants.WebServices_ServiceTimeout], out int timeoutMinutes);
            timeoutMinutes = timeoutMinutes > 0 ? timeoutMinutes : 5;
            _pciWebServices.Timeout = timeoutMinutes * 60000;
            _pciWebServices.SetClientCredential(token);
            _pciWebServices.SetPolicy(PciConstants.WebServices_ClientPolicy);
            if (isSecurityProtocol)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
            base.ReportWS = _pciWebServices;
            base.Token1 = userName;
            base.Token2 = passWord;
            PciLoggerManager.Info(string.Format("Init PciServices PCI MT: Url={0}", url));
        }
        #endregion
        #region Public Method
        public DataTable GetReports(string spName, FilterParameterCollection _parameters)
        {
            try
            {
                PciFilterParameter[] _params = new PciFilterParameter[_parameters.Count];
                for (int i = 0; i < _params.Length; i++)
                {
                    _params[i] = new PciFilterParameter
                    {
                        ParameterName = _parameters[i].ParameterName,
                        ParameterValue = _parameters[i].ParameterValue,
                        ParameterType = _parameters[i].ParameterType
                    };
                }
                return _pciWebServices.GetReports(spName, _params);
            }
            catch (Exception ex)
            {
                var isIgnore = IsIgnoreException();
                LogHepler.WriteLogException(GetMessageToRequestUrl("GetReports", isIgnore), spName, _parameters, ex);
                if (isIgnore) return null;
                else throw;
            }
        }
        public int ExecuteNonQueryCommand(string spName, FilterParameterCollection _parameters, out FilterParameterCollection OutputParams)
        {
            try
            {
                PciFilterParameter[] _params = ConvertToPciFilterParameterArray(_parameters);
                int AffectedRows = _pciWebServices.ExecuteNonQueryCommand(spName, _params, out PciFilterParameter[] _OutputParams);
                OutputParams = ConvertToCommonFilterParamCollection(_OutputParams);
                return AffectedRows;
            }
            catch (Exception ex)
            {
                var isIgnore = IsIgnoreException();
                LogHepler.WriteLogException(GetMessageToRequestUrl("ExecuteNonQueryCommand", isIgnore), spName, _parameters, ex);
                if (isIgnore)
                {
                    OutputParams = new FilterParameterCollection
                    {
                        new FilterParameter("@Result", 0, DbType.Boolean, true)
                    };
                    return -1;
                }
                else throw;
            }
        }

        public string EncryptText(string encryptStr)
        {
            return _pciWebServices.EncryptText(encryptStr);
        }
        public string DecryptText(string decryptStr)
        {
            return _pciWebServices.DecryptText(decryptStr);
        }
        #endregion

        #region public Request Header
        public void AddRequestHeader(string name, string value)
        {
            _pciWebServices.AddRequestHeader(name, value);
        }
        public void AddRequestHeaders(Dictionary<string, string> dictHeader)
        {
            _pciWebServices.AddRequestHeaders(dictHeader);
        }
        public void InsertOrUpdateRequestHeader(string name, string value)
        {
            _pciWebServices.InsertOrUpdateRequestHeader(name, value);
        }
        #endregion

        #region Private function
        private static PciFilterParameter[] ConvertToPciFilterParameterArray(FilterParameterCollection filterParams)
        {
            if (filterParams == null)
                return new PciFilterParameter[0];
            List<PciFilterParameter> array = new List<PciFilterParameter>();
            foreach (FilterParameter param in filterParams)
            {
                array.Add(ConvertToPciFilterParameter(param));
            }
            return array.ToArray();
        }
        private static PciFilterParameter ConvertToPciFilterParameter(FilterParameter filterParam)
        {
            if (filterParam == null)
                return null;
            return new PciFilterParameter
            {
                ParameterName = filterParam.ParameterName,
                ParameterType = filterParam.ParameterType,
                ParameterValue = filterParam.ParameterValue,
                IsOutParameter = filterParam.IsOutParameter
            };
        }
        private static FilterParameterCollection ConvertToCommonFilterParamCollection(PciFilterParameter[] wsParams)
        {
            if (wsParams == null)
                return new FilterParameterCollection();
            FilterParameterCollection CommonFilterParams = new FilterParameterCollection();
            FilterParameter CommonParam = null;
            foreach (PciFilterParameter wsParam in wsParams)
            {
                CommonParam = ConvertToCommonFilterParam(wsParam);
                CommonFilterParams.Add(CommonParam);
            }
            return CommonFilterParams;
        }
        private static FilterParameter ConvertToCommonFilterParam(PciFilterParameter wsFilterParameter)
        {
            if (wsFilterParameter == null)
                return null;
            return new FilterParameter
            {
                ParameterName = wsFilterParameter.ParameterName,
                ParameterValue = wsFilterParameter.ParameterValue,
                ParameterType = wsFilterParameter.ParameterType,
                IsOutParameter = wsFilterParameter.IsOutParameter
            };
        }
        private static bool IsIgnoreException()
        {
            var enviroment = System.Configuration.ConfigurationManager.AppSettings[PciConstants.WebServices_PCI_Enviroment];
            var arrIgnores = new List<string>()
            {
                    EnumEnviroment.NONE.ToString(),
                    EnumEnviroment.DEV.ToString(),
                    EnumEnviroment.INT.ToString(),
                    EnumEnviroment.VNQA.ToString()
            };
            return arrIgnores.Any(x => x.Equals(enviroment, StringComparison.OrdinalIgnoreCase));
        }
        private string GetMessageToRequestUrl(string method, bool isIgnore)
        {
            string msgWithUrl = GetMessageToRequestUrl();
            string msgIgnore = isIgnore ? " - IgnoreException=True" : "";
            return string.Format("{0}:{1}{2}", method, msgWithUrl, msgIgnore);
        }
        private string GetMessageToRequestUrl()
        {
            var requestUrl = HttpContext.Current?.Request?.CurrentExecutionFilePath;
            return string.Format("{0} Page call service PCI MT", requestUrl);
        }
        #endregion
    }
}
