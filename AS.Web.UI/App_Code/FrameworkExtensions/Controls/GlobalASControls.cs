using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Web.UI.Controls;
using System.IO;
using AS.Common.DBManager;
using Telerik.Web.UI;
using System.Collections.Generic;
using AS.Controls.Telerik;

/// <summary>
/// Summary description for GlobalASControls
/// </summary>
namespace AS.Controls.Global
{
    public class Exporter : AS.Controls.UserControls.UxExport
    {
        public Exporter()
        {
            this.ShowWord = false;
            this.ShowPDF = true;

            this.ButtonClicking += Exporter_ButtonClicking;
        }

        void Exporter_ButtonClicking(object sender, EventArgs e)
        {
            if (this.Grid != null && this.Grid is ASGrid)
            {
                ((ASGrid)this.Grid).ReportTotalText = Resources.LanguageResource.AS_ASGrid_ReportTotal;
            }
        }
    }
    public class Validator : AS.Controls.Validators.Validator
    {
        public Validator()
        {
            MessageType = AS.Controls.Validators.MessageType.Inline;
        }
    }
    public class ValidatorMessage : AS.Controls.Validators.ValidatorMessage
    {
    }
    public class ValidatorLabel : AS.Controls.Validators.ValidatorLabel
    {
        public ValidatorLabel()
        {
            CssClass = "control-label";
        }
    }
    public class BasicValidationItem : AS.Controls.Validators.BasicValidationItem
    {
    }
    public class CustomValidationItem : AS.Controls.Validators.CustomValidationItem
    {
    }
    public class CompareValidationItem : AS.Controls.Validators.CompareValidationItem
    {
    }
    public class RegExValidationItem : AS.Controls.Validators.RegExValidationItem
    {
    }

    public class Container : ASContainer
    {
        public Container()
            : base()
        {
            this.TemplateName = "ascontainer_greyborder.tpl";
        }
    }
    public class MultiSelector : AS.Controls.Selector.MultiSelector
    {
        public MultiSelector()
        {
            this.FilterImageUrl = ResolveUrl("~/res/img/filter_ico.png");            
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            string selectorID = this.ID;
            RadContextMenu mnuFilter = (RadContextMenu)this.FindControl(selectorID + "_mnuFilter");

            if (mnuFilter != null)
            {
                foreach (RadMenuItem item in mnuFilter.Items)
                {
                    var result = string.Empty;
                    switch (item.Value)
                    {
                        case "NoFilter":
                            result = Resources.LanguageResource.AS_ASGrid_FilterMenu_NoFilter;
                            break;
                        case "Contains":
                            result = Resources.LanguageResource.AS_ASGrid_FilterMenu_Contains;
                            break;
                        case "DoesNotContain":
                            result = Resources.LanguageResource.AS_ASGrid_FilterMenu_DoesNotContain;
                            break;
                        case "StartsWith":
                            result = Resources.LanguageResource.AS_ASGrid_FilterMenu_StartsWith;
                            break;
                        case "EndsWith":
                            result = Resources.LanguageResource.AS_ASGrid_FilterMenu_EndsWith;
                            break;
                        case "EqualTo":
                            result = Resources.LanguageResource.AS_ASGrid_FilterMenu_EqualTo;
                            break;
                        case "NotEqualTo":
                            result = Resources.LanguageResource.AS_ASGrid_FilterMenu_NotEqualTo;
                            break;
                    }
                    item.Text = result;
                }
            }

            var url = this.Page.Request.Url.AbsolutePath;
            var segments = this.Page.Request.Url.Segments;

            if (segments != null && segments.Length > 0)
                url = segments[segments.Length - 1];

            var textLeft = (System.Web.UI.WebControls.TextBox)this.FindControl(selectorID + "_txtFilterLeft");
            var textRight = (System.Web.UI.WebControls.TextBox)this.FindControl(selectorID + "_txtFilterRight");
            var jsValidateFunction = string.Format("addValidationMultiSelectorFilter(this, event,'{0}');", url);
            
            if (textLeft != null)
            {
                textLeft.Attributes.Add("onblur", jsValidateFunction);
            }

            if (textRight != null)
            {
                textRight.Attributes.Add("onblur", jsValidateFunction);
            }
        }
    }
    public class MultiSelectorGrid : AS.Controls.Selector.MultiSelectorGrid
    {
    }
    public class MultiSelectorSimpleGrid : AS.Controls.Selector.MultiSelectorSimpleGrid
    {
    }


