using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;
using System.Web;
using System.Drawing;
using AS.Utilities;

public partial class UserControls_NewRiskTransactionHistory : GlobalUserControl
{
    private const string TRANS_TYPE = "TransType";
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string REPORT_DATE = "ReportDate";
    private const string TRANSACTION_AMOUNT = "TransactionAmount";
    private const string XML_CONFIG_PATH = "~/App_Data/TransactionHistoryColumn.xml";
    private const string IMAGE = "<a class=\"image-link\" href=\"#\"  onclick=\" return ShowPopupModalChild({0},'{1}','auto');\"><img src='{2}res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";
    private const string MATCHED_CODE = "MatchCode";
    private const string MATCHED_NAME = "MatchName";

    private int index = 0;
    private string m_currentSortExpr = string.Empty;
    private string m_currentSortOrder = string.Empty;
    private string _CurrentSortControls = string.Empty;

    public int TotalRows;
    public string MerchantNumber
    {
        get
        {
            if (ViewState["MerchantNum"] != null) return ViewState["MerchantNum"].ToString();
            else return string.Empty;
        }
        set { ViewState["MerchantNum"] = value; }
    }
    public DateTime ReportDate
    {
        get
        {
            if (ViewState["ReportDate"] != null) return DateTime.Parse(ViewState["ReportDate"].ToString());
            else return DateTime.Now.Date;
        }
        set { ViewState["ReportDate"] = value; }
    }
    public int FlagLink
    {
        get
        {
            if (ViewState["FlagLink"] != null)
                return Int32.Parse(ViewState["FlagLink"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["FlagLink"] = value;
        }
    }

    public string XMLConfig
    {
        get
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (System.Web.HttpRuntime.Cache["XMLConfig"] == null)
            {
                if (File.Exists(Server.MapPath(XML_CONFIG_PATH)))
                {
                    xmlDoc.Load(Server.MapPath(XML_CONFIG_PATH));
                    System.Web.HttpRuntime.Cache.Insert("XMLConfig", xmlDoc.OuterXml, new System.Web.Caching.CacheDependency(Server.MapPath(XML_CONFIG_PATH)));
                }
            }

            return System.Web.HttpRuntime.Cache["XMLConfig"].ToString();
        }
    }


    string _PartialCardSearchIntruderQuery = string.Empty;
    private string PartialCardSearchIntruderQuery
    {
        get
        {

            if (_PartialCardSearchIntruderQuery == string.Empty)
            {
                _PartialCardSearchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxTransactionDetails.ID, new string[] { "AccountNumber" });
            }
            return _PartialCardSearchIntruderQuery;
        }
    }
    string _AuthIntruderQuery = string.Empty;
    protected string AuthIntruderQuery
    {
        get
        {
            if (this._AuthIntruderQuery == string.Empty) this._AuthIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.uxTransactionDetails.ID, new string[] { "AuthorizationNumber" });
            return this._AuthIntruderQuery;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uxTransactionDetails.IsIntruder = true;
        uxTransactionDetails.IntruderSourceName = GeneralFuncsLib.GetPageUrlFileName() + uxTransactionDetails.ID;
        if (FlagLink == 1)
        {
            string queryString = this.Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                   MerchantNumber, ReportDate));
            string url = string.Format("<span class=\"dark-blue\"><a class=\"link-back\" href=\"#\" onclick=\"OpenInstanceWindow('NewRiskReport_TransactionHistoryModal.aspx?{0}','DQWindow'); return false;\">", queryString);
            uxTransHistoryHeader.Text = VeraCodeSolution.DoVeraCode(url + GetLocalResourceObject("RiskTransactionHistory_ascx_cs_NewWindow").ToString() + "</a></span>");
        }
        else
        {
            uxTransHistoryHeader.Text = "";
        }
        if (!IsPostBack)
        {
            RiskSessionManager.TransactionHistorySortOrder = "ReportDate DESC, PartialCardNumber ASC";

            BindTransDateRange();
            BindOrderBy();
            BindOrderByFromSession();
            uxTransactionDetails.Rebind();
        }
    }

    private bool _IsExporting = false;
    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {
        _IsExporting = true;
        uxTransactionDetails.Columns.FindByUniqueName("CardNumber").Visible = false;
        uxTransactionDetails.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        uxTransactionDetails.Columns.FindByUniqueName("BatchAmount").HeaderText =
          uxTransactionDetails.Columns.FindByUniqueName("BatchAmount").HeaderText.ToCurrencySymbol();

        string fileName = GeneralFuncsLib.FormatFileName(exportConfig.FileName);
        exportConfig.FileName = HttpUtility.UrlEncode(fileName);
        exportConfig.ReportHeader = GetLocalResourceObject("RiskTransactionHistory_ascx_cs_TransactionHistory").ToString();

    }
    protected void uxTransactionDetails_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        RiskSessionManager.TransactionHistorySortOrder = string.Empty;
        BindOrderByFromSession();
    }
    protected void uxTransactionDetails_DataSourceReady(object sender, EventArgs e)
    {
        if (uxTransactionDetails.AS_DataSource.Rows.Count == 0)
        {
            uxTransactionDetails.AllowSorting = false;
        }
    }
    protected void uxTransactionDetails_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        //46652 - AW - Multi-Currency Transaction Display
        uxTransactionDetails.Columns.FindByUniqueName("BatchAmount").HeaderText =
           uxTransactionDetails.Columns.FindByUniqueName("BatchAmount").HeaderText.ToCurrencySymbol();

        if (RiskSessionManager.TransactionHistorySortOrder != string.Empty)
        {
            uxTransactionDetails.AS_SortExpression = RiskSessionManager.TransactionHistorySortOrder;
        }
        if (GeneralFuncsLib.CheckCSViewFullCard(this.Page) && !_IsExporting)
        {
            uxTransactionDetails.Columns.FindByUniqueName("CardNumber").Visible = true;
            uxTransactionDetails.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
        }
        else
        {
            uxTransactionDetails.Columns.FindByUniqueName("CardNumber").Visible = false;
            uxTransactionDetails.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        }
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserRiskParams();
        SecurePage securePage = HttpContext.Current.Handler as SecurePage;
        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
        {
            parameters.AddDecryptDataParams("CardNumber", _IsExporting);
        }
        parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@DateRange", RiskSessionManager.TransDateRangeModal, DbType.Int32));
        parameters.AddLanguageID();
        uxTransactionDetails.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices,
                    WebSiteConstants.GET_REPORT_METHOD_NAME,
                    new object[] { "spa_rm_cs_RiskReport_GetTransactionHistory", 
                ReportServices.ConvertToFilterParamWSArray(parameters) });
    }

    protected void uxTransactionDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case GridItemType.AlternatingItem:
            case GridItemType.Item:
                {

                    GridDataItem dataItem = e.Item as GridDataItem;
                    var rowItem = (e.Item.DataItem as DataRowView).Row;
                    dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["CardDescription"].ToString());
                    dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["EntryModeDescription"].ToString());
                    dataItem["ADF"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["ADFDescription"].ToString());
                    dataItem["ResponseCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["AuthResponseDescription"].ToString());
                    dataItem["AVS"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["AVSDescription"].ToString());
                    dataItem["CVV"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["CVVDescription"].ToString());
                    dataItem["ExpirationDate"].ToolTip = "MM/YY";
                    dataItem["CountryCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["CountryName"].ToString());
                    dataItem["DupeCount"].ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["DupeAmount"]));

                    if (GeneralFuncsLib.NvlString(rowItem["ADF"]).Equals("F", StringComparison.OrdinalIgnoreCase)
                        || GeneralFuncsLib.NvlString(rowItem["ADF"]).Equals("D", StringComparison.OrdinalIgnoreCase))
                    {
                        dataItem["ADF"].Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatBorderText(rowItem["ADF"].ToString(), Color.Red));
                    }


                    Color transColor = Color.White;
                    if (GeneralFuncsLib.NvlString(rowItem[TRANS_TYPE]).Equals("Sales") || GeneralFuncsLib.NvlString(rowItem[TRANS_TYPE]).Equals("Ventas"))
                    {
                        transColor = Color.Green;
                    }
                    else if (GeneralFuncsLib.NvlString(rowItem[TRANS_TYPE]).Equals("Auth") || GeneralFuncsLib.NvlString(rowItem[TRANS_TYPE]).Equals("Autorización"))
                    {
                        transColor = Color.LightBlue;
                    }
                    if (transColor != Color.White)
                    {
                        dataItem[TRANS_TYPE].Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatBorderText(rowItem[TRANS_TYPE].ToString(), transColor));
                    }

                    if (rowItem["TransDate"].ToString().Trim().Length > 10)
                    {
                        dataItem["TransDate"].Width = 90;
                    }

                    if (!GeneralFuncsLib.NvlString(rowItem[TRANS_TYPE]).Equals("Auth"))
                    {
                        dataItem["TransDate"].Text = VeraCodeSolution.ValidateResponseData(dataItem["TransDate"].Text.Split(' ')[0]);
                    }
                    if (!String.IsNullOrEmpty(rowItem["CardNumber"].ToString()))
                    {
                        string fullcard = VeraCodeSolution.ValidateResponseData(rowItem["CardNumber"].ToString());// WebServices.RiskServices.DecryptText(rowView["AccountNumber"].ToString(), SessionManager.CurrentUser.ASClient);

                        string queryString = Page.BuildSecureQueryString("cn=" + rowItem["PartialCardNumber"] + "&cnf=" + fullcard + "&merch=" + MerchantNumber + "&isRisk=1");
                        string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
                        string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";

                        if (_IsExporting)
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(rowItem["PartialCardNumber"].ToString());
                        }
                        else if (GeneralFuncsLib.CheckCSViewFullCard(this.Page))
                        {
                            dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + fullcard + "</a>");
                        }
                        else
                        {
                            if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this.Page))
                            {
                                dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(
                                    String.Format(IMAGE, (index + 1), BuildUrlForFullCard(rowItem["ReportType"].ToString(), rowItem["RecordId"].ToString(), rowItem["IssuingBank"].ToString(), rowItem["ReportDate"].ToString()), ResolveUrl("~/")) + VeraCodeSolution.DoVeraCode(urlCard + rowItem["PartialCardNumber"].ToString() + "</a>")
                                    );
                            }
                            else
                            {
                                dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowItem["PartialCardNumber"].ToString() + "</a>");
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(rowItem["SettleType"].ToString()))
                    {
                        dataItem["Settled"].Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
                    }

                    // 46807 
                    dataItem[MATCHED_CODE].ToolTip = VeraCodeSolution.DoVeraCode(rowItem[MATCHED_NAME].ToString());

                    string matchedCode = rowItem[MATCHED_CODE].ToString();

                    if (matchedCode.Equals("P", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("CP", StringComparison.OrdinalIgnoreCase))
                    {
                        dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, Color.Blue);
                    }
                    else if (matchedCode.Equals("U", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("SC", StringComparison.OrdinalIgnoreCase))
                    {
                        dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, Color.Red);
                    }
                    else if (matchedCode.Equals("M", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("C", StringComparison.OrdinalIgnoreCase))
                    {
                        dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, "#3fbf00".ToColor());
                    }
                }
                break;
        }
    }
    private string BuildUrlForFullCard(string reportType, string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = Page.BuildSecureQueryString("rt=" + reportType + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate + "&idx=" + (index + 1));
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }

    private void BindTransDateRange()
    {
        List<RadComboBoxItem> items = new List<RadComboBoxItem>()
        {
            new RadComboBoxItem("1", "1"),
            new RadComboBoxItem("2", "2"),
            new RadComboBoxItem("3", "3"),
            new RadComboBoxItem("5", "5"),
            new RadComboBoxItem("7", "7"),
            new RadComboBoxItem("14", "14"),
            new RadComboBoxItem("30", "30"),
            new RadComboBoxItem("60", "60"),
            new RadComboBoxItem("90", "90")
        };
        uxTransDateRange.Items.AddRange(items);

        RadComboBoxItem item = items.Find(i => i.Value == GeneralFuncsLib.GetDefaultDaysOfRiskTransactionHistory());
        item.Selected = true;
    }

    protected void uxTransDateRange_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RiskSessionManager.TransDateRangeModal = int.Parse(uxTransDateRange.SelectedValue);
        uxTransactionDetails.Rebind();
    }
    private string _OrderBy
    {
        get
        {
            string orderBy = string.Empty;
            if (uxOrderBy1.SelectedValue != string.Empty)
            {
                orderBy = uxOrderBy1.SelectedValue;
                if (optAsc1.Checked)
                    orderBy += " ASC";
                else
                    orderBy += " DESC";
            }
            if (uxOrderBy2.SelectedValue != string.Empty && !orderBy.Contains(uxOrderBy2.SelectedValue))
            {

                if (!String.IsNullOrEmpty(orderBy))
                    orderBy += ", " + uxOrderBy2.SelectedValue;
                else orderBy = uxOrderBy2.SelectedValue;
                if (optAsc2.Checked)
                    orderBy += " ASC";
                else
                    orderBy += " DESC";
            }
            if (uxOrderBy3.SelectedValue != string.Empty && !orderBy.Contains(uxOrderBy3.SelectedValue))
            {
                if (!String.IsNullOrEmpty(orderBy))
                    orderBy += ", " + uxOrderBy3.SelectedValue;
                else orderBy = uxOrderBy3.SelectedValue;
                if (optAsc3.Checked)
                    orderBy += " ASC";
                else
                    orderBy += " DESC";
            }
            return orderBy;
        }
    }

    private void BindOrderBy()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath(XML_CONFIG_PATH));
        XmlNodeList nodeList = xmlDoc.SelectNodes("//Item");
        if (nodeList != null)
        {
            foreach (XmlNode node in nodeList)
            {
                if (!string.IsNullOrEmpty(node.Attributes["Text"].Value))
                    node.Attributes["Text"].Value = GetLocalResourceObject(node.Attributes["Text"].Value).ToString().ToCurrencySymbol();
            }
        }
        string xmlConfig = xmlDoc.OuterXml;
        uxOrderBy1.LoadXml(xmlConfig);
        uxOrderBy2.LoadXml(xmlConfig);
        uxOrderBy3.LoadXml(xmlConfig);

        RadComboBoxItem emptyItem1 = new RadComboBoxItem(string.Empty, string.Empty);
        emptyItem1.Height = Unit.Pixel(13);
        uxOrderBy1.Items.Insert(0, emptyItem1);

        RadComboBoxItem emptyItem2 = new RadComboBoxItem(string.Empty, string.Empty);
        emptyItem2.Height = Unit.Pixel(13);
        emptyItem2.Selected = true;
        uxOrderBy2.Items.Insert(0, emptyItem2);

        RadComboBoxItem emptyItem3 = new RadComboBoxItem(string.Empty, string.Empty);
        emptyItem3.Height = Unit.Pixel(13);
        emptyItem3.Selected = true;
        uxOrderBy3.Items.Insert(0, emptyItem3);

        uxOrderBy1.Sort = uxOrderBy2.Sort = uxOrderBy3.Sort = RadComboBoxSort.Ascending;
        uxOrderBy1.Items.Sort();
        uxOrderBy2.Items.Sort();
        uxOrderBy3.Items.Sort();

    }
    private void BindOrderByFromSession()
    {
        if (RiskSessionManager.TransactionHistorySortOrder != string.Empty)
        {
            string[] arrOrderBy = RiskSessionManager.TransactionHistorySortOrder.Split(',');

            if (arrOrderBy.Length > 0)
            {
                string orderBy1 = arrOrderBy[0];
                if (orderBy1.Contains("ASC"))
                {
                    optAsc1.Checked = true;
                }
                else
                {
                    optDesc1.Checked = true;
                }
                orderBy1 = orderBy1.Replace("DESC", "");
                orderBy1 = orderBy1.Replace("ASC", "");
                orderBy1 = orderBy1.Trim();
                uxOrderBy1.SelectedValue = orderBy1;
            }
            else { optDesc1.Checked = true; }

            if (arrOrderBy.Length > 1)
            {
                string orderBy2 = arrOrderBy[1];
                if (orderBy2.Contains("ASC"))
                {
                    optAsc2.Checked = true;
                }
                else
                {
                    optDesc2.Checked = true;
                }
                orderBy2 = orderBy2.Replace("DESC", "");
                orderBy2 = orderBy2.Replace("ASC", "");
                orderBy2 = orderBy2.Trim();
                uxOrderBy2.SelectedValue = orderBy2;
            }
            else { optDesc2.Checked = true; }

            if (arrOrderBy.Length > 2)
            {
                string orderBy3 = arrOrderBy[1];
                if (orderBy3.Contains("ASC"))
                {
                    optAsc3.Checked = true;
                }
                else
                {
                    optDesc3.Checked = true;
                }
                orderBy3 = orderBy3.Replace("DESC", "");
                orderBy3 = orderBy3.Replace("ASC", "");
                orderBy3 = orderBy3.Trim();
                uxOrderBy3.SelectedValue = orderBy3;
            }
            else { optDesc3.Checked = true; }
        }
        else
        {
            uxOrderBy1.SelectedValue = string.Empty;
            uxOrderBy2.SelectedValue = string.Empty;
            uxOrderBy3.SelectedValue = string.Empty;
            optDesc1.Checked = true;
            optDesc2.Checked = true;
            optDesc3.Checked = true;

            //Trigger client function to hide order by options
            if (this.Page is ReportPage)
            {
                ((ReportPage)this.Page).AjaxAddResponseScript("window.renderOrderByOptions();");
            }
            else if (this.Page is NonReportPage)
            {
                ((NonReportPage)this.Page).AjaxAddResponseScript("window.renderOrderByOptions();");
            }
        }
    }

    private void SetOrderByToSession()
    {
        RiskSessionManager.TransactionHistorySortOrder = _OrderBy;
    }

    protected void uxOrderBy_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        SortTransactionHistory();
    }

    void BindSortFilterBySelectedOrder(RadComboBox cbb, RadioButton radio1, RadioButton radio2)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(XMLConfig);
        string where = string.Format("//Items/Item[@Value='{0}']", cbb.SelectedValue);
        XmlNode node = doc.SelectSingleNode(where);

        if (node != null)
        {
            radio1.Text = GetLocalResourceObject(node.Attributes["Filters"].Value.Split(',')[0]).ToString();
            radio2.Text = GetLocalResourceObject(node.Attributes["Filters"].Value.Split(',')[1]).ToString();
        }
    }

    protected void optAscDesc_CheckedChanged(object sender, EventArgs e)
    {
        SortTransactionHistory();
    }

    private void SortTransactionHistory()
    {
        SetOrderByToSession();
        uxTransactionDetails.Rebind();
    }
    public void Rebind()
    {
        string queryString = this.Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                   MerchantNumber, ReportDate));
        string url = string.Format("<span class=\"dark-blue\"><a class=\"link-back\" href=\"#\" onclick=\"OpenInstanceWindow('NewRiskReport_TransactionHistoryModal.aspx?{0}','DQWindow'); return false;\">", queryString);
        uxTransHistoryHeader.Text = VeraCodeSolution.DoVeraCode(url + GetLocalResourceObject("RiskTransactionHistory_ascx_cs_NewWindow").ToString() + "</a></span>");
        uxTransactionDetails.Rebind();
    }

    public bool isOntop { get; set; }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (isOntop)
            uxTitle.Attributes["class"] += " on-top";

        BindSortFilterBySelectedOrder(uxOrderBy1, optAsc1, optDesc1);
        BindSortFilterBySelectedOrder(uxOrderBy2, optAsc2, optDesc2);
        BindSortFilterBySelectedOrder(uxOrderBy3, optAsc3, optDesc3);
    }
}
