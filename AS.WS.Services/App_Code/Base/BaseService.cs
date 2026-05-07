using AS.ApiClient.UnderWriting;
using AS.ApiClient.UnderWriting.Models;
using AS.Common.DBManager;
using AS.WS.Business;
using AS.WS.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for fdc_ReportingServices
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[Microsoft.Web.Services3.Policy("ServerPolicy")]

public class BaseService : AS.Common.WSE.ASWebService
{
    protected ReportingBusiness _ReportingBusiness = null;    
    private int AsClientId { get; set; }
    
    [WebMethod]
    public string Ping()
    {
        return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
    }
    [WebMethod]
    public string HelloWorld()
    {
        return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
    }
    public BaseService()
    {
        if (Microsoft.Web.Services3.ResponseSoapContext.Current == null)
        {
            throw new UnauthorizedAccessException("Access Denied");
        }

        AsClientId = int.Parse(RequestHeaders["ClientId"]);
        _ReportingBusiness = new ReportingBusiness();
    }

    [WebMethod]
    public DataTable GetReports(string spName, FilterParameterCollection parameters)
    {  
        var reportRequest = ProccessParameters(parameters);        
        DataTable reportData = _ReportingBusiness.GetReports(spName, reportRequest.Params);       
        DecryptData(reportData, reportRequest);
        return reportData;
    }

    [WebMethod]
    public string GetHashData(string plainText)
    {
        var result = _ReportingBusiness.GetHashData(plainText);       
        return result;
    }

    public IDataReader GetReports_ByDataReader(string spName, FilterParameterCollection parameters)
    {
        var reportRequest = ProccessParameters(parameters);
        return _ReportingBusiness.GetReportsByDataReader(spName, reportRequest.Params);
    }

    [WebMethod]
    public int ExecuteNonQueryCommand(string spName, AS.Common.DBManager.FilterParameterCollection parameters, out AS.Common.DBManager.FilterParameterCollection OutputParams)
    {
        return _ReportingBusiness.ExecuteNonQueryCommand(spName, parameters, out OutputParams);
    }

    [WebMethod]
    public DataSet ExecuteQueryCommand(string spName, AS.Common.DBManager.FilterParameterCollection parameters, out AS.Common.DBManager.FilterParameterCollection OutputParams)
    {
        return _ReportingBusiness.ExecuteQueryCommand(spName, parameters, out OutputParams);
    }

    [WebMethod]
    public DataSet GetReportsAsDataSet(string spName, AS.Common.DBManager.FilterParameterCollection parameters)
    {
        if (parameters != null && (parameters.FindFilterParameterByName("@EncryptedParams", true) != null || parameters.FindFilterParameterByName("@EncryptedColumn", true) != null))
        {
            var reportRequest = ProccessParameters(parameters);
            DataSet dataSet = _ReportingBusiness.GetReportsAsDataSet(spName, reportRequest.Params);
            
            foreach (DataTable table in dataSet.Tables)
            {
                DecryptData(table, (ReportRequestModel)reportRequest.Clone());
            }
            return dataSet;
        }
        else
        {
            return _ReportingBusiness.GetReportsAsDataSet(spName, parameters);
        }
    }
    
    [WebMethod]
    public void ExportFlatReport(string spaName, FilterParameterCollection parameters, string fileName, string resources, string currencyFormat, bool hasRQColumn, string columnsCustomView)
    {
        string filePath = ConfigurationManager.AppSettings["ExportTempFolder"].ToString();
        bool isNetworkPath = filePath.StartsWith("\\\\");

        if (!isNetworkPath)
        {
            filePath = HttpContext.Current.Server.MapPath("~/" + filePath);
        }

        DataSet flatReportInfo = GetReportsAsDataSet(spaName, parameters);
        _ReportingBusiness.ExportFlatReport(flatReportInfo, filePath, fileName, resources, currencyFormat, hasRQColumn, columnsCustomView);
    }
    #region For Encrypt/Decrypt data
    [WebMethod]
    public string EncryptText(string encryptStr, int clientID)
    {
        return _ReportingBusiness.ExcryptText(encryptStr, clientID, Server.MapPath("~/App_Data/"));
    }
    [WebMethod]
    public DataTable EncryptTexts(DataTable encryptTable, int clientID)
    {
        return _ReportingBusiness.ExcryptTexts(encryptTable, clientID, Server.MapPath("~/App_Data/"));
    }
    [WebMethod]
    public string DecryptText(string decryptStr, int clientID)
    {
        return _ReportingBusiness.DecryptText(decryptStr, clientID, Server.MapPath("~/App_Data/"));
    }
    [WebMethod]
    public string EncryptTextWithMultiKey(string plainText, int clientID)
    {
        return _ReportingBusiness.EncryptTextWithMultiKey(plainText, clientID, Server.MapPath("~/App_Data/"));
    }
    #endregion