    public class MPSReportFilter : DropdownTelerikReportFilter
    {
        public string CustomTemplate { get; set; }

        private string _extendHierarchyMode;
        public string ExtendHierarchyMode
        {
            get
            {
                if (!string.IsNullOrEmpty(_extendHierarchyMode))
                {
                    return _extendHierarchyMode;
                }
                return string.Empty;
            }
            set
            {
                _extendHierarchyMode = value;
            }
        }

        public MPSReportFilter()
        {
            BackOneVisible = false;
            BackTopVisible = false;
            IsResetDateOptionOnClick = true;
            ButtonSubmitText = "Search";
            MainComboBox.Filter = RadComboBoxFilter.Contains;
            MainComboBox.MarkFirstMatch = true;
        }
        protected override void IntialControls()
        {
            base.IntialControls();
            // _btSubmit = new Button();

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.Page.ClientScript.RegisterStartupScript(GetType(), "handle_check_special_characters", "addCheckSpecialCharactersMPSReportFilter();", true);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.ButtonSubmitText = Resources.LanguageResource.AS_FilteringOptions_Search;
            this._rdDailyOption.Text = Resources.LanguageResource.AS_FilteringOptions_DateOption_Daily;
            this._rdMonthlyOption.Text = Resources.LanguageResource.AS_FilteringOptions_DateOption_Monthly;
            this._rdDateRangeOption.Text = Resources.LanguageResource.AS_FilteringOptions_DateOption_DateRange;
            if (GeneralFuncsLib.GetDataOfExtendedSetting("MultiLanguage").ToLower().Equals("true"))
            {
                string curCulture = GeneralFuncsLib.GetCurrentCulture();
                this._dpFromDate.Culture = new System.Globalization.CultureInfo(curCulture);
                this._dpFromDate.DateInput.DateFormat = WebSiteConstants.DATE_FORMAT;
                this._dpFromDate.Calendar.FastNavigationSettings.TodayButtonCaption = Resources.LanguageResource.RadDatePicker_TodayButtonCaption;// HttpContext.GetGlobalResourceObject("LanguageResource", "RadDatePicker_TodayButtonCaption").ToString();
                this._dpFromDate.Calendar.FastNavigationSettings.CancelButtonCaption = Resources.LanguageResource.RadDatePicker_CancelButtonCaption;//HttpContext.GetGlobalResourceObject("LanguageResource", "RadDatePicker_CancelButtonCaption").ToString();
                this._dpFromDate.Calendar.FastNavigationSettings.OkButtonCaption = Resources.LanguageResource.RadDatePicker_OkButtonCaption;//HttpContext.GetGlobalResourceObject("LanguageResource", "RadDatePicker_OkButtonCaption").ToString();
                this._dpToDate.Culture = new System.Globalization.CultureInfo(curCulture);
                this._dpToDate.DateInput.DateFormat = WebSiteConstants.DATE_FORMAT;
                this._dpToDate.Calendar.FastNavigationSettings.TodayButtonCaption = Resources.LanguageResource.RadDatePicker_TodayButtonCaption;//HttpContext.GetGlobalResourceObject("LanguageResource", "RadDatePicker_TodayButtonCaption").ToString();
                this._dpToDate.Calendar.FastNavigationSettings.CancelButtonCaption = Resources.LanguageResource.RadDatePicker_CancelButtonCaption;//HttpContext.GetGlobalResourceObject("LanguageResource", "RadDatePicker_CancelButtonCaption").ToString();
                this._dpToDate.Calendar.FastNavigationSettings.OkButtonCaption = Resources.LanguageResource.RadDatePicker_OkButtonCaption;//HttpContext.GetGlobalResourceObject("LanguageResource", "RadDatePicker_OkButtonCaption").ToString();
                this._dpFromDate.DatePopupButton.ToolTip = Resources.LanguageResource.uxReportDateDatePopupButtonToolTip; ;
                this._dpToDate.DatePopupButton.ToolTip = Resources.LanguageResource.uxReportDateDatePopupButtonToolTip; ;
            }
        }

