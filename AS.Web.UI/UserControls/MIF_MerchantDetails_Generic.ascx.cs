using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using AS.Common.Formater;

namespace As.VisionWeb.Web
{
    public partial class MerchantDetailsGenericControl : ExportMultiSections
    {
        #region Constants
        // Export file
        private readonly string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
        #endregion Constants

        #region Fields

        private bool _isCaseManagement = false;

        private Dictionary<string, DataTable> _MerchantData;

        #endregion Fields

        #region Properties

        public bool GetIsCaseManagement()
        {
            return _isCaseManagement;
        }
        public void SetIsCaseManagement(bool value)
        {
            _isCaseManagement = value;
        }
        protected string MerchantNumber
        {
            get
            {
                return (string)ViewState["MerchantNumber"];
            }
            set
            {
                ViewState["MerchantNumber"] = value;
            }
        }
        public string Processor { get; set; }
        protected EngineBuilder _Engine;

        #endregion Properties

        #region Methods

        public void Rebind()
        {
            StartEngine();
            if (_MerchantData != null && _MerchantData.Values.Count > 0)
            {
                //render template
                merchantInfoDetail.InnerHtml = BuildTemplate();
            }
        }
        private void StartEngine()
        {
            MerchantNumber = ReportPage.ReportFilter.CurrentValue.Value;
            var asClientID = SessionManager.CurrentUser.ASClient;
            _Engine = new EngineBuilder(asClientID, MerchantNumber);
            ExportSectionNames = _Engine._ExportNames;
            _MerchantData = _Engine._MerchantData;
            Processor = _Engine._Processor;
        }
        protected override Dictionary<int, Func<string>> GetListFunctions()
        {
            Dictionary<int, Func<string>> exportFucntions = new Dictionary<int, Func<string>>();
            int index = 0;
            StartEngine();
            _MerchantData = _Engine._MerchantData;

            if (_MerchantData != null && _MerchantData.Values.Count > 0)
            {
                foreach (var item in _Engine._MerchantConfig.Sections)
                {
                    if (!item.IsHide)
                    {
                        exportFucntions.Add(index, () => { return ExportSectionData(_Engine, item); });
                        index += 1;
                    }
                }
            }

            return exportFucntions;
        }
        protected string ExportSectionData(EngineBuilder engine, Section config)
        {
            if (config == null)
                return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, "EmptySheet", "xls", string.Empty);

            var masterTemplate = engine.LoadTemplate("master-layout.html", true);
            var excelContent = string.Empty;
            var template = engine.LoadTemplate(config.ExportTemplate, true);

            if (!string.IsNullOrEmpty(template))
            {
                var keys = engine.GetTemplateKey(template);
                excelContent = RenderTemplate(keys, template);
                masterTemplate = masterTemplate.Replace("[Content]", excelContent);
            }
            else
            {
                masterTemplate = masterTemplate.Replace("[Content]", excelContent);
            }

            string excelFileName = GeneralFuncsLib.GetFileName(config.Title);
            excelFileName = excelFileName.Length > 30 ? excelFileName.Substring(0, 30) : excelFileName;
            return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", masterTemplate);
        }

        #endregion Methods

