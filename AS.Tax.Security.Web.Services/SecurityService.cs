using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Microsoft.Web.Services3.Security.Tokens;
using AS.Common.WebUI;
using AS.Common.DBManager;
using AS.Common.WSE;
using FilterParamWS = AS.Tax.Security.Web.Services.SecService.FilterParameter;
using AS.Tax.Security.Web.Services.Model;

namespace AS.Tax.Security.Web.Services
{
    public class SecurityService : ASReportBusiness, ICryptor
    {
        private readonly SecService.SecurityServices _Service;
        public int ASClientID
        {
            set { _Service.ASClientID = value; }
            get { return _Service.ASClientID; }
        }
        public SecurityService(int asClientID)
        {
            _Service = new SecService.SecurityServices(asClientID);
            string userName = Common.DataProtection.Cryptophy.DecryptText(System.Configuration.ConfigurationManager.AppSettings["TAX_SEC_WS_Token1"]);
            string passWord = Common.DataProtection.Cryptophy.DecryptText(System.Configuration.ConfigurationManager.AppSettings["TAX_SEC_WS_Token2"]);
            var _url = System.Configuration.ConfigurationManager.AppSettings["TAX_SEC_WS_URL"];
            UsernameToken token = new UsernameToken(userName, passWord, PasswordOption.SendPlainText);           
            _Service.Url = _url;
            _Service.SetClientCredential(token);
            _Service.SetPolicy("ClientPolicy");
            this.ReportWS = _Service;
            base.Token1 = userName;
            base.Token2 = passWord;
        }
       
               
        #region Generic Services
        public string EncryptText(string str)
        {
            return _Service.EncryptText(str);
        }
        public string EncryptText(string str, string key)
        {
            return _Service.EncryptTextWithKey(str, key);
        }
        public string DecryptText(string str)
        {
            return _Service.DecryptText(str);
        }
        public string DecryptText(string str, string key)
        {
            return _Service.DecryptTextWithKey(str, key);
        }
        public int GenNum(int min, int max)
        {
            return _Service.GenNum(min, max);
        }
        public string GenPwd()
        {
            return _Service.GenPwd();
        }

        #endregion

        #region JumpSite
        public string CreateJumpSiteTicket(Guid userId, string remoteIP, int desSysId, Guid jumper)
        {
            return _Service.CreateJumpSiteTicket(userId, remoteIP, desSysId, jumper);
        }

        public bool CheckJumpSiteTicket(Guid userId, Guid jumper, string tempPassword, int desSysId, int clientId)
        {
            return _Service.CheckJumpSiteTicket(userId, jumper, tempPassword, HttpContext.Current.Request.UserHostAddress, desSysId, clientId, GetLocalIPs(HttpContext.Current.Server.MachineName), HttpContext.Current.Request.UserAgent);
        }
        string GetLocalIPs(string hostname)
        {

            System.Net.IPHostEntry ips = System.Net.Dns.GetHostEntry(hostname);
            if (ips.AddressList.Length > 0)
            {
                return ips.AddressList[0].ToString();
            }
            return string.Empty;
        }
        #endregion
        

        #region Common
        public DataTable GetReports(string spName, FilterParameterCollection _parameters)
        {
            FilterParamWS[] _params = new FilterParamWS[_parameters.Count];
            for (int i = 0; i < _params.Length; i++)
            {
                _params[i] = new FilterParamWS();
                _params[i].ParamaterName = _parameters[i].ParameterName;
                _params[i].ParamaterValue = _parameters[i].ParameterValue;
                _params[i].ParamaterType = _parameters[i].ParameterType;

            }
            return _Service.GetReports(spName, _params);
        }
        public DataSet GetReportsAsDataSet(string spName, FilterParameterCollection _parameters)
        {
            FilterParamWS[] _params = new FilterParamWS[_parameters.Count];
            for (int i = 0; i < _params.Length; i++)
            {
                _params[i] = new FilterParamWS();
                _params[i].ParamaterName = _parameters[i].ParameterName;
                _params[i].ParamaterValue = _parameters[i].ParameterValue;
                _params[i].ParamaterType = _parameters[i].ParameterType;
                _params[i].IsOutParameter = _parameters[i].IsOutParameter;

            }
            return _Service.GetReportsAsDataSet(spName, _params);
        }

