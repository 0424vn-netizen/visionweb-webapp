using System;
using System.Collections.Generic;
using System.Linq;
using AS.Common.DBManager;
using System.Data;
using System.Web;
using Newtonsoft.Json;
using AS.Core.Common.Utilities;
using System.Web.UI;
using System.IO;
using Telerik.Web.UI;

public partial class UserControls_UxAdvancedFilter : GlobalUserControl
{
    private const string QuoteCharEncode = "&apos";

    public AdvancedFilterConfig AdvancedFilterConfig { get; set; }
    public FilterPageEnums FilterPage { get; set; }
    public string ApplyButtonText
    {
        get
        {
            if (IsGenerate)
                return GetGlobalResourceObject("AdvancedFilterResource", "btnGenerateReport").ToString();
            return GetGlobalResourceObject("AdvancedFilterResource", "btnApply").ToString();
        }
    }

    public bool IsGenerate
    {
        get
        {
            return FilterPage == FilterPageEnums.PortfolioStatistics;
        }
    }
    public bool IsBindData { get; set; }

    public int LimitGenerateStatisticsReport
    {
        get
        {
            int result = 0;
            if (GeneralFuncsLib.GetDataOfExtendedSetting("LimitGenerateStatisticsReport") != null)
            {
                Int32.TryParse(GeneralFuncsLib.GetDataOfExtendedSetting("LimitGenerateStatisticsReport"), out result);
            }
            return result;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack || IsBindData)
        {
            AdvancedFilterConfig = GetAdvancedFilterConfig();
            uxSavedFilter.FilterSearchs = GetSavedFilterSearch((int)FilterPage);
            uxRecentFilter.FilterSearchs = GetRecentFilterSearch((int)FilterPage);
            GetDateAvailableFilter();
            ProcessItems();
        }
    }

    #region Public method

    public static ObjectResult SaveFilter(int pageId, string nameFilter, string filterContent)
    {
        AdvancedFilterInfo savedFilterInfo = new AdvancedFilterInfo()
        {
            FilterPage = pageId,
            FilterName = nameFilter,
            FilterContent = filterContent,
            IsRecent = false
        };

        return InsertFilter(savedFilterInfo);
    }

    public static string UpdateFilterById(int pageId, long id, string name, string filterContent)
    {
        AdvancedFilterInfo savedFilterInfo = new AdvancedFilterInfo()
        {
            FilterPage = pageId,
            FilterSearchID = id,
            FilterName = name,
            FilterContent = filterContent
        };

        return UpdateFilter(savedFilterInfo); ;
    }

    public static string DeleteFilterById(long id)
    {
        string message = string.Empty;

        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AdvancedFilterSearchID", id, DbType.Int32));

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_DeleteAdvancedFilterSearch_ByAdvancedFilterSearchID", parameters, out parameterOut);

        return message;
    }

    public static string GenerateSavedFilterList(int pageId, string filterSection)
    {
        bool isRecent = filterSection == FilterTypeEnums.RecentFilter.ToString();
        if (isRecent)
        {
            return GenerateRecentFilterList(pageId);
        }

        string uxControl = "UserControls/RiskHtml/SaveFilterList.ascx";
        using (Page page = new Page())
        {
            UserControls_RiskHtml_SaveFilterList userControl = (UserControls_RiskHtml_SaveFilterList)page.LoadControl(uxControl);
            userControl.FilterSearchs = GetSavedFilterSearch(pageId);
            page.Controls.Add(userControl);
            using (StringWriter writer = new StringWriter())
            {
                page.Controls.Add(userControl);
                HttpContext.Current.Server.Execute(page, writer, false);
                return writer.ToString();
            }
        }
    }