        #region render template    
        private string BuildTemplate(bool isExport = false)
        {
            var masterTemplate = _Engine.LoadTemplate("master-layout.html", isExport);

            if (_Engine._MerchantConfig.Sections != null && _Engine._MerchantConfig.Sections.Any())
            {
                foreach (var item in _Engine._MerchantConfig.Sections)
                {
                    var template = string.Empty;

                    if (!string.IsNullOrEmpty(item.Title))
                    {
                        var title = GetResourceDefault(item.Title);
                        item.Title = title != null ? title.ToString() : item.Title;
                    }

                    if (!string.IsNullOrEmpty(item.TemplateFile) && !item.IsHide)
                    {
                        var templateName = isExport ? item.ExportTemplate : item.TemplateFile;
                        template = _Engine.LoadTemplate(templateName, isExport);

                        if (!string.IsNullOrEmpty(template))
                        {
                            var keys = _Engine.GetTemplateKey(template);
                            template = RenderTemplate(keys, template);
                        }
                    }

                    masterTemplate = masterTemplate.Replace(item.TemplateKey, template);
                }
            }
            return masterTemplate;
        }
        private string RenderTemplate(List<TemplateConfig> keys, string template, DataRow dataSource = null)
        {
            if (string.IsNullOrEmpty(template))
                return string.Empty;

            if (keys != null && keys.Any())
            {
                foreach (var item in keys)
                {
                    StringBuilder textReplace = new StringBuilder();
                    if (item.Type == TemplateConfigType.Resource || item.Type == TemplateConfigType.Data)
                    {
                        textReplace.Append(BindTemplate(item, dataSource));
                        template = template.Replace(item.Value, textReplace.ToString());
                    }
                    else
                    {
                        if (item.Type == TemplateConfigType.Loop)
                        {
                            var templateKeys = _Engine.GetTemplateKey(item.Value);
                            var source = GetDataSouce(item.Source);
                            if (source != null && source.Rows.Count > 0)
                            {
                                foreach (DataRow row in source.Rows)
                                {
                                    var rowTemplate = item.Value;

                                    foreach (var subKey in templateKeys)
                                    {
                                        rowTemplate = rowTemplate.Replace(subKey.Value, BindTemplate(subKey, row));
                                    }

                                    textReplace.Append(rowTemplate);
                                }
                            }

                            template = template.Replace(item.OriginalKey, textReplace.ToString());
                        }

                        if (item.Type == TemplateConfigType.Permission)
                        {
                            if (string.IsNullOrEmpty(item.Key) && string.IsNullOrEmpty(item.Value))
                            {
                                textReplace = new StringBuilder();
                            }
                            else
                            {
                                var templateKeys = _Engine.GetTemplateKey(item.Value);
                                var permissionTemplate = item.Value;

                                if (templateKeys != null && templateKeys.Any())
                                {
                                    foreach (var subItem in templateKeys)
                                    {
                                        var bindValue = BindTemplate(subItem);
                                        permissionTemplate = permissionTemplate.Replace(subItem.Value, bindValue);
                                    }

                                    textReplace.Append(permissionTemplate);
                                }
                            }
                            template = template.Replace(item.OriginalKey, textReplace.ToString());
                        }
                    }
                }
            }

            return template;
        }

