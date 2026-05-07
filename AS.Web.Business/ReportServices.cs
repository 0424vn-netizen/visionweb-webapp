using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.Services3.Security.Tokens;
using AS.Common.DataProtection;
using System.Data;

using FilterParamWS = AS.Web.Business.ReportService.FilterParameter;
using ExportInfoWS = AS.Web.Business.ReportService.ExportInfo;
using AS.Common.DBManager;
using AS.Common.WSE;
using AS.Common.WebUI;
using AS.WS.Entities;
using System.Threading.Tasks;
using AS.Common.Logger;
using AS.Security.WS.Entities.Utility;

namespace AS.Web.Business
{
    public class ReportServices : ASReportBusiness, IReportServices
    {
        readonly ReportService.BaseService _ReportingWS = new ReportService.BaseService();
        public int ClientId
        {
            get;
            set;
        }

        public ReportServices(string url, string token1, string token2)
        {
            string userName = Cryptophy.DecryptText(token1);
            string passWord = Cryptophy.DecryptText(token2);
            UsernameToken token = new UsernameToken(userName, passWord, PasswordOption.SendPlainText);
            _ReportingWS.Url = url;
            try
            {
                _ReportingWS.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ServiceTimeout"]) * 60000;
            }
            catch
            {
                _ReportingWS.Timeout = 5 * 60000;
            }
            _ReportingWS.SetClientCredential(token);
            _ReportingWS.SetPolicy("ClientPolicy");
            base.ReportWS = _ReportingWS;
            base.Token1 = userName;
            base.Token2 = passWord;
        }

        public string GetHashData(string plainText)
        {
            var result = _ReportingWS.GetHashData(plainText);
            return result;
        }

        public DataTable GetReports(string spName, FilterParameterCollection _parameters)
        {
            try
            {
                FilterParamWS[] _params = new FilterParamWS[_parameters.Count];            
                for (int i = 0; i < _params.Length; i++)
                {
                    _params[i] = new FilterParamWS();
                    _params[i].ParameterName = _parameters[i].ParameterName;
                    _params[i].ParameterValue = _parameters[i].ParameterValue;
                    _params[i].ParameterType = _parameters[i].ParameterType;

                }
                return _ReportingWS.GetReports(spName, _params);
            }
            catch (Exception ex)
            {
                LogHepler.WriteLogException("GetReports", spName, _parameters, ex);
                throw;
            }
        }

        public DataSet GetReportsAsDataSet(string spName, FilterParameterCollection _parameters)
        {
            FilterParamWS[] _params = new FilterParamWS[_parameters.Count];
            try
            {
                for (int i = 0; i < _params.Length; i++)
                {
                    _params[i] = new FilterParamWS();
                    _params[i].ParameterName = _parameters[i].ParameterName;
                    _params[i].ParameterValue = _parameters[i].ParameterValue;
                    _params[i].ParameterType = _parameters[i].ParameterType;
                    _params[i].IsOutParameter = _parameters[i].IsOutParameter;

                }
                return _ReportingWS.GetReportsAsDataSet(spName, _params);
            }
            catch (Exception ex)
            {
                LogHepler.WriteLogException("GetReportsAsDataSet", spName, _parameters, ex);
                throw;
            }
        }