    public static ObjectResult SaveAppliedFilter(int pageId, long id, string filterContent, string filterSection, bool isGenerate)
    {
        if (string.IsNullOrEmpty(filterContent))
            return new ObjectResult { Id = id.ToString(), ErrorMessage = "Filter Content is empty" }; ;

        string messageCode = string.Empty;
        if (id == 0)
        {
            // general new recent filter and save applied filter
            AdvancedFilterInfo savedFilterInfo = new AdvancedFilterInfo()
            {
                FilterPage = pageId,
                FilterName = string.Format("Filter Applied ({0})", DateTime.Now.ToString("MM/dd/yy hh:mm:ss tt")),
                FilterContent = filterContent,
                IsRecent = true,
                IsGenerate = isGenerate
            };

            var result = InsertFilter(savedFilterInfo);

            return new ObjectResult { Id = result.Id, ErrorMessage = result.ErrorMessage };
        }
        else
        {
            var result = new ObjectResult { Id = id.ToString() };
            int filterType = (int)(FilterTypeEnums)Enum.Parse(typeof(FilterTypeEnums), filterSection);
            // Rule only apply for Generate Business
            if ((FilterTypeEnums)Enum.Parse(typeof(FilterTypeEnums), filterSection) == FilterTypeEnums.RecentFilter && isGenerate)
            {
                result = InsertAppliedFilter(id, isGenerate, filterType);
            }

            // if filter is recent filter then not save applied
            if ((FilterTypeEnums)Enum.Parse(typeof(FilterTypeEnums), filterSection) == FilterTypeEnums.SavedFilter)
            {
                result = InsertAppliedFilter(id, isGenerate, filterType);
            }

            return new ObjectResult { Id = result.Id, ErrorMessage = result.ErrorMessage };
        }
    }

    public static string GenerateItemSource(int page, string type, string key, string subValue)
    {
        string htmlControl = string.Empty;
        string htmlAutocomplete = string.Empty;
        string item = string.Empty;
        string itemList = string.Empty;
        InputTypeEnums typeItem = (InputTypeEnums)Enum.Parse(typeof(InputTypeEnums), type);
        switch (typeItem)
        {
            case InputTypeEnums.MultiSelection:
                item = "<div value='{0}' keyword='{0}' data-display-value='{1}'>"
                        + "<div class='checkbox'>"
                        + "<label>"
                         + "<input type='checkbox' name='{0}' value='{0}' display-value='{1}' />"
                          + "<span>{2}</span>"
                         + "</label>"
                        + "</div>"
                        + "</div>";
                break;
            case InputTypeEnums.SingleSelection:
                item = "<div value='{0}' keyword='{0}' data-display-value='{1}'>"
                        + "<label>{2}</label>"
                        + "</div>";
                break;
            case InputTypeEnums.MultiChoose:
                htmlControl += "<select multiple='multiple' id='" + key + "' class='select-multi-choose' data-placeholder=' '>";
                item = "<option value='{0}' display='{2}'>{1}</option>";
                break;
            case InputTypeEnums.OrderBy:
                htmlControl = "<select id='ComboboxID' name='ComboboxID'>";
                item = "<option value='{0}'>{1}</option>";
                break;
            case InputTypeEnums.AutoComplete:
                htmlControl = "<div class='chosen-container chosen-container-multi chosen-with-drop chosen-container-active'>"
                  + "<ul class='chosen-choices' id='autocompleteID'>"
                  + "<li class='search-field'>"
                  + "<input type='text' id='inputValue' autocomplete='off'>"
                  + "</li>"
                  + "</ul>"
                  + "</div>"
                  + "<div class='no-result hide' id='autoCompleteNoResult'>"
                  + "</div>";
                break;
        }

        if (typeItem == InputTypeEnums.AutoComplete) return htmlControl;

        var data = GetDataSourceItem(key, subValue, string.Empty, string.Empty);

        List<object> jsonList = new List<object>();
        if (data != null && data.Rows.Count > 0)
        {
            foreach (DataRow row in data.Rows)
            {
                string dataItem = string.IsNullOrEmpty(row["DataText"].ToString()) ? "&nbsp;" : row["DataText"].ToString();
                string dataKey = row["DataKey"].ToString();
                string dataDisplay = row["DataDisplay"].ToString();
                string realDataItem = dataItem;
                if (!string.IsNullOrEmpty(dataItem) && dataItem.Contains("'") && typeItem != InputTypeEnums.MultiChoose)
                {
                    dataItem = dataItem.Replace("'", QuoteCharEncode);
                    dataKey = dataKey.Replace("'", "");
                }

                if (typeItem == InputTypeEnums.MultiSelection)
                {
                    jsonList.Add(new
                    {
                        DataKey = dataKey,
                        DataItem = dataItem,
                        DataDisplay = dataDisplay
                    });
                }
                else if (typeItem == InputTypeEnums.MultiChoose || typeItem == InputTypeEnums.OrderBy)
                {
                    htmlControl += string.Format(item, dataKey, dataItem, dataDisplay);
                }
                else
                {
                    itemList += string.Format(item, dataKey, dataItem, realDataItem);
                }
            }
        }

        if (typeItem == InputTypeEnums.MultiSelection)
        {
            itemList = JsonConvert.SerializeObject(jsonList);
        }

        return typeItem == InputTypeEnums.MultiChoose || typeItem == InputTypeEnums.OrderBy ? htmlControl += "</select>" : itemList;
    }