    [WebMethod]
    public DataTable HashDataTable(DataTable data)
    {
        return _ReportingBusiness.HashDataTable(data);
    }

    [WebMethod(Description = "Get Encrypt and hash values")]
    public DataTable GetEncryptAndHashValues(DataTable data, int clientID, bool isRunHash = true)
    {
        var nCols = data.Columns.Count;

        for (int i = 0; i < nCols; i++)
        {
            DataColumn column = data.Columns[i];
            data.Columns.Add(column.ColumnName + "_Original", typeof(string));
            if (isRunHash)
            {
                data.Columns.Add(column.ColumnName + "_Hashed", typeof(string));
            }

            foreach (DataRow row in data.Rows)
            {
                if (row[column.ColumnName] != null)
                {
                    var valueItem = row[column.ColumnName].ToString();
                    row[column.ColumnName + "_Original"] = valueItem;
                    row[column.ColumnName] = _ReportingBusiness.ExcryptText(valueItem, clientID, Server.MapPath("~/App_Data/"));
                    if (isRunHash)
                    {
                        row[column.ColumnName + "_Hashed"] = SHA2(valueItem);
                    }
                }
            }
        }

        return data;
    }



    [WebMethod]
    public DataTable GetApproveGroup(string userName)
    {
        var emptyTable = new DataTable("ApproveGroups");
        emptyTable.Columns.Add("Id", typeof(string));
        emptyTable.Columns.Add("ApproverGroupName", typeof(string));
        emptyTable.Columns.Add("IsAssigned", typeof(bool));
        
        var isDisable = ConfigurationManager.AppSettings["UnderWritingApiDisable"] == "true";
        if (isDisable)
            return emptyTable;
        List<JsonFilter> filters = new List<JsonFilter>();
        filters.Add(new JsonFilter() { AttributeName = "RecordStatus", Value = "1", SearchType = "Equal" });

        var headerRequest = new GetApproveGroupRequest()
        {
            Header = new Header()
            {
                AsclientId = AsClientId,
                UserId = userName,
                LanguageId = "1"
            },
            Data = new RequestFilterBody() { Filters = filters.ToArray() }
        };

        var client = new UnderWritingClient();
        var approveGroup = client.GetApproveGroupsOfMember(headerRequest);
        if (approveGroup != null && approveGroup.Any())
        {
            var result = AS.Core.Common.Utilities.Converter.ToDataTable(approveGroup);
            result.TableName = "ApproveGroups";
            return result;
        }

        return emptyTable;
    }

    [WebMethod]
    public bool UpdatetApproveGroup(string userName, string fullName, List<int> groups)
    {
        var isDisable = ConfigurationManager.AppSettings["UnderWritingApiDisable"]  == "true";
        if (isDisable)
            return true;

        var headerRequest = new UpdateApproveGroupRequest()
        {
            Header = new Header()
            {
                AsclientId = AsClientId,
                UserId = userName,
                LanguageId = "1"
            },
            Data = new UpdateApproveGroupData()
            {
                MemberId = userName,
                MemberFullname = fullName,
                ApproverGroupIds = groups
            }
        };

        var client = new UnderWritingClient();
        return client.UpdateApproverGroupMember(headerRequest, "UpdateApproverGroupMember");
    }