        public DataSet GetReportsAsDataSet(string[] _spName, FilterParameterCollection[] __parameters, string[] _tableNames)
        {
            List<Task> tasks = new List<Task>();
            var result = new DataSet();
            try
            {
                for (int j = 0; j < _spName.Length; j++)
                {
                    var spName = _spName[j];
                    var tableNames = _tableNames[j];

                    var _parameters = __parameters[j];
                    FilterParamWS[] _params = new FilterParamWS[_parameters.Count];
                    for (int i = 0; i < _params.Length; i++)
                    {
                        _params[i] = new FilterParamWS();
                        _params[i].ParameterName = _parameters[i].ParameterName;
                        _params[i].ParameterValue = _parameters[i].ParameterValue;
                        _params[i].ParameterType = _parameters[i].ParameterType;
                        _params[i].IsOutParameter = _parameters[i].IsOutParameter;

                    }

                    tasks.Add(Task.Run(() =>
                    {
                        try
                        {
                            var _result = _ReportingWS.GetReports(spName, _params);
                            if (_result != null)
                            {
                                _result.TableName = tableNames;
                                result.Tables.Add(_result);
                            }
                            else
                            {
                                result.Tables.Add(new DataTable(tableNames));
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHepler.WriteLogException("GetReportsAsDataSet", spName, _parameters, ex);
                            throw;
                        }

                    }));
                }
                Task.WhenAll(tasks.ToArray()).Wait();
            }
            catch (Exception ex)
            {
                LogHepler.WriteLogException("GetReportsAsDataSet", string.Join(", ", _spName), null, ex);
                throw;
            }
            return result;
        }

        public void ExportFlatReport(string spaName, FilterParameterCollection paramters, string fileName, string resources, string currencyFormat, bool hasRQColumn, string columnsCustomview)
        {
            try
            {
                FilterParamWS[] _params = ConvertToFilterParamWSArray(paramters);
                _ReportingWS.ExportFlatReport(spaName, _params, fileName, resources, currencyFormat, hasRQColumn, columnsCustomview);
            }
            catch (Exception ex)
            {
                LogHepler.WriteLogException("ExportFlatReport", spaName, paramters, ex);
                throw;
            }
        }

        public int ExecuteNonQueryCommand(string spName, FilterParameterCollection _parameters, out FilterParameterCollection OutputParams)
        {
            try
            {
                FilterParamWS[] _params = ConvertToFilterParamWSArray(_parameters);
                FilterParamWS[] _OutputParams;

                int AffectedRows = _ReportingWS.ExecuteNonQueryCommand(spName, _params, out _OutputParams);

                OutputParams = ConvertToCommonFilterParamCollection(_OutputParams);

                return AffectedRows;
            }
            catch (Exception ex)
            {
                LogHepler.WriteLogException("ExecuteNonQueryCommand", spName, _parameters, ex);
                throw;
            }
        }

        public static FilterParamWS ConvertToFilterParamWS(FilterParameter filterParam)
        {
            if (filterParam == null)
                return null;
            return new FilterParamWS
            {
                ParameterName = filterParam.ParameterName,
                ParameterType = filterParam.ParameterType,
                ParameterValue = filterParam.ParameterValue,
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

        public static FilterParameterCollection ConvertToCommonFilterParamCollection(FilterParamWS[] wsParams)
        {
            if (wsParams == null)
                return new FilterParameterCollection();
            FilterParameterCollection CommonFilterParams = new FilterParameterCollection();
            FilterParameter CommonParam = null;
            foreach (FilterParamWS wsParam in wsParams)
            {
                CommonParam = ConvertToCommonFilterParam(wsParam);
                CommonFilterParams.Add(CommonParam);
            }
            return CommonFilterParams;
        }

        public string EncryptText(string encryptStr, int clientID)
        {
            return _ReportingWS.EncryptText(encryptStr, clientID);
        }

        public string DecryptText(string decryptStr, int clientID)
        {
            return _ReportingWS.DecryptText(decryptStr, clientID);
        }

        public string EncryptTextWithMultiKey(string plainText, int clientID)
        {
            return _ReportingWS.EncryptTextWithMultiKey(plainText, clientID);
        }

        public void AddRequestHeader(string name, string value)
        {
            _ReportingWS.AddRequestHeader(name, value);
        }

        #region Escalation Status

        public int UpdateEscalationStatus(FilterParameterCollection parameters, int statusID, string description, int isActive)
        {
            return UpdateEscalationStatus(parameters, statusID, description, isActive, false);
        }

        public int UpdateEscalationStatus(FilterParameterCollection parameters, int statusID, string description, int isActive, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@StatusID", statusID, DbType.Int32));
            parameters.Add(new FilterParameter("@Description", description, DbType.String));
            parameters.Add(new FilterParameter("@IsActive", isActive, DbType.Int32));
            parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
            FilterParameterCollection parameterOut;
            string spa = isMCFRisk ? "spa_RM_MCF_UpdateEscalationStatus" : "spa_rm_cs_UpdateEscalationStatus";
            ExecuteNonQueryCommand(spa, parameters, out parameterOut);

            return Convert.ToInt32(parameterOut[0].ParameterValue);
        }

        public DataTable GetEscalationStatus(FilterParameterCollection parameters, int filterStatus, string sortExpression)
        {
            return GetEscalationStatus(parameters, filterStatus, sortExpression, false);
        }

        public DataTable GetEscalationStatus(FilterParameterCollection parameters, int filterStatus, string sortExpression, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@IsActive", filterStatus, DbType.Int32));
            parameters.Add(new FilterParameter("@SortOrder", sortExpression, DbType.String));
            string spa = isMCFRisk ? "spa_RM_MCF_GetEscalationStatus" : "spa_rm_cs_GetEscalationStatus";
            return GetReports(spa, parameters);
        }

        public int AddEscalationStatus(FilterParameterCollection parameters, string status)
        {
            return AddEscalationStatus(parameters, status, false);
        }

        public int AddEscalationStatus(FilterParameterCollection parameters, string status, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@Description", status, DbType.String));
            parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
            FilterParameterCollection parameterOut;
            string spa = isMCFRisk ? "spa_RM_MCF_AddEscalationStatus" : "spa_rm_cs_AddEscalationStatus";
            ExecuteNonQueryCommand(spa, parameters, out parameterOut);
            return Convert.ToInt32(parameterOut[0].ParameterValue);
        }

        public int ActiveDeactiveEscalationStatus(FilterParameterCollection parameters, int statusId, int isActive)
        {
            return ActiveDeactiveEscalationStatus(parameters, statusId, isActive, false);
        }

        public int ActiveDeactiveEscalationStatus(FilterParameterCollection parameters, int statusId, int isActive, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@StatusID", statusId, DbType.Int32));
            parameters.Add(new FilterParameter("@IsActive", isActive, DbType.Int32));
            parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
            FilterParameterCollection parameterOut;
            string spa = isMCFRisk ? "spa_RM_MCF_DeleteEscalationStatus" : "spa_rm_cs_DeleteEscalationStatus";
            ExecuteNonQueryCommand(spa, parameters, out parameterOut);
            return Convert.ToInt32(parameterOut[0].ParameterValue);
        }

        #endregion Escalation Status

        #region Group Maintenance

        public int UpdateGroup(FilterParameterCollection parameters, int groupId, string groupName, string description)
        {
            return UpdateGroup(parameters, groupId, groupName, description, false);
        }

        public int UpdateGroup(FilterParameterCollection parameters, int groupId, string groupName, string description, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@GroupID", groupId, DbType.Int32));
            parameters.Add(new FilterParameter("@GroupName", groupName, DbType.String));
            parameters.Add(new FilterParameter("@Description", description, DbType.String));
            parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
            FilterParameterCollection parameterOut;
            string spa = isMCFRisk ? "spa_RM_MCF_UpdateGroup" : "spa_rm_cs_UpdateGroup";
            ExecuteNonQueryCommand(spa, parameters, out parameterOut);

            return Convert.ToInt32(parameterOut[0].ParameterValue);
        }

        public int AddGroup(FilterParameterCollection parameters, string groupName, string description)
        {
            return AddGroup(parameters, groupName, description, false);
        }

        public int AddGroup(FilterParameterCollection parameters, string groupName, string description, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@GroupName", groupName, DbType.String));
            parameters.Add(new FilterParameter("@Description", description, DbType.String));
            parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
            FilterParameterCollection parameterOut;
            string spa = isMCFRisk ? "spa_RM_MCF_AddGroup" : "spa_rm_cs_AddGroup";
            ExecuteNonQueryCommand(spa, parameters, out parameterOut);

            return Convert.ToInt32(parameterOut[0].ParameterValue);
        }

        public int ActiveDeactiveGroup(FilterParameterCollection parameters, int groupId, int isActive)
        {
            return ActiveDeactiveGroup(parameters, groupId, isActive, false);
        }

        public int ActiveDeactiveGroup(FilterParameterCollection parameters, int groupId, int isActive, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@GroupID", groupId, DbType.Int32));
            parameters.Add(new FilterParameter("@IsActive", isActive, DbType.Int32));
            parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
            FilterParameterCollection parameterOut;
            string spa = isMCFRisk ? "spa_RM_MCF_DeleteGroup" : "spa_rm_cs_DeleteGroup";
            ExecuteNonQueryCommand(spa, parameters, out parameterOut);

            return Convert.ToInt32(parameterOut[0].ParameterValue);
        }

        #endregion Group Maintenance

        #region Resolution Maintenance

        public DataTable GetResolution(FilterParameterCollection parameters, int isActive, string sortExpression)
        {
            return GetResolution(parameters, isActive, sortExpression, false);
        }

        public DataTable GetResolution(FilterParameterCollection parameters, int isActive, string sortExpression, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@IsActive", isActive, DbType.Int32));
            parameters.Add(new FilterParameter("@SortOrder", sortExpression, DbType.String));
            string spa = isMCFRisk ? "spa_RM_MCF_GetResolution" : "spa_rm_cs_GetResolution";
            return GetReports(spa, parameters);
        }

        public int ActiveDeactiveResolution(FilterParameterCollection parameters, int resolutionID, int isActive)
        {
            return ActiveDeactiveResolution(parameters, resolutionID, isActive, false);
        }

        public int ActiveDeactiveResolution(FilterParameterCollection parameters, int resolutionID, int isActive, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@ResolutionID", resolutionID, DbType.Int32));
            parameters.Add(new FilterParameter("@IsActive", isActive, DbType.Int32));
            parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
            FilterParameterCollection parameterOut;
            string spa = isMCFRisk ? "spa_RM_MCF_DeleteResolution" : "spa_rm_cs_DeleteResolution";
            ExecuteNonQueryCommand(spa, parameters, out parameterOut);
            return Convert.ToInt32(parameterOut[0].ParameterValue);
        }

        public int AddResolution(FilterParameterCollection parameters, string resolution)
        {
            return AddResolution(parameters, resolution, false);
        }

        public int AddResolution(FilterParameterCollection parameters, string resolution, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@Description", resolution, DbType.String));
            parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
            FilterParameterCollection parameterOut;
            string spa = isMCFRisk ? "spa_RM_MCF_AddResolution" : "spa_rm_cs_AddResolution";
            ExecuteNonQueryCommand(spa, parameters, out parameterOut);
            return Convert.ToInt32(parameterOut[0].ParameterValue);
        }

        public int UpdateResolution(FilterParameterCollection parameters, int resolutionID, string description)
        {
            return UpdateResolution(parameters, resolutionID, description, false);
        }

        public int UpdateResolution(FilterParameterCollection parameters, int resolutionID, string description, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@ResolutionID", resolutionID, DbType.Int32));
            parameters.Add(new FilterParameter("@Description", description, DbType.String));
            // @IsActive = -1, it means that we don't update IsActive value
            parameters.Add(new FilterParameter("@IsActive", -1, DbType.Int32));
            parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
            FilterParameterCollection parameterOut;
            string spa = isMCFRisk ? "spa_RM_MCF_UpdateResolution" : "spa_rm_cs_UpdateResolution";
            ExecuteNonQueryCommand(spa, parameters, out parameterOut);
            return Convert.ToInt32(parameterOut[0].ParameterValue);
        }

        #endregion Resolution Maintenance

        #region Statement

        public DataTable GetBEProcessor(int clientId, int siteId, string merchantNr, DateTime date)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new AS.Common.DBManager.FilterParameter(
                "@ASClient", clientId, System.Data.DbType.Int32));
            parameters.Add(new AS.Common.DBManager.FilterParameter(
                "@SiteID", siteId, System.Data.DbType.Int32));
            parameters.Add(new AS.Common.DBManager.FilterParameter(
                "@MerchantNumber", merchantNr, System.Data.DbType.String));
            if (date > DateTime.MinValue)
            {
                parameters.Add(new AS.Common.DBManager.FilterParameter(
                    "@ReportDate", date.Date, System.Data.DbType.Date));
            }
            return GetReports("spa_cs_GetBEProcessor", parameters);
        }

        public DataTable GetStatementReportDate(FilterParameterCollection parameters, string merchantNr)
        {
            parameters.Add(new FilterParameter("@MerchantNumber", merchantNr, DbType.AnsiString));
            return GetReports("spa_GetStatementReportDate", parameters);
        }

        public DataTable GetMonthEndChainStatementSummary(FilterParameterCollection parameters, string chainNumber)
        {
            parameters.Add(new FilterParameter("@ChainNumber", chainNumber, DbType.AnsiString));
            return GetReports("spa_MonthEndChainStatementSummary", parameters);
        }

        #endregion Statement

        #region Assignment

        public string GetRealHierarchyFilterValue(FilterParameterCollection parameters,
            string hierarchyFilterMode, string entityId)
        {
            return GetRealHierarchyFilterValue(parameters, hierarchyFilterMode, entityId, false);
        }

        public string GetRealHierarchyFilterValue(FilterParameterCollection parameters,
            string hierarchyFilterMode, string entityId, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@HierarchyFilterMode", hierarchyFilterMode, DbType.AnsiString));
            parameters.Add(new FilterParameter("@EntityID", entityId, DbType.AnsiString));
            parameters.Add(new FilterParameter("@HierarchyFilterValue", string.Empty, DbType.AnsiString, true));
            string spa = isMCFRisk ? "spa_RM_MCF_GetRealHierarchyFilterValueByHierarchyID" : "spa_rm_GetRealHierarchyFilterValueByHierarchyID";
            DataTable info = GetReports(spa, parameters);
            return info.Rows[0]["HierarchyFilterValue"].ToString();
        }

        public void SaveHierarchyFilter(FilterParameterCollection parameters,
            string primaryId, string hierarchyFilterMode, string hierarchyFilterValue,
            int mode)
        {
            SaveHierarchyFilter(parameters, primaryId, hierarchyFilterMode, hierarchyFilterValue, mode, false);
        }

        /// <summary>
        /// Save Hierarchy Filter
        /// </summary>
        /// <param name="mode">1,2: Assignment, 3: adhoc</param>
        public void SaveHierarchyFilter(FilterParameterCollection parameters,
            string primaryId, string hierarchyFilterMode, string hierarchyFilterValue,
            int mode, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@PrimaryID", primaryId, DbType.Int32));
            parameters.Add(new FilterParameter("@HierarchyFilterMode", hierarchyFilterMode, DbType.AnsiString));
            parameters.Add(new FilterParameter("@FilterValues", hierarchyFilterValue, DbType.AnsiString));
            parameters.Add(new FilterParameter("@Mode", mode, DbType.Int32));
            FilterParameterCollection paramsOut;
            string spa = isMCFRisk ? "spa_RM_MCF_SaveHierarchyFilter" : "spa_rm_SaveHierarchyFilter";
            ExecuteNonQueryCommand(spa, parameters, out paramsOut);
        }

        public DataTable GetHierarchyFilterList(FilterParameterCollection parameters)
        {
            return GetHierarchyFilterList(parameters, false);
        }

        public DataTable GetHierarchyFilterList(FilterParameterCollection parameters, bool isMCFRisk)
        {
            string spa = isMCFRisk ? "spa_RM_MCF_GetHierarchyFilterList" : "spa_rm_GetHierarchyFilterList";
            return GetReports(spa, parameters);
        }

        public DataTable GetAssignmentMerchantFilter(FilterParameterCollection parameters,
            int assignmentId, int? mode)
        {
            return GetAssignmentMerchantFilter(parameters, assignmentId, mode, false);
        }

        public DataTable GetAssignmentMerchantFilter(FilterParameterCollection parameters,
            int assignmentId, int? mode, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@StgAssignmentID", assignmentId, DbType.Int32));
            if (mode.HasValue)
            {
                parameters.Add(new FilterParameter("@FilterMode", mode.Value, DbType.Int32));
            }

            string spa = isMCFRisk ? "spa_RM_MCF_Get_Assignment_Filter" : "spa_rm_cs_GetAssignmentMerchantFilter";
            return GetReports(spa, parameters);
        }

        public void SaveAssignmentMerchantFilter(FilterParameterCollection parameters, AssignmentMerchantFilterModel assignmentMerchantFilterModel, bool? isExcludeMerchantsClosedStatus = null)
        {
            SaveAssignmentMerchantFilter(parameters, assignmentMerchantFilterModel, false, isExcludeMerchantsClosedStatus);
        }


        public void SaveAssignmentMerchantFilter(FilterParameterCollection parameters, AssignmentMerchantFilterModel assignmentMerchantFilterModel,
            bool isMCFRisk, bool? isExcludeMerchantsClosedStatus = null)
        {
            parameters.Add(new FilterParameter("@StgAssignmentID", assignmentMerchantFilterModel.AssignmentId, DbType.Int32));
            parameters.Add(new FilterParameter("@IsMerchantsOnWatch", assignmentMerchantFilterModel.IsMerchantsOnWatch, DbType.AnsiString));
            if (assignmentMerchantFilterModel.IsAllMerchants.HasValue)
            {
                parameters.Add(new FilterParameter("@IsAllMerchants", assignmentMerchantFilterModel.IsAllMerchants, DbType.AnsiString));
            }
            if (assignmentMerchantFilterModel.ReportDate.HasValue)
            {
                parameters.Add(new FilterParameter("@ReportDate", assignmentMerchantFilterModel.ReportDate.Value, DbType.DateTime));
            }
            if (!string.IsNullOrEmpty(assignmentMerchantFilterModel.MerchantNumber))
            {
                parameters.Add(new FilterParameter("@MerchantNumber", assignmentMerchantFilterModel.MerchantNumber, DbType.AnsiString));
            }
            if (assignmentMerchantFilterModel.Mode.HasValue)
            {
                parameters.Add(new FilterParameter("@FilterMode", assignmentMerchantFilterModel.Mode.Value, DbType.Int32));
            }
            if (isExcludeMerchantsClosedStatus.HasValue)
            {
                parameters.Add(new FilterParameter("@IsExcludeMerchantsClosedStatus", isExcludeMerchantsClosedStatus, DbType.Boolean));
            }
            FilterParameterCollection paramsOut;            
            string spa = isMCFRisk ? "spa_RM_MCF_Save_Assignment_Filter" : "spa_rm_cs_SaveAssignmentMerchantFilter";            
            ExecuteNonQueryCommand(spa, parameters, out paramsOut);
        }

        public int GetAssignmentMerchantCount(FilterParameterCollection parameters,
            int assignmentId, int filterMode)
        {
            return GetAssignmentMerchantCount(parameters, assignmentId, filterMode, false);
        }

        public int GetAssignmentMerchantCount(FilterParameterCollection parameters,
            int assignmentId, int filterMode, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@AssignmentID", assignmentId, DbType.Int32));
            parameters.Add(new FilterParameter("@FilterMode", filterMode, DbType.Int32));
            parameters.Add(new FilterParameter("@MerchantTotal", 0, DbType.Int32, true));
            FilterParameterCollection paramsOut;
            string spa = isMCFRisk ? "spa_RM_MCF_Get_Assignment_MerchantCount" : "spa_rm_cs_GetAssignmentMerchantCount";
            ExecuteNonQueryCommand(spa, parameters, out paramsOut);
            int merchantCount = 0;
            int.TryParse(paramsOut[0].ParameterValue.ToString(), out merchantCount);
            return merchantCount;
        }

        #endregion Assignment

        #region Escalation

        public DataTable GetEscalation(FilterParameterCollection parameters, GetEscalationParamsModel getEscalationParamsModel)
        {
            return GetEscalation(parameters, getEscalationParamsModel, false);
        }
        /// <summary>
        /// Get Escalation
        /// </summary>
        /// <param name="followUpCode">ALL</param>
        public DataTable GetEscalation(FilterParameterCollection parameters, GetEscalationParamsModel getEscalationParamsModel, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@AssignedToList", getEscalationParamsModel.AssignedToList, DbType.String));
            parameters.Add(new FilterParameter("@ResolutionList", getEscalationParamsModel.ResolutionList, DbType.String));
            parameters.Add(new FilterParameter("@StatusList", getEscalationParamsModel.StatusList, DbType.String));
            parameters.Add(new FilterParameter("@OpenClosedCode", getEscalationParamsModel.OpenClosedCode, DbType.String));
            parameters.Add(new FilterParameter("@OpenClosedFromDate", getEscalationParamsModel.OpenClosedFromDate, DbType.DateTime));
            parameters.Add(new FilterParameter("@OpenClosedToDate", getEscalationParamsModel.OpenClosedToDate, DbType.DateTime));
            parameters.Add(new FilterParameter("@KeyType", getEscalationParamsModel.KeyType, DbType.String));
            parameters.Add(new FilterParameter("@KeyValue", getEscalationParamsModel.KeyValue, DbType.String));

            parameters.Add(new FilterParameter("@FollowUpCode", getEscalationParamsModel.FollowUpCode, DbType.String));
            if (getEscalationParamsModel.FollowUpFromDate.HasValue)
            {
                parameters.Add(new FilterParameter("@FollowUpFromDate", getEscalationParamsModel.FollowUpFromDate.Value, DbType.DateTime));
            }
            else
            {
                parameters.Add(new FilterParameter("@FollowUpFromDate", null, DbType.DateTime));
            }
            if (getEscalationParamsModel.FollowUpToDate.HasValue)
            {
                parameters.Add(new FilterParameter("@FollowUpToDate", getEscalationParamsModel.FollowUpToDate.Value, DbType.DateTime));
            }
            else
            {
                parameters.Add(new FilterParameter("@FollowUpToDate", null, DbType.DateTime));
            }

            parameters.Add(new FilterParameter("@stOrder", getEscalationParamsModel.Order, DbType.String));

            string spa = isMCFRisk ? "spa_RM_MCF_GetEscalation" : "spa_rm_cs_GetEscalation";
            return GetReports(spa, parameters);
        }

        public DataTable GetEscalationHistory(FilterParameterCollection parameters,
            string merchantNumber, int escalationId)
        {
            return GetEscalationHistory(parameters, merchantNumber, escalationId, false);
        }

        public DataTable GetEscalationHistory(FilterParameterCollection parameters,
            string merchantNumber, int escalationId, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.AnsiString));
            parameters.Add(new FilterParameter("@EscalationID", escalationId, DbType.Int32));
            string spa = isMCFRisk ? "spa_RM_MCF_GetEscalationHistory" : "spa_rm_cs_GetEscalationHistory";
            return GetReports(spa, parameters);
        }