        public class TextboxFilter : TextBoxHierarchyFilter
        {

            public TextboxFilter(
                string id,
                string mode,
                string text,
                string nextID,
                string preID,
                int minLength,
                int maxLength,
                string maxLengthMsg,
                string minLengthMsg,
                int groupOrder)
                : base(id, text, mode, nextID, preID, "rf_Textbox_DoValidateInput", groupOrder)
            {
                this.JV_DateTimeValidation = "rf_ValidateDateTime_Ext";

                _textbox.CssClass = "rf_TextBox";
                _textbox.Text = "";
                if (maxLength >= 0)
                {
                    _textbox.Attributes.Add("maxLength", maxLength.ToString());
                    _textbox.MaxLength = maxLength;
                    _maxLen = maxLength;
                }
                if (minLength >= 0)
                {
                    _textbox.Attributes.Add("minLength", minLength.ToString());
                    _minLen = minLength;
                }
                if (maxLengthMsg != null)
                {
                    _textbox.Attributes.Add("maxMsg", maxLengthMsg);
                }
                if (minLengthMsg != null)
                {
                    _textbox.Attributes.Add("minMsg", minLengthMsg);
                }

            }
            public TextboxFilter(
                string id,
                string mode,
                string text,
                string nextID,
                string preID,
                int minLength,
                int maxLength,
                string formEx,
                string maxLengthMsg,
                string minLengthMsg,
                string invalidFormatMsg,
                int groupOrder)
                : this(id, mode, text, nextID, preID, minLength, maxLength, maxLengthMsg, minLengthMsg, groupOrder)
            {

                if (formEx != null)
                {
                    _textbox.Attributes.Add("format", formEx);
                    _formatInput = formEx;
                }
                if (invalidFormatMsg != null)
                {
                    _textbox.Attributes.Add("invalidFormat", invalidFormatMsg);
                }

            }
            string _formatInput = "", _onlyInput = "", _exceptInput = "";
            int _maxLen = -1, _minLen = -1;
            public TextboxFilter(
                string id,
                string mode,
                string text,
                string nextID,
                string preID,
                int minLength,
                int maxLength,
                string validateEx,
                bool validInput,
                string maxLengthMsg,
                string minLengthMsg,
                string invalidInputMsg,
                int groupOrder)
                : this(id, mode, text, nextID, preID, minLength, maxLength, maxLengthMsg, minLengthMsg, groupOrder)
            {

                if (validateEx != null)
                {
                    if (validInput)
                    {
                        _textbox.Attributes.Add("only", validateEx);
                    }
                    else
                    {
                        _textbox.Attributes.Add("except", validateEx);
                    }
                }
                if (invalidInputMsg != null)
                {
                    _textbox.Attributes.Add("invalidMsg", invalidInputMsg);
                }
            }
            private bool Validate(string inputVal)
            {
                if (_minLen > 0)
                {
                    if (inputVal.Length < _minLen) return false;
                }
                if (_maxLen > 0)
                {
                    if (inputVal.Length > _maxLen) return false;
                }
                if (_formatInput != "" && _minLen > 0)
                {
                    return GeneralFuncsLib.IsValidWithRegularExpression(_formatInput, inputVal);
                }
                if (_onlyInput != "" && _minLen > 0)
                {
                    return GeneralFuncsLib.IsValidWithAllowCharaters(_onlyInput, inputVal);
                }
                if (_exceptInput != "" && _minLen > 0)
                {
                    return GeneralFuncsLib.IsValidWithNotAllowCharaters(_exceptInput, inputVal);
                }
                return true;
            }
            public override bool ValidateInput(string hiddenValue)
            {
                string inputVal = hiddenValue;
                if (!Validate(inputVal))
                {
                    return false;
                }
                inputVal = _textbox.Text;
                if (!Validate(inputVal))
                {
                    return false;
                }
                return base.ValidateInput(hiddenValue);
            }
            TextBox _textbox;
            protected override WebControl BuildInputControl()
            {
                _textbox = new TextBox();
                _textbox.Text = "";
                return _textbox;
            }
        }