    public static List<FilterDataItem> GetDataAutoComplete(int page, string type, string key, string subValue, string filterText, string ignoreValue)
    {
        List<FilterDataItem> data = new List<FilterDataItem>();
        var result = GetDataSourceItem(key, subValue, filterText, ignoreValue);
        if (result != null && result.Rows.Count > 0)
        {
            foreach (DataRow item in result.Rows)
            {
                data.Add(new FilterDataItem
                {
                    Key = item["DataKey"].ToString(),
                    Value = item["DataText"].ToString(),
                    DisplayValue = item["DataDisplay"].ToString(),
                });
            }
        }
        return data;
    }

    public static AdvancedFilterInfo GetInfoFilterItem(int pageId, long id, bool isRecent)
    {
        int filterType = isRecent ? (int)FilterTypeEnums.RecentFilter : (int)FilterTypeEnums.SavedFilter;
        AdvancedFilterInfo filter = new AdvancedFilterInfo();
        DataTable result = GetFilterInfoById(id, filterType);
        if (result != null && result.Rows.Count > 0)
        {
            var data = result.Rows[0];
            filter = new AdvancedFilterInfo
            {
                FilterSearchID = id,
                FilterName = data["FilterName"].ToString(),
                FilterItems = DeserializeXMLToFilterItems(data["FilterContent"].ToString(), pageId, id, filterType)
            };
        }

        return filter;
    }

    #endregion

    #region Private Method