        #endregion Escalation

        #region Detection Queue

        public int UpdateMerchantWorked(FilterParameterCollection parameters,
            DateTime reportDate, string merchantNumber, bool status, int? assignmentID, string pageName)
        {
            return UpdateMerchantWorked(parameters, reportDate, merchantNumber, status, assignmentID, pageName, false);
        }

        public int UpdateMerchantWorked(FilterParameterCollection parameters,
            DateTime reportDate, string merchantNumber, bool status, int? assignmentID, string pageName, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
            parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
            parameters.Add(new FilterParameter("@Status", status, DbType.Boolean));
            parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
            parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
            parameters.Add(new FilterParameter("@WorkSource", pageName, DbType.String));
            FilterParameterCollection parameterOut;
            string spa = isMCFRisk ? "spa_RM_MCF_UpdateMerchantWorked" : "spa_rm_cs_UpdateMerchantWorked";
            ExecuteNonQueryCommand(spa, parameters, out parameterOut);
            return Convert.ToInt32(parameterOut[0].ParameterValue);
        }

        #endregion Detection Queue

        #region Portfolio Credits

        public int UpdatePortfolioCredits(FilterParameterCollection parameters,
           int recordId, bool status)
        {
            return UpdatePortfolioCredits(parameters, recordId, status, false);
        }

        public int UpdatePortfolioCredits(FilterParameterCollection parameters,
            int recordId, bool status, bool isMCFRisk)
        {
            parameters.Add(new FilterParameter("@RecordID", recordId, DbType.Int32));
            parameters.Add(new FilterParameter("@Status", status, DbType.Boolean));
            FilterParameterCollection parameterOut;
            string spa = isMCFRisk ? "spa_RM_MCF_UpdatePortfolioCredits" : "spa_rm_cs_UpdatePortfolioCredits";
            ExecuteNonQueryCommand(spa, parameters, out parameterOut);
            return 1;
        }

        public DataTable GetApproveGroup(string userName)
        {
            try
            {
                return _ReportingWS.GetApproveGroup(userName);
            }
            catch (Exception ex)
            {
                LogHepler.WriteLogException("GetApproveGroup", userName, null, ex);
                throw;
            }
        }

        public bool UpdateApproveGroup(string userName, string fullName, List<int> group)
        {
            try
            {
                return _ReportingWS.UpdatetApproveGroup(userName, fullName, group.ToArray());
            }
            catch (Exception ex)
            {
                LogHepler.WriteLogException("UpdateApproveGroup", $"{userName} {fullName}", null, ex);
                throw;
            }                      
        }

        public int CreateUpdateDocumentType(string userName, FilterParameterCollection parameters, ReportService.DocumentTypeModel model)
        {
            try
            {
                FilterParamWS[] _params = ConvertToFilterParamWSArray(parameters);
                return _ReportingWS.CreateUpdateDocumentType(userName, _params, model);
            }
            catch (Exception ex)
            {
                LogHepler.WriteLogException("CreateUpdateDocumentType", userName, parameters, ex);
                throw;
            }
        }

        #endregion Portfolio Credits
    }
}