        private string BindTemplate(TemplateConfig item, DataRow dataSource = null)
        {
            var result = string.Empty;
            if (item.Type == TemplateConfigType.Resource)
            {
                var resText = GetResourceDefault(item.Key);
                result = resText != null ? resText.ToString() : string.Empty;
            }

            if (item.Type == TemplateConfigType.Data)
            {
                DataRow itemSource = dataSource;

                if (itemSource == null)
                {
                    var data = GetDataSouce(item.Source);
                    if (data != null && data.Rows.Count > 0)
                        itemSource = data.Rows[0];
                }

                result = BuildPropertiesValue(item, itemSource);
            }
            return result;
        }
        private string BuildPropertiesValue(TemplateConfig item, DataRow dataSource)
        {
            var textReplace = string.Empty;
            switch (item.FormatType)
            {
                case "Text":
                    textReplace = GetColumnValueWithDefaultValue(item.Key, dataSource);
                    break;

                case "RawText":
                    textReplace = GetColumnValue(item.Key, dataSource);
                    break;

                case "WebSite":
                    textReplace = BuildWebsite(item.Key, dataSource);
                    break;

                case "Phone":
                    {
                        var phone = GetColumnValue(item.Key, dataSource);
                        textReplace = string.IsNullOrEmpty(phone) ? WebSiteConstants.HTML_EM_DASH_ENCODE : FormatData.FormatPhoneNumber(phone);
                        break;
                    }
                case "SIC":
                    {
                        var code = GetColumnValue("SICCode", dataSource);
                        var description = GetColumnValue("SICCodeDesc", dataSource);
                        textReplace = FormatTemplateHelper.FormatSIC(code, description);
                        break;
                    }

                case "Address":
                    {
                        var address1 = GetColumnValue("Address1", dataSource);
                        var address2 = GetColumnValue("Address2", dataSource);
                        var address3 = GetColumnValue("Address3", dataSource);
                        var city = GetColumnValue("City", dataSource);
                        var state = GetColumnValue("State", dataSource);
                        var zip = GetColumnValue("Zip", dataSource);
                        textReplace = FormatTemplateHelper.BindAddress(address1, address2, address3, city, state, zip);
                        break;
                    }
                case "CorporateAddress":
                    {
                        var address = GetColumnValue("CorporateAddress", dataSource);
                        var city = GetColumnValue("CorporateCity", dataSource);
                        var state = GetColumnValue("CorporateState", dataSource);
                        var zip = GetColumnValue("CorporateZip", dataSource);
                        textReplace = FormatTemplateHelper.BindAddress(address, string.Empty, string.Empty, city, state, zip);
                        break;
                    }

                case "BatchLink":
                    textReplace = BuildLastBatchActivityLink(item.Key, dataSource);
                    break;

                case "TaxView":
                    {
                        var data = GetColumnValue(item.Key, dataSource);
                        if (string.IsNullOrEmpty(data))
                        {
                            textReplace = WebSiteConstants.HTML_EM_DASH_ENCODE;
                        }
                        else
                        {
                            if (GeneralFuncsLib.HasUserPermission(WebSiteConstants.SEC_PERMISSION_TAX_ID))
                            {
                                textReplace = data;
                            }
                            else
                            {
                                textReplace = GetColumnValueWithDefaultValue("Partial" + item.Key, dataSource);
                            }
                        }

                        break;
                    }
                case "Date":
                    {
                        var date = GetColumnValue(item.Key, dataSource);
                        textReplace = FormatTemplateHelper.FormatDate(date);
                        break;
                    }
                case "Currency":
                    {
                        var data = GetColumnValue(item.Key, dataSource);
                        if (!string.IsNullOrEmpty(data))
                        {
                            decimal outputData;
                            if (decimal.TryParse(data, out outputData))
                                textReplace = FormatTemplateHelper.FormatCurrency(outputData);
                            else
                                textReplace = data;
                        }
                        else
                        {
                            textReplace = WebSiteConstants.HTML_EM_DASH_ENCODE;
                        }

                        break;
                    }
                case "Percent":
                    {
                        var data = GetColumnValue(item.Key, dataSource);
                        if (!string.IsNullOrEmpty(data))
                        {
                            decimal outputData;
                            if (decimal.TryParse(data, out outputData))
                                textReplace = FormatTemplateHelper.FormatPercent(outputData);
                            else
                                textReplace = data;
                        }
                        else
                        {
                            textReplace = WebSiteConstants.HTML_EM_DASH_ENCODE;
                        }
                        break;
                    }
                case "SiteAcessLink":
                    {
                        textReplace = BuildSiteAcessLink(item.Key, dataSource);
                        break;
                    }
                case "SiteAcessLinkExport":
                    {
                        if (!GeneralFuncsLib.HasMSProductEnvironment())
                            textReplace = GetResourceDefault("NotAvailable");
                        else
                            textReplace = GetColumnValue(item.Key, dataSource);
                        break;
                    }
                case "DDAView":
                    {
                        var data = GetColumnValue(item.Key, dataSource);
                        if (string.IsNullOrEmpty(data))
                            textReplace = WebSiteConstants.HTML_EM_DASH_ENCODE;
                        else
                        {
                            if (GeneralFuncsLib.HasUserPermission(WebSiteConstants.SEC_PERMISSION_DDA)
                                || GeneralFuncsLib.HasUserPermission(WebSiteConstants.SEC_PERMISSION_DDA_MS))
                            {
                                textReplace = data;
                            }
                            else
                            {
                                textReplace = GetColumnValueWithDefaultValue("Partial" + item.Key, dataSource);
                            }
                        }
                        break;
                    }
                case "PartialData":
                    {
                        var fullData = GetColumnValueWithDefaultValue(item.Key, dataSource);
                        var partialData = GetColumnValueWithDefaultValue("Partial" + item.Key, dataSource);
                        textReplace = MerchantProfileHelper.GetRoutingNumberWithoutDecrypt(fullData, partialData);
                        break;
                    }
                case "Hierarchy":
                    {
                        textReplace = BuildHierarchyLink(item.Key, dataSource, item.AdditionData);
                        break;
                    }
                case "ChainLink":
                    {
                        textReplace = BuildChainLink(item.Key, dataSource, item.AdditionData);
                        break;
                    }
                case "ViewRiskReportLink":
                    {
                        if (GeneralFuncsLib.HasUserPermission("RskRP") || GeneralFuncsLib.HasUserPermission("MSRskRP"))
                        {
                            var link = string.Format("<a class=\"link-back font-size-default\" href=\"risk_MCF/rm_MCF_RiskReport.aspx\">{0}</a>", GetResourceDefault(item.Key));
                            textReplace = string.Format("<h1 class=\"dark-blue pull-right\">{0}</h1>", link);
                        }
                        break;
                    }
                case "SiteJump":
                    {
                        textReplace = BuildSiteJumpLink(item.Key, dataSource);
                        break;
                    }
                case "AgentLink":
                    {
                        textReplace = BuildAgentLink(item.Key, dataSource, item.AdditionData);
                        break;
                    }
                case "SecondaryAccessChainLink":
                    {
                        textReplace = BuildSecondaryAccessChainLink(item.Key, dataSource, item.AdditionData);
                        break;
                    }
                case "PricingToolTip":
                    {
                        textReplace = PricingToolTip(item.Key, dataSource);
                        break;
                    }
                case "MC_ICA_AVS":
                    {
                        textReplace = Format_MC_ICA_AVS(item.Key, dataSource);
                        break;
                    }
                case "ToolTipTitle":
                    {
                        var data = GetColumnValue(item.Key, dataSource);
                        var titleToolTip = GetColumnValue(item.AdditionData, dataSource);
                        textReplace = !string.IsNullOrEmpty(data) ? titleToolTip : string.Empty;
                        break;
                    }
                case "NoRecords":
                    {
                        var resource = GetDataSouce(item.Source);

                        if (resource == null || resource.Rows.Count == 0)
                        {
                            var noRecords = GetResourceDefault(item.Key);
                            textReplace = string.Format("<div class='NoRecords'>{0}</div>", noRecords);
                        }

                        break;
                    }
                case "Resource":
                    {
                        var resourceKey = GetColumnValueWithDefaultValue(item.Key, dataSource);
                        textReplace = GetResourceDefault(resourceKey);
                        break;
                    }
                case "AccountStatus":
                    {
                        var data = GetColumnValue(item.Key, dataSource);
                        var resourceKey = !string.IsNullOrEmpty(data) && data.ToSafeString().ToBoolean() ? item.Key : item.AdditionData;
                        textReplace = GetResourceDefault(resourceKey);
                        break;
                    }
                case "AccountEffectedDate":
                    {
                        var date = GetColumnValue(item.Key, dataSource);
                        var additionData = GetColumnValue(item.AdditionData, dataSource);
                        if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(additionData))
                        {
                            textReplace = GeneralFuncsLib.NA_VALUE;
                        }
                        else
                        {
                            textReplace = FormatTemplateHelper.FormatDate(date);
                        }
                        break;
                    }
                case "OptStatusAction":
                    textReplace = BuildMerchantOptStatus(item.Key, dataSource);
                    break;
                case "OptStatusExport":
                    textReplace = BuildMerchantOptStatusExport(item.Key, dataSource);
                    break;
                default:
                    textReplace = GetColumnValueWithDefaultValue(item.Key, dataSource);
                    break;
            }