    private static ObjectResult InsertFilter(AdvancedFilterInfo filter)
    {
        string message = string.Empty;
        string id = string.Empty;
        string filterContent = SerializeFilterItemsToXML(JsonConvert.DeserializeObject<List<FilterItem>>(filter.FilterContent), filter.FilterPage);

        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AdvancedFilterSearchPage", filter.FilterPage, DbType.Int32));
        parameters.Add(new FilterParameter("@FilterName", filter.FilterName, DbType.String));
        parameters.Add(new FilterParameter("@FilterContent", filterContent, DbType.Xml));
        parameters.Add(new FilterParameter("@IsRecent", filter.IsRecent, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsGenerateReport", filter.IsGenerate, DbType.Boolean));

        parameters.Add(new FilterParameter("@AdvancedFilterSearchID", null, DbType.Int64, true));
        parameters.Add(new FilterParameter("@AdvancedFilterSearchApplyID", null, DbType.Int64, true));
        parameters.Add(new FilterParameter("@MessageCode", null, DbType.Int64, true));

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_InsertAdvancedFilterSearch", parameters, out parameterOut);

        object msgVal = parameterOut.FindFilterParameterByName("@MessageCode", true).ParameterValue;
        object filterSearchID = parameterOut.FindFilterParameterByName("@AdvancedFilterSearchID", true).ParameterValue;
        object filterApplySearchID = parameterOut.FindFilterParameterByName("@AdvancedFilterSearchApplyID", true).ParameterValue;

        if (filterSearchID != null && !filter.IsRecent)
        {
            id = filterSearchID.ToString();
        }

        if (filterApplySearchID != null && filter.IsRecent)
        {
            id = filterApplySearchID.ToString();
        }

        if (msgVal != null)
            message = msgVal.ToString();


        return new ObjectResult { Id = id, ErrorMessage = message };
    }

    private static string UpdateFilter(AdvancedFilterInfo filter)
    {
        string message = string.Empty;
        string filterContent = string.Empty;
        string name = !string.IsNullOrEmpty(filter.FilterName) ? filter.FilterName : null;
        filterContent = !string.IsNullOrEmpty(filter.FilterContent) ? SerializeFilterItemsToXML(JsonConvert.DeserializeObject<List<FilterItem>>(filter.FilterContent), filter.FilterPage) : null;

        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AdvancedFilterSearchID", filter.FilterSearchID, DbType.Int32));
        parameters.Add(new FilterParameter("@FilterName", name, DbType.String));
        parameters.Add(new FilterParameter("@FilterContent", filterContent, DbType.Xml));
        parameters.Add(new FilterParameter("@IsRecent", filter.IsRecent, DbType.Boolean));

        parameters.Add(new FilterParameter("@MessageCode", string.Empty, DbType.Int64, true));

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_UpdateAdvancedFilterSearch_ByAdvancedFilterSearchID", parameters, out parameterOut);

        object msgVal = parameterOut.FindFilterParameterByName("@MessageCode", true).ParameterValue;
        if (msgVal != null)
            message = msgVal.ToString();

        return message;
    }

    private AdvancedFilterConfig GetAdvancedFilterConfig()
    {
        AdvancedFilterConfig filterConfig = new AdvancedFilterConfig();
        filterConfig = (AdvancedFilterConfig)GeneralFuncsLib.ConvertXMLToObject(GetFilterList(FilterPage), typeof(AdvancedFilterConfig));
        filterConfig.FilterPage = FilterPage;
        // Ordered by alphabet
        if (filterConfig != null && filterConfig.KeywordItems.IsNotNullData())
        {
            filterConfig.KeywordItems = filterConfig.KeywordItems.OrderBy(x => x.Display).ToList();
            filterConfig.KeywordItems.ForEach(m =>
            {
                m.ValueItems = m.ValueItems.OrderBy(x => x.Display).ToList();
            });
        }

        RiskSessionManager.AdvancedFilterConfigs = filterConfig;

        return filterConfig;
    }

    private string GetFilterList(FilterPageEnums filter)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@AdvancedFilterSearchPage", (int)filter, DbType.Int32));

        var data = WebServices.RiskServices.GetReports("spa_REF_RM_ADVF_Get_Item", parameters);
        string result = string.Empty;
        if (data != null && data.Rows.Count > 0)
        {
            foreach (DataRow row in data.Rows)
            {
                result += row[0].ToString();
            }
        }

        return result;
    }

    //private string GetFilterList(FilterPageEnums filter)
    //{
    //    string xmlFilePath = Server.MapPath("~/App_Data/AdvancedFilterTest.xml");
    //    return VisionWebIOExtensions.ReadTextFile(xmlFilePath).ToString();
    //}

    private static List<AdvancedFilterInfo> GetSavedFilterSearch(int pageFilter)
    {
        List<AdvancedFilterInfo> lstSavedFilter = new List<AdvancedFilterInfo>();
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AdvancedFilterSearchPage", pageFilter, DbType.Int32));

        var data = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAdvancedFilterSearches", parameters);
        if (data != null && data.Rows.Count > 0)
        {
            foreach (DataRow row in data.Rows)
            {
                if (!row["IsRecent"].ToBoolean())
                {
                    AdvancedFilterInfo filter = new AdvancedFilterInfo
                    {
                        FilterSearchID = row["AdvancedFilterSearchID"].ToLong(),
                        FilterName = row["FilterName"].ToString(),
                    };

                    lstSavedFilter.Add(filter);
                }
            }
        }

        return lstSavedFilter;
    }

    private static List<AdvancedFilterInfo> GetRecentFilterSearch(int pageFilter)
    {
        List<AdvancedFilterInfo> lstRecentFilter = new List<AdvancedFilterInfo>();
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AdvancedFilterSearchPage", pageFilter, DbType.Int32));

        var data = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAppliedAdvancedFilterSearches", parameters);
        if (data != null && data.Rows.Count > 0)
        {
            foreach (DataRow row in data.Rows)
            {
                AdvancedFilterInfo filter = new AdvancedFilterInfo
                {
                    FilterSearchID = row["AdvancedFilterSearchApplyID"].ToLong(),
                    FilterName = row["FilterName"].ToString(),
                };

                lstRecentFilter.Add(filter);
            }
        }

        return lstRecentFilter;
    }

    private static string GenerateRecentFilterList(int pageId)
    {
        string uxControl = "UserControls/RiskHtml/RecentFilterList.ascx";
        using (Page page = new Page())
        {
            UserControls_RiskHtml_RecentFilterList userControl = (UserControls_RiskHtml_RecentFilterList)page.LoadControl(uxControl);
            userControl.FilterSearchs = GetRecentFilterSearch(pageId);
            page.Controls.Add(userControl);
            using (StringWriter writer = new StringWriter())
            {
                page.Controls.Add(userControl);
                HttpContext.Current.Server.Execute(page, writer, false);
                return writer.ToString();
            }
        }
    }

    private static string SerializeFilterItemsToXML(List<FilterItem> filterItems, int pageFilter)
    {
        string result = string.Empty;
        if (filterItems.IsNotNullData() && filterItems.Any())
        {
            AdvancedFilterConfig config = RiskSessionManager.AdvancedFilterConfigs;
            if (config != null)
            {
                foreach (FilterItem filterItem in filterItems)
                {
                    KeywordItem configItem = config.KeywordItems.FirstOrDefault(m => m.Key == filterItem.Key);

                    if (filterItem.SubFilterItems != null && filterItem.SubFilterItems.Count > 0)
                    {
                        for(int i = 0; i < filterItem.SubFilterItems.Count; i++)
                        {
                            var subItem = filterItem.SubFilterItems[i];
                            KeywordItem configSubItem = configItem.KeywordItems.FirstOrDefault(m => m.Key == subItem.Key);
                            if (configSubItem == null)
                            {
                                filterItem.SubFilterItems.RemoveAt(i);
                            }
                            else
                            {
                                ProcessFilterItem(configSubItem, subItem);
                            }
                        }
                    }

                    ProcessFilterItem(configItem, filterItem);
                }


            }

            return General.SerializeObjectToXML<List<FilterItem>>(filterItems, true);
        }

        return result;
    }

    private static void ProcessFilterItem(KeywordItem configItem, FilterItem filterItem)
    {
        if (configItem != null)
        {
            filterItem.Operator = configItem.Operator;
            if (configItem.InputType == InputTypeEnums.RangeRadio)
            {
                string[] data = filterItem.Value.Split('-');
                filterItem.Operator = (OperatorEnums)Enum.Parse(typeof(OperatorEnums), data[0]);
                filterItem.Value = data[1];
                filterItem.Value2 = data.Length == 3 ? data[2] : string.Empty;
            }
            else if (configItem.InputType == InputTypeEnums.DatePicker)
            {
                string[] data = filterItem.Value.Split('-');
                filterItem.Value = data[1];
                filterItem.Value2 = data.Length == 3 ? data[2] : string.Empty;
            }
        }
    }

    private static List<FilterItem> DeserializeXMLToFilterItems(string filterContent, int pageId, long filterId, int filterType)
    {
        if (string.IsNullOrEmpty(filterContent))
            return new List<FilterItem>();

        List<FilterItem> result = General.DeserializeXMLToObject<List<FilterItem>>(filterContent);
        if (result != null && result.Count > 0)
        {
            AdvancedFilterConfig config = RiskSessionManager.AdvancedFilterConfigs;
            // Check SubFilter
            var key = result[0].Key;
            KeywordItem configItem = config.KeywordItems.FirstOrDefault(m => m.Key == result[0].Key);
            if (configItem != null && configItem.InputType == InputTypeEnums.SubFilter)
            {
                result[0].InputType = configItem.InputType;
                result[0].DisplayName = configItem.Display;
                result[0].ParentName = configItem.ParentName;
                result[0].DisplayValue = configItem.Display;

                ProcessFilterItem(result[0].SubFilterItems, filterId, filterType, key);
            }
            else
            {
                ProcessFilterItem(result, filterId, filterType, null);
            }
        }

        return result;
    }

    private static void ProcessFilterItem(List<FilterItem> result, long filterId, int filterType, string parentKey)
    {
        AdvancedFilterConfig config = RiskSessionManager.AdvancedFilterConfigs;
        List<FilterDataItem> data = GetDataFitlerItems(filterId, filterType);

        KeywordItem configParent = new KeywordItem();
        if (!string.IsNullOrEmpty(parentKey))
            configParent = config.KeywordItems.FirstOrDefault(m => m.Key == parentKey);

        foreach (var item in result)
        {
            KeywordItem configItem;
            if (!string.IsNullOrEmpty(parentKey))
                configItem = configParent.KeywordItems.FirstOrDefault(m => m.Key == item.Key);
            else
                configItem = config.KeywordItems.FirstOrDefault(m => m.Key == item.Key);

            if (configItem != null)
            {
                item.DisplayName = configItem.Display;
                item.InputType = configItem.InputType;
                item.ParentKey = parentKey;
            }

            switch (item.InputType)
            {
                case InputTypeEnums.RangeRadio:
                    if (item.Operator == OperatorEnums.Between)
                    {
                        item.Value = item.Operator.ToString() + "-" + item.Value + "-" + item.Value2;
                    }
                    else
                    {
                        item.Value = item.Operator.ToString() + "-" + item.Value;
                    }
                    break;
                case InputTypeEnums.SingleSelection:
                    if (config != null)
                    {
                        if (configItem != null && configItem.ValueItems != null && configItem.ValueItems.Count > 0)
                        {
                            Item valItem = configItem.ValueItems.FirstOrDefault(x => x.Value == item.Value);
                            if (valItem != null)
                                item.DisplayValue = valItem.Display;
                        }
                        else
                        {
                            FilterDataItem dataItem = data.FirstOrDefault(x => x.Key == item.Key);
                            if (dataItem != null) item.DisplayValue = dataItem.DisplayValue;
                        }
                    }
                    break;
                case InputTypeEnums.MultiSelection:
                    var dataItems = data.Where(x => x.Key == item.Key);
                    if (dataItems != null)
                    {
                        item.DisplayValue = string.Join(", ", dataItems.Select(x => x.DisplayValue).ToArray());
                        item.DisplayValueReal = string.Join("#char#", dataItems.Select(x => x.DisplayValue).ToArray());
                    }
                    break;
                case InputTypeEnums.MultiChoose:
                    var datas = data.Where(x => x.Key == item.Key);
                    if (datas != null)
                    {
                        item.DisplayValue = string.Join(", ", datas.Select(x => x.DisplayValue).ToArray());
                    }
                    break;
                case InputTypeEnums.AutoComplete:
                    var dataAuto = data.Where(x => x.Key == item.Key);
                    if (dataAuto != null)
                    {
                        item.DisplayValue = string.Join(", ", dataAuto.Select(x => x.DisplayValue).ToArray());
                        item.DisplayAutoComplete = string.Join("#;", dataAuto.Select(x => x.DisplayText).ToArray());
                    }
                    break;
                case InputTypeEnums.DatePicker:
                    if (!string.IsNullOrEmpty(item.Value2))
                    {
                        item.Value = "DateRange" + "-" + item.Value + "-" + item.Value2;
                    }
                    else
                    {
                        item.Value = "Daily" + "-" + item.Value;
                    }
                    break;
                case InputTypeEnums.OrderBy:
                    FilterDataItem itemOrder = data.FirstOrDefault(x => x.Key == item.Key && x.Value == item.Value);
                    if (itemOrder != null) item.DisplayValue = itemOrder.DisplayValue;
                    break;
                default:
                    item.DisplayValue = item.Value;
                    break;
            }
        }
    }

    private static ObjectResult InsertAppliedFilter(long id, bool isGenerate, int filterType)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AdvancedFilterSearchID", id, DbType.Int32));
        parameters.Add(new FilterParameter("@AdvancedFilterType", filterType, DbType.Int32));
        parameters.Add(new FilterParameter("@IsGenerateReport", isGenerate, DbType.Boolean));

        parameters.Add(new FilterParameter("@AdvancedFilterSearchApplyID", null, DbType.Int64, true));
        parameters.Add(new FilterParameter("@MessageCode", null, DbType.Int64, true));

        int result = WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_InsertAppliedAdvancedFilterSearches", parameters, out parameterOut);
        object filterApplySearchID = parameterOut.FindFilterParameterByName("@AdvancedFilterSearchApplyID", true).ParameterValue;
        object msgCode = parameterOut.FindFilterParameterByName("@MessageCode", true).ParameterValue;

        return new ObjectResult { Id = filterApplySearchID.ToString(), ErrorMessage = msgCode.ToString() };
    }

    private static DataTable GetDataSourceItem(string categoryKey, string subValue, string filterText, string ignoreValue)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@CategoryKey", categoryKey, DbType.String));
        parameters.Add(new FilterParameter("@FilterText", filterText, DbType.String));
        parameters.Add(new FilterParameter("@IgnoreValue", ignoreValue, DbType.String));
        parameters.Add(new FilterParameter("@SubValue", subValue, DbType.String));

        return WebServices.RiskServices.GetReports("spa_REF_RM_ADVF_Get_ItemValue", parameters);
    }

    private void GetDateAvailableFilter()
    {
        if (RiskSessionManager.DateAvailableFilter == null)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

            var data = WebServices.RiskServices.GetReports("spa_RM_MCF_GetBeginReportDate", parameters);
            if (data != null && data.Rows.Count > 0)
            {
                RiskSessionManager.DateAvailableFilter = data.Rows[0]["ReportDate"].ToDateTime();
            }
        }
    }

    private static List<FilterDataItem> GetDataFitlerItems(long id, int filterType)
    {
        List<FilterDataItem> data = new List<FilterDataItem>();
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@AdvancedFilterID", id, DbType.Int64));
        parameters.Add(new FilterParameter("@AdvancedFilterType", filterType, DbType.Int16));

        DataTable result = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAdvancedFilterItems", parameters);
        if (result != null && result.Rows.Count > 0)
        {
            foreach (DataRow item in result.Rows)
            {
                data.Add(new FilterDataItem
                {
                    Key = item["FilterKey"].ToString(),
                    Value = item["Value"].ToString(),
                    DisplayValue = item["Display"].ToString(),
                    DisplayText = item["DataText"].ToString(),
                });
            }
        }

        return data;
    }

    private static DataTable GetSavedFilterInfoById(int page, long id)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AdvancedFilterSearchID", id, DbType.Int32));
        parameters.Add(new FilterParameter("@AdvancedFilterSearchPage", page, DbType.Int32));

        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetFilterSearches", parameters);
    }

    private static DataTable GetFilterInfoById(long id, int filterSection)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AdvancedFilterID", id, DbType.Int32));
        parameters.Add(new FilterParameter("@AdvancedFilterType", filterSection, DbType.Int32));

        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetAdvancedFilterSearchDetail", parameters);
    }

    private void ProcessItems()
    {
        List<KeywordItem> textList = AdvancedFilterConfig.KeywordItems.Where(m => m.InputType == InputTypeEnums.Text).ToList();
        List<KeywordItem> singleList = AdvancedFilterConfig.KeywordItems.Where(m => m.InputType == InputTypeEnums.SingleSelection).ToList();
        List<KeywordItem> multiSelectionList = AdvancedFilterConfig.KeywordItems.Where(m => m.InputType == InputTypeEnums.MultiSelection).ToList();
        List<KeywordItem> multiChooseList = AdvancedFilterConfig.KeywordItems.Where(m => m.InputType == InputTypeEnums.MultiChoose).ToList();
        List<KeywordItem> rangeRadioList = AdvancedFilterConfig.KeywordItems.Where(m => m.InputType == InputTypeEnums.RangeRadio).ToList();
        List<KeywordItem> subFilterList = AdvancedFilterConfig.KeywordItems.Where(m => m.InputType == InputTypeEnums.SubFilter).ToList();
        List<KeywordItem> orderByList = AdvancedFilterConfig.KeywordItems.Where(m => m.InputType == InputTypeEnums.OrderBy).ToList();
        List<KeywordItem> datePickerList = AdvancedFilterConfig.KeywordItems.Where(m => m.InputType == InputTypeEnums.DatePicker).ToList();
        List<KeywordItem> autoCompleteList = AdvancedFilterConfig.KeywordItems.Where(m => m.InputType == InputTypeEnums.AutoComplete).ToList();

        var parents = AdvancedFilterConfig.KeywordItems != null ? AdvancedFilterConfig.KeywordItems.Where(m => m.IsParent).ToList() : null;
        if (subFilterList != null && subFilterList.Count > 0)
        {
            foreach (var child in subFilterList)
            {
                List<KeywordItem> childText = ProcessChildItem(child, InputTypeEnums.Text);
                textList.AddRange(childText);

                List<KeywordItem> childSingle = ProcessChildItem(child, InputTypeEnums.SingleSelection);
                singleList.AddRange(childSingle);

                List<KeywordItem> childMultiSelection = ProcessChildItem(child, InputTypeEnums.MultiSelection);
                multiSelectionList.AddRange(childMultiSelection);

                List<KeywordItem> childMultiChoose = ProcessChildItem(child, InputTypeEnums.MultiChoose);
                multiChooseList.AddRange(childMultiChoose);

                List<KeywordItem> childRangeRadio = ProcessChildItem(child, InputTypeEnums.RangeRadio);
                rangeRadioList.AddRange(childRangeRadio);

                List<KeywordItem> childOrderBy = ProcessChildItem(child, InputTypeEnums.OrderBy);
                orderByList.AddRange(childOrderBy);

                List<KeywordItem> childDatePicker = ProcessChildItem(child, InputTypeEnums.DatePicker);
                datePickerList.AddRange(childDatePicker);

                List<KeywordItem> childAutoComplete = ProcessChildItem(child, InputTypeEnums.AutoComplete);
                autoCompleteList.AddRange(childAutoComplete);
            }
        }

        uxTextControl.KeywordItems = textList;
        uxSingleSelection.KeywordItems = singleList;
        uxMultiSelectionControl.KeywordItems = multiSelectionList;
        uxMultiChooseControl.KeywordItems = multiChooseList;
        uxRangeRadioControl.KeywordItems = rangeRadioList;
        uxSubFilterControl.KeywordItems = subFilterList;
        uxOrderByControl.KeywordItems = orderByList;
        uxDatePickerControl.KeywordItems = datePickerList;
        uxAutocompleteControl.KeywordItems = autoCompleteList;
    }

    private List<KeywordItem> ProcessChildItem(KeywordItem childItem, InputTypeEnums type)
    {
        List<KeywordItem> items = childItem.KeywordItems.Where(m => m.InputType == type).ToList();
        if (items.Count > 0)
        {
            foreach (var item in items)
            {
                item.ParentKey = childItem.Key;
            }
        }

        return items;
    }
    #endregion
}