using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using AS.Controls.Exporter;
using AS.Controls.Pages;
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.UserControls;
using System.Drawing;
using AS.Common.Formater;
using System.Collections.Generic;


[PagePermission("RskQueue,RskAdhoc,MSRskQueue,MSRskAdhoc")]
public partial class rm_MCF_RiskScoreDetailModal : ReportPage
{

    enum DataBindAction
    {
        BindTitleInfo,
        BindParameterGrid,
        BindMerchantClassfisication,
        BindAttributeGrid

    }

    #region properties
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string REPORT_DATE = "ReportDate";
    private const string MERCHANT_NAME = "MerchantName";

    string MerchantNumber = string.Empty;
    string merchantName = string.Empty;
    DateTime ReportDate;

    private string MerchantClassificationName
    {
        get
        {
            if (ViewState["MerchantClassificationName"] == null)
                return string.Empty;
            else
                return ViewState["MerchantClassificationName"].ToString();
        }
        set
        {
            ViewState["MerchantClassificationName"] = value;
        }
    }
    private string Multiplier
    {
        get
        {
            if (ViewState["Multiplier"] == null)
                return string.Empty;
            else
                return ViewState["Multiplier"].ToString();
        }
        set
        {
            ViewState["Multiplier"] = value;
        }
    }
    private string TotalRS
    {
        get
        {
            if (ViewState["TotalRS"] == null)
                return string.Empty;
            else
                return ViewState["TotalRS"].ToString();
        }
        set
        {
            ViewState["TotalRS"] = value;
        }
    }

    #endregion

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxParameterList");
        this.ExporterIDs.Add("uxExportTop");
        base.PageInitialize();
        this.IsSecureCSRF = true;
        IsBindDataOnLoad = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        IsBindDataOnLoad = true;

        MerchantNumber = SecureQueryString[MERCHANT_NUMBER];
        ReportDate = DateTime.Parse(SecureQueryString[REPORT_DATE]);
        if (!IsPostBack)
        {
            PageType = SecurePageType.Modal;
            OnDataBindControls(DataBindAction.BindTitleInfo);
            OnDataBindControls(DataBindAction.BindMerchantClassfisication);
        }

        bool enabledRiskScoreModule = GeneralFuncsLib.CheckOnOffModule(WebSiteConstants.AttributeRiskScoreModuleName);
        //44810 - Remove permission to show/hide attributes risk score
        //43407 - MCPS - Clean up MS/ISO Attribute Records
        pnlAttributeScore.Visible = pnlAttributeRiskScoreGrid.Visible = enabledRiskScoreModule;