        public int ExecuteNonQueryCommand(string spName, FilterParameterCollection _parameters, out FilterParameterCollection OutputParams)
        {

            FilterParamWS[] _params = ConvertToFilterParamWSArray(_parameters);
            FilterParamWS[] _OutputParams;

            int AffectedRows = _Service.ExecuteNonQueryCommand(spName, _params, out _OutputParams);

            OutputParams = ConvertToCommonFilterParamCollection(_OutputParams);
            return AffectedRows;
        }

        public static FilterParamWS ConvertToFilterParamWS(FilterParameter filterParam)
        {
            if (filterParam == null) return null;
            return new FilterParamWS
            {
                ParamaterName = filterParam.ParameterName,
                ParamaterType = filterParam.ParameterType,
                ParamaterValue = filterParam.ParameterValue,
                IsOutParameter = filterParam.IsOutParameter
            };
        }

        public static FilterParamWS[] ConvertToFilterParamWSArray(FilterParameterCollection filterParams)
        {
            if (filterParams == null) 
                return new FilterParamWS[0];

            List<FilterParamWS> array = new List<FilterParamWS>();
            foreach (FilterParameter param in filterParams)
            {
                array.Add(ConvertToFilterParamWS(param));
            }
            return array.ToArray();
        }

        public static FilterParameter ConvertToCommonFilterParam(FilterParamWS wsFilterParameter)
        {
            if (wsFilterParameter == null) return null;
            return new FilterParameter
            {
                ParameterName = wsFilterParameter.ParamaterName,
                ParameterValue  = wsFilterParameter.ParamaterValue,
                ParameterType = wsFilterParameter.ParamaterType,
                IsOutParameter = wsFilterParameter.IsOutParameter
            };
        }
        public static FilterParameterCollection ConvertToCommonFilterParamCollection(FilterParamWS[] wsParams)
        {
            FilterParameterCollection CommonFilterParams = new FilterParameterCollection();
            
            if (wsParams == null) 
                return CommonFilterParams;  
            
            foreach (FilterParamWS wsParam in wsParams)
            {
                CommonFilterParams.Add(ConvertToCommonFilterParam(wsParam));
            }
            return CommonFilterParams;
        }

        #endregion


        #region Create/Update User

        /// <summary>
        /// Create new User
        /// </summary>
        /// <returns></returns>
        public Guid InsertUser(UserModel user)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            FilterParameterCollection outParams = new FilterParameterCollection();
            outParams.Add("@UserId", string.Empty, DbType.String, true);
            parameters.Add("@UserId", string.Empty, DbType.String, true);            
            parameters.Add("@UserName", user.UserName, DbType.String);
            parameters.Add("@ASClient", user.ClientID, DbType.Int32);
            parameters.Add("@Email", user.Email, DbType.String);
            parameters.Add("@UserNameFirst", user.FirstName, DbType.String);
            parameters.Add("@UserNameLast", user.LastName, DbType.String);
            parameters.Add("@UserPassword", GeneratePassword(), DbType.String);
            parameters.Add("@UserPasswordType", 9, DbType.Int32);
            parameters.Add("@ActiveStatus", user.ActiveStatus, DbType.String);
            parameters.Add("@UserType", user.UserType, DbType.Int32);
            parameters.Add("@HierarchyIds", user.HierarchyIDs, DbType.String);
            parameters.Add("@CreatedBy", user.CreatedBy, DbType.Guid);
            parameters.Add(new FilterParameter("@ThemeIdNew", user.ThemeID, DbType.Int32));
            ExecuteNonQueryCommand("spa_SEC_CreateASUser", parameters, out outParams);
            Guid userRecID = new Guid(outParams[0].ParameterValue.ToString());
            return userRecID;
        }

        string GeneratePassword()
        {
            return AS.Common.DataProtection.Cryptophy.SHA1(DateTime.Now.Ticks.ToString());
        }