            return textReplace;
        }
        private string GetColumnValueWithDefaultValue(string colName, DataRow source)
        {
            var colValue = GetColumnValue(colName, source);

            if (string.IsNullOrEmpty(colValue))
                return WebSiteConstants.HTML_EM_DASH_ENCODE;

            return colValue;
        }
        private string GetColumnValue(string colName, DataRow source)
        {
            var columnValue = string.Empty;
            try
            {
                columnValue = source[colName] == DBNull.Value || source[colName].ToString().IsNullOrEmpty() ? string.Empty : source[colName].ToString();
            }
            catch (Exception)
            {
                return columnValue;
            }

            return columnValue;
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
        private string BuildWebsite(string key, DataRow soure)
        {
            var link = WebSiteConstants.HTML_EM_DASH_ENCODE;
            var data = GetColumnValue(key, soure);
            if (!string.IsNullOrEmpty(data))
            {
                var website = data.ToLower().Contains("http") ? data : string.Format("//{0}", data);
                link = string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", website, data);
            }

            return link;
        }
        private string BuildLastBatchActivityLink(string key, DataRow soure)
        {
            var link = WebSiteConstants.HTML_EM_DASH_ENCODE;
            var data = GetColumnValue(key, soure);
            if (!string.IsNullOrEmpty(data))
            {
                var lastBatchActivity = FormatTemplateHelper.FormatDate(data);
                var merchantNumber = MerchantNumber;
                DateTime tempDate;
                var paramDate = "0";
                if (DateTime.TryParse(lastBatchActivity, out tempDate))
                {
                    paramDate = tempDate.Ticks.ToString();
                }

                link = GeneralFuncsLib.BuildLastBatchHistoryLink(Page, paramDate, merchantNumber, lastBatchActivity);
            }

            return link;
        }
        private string BuildSiteAcessLink(string key, DataRow soure)
        {
            if (!GeneralFuncsLib.HasMSProductEnvironment())
                return GetResourceDefault("NotAvailable");

            if (GetIsCaseManagement())
            {
                return GetResourceDefault("NotAvailable");
            }
            else
            {
                var data = GetColumnValue(key, soure);

                if (MerchantProfileHelper.SetVisibleSiteAccessLink(data, Page))
                {
                    var merchantNumber = MerchantNumber;
                    var optedInText = data.Equals(Resources.Template.OptedIn, StringComparison.OrdinalIgnoreCase)
                            ? MerchantProfileHelper.OPTED_IN_VALUE : MerchantProfileHelper.OPTED_OUT_VALUE;
                    var merchantEmail = GetColumnValue("Email", soure);
                    var url = MerchantProfileHelper.BuildURLForSiteAccessInMIF(Page, merchantNumber, optedInText, merchantEmail);
                    var link = string.Format("<input type=\"button\" value=\"{0}\" onclick=\"{1}\">", FormatTemplateHelper.GetStatusText(data), url);
                    var content = "<div class=\"row\">";
                    content += string.Format("<div class=\"col-md-6\"><div class=\"mt-1x\">{0}</div></div>", data);
                    content += string.Format("<div class=\"col-md-6 text-right\">{0}</div>", link);
                    content += "</div>";

                    return content;
                }
                else
                {
                    return data;
                }
            }
        }
        private string BuildMerchantOptStatus(string key, DataRow soure)
        {
            var data = GetColumnValue(key, soure);
            if (!GeneralFuncsLib.HasMSProductEnvironment()
                || GetIsCaseManagement()
                || string.IsNullOrEmpty(data))
                return GetResourceDefault("NotAvailable");

            var merchantStatusText = GetColumnValue("SiteAccess", soure);

            if (GeneralFuncsLib.HasOptInOutPermission(Page)
                && (data == MerchantProfileHelper.OPTED_IN_VALUE || data == MerchantProfileHelper.OPTED_OUT_VALUE))
            {
                var merchantNumber = MerchantNumber;
                var merchantEmail = GetColumnValue("Email", soure);
                var actionText = data == MerchantProfileHelper.OPTED_IN_VALUE ? Resources.Template.MIF_MerchantProfile_OptIn : Resources.Template.MIF_MerchantProfile_OptOut;
                var url = MerchantProfileHelper.BuildURLForSiteAccessInMIF(Page, merchantNumber, data, merchantEmail);
                var link = string.Format("<input type=\"button\" value=\"{0}\" onclick=\"{1}\">", actionText, url);
                var content = "<div class=\"row\">";
                content += string.Format("<div class=\"col-md-6\"><div class=\"mt-1x\">{0}</div></div>", merchantStatusText);
                content += string.Format("<div class=\"col-md-6 text-right\">{0}</div>", link);
                content += "</div>";

                return content;
            }
            else
            {
                return merchantStatusText;
            }
        }
        private string BuildMerchantOptStatusExport(string key, DataRow soure)
        {
            var data = GetColumnValue(key, soure);
            if (!GeneralFuncsLib.HasMSProductEnvironment()
                || GetIsCaseManagement()
                || string.IsNullOrEmpty(data))
                return GetResourceDefault("NotAvailable");

            return GetColumnValue("SiteAccess", soure);
        }
        private string BuildSiteJumpLink(string key, DataRow soure)
        {
            var data = GetColumnValue(key, soure);
            if (string.IsNullOrEmpty(data))
            {
                data = WebSiteConstants.HTML_EM_DASH_ENCODE;
                return data;
            }

            var hasPermission = GeneralFuncsLib.HasUserPermission(WebSiteConstants.SEC_PERMISSION_JSACCESS);
            if (!hasPermission)
            {
                return data;
            }

            var sitJumpText = GetResourceDefault("SiteAccessText");
            var encUserID = CryptorServices.Current.EncryptText(data);
            var button = string.Format("<input type=\"button\" value=\"{0}\" onclick=\"siteJumpClick(this,'{1}')\" class=\"btn btn-default\">", sitJumpText, encUserID);
            var content = string.Format("<div class=\"opt-in-out-text mt-1x\">{0}</div>", data);
            content += string.Format("<div class=\"pull-right opt-in-out-button\">{0}</div>", button);

            return content;
        }
        private string BuildHierarchyLink(string hierarchyCode, DataRow soure, string hierarchyName = "")
        {
            var hierarchyConfig = !string.IsNullOrEmpty(hierarchyName) ? hierarchyName : hierarchyCode;
            var hierarchyValue = GetColumnValue(hierarchyConfig, soure);
            var displayText = GetColumnValue(hierarchyConfig + "Text", soure);

            if (MerchantProfileHelper.CheckHierarchy(hierarchyCode))
            {
                if (!string.IsNullOrEmpty(hierarchyValue))
                {
                    var url = MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(hierarchyCode, hierarchyValue);
                    var link = string.Format("<a class=\"pointer\" onclick=\"{0}\">{1}</a>", url, !string.IsNullOrEmpty(displayText) ? displayText : hierarchyValue);
                    return link;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(displayText))
                    return displayText;

                return GetColumnValueWithDefaultValue(!string.IsNullOrEmpty(hierarchyName) ? hierarchyName : hierarchyCode, soure);
            }

            return WebSiteConstants.HTML_EM_DASH_ENCODE;
        }
        private string BuildAgentLink(string hierarchyCode, DataRow soure, string agentName = "")
        {
            if (MerchantProfileHelper.CheckHierarchy(hierarchyCode) || MerchantProfileHelper.CheckHierarchy(agentName))
            {
                var agentValue = GetColumnValue(hierarchyCode, soure);
                if (!string.IsNullOrEmpty(agentValue))
                {
                    var url = MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(!string.IsNullOrEmpty(agentName) ? agentName : hierarchyCode, agentValue);
                    var link = string.Format("<a class=\"pointer\" onclick=\"{0}\">{1}</a>", url, agentValue);
                    return link;
                }
            }
            else
            {
                return GetColumnValueWithDefaultValue(hierarchyCode, soure);
            }

            return WebSiteConstants.HTML_EM_DASH_ENCODE;
        }
        private string BuildChainLink(string hierarchyCode, DataRow soure, string additionData)
        {
            var chainValue = GetColumnValue(hierarchyCode, soure);
            var returnLink = string.Empty;

            bool isAddEditChainWithAdditionData = !string.IsNullOrEmpty(additionData) && additionData.ToLower().Equals("isshowchainlink");
            //not exist in chain: show create new chain link
            if (string.IsNullOrEmpty(chainValue)
                && GeneralFuncsLib.HasUserPermission("AddEditChain")
                && (MerchantProfileHelper.SiteAccessIsOptInOut(GetColumnValue("SiteAccess", soure)) || isAddEditChainWithAdditionData))
            {
                returnLink = BuildCreateNewChainLink(soure);
            }
            else
            {
                if (MerchantProfileHelper.CheckHierarchy(hierarchyCode))
                {
                    returnLink += BuildHierarchyLink(hierarchyCode, soure);
                }
                else
                {
                    returnLink += chainValue;
                }
            }

            //add to exist chain link
            if (MerchantProfileUtils.CheckPermissionAddEditChain(GetColumnValue("SiteAccess", soure), isAddEditChainWithAdditionData))
            {
                returnLink += GetResourceDefault("OR");
                returnLink += CreateAddExistChain(chainValue, soure);
            }

            return returnLink;
        }
        private string BuildCreateNewChainLink(DataRow soure)
        {
            var merchantNumber = GetColumnValue("MerchantNumber", soure);
            var createNewChainLinkUrl = "CreateNewChainModal.aspx?" + Page.BuildSecureQueryString("MerchNum=" + merchantNumber);
            var onClickEvent = string.Format("return ShowPopupModal('{0}','auto');", createNewChainLinkUrl);
            var createNewChainLink = string.Format("<a class=\"pointer\" onclick=\"{0}\">{1}</a>", onClickEvent, GetResourceDefault("CreateNewChain"));
            return createNewChainLink;
        }
        private string CreateAddExistChain(string chainValue, DataRow soure)
        {
            var merchantNumber = GetColumnValue("MerchantNumber", soure);
            var editChainLinkUrl = "ModifyMerchantChainModal.aspx?" + Page.BuildSecureQueryString("MerchNum=" + merchantNumber + "&Chain=" + chainValue);
            var editOnClickEvent = string.Format("return ShowPopupModal('{0}','auto');", editChainLinkUrl);
            var editChainLink = string.Format("<a class=\"pointer\" onclick=\"{0}\">{1}</a>", editOnClickEvent, GetResourceDefault("AddToExistingChain"));
            return editChainLink;
        }
        private string BuildSecondaryAccessChainLink(string hierarchyCode, DataRow source, string additionalData)
        {
            var chainValue = GetColumnValue(additionalData, source);
            var returnLink = string.Empty;

            //not exist in chain: show create new secondary chain link
            if (string.IsNullOrEmpty(chainValue)
                && GeneralFuncsLib.HasUserPermission("AddEditAccessChain")
                && MerchantProfileHelper.SiteAccessIsOptInOut(GetColumnValue("SiteAccess", source)))
            {
                returnLink = BuildCreateNewSecondaryAccessChainLink(source);
            }
            else
            {
                if (MerchantProfileHelper.CheckHierarchy(hierarchyCode))
                {
                    returnLink += BuildHierarchyLink(hierarchyCode, source, additionalData);
                }
                else
                {
                    returnLink += chainValue;
                }
            }

            //add to exist chain link
            if (MerchantProfileUtils.CheckPermissionAddEditSecondaryAccessChain(GetColumnValue("SiteAccess", source)))
            {
                returnLink += GetResourceDefault("OR");
                returnLink += CreateAddExistSecondaryAccessChain(chainValue, source);
            }

            return returnLink;
        }
        private string BuildCreateNewSecondaryAccessChainLink(DataRow source)
        {
            var merchantNumber = GetColumnValue("MerchantNumber", source);
            var createNewSecondaryChainLinkUrl = "CreateNewSecondaryAccessChainModal.aspx?" + Page.BuildSecureQueryString("MerchNum=" + merchantNumber);
            var onClickEvent = string.Format("return ShowPopupModal('{0}','auto');", createNewSecondaryChainLinkUrl);
            var createNewSecondaryChainLink = string.Format("<a class=\"pointer\" onclick=\"{0}\">{1}</a>", onClickEvent, GetResourceDefault("CHAIN_Create_New_Secondary"));
            return createNewSecondaryChainLink;
        }
        private string CreateAddExistSecondaryAccessChain(string chainValue, DataRow source)
        {
            var merchantNumber = GetColumnValue("MerchantNumber", source);
            var editSecondaryAccessChainLinkUrl = "ModifyMerchantSecondaryAccessChainModal.aspx?" + Page.BuildSecureQueryString("MerchNum=" + merchantNumber + "&AccessChain=" + chainValue);
            var editOnClickEvent = string.Format("return ShowPopupModal('{0}','auto');", editSecondaryAccessChainLinkUrl);
            var editSecondaryAccessChainLink = string.Format("<a class=\"pointer\" onclick=\"{0}\">{1}</a>", editOnClickEvent, GetResourceDefault("CHAIN_Add_Existing_Secondary"));
            return editSecondaryAccessChainLink;
        }
        protected string PricingToolTip(string key, DataRow source)
        {
            string value = GetColumnValue(key, source);
            if (value == "N" && key == "TransIntegrityFee")
                return GetResourceDefault("DoNotPassTIF");
            else if (value == "Y" && key == "TransIntegrityFee")
                return GetResourceDefault("PassTIF");
            else if (value == string.Empty && (key == "TransIntegrityFee" || key == "NPFFeeFlag"))
                return string.Empty;
            else if (value == "N" && key == "NPFFeeFlag")
                return GetResourceDefault("DoNotPassNPF");
            else if (value == "Y" && key == "NPFFeeFlag")
                return GetResourceDefault("PassNPF");
            else
                return string.Empty;
        }
        protected string Format_MC_ICA_AVS(string key, DataRow source)
        {
            string value = GetColumnValue(key, source);
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            else
            {
                var result = string.Format("{0:#,0.000000;#,0.000000}", value);
                var temp = Convert.ToDecimal(value);
                if (temp < 0)
                    result = string.Format("<span class=\"negative\">({0})</span>", result.Replace("-", ""));
                return result;
            }
        }
        public static string GetSiteJumpUrl(string encUser)
        {
            if (string.IsNullOrEmpty(encUser))
                return string.Empty;

            string userId = CryptorServices.Current.DecryptText(encUser);

            if (string.IsNullOrEmpty(userId))
                return string.Empty;

            var hasPermission = GeneralFuncsLib.HasUserPermission(WebSiteConstants.SEC_PERMISSION_JSACCESS);
            if (!hasPermission)
                return string.Empty;

            var url = MerchantProfileHelper.CreateSiteJumpLink(userId);
            return url;
        }
        private string GetResourceDefault(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            var resource = GetLocalResourceObject(key);
            if (resource != null)
                return resource.ToString();

            return string.Empty;
        }
        #endregion
    }
}