        //46652 - AW - Multi-Currency Transaction Display
        uxParameterList.Columns.FindByUniqueName("ParameterIndicatorFrom").HeaderText = uxParameterList.Columns.FindByUniqueName("ParameterIndicatorFrom").HeaderText.ToCurrencySymbol();
        uxParameterList.Columns.FindByUniqueName("ParameterIndicatorTo").HeaderText = uxParameterList.Columns.FindByUniqueName("ParameterIndicatorTo").HeaderText.ToCurrencySymbol();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    #region Grid events

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxParameterList)
        {
            OnDataBindControls(DataBindAction.BindParameterGrid, sender);
        }
    }
    int totalParameter = 0;
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (sender == uxParameterList)
        {
            decimal TmpDecimal = 0;
            decimal TmpDecimalHigh = 0;
            decimal TmpScore = 0;
            switch (e.Item.ItemType)
            {
                case GridItemType.Item:
                case GridItemType.AlternatingItem:
                    {
                        GridDataItem dataItem = (GridDataItem)e.Item;
                        DataRowView dataRow = e.Item.DataItem as DataRowView;
                        totalParameter = totalParameter + (dataRow["ParameterScore"].IsNotNullData() ? dataRow["ParameterScore"].ToInt() : 0);
                        string parameterThreshold = dataRow["ParameterThreshold"].ToString();
                        string strTempIndicatorFrom = string.Empty;
                        string strTempIndicatorTo = string.Empty;
                        string parameterIndicatorFrom = strTempIndicatorFrom = dataRow["ParameterIndicatorFrom"].ToString();
                        string parameterIndicatorTo = strTempIndicatorTo = dataRow["ParameterIndicatorTo"].ToString();
                        string parameterDataType = dataRow["ParameterDataType"].ToString().ToLower();
                        string parameterScore = dataRow["ParameterScore"].ToString();
                        int parameterPrecision = int.Parse(dataRow["ParameterPrecision"].ToString());
                        string indicatorFormat = "#,##0" + (parameterPrecision > 0 ? ".".PadRight(parameterPrecision + 1, '0') : string.Empty);

                        string parameterThresholdHigh = dataRow["ParameterThresholdHigh"].ToString();
                        string parameterThresholdType = dataRow["ParameterThresholdType"].ToString();
                        string thresholdType = dataRow["ThresholdType"].ToString().ToLower();


                        bool isIndicatorNagative = false;
                        bool isThresholdNagative = false;

                        dataItem["ParameterName"].ToolTip= dataRow["ParameterDescription"].ToString();
                        if (dataRow["ThresholdNegative"].ToString() == "True")
                        {
                            isThresholdNagative = true;
                        }
                        if (dataRow["IndicatorNegative"].ToString() == "True")
                        {
                            isIndicatorNagative = true;
                        }


                        if (isIndicatorNagative)
                        {
                            dataItem["ParameterIndicatorFrom"].ForeColor = Color.Red;
                            dataItem["ParameterIndicatorTo"].ForeColor = Color.Red;

                        }


                        if (!parameterIndicatorFrom.Equals("0") && !String.IsNullOrEmpty(parameterIndicatorFrom))
                        {
                            parameterIndicatorFrom = decimal.Parse(parameterIndicatorFrom).ToString(indicatorFormat);
                        }
                        if (!parameterIndicatorTo.Equals("0") && !String.IsNullOrEmpty(parameterIndicatorTo))
                        {
                            parameterIndicatorTo = decimal.Parse(parameterIndicatorTo).ToString(indicatorFormat);
                        }
                        if (!parameterThreshold.Equals("0") && !String.IsNullOrEmpty(parameterThreshold))
                        {
                            parameterThreshold = decimal.TryParse(parameterThreshold, out TmpDecimal) ? TmpDecimal.ToString("#,###") : string.Empty;
                        }
                        if (!parameterThresholdHigh.Equals("0") && !String.IsNullOrEmpty(parameterThresholdHigh))
                        {
                            parameterThresholdHigh = decimal.TryParse(parameterThresholdHigh, out TmpDecimalHigh) ? TmpDecimal.ToString("#,###") : string.Empty;
                        }
                        if (!parameterScore.Equals("0") && !String.IsNullOrEmpty(parameterScore))
                        {
                            if (decimal.TryParse(parameterScore, out TmpScore))
                            {
                                if (TmpScore < 0)
                                {
                                    dataItem["ParameterScore"].Text = "(" + TmpScore.ToString("#,###").Replace("-", "") + ")";
                                    dataItem["ParameterScore"].Style.Add("color", "Red");
                                }
                                else
                                {
                                    dataItem["ParameterScore"].Text = TmpScore.ToString("#,###");
                                }
                            }
                            else
                            {
                                dataItem["ParameterScore"].Text = string.Empty;
                            }
                        }
                        if (parameterDataType == SessionManager.CurrencySymbol)
                        {
                            if (Double.Parse(strTempIndicatorFrom) >= 0)
                            {
                                dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData(parameterDataType + (parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom));
                            }
                            else
                            {
                                dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterDataType + parameterIndicatorFrom.Replace("-", "") + ")");
                            }
                            if (Double.Parse(strTempIndicatorTo) >= 0)
                            {
                                dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData(parameterDataType + (parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo));
                            }
                            else
                            {
                                dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterDataType + parameterIndicatorTo.Replace("-", "") + ")");
                            }
                        }
                        switch (parameterDataType)
                        {
                            case "%":
                                if (Double.Parse(strTempIndicatorFrom) >= 0)
                                {
                                    dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData(parameterIndicatorFrom + parameterDataType);
                                }
                                else
                                {
                                    dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterIndicatorFrom.Replace("-", "") + parameterDataType + ")");
                                }
                                if (Double.Parse(strTempIndicatorTo) >= 0)
                                {
                                    dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData(parameterIndicatorTo + parameterDataType);
                                }
                                else
                                {
                                    dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterIndicatorTo.Replace("-", "") + parameterDataType + ")");
                                }

                                break;
                            //case "$":
                            //    if (Double.Parse(strTempIndicatorFrom) >= 0)
                            //    {
                            //        dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData(parameterDataType + (parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom));
                            //    }
                            //    else
                            //    {
                            //        dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterDataType + parameterIndicatorFrom.Replace("-", "") + ")");
                            //    }
                            //    if (Double.Parse(strTempIndicatorTo) >= 0)
                            //    {
                            //        dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData(parameterDataType + (parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo));
                            //    }
                            //    else
                            //    {
                            //        dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterDataType + parameterIndicatorTo.Replace("-", "") + ")");
                            //    }

                            //    break;
                            case "#":
                                if (Double.Parse(strTempIndicatorFrom) >= 0)
                                {
                                    dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData((parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom));
                                }
                                else
                                {
                                    dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterIndicatorFrom.Replace("-", "") + ")");
                                }
                                if (Double.Parse(strTempIndicatorTo) >= 0)
                                {
                                    dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData((parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo));
                                }
                                else
                                {
                                    dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterIndicatorTo.Replace("-", "") + ")");
                                }
                                break;
                            case "days":
                                if (Double.Parse(strTempIndicatorFrom) >= 0)
                                {
                                    dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData((parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom) + " " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_Days").ToString());
                                }
                                else
                                {
                                    dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterIndicatorFrom.Replace("-", "") + " " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_Days").ToString() + " )");
                                }
                                if (Double.Parse(parameterIndicatorTo) >= 0)
                                {
                                    dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData((parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo) + " " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_Days").ToString());
                                }
                                else
                                {
                                    dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterIndicatorTo.Replace("-", "") + " " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_Days").ToString() + " )");
                                }
                                break;
                            case "":
                                dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData(parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom);
                                dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData(parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo);
                                break;
                        }

                        //for case : indicator = 0 and isNegative

                        if (!strTempIndicatorFrom.IsNullOrEmpty())
                        {
                            if (Double.Parse(strTempIndicatorFrom) == 0 && isIndicatorNagative)
                            {
                                dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData("(" + dataItem["ParameterIndicatorFrom"].Text + ")");
                            }
                        }
                        else
                        {
                            dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_NA").ToString());
                        }

                        if (!parameterIndicatorTo.IsNullOrEmpty())
                        {
                            if (Double.Parse(parameterIndicatorTo) == 0 && isIndicatorNagative)
                            {
                                dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData("(" + dataItem["ParameterIndicatorTo"].Text + ")");
                            }
                        }
                        else
                        {
                            dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_NA").ToString());
                        }



                        //THRESHOLD

                        if (String.IsNullOrEmpty(parameterThreshold) || parameterThreshold == "0.00")
                        {
                            dataItem["ParameterThreshold"].Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_NA").ToString());
                        }
                        else
                        {
                            if (parameterThreshold.Length > 0)
                            {
                                //dataItem["ParameterThreshold"].Text = Int32.Parse(parameterThreshold) > 0 ? VeraCodeSolution.ValidateResponseData("$" + parameterThreshold) : VeraCodeSolution.ValidateResponseData("($" + parameterThreshold.Replace("-", string.Empty) + ")");
                                //if (Int32.Parse(parameterThreshold) < 0)
                                //{
                                //    dataItem["ParameterThreshold"].Style.Add("color", "Red");
                                //}
                                string thresholdValue = string.Empty;
                                string thresholdHighValue = string.Empty;

                                if (thresholdType == SessionManager.CurrencySymbol)
                                {
                                    thresholdValue = Double.Parse(parameterThreshold) >= 0 ? VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(parameterThreshold, SessionManager.CurrencyFortmat)) : VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(parameterThreshold, SessionManager.CurrencyFortmat).Replace("-", string.Empty) + ")");
                                    if (!String.IsNullOrEmpty(parameterThresholdHigh))
                                    {
                                        thresholdHighValue = Double.Parse(parameterThresholdHigh) >= 0 ? VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(parameterThresholdHigh)) : VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(parameterThresholdHigh).Replace("-", string.Empty) + ")");
                                    }
                                }
                                switch (thresholdType)
                                {
                                    case "%":
                                        thresholdValue = Double.Parse(parameterThreshold) >= 0 ? VeraCodeSolution.ValidateResponseData("" + parameterThreshold + "%") : VeraCodeSolution.ValidateResponseData("(" + parameterThreshold.Replace("-", string.Empty) + "%)");
                                        if (!String.IsNullOrEmpty(parameterThresholdHigh))
                                        {
                                            thresholdHighValue = Double.Parse(parameterThresholdHigh) >= 0 ? VeraCodeSolution.ValidateResponseData("" + parameterThresholdHigh + "%") : VeraCodeSolution.ValidateResponseData("(" + parameterThresholdHigh.Replace("-", string.Empty) + "%)");
                                        }
                                        break;
                                    //case "$":
                                    //    thresholdValue = Double.Parse(parameterThreshold) >= 0 ? VeraCodeSolution.ValidateResponseData("$" + parameterThreshold) : VeraCodeSolution.ValidateResponseData("($" + parameterThreshold.Replace("-", string.Empty) + ")");
                                    //    if (!String.IsNullOrEmpty(parameterThresholdHigh))
                                    //    {
                                    //        thresholdHighValue = Double.Parse(parameterThresholdHigh) >= 0 ? VeraCodeSolution.ValidateResponseData("$" + parameterThresholdHigh) : VeraCodeSolution.ValidateResponseData("($" + parameterThresholdHigh.Replace("-", string.Empty) + ")");
                                    //    }
                                    //    break;
                                    case "#":
                                        thresholdValue = Double.Parse(parameterThreshold) >= 0 ? VeraCodeSolution.ValidateResponseData("" + parameterThreshold) : VeraCodeSolution.ValidateResponseData("(" + parameterThreshold.Replace("-", string.Empty) + ")");
                                        if (!String.IsNullOrEmpty(parameterThresholdHigh))
                                        {
                                            thresholdHighValue = Double.Parse(parameterThresholdHigh) >= 0 ? VeraCodeSolution.ValidateResponseData("" + parameterThresholdHigh) : VeraCodeSolution.ValidateResponseData("(" + parameterThresholdHigh.Replace("-", string.Empty) + ")");
                                        }
                                        break;
                                    case "days":
                                        thresholdValue = Double.Parse(parameterThreshold) >= 0 ? VeraCodeSolution.ValidateResponseData("" + parameterThreshold + " " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_Days").ToString()) : VeraCodeSolution.ValidateResponseData("(" + parameterThreshold.Replace("-", string.Empty) + " " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_Days").ToString() + ")");
                                        if (!String.IsNullOrEmpty(parameterThresholdHigh))
                                        {
                                            thresholdHighValue = Double.Parse(parameterThresholdHigh) >= 0 ? VeraCodeSolution.ValidateResponseData("" + parameterThresholdHigh + " " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_Days").ToString()) : VeraCodeSolution.ValidateResponseData("(" + parameterThresholdHigh.Replace("-", string.Empty) + " " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_Days").ToString() + ")");
                                        }
                                        break;
                                }

                                //for case : indicator = 0 and isNegative
                                if (Double.Parse(parameterThreshold) == 0 && isThresholdNagative)
                                {
                                    thresholdValue = VeraCodeSolution.ValidateResponseData("(" + thresholdValue + ")");
                                }
                                if (parameterThresholdType == "LowHigh")
                                {
                                    dataItem["ParameterThreshold"].Text = thresholdValue + " - " + thresholdHighValue;
                                }
                                else
                                {
                                    dataItem["ParameterThreshold"].Text = thresholdValue;
                                }
                                //if (Double.Parse(parameterThreshold) < 0)
                                //{
                                //    dataItem["ParameterThreshold"].Style.Add("color", "Red");
                                //}
                                if (isThresholdNagative)
                                {
                                    dataItem["ParameterThreshold"].Style.Add("color", "Red");
                                }
                            }
                            else
                            {
                                dataItem["ParameterThreshold"].Text = string.Empty;
                            }
                        }

                        if ((String.IsNullOrEmpty(parameterThreshold) || parameterThreshold == "0"))
                        {
                            dataItem["ParameterThreshold"].Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_NA").ToString());
                        }
                    }
                    break;
            }
        }

    }

    #endregion


    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (this.IsIntruderDetected) return;
        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindParameterGrid:
                {
                    ASGrid grid = (ASGrid)sender;

                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                    parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
                    parameters.AddLanguageID();
                    DataTable td = WebServices.RiskServices.GetReports("spa_rm_mcf_GetRiskScoreDetails", parameters);
                    grid.DataSource = _isExporting ? DataExport() : td;
                    grid.ShowFooter = td.HasData();

                }
                break;
            case DataBindAction.BindTitleInfo:
                {
                    merchantName = GeneralFuncsLib.GetMerchantName(MerchantNumber);
                    ltrMerchantInfor.Text = GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_MerchantNumber").ToString() + " " + VeraCodeSolution.ValidateResponseData(MerchantNumber + " - " + merchantName);
                    uxReportDate.Text = GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_ReportDate").ToString() + " " + ReportDate.ToString("MM/dd/yyyy");
                }
                break;
            case DataBindAction.BindMerchantClassfisication:
                {
                    parameters.AddLoggedInUserParamsWithRecId();
                    parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                    parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
                    _RiskScoreInfor = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_MIFClassificationByMerchant", parameters);
                    MerchantClassificationName = BindValue("MerchantClassificationName");
                    Multiplier = BindValue("Multiplier");
                    TotalRS = BindValue("TotalRS");
                }
                break;
            case DataBindAction.BindAttributeGrid:
                {
                    // Fixed #33991
                    parameters.AddLanguageID();
                    parameters.AddLoggedInUserParamsWithRecId();
                    parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                    parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
                    DataTable td = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_RiskScoreAttributeDetails", parameters);
                    uxAttributeGrid.DataSource = td;
                    uxAttributeGrid.ShowFooter = td.HasData();
                }
                break;
        }
    }



    # region Export
    bool _isExporting = false;
    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        if (sender == uxExportTop)
        {
            _isExporting = true;
            UxExport Export = sender as UxExport;
            merchantName = GeneralFuncsLib.GetMerchantName(MerchantNumber);
            string header = System.Environment.NewLine;

            if (Export.ExportButtonType != UxExport.ExportType.Excel)
            {
                header = GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_MakeUpTheRiskScore").ToString() + " " + System.Environment.NewLine + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_MerchantNumber").ToString() + " "
                + VeraCodeSolution.ValidateResponseData(MerchantNumber + " - " + merchantName + " ") + System.Environment.NewLine + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_ReportDate").ToString() + " " + ReportDate.ToString("MM/dd/yyyy");
            }
            else
            {
                header = GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_MakeUpTheRiskScore").ToString() + " " + "\r\n" + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_MerchantNumber").ToString() + " "
                + VeraCodeSolution.ValidateResponseData(MerchantNumber + " - " + merchantName) + "\r\n " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_ReportDate").ToString() + " " + ReportDate.ToString("MM/dd/yyyy");
            }
            base.DoNeedExportConfig(sender, exportConfig);
            exportConfig.ReportHeader = header;
            exportConfig.FileName = GeneralFuncsLib.GetLegalFileName(GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_RiskScoreDetail").ToString());
        }
    }

    private DataTable DataExport()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
        parameters.AddLanguageID();
        DataTable dt = WebServices.RiskServices.GetReports("spa_rm_mcf_GetRiskScoreDetails", parameters);
        int footerTotal = 0;
        DataTable tb1 = dt.Clone();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            decimal TmpDecimal = 0;
            string parameterIndicatorFrom = dt.Rows[i]["ParameterIndicatorFrom"].ToString();
            string parameterIndicatorTo = dt.Rows[i]["ParameterIndicatorTo"].ToString();
            string parameterDataType = dt.Rows[i]["ParameterDataType"].ToString().ToLower();
            int parameterPrecision = int.Parse(dt.Rows[i]["ParameterPrecision"].ToString());
            string indicatorFormat = "#,##0" + (parameterPrecision > 0 ? ".".PadRight(parameterPrecision + 1, '0') : string.Empty);
            string parameterThreshold = dt.Rows[i]["ParameterThreshold"].ToString();
            string parameterScore = dt.Rows[i]["ParameterScore"].ToString();
            string thresholdType = dt.Rows[i]["ThresholdType"].ToString().ToLower();
            footerTotal += (!string.IsNullOrEmpty(parameterScore) ? parameterScore.ToInt() : 0);

            if (!parameterIndicatorFrom.Equals("0") && !String.IsNullOrEmpty(parameterIndicatorFrom))
            {
                parameterIndicatorFrom = decimal.Parse(parameterIndicatorFrom).ToString(indicatorFormat);
            }
            if (!parameterIndicatorTo.Equals("0") && !String.IsNullOrEmpty(parameterIndicatorTo))
            {
                parameterIndicatorTo = decimal.Parse(parameterIndicatorTo).ToString(indicatorFormat);
            }
            if (!parameterThreshold.Equals("0") && !String.IsNullOrEmpty(parameterThreshold))
            {
                parameterThreshold = decimal.TryParse(parameterThreshold, out TmpDecimal) ? TmpDecimal.ToString("#,###") : string.Empty;
            }

            if (parameterDataType == SessionManager.CurrencySymbol)
            {
                dt.Rows[i]["ParameterIndicatorTo"] = parameterDataType + (parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo);
                dt.Rows[i]["ParameterIndicatorFrom"] = parameterDataType + (parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom);
            }
            switch (parameterDataType)
            {
                case "%":
                    dt.Rows[i]["ParameterIndicatorTo"] = parameterIndicatorTo + parameterDataType;
                    dt.Rows[i]["ParameterIndicatorFrom"] = parameterIndicatorFrom + parameterDataType;
                    break;
                //case "$":
                //    dt.Rows[i]["ParameterIndicatorTo"] = parameterDataType + (parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo);
                //    dt.Rows[i]["ParameterIndicatorFrom"] = parameterDataType + (parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom);
                //    break;
                case "#":
                    dt.Rows[i]["ParameterIndicatorTo"] = (parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo);
                    dt.Rows[i]["ParameterIndicatorFrom"] = (parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom);
                    break;
                case "days":
                    dt.Rows[i]["ParameterIndicatorTo"] = (parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo) + " " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_Days").ToString();
                    dt.Rows[i]["ParameterIndicatorFrom"] = (parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom) + " " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_Days").ToString();
                    break;
            }
            if (String.IsNullOrEmpty(parameterThreshold) || parameterThreshold.Equals("0"))
            {
                dt.Rows[i]["ParameterThreshold"] = GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_NA").ToString();
            }
            else
            {
                if (thresholdType == SessionManager.CurrencySymbol)
                {
                    dt.Rows[i]["ParameterThreshold"] = (parameterThreshold.Length > 0 ?
                       FormatData.FormatCurrency(parameterThreshold, SessionManager.CurrencyFortmat) : string.Empty);
                }
                switch (thresholdType)
                {
                    case "%":
                        dt.Rows[i]["ParameterThreshold"] = (parameterThreshold.Length > 0 ?
                  "" + parameterThreshold + "%" : string.Empty);
                        break;
                    // case "$":
                    //     dt.Rows[i]["ParameterThreshold"] = (parameterThreshold.Length > 0 ?
                    //"$" + parameterThreshold : string.Empty);
                    //     break;
                    case "#":
                        dt.Rows[i]["ParameterThreshold"] = (parameterThreshold.Length > 0 ?
                   "" + parameterThreshold + "" : string.Empty);
                        break;
                    case "days":
                        dt.Rows[i]["ParameterThreshold"] = (parameterThreshold.Length > 0 ?
                  "" + parameterThreshold + " " + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_Days").ToString() : string.Empty);
                        break;
                }
            }

            DataRow row = tb1.NewRow();
            row["ParameterGroupDescription"] = dt.Rows[i]["ParameterGroupDescription"];
            row["ParameterDescription"] = dt.Rows[i]["ParameterDescription"];
            row["ParameterName"] = dt.Rows[i]["ParameterName"];
            row["ParameterIndicatorFrom"] = parameterIndicatorFrom.IsNullOrEmpty() ? GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_NA").ToString()
                : dt.Rows[i]["ParameterIndicatorFrom"];
            row["ParameterIndicatorTo"] = parameterIndicatorTo.IsNullOrEmpty() ? GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_NA").ToString()
                : dt.Rows[i]["ParameterIndicatorTo"];

            row["ParameterThreshold"] = dt.Rows[i]["ParameterThreshold"].ToString();
            row["ParameterScore"] = dt.Rows[i]["ParameterScore"];
            tb1.Rows.Add(row);
        }
        return tb1;
    }

    #endregion
    private DataTable _RiskScoreInfor;

    public string BindValue(string colName)
    {
        if (_RiskScoreInfor == null || _RiskScoreInfor.Rows.Count <= 0)
            return WebSiteConstants.HTML_EM_DASH_ENCODE;
        DataRow dr = _RiskScoreInfor.Rows[0];
        return dr[colName] == DBNull.Value || dr[colName].ToString().IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : dr[colName].ToString();

    }
    public string BindValueDecimal(string colName)
    {
        if (_RiskScoreInfor == null || _RiskScoreInfor.Rows.Count <= 0)
            return string.Empty;
        DataRow dr = _RiskScoreInfor.Rows[0];
        if (dr[colName] == DBNull.Value || dr[colName].ToString().IsNullOrEmpty())
        {
            return WebSiteConstants.HTML_EM_DASH_ENCODE;
        }
        else
        {
            decimal v = 0;
            decimal.TryParse(dr[colName].ToString(), out v);
            return v.ToString("#,###");
        }

    }

    protected void uxAttributeGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindAttributeGrid, sender);
    }
    protected void uxExportAttributeGrid_NeedExportConfig(object sender, ExportConfig exportConfig)
    {
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExportAttributeGrid.GridHeader);
        UxExport Export = sender as UxExport;
        string header = System.Environment.NewLine;
        if (Export.ExportButtonType != UxExport.ExportType.Excel)
        {
            header = System.Environment.NewLine + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_MerchantNumber").ToString() + " "
            + VeraCodeSolution.ValidateResponseData(MerchantNumber)
            + System.Environment.NewLine + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_ReportDate").ToString()
            + " " + ReportDate.ToString("MM/dd/yyyy")
            + System.Environment.NewLine
            + GetLocalResourceObject("ltrClassificationResource.Text").ToString()
            + " " + MerchantClassificationName
            + System.Environment.NewLine
            + GetLocalResourceObject("ltrMultiplierResource.Text").ToString()
            + " " + Multiplier
            + System.Environment.NewLine
            + GetLocalResourceObject("ltrTotalRiskScoreResource.Text").ToString()
            + " " + TotalRS
            + System.Environment.NewLine;
        }
        else
        {
            header = "\r\n" + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_MerchantNumber").ToString() + " "
            + VeraCodeSolution.ValidateResponseData(MerchantNumber)
            + "\r\n" + GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_ReportDate").ToString()
            + " " + ReportDate.ToString("MM/dd/yyyy")
            + "\r\n"
            + GetLocalResourceObject("ltrClassificationResource.Text").ToString()
            + " " + MerchantClassificationName
            + "\r\n"
            + GetLocalResourceObject("ltrMultiplierResource.Text").ToString()
            + " " + Multiplier
            + "\r\n"
            + GetLocalResourceObject("ltrTotalRiskScoreResource.Text").ToString()
            + " " + TotalRS
            + "\r\n";
        }

        uxExportAttributeGrid.Formatter = new Dictionary<string, Func<object, string>>();
        uxExportAttributeGrid.Formatter.Add("FromValue", new Func<object, string>(MyConvert));
        uxExportAttributeGrid.Formatter.Add("ToValue", new Func<object, string>(MyConvert));
        exportConfig.ReportHeader = exportConfig.ReportHeader + header;
        //exportConfig.FileName = GeneralFuncsLib.GetLegalFileName(GetLocalResourceObject("rm_RiskScoreDetailModal_aspx_cs_RiskScoreDetail").ToString());


    }
    protected string MyConvert(object obj)
    {
        if (obj == null || obj.ToString() == String.Empty)
            return String.Empty;
        else
            return Convert.ToInt32(Math.Round(Convert.ToDouble(obj.ToString()))).ToString();
    }

}