        /// <summary>
        /// Update Excluded Permissions For User
        /// </summary>
        /// <param name="userRecID"></param>
        /// <param name="excludedPermissions"></param>
        public void UpdateExcludedPermissionsForUser(int clientID, Guid userRecID, string excludedPermissions)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            FilterParameterCollection outParam;
            parameters.Add("@ASClient", clientID, DbType.Int32);
            parameters.Add("@UserRecID", userRecID, DbType.Guid);
            parameters.Add("@PermissionIds", excludedPermissions, DbType.String);
            ExecuteNonQueryCommand("spa_SEC_UpdateExcludedPermissionsForUser", parameters, out outParam);
        }


        /// <summary>
        /// Update user
        /// </summary>
        public void UpdateUser(UserModel user, int system)
        {
            FilterParameterCollection _params = new FilterParameterCollection();
            FilterParameterCollection _outparams;
            _params.Add(new FilterParameter("@ASClient", user.ClientID, DbType.Int32));
            _params.Add(new FilterParameter("@SystemID", system, DbType.Int32));
            _params.Add(new FilterParameter("@UserName", user.UserName, DbType.AnsiString));
            _params.Add(new FilterParameter("@UserNameFirst", user.FirstName, DbType.AnsiString));
            _params.Add(new FilterParameter("@UserNameLast", user.LastName, DbType.AnsiString));
            _params.Add(new FilterParameter("@UserNameFull", user.FirstName + " " + user.LastName, DbType.AnsiString));
            _params.Add(new FilterParameter("@Email", user.Email, DbType.AnsiString));
            if (user.Question.HasValue)
            {
                _params.Add(new FilterParameter("@LoginQuestionIndex", user.Question, DbType.Int32));
                _params.Add(new FilterParameter("@LoginQuestionAnswer", user.Answer, DbType.AnsiString));
            }
            _params.Add(new FilterParameter("@ActvStatus", user.ActiveStatus, DbType.AnsiString));
            _params.Add(new FilterParameter("@UserType", user.UserType, DbType.Int32));
            _params.Add(new FilterParameter("@HierarchyIds", user.HierarchyIDs, DbType.AnsiString));
            _params.Add(new FilterParameter("@IsUpdateExcludeFunction", false, DbType.Boolean));
            ExecuteNonQueryCommand("spa_SEC_UpdateASUser", _params, out _outparams);
        }
        public SecService.User GetUser(int clientId, string userName)
        {
            return _Service.GetUser(clientId, userName);
        }
        #endregion

        #region Get Theme
        public DataTable GetThemeInfo(int clientID)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add("@ASClient", clientID, DbType.Int32);
            return GetReports("spa_SEC_GetThemeOfClient", parameters);
        }
        public DataTable GetThemeInfoByUser(int clientID, string UserName)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add("@ASClient", clientID, DbType.Int32);
            parameters.Add("@UserName", UserName, DbType.AnsiString);
            parameters.Add("@HierarchyId", 0, DbType.Int32);
            return GetReports("spa_SEC_GetThemeOfUser", parameters);
        }
        public void SynchPermission(int clientID, string username, int hierarchyID, bool isViewFullTIN)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            FilterParameterCollection outParam;
            parameters.Add("@ASClientID", clientID, DbType.Int32);
            parameters.Add("@UserName", username, DbType.String);
            parameters.Add("@HiearchyID", hierarchyID, DbType.Int32);
            parameters.Add("@IsViewFullTIN", isViewFullTIN, DbType.Boolean);
            this.ExecuteNonQueryCommand("spa_SEC_SynchUserFromVisionWeb", parameters, out outParam);
        }
        public SecService.Hierarchy GetHierarchyOfUser(int clientID, string userName)
        {
            AS.Tax.Security.Web.Services.SecService.Hierarchy[] hierarchies= _Service.GetHierarchiesOfUser(clientID, userName, 0);
            if (hierarchies == null || hierarchies.Length <= 0) return null;
            return hierarchies[0];
        }
        #endregion
    }
}