    [WebMethod]
    public int CreateUpdateDocumentType(string userName, FilterParameterCollection parameters, DocumentTypeModel data)
    {
        var isAdd = true;
        var spaName = "spa_MDT_Add_DocumentType";        
        if (data.Id == 0)
        {
            parameters.Add("@SourceIDList", data.SourceIdList, DbType.AnsiString);
            parameters.Add("@DocumentTypeID", 0, DbType.Int32, true);            
        }
        else
        {
            spaName = "spa_MDT_Update_DocumentType";
            parameters.Add("@SourceID", data.SourceId, DbType.Int32);
            parameters.Add("@DocumentTypeID", data.Id, DbType.Int32);
            isAdd = false;
        }

        parameters.Add("@DocumentType", data.Name, DbType.AnsiString);
        parameters.Add("@Description", data.Description, DbType.AnsiString);
        parameters.Add("@IsActive", data.IsActive, DbType.Boolean);
        parameters.Add("@ResultType", 0, DbType.Int32, true);

        ExecuteQueryCommand(spaName, parameters, out parameters);
        int ouputResult;
        var  result = parameters.FindFilterParameterByName("@ResultType", true).ParameterValue;       

        if (result == null || result == DBNull.Value) 
            ouputResult = 0;
        else
            ouputResult = int.Parse(result.ToString());

        //sync data
        if (ouputResult != 0 && ouputResult != -1 && ouputResult != -2 && data.IsSyncData)
        {
            if (isAdd)
            {
                var outDocumentId = parameters.FindFilterParameterByName("@DocumentTypeID", true).ParameterValue;

                if (outDocumentId != null && outDocumentId != DBNull.Value)
                {
                    data.Id = int.Parse(outDocumentId.ToString());
                }
            }

            if (data.Id > 0)
            {
                // call api under writing
                var headerRequest = new DocumentTypeRequest()
                {
                    Header = new Header()
                    {
                        AsclientId = AsClientId,
                        UserId = userName,
                        LanguageId = "1"
                    },
                    Data = new DocumentType()
                    {
                        Id = data.Id,
                        DocumentTypeName = data.Name,
                        RecordStatus = data.IsActive ? 1 : 0,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = userName,
                        UpdatedDate = DateTime.Now
                    }
                };

                var client = new UnderWritingClient();
                client.CreateUpdateDocumentType(headerRequest, isAdd? "CreateDocumentType" : "UpdateDocumentType");
            }            
        }
        return ouputResult;
    }

    private ReportRequestModel ProccessParameters(FilterParameterCollection parameters)
    {
        FilterParameterCollection custom_parameters = new FilterParameterCollection();

        if (parameters != null)
            custom_parameters = parameters;

        if (parameters != null && parameters.FindFilterParameterByName("@EncryptedParams", true) != null)
            custom_parameters = CopyParameterCollection(parameters);

        var isExporting = false;
        FilterParameter param = custom_parameters.FindFilterParameterByName("@IsExporting", true);
        if (param != null)
        {
            //Get value
            isExporting = Convert.ToBoolean(param.ParameterValue);
            //Remove param: @IsExporting
            custom_parameters.Remove(param);
        }

        var clientId = 0;
        param = custom_parameters.FindFilterParameterByName("@ASClient", false);
        if (param != null)
            int.TryParse(param.ParameterValue.ToString(), out clientId);

        var hashColumnsConfig = ClientExtendSettingSingleton.ShareInstance.GetDataOfExtendedSetting("HASH_COLUMNS");
        var hashClientsConfig = ClientExtendSettingSingleton.ShareInstance.GetDataOfExtendedSetting("HASH_CLIENTIDS");
        var encryptedColumnsRequest = ProcessEncryptedColumn(custom_parameters, hashColumnsConfig, hashClientsConfig, clientId);
        ProcessEncryptedParams(clientId, parameters, custom_parameters, hashColumnsConfig, hashClientsConfig);
        var requestModel = new ReportRequestModel()
        {
            AsClientId = clientId,
            IsExport = isExporting,
            IsHash = encryptedColumnsRequest.IsHash,
            EncryptedColumnRequest = encryptedColumnsRequest.EncryptedColumnRequest,
            HashColumnsConfig = hashColumnsConfig,
            HashClientsConfig = hashClientsConfig,
            Params = custom_parameters,
            HashColumns = GetHashColumns(encryptedColumnsRequest.EncryptedColumnRequest, GetHashColumnsConfig(hashColumnsConfig)),
            DecryptColumns = GetDecryptColumns(encryptedColumnsRequest.EncryptedColumnRequest, GetHashColumnsConfig(hashColumnsConfig))
        };

        return requestModel;
    }

    private List<HashColEntity> GetHashColumns(List<string> columnRequest, List<string> hashColumnConfig)
    {
        if (columnRequest!=null && columnRequest.Any()
            && hashColumnConfig != null && hashColumnConfig.Any())
        {
            return columnRequest.Where(x => hashColumnConfig.Any(config => config.Equals(x, StringComparison.OrdinalIgnoreCase))).Select(x => new HashColEntity(x, "")).ToList();
        }
        return new List<HashColEntity>();
    }
    private List<string> GetDecryptColumns(List<string> columnRequest, List<string> hashColumnConfig)
    {
        if (columnRequest != null && columnRequest.Any()
            && hashColumnConfig != null && hashColumnConfig.Any())
        {
            return columnRequest.Where(x => !hashColumnConfig.Any(config => config.Equals(x, StringComparison.OrdinalIgnoreCase))).ToList();
        }
        return new List<string>();
    }
    private List<string> GetHashColumnsConfig(string config)
    {
        if (string.IsNullOrEmpty(config))
            return new List<string>();

        return config.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
    private ReportRequestModel ProcessEncryptedColumn(FilterParameterCollection parameters, string hashColumns, string hashClientIDs, int clientId)
    {        
        bool isHash = false;
        var encryptedCol = new List<string>();
        FilterParameter param = parameters.FindFilterParameterByName("@EncryptedColumn", true);        
        
        if (param != null)
        {
            var encryptedColumns = param.ParameterValue.ToString();
            //Remove param: @EncryptedColumn
            parameters.Remove(param);

            if (!string.IsNullOrEmpty(encryptedColumns) && !string.IsNullOrEmpty(hashColumns))
            {
                encryptedCol = encryptedColumns.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                hashColumns = string.Format(",{0},", hashColumns);

                //Check encrypt column exist in list columns hash.
                foreach (string encryptedColumn in encryptedCol)
                {
                    if (!string.IsNullOrEmpty(encryptedColumn) && hashColumns.Contains(string.Format(",{0},", encryptedColumn)))
                    {
                        //check client that use hash or not
                        hashClientIDs = string.Format(",{0},", hashClientIDs);
                        isHash = hashClientIDs.Contains(string.Format(",{0},", clientId.ToString()));

                        if (parameters.FindFilterParameterByName("@IsStat", true) == null)
                        {
                            parameters.Add(new FilterParameter("@IsStat", isHash, DbType.Boolean));
                        }
                        break;
                    }
                }
            }
        }
        return new ReportRequestModel() { IsHash = isHash, EncryptedColumnRequest = encryptedCol };
    }

    private void ProcessEncryptedParams(int ASClientID, FilterParameterCollection org_parameters, FilterParameterCollection parameters, string hashColumns, string hashClientIDs)
    {
        FilterParameter param = parameters.FindFilterParameterByName("@EncryptedParams", true);
        //Find Encrypted Param: @EncryptedParam        

        if (param != null)
        {
            string encryptedParams = param.ParameterValue.ToString();
            //Remove param: @EncryptedColumn
            parameters.Remove(param);

            if (!string.IsNullOrEmpty(encryptedParams))
            {
                string[] encryptedPrs = encryptedParams.Split(',');
                if (!string.IsNullOrEmpty(hashColumns))
                    hashColumns = string.Format(",{0},", hashColumns);

                foreach (string encryptedPr in encryptedPrs)
                {
                    FilterParameter pr = parameters.FindFilterParameterByName("@" + encryptedPr, true);
                    FilterParameter org_pr = org_parameters.FindFilterParameterByName("@" + encryptedPr, true);
                    if (pr != null && pr.ParameterValue != null)
                    {
                        string val = org_pr.ParameterValue.ToString();
                        if (!string.IsNullOrEmpty(hashColumns) && hashColumns.Contains(string.Format(",{0},", encryptedPr)))
                        {

                            hashClientIDs = string.Format(",{0},", hashClientIDs);
                            bool isStat = hashClientIDs.Contains(string.Format(",{0},", ASClientID.ToString()));

                            if (parameters.FindFilterParameterByName("@IsStat", true) == null)
                            {
                                parameters.Add(new FilterParameter("@IsStat", isStat, DbType.Boolean));
                            }
                            if (isStat)
                                pr.ParameterValue = _ReportingBusiness.GetHashData(val);
                            else
                                pr.ParameterValue = EncryptTextWithMultiKey(val, ASClientID);
                        }
                        else
                        {
                            pr.ParameterValue = EncryptTextWithMultiKey(val, ASClientID);
                        }
                    }
                }
            }
        }
    }

    private void ProccessDataToFinalTable(DataTable list, ReportRequestModel request)
    {
        if (!request.IsHash)
        {
            foreach (string encryptedColumn in request.EncryptedColumnRequest)
            {
                if (!string.IsNullOrEmpty(encryptedColumn) && list != null)
                {
                    int rowCount = list.Rows.Count;
                    if (!request.IsExport)
                    {
                        if (rowCount > 0)
                        {
                            list.Columns.Add(encryptedColumn + "_Original", typeof(string));
                            for (int index = 0; index < rowCount; index++)
                            {
                                if (list.Columns.Contains(encryptedColumn) && list.Rows[index][encryptedColumn] != DBNull.Value)
                                {
                                    list.Rows[index][encryptedColumn + "_Original"] = list.Rows[index][encryptedColumn].ToString();
                                    list.Rows[index][encryptedColumn] = DecryptText(list.Rows[index][encryptedColumn].ToString(), request.AsClientId);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int index = 0; index < rowCount; index++)
                        {
                            if (list.Columns.Contains(encryptedColumn) && list.Rows[index][encryptedColumn] != DBNull.Value)
                            {
                                list.Rows[index][encryptedColumn] = DecryptText(list.Rows[index][encryptedColumn].ToString(), request.AsClientId);
                            }
                        }
                    }
                }
            }
        }
        else if (!request.IsExport)
        {
            _ReportingBusiness.HashData(request.AsClientId, list, request.EncryptedColumnRequest.ToArray(), request.HashColumnsConfig, Server.MapPath("~/App_Data/"));
        }
    }

    private void DecryptData(DataTable data, ReportRequestModel request)
    {
        if (!request.IsHash)
        {
            foreach (string encryptedColumn in request.EncryptedColumnRequest)
            {
                if (!string.IsNullOrEmpty(encryptedColumn) && data != null && data.Rows.Count > 0)
                {
                    if (!request.IsExport)
                        data.Columns.Add(encryptedColumn + "_Original", typeof(string));

                    for (int index = 0; index < data.Rows.Count; index++)
                    {
                        if (data.Columns.Contains(encryptedColumn) && data.Rows[index][encryptedColumn] != DBNull.Value 
                            && !string.IsNullOrEmpty(data.Rows[index][encryptedColumn].ToString()))
                        {
                            var cellValue = data.Rows[index][encryptedColumn].ToString();
                            
                            if (!request.IsExport)
                                data.Rows[index][encryptedColumn + "_Original"] = cellValue;

                            data.Rows[index][encryptedColumn] = DecryptText(cellValue, request.AsClientId);
                        }
                    }
                }
            }
        }
        else if (!request.IsExport)
        {
            _ReportingBusiness.HashData(request, data, Server.MapPath("~/App_Data/"));
        }
    }

    private FilterParameterCollection CopyParameterCollection(FilterParameterCollection parameters)
    {
        FilterParameterCollection list = new FilterParameterCollection();

        for (int i = 0; i < parameters.Count; i++)
        {
            var parameter = new FilterParameter
            {
                ParameterName = parameters[i].ParameterName,
                ParameterValue = parameters[i].ParameterValue,
                ParameterType = parameters[i].ParameterType,
                IsOutParameter = parameters[i].IsOutParameter
            };
            list.Add(parameter);
        }

        return list;
    }

    /// <summary>
    /// Get from HashTaxId MIF Fulton
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private static string SHA2(string text)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        SHA256Managed sha2Managed = new SHA256Managed();
        StringBuilder strBuilder = new StringBuilder("");
        byte[] hashed = sha2Managed.ComputeHash(encoding.GetBytes(text));
        for (int i = 0; i < hashed.Length; i++)
        {
            strBuilder.Append(hashed[i].ToString("X2"));
        }
        return strBuilder.ToString();
    }
    public sealed class ClientExtendSettingSingleton
    {
        private static ClientExtendSettingSingleton instance;
        private readonly DataTable ExtendedClientSettings;
        private ClientExtendSettingSingleton()
        {
            ExtendedClientSettings = GeneralFuncsLib.ExtendedClientSettings();
        }

        public static ClientExtendSettingSingleton ShareInstance
        {
            get
            {
                if (instance == null)
                    instance = new ClientExtendSettingSingleton();
                return instance;
            }
        }

        public string GetDataOfExtendedSetting(string settingName)
        {
            if (ExtendedClientSettings != null && ExtendedClientSettings.Rows.Count > 0)
            {
                var extendedSetting = ExtendedClientSettings.Rows.Cast<DataRow>().FirstOrDefault(row => row["settingName"].ToString().ToLower().Equals(settingName.ToLower()));

                if (extendedSetting != null)
                {
                    return extendedSetting["data"].ToString();
                }
            }
            return string.Empty;
        }
    }
}