        HierarchyFilterCollection CreateHierarchyFilter(string PageMode)
        {
            if (GeneralFuncsLib.HasExtendedSetting("FILTERING_OPTIONS_DATERANGE"))
            {
                this.DefaultDateRange.From = DateTime.Now.AddDays(-90);
                this.DefaultDateRange.To = DateTime.Now;
            }

            FilterParameterCollection parameters = new FilterParameterCollection();
            //get all hierarchy filter
            if (SessionManager.AllHierarchyFilter == null)
            {
                parameters.Clear();
                parameters.AddLoggedInUserReportingParams(true);
                parameters.AddLanguageID();
                SessionManager.AllHierarchyFilter = WebServices.RiskServices.GetReports("spa_GetAllHierarchyFilters", parameters);
            }

            //get full hierarchy Filter of login user
            if (SessionManager.HierarchyFilter == null)
            {
                parameters.Clear();
                parameters.AddLoggedInUserReportingParams(true);
                parameters.AddLanguageID();
                SessionManager.HierarchyFilter = WebServices.RiskServices.GetReports("spa_GetHierarchyFilter", parameters);
            }

            //get full extend hierarchy Filter of login user
            if (SessionManager.HierarchyFilterExtend == null)
            {
                parameters.Clear();
                parameters.AddLoggedInUserReportingParams(true);
                SessionManager.HierarchyFilterExtend = WebServices.RiskServices.GetReports("spa_cs_GetHierarchyFilterExtend", parameters);
            }

            //get hierarchy filter for merchant profile
            if (SessionManager.HierarchyFilterForMerchantProfile == null)
            {
                parameters.Clear();
                parameters.AddLoggedInUserReportingParams(true);
                parameters.Add(new FilterParameter("@PageMode", "MerchantProfile", DbType.String));
                parameters.AddLanguageID();
                SessionManager.HierarchyFilterForMerchantProfile = WebServices.RiskServices.GetReports("spa_GetHierarchyFilter", parameters);
            }

            if (SessionManager.HierarchyFilterDrillDown == null)
            {
                parameters.Clear();
                parameters.AddLoggedInUserReportingParams(true);
                parameters.AddLanguageID();
                SessionManager.HierarchyFilterDrillDown = WebServices.RiskServices.GetReports("spa_GetHierarchyFilterLevel", parameters);
            }
            DataTable info = new DataTable();
            if (PageMode == "MerchantProfile")
            {
                info = SessionManager.HierarchyFilterForMerchantProfile;
            }
            else
            {
                info = SessionManager.HierarchyFilter;
            }

            if (SessionManager.HierarchyFilterExtend != null)
            {
                DataTable tbHFExtend = SessionManager.HierarchyFilterExtend;
                var _temp = tbHFExtend.Select("Url = '" + HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath + "'");
                if (_temp != null && _temp.Count() > 0)
                {
                    DataTable currentHFExtend = _temp.CopyToDataTable();
                    if (currentHFExtend != null && currentHFExtend.Rows.Count > 0)
                    {
                        string extendHM = string.Empty;
                        foreach (DataRow row in currentHFExtend.Rows)
                        {
                            extendHM += row["HierarchyMode"].ToString() + ",";
                        }
                        if (!string.IsNullOrEmpty(extendHM))
                        {
                            ExtendHierarchyMode = extendHM.Remove(extendHM.LastIndexOf(','));
                        }
                    }
                }
            }

            HierarchyFilterCollection hierarchyFilters = new HierarchyFilterCollection();


            if (SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.Merchant)
            {
                DataTable tbTemp = info.Copy();
                if (tbTemp.Columns.Contains("IsExtend"))
                {
                    string[] arrExtendFilter = ExtendHierarchyMode.Split(',');
                    var tbt = tbTemp.Select("IsExtend = true");
                    if (tbt != null && tbt.Count() > 0)
                    {
                        DataTable tbExtend = tbt.CopyToDataTable();
                        foreach (DataRow row in tbExtend.Rows)
                        {
                            if (!arrExtendFilter.Contains(row["HierarchyMode"].ToString()))
                            {
                                UpdateNextAndPreviousHierarchy(tbTemp, row["HierarchyID"].ToString());
                                DataRow delRow = tbTemp.Select("HierarchyID = " + row["HierarchyID"].ToString()).FirstOrDefault();
                                if (delRow != null)
                                {
                                    tbTemp.Rows.Remove(delRow);
                                }
                            }
                        }
                    }
                }
                foreach (DataRow row in tbTemp.Rows)
                {
                    string id = row["HierarchyID"].ToString();
                    string mode = row["HierarchyMode"].ToString();
                    string text = row["HierarchyName"].ToString();
                    string nextID = row["NextHierarchyID"].ToString();
                    string preID = row["PreviousHierarchyID"].ToString();

                    // check if exist next or previous id of this hierarchy on this group
                    if (!tbTemp.CheckExistObject("HierarchyID", nextID))
                        nextID = string.Empty;
                    if (!tbTemp.CheckExistObject("HierarchyID", preID))
                        preID = string.Empty;
                    //end
                    int minLength = row["MinLength"] is DBNull ? 0 : int.Parse(row["MinLength"].ToString());
                    int maxLength = row["MaxLength"] is DBNull ? 0 : int.Parse(row["MaxLength"].ToString());
                    string validateEx = row["ValidateExpression"].ToString();
                    Boolean isSelectable = row["Selectable"] is DBNull ? true : row["Selectable"].ToBoolean();
                    String childOf = row["ChildOf"] is DBNull ? "" : row["ChildOf"].ToString();

                    string maxLengthMsg = row["MaxLenMsg"].ToString();
                    string minLengthMsg = row["MinLenMsg"].ToString();
                    string invalidInputMsg = row["InvalidMsg"].ToString();
                    int pos = row["PositionGroup"] is DBNull ? 0 : int.Parse(row["PositionGroup"].ToString());
                    IHierarchyFilter item = null;
                    if (row["ValidInput"] is DBNull)
                    {
                        item = new TextboxFilter(id, mode, text, nextID, preID, minLength, maxLength, validateEx, maxLengthMsg, minLengthMsg, invalidInputMsg, 1);

                    }
                    else
                    {
                        bool validInput = bool.Parse(row["ValidInput"].ToString());
                        item = new TextboxFilter(id, mode, text, nextID, preID, minLength, maxLength, validateEx, validInput, maxLengthMsg, minLengthMsg, invalidInputMsg, pos);

                        //item = new  DropDownHierarchyFilter("")
                    }

                    //45535 - FIS - Activation Report
                    if (row["ControlType"].IsNotNullData() && row["ControlType"].ToString() == "DropdownList")
                    {
                        List<RefDataForDropdownList> ds = new List<RefDataForDropdownList>();
                        string sessionStoreData = "ControlHierachyData_" + id;
                        if (HttpContext.Current.Session[sessionStoreData] == null)
                        {
                            ds = GetHierarchyData(id, string.Empty, false);
                            HttpContext.Current.Session[sessionStoreData] = ds;
                        }
                        else
                        {
                            ds = HttpContext.Current.Session[sessionStoreData] as List<RefDataForDropdownList>;
                        }
                        item = new DropDownHierarchyFilter(id, text, mode, nextID, preID, 1, ds, "Value", "Key");
                    }
                    // 46747 - [AW] - Reporting Development - FE
                    if (row["ControlType"].IsNotNullData() && row["ControlType"].ToString() == "Combobox")
                    {
                        string controlDataMode = row["ControlDataMode"].ToString();
                        List<RefDataForDropdownList> ds = new List<RefDataForDropdownList>();

                        if (controlDataMode.Equals("BindOnLoad", StringComparison.OrdinalIgnoreCase))
                        {
                            string sessionStoreData = "ControlHierachyData_" + id;
                            if (HttpContext.Current.Session[sessionStoreData] == null)
                            {
                                ds = GetHierarchyData(id, string.Empty);
                                HttpContext.Current.Session[sessionStoreData] = ds;
                            }
                            else
                            {
                                ds = HttpContext.Current.Session[sessionStoreData] as List<RefDataForDropdownList>;
                            }
                            item = new ComboboxHierarchyFilter(id, text, mode, nextID, preID, 1, ds, "Value", "Key");

                        }
                        else if (controlDataMode.Equals("BindOnTyping", StringComparison.OrdinalIgnoreCase))
                        {
                            string sessionStoreData = "ControlHierachyData_" + id;
                            string keyword = "ControlHierachy_" + id;
                            ds = new List<RefDataForDropdownList>();

                            if (HttpContext.Current.Session[sessionStoreData] == null)
                            {
                                HttpContext.Current.Session[sessionStoreData] = GetHierarchyData(id, string.Empty);
                            }
                            if (HttpRuntime.Cache[keyword] != null)
                            {
                                ds = HttpContext.Current.Session[sessionStoreData] as List<RefDataForDropdownList>;
                                ds = ds.Where(d => d.Value.Contains(HttpRuntime.Cache[keyword].ToString().Trim())).ToList();
                            }
                            if (SessionManager.CurrentReportFilter != null)
                            {
                                string hvalue = SessionManager.CurrentReportFilter.Value;
                                if (!string.IsNullOrEmpty(hvalue) && ds.Where(d => d.Key.Equals(hvalue)).Count() == 0)
                                {
                                    List<RefDataForDropdownList> dsnew = HttpContext.Current.Session[sessionStoreData] as List<RefDataForDropdownList>;
                                    RefDataForDropdownList hData = dsnew.Where(d => d.Key.Equals(hvalue)).FirstOrDefault();
                                    if (hData != null)
                                    {
                                        ds.Add(hData);
                                    }
                                }
                            }
                            item = new ComboboxHierarchyFilter(id, text, mode, nextID, preID, 1, ds, "Value", "Key");
                            ASRadComboBox combobox = ((WebControl)(item.InputControl)) as ASRadComboBox;
                            int limitLength = row["MinLength"] != DBNull.Value ? int.Parse(row["MinLength"].ToString()) : 0;
                            combobox.OnClientItemsRequesting = "OnClientItemsRequestingHierachy";
                            combobox.Attributes.Add("OnClientItemsRequestingLength", limitLength.ToString());
                            //if (SessionManager.CurrentReportFilter != null)
                            //{
                            //    string hvalue = SessionManager.CurrentReportFilter.Value;
                            //    if (SessionManager.CurrentReportFilter.ID == id)
                            //        combobox.SelectedValue = hvalue; 
                            //}
                            if (combobox != null)
                            {
                                combobox.ItemsRequested += (s, e) =>
                                {
                                    string filterValue = e.Text;
                                    if (filterValue.Length >= limitLength)
                                    {
                                        ds = HttpContext.Current.Session[sessionStoreData] as List<RefDataForDropdownList>;
                                        ds = ds.Where(d => d.Value.ToLower().Contains(filterValue.ToLower().Trim()) || d.Key.ToLower().Contains(filterValue.ToLower().Trim())).ToList();

                                        if (SessionManager.CurrentReportFilter != null)
                                        {
                                            string hvalue = SessionManager.CurrentReportFilter.Value;
                                            if (!string.IsNullOrEmpty(hvalue) && ds.Where(d => d.Key.Equals(hvalue)).Count() == 0)
                                            {
                                                List<RefDataForDropdownList> dsnew = HttpContext.Current.Session[sessionStoreData] as List<RefDataForDropdownList>;
                                                RefDataForDropdownList hData = dsnew.Where(d => d.Key.Equals(hvalue)).FirstOrDefault();
                                                if (hData != null)
                                                {
                                                    ds.Add(hData);
                                                }
                                            }
                                        }
                                        combobox.DataSource = ds;
                                        combobox.DataBind();
                                        if (SessionManager.CurrentReportFilter != null)
                                        {
                                            string hvalue = SessionManager.CurrentReportFilter.Value;
                                            if (SessionManager.CurrentReportFilter.ID == id)
                                                combobox.SelectedValue = hvalue;
                                        }

                                    }
                                };
                            }
                        }
                        else if (controlDataMode.Equals("BindOnSeletedChange", StringComparison.OrdinalIgnoreCase))
                        {
                            string sessionStoreData = "ControlHierachyData_" + id;
                            string keyword = "ControlHierachy_" + id;
                            ds = new List<RefDataForDropdownList>();

                            if (HttpContext.Current.Session[sessionStoreData] == null)
                            {
                                HttpContext.Current.Session[sessionStoreData] = GetHierarchyData(id, string.Empty);
                            }
                            item = new ComboboxHierarchyFilter(id, text, mode, nextID, preID, 1, ds, "Value", "Key");
                            ASRadComboBox combobox = ((WebControl)(item.InputControl)) as ASRadComboBox;
                            if (!this.Page.IsPostBack)
                            {
                                combobox.Attributes.Add("triggerloaditem", "1");
                            }

                            if (combobox != null)
                            {
                                combobox.ItemsRequested += (s, e) =>
                                {
                                    string filterValue = e.Text;
                                    ds = HttpContext.Current.Session[sessionStoreData] as List<RefDataForDropdownList>;
                                    combobox.ClearSelection();
                                    combobox.DataSource = ds;
                                    combobox.DataBind();
                                };
                            }
                        }
                    }

                    if (!(row["ShowPrefix"] is DBNull) && bool.Parse(row["ShowPrefix"].ToString()))
                    {
                        item.DefaultInputValue = row["HierarchyPrefix"].ToString();
                    }
                    item.IsSelectable = isSelectable;

                    if (!string.IsNullOrEmpty(childOf))
                    {
                        item.ChildOf = childOf;
                    }
                    hierarchyFilters.Add(item);
                }
                if (HttpRuntime.Cache["FilterTemplate"] == null)
                {
                    StreamReader tpl = new StreamReader(MapPathSecure("~/App_Data/filtering_option.tpl"));
                    CustomHtmlTemplate = tpl.ReadToEnd();
                    tpl.Close();

                    HttpRuntime.Cache.Insert("FilterTemplate", CustomHtmlTemplate, new System.Web.Caching.CacheDependency(MapPathSecure("~/App_Data/filtering_option.tpl")));
                }
                else
                {
                    CustomHtmlTemplate = HttpRuntime.Cache["FilterTemplate"].ToString();
                }
            }
            else
            {
                HierarchyOptionVisible = false;
                HierarchyDetail merchant = GeneralFuncsLib.GetMerchantHierarchyInfo();

                IHierarchyFilter empty = new TextboxFilter(merchant.HierarchyID, merchant.HierarchyMode, merchant.HierarchyName, "", "", 0, 16, "0123456789", true, string.Empty, "", "", 1);
                empty.DefaultInputValue = "";
                hierarchyFilters.Add(empty);

                if (HttpRuntime.Cache["FilterTemplateMerchant"] == null)
                {
                    StreamReader tpl = new StreamReader(MapPathSecure("~/App_Data/filtering_option1.tpl"));
                    CustomHtmlTemplate = tpl.ReadToEnd();
                    tpl.Close();

                    HttpRuntime.Cache.Insert("FilterTemplateMerchant", CustomHtmlTemplate, new System.Web.Caching.CacheDependency(MapPathSecure("~/App_Data/filtering_option1.tpl")));
                }
                else
                {
                    CustomHtmlTemplate = HttpRuntime.Cache["FilterTemplateMerchant"].ToString();
                }
            }

            if (!CustomTemplate.IsNullOrEmpty())
            {
                StreamReader tpl = new StreamReader(MapPathSecure(CustomTemplate));
                CustomHtmlTemplate = tpl.ReadToEnd();
                tpl.Close();
            }

            return hierarchyFilters;
        }

