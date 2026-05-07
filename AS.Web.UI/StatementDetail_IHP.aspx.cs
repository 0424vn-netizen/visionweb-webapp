using AS.Common;
using AS.Common.DBManager;
using AS.Common.Utilities;
using AS.Controls.Pages;
using Dundas.Charting.WebControl;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSupergoo.ABCpdf9;

[PagePermission("StatementRpt,MSStatementRpt")]
public partial class StatementDetail_IHP : ReportPage
{
    #region Enum
    enum DataBindAction
    {
        BindMerchantInfor
        ,
        BindChart
            ,
        BindMerchantAccountInformationSummary
            ,
        BindDeposit
            ,
        BindAdjustment
            ,
        BindSettelement
            ,
        BindProducts
            ,
        BindDue
            , BindInterchange
    }
    enum PostBackAction
    {
        ExportPDF
    }
    #endregion

    #region Properties
    string MerchantNumber = "";
    protected DateTime ReportDate = DateTime.Now;
    protected string HeaderString = "";
    protected string MinBillAdjustment = "0.0";
    protected string total_debit = "0.0";
    protected int _lastIndex;
    const int defaultValue = 123;
    const string defaultString = "123";
    const int FooterSize = 25;
    #endregion

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;

        this.GridIDs.Add("uxDeposits");
        this.GridIDs.Add("uxCardSumary");
        this.GridIDs.Add("uxSettlementDiscount");
        this.GridIDs.Add("uxSurcharge");
        this.GridIDs.Add("uxOtherFee");
        base.PageInitialize();

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        if (IsSecureQueryString)
        {
            this.MerchantNumber = SecureQueryString["MerchantNumber"];
            string _reportdate = SecureQueryString["ReportDate"];
            //DateTime tempDate = new DateTime(2012, 10, 31);
            //this.MerchantNumber = "216";
            //string _reportdate = tempDate.Ticks.ToString();
            this.ReportDate = new DateTime(long.Parse(_reportdate));
            if (!Page.IsPostBack)
            {
                OnDataBindControls(DataBindAction.BindMerchantInfor);
                OnDataBindControls(DataBindAction.BindMerchantAccountInformationSummary);
                OnDataBindControls(DataBindAction.BindDeposit);
                OnDataBindControls(DataBindAction.BindAdjustment);
                OnDataBindControls(DataBindAction.BindSettelement);
                OnDataBindControls(DataBindAction.BindProducts);
                OnDataBindControls(DataBindAction.BindDue);
                OnDataBindControls(DataBindAction.BindInterchange);
            }
            OnDataBindControls(DataBindAction.BindChart);
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    #region get SPAs

    private DataTable GetMerchantInfomation(FilterParameterCollection _Parameters)
    {
        DataTable merchantInfor = WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantHeaderInformation_mps", _Parameters);
        if (merchantInfor.Rows.Count > 0)
        {
            uxClientAddress1_PDF.Text = uxClientAddress1.Text = VeraCodeSolution.DoVeraCode(merchantInfor.Rows[0]["ReturnAddressLine1"].ToString());
            uxClientAddress2_PDF.Text = uxClientAddress2.Text = VeraCodeSolution.DoVeraCode(merchantInfor.Rows[0]["ReturnAddressLine2"].ToString());
            uxClientAddress1.Font.Size = uxClientAddress2.Font.Size = FontUnit.Parse("12px");
        }
        return merchantInfor;
    }
    private DataTable GetSettlementDiscount(FilterParameterCollection _Parameters)
    {
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantSettleDiscount_mps", _Parameters);
    }
    private DataTable GetSettlementDiscount_Total(FilterParameterCollection _Parameters)
    {
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantSettleDiscount_Total_mps", _Parameters);
    }

    private DataTable GetProductsServices(FilterParameterCollection _Parameters)
    {
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantProductService_mps", _Parameters);
    }
    private DataTable GetProductsServices_Total(FilterParameterCollection _Parameters)
    {
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantProductService_Total_mps", _Parameters);
    }
    private DataTable GetDuesAssessments(FilterParameterCollection _Parameters)
    {
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantDuesAssessments_mps", _Parameters);
    }
    private DataTable GetDuesAssessments_Total(FilterParameterCollection _Parameters)
    {
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantDuesAssessments_Total_mps", _Parameters);
    }
    private DataTable GetInterchange(FilterParameterCollection _Parameters)
    {
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantInterchange_mps", _Parameters);
    }
    private DataTable GetInterchange_Total(FilterParameterCollection _Parameters)
    {
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantInterchange_Total_mps", _Parameters);
    }

    private DataTable GetDepositDetail(FilterParameterCollection _Parameters)
    {
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantDepositDetail_mps", _Parameters);
    }
    private DataTable GetDepositDetail_BatchTotal(FilterParameterCollection _Parameters)
    {
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantDepositDetail_Total_mps", _Parameters);
    }
    private DataTable GetDepositDetail_Adjustment(FilterParameterCollection _Parameters)
    {
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantAdjustmentCredits_mps", _Parameters);
    }

    #endregion

    #region BindChart

    private void BindVolumeCardType(FilterParameterCollection _Parameters)
    {
        DataTable dtChart = WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantAccountVolumeChart_mps", _Parameters);

        if (dtChart == null || dtChart.Rows.Count <= 0)
        {
            uxVolumeCardTypeChart.Series["Default"].Points.Clear();
            uxVolumeCardTypeChartForPrint.Series["Default"].Points.Clear();
            return;
        }

        for (int i = 0; i < dtChart.Rows.Count; i++)
        {
            DataRow dr = dtChart.Rows[i];
            DataPoint point = new DataPoint();
            point.LegendText = dtChart.Rows[i]["CardType"].ToString();
            switch (dtChart.Rows[i]["CardType"].ToString())
            {
                case "VISA":
                    {
                        point.Color = System.Drawing.ColorTranslator.FromHtml("#4573A7");
                        break;
                    }
                case "EBT":
                    {
                        point.Color = System.Drawing.ColorTranslator.FromHtml("#DC833F");
                        break;
                    }
                case "PIN DBT":
                    {
                        point.Color = System.Drawing.ColorTranslator.FromHtml("#3C9BAF");
                        break;
                    }
                case "AMEX":
                    {
                        point.Color = System.Drawing.ColorTranslator.FromHtml("#71588F");
                        break;
                    }
                case "DS":
                    {
                        point.Color = System.Drawing.ColorTranslator.FromHtml("#89A54E");
                        break;
                    }
                case "MC":
                    {
                        point.Color = System.Drawing.ColorTranslator.FromHtml("#AA4644");
                        break;
                    }

            }
            if (Convert.ToDouble(dr["PercentVolume"]) == 0) point.Empty = true;
            point.YValues[0] = Convert.ToDouble(dr["PercentVolume"]);
            point.ToolTip = (Convert.ToDouble(dr["PercentVolume"]) * 100).ToString() + "%";

            uxVolumeCardTypeChart.Series["Default"].Points.Add(point);
            uxVolumeCardTypeChartForPrint.Series["Default"].Points.Add(point);
        }

    }

    private void BindYTD(FilterParameterCollection _Parameters)
    {
        DataTable _YTDData = WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantAccount12MonthVolumeChart_mps", _Parameters);
        if (_YTDData == null || _YTDData.Rows.Count <= 0)
        {
            uxVolumeYTDChart.Series["Default"].Points.Clear();
            uxVolumeYTDChartForPrint.Series["Default"].Points.Clear();
            return;
        }

        int MonthsNumber = 12;

        decimal _YTDTotalVolume = 0;
        for (int i = 0; i < MonthsNumber; i++)
        {
            _YTDTotalVolume += (decimal)_YTDData.Rows[i]["Volume"];

        }

        object[] xValues = new object[MonthsNumber];
        object[] yVolumes = new object[MonthsNumber];
        uxVolumeYTDChart.Series["Default"].Points.Clear();
        uxVolumeYTDChartForPrint.Series["Default"].Points.Clear();
        for (int i = 0; i < MonthsNumber; i++)
        {
            xValues[i] = _YTDData.Rows[i]["Month"];
        }

        for (int i = 0; i < MonthsNumber; i++)
        {
            yVolumes[i] = _YTDData.Rows[i]["Volume"];
        }

        uxVolumeYTDChart.Series["Default"].Points.DataBindXY(xValues, yVolumes);
        uxVolumeYTDChartForPrint.Series["Default"].Points.DataBindXY(xValues, yVolumes);

        if (_YTDTotalVolume == 0)
        {
            uxVolumeYTDChartForPrint.ChartAreas["Area1"].AxisY.Enabled = uxVolumeYTDChart.ChartAreas["Area1"].AxisY.Enabled = AxisEnabled.True;
            uxVolumeYTDChartForPrint.ChartAreas["Area1"].AxisY.StartFromZero = uxVolumeYTDChart.ChartAreas["Area1"].AxisY.StartFromZero = true;
            uxVolumeYTDChartForPrint.ChartAreas["Area1"].AxisY.Minimum = uxVolumeYTDChart.ChartAreas["Area1"].AxisY.Minimum = 0;
            uxVolumeYTDChartForPrint.ChartAreas["Area1"].AxisY.Maximum = uxVolumeYTDChart.ChartAreas["Area1"].AxisY.Maximum = 4000;
            uxVolumeYTDChartForPrint.ChartAreas["Area1"].AxisY.Interval = uxVolumeYTDChart.ChartAreas["Area1"].AxisY.Interval = 2000;
            uxVolumeYTDChartForPrint.ChartAreas["Area1"].AxisX.LabelsAutoFit = uxVolumeYTDChart.ChartAreas["Area1"].AxisX.LabelsAutoFit = true;
            uxVolumeYTDChartForPrint.ChartAreas["Area1"].AxisX.LabelsAutoFitStyle = uxVolumeYTDChart.ChartAreas["Area1"].AxisX.LabelsAutoFitStyle = LabelsAutoFitStyle.OffsetLabels;
        }

    }


    private void BindMerchantAccountInformationSummary(FilterParameterCollection _Parameters)
    {
        DataTable merchantAccountInformationSummary = WebServices.CsReportServices.GetReports("spa_stmnt_GetMerchantAccountInformationSummary_mps", _Parameters);

        uxMerchantAccountInformationSummary.DataSource = merchantAccountInformationSummary;
        uxMerchantAccountInformationSummary.DataBind();
        uxMerchantAccountInformationSummary_PDF.DataSource = merchantAccountInformationSummary;
        uxMerchantAccountInformationSummary_PDF.DataBind();
        uxMerchantAccountInformationSummaryForPrint.DataSource = merchantAccountInformationSummary;
        uxMerchantAccountInformationSummaryForPrint.DataBind();
    }

    #endregion

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection _Parameters = new FilterParameterCollection();
        _Parameters.AddLoggedInUserReportingParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMerchantInfor:
                {
                    uxMerchantInfo.DataSource = GetMerchantInfomation(_Parameters);
                    uxMerchantInfo.DataBind();
                    uxMerchantInfor_PDF.DataSource = GetMerchantInfomation(_Parameters);
                    uxMerchantInfor_PDF.DataBind();
                    uxMerchantInfo_Header.DataSource = GetMerchantInfomation(_Parameters);
                    uxMerchantInfo_Header.DataBind();
                    break;
                }
            case DataBindAction.BindChart:
                {
                    BindVolumeCardType(_Parameters);
                    BindYTD(_Parameters);
                    break;

                }
            case DataBindAction.BindMerchantAccountInformationSummary:
                {
                    BindMerchantAccountInformationSummary(_Parameters);
                    break;
                }
            case DataBindAction.BindDeposit:
                {
                    DataTable depositTable = GetDepositDetail(_Parameters);
                    if (depositTable.Rows.Count > 0)
                    {
                        uxDeposits.DataSource = depositTable;
                        uxDeposits.DataBind();

                        uxDeposits_BatchTotal.DataSource = GetDepositDetail_BatchTotal(_Parameters);
                        uxDeposits_BatchTotal.DataBind();

                        uxDeposits_Adjustment.DataSource = GetDepositDetail_Adjustment(_Parameters);
                        uxDeposits_Adjustment.DataBind();
                    }
                    else
                    {
                        pnlDeposit.Visible = false;
                    }
                    break;
                }
            case DataBindAction.BindAdjustment:
                {
                    uxAdjustment.DataSource = new DataTable();
                    uxAdjustment.DataBind();

                    pnlAdjustment.Visible = false;
                    break;
                }
            case DataBindAction.BindSettelement:
                {
                    DataTable settlementTable = GetSettlementDiscount(_Parameters);
                    if (settlementTable.Rows.Count > 0)
                    {
                        uxSettelement.DataSource = settlementTable;
                        uxSettelement.DataBind();

                        uxSettelement_Total.DataSource = GetSettlementDiscount_Total(_Parameters);
                        uxSettelement_Total.DataBind();
                    }
                    else
                    {
                        pnlSettlement.Visible = false;
                    }
                    break;
                }
            case DataBindAction.BindProducts:
                {
                    DataTable productTable = GetProductsServices(_Parameters);
                    if (productTable.Rows.Count > 0)
                    {
                        uxProducts.DataSource = productTable;
                        uxProducts.DataBind();

                        uxProducts_Total.DataSource = GetProductsServices_Total(_Parameters);
                        uxProducts_Total.DataBind();
                    }
                    else
                    {
                        pnlProducts.Visible = false;
                    }

                    break;
                }
            case DataBindAction.BindDue:
                {
                    DataTable dueTable = GetDuesAssessments(_Parameters);
                    if (dueTable.Rows.Count > 0)
                    {
                        uxDue.DataSource = dueTable;
                        uxDue.DataBind();

                        uxDue_Total.DataSource = GetDuesAssessments_Total(_Parameters);
                        uxDue_Total.DataBind();
                    }
                    else
                    {
                        pnlDue.Visible = false;
                    }
                    break;
                }
            case DataBindAction.BindInterchange:
                {
                    DataTable interchangeTable = GetInterchange(_Parameters);
                    if (interchangeTable.Rows.Count > 0)
                    {
                        uxInterchange.DataSource = interchangeTable;
                        uxInterchange.DataBind();

                        uxInterchange_Total.DataSource = GetInterchange_Total(_Parameters);
                        uxInterchange_Total.DataBind();
                    }
                    else
                    {
                        pnlInterchange.Visible = false;
                    }
                    break;
                }
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.ExportPDF:
                {
                    string storedFile = System.Guid.NewGuid().ToString();

                    ExportPDF(Server.MapPath("~/App_Data/ExportedFiles/" + storedFile + ".pdf"));
                    break;
                }
        }

    }

    protected void uxExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        OnPostBackActions(PostBackAction.ExportPDF);
    }
    public void ExportPDF(string fileName)
    {
        PdfFactory pdfCreator = new PdfFactory();
        pdfCreator.SetHeaderSize(120);
        pdfCreator.SetFooterSize(FooterSize);
        pdfCreator.CreateHeader += new PdfFactory.PdfFactoryHanlder(pdfCreator_CreateHeader);
        pdfCreator.CreateContent += new PdfFactory.PdfFactoryHanlder(pdfCreator_CreateContent);
        pdfCreator.CreateFooter += new PdfFactory.PdfFactoryHanlder(pdfCreator_CreateFooter);
        pdfCreator.Create(Response.OutputStream);

        string trueFileName = GetLocalResourceObject("StatementDetail_IHP_aspx_cs_StatementFilenameStatement").ToString() + "_" + MerchantNumber + "_" + this.ReportDate.Month.ToString("0#") + "_" + this.ReportDate.Year.ToString();
        Response.AppendHeader("content-disposition", "attachment; filename=" + trueFileName + ".pdf");
        Response.ContentType = "application/pdf";
        Response.Flush();
        Response.End();

    }
    void pdfCreator_CreateContent(PdfFactory.PdfAgent pdfAgent, XRect rect)
    {
        string fileName = GetLocalResourceObject("StatementDetail_IHP_aspx_cs_CardTypeFilename").ToString() + "_" + this.MerchantNumber + "_" + DateTime.Now.Ticks.ToString() + ".gif";
        string filePath = Server.MapPath("~/App_Themes/MPS/images/" + fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            uxVolumeCardTypeChart.Save(filePath);
        }
        else
            uxVolumeCardTypeChart.Save(filePath);

        //YTD
        string fileName_YTD = "YTD_" + this.MerchantNumber + "_" + DateTime.Now.Ticks.ToString() + ".gif";
        string filePath_YTD = Server.MapPath("~/App_Themes/MPS/images/" + fileName_YTD);
        if (File.Exists(filePath_YTD))
        {
            File.Delete(filePath_YTD);
            uxVolumeYTDChart.Save(filePath_YTD);
        }
        else
            uxVolumeYTDChart.Save(filePath_YTD);

        FilterParameterCollection _Parameters = new FilterParameterCollection();
        _Parameters.AddLoggedInUserReportingParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);

        Doc doc = pdfAgent.PdfDoc;

        doc.FontSize = 7;

        doc.Font = doc.AddFont("Arial");

        string headerStyle = "<p align=\"center\">{0}</p>";
        string headerStyle_Left = "<p align=\"left\">{0}</p>";

        string domain = string.Format("file:///{0}", Server.MapPath("~/"));  //ConfigurationManager.AppSettings["VisionWeb_UrlByIP"];//string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath);

        string imgPieChart = String.Format("{0}{1}", domain, "/App_Themes/MPS/images/" + fileName);
        string imgBarChart = String.Format("{0}{1}", domain, "/App_Themes/MPS/images/" + fileName_YTD);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter hWriter = new HtmlTextWriter(sw);
        uxMerchantAccountInformationSummary_PDF.Visible = true;
        uxMerchantAccountInformationSummary_PDF.RenderControl(hWriter);
        uxMerchantAccountInformationSummary_PDF.Visible = false;
        string merchantAccountInfor = "<style type='text/css'>body{background-color:#fff;font-family:Arial;font-size:10px;color:#000000}</style>" + String.Format("<table style='font-family=Arial !important;font-size:10px !important;background-color:#fff;color:#000000'><tr><td>{0}</td><td>{1}</td><td>{2}</td></tr></table>", String.Format("<img src='{0}'/>", imgPieChart), String.Format("<img src='{0}'/>", imgBarChart), sb.ToString());

        pdfAgent.NewLine();

        pdfAgent.AddHtml("<p><font color='#000000'>&nbsp;" + GetLocalResourceObject("Literal23Resource2.Text").ToString() + "</font><p>", "0 164 228", 3);

        pdfAgent.NewLine();

        for (int i = 0; i < 17; i++)
        {
            pdfAgent.NewLine();
        }

        double chartPosY = doc.Pos.Y;

        if (doc.Pos.Y + 2 * doc.FontSize < rect.Bottom)
        {
            pdfAgent.NewPage();
        }

        pdfAgent.NewLine();

        pdfAgent.AddHtml("<p><font color='#000000'>&nbsp;" + GetLocalResourceObject("Literal47Resource2.Text").ToString() + "</font></p>", "0 164 228", 3);

        pdfAgent.NewLine();

        DataTable dtDepopsit_Total = GetDepositDetail_BatchTotal(_Parameters);
        DataTable dtDeposit = GetDepositDetail(_Parameters);
        _lastIndex = dtDeposit.Rows.Count + 1;

        if (dtDepopsit_Total.Rows.Count > 0)
        {
            DataRow newRow = dtDeposit.NewRow();
            newRow["DepositDate"] = DateTime.Now;
            newRow["ReferenceNumber"] = dtDepopsit_Total.Rows[0]["Deposits"];
            newRow["Items"] = dtDepopsit_Total.Rows[0]["Items"];
            newRow["Sales"] = dtDepopsit_Total.Rows[0]["Sales"];
            newRow["Credits"] = dtDepopsit_Total.Rows[0]["Credits"];
            newRow["Discounts"] = dtDepopsit_Total.Rows[0]["Discounts"];
            newRow["NetDeposit"] = dtDepopsit_Total.Rows[0]["NetDeposit"];
            dtDeposit.Rows.Add(newRow);
        }
        DataTable dtDepopsit_Adjustment = GetDepositDetail_Adjustment(_Parameters);
        if (dtDepopsit_Adjustment.Rows.Count > 0)
        {
            DataRow newRow = dtDeposit.NewRow();
            newRow["DepositDate"] = DateTime.Now;
            newRow["ReferenceNumber"] = defaultString;
            newRow["Items"] = dtDepopsit_Adjustment.Rows[0]["Items"];
            newRow["Sales"] = dtDepopsit_Adjustment.Rows[0]["Sales"];
            newRow["Credits"] = dtDepopsit_Adjustment.Rows[0]["Credits"];
            newRow["Discounts"] = defaultValue;
            newRow["NetDeposit"] = defaultValue;
            dtDeposit.Rows.Add(newRow);
        }
        pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "Deposit", Border = PdfFactory.TableSettings.BorderType.None },
                new PdfFactory.ColumnSettings[]{
                new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle_Left,GetLocalResourceObject("Literal47Resource3.Text").ToString()),DataField="DepositDate",Width=55,FormatString="{0:MM/dd/yyyy}"},
                new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal48Resource1.Text").ToString()),DataField="ReferenceNumber",Width=50},
                new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal49Resource1.Text").ToString()),DataField="Items",Width=20,FormatString="<p align=\"right\">{0}</p>"},
                new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal50Resource1.Text").ToString()),DataField="Sales",Width=30,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
                new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal51Resource1.Text").ToString()),DataField="Credits",Width=30,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
                new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal52Resource1.Text").ToString()),DataField="Discounts",Width=25,FormatString="<p align=\"right\">{0:#,##0.0000}</p>"},
                new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal53Resource1.Text").ToString()),DataField="NetDeposit",Width=30,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
            }, dtDeposit, FormatCell);

        if (pnlSettlement.Visible)
        {
            pdfAgent.NewLine();

            if (doc.Pos.Y - FooterSize < rect.Bottom)
            {
                pdfAgent.NewPage();
            }

            pdfAgent.AddHtml("<p><font color='#000000'>&nbsp;" + GetLocalResourceObject("Literal55Resource2.Text").ToString() + "</font><p>", "0 164 228", 3);

            pdfAgent.NewLine();
            DataTable dtSettelement_Total = GetSettlementDiscount_Total(_Parameters);
            DataTable dtSettelement = GetSettlementDiscount(_Parameters);
            _lastIndex = dtSettelement.Rows.Count + 1;

            if (dtSettelement_Total.Rows.Count > 0)
            {
                DataRow newRow = dtSettelement.NewRow();
                newRow["Description"] = dtSettelement_Total.Rows[0]["Description"];
                newRow["Items"] = dtSettelement_Total.Rows[0]["Items"];
                newRow["Volume"] = dtSettelement_Total.Rows[0]["Volume"];
                newRow["AverageTicket"] = dtSettelement_Total.Rows[0]["AverageTicket"];
                newRow["DiscountRate"] = defaultValue;//dtSettelement_Total.Rows[0]["DiscountRate"];
                newRow["ItemRate"] = defaultValue;// dtSettelement_Total.Rows[0]["ItemRate"];
                newRow["Amount"] = dtSettelement_Total.Rows[0]["Amount"];
                dtSettelement.Rows.Add(newRow);
            }
            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "Settlement", Border = PdfFactory.TableSettings.BorderType.None },
                    new PdfFactory.ColumnSettings[]{
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle_Left,GetLocalResourceObject("Literal55Resource3.Text").ToString()),DataField="Description",Width=60,},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal56Resource1.Text").ToString()),DataField="Items",Width=20,FormatString="<p align=\"right\">{0}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal57Resource1.Text").ToString()),DataField="Volume",Width=30,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal58Resource1.Text").ToString()),DataField="AverageTicket",Width=35,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal59Resource1.Text").ToString()),DataField="DiscountRate",Width=35,FormatString="<p align=\"right\">{0:#,##0.0000}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal60Resource1.Text").ToString()),DataField="ItemRate",Width=25,FormatString="<p align=\"right\">{0:#,##0.0000}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal61Resource1.Text").ToString()),DataField="Amount",Width=30,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
                }, dtSettelement, FormatCell);
        }
        if (pnlProducts.Visible)
        {
            pdfAgent.NewLine();

            if (doc.Pos.Y - FooterSize < rect.Bottom)
            {
                pdfAgent.NewPage();
            }

            pdfAgent.AddHtml("<p><font color='#000000'>&nbsp;" + GetLocalResourceObject("Literal61Resource3.Text").ToString() + "</font><p>", "0 164 228", 3);

            pdfAgent.NewLine();

            DataTable dtProducts_Total = GetProductsServices_Total(_Parameters);
            DataTable dtProducts = GetProductsServices(_Parameters);
            _lastIndex = dtProducts.Rows.Count + 1;

            if (dtProducts_Total.Rows.Count > 0)
            {
                DataRow newRow = dtProducts.NewRow();
                newRow["Description"] = GetLocalResourceObject("StatementDetail_IHP_aspx_cs_Total").ToString();
                newRow["Items"] = defaultValue;
                newRow["Volume"] = defaultValue;
                newRow["VolumeRate"] = defaultValue;//dtSettelement_Total.Rows[0]["DiscountRate"];
                newRow["ItemRate"] = defaultValue;// dtSettelement_Total.Rows[0]["ItemRate"];
                newRow["Amount"] = dtProducts_Total.Rows[0]["Amount"];
                dtProducts.Rows.Add(newRow);
            }
            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "Products", Border = PdfFactory.TableSettings.BorderType.None },
                    new PdfFactory.ColumnSettings[]{
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle_Left,GetLocalResourceObject("Literal61Resource4.Text").ToString()),DataField="Description",Width=60,},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal62Resource1.Text").ToString()),DataField="Items",Width=20,FormatString="<p align=\"right\">{0}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal63Resource1.Text").ToString()),DataField="Volume",Width=30,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal64Resource1.Text").ToString()),DataField="VolumeRate",Width=30,FormatString="<p align=\"right\">{0:#,##0.0000}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal65Resource1.Text").ToString()),DataField="ItemRate",Width=25,FormatString="<p align=\"right\">{0:#,##0.0000}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal66Resource1.Text").ToString()),DataField="Amount",Width=30,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
                }, dtProducts, FormatCell);
        }
        if (pnlDue.Visible)
        {
            pdfAgent.NewLine();

            if (doc.Pos.Y - FooterSize < rect.Bottom)
            {
                pdfAgent.NewPage();
            }

            pdfAgent.AddHtml("<p><font color='#000000'>&nbsp;" + GetLocalResourceObject("Literal66Resource3.Text").ToString() + "</font><p>", "0 164 228", 3);

            pdfAgent.NewLine();

            DataTable dtDue_Total = GetDuesAssessments_Total(_Parameters);
            DataTable dtDue = GetDuesAssessments(_Parameters);
            _lastIndex = GetDuesAssessments(_Parameters).Rows.Count + 1;

            if (dtDue_Total.Rows.Count > 0)
            {
                DataRow newRow = dtDue.NewRow();
                newRow["Description"] = GetLocalResourceObject("StatementDetail_IHP_aspx_cs_Total").ToString();
                newRow["Items"] = defaultValue;
                newRow["Volume"] = defaultValue;
                newRow["VolumeRate"] = defaultValue;//dtSettelement_Total.Rows[0]["DiscountRate"];
                newRow["ItemRate"] = defaultValue;// dtSettelement_Total.Rows[0]["ItemRate"];
                newRow["Amount"] = dtDue_Total.Rows[0]["Amount"];
                dtDue.Rows.Add(newRow);
            }
            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "Due", Border = PdfFactory.TableSettings.BorderType.None },
                    new PdfFactory.ColumnSettings[]{
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle_Left,GetLocalResourceObject("Literal66Resource4.Text").ToString()),DataField="Description",Width=60,},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal67Resource1.Text").ToString()),DataField="Items",Width=20,FormatString="<p align=\"right\">{0}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal68Resource1.Text").ToString()),DataField="Volume",Width=30,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal69Resource1.Text").ToString()),DataField="VolumeRate",Width=30,FormatString="<p align=\"right\">{0:#,##0.0000}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal70Resource1.Text").ToString()),DataField="ItemRate",Width=25,FormatString="<p align=\"right\">{0:#,##0.0000}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal71Resource1.Text").ToString()),DataField="Amount",Width=30,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
                }, dtDue, FormatCell);

        }

        if (pnlInterchange.Visible)
        {
            pdfAgent.NewLine();

            pdfAgent.AddHtml("<p><font color='#000000'>&nbsp;" + GetLocalResourceObject("Literal71Resource3.Text").ToString() + "</font><p>", "0 164 228", 3);

            pdfAgent.NewLine();

            DataTable dtInterchange_Total = GetInterchange_Total(_Parameters);
            DataTable dtInterchange = GetInterchange(_Parameters);
            _lastIndex = GetInterchange(_Parameters).Rows.Count + 1;

            if (dtInterchange_Total.Rows.Count > 0)
            {
                DataRow newRow = dtInterchange.NewRow();
                newRow["CardType"] = GetLocalResourceObject("StatementDetail_IHP_aspx_cs_Total").ToString();
                newRow["Description"] = defaultString;
                newRow["Items"] = dtInterchange_Total.Rows[0]["Items"];
                newRow["Volume"] = dtInterchange_Total.Rows[0]["Volume"];
                newRow["VolumeRate"] = defaultValue;//dtSettelement_Total.Rows[0]["DiscountRate"];
                newRow["ItemRate"] = defaultValue;// dtSettelement_Total.Rows[0]["ItemRate"];
                newRow["Amount"] = dtInterchange_Total.Rows[0]["Amount"];
                dtInterchange.Rows.Add(newRow);
            }
            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "Interchange", Border = PdfFactory.TableSettings.BorderType.None },
                     new PdfFactory.ColumnSettings[]{
                             new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle_Left,GetLocalResourceObject("Literal71Resource4.Text").ToString()),DataField="CardType",Width=30,},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle_Left,GetLocalResourceObject("Literal72Resource1.Text").ToString()),DataField="Description",Width=60,},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal73Resource1.Text").ToString()),DataField="Items",Width=20,FormatString="<p align=\"right\">{0}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal74Resource1.Text").ToString()),DataField="Volume",Width=30,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal75Resource1.Text").ToString()),DataField="VolumeRate",Width=30,FormatString="<p align=\"right\">{0:#,##0.0000}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal76Resource1.Text").ToString()),DataField="ItemRate",Width=25,FormatString="<p align=\"right\">{0:#,##0.0000}</p>"},
                    new PdfFactory.ColumnSettings(){HeaderText=string.Format(headerStyle,GetLocalResourceObject("Literal77Resource1.Text").ToString()),DataField="Amount",Width=30,FormatString="<p align=\"right\">{0:#,##0.00}</p>"},
                }, dtInterchange, FormatCell);

            //pdfAgent.NewLine();
            //string message3 = "\"INCREASINGLY, CONSUMERS ARE LOOKING FOR A GREAT DEAL AND SAVINGS - WHETHER THAT IS " +
            //                "IN PAPER COUPONS OR THROUGH DIGITAL CHANNELS THAT CREATE A " +
            //                "MORE ENHANCED HOLISTIC SHOPPING EXPERIENCE,\" SAID MARIO SHILIASHKI, GROUP HEAD, " +
            //                "U.S MARKETS EMERGING PAYMENTS LEAD, MASTERCARD. \"OUR " +
            //                "COLLABORATION WITH PARTNERS LIKE LOCAL OFFER NETWORK WILL MAKE MASTERCARD THE 'GO-TO' " +
            //                "OFFERS SOLUTION FOR MERCHANTS AND ISSUERS LOOKING FOR " +
            //                "A STRONGER CONNECTION WITH OUR CARDHOLDERS.\"";

            //pdfAgent.AddHtml(message3);


        }


        //reset page/position
        doc.PageNumber = 1;
        doc.Pos.Y = chartPosY;
        XRect rectChart = new XRect();
        rectChart.Move(doc.Pos.X, chartPosY);
        rectChart.Resize(doc.Rect.Width, 120);
        doc.Rect.SetRect(rectChart);
        doc.AddImageHtml(merchantAccountInfor);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        if (File.Exists(filePath_YTD))
        {
            File.Delete(filePath_YTD);
        }

    }
    void pdfCreator_CreateHeader(PdfFactory.PdfAgent pdfAgent, XRect rect)
    {
        Doc doc = pdfAgent.PdfDoc;

        string domain = string.Format("file:///{0}", Server.MapPath("~/"));  //ConfigurationManager.AppSettings["VisionWeb_UrlByIP"]; //string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath);

        uxImageLogo.Src = String.Format("{0}{1}", domain, "/App_Themes/MPS/images/logo.jpg");


        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter hWriter = new HtmlTextWriter(sw);
        Logo.Visible = true;
        Logo.RenderControl(hWriter);
        Logo.Visible = false;

        string pdf = String.Empty;
        if (doc.PageNumber == 1)
        {
            System.Text.StringBuilder sb_Infor = new System.Text.StringBuilder();
            StringWriter sw_Infor = new StringWriter(sb_Infor);
            HtmlTextWriter hWriter_Infor = new HtmlTextWriter(sw_Infor);
            uxMerchantInfor_PDF.Visible = true;
            uxMerchantInfor_PDF.RenderControl(hWriter_Infor);
            uxMerchantInfor_PDF.Visible = false;
            pdf = "<style type='text/css'>body{background-color:#ffffff;color:#000000}</style>" + string.Format("<div style='background-color:#fff; font-family:Arial; font-size:10px;color:#000000'>{0}</div>", sb.ToString() + "<br style='clear: both'/><br style='clear: both' />" + sb_Infor.ToString());
        }

        if (doc.PageNumber != 1)
        {
            System.Text.StringBuilder sb_Header = new System.Text.StringBuilder();
            StringWriter sw_Header = new StringWriter(sb_Header);
            HtmlTextWriter hWriter_Header = new HtmlTextWriter(sw_Header);
            uxMerchantInfo_Header.Visible = true;
            uxMerchantInfo_Header.RenderControl(hWriter_Header);
            uxMerchantInfo_Header.Visible = false;
            pdf = "<style type='text/css'>body{background-color:#ffffff;color:#000000}</style>" + string.Format("<div style='background-color:#fff; font-family:Arial; font-size:10px;color:#000000'>{0}</div>", sb.ToString() + "<br style='clear: both'/><br style='clear: both' />" + sb_Header.ToString());
        }

        doc.AddImageHtml(pdf);
    }
    void pdfCreator_CreateFooter(PdfFactory.PdfAgent pdfAgent, XRect rect)
    {
        //Doc doc = pdfAgent.PdfDoc;

        //doc.FontSize = 7;

        //doc.Font = doc.AddFont("Arial");

        //doc.Rect.SetRect(15, 15, 520, FooterSize);

        //doc.FrameRect();

        //XRect rectChart = new XRect();

        //doc.Rect.SetRect(20, 20, 515, FooterSize);

        //string message4 = "NEWS AND OTHER INFORMATION <br>" +
        //                   "“INCREASINGLY, CONSUMERS ARE LOOKING FOR A GREAT DEAL AND SAVINGS - WHETHER THAT IS IN PAPER COUPONS OR THROUGH DIGITAL CHANNELS THAT CREATE A " +
        //                   "MORE ENHANCED HOLISTIC SHOPPING EXPERIENCE,” SAID MARIO SHILIASHKI, GROUP HEAD, U.S. MARKETS EMERGING PAYMENTS LEAD, MASTERCARD. “OUR " +
        //                   "COLLABORATION WITH PARTNERS LIKE LOCAL OFFER NETWORK WILL MAKE MASTERCARD THE ‘GO-TO’ OFFERS SOLUTION FOR MERCHANTS AND ISSUERS LOOKING FOR " +
        //                   "A STRONGER CONNECTION WITH OUR CARDHOLDERS.”";
        //;

        //string message1 = "NEWS AND OTHER INFORMATION <br>" +
        //                  "INTEGRATED PAYMENT PROCESSOR, MERCURY PAYMENT SYSTEMS HAS BEEN NAMED 2012 BEST CHANNEL VENDOR FOR PAYMENT PROCESSING BY BUSINESS " +
        //                  "SOLUTIONS MAGAZINE (BSM) FOR THE FOURTH YEAR IN A ROW. POINT-OF-SALE (POS) RESELLERS SURVEYED BY BSM GAVE  MERCURY AN OVERALL SCORE OF 4.54 ON A " +
        //                  "SCALE OF 0 - 5 IN SEVEN CATEGORIES: SERVICE/SUPPORT, CHANNEL FRIENDLY, CHANNEL PROGRAM, PRODUCT FEATURES, PRODUCT RELIABILITY, PRODUCT " +
        //                  "INNOVATION, AND ADEQUATE VAR MARGINS.";

        //if (doc.PageNumber == 1)
        //{

        //    pdfAgent.AddHtml(String.Format("<p>{0}</p>", message1));
        //}
        //else
        //{
        //    pdfAgent.AddHtml(String.Format("<p>{0}</p>", message4));
        //}
    }
    string FormatCell(string tableName, int rowIndex, int colIndex, string colName, string text, object dataCell, int dataIndex)
    {
        string returnText = text;
        switch (tableName)
        {
            case "Settlement":
                {
                    if (dataIndex == _lastIndex)
                    {
                        if (colIndex == 4 || colIndex == 5)
                        {
                            returnText = String.Empty;
                        }
                    }
                    else
                    {
                        if (colIndex == 5)
                        {
                            if (dataCell == DBNull.Value)
                                return String.Empty;
                        }
                    }
                    break;
                }
            case "Products":
            case "Due":
                {
                    if (dataIndex == _lastIndex)
                    {
                        if (colIndex == 1 || colIndex == 2 || colIndex == 3 || colIndex == 4)
                        {
                            returnText = String.Empty;
                        }
                    }
                    else
                    {
                        if (colIndex == 4) // ItemRate
                        {
                            if (dataCell == DBNull.Value)
                                return String.Empty;
                        }
                    }
                    break;
                }
            case "Interchange":
                {
                    if (dataIndex == _lastIndex)
                    {
                        if (colIndex == 1 || colIndex == 4 || colIndex == 5)
                        {
                            returnText = String.Empty;
                        }
                    }
                    else
                    {
                        if (colIndex == 4)
                        {
                            if (dataCell.ToString() == "0.0000")
                                return String.Empty;
                        }
                        else if (colIndex == 5)
                        {
                            if (dataCell == DBNull.Value)
                            {
                                return string.Empty;
                            }
                        }
                    }
                    break;
                }
            case "Deposit":
                {
                    if (dataIndex == _lastIndex)
                    {
                        if (colIndex == 0)
                        {
                            returnText = GetLocalResourceObject("StatementDetail_IHP_aspx_cs_BatchTotal").ToString();
                        }
                        if (colIndex == 1)
                        {
                            returnText = String.Format("<p align=\"right\">{0}</p>", text);
                        }
                    }
                    if (dataIndex == _lastIndex + 1)
                    {
                        if (colIndex == 0)
                        {
                            returnText = GetLocalResourceObject("StatementDetail_IHP_aspx_cs_SummaryOfAdjustments").ToString();
                        }
                        if (colIndex == 1 || colIndex == 5 || colIndex == 6)
                        {
                            returnText = String.Empty;
                        }
                    }
                    break;
                }
        }
        return returnText;

    }
}
