using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Controls.Global;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.VW.Common;
using AS.VW.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_Chargebacks : System.Web.UI.UserControl
{
    private const string SPA_GET_CHARGEBACK = "spa_RM_MCF_GetFlatReportChargebackDetail_MultiMerchant";
    private const string MERCHANT_NAME = "MerchantName";
    private int _totalRows = 0;
    private int _ChargebackCountKeyed = 0;
    private decimal _ChargebackAmount = 0;
    int index = 0;

    enum DataBindAction
    {
        BindChargeback,
    }

    public string MerchantList { get; set; }
    public int FilterWorkingStatus { get; set; }
    public DateTime ReportDate { get; set; }
    public bool IsCSViewFullCard { get; set; }
    public List<DataSourceParallelResponse> DataSources { get; set; }

    public bool IsExport { get; set; }
    public bool IsEnableExport { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindChargeback();
    }

    protected void ExportButtonExcel_Click(object sender, EventArgs e)
    {
        IsExport = true;
        MerchantList = RiskSessionManager.currentMerchantNumber;
        ReportDate = DateTime.UtcNow;
    }

    #region ChargeBacks
    protected void uxChargeback_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem dataItem = e.Item as RepeaterItem;
        DataRowView rowView = e.Item.DataItem as DataRowView;

        string acctNumberText = string.Format("{0};{1};{2};{3};{4};{5}", e.Item.ItemIndex.ToString(), ReportType.CHARGEBACKS, rowView["RecordID"].ToString(),
            rowView["PartialAccountNumber"].ToString(), rowView["ReportDate"].ToString(), rowView["MerchantNumber"].ToString());
        string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"parent.ShowPopupAcctNumber('" + acctNumberText + "');\">";

        TableColumnContent partialAccountNumber = dataItem.FindControl("uxPartialAccountNumber") as TableColumnContent;
        if (!IsCSViewFullCard)
            partialAccountNumber.Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>");
        else
            partialAccountNumber.Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["AccountNumber"].ToString() + "</a>");


        TableColumnContent transactionDate = dataItem.FindControl("uxTransactionDate") as TableColumnContent;
        transactionDate.Text = VeraCodeSolution.DoVeraCode(transactionDate.CustomFormatDataValue(rowView["TransactionDate"], FormatType.Date));
        TableColumnContent reasonCode = dataItem.FindControl("uxReasonCode") as TableColumnContent;
        reasonCode.Text = VeraCodeSolution.DoVeraCode(rowView["ReasonCode"].ToASString() + "&nbsp;");
        TableColumnContent keyed = dataItem.FindControl("uxKeyed") as TableColumnContent;
        //  42867 - Chargeback section of the Next Queue 'Key' always N
        if (rowView["Keyed"].IsNullOrEmpty())
        {
            keyed.Text = WebSiteConstants.HTML_EM_DASH;
        }
        else
        {
            keyed.Text = VeraCodeSolution.DoVeraCode(rowView["Keyed"].ToASString());
        }

        TableColumnContent transactionAmount = dataItem.FindControl("uxTransactionAmount") as TableColumnContent;
        transactionAmount.Text = FormatData.FormatCurrency(rowView["TransactionAmount"].ToDecimalAmount(), SessionManager.CurrencyFortmat);

        // Card type
        TableColumnContent cardType = dataItem.FindControl("uxCardType") as TableColumnContent;
        cardType.Text = VeraCodeSolution.DoVeraCode(rowView["CardType"].ToASString());
        
        // Report date
        TableColumnContent reportDate = dataItem.FindControl("uxReportDate") as TableColumnContent;
        reportDate.Text = VeraCodeSolution.DoVeraCode(reportDate.CustomFormatDataValue(rowView["ReportDate"], FormatType.Date));

    }

    protected void uxChargeback_PreRender(object sender, EventArgs e)
    {
        uxExportPannel.Visible = IsEnableExport;

        if (uxChargeback.Items.Count == 0)
            uxChargebackFooter.Visible = false;
        else uxChargebackFooter.Visible = true;

        uxPartialAccountNumberTotal.Text = "&nbsp;" + GetLocalResourceObject("DQNextQReportCS_Text_Total").ToString() + ": " + _totalRows;
        uxKeyedTotal.Text = _ChargebackCountKeyed.ToString();
        uxTransactionAmountTotal.Text = FormatData.FormatCurrency(_ChargebackAmount, SessionManager.CurrencyFortmat);
    }
    #endregion

    private void BindChargeback()
    {
        DataTable data = GetChargebackList();
        CalculateChargebackTotal(data);
        uxChargeback.DataSource = data;
        uxChargeback.DataBind();

        IsEnableExport = data != null && data.Rows.Count > 0;
    }

    private void CalculateChargebackTotal(DataTable dt)
    {
        if (dt.IsNotNullData() && dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            _totalRows = Convert.ToInt32(row["TotalRows"].ToString());
            _ChargebackCountKeyed = Convert.ToInt32(row["Count_Key"].ToString());
            _ChargebackAmount = Convert.ToDecimal(row["SUM_Amount"].ToString());
        }
    }

    public DataTable GetChargebackList()
    {
        DataTable dataTable = null;

        if (string.IsNullOrEmpty(MerchantList))
        {
            dataTable = new DataTable();
        }

        if (GeneralFuncsLib.IsNotNullData((object)DataSources))
        {
            DataSourceParallelResponse val = DataSources.SingleOrDefault((DataSourceParallelResponse m) => m.FeatureName == DataBindAction.BindChargeback.ToString());
            if (GeneralFuncsLib.IsNotNullData((object)val))
            {
                dataTable = val.DataSource;
            }
        }

        if (GeneralFuncsLib.IsNotNullData((object)dataTable))
        {
            return dataTable;
        }

        FilterParameterCollection parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.AddLanguageID();
        parameterList.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
        parameterList.Add(new FilterParameter("@MerchantList", MerchantList ?? string.Empty, DbType.String));

        if (IsCSViewFullCard)
        {
            parameterList.AddDecryptDataParams("AccountNumber");
        }

        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetFlatReportChargebackDetail_MultiMerchant", parameterList);
    }

    private string FormatChargebackData(DataTable data)
    {
        CalculateChargebackTotal(data);
        return FormatChargeback(data);

    }

    private string FormatChargeback(DataTable data)
    {
        StringBuilder list = new StringBuilder();
        var totalData = data.Rows.Count;

        for (int i = 0; i < totalData; i++)
        {
            DataRow row = data.Rows[i];

            TableColumnContent uxPartialAccountNumber = new TableColumnContent(Alignment.Center);
            uxPartialAccountNumber.Text = VeraCodeSolution.DoVeraCode(row["PartialAccountNumber"].ToString());

            TableColumnContent uxTransactionDate = new TableColumnContent(Alignment.Center);
            uxTransactionDate.Text = VeraCodeSolution.DoVeraCode(uxTransactionDate.CustomFormatDataValue(row["TransactionDate"], FormatType.Date));

            TableColumnContent reasonCode = new TableColumnContent(Alignment.Left);
            reasonCode.Text = VeraCodeSolution.DoVeraCode(row["ReasonCode"].ToASString() + "&nbsp;");

            TableColumnContent keyed = new TableColumnContent(Alignment.Center);
            if (row["Keyed"].IsNullOrEmpty())
            {
                keyed.Text = WebSiteConstants.HTML_EM_DASH;
            }
            else
            {
                keyed.Text = VeraCodeSolution.DoVeraCode(row["Keyed"].ToASString());
            }

            TableColumnContent uxTransactionAmount = new TableColumnContent(Alignment.Right);
            uxTransactionAmount.Text = FormatData.FormatCurrency(row["TransactionAmount"].ToDecimalAmount(), SessionManager.CurrencyFortmat);
            
            TableColumnContent cardType = new TableColumnContent(Alignment.Center);
            cardType.Text = VeraCodeSolution.DoVeraCode(row["CardType"].ToASString());

            TableColumnContent reportDate = new TableColumnContent(Alignment.Center);
            reportDate.Text = VeraCodeSolution.DoVeraCode(reportDate.CustomFormatDataValue(row["ReportDate"], FormatType.Date));

            list.Append(string.Format("<tr class='{0}'>", i % 2 == 0 ? "Row" : "AltRow"));
            list.Append(uxPartialAccountNumber.RenderHtml());
            list.Append(cardType.RenderHtml());
            list.Append(reportDate.RenderHtml());
            list.Append(uxTransactionDate.RenderHtml());
            list.Append(reasonCode.RenderHtml());
            list.Append(keyed.RenderHtml());
            list.Append(uxTransactionAmount.RenderHtml());

            list.Append("</tr>");
        }
        // Append total row
        if (data.Rows.Count > 0)
            list.Append(ChargebackTotal());

        return list.ToString();
    }

    private string ChargebackTotal()
    {
        StringBuilder line = new StringBuilder();

        line.Append("<tr class='Footer'>");
        line.Append(string.Format("<td align='left'>{0}: {1}</td>", GetLocalResourceObject("DQNextQReportCS_Text_Total").ToString(), VeraCodeSolution.DoVeraCode(_totalRows.ToString())));
        line.Append("<td></td><td></td><td></td><td></td>");
        line.Append(string.Format("<td align='center'>{0}</td>", VeraCodeSolution.DoVeraCode(_ChargebackCountKeyed.ToString())));
        line.Append(string.Format("<td align='right'>{0}</td>", VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(_ChargebackAmount, SessionManager.CurrencyFortmat))));
        line.Append("</tr>");
        return line.ToString();
    }

    private string ChargebackHtml()
    {
        var charbackData = GetChargebackList();
        string data = FormatChargebackData(charbackData);
        if (string.IsNullOrEmpty(data))
            return string.Empty;
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        StringBuilder strs = new StringBuilder();

        //Header
        strs.Append("<table> <tr>");
        uxPartialAccountNumberHeader.RenderControl(hw);
        TableColumnHeader44.RenderControl(hw);
        TableColumnHeader45.RenderControl(hw);
        TableColumnHeader26.RenderControl(hw);
        TableColumnHeader27.RenderControl(hw);
        TableColumnHeader28.RenderControl(hw);
        TableColumnHeader29.RenderControl(hw);
        strs.Append(tw.ToString());
        strs.Append("</tr>");

        //Data
        strs.Append(data);
        strs.Append("</table>");

        return strs.ToString();
    }

    private void ExportToExcel(string htmlString)
    {
        htmlString = "<html xmlns=\"http://www.w3.org/1999/xhtml\"><meta http-equiv=\"content-type\" content=\"application/xhtml+xml; charset=UTF-8\" />  "
                + @"
                     <head>
                        <style type='text/css'>
                            table tr{
                                border: thin solid #333;
                            }  
                            tr.Footer td
                            {
                                font-weight:bold;
                                font-style: italic;    
                            }
                            .rgMasterTable, .MPSBorder{ border:thin solid #000;} 
                            .sub-table{ border:0px solid #000;}
                            .rgHeader{ background-color: #ddd; font-weight:bold; } 
                            table.MPSBorder td.Caption{ background-color: #ddd;}
                        </style>
                    </head>"
            + htmlString + "</html>";
        htmlString = htmlString.Replace("td class=\"heading", "td style=\"font-weight:bold;\" class=\"");
        htmlString = htmlString.Replace("!important", string.Empty);
        htmlString = htmlString.Replace("class=\"Caption AltRow\"", "class=\"Caption\"");
        string fileName = string.Format("90-DayCB{0}", DateTime.UtcNow.ToString("yyyyMMddhhmm"));
        Response.Clear(); //this clears the Response of any headers or previous output
        Response.Buffer = true; //make sure that the entire output is rendered simultaneously
        Response.ContentType = "application/vnd.ms-excel";
        HttpContext.Current.Response.AppendHeader("content-disposition", AS.Common.VeraCodeSolution.RemoveCRLF(string.Format("attachment; filename={0}.xls", fileName)));
        Response.Write(htmlString);
        Response.End();
        IsExport = false;
    }

    protected override void Render(HtmlTextWriter writer)
    {
        if (IsExport)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter hWriter = new HtmlTextWriter(sw);
            base.Render(hWriter);

            var exportHtml = new StringBuilder();
            string dataCB = ChargebackHtml();

            var merchantName = string.Empty;
            if (RiskSessionManager.RiskReportMerchantInfo != null && RiskSessionManager.RiskReportMerchantInfo.Rows.Count > 0)
            {
                var rowData = RiskSessionManager.RiskReportMerchantInfo.Rows[0];
                merchantName = rowData[MERCHANT_NAME].ToString();
            }
            exportHtml.Append(string.Format("<br/><div><strong>Merchant Name: </strong>{0}</div>", merchantName));
            exportHtml.Append(string.Format("<div><strong>Merchant Number: </strong>{0}</div>", RiskSessionManager.currentMerchantNumber));
            exportHtml.Append(string.Format("<br/><div><strong>{0}</strong></div>", GetLocalResourceObject("litHeaderChargeback90daysResource1.Text").ToString()));
            exportHtml.Append(dataCB);

            var html = Regex.Replace(exportHtml.ToString(), "<input.*? />", string.Empty);
            ExportToExcel(html);
        }
        else
        {
            base.Render(writer);
        }
    }

    private FilterParameterCollection GetChargeBacksParameters()
    {
        FilterParameterCollection parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.AddLanguageID();
        parameterList.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
        parameterList.Add(new FilterParameter("@MerchantList", MerchantList, DbType.String));

        if (IsCSViewFullCard)
        {
            parameterList.AddDecryptDataParams("AccountNumber");
        }
        return parameterList;
    }

    public void BindDataFromThread(List<DataSourceParallelResponse> dataSources)
    {
        DataSources = dataSources;
        BindChargeback();
    }


    #region SPA INFO
    public SpaInfo SpaGetChargeBacks90Days
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindChargeback.ToString(),
                SpaName = SPA_GET_CHARGEBACK,
                Parameters = GetChargeBacksParameters()
            };
        }
    }
    #endregion  
}