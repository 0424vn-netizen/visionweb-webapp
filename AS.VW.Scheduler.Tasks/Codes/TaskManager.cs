using AS.Common.DBManager;
using AS.VW.Scheduler.Tasks.Codes.Models;
using AS.WS.Business;
using DocumentFormat.OpenXml.Office2010.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Tasks
{
    public class TaskManager : ITaskManager
    {
        protected ReportingBusiness _ReportingBusiness = null;
        public TaskManager()
        {
            int _SqlCommandTimeout = 0;
            if (Int32.TryParse(ConfigurationManager.AppSettings["SqlCommandTimeout"], out _SqlCommandTimeout))
            {
                ASSqlDatabase.CommandTimeout = _SqlCommandTimeout;
            }

            _ReportingBusiness = new ReportingBusiness();
            try
            {
                _ReportingBusiness.InitializeForCS(ConfigurationManager.ConnectionStrings["VisionWebDB"].ConnectionString);
            }
            catch
            {
                AS.Common.Logger.LoggerManager.Debug("Get ConnectionStrings:Failed. \n");
                AS.Common.Logger.LoggerManager.Debug("Maybe name file config exists(*.dll.config). You should change to (*.config). \n");
            }
        }

        public void UpdateStatus(Processer pro, string docId, ProcessStatus status, string fileType)
        {
            UpdateStatus(pro, docId, status, fileType, string.Empty);
        }
        public void UpdateStatus(Processer pro, string docId, ProcessStatus status, string fileType, string mode)
        {
            FilterParameterCollection paramIn = new FilterParameterCollection();
            paramIn.Add(new FilterParameter("@ASClient", pro.ASClientID, DbType.Int32));
            paramIn.Add(new FilterParameter("@LogID", pro.ProcessLogID, DbType.Int32));
            paramIn.Add(new FilterParameter("@Status", (int)status, DbType.Int32));
            paramIn.Add(new FilterParameter("@DocID", docId, DbType.String));
            paramIn.Add(new FilterParameter("@FileType", pro.FileType.ToString(), DbType.String));
            paramIn.Add(new FilterParameter("@Mode", Int32.Parse(mode), DbType.Int32));

            _ReportingBusiness.ExecuteNonQueryCommand("spa_rm_UpdateStatusExportExtractMerchant", paramIn, out paramIn);
        }

        public IDataReader GetExtractReport(Processer pro)
        {
            FilterParameterCollection param = new FilterParameterCollection();
            param.Add(new FilterParameter("@ProcessLogID", pro.ProcessLogID, DbType.Int32));
            param.Add(new FilterParameter("@IsCheckData", 0, DbType.Int32, true));
            param.Add(new FilterParameter("@ASClient", pro.ASClientID, DbType.Int32));
            return _ReportingBusiness.GetReportsByDataReader(pro.SpaName, param);
        }

        public DataSet GetExtractReport(Processer pro, bool isGetTotal)
        {
            FilterParameterCollection param = new FilterParameterCollection();
            param.Add(new FilterParameter("@ProcessLogID", pro.ProcessLogID, DbType.Int32));
            param.Add(new FilterParameter("@IsCheckData", 0, DbType.Int32, true));

            if (isGetTotal)
            {
                param.Add(new FilterParameter("@ASClient", pro.ASClientID, DbType.Int32));
                param.Add(new FilterParameter("@IsPaging", false, DbType.Int32));
                param.Add(new FilterParameter("@IsCountPageTotal", true, DbType.Boolean));
            }
            return _ReportingBusiness.GetReportsAsDataSet(pro.SpaName, param);
        }

        public DataTable GetExtractReportAutoBindingParameters(Processer pro, bool isGetTotal)
        {
            FilterParameterCollection param = AutoMappingDataTypeByParameters(pro);

            param.RemoveByName("@ASClient");
            param.RemoveByName("@IsPaging");
            param.RemoveByName("@IsCountPageTotal");
            param.Add(new FilterParameter("@ASClient", pro.ASClientID, DbType.Int32));
            if (isGetTotal)
            {
                param.Add(new FilterParameter("@IsPaging", false, DbType.Int32));
                param.Add(new FilterParameter("@IsCountPageTotal", true, DbType.Boolean));
            }
            else
            {
                param.Add(new FilterParameter("@IsPaging", true, DbType.Int32));
                param.Add(new FilterParameter("@IsCountPageTotal", false, DbType.Boolean));
            }

            BuildDefaultParameters(pro, param);

            var dt = _ReportingBusiness.GetReports(pro.SpaName, param);
            return dt;
        }

        public IDataReader GetExtractReportAutoBindingParameters(Processer pro, int pageSize, int pageNo)
        {
            FilterParameterCollection param = AutoMappingDataTypeByParameters(pro);

            param.RemoveByName("@ASClient");
            param.RemoveByName("@IsPaging");
            param.RemoveByName("@IsCountPageTotal");
            param.RemoveByName("@PageNo");
            param.RemoveByName("@PageSize");

            param.Add(new FilterParameter("@ASClient", pro.ASClientID, DbType.Int32));
            param.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
            param.Add(new FilterParameter("@IsCountPageTotal", false, DbType.Boolean));
            param.Add(new FilterParameter("@PageNo ", pageNo, DbType.Int32));
            param.Add(new FilterParameter("@PageSize ", pageSize, DbType.Int32));

            BuildDefaultParameters(pro, param);

            try
            {
                IDataReader data = _ReportingBusiness.GetReportsByDataReader(pro.SpaName, param);
                return data;
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Debug(ex.ToString());
            }
            return null;
        }

        private void BuildDefaultParameters(Processer pro, FilterParameterCollection currentParam)
        {
            // Build default parameter model from Processer
            DefaultParameterModel defaultParameter = new DefaultParameterModel
            {
                ASClient = pro.ASClientID,
                UserID = pro.UserID ?? string.Empty,
                UserMode = pro.UserMode ?? string.Empty,
                SiteID = pro.SiteID,
                LanguageID = 1,
                StOrder = pro.ReportConfig?.ExportReportFilter?.SortItem.Value ?? string.Empty,
                StFilter = pro.ReportConfig?.ExportReportFilter?.FilterItem.Value ?? string.Empty
            };

            // Process each default parameter
            // @ASClient
            AddOrUpdateParameter(currentParam, "@ASClient", defaultParameter.ASClient, DbType.Int32);

            // @UserID
            AddOrUpdateParameter(currentParam, "@UserID", defaultParameter.UserID, DbType.String);

            // @UserMode
            AddOrUpdateParameter(currentParam, "@UserMode", defaultParameter.UserMode, DbType.String);

            // @SiteID
            AddOrUpdateParameter(currentParam, "@SiteID", defaultParameter.SiteID, DbType.Int32);

            // @LanguageID
            AddOrUpdateParameter(currentParam, "@LanguageID", defaultParameter.LanguageID, DbType.Int32);

            AddOrUpdateParameter(currentParam, "@stOrder", defaultParameter.StOrder, DbType.String);

            AddOrUpdateParameter(currentParam, "@stFilter", defaultParameter.StFilter, DbType.String);
        }

        /// <summary>
        /// Add parameter if not exists, or update with default value if existing value is null/empty
        /// </summary>
        private void AddOrUpdateParameter(FilterParameterCollection currentParam, string paramName, object defaultValue, DbType dbType)
        {
            // Find existing parameter in currentParam
            FilterParameter existingParam = currentParam.FindFilterParameterByName(paramName, false);

            if (existingParam == null)
            {
                // Parameter not exists in currentParam, add it with default value
                currentParam.Add(new FilterParameter(paramName, defaultValue, dbType));
            }
            else if (!HasValidValue(existingParam))
            {
                // Parameter exists but has null or empty value, update with default value
                existingParam.ParameterValue = defaultValue;
            }
            // If parameter exists and has valid value, keep the existing value
        }

        /// <summary>
        /// Check if a FilterParameter has a valid (non-null, non-empty) value
        /// </summary>
        private bool HasValidValue(FilterParameter param)
        {
            if (param == null || param.ParameterValue == null)
            {
                return false;
            }

            // Check if string value is empty
            if (param.ParameterValue is string strValue)
            {
                return !string.IsNullOrEmpty(strValue);
            }

            // For numeric types, check if it's not default value (0)
            // But we should consider 0 as valid for some cases like SiteID
            // So we only check for null here
            return true;
        }

        private FilterParameterCollection AutoMappingDataTypeByParameters(Processer pro)
        {
            FilterParameterCollection param = new FilterParameterCollection();

            try
            {
                FilterParameterCollection spaInfoParas = new FilterParameterCollection();
                spaInfoParas.Add(new FilterParameter("@ProcName", pro.SpaName, DbType.String));
                DataTable dtParamInfos = _ReportingBusiness.GetReports("spp_GetProcParameterInfo", spaInfoParas);

                //dtParamInfos is a datatable with 3 columns: ParamName, DataType, AllowNull
                //For example: ParamName = "@ASClient", DataType="int", AllowNull="1"

                if (dtParamInfos != null && dtParamInfos.Rows.Count > 0)
                {
                    // Build a dictionary for quick lookup of ParamInput values
                    Dictionary<string, string> paramInputValues = new Dictionary<string, string>();

                    if (pro.ReportConfig?.ExportReportFilter?.ParamInputs != null)
                    {
                        foreach (var paramInput in pro.ReportConfig.ExportReportFilter.ParamInputs)
                        {
                            if (!string.IsNullOrEmpty(paramInput.Key))
                            {
                                paramInputValues[paramInput.Key.ToLower()] = paramInput.Value ?? string.Empty;
                            }
                        }
                    }

                    foreach (DataRow row in dtParamInfos.Rows)
                    {
                        string paramName = row["ParamName"]?.ToString() ?? string.Empty;
                        string dataType = row["DataType"]?.ToString() ?? string.Empty;
                        string allowNull = row["AllowNull"]?.ToString() ?? "0";

                        if (string.IsNullOrEmpty(paramName) || string.IsNullOrEmpty(dataType))
                        {
                            continue;
                        }

                        // Map DataType string to DbType enum
                        DbType dbType = MapStringToDbType(dataType);

                        // Determine if parameter allows null
                        bool isNullable = allowNull == "1" || allowNull.ToLower() == "true";

                        // Try to find the parameter value from ParamInputs
                        // Match by removing @ symbol for comparison
                        string paramKeyForLookup = paramName.StartsWith("@")
                            ? paramName.Substring(1).ToLower()
                            : paramName.ToLower();

                        object paramValue = null;

                        // Look for exact match first (without @ symbol)
                        if (paramInputValues.TryGetValue(paramKeyForLookup, out string foundValue))
                        {
                            paramValue = ConvertParameterValue(foundValue, dbType);
                        }
                        // Also try with @ symbol
                        else if (paramInputValues.TryGetValue(paramName.ToLower(), out string foundValueWithAt))
                        {
                            paramValue = ConvertParameterValue(foundValueWithAt, dbType);
                        }

                        // Create FilterParameter with the value found or null
                        FilterParameter filterParam = new FilterParameter
                        {
                            ParameterName = paramName,
                            ParameterType = dbType,
                            ParameterValue = paramValue,
                            IsOutParameter = false
                        };

                        param.Add(filterParam);
                    }
                }
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Debug(string.Format("AutoMappingDataTypeByParameters Error: {0}", ex.ToString()));
            }

            return param;
        }

        /// <summary>
        /// Convert string value to appropriate type based on DbType
        /// </summary>
        private object ConvertParameterValue(string stringValue, DbType dbType)
        {
            if (stringValue == string.Empty)
                return "";

            if (stringValue.Equals("NULL", StringComparison.OrdinalIgnoreCase))
                return null;

            try
            {
                switch (dbType)
                {
                    // Numeric types
                    case DbType.Int32:
                    case DbType.Int16:
                    case DbType.Int64:
                    case DbType.Byte:
                        if (int.TryParse(stringValue, out int intValue))
                            return intValue;
                        break;

                    case DbType.Decimal:
                    case DbType.Double:
                        if (decimal.TryParse(stringValue, out decimal decimalValue))
                            return decimalValue;
                        break;

                    // Date/Time types
                    case DbType.DateTime:
                    case DbType.Date:
                    case DbType.Time:
                        if (DateTime.TryParse(stringValue, out DateTime dateValue))
                            return dateValue;
                        break;

                    // Boolean type
                    case DbType.Boolean:
                        if (bool.TryParse(stringValue, out bool boolValue))
                            return boolValue;
                        // Also handle 0/1 for boolean
                        if (stringValue == "1")
                            return true;
                        if (stringValue == "0")
                            return false;
                        break;

                    // Guid type
                    case DbType.Guid:
                        if (Guid.TryParse(stringValue, out Guid guidValue))
                            return guidValue;
                        break;

                    // Binary type
                    case DbType.Binary:
                        try
                        {
                            return Convert.FromBase64String(stringValue);
                        }
                        catch
                        {
                            return Encoding.UTF8.GetBytes(stringValue);
                        }

                    // String types and default
                    case DbType.String:
                    case DbType.AnsiString:
                    case DbType.StringFixedLength:
                    case DbType.AnsiStringFixedLength:
                    default:
                        return stringValue;
                }
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Debug(string.Format("ConvertParameterValue Error for value '{0}' with DbType '{1}': {2}", stringValue, dbType, ex.ToString()));
                // Return the original string if conversion fails
                return stringValue;
            }

            return stringValue;
        }

        /// <summary>
        /// Map SQL Server data type string to DbType enum
        /// </summary>
        private DbType MapStringToDbType(string sqlDataType)
        {
            if (string.IsNullOrEmpty(sqlDataType))
                return DbType.String;

            string dataType = sqlDataType.Trim().ToLower();

            switch (dataType)
            {
                // Numeric types
                case "int":
                case "integer":
                    return DbType.Int32;

                case "bigint":
                    return DbType.Int64;

                case "smallint":
                    return DbType.Int16;

                case "tinyint":
                    return DbType.Byte;

                case "decimal":
                case "numeric":
                    return DbType.Decimal;

                case "float":
                case "real":
                    return DbType.Double;

                // String types
                case "varchar":
                case "char":
                case "nvarchar":
                case "nchar":
                case "text":
                case "ntext":
                    return DbType.String;

                // Date/Time types
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return DbType.DateTime;

                case "date":
                    return DbType.Date;

                case "time":
                    return DbType.Time;

                // Other types
                case "bit":
                    return DbType.Boolean;

                case "binary":
                case "varbinary":
                case "image":
                    return DbType.Binary;

                case "uniqueidentifier":
                    return DbType.Guid;

                case "money":
                case "smallmoney":
                    return DbType.Decimal;

                // Default
                default:
                    return DbType.String;
            }
        }

        public IDataReader GetExtractReport(Processer pro, int pageSize, int pageNo)
        {
            FilterParameterCollection param = new FilterParameterCollection();
            param.Add(new FilterParameter("@ProcessLogID", pro.ProcessLogID, DbType.Int32));
            param.Add(new FilterParameter("@IsCheckData", 0, DbType.Int32, true));
            param.Add(new FilterParameter("@ASClient", pro.ASClientID, DbType.Int32));
            param.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
            param.Add(new FilterParameter("@IsCountPageTotal", false, DbType.Boolean));
            param.Add(new FilterParameter("@PageNo ", pageNo, DbType.Int32));
            param.Add(new FilterParameter("@PageSize ", pageSize, DbType.Int32));
            try
            {
                IDataReader data = _ReportingBusiness.GetReportsByDataReader(pro.SpaName, param);
                return data;
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Debug(ex.ToString());
            }
            return null;
        }




        public List<Processer> GetDataProcessInQueue()
        {
            return GetDataProcessInQueue(string.Empty);
        }
        public List<Processer> GetDataProcessInQueue(string mode)
        {
            FilterParameterCollection param = new FilterParameterCollection();
            param.Add(new FilterParameter("@MaxRows", AppConfigurations.ExportThreadMax, DbType.Int32));
            if (!string.IsNullOrEmpty(mode))
            {
                param.Add(new FilterParameter("@Mode", mode, DbType.String));
            }
            DataTable dataProcess = _ReportingBusiness.GetReports("spa_rm_GetExtractListInQueue", param);
            return dataProcess.ConvertDataTable<Processer>();
        }

        public ExportReportConfigResponse GetReportConfig(ExportReportConfigRequest reportConfigRequest)
        {
            var response = new ExportReportConfigResponse();

            FilterParameterCollection param = new FilterParameterCollection();

            param.Add(new FilterParameter("@AsClientid", reportConfigRequest.AsClientId, DbType.Int32));
            param.Add(new FilterParameter("@CategoryCode", reportConfigRequest.CategoryCode, DbType.String));
            param.Add(new FilterParameter("@SubCategoryCode", reportConfigRequest.SubCategoryCode, DbType.String));

            DataTable dataProcess = _ReportingBusiness.GetReports("spa_ExportingService_GetReportConfig", param);

            string xmlFilterContent = dataProcess.Rows[0]["FilterContent"].ToString();
            
            if (AppConfigurations.IsDebug)
            {
                //xmlFilterContent = @"<ExportFilter>
                //  <ParamInputs>
                //<ParamInput Key=""HierarchyFilterMode"" Value=""MERCHANTNUMBER"" />
                //<ParamInput Key=""UserMode"" Value=""CSUSER"" />
                //<ParamInput Key=""UserID"" Value=""asadmin"" />
                //<ParamInput Key=""ASClient"" Value=""200"" />
                //<ParamInput Key=""SiteID"" Value=""0"" />
                //<ParamInput Key=""LanguageID"" Value=""1"" />
                //<ParamInput Key=""HierarchyFilterValue"" Value="""" />
                //<ParamInput Key=""stOrder"" Value="""" />
                //<ParamInput Key=""stFilter"" Value="""" />
                //  </ParamInputs>
                //  <FilterItem Value="""" />
                //  <SortItem Value="""" />
                //  <CustomView Value="""" />
                //  <ReportHeader Value=""MerchantList - All ISOs"" />
                //</ExportFilter>";

                xmlFilterContent = @"<ExportFilter>
              <ParamInputs>            
            <ParamInput Key=""UserMode"" Value=""CSUSER"" />
            <ParamInput Key=""UserID"" Value=""asadmin"" />            
            <ParamInput Key=""MerchantNumber"" Value=""7097977223406659"" />
            <ParamInput Key=""DateRange"" Value=""3"" />
            <ParamInput Key=""ReportDate"" Value=""2025-09-05"" />
              </ParamInputs>
              <FilterItem Value="""" />
              <SortItem Value=""ReportDate DESC, PartialCardNumber ASC"" />
              <CustomView Value=""ReportDate,TransDate,CardType,CountryCode,CardNumber,BinNumber,DupeCount,ExpirationDate,TransType,Amount,MatchCode,ADF,ResponseCode,AuthCode,AVS,CVV,KeyedEntry,EMVIndicator,SettleType,FileSource,TerminalNumber"" />
              <ReportHeader Value=""Transaction History"" />
            </ExportFilter>";
            }

            var parseResult = GeneralFuncLibraries.ExportReportFilterModelParse(xmlFilterContent);
            response.ExportReportFilter = parseResult;
            return response;
        }


        public void UpdateFileName(int logID, ProcessStatus status)
        {
            FilterParameterCollection paramIn = new FilterParameterCollection();
            paramIn.Add(new FilterParameter("@ProcessLogID", logID, DbType.Int32));
            paramIn.Add(new FilterParameter("@DocID", -1, DbType.Int64)); //dump docid: not use
            paramIn.Add(new FilterParameter("@Status", (int)ProcessStatus.Zipping, DbType.Int32));
            _ReportingBusiness.ExecuteNonQueryCommand("spa_rm_UpdateStatusExportExtractMerchant", paramIn, out paramIn);
        }

        public IDataReader GetExtractReport(Processer pro, DateTime reportDate)
        {
            FilterParameterCollection param = new FilterParameterCollection();
            param.Add(new FilterParameter("@ProcessLogID", pro.ProcessLogID, DbType.Int32));
            param.Add(new FilterParameter("@IsCheckData", 0, DbType.Int32, true));
            param.Add(new FilterParameter("@ASClient", pro.ASClientID, DbType.Int32));
            param.Add(new FilterParameter("@IsPaging", false, DbType.Boolean));
            param.Add(new FilterParameter("@IsCountPageTotal", false, DbType.Boolean));
            param.Add(new FilterParameter("@ReportDate ", reportDate, DbType.DateTime));
            try
            {
                IDataReader data = _ReportingBusiness.GetReportsByDataReader(pro.SpaName, param);
                return data;
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Debug(ex.ToString());
            }
            return null;
        }
    }
}