using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for EngineBuilder
/// </summary>
public class EngineBuilder
{
    public int AsClientId { get; set; }
    public string _MerchantNumber { get; set; }
    public Dictionary<string, DataTable> _MerchantData { get; set; }
    public MerchantProfileClientConfig _ClientConfig { get; set; }
    public MerchantProfileConfig _MerchantConfig { get; set; }

    public string _Processor { get; set; }
    public Dictionary<int, string> _ExportNames { get; set; }
    public EngineBuilder(int asClientId, string merchantNumber)
    {
        AsClientId = asClientId;
        _MerchantNumber = merchantNumber;
        _ClientConfig = MerchantProfileHelper.GetClientConfig();
        _Processor = GetProcessor();
        _MerchantConfig = MerchantProfileHelper.GetMerchantProfileConfigs(_Processor);
        Init();
    }
    private void Init()
    {
        BuildProfileModel();
        GetExportSectionNames();
    }
    private void GetExportSectionNames()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();

        if (_MerchantData != null && _MerchantData.Values.Count > 0)
        {
            int index = 0;
            foreach (var item in _MerchantConfig.Sections)
            {
                if (item.IsHide == false)
                {
                    exportNames.Add(index, item.Title);
                    index = index + 1;
                }
            }
        }

        _ExportNames = exportNames;
    }
    public void BuildProfileModel()
    {
        _MerchantData = new Dictionary<string, DataTable>();

        if (_MerchantConfig != null && _MerchantConfig.Sections.Any())
        {
            foreach (var item in _MerchantConfig.Sections)
            {
                var hasPermission = true;
                item.Type = item.Type == 0 ? SectionType.Normal : item.Type;

                if (!string.IsNullOrEmpty(item.HasPermissionCode))
                {
                    hasPermission = false;
                    var permissions = item.HasPermissionCode.Split(',').ToList();
                    foreach (var permission in permissions)
                    {
                        if (!string.IsNullOrEmpty(permission))
                        {
                            if (GeneralFuncsLib.HasUserPermission(permission))
                                hasPermission = true;
                        }
                    }
                }

                if (hasPermission == false)
                {
                    item.IsHide = true;
                    continue;
                }

                //get data of each section
                if (!string.IsNullOrEmpty(item.SpaName))
                {
                    var data = GetAdditionalData(item.SpaName);
                    _MerchantData.Add(item.Id, data);
                }
                else if(item.IgnoreGetData == false)
                {
                    var data = GetSectionData(_MerchantConfig.MerchantSpaName, item.Id, _ClientConfig.DecryptColumns);
                    _MerchantData.Add(item.Id, data);
                }
            }
        }
    }
    private DataTable GetSectionData(string spaName, string sectionCode, string decryptColumns)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.AddLoggedInUserPrimaryUserID();
        parameters.Add(new FilterParameter("@MerchantNumber", _MerchantNumber, DbType.AnsiString));
        parameters.Add(new FilterParameter("@SectionCode", sectionCode, DbType.AnsiString));
        parameters.AddLanguageID();

        if (!string.IsNullOrEmpty(decryptColumns))
        {
            parameters.AddDecryptDataParams(decryptColumns);
        }
        else
        {
            parameters.AddDecryptDataParams("RoutingNumber");
        }
        var  data = WebServices.CsReportServices.GetReports(spaName, parameters);        
        return data;
    }
    private DataTable GetAdditionalData(string spaName)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParamsWithRecId();
        parameters.Add(new FilterParameter("@MerchantNumber", _MerchantNumber, DbType.AnsiString));
        var riskData = WebServices.CsReportServices.GetReports(spaName, parameters);
        return riskData;
    }
    private string GetProcessor()
    {
        var processor = string.Empty;

        if (_ClientConfig.HasMultiProcessor)
        {
            processor = MerchantProfileHelper.GetMerchantProcessor(_MerchantNumber);
        }

        if (string.IsNullOrEmpty(processor) || !Directory.Exists(MerchantProfileHelper.GetFolderPath(processor)))
            processor = _ClientConfig.DefaultProcessor;

        return processor;
    }
    private DataTable GetDataSouce(string source)
    {
        if (_MerchantData == null)
            return null;

        if (string.IsNullOrEmpty(source))
        {
            DataTable outputTable;
            _MerchantData.TryGetValue("MerchantInformation", out outputTable);

            return outputTable;
        }
        else
        {
            DataTable outputTable;
            _MerchantData.TryGetValue(source, out outputTable);

            return outputTable;
        }
    }
    private string GetColumnValueWithDefaultValue(string colName, string source)
    {
        var colValue = GetColumnValue(colName, source);

        if (string.IsNullOrEmpty(colValue))
            return WebSiteConstants.HTML_EM_DASH_ENCODE;

        return colValue;
    }
    private string GetColumnValue(string colName, string source)
    {
        var dataSouce = GetDataSouce(source);
        if (dataSouce == null || dataSouce.Rows.Count <= 0)
            return string.Empty;

        if (dataSouce.Columns.Contains(colName) == false)
        {
            return string.Empty;
        }

        DataRow dr = dataSouce.Rows[0];
        var columnValue = dr[colName] == DBNull.Value || dr[colName].ToString().IsNullOrEmpty() ? string.Empty : dr[colName].ToString();

        return columnValue;
    }
    public string LoadTemplate(string name, bool isExport = false)
    {
        var filePath = string.Format("~/App_Data/MerchantProfileTemplate/{0}/{1}/{2}", AsClientId, _MerchantConfig.Processor, name);

        if (isExport)
            filePath = string.Format("~/App_Data/MerchantProfileTemplate/{0}/{1}/{2}/{3}", AsClientId, _MerchantConfig.Processor, "ExportTemplate", name);

        var physicalFilePath = HttpContext.Current.Server.MapPath(filePath);
        if (File.Exists(physicalFilePath))
        {
            var key = physicalFilePath.Replace(@"\", "_").Replace(" ", "_");

            if (HttpRuntime.Cache[key] == null)
            {
                var fileCotent = File.ReadAllText(physicalFilePath);
                HttpRuntime.Cache.Insert(key, fileCotent, new System.Web.Caching.CacheDependency(physicalFilePath));
            }

            return HttpRuntime.Cache[key] as string;
        }
        return string.Empty;
    }
    public List<TemplateConfig> GetTemplateKey(string content)
    {
        var result = new List<TemplateConfig>();

        if (string.IsNullOrEmpty(content))
            return result;

        var permissionKeys = MerchantProfileUtils.GetPermissionKey(content, "<!--PERMISSION_START-->", "<!--PERMISSION_END-->");
        if (permissionKeys != null && permissionKeys.Any())
        {
            foreach (var item in permissionKeys)
            {
                if (!string.IsNullOrEmpty(item.Source))
                {
                    var hasPermission = GeneralFuncsLib.HasUserPermission(item.Source);

                    if (hasPermission == false)
                    {
                        item.Key = string.Empty;
                        item.Value = string.Empty;
                    }

                    result.Add(item);
                }
            }
        }

        var riskReport_Extend = MerchantProfileUtils.GetPermissionKey(content, "<!--RISK_REPORT_EXTEND_START-->", "<!--RISK_REPORT_EXTEND_END-->");
        if (riskReport_Extend != null && riskReport_Extend.Any())
        {
            foreach (var item in riskReport_Extend)
            {
                if (!string.IsNullOrEmpty(item.Source))
                {
                    var permision = item.Source.ToEnum(WebSiteEnums.Filter_Extend.GauthenticateCode);
                    var hasPermission = GeneralFuncsLib.RiskReportExtend(permision);

                    if (hasPermission == false)
                    {
                        item.Key = string.Empty;
                        item.Value = string.Empty;
                    }

                    result.Add(item);
                }
            }
        }

        var loopKeys = MerchantProfileUtils.GetLoopKey(content);
        if (loopKeys != null && loopKeys.Count > 0)
            result.AddRange(loopKeys);

        MatchCollection templateKeys;
        templateKeys = MerchantProfileUtils.GetAllBetween(content, "{{", "}}");

        if (templateKeys == null || templateKeys.Count == 0)
            return result;

        foreach (Match m in templateKeys)
        {
            var originKey = m.Value;
            var value = m.Groups[1].Value;

            var items = value.Split(':').ToList();
            if (items != null && items.Any())
            {
                var config = new TemplateConfig()
                {
                    OriginalKey = originKey,
                    Value = originKey
                };

                if (items[0].Equals("res"))
                    config.Type = TemplateConfigType.Resource;
                else if (items[0].Equals("data"))
                    config.Type = TemplateConfigType.Data;

                if (items.Count > 1 && !string.IsNullOrEmpty(items[1]))
                {
                    var data = items[1];
                    var itemData = data.Split('|').ToList();
                    if (itemData != null && itemData.Any())
                    {
                        //key
                        config.Key = itemData[0];

                        //format
                        if (itemData.Count > 1 && !string.IsNullOrEmpty(itemData[1]))
                        {
                            config.FormatType = itemData[1];
                        }
                        else
                        {
                            config.FormatType = "Text";
                        }

                        //source
                        if (itemData.Count > 2 && !string.IsNullOrEmpty(itemData[2]))
                        {
                            config.Source = itemData[2];
                        }
                        else
                        {
                            config.Source = "MerchantInformation";
                        }

                        //additional data
                        if (itemData.Count > 3 && !string.IsNullOrEmpty(itemData[3]))
                        {
                            config.AdditionData = itemData[3];
                        }
                    }

                    var existItem = result.FirstOrDefault(x => x.Equals(originKey));
                    if (existItem == null)
                        result.Add(config);
                }
            }
        }

        return result;
    }
    #region template action

    #endregion
}