        // // 46747 - [AW] - Reporting Development - FE
        List<RefDataForDropdownList> GetHierarchyData(string hierarchyID, string hierarchyFilterValue, bool addBlankOption = true)
        {
            List<RefDataForDropdownList> ds = new List<RefDataForDropdownList>() { };
            if (addBlankOption)
            {
                ds.Add(new RefDataForDropdownList()
                {
                    Key = "",
                    Value = ""
                });
            }
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams(true);
            parameters.AddLanguageID();
            parameters.Add("@HierarchyID", hierarchyID, DbType.AnsiString);
            parameters.Add("@HierarchyFilterValue", hierarchyFilterValue, DbType.AnsiString);
            DataTable dt = WebServices.RiskServices.GetReports("spa_GetHierarchyData", parameters);
            if (dt.HasData())
            {
                foreach (DataRow row in dt.Rows)
                {
                    ds.Add(new RefDataForDropdownList()
                    {
                        Key = row["EntityNumber"].ToString(),
                        Value = row["EntityName"].ToString()
                    });
                }

            }
            return ds;
        }

        // 
        private void UpdateNextAndPreviousHierarchy(DataTable info, string ExtendHierarchyID)
        {
            DataRow extendRow = info.Select("HierarchyID = " + ExtendHierarchyID).FirstOrDefault();
            string preID = extendRow["PreviousHierarchyID"].ToString();
            string nextID = extendRow["NextHierarchyID"].ToString();

            if (string.IsNullOrEmpty(preID))
            {
                preID = "-1";
            }
            if (string.IsNullOrEmpty(nextID))
            {
                nextID = "-1";
            }
            DataRow rowPrevious = info.Select("HierarchyID = " + preID).FirstOrDefault();
            DataRow rowNext = info.Select("HierarchyID = " + nextID).FirstOrDefault();
            if (rowPrevious != null)
            {
                rowPrevious["NextHierarchyID"] = nextID;
            }

            if (rowNext != null)
            {
                rowNext["PreviousHierarchyID"] = preID;
            }
        }

        public override HierarchyFilterCollection InitializeHierachyFilterCollection()
        {
            string pageRequest = Page.Request.CurrentExecutionFilePath.Substring(Page.Request.CurrentExecutionFilePath.LastIndexOf('/') + 1).ToLower();

            switch (pageRequest)
            {
                case "merchantprofile.aspx":
                case "merchantinformation.aspx":
                case "hierarchyinformation.aspx":
                case "statement.aspx":
                    {
                        DateOptionVisible = false;
                        return CreateHierarchyFilter("MerchantProfile");
                    }

                case "rm_retcb.aspx":
                case "rm_mcf_retcb.aspx":
                    DateOptionDateRangeVisible = false;
                    DateOptionMonthlyVisible = false;
                    return CreateHierarchyFilter("");
                case "transactionreport.aspx":
                    DateOptionDateRangeVisible = false;
                    HierarchyOptionVisible = false;
                    return CreateHierarchyFilter("");

                default:
                    return CreateHierarchyFilter("");
            }
        }
    }

    [Serializable]
    public class RefDataForDropdownList
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
