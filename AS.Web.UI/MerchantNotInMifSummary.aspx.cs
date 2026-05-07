using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Core.Common.VeraCode;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class MerchantNotInMifSummary : ReportPage
{
    #region constant
    public enum DataBindAction
    {
        BindMerchantNotInMIFGrid,
        BindHierarchy,
        BindHierarchyValue
    }

    public enum HierarchySearchMode
    {
        FileSource,
        FileType,
        MERCHANTNAME,
        PARTIALMERCHNUMBER,
        MERCHANTNUMBER

    }
    public enum PostBackAction
    {
        Add,
        Remove
    }
    #endregion
    private bool IsSearch = false;
    private HierarchyFilterValue _reportValue = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            GetFilter();

            OnDataBindControls(DataBindAction.BindHierarchy);
            OnDataBindControls(DataBindAction.BindHierarchyValue);
            OnDataBindControls(DataBindAction.BindMerchantNotInMIFGrid, uxReportGrid);

            SetFilter();
        }
    }

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxReportGrid");
        this.ExporterIDs.Add("uxExporter");
        base.PageInitialize();
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMerchantNotInMIFGrid:
                {
                    string hierarchyValue = this._reportValue.HierarchyMode.Equals(HierarchySearchMode.MERCHANTNAME.ToString()) ?
                        EscapeSpecialCharacter(this._reportValue.Value) : this._reportValue.Value;
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    //parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.AddLoggedInUserParams(10000);
                    parameters.AddLanguageID();
                    parameters.Add("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid);
                    parameters.Add("@DateFilterMode", this._reportValue.DateOption, DbType.Int16);
                    parameters.Add("@BeginDate", this._reportValue.DateOptionValue.From, DbType.DateTime);
                    DateTime endDate = this._reportValue.DateOption == DateOptionMode.DateRange ? this._reportValue.DateOptionValue.To : this._reportValue.DateOptionValue.From;
                    parameters.Add("@EndDate", endDate, DbType.DateTime);
                    parameters.Add("@HierarchyFilterMode", this._reportValue.HierarchyMode, DbType.AnsiString);
                    parameters.Add("@HierarchyFilterValue", hierarchyValue, DbType.AnsiString);
                    if (IsSearch)
                    {
                        uxReportGrid.MasterTableView.CurrentPageIndex = 0;
                        uxReportGrid.MasterTableView.SortExpressions.Clear();
                    }

                    var sortExpression = uxReportGrid.MasterTableView.SortExpressions.Count == 0 ? null : uxReportGrid.MasterTableView.SortExpressions[0].ToString();
                    parameters.Add("@stOrder", sortExpression, DbType.AnsiString);

                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] { "spa_MerchantsNotInMif_Get_NotInMifSummary", ReportServices.ConvertToFilterParamWSArray(parameters) });
                    //Set title for grid
                    uxExporter.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("uxGridTitleResource.GridTitle").ToString());
                    uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(GetSubTitle());

                    break;
                }
            case DataBindAction.BindHierarchy:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.AddLanguageID();

                    DataTable dt = WebServices.SecurityServices.GetReports("spa_MerchantsNotInMif_Get_HierarchyList", parameters);

                    uxHierarchy.DataTextField = "HierarchyName";
                    uxHierarchy.DataValueField = "HierarchyMode";

                    uxHierarchy.DataSource = dt;
                    uxHierarchy.DataBind();

                    break;
                }

            case DataBindAction.BindHierarchyValue:
                {
                    uxHierarchyValue.DataTextField = "DataDescription";
                    uxHierarchyValue.DataValueField = "Data";

                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid);
                    parameters.Add("@HierarchyMode", uxHierarchy.SelectedValue, DbType.AnsiString);
                    parameters.AddLanguageID();

                    uxHierarchyValue.DataSource = WebServices.SecurityServices.GetReports("spa_MerchantsNotInMif_Get_HierarchyDetailData", parameters);
                    uxHierarchyValue.DataBind();

                    HtmlGenericControl hierarchyValueObj = pnlHierarchyValue.FindControl("ucHierarchyValue") as HtmlGenericControl;
                    if (uxHierarchy.SelectedValue.Equals("FileSource") || uxHierarchy.SelectedValue.Equals("FileType"))
                    {
                        hierarchyValueObj.Attributes.Add("class", "multichooser-wrapper text-left");
                    }
                    else
                    {
                        hierarchyValueObj.Attributes.Add("class", "hide");
                    }
                    uxMerchantNumValue.CssClass = (uxHierarchy.SelectedValue.Equals("MERCHANTNUMBER") || uxHierarchy.SelectedValue.Equals(HierarchySearchMode.PARTIALMERCHNUMBER.ToString())) ? "rf_TextBox" : "hide";
                    uxMerchantNameValue.CssClass = uxHierarchy.SelectedValue.Equals("MERCHANTNAME") ? "rf_TextBox" : "hide";
                    break;
                }

        }
    }

    protected override void DoGridNeedDataSource(ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && uxReportGrid.Visible)
        {
            GetFilter();
            OnDataBindControls(DataBindAction.BindMerchantNotInMIFGrid, sender);
        }
    }

    protected void uxReportGrid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            if (dataRow.IsNotNullData() && dataRow["TransactionCount"].IsNullOrEmpty())
            {
                item["TransactionCount"].Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
            }
            else
            {
                string url = BuildUrlViewNotInMifDetails(dataRow["FileType"].ToString(), dataRow["FileSource"].ToString(), dataRow["ReportDate"].ToString(), dataRow["MerchantNumber"].ToString());
                string mifDetailLink = string.Format("<a href=\"#\" onclick=\" return ShowPopupModal('{0}', 'auto');\">{1}</a>", url, dataRow["TransactionCount"].ToString());
                item["TransactionCount"].Text = mifDetailLink;
            }
        }
    }

    private string BuildUrlViewNotInMifDetails(string fileType, string fileSource, string reportDate, string merchantNumber)
    {
        return ResolveUrl("~") + "NotInMifDetailModal.aspx?" + BuildSecureQueryString(string.Format("reportDate={0}&fileSource={1}&fileType={2}&merchantNumber={3}", reportDate, fileSource, fileType, merchantNumber));
    }

    protected void uxHierarchy_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        uxHierarchyValue.Items.Clear();
        uxMerchantNumValue.Text = uxMerchantNameValue.Text = string.Empty;
        OnDataBindControls(DataBindAction.BindHierarchyValue);

        this.AjaxAddResponseScript("addCheckSpecialCharacters();");
    }


    protected void uxSearch_Click(object sender, EventArgs e)
    {
        IsSearch = true;
        SetFilter();
        uxReportGrid.Rebind();
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = VeraCodeSolution.DoVeraCode(GetSubTitle(true));
        exportConfig.ReportHeader = string.Format("{0} - {1}", GetLocalResourceObject("ExportTitle").ToString(), GetSubTitle());
    }
    private string GetSubTitle(bool isExport = false)
    {
        GetFilter();
        var count = uxHierarchyValue.Items.Count;
        string subTitle = string.Empty;
        string dateTitle = string.Empty;

        switch (this._reportValue.HierarchyMode)
        {
            case "FileSource":
                {
                    //Selected all hierarchy or doesn't select anythings
                    int selectedValue = !this._reportValue.Value.IsNullOrEmpty() ? this._reportValue.Value.Split(',').Length : 0;
                    if (selectedValue == 0 || selectedValue == uxHierarchyValue.Items.Count)
                        subTitle = GetLocalResourceObject("AllFileSourcesSubTitle").ToString();
                    else if (selectedValue == 1)
                        subTitle = !isExport ? this._reportValue.Value : string.Format("{0}_{1}", GetLocalResourceObject("FileSource").ToString(), this._reportValue.Value);
                    else
                        subTitle = GetLocalResourceObject("MultipleFileSourcesSubTitle").ToString();
                    break;
                }
            case "FileType":
                {
                    //Selected all hierarchy or doesn't select anythings
                    int selectedValue = !this._reportValue.Value.IsNullOrEmpty() ? this._reportValue.Value.Split(',').Length : 0;
                    if (selectedValue == 0 || selectedValue == uxHierarchyValue.Items.Count)
                        subTitle = GetLocalResourceObject("AllFileTypeSubTitle").ToString();
                    else if (selectedValue == 1)
                        subTitle = !isExport ? GetLocalResourceObject(this._reportValue.Value).ToString() :
                            string.Format("{0}_{1}", GetLocalResourceObject("FileType").ToString(), GetLocalResourceObject(this._reportValue.Value).ToString());
                    else
                        subTitle = GetLocalResourceObject("MultipleFileTypesSubTitle").ToString();
                    break;
                }
            case "MERCHANTNAME":
                {
                    subTitle = !isExport ? string.Format(GetLocalResourceObject("MerchantNameSubTitle").ToString(), this._reportValue.Value)
                        : string.Format("{0}_{1}", GetLocalResourceObject("MerchantName").ToString(), this._reportValue.Value);
                    break;
                }
            case "MERCHANTNUMBER":
                {
                    if (isExport)
                    {
                        subTitle = this._reportValue.Value.IsNullOrEmpty() ? GetLocalResourceObject("AllMerchants").ToString()
                            : string.Format("{0}_{1}", GetLocalResourceObject("Merchant").ToString(), this._reportValue.Value);
                    }
                    else
                    {
                        string merchantName = GeneralFuncsLib.GetMerchantName(this._reportValue.Value, true);
                        string titleSearchWithMerchant = merchantName.IsNullOrEmpty() ? this._reportValue.Value : string.Format("{0}: {1}", this._reportValue.Value, merchantName);
                        subTitle = this._reportValue.Value.IsNullOrEmpty() ? GetLocalResourceObject("AllMerchants").ToString() : titleSearchWithMerchant;
                    }
                    break;
                }
        }
        //Get date filter
        if (this._reportValue.DateOption == DateOptionMode.Daily)
            dateTitle = !isExport ? string.Format("({0})", this._reportValue.DateOptionValue.From.ToString(WebSiteConstants.DATE_FORMAT))
                : string.Format("{0}", this._reportValue.DateOptionValue.From.ToString(WebSiteConstants.DATE_FORMAT));
        else if (this._reportValue.DateOption == DateOptionMode.Monthly)
        {
            dateTitle = !isExport ? string.Format("({0} - {1})", this._reportValue.DateOptionValue.From.GetFirstDayOfMonth().ToString(WebSiteConstants.DATE_FORMAT),
                           this._reportValue.DateOptionValue.To.ToString(WebSiteConstants.DATE_FORMAT))
                           : string.Format("{0}-{1}", this._reportValue.DateOptionValue.From.GetFirstDayOfMonth().ToString(WebSiteConstants.DATE_FORMAT),
                           this._reportValue.DateOptionValue.To.ToString(WebSiteConstants.DATE_FORMAT));
        }
        else
            dateTitle = !isExport ? string.Format("({0} - {1})", this._reportValue.DateOptionValue.From.ToString(WebSiteConstants.DATE_FORMAT),
                this._reportValue.DateOptionValue.To.ToString(WebSiteConstants.DATE_FORMAT))
                : string.Format("{0}-{1}", this._reportValue.DateOptionValue.From.ToString(WebSiteConstants.DATE_FORMAT),
                this._reportValue.DateOptionValue.To.ToString(WebSiteConstants.DATE_FORMAT));

        var temp = string.Format("{0}_{1}_{2}", GetLocalResourceObject("ExportFileName").ToString().ToUpper(), subTitle.Replace(" ", ""), dateTitle);
        return !isExport ? string.Format("{0} {1}", subTitle, dateTitle)
            : string.Format("{0}_{1}_{2}", GetLocalResourceObject("ExportFileName").ToString().ToUpper(), subTitle.Replace(" ", ""), dateTitle);
    }


    #region Helper

    // Get Date from Session if not null and assign to control
    private void GetFilter()
    {
        this._reportValue = SavedReportFilterValue;
        if (this._reportValue == null)
        {
            this._reportValue = new HierarchyFilterValue();
            this._reportValue.DateOption = DateOptionMode.DateRange;
            this._reportValue.DateOptionValue.To = !uxEndDate.SelectedDate.IsNullOrEmpty() ? uxEndDate.SelectedDate.Value : DateTime.Now;
            this._reportValue.DateOptionValue.From = !uxFromDate.SelectedDate.IsNullOrEmpty() ? uxFromDate.SelectedDate.Value : DateTime.Now.GetFirstDayOfMonth();
            this._reportValue.HierarchyMode = HierarchySearchMode.FileSource.ToString();
        }
        switch (_reportValue.DateOption)
        {
            case DateOptionMode.Daily:
                this.uxDaily.Checked = true;
                this.uxDate.SelectedDate = this._reportValue.DateOptionValue.From;
                break;
            case DateOptionMode.Monthly:
                this.uxMonthly.Checked = true;
                this.uxDate.SelectedDate = this._reportValue.DateOptionValue.From;
                break;
            case DateOptionMode.DateRange:
                this.uxRange.Checked = true;
                this.uxFromDate.SelectedDate = this._reportValue.DateOptionValue.From;
                this.uxEndDate.SelectedDate = this._reportValue.DateOptionValue.To;
                break;
        }
        if (this._reportValue.HierarchyMode.Equals(HierarchySearchMode.MERCHANTNAME.ToString()) ||
            this._reportValue.HierarchyMode.Equals(HierarchySearchMode.PARTIALMERCHNUMBER.ToString()) ||
            this._reportValue.HierarchyMode.Equals(HierarchySearchMode.MERCHANTNUMBER.ToString()))
        {
            uxHierarchy.SelectedValue = this._reportValue.HierarchyMode;
            uxMerchantNameValue.Text = this._reportValue.HierarchyMode.Equals(HierarchySearchMode.MERCHANTNAME.ToString()) ? this._reportValue.Value : string.Empty;
            uxMerchantNumValue.Text = (this._reportValue.HierarchyMode.Equals(HierarchySearchMode.MERCHANTNUMBER.ToString()) || this._reportValue.HierarchyMode.Equals(HierarchySearchMode.PARTIALMERCHNUMBER.ToString())) ? this._reportValue.Value : string.Empty;
        }
    }

    // Set Date to Session
    private void SetFilter()
    {
        //Get report filter form session
        if (SavedReportFilterValue != null)
            this._reportValue = SavedReportFilterValue;
        else
            this._reportValue = new HierarchyFilterValue();
        // Set Date Option         
        if (uxDaily.Checked)
        {
            this._reportValue.DateOption = DateOptionMode.Daily;
            this._reportValue.DateOptionValue.From = this._reportValue.DateOptionValue.To = uxDate.SelectedDate.Value;
        }
        else if (uxMonthly.Checked)
        {
            var date = new DateTime(uxDate.SelectedDate.Value.Year, uxDate.SelectedDate.Value.Month, 1);

            this._reportValue.DateOption = DateOptionMode.Monthly;
            this._reportValue.DateOptionValue.From = uxDate.SelectedDate.Value;
            this._reportValue.DateOptionValue.To = (date.Ticks < DateTime.Now.Ticks && (date.Month != DateTime.Now.Month || date.Year != DateTime.Now.Year)) ?
                uxDate.SelectedDate.Value.GetLastDayOfMonth() : DateTime.Now;
        }
        else
        {
            this._reportValue.DateOption = DateOptionMode.DateRange;
            this._reportValue.DateOptionValue.From = uxFromDate.SelectedDate.Value;
            this._reportValue.DateOptionValue.To = uxEndDate.SelectedDate.Value;
        }

        // Set report filter
        this._reportValue.HierarchyMode = uxHierarchy.SelectedValue;

        if (uxHierarchy.SelectedValue.Equals(HierarchySearchMode.MERCHANTNAME.ToString()) ||
            uxHierarchy.SelectedValue.Equals(HierarchySearchMode.MERCHANTNUMBER.ToString())||
            uxHierarchy.SelectedValue.Equals(HierarchySearchMode.PARTIALMERCHNUMBER.ToString()))
        {
            this._reportValue.ID = GeneralFuncsLib.GetHierarchyInfo(uxHierarchy.SelectedValue).HierarchyID;
            this._reportValue.Value = uxHierarchy.SelectedValue.Equals(HierarchySearchMode.MERCHANTNAME.ToString()) ? uxMerchantNameValue.Text : uxMerchantNumValue.Text;
        }
        else
        {
            this._reportValue.ID = string.Empty;
            this._reportValue.Value = HandleString(uxHierarchyValue.SelectedItems);
        }

        SavedReportFilterValue = this._reportValue;
    }

    private string HandleString(ListItemCollection items)
    {
        if (items.Count == 0)
            return string.Empty;
        StringBuilder result = new StringBuilder();
        string delim = "";
        foreach (ListItem item in items)
        {
            result.Append(delim); delim = ",";
            result.Append(item.Value);
        }
        return result.ToString();
    }

    private string EscapeSpecialCharacter(string text)
    {
        return text.Replace("[", "[[]").Replace("%", "[%]").Replace(",", "[,]").Replace("_", "[_]");
    }
    #endregion
}