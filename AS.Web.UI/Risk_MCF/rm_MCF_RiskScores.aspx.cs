using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using BuGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;

[PagePermission("RskScore,MSRskScore")]
public partial class rm_MCF_RiskScores : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindRiskScore,
        BindAttributeRiskScore
    }

    enum PostBackAction
    {
        CreateRiskScore,
        EditRiskScore,
        DeleteRiskScore
    }

    #endregion

    #region Properties
    bool _isExporting = false;
    #endregion

    #region Overrides
    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxRadGrid");
        this.ExporterIDs.Add("uxExportTop");
        //this.ExporterIDs.Add("uxExportBottom");
        base.PageInitialize();
        this.IsSecureCSRF = true;
        IsBindDataOnLoad = true;
    }

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        if (IsIntruderDetected) return;
        UxExport uxExport = sender as UxExport;
        ASGrid grid = uxExport.Grid as ASGrid;
        _isExporting = true;
        grid.Columns.FindByUniqueName("ParameterIndicatorToText").Visible = true;
        grid.Columns.FindByUniqueName("ParameterIndicatorTo").Visible = false;
        grid.Columns.FindByUniqueName("ParameterIndicatorFromText").Visible = true;
        grid.Columns.FindByUniqueName("ParameterIndicatorFrom").Visible = false;
        grid.Columns.FindByUniqueName("ParameterThresholdText").Visible = true;
        grid.Columns.FindByUniqueName("ParameterThreshold").Visible = false;
        //Fortmat data
        uxExport.Formatter = new System.Collections.Generic.Dictionary<string, Func<object, string>>();
        //Format ThresHold
        uxExport.Formatter.Add("ParameterThreshold", obj => obj.ToString() == "0" ?
            string.Empty : AS.Common.Formater.FormatData.FormatCurrency(obj.ToString(), SessionManager.CurrencyFortmat));

        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExportTop.GridHeader);
        exportConfig.ReportHeader = VeraCodeSolution.DoVeraCode(uxExportTop.GridHeader);

    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindRiskScore, sender);

    }
    protected override void OnPreRenderComplete(EventArgs e)
    {
        base.OnPreRenderComplete(e);
    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
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
                    string strTempIndicatorFrom = string.Empty;
                    string strTempIndicatorTo = string.Empty;
                    string parameterIndicatorFrom = strTempIndicatorFrom = dataRow["ParameterIndicatorFrom"].ToString();
                    string parameterIndicatorTo = strTempIndicatorTo = dataRow["ParameterIndicatorTo"].ToString();
                    string parameterDataType = dataRow["ParameterDataType"].ToString().ToLower();
                    int parameterPrecision = int.Parse(dataRow["ParameterPrecision"].ToString());
                    string indicatorFormat = "#,##0" + (parameterPrecision > 0 ? ".".PadRight(parameterPrecision + 1, '0') : string.Empty);
                    string parameterThreshold = dataRow["ParameterThreshold"].ToString();
                    string parameterThresholdHigh = dataRow["ParameterThresholdHigh"].ToString();
                    string parameterScore = dataRow["ParameterScore"].ToString();
                    //Display From/To is N/A if ParameterIndicator is NULL
                    string parameterIndicator = dataRow["ParameterIndicator"].ToString();
                    string thresholdType = dataRow["ThresholdType"].ToString().ToLower();



                    bool isIndicatorNagative = false;
                    bool isThresholdNagative = false;

                    dataItem["ParameterDisplayName"].ToolTip = dataRow["ParameterDescription"].ToString();

                    if (dataRow["IsThresholdNegative"].ToString() == "True")
                    {
                        isThresholdNagative = true;
                    }
                    if (dataRow["IsIndicatorNegative"].ToString() == "True")
                    {
                        isIndicatorNagative = true;
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
                    if (!parameterThresholdHigh.Equals("0") && !String.IsNullOrEmpty(parameterThreshold))
                    {
                        parameterThresholdHigh = decimal.TryParse(parameterThresholdHigh, out TmpDecimalHigh) ? TmpDecimalHigh.ToString("#,###") : string.Empty;
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
                                dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData((parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom) + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString());
                            }
                            else
                            {
                                dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterIndicatorFrom.Replace("-", "") + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString() + " )");
                            }
                            if (Double.Parse(parameterIndicatorTo) >= 0)
                            {
                                dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData((parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo) + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString());
                            }
                            else
                            {
                                dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData("(" + parameterIndicatorTo.Replace("-", "") + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString() + " )");
                            }

                            break;
                        case "":
                            dataItem["ParameterIndicatorFrom"].Text = VeraCodeSolution.ValidateResponseData(parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom);
                            dataItem["ParameterIndicatorTo"].Text = VeraCodeSolution.ValidateResponseData(parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo);

                            break;


                    }

                    if (String.IsNullOrEmpty(parameterThreshold) || parameterThreshold == "0")
                    {
                        dataItem["ParameterThreshold"].Text = string.Empty;
                    }
                    else
                    {
                        if (parameterThreshold.Length > 0)
                        {
                            string thresholdValue = string.Empty;
                            string thresholdHighValue = string.Empty;

                            if (thresholdType == SessionManager.CurrencySymbol)
                            {
                                thresholdValue = Double.Parse(parameterThreshold) >= 0 ? VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(parameterThreshold, SessionManager.CurrencyFortmat)) : VeraCodeSolution.ValidateResponseData(("(" + FormatData.FormatCurrency(parameterThreshold, SessionManager.CurrencyFortmat).Replace("-", string.Empty) + ")"));
                                if (!String.IsNullOrEmpty(parameterThresholdHigh))
                                {
                                    thresholdHighValue = Double.Parse(parameterThresholdHigh) >= 0 ? VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(parameterThresholdHigh, SessionManager.CurrencyFortmat)) : VeraCodeSolution.ValidateResponseData(("(" + FormatData.FormatCurrency(parameterThresholdHigh, SessionManager.CurrencyFortmat).Replace("-", string.Empty) + ")"));
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
                                    thresholdValue = Double.Parse(parameterThreshold) >= 0 ? VeraCodeSolution.ValidateResponseData("" + parameterThreshold + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString()) : VeraCodeSolution.ValidateResponseData("(" + parameterThreshold.Replace("-", string.Empty) + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString() + ")");
                                    if (!String.IsNullOrEmpty(parameterThresholdHigh))
                                    {
                                        thresholdHighValue = Double.Parse(parameterThresholdHigh) >= 0 ? VeraCodeSolution.ValidateResponseData("" + parameterThresholdHigh + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString()) : VeraCodeSolution.ValidateResponseData("(" + parameterThresholdHigh.Replace("-", string.Empty) + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString() + ")");
                                    }
                                    break;
                            }
                            //for case : indicator = 0 and isNegative
                            if (Double.Parse(dataRow["ParameterThreshold"].ToString()) == 0 && isThresholdNagative)
                            {
                                thresholdValue = VeraCodeSolution.ValidateResponseData("(" + thresholdValue + ")");
                                thresholdHighValue = VeraCodeSolution.ValidateResponseData("(" + thresholdHighValue + ")");
                            }
                            if (!String.IsNullOrEmpty(thresholdHighValue))
                            {
                                dataItem["ParameterThreshold"].Text = thresholdValue + " - " + thresholdHighValue;
                            }
                            else
                            {
                                dataItem["ParameterThreshold"].Text = thresholdValue;
                            }
                            //if (Double.Parse(dataRow["ParameterThreshold"].ToString()) < 0)
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
                    if (string.IsNullOrEmpty(parameterIndicator))
                    {
                        dataItem["ParameterIndicatorFrom"].Text = string.Empty;
                        dataItem["ParameterIndicatorTo"].Text = string.Empty;
                    }

                }
                break;

            case GridItemType.EditFormItem:
                {
                    if (e.Item.IsInEditMode)
                    {
                        GridEditFormItem dataItem = (GridEditFormItem)e.Item;
                        DataRowView dataRow = e.Item.DataItem as DataRowView;

                        int parameterPrecisionInt = 0;
                        if ((dataRow["ParameterPrecision"] != DBNull.Value && !string.IsNullOrEmpty(dataRow["ParameterPrecision"].ToString())))
                            parameterPrecisionInt = int.Parse(dataRow["ParameterPrecision"].ToString());
                        string parameterIndicatorFrom = (dataRow["ParameterIndicatorFrom"] != DBNull.Value && !string.IsNullOrEmpty(dataRow["ParameterIndicatorFrom"].ToString())) ? Math.Round(double.Parse(dataRow["ParameterIndicatorFrom"].ToString()), parameterPrecisionInt).ToString() : "0";
                        string parameterIndicatorTo = (dataRow["ParameterIndicatorTo"] != DBNull.Value && !string.IsNullOrEmpty(dataRow["ParameterIndicatorTo"].ToString())) ? Math.Round(double.Parse(dataRow["ParameterIndicatorTo"].ToString()), parameterPrecisionInt).ToString() : "0";
                        string parameterThreshold = dataRow["ParameterThreshold"].ToString();
                        string parameterThresholdHigh = dataRow["ParameterThresholdHigh"].ToString();
                        string parameterScore = dataRow["ParameterScore"].ToString();
                        string parameterPrecision = dataRow["ParameterPrecision"].ToString();
                        bool parameterIndicatorNegative = dataRow["IsIndicatorNegative"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase);

                        UserControls_rm_MCF_EditRiskScore uxEditRiskScore = (UserControls_rm_MCF_EditRiskScore)e.Item.FindControl("uxEditRiskScore");

                        uxEditRiskScore.ParameterIndicatorFrom = VeraCodeSolution.DoVeraCode(Regex.Replace(parameterIndicatorFrom, "[-%$#days,()]", string.Empty).TrimEnd());
                        uxEditRiskScore.ParameterIndicatorTo = VeraCodeSolution.DoVeraCode(Regex.Replace(parameterIndicatorTo, "[-%$#days,()]", string.Empty).TrimEnd());
                        uxEditRiskScore.ParameterPrecision = parameterPrecision;
                        uxEditRiskScore.ParameterIndicatorNegative = parameterIndicatorNegative;

                        var decimalAndPrecisionInConfig = GetDecimalAndPrecisionInConfig(dataRow["ParameterKey"].ToString(), parameterPrecisionInt);
                        uxEditRiskScore.IsAllowDecimalValue = decimalAndPrecisionInConfig.Item1;
                        uxEditRiskScore.PrecisionValue = decimalAndPrecisionInConfig.Item2;

                        if (uxEditRiskScore.ParameterThresholdTypeHigh == "1" && !parameterThreshold.IsNullOrEmpty())
                        {
                            parameterThreshold = Regex.Replace(parameterThreshold,
                                "[-%$#days,()]", string.Empty).TrimEnd();

                            //uxEditRiskScore.ParameterThreshold = VeraCodeSolution.DoVeraCode(
                            //    parameterThreshold.Replace(".00", string.Empty));

                            uxEditRiskScore.ParameterThreshold = VeraCodeSolution.DoVeraCode(
                               ReplacePrecision(parameterThreshold));

                            parameterThresholdHigh = Regex.Replace(parameterThresholdHigh,
                                "[-%$#days,()]", string.Empty).TrimEnd();
                            uxEditRiskScore.ParameterThresholdHigh = VeraCodeSolution.DoVeraCode(
                                parameterThresholdHigh.Replace(".00", string.Empty));
                        }
                        else
                        {
                            parameterThreshold = Regex.Replace(parameterThreshold,
                                "[-%$#days,()]", string.Empty).TrimEnd();
                            //uxEditRiskScore.ParameterThreshold = VeraCodeSolution.DoVeraCode(
                            //    parameterThreshold.Replace(".00", string.Empty));

                            uxEditRiskScore.ParameterThreshold = VeraCodeSolution.DoVeraCode(
                              ReplacePrecision(parameterThreshold));

                            if (uxEditRiskScore.ParameterThresholdTypeHigh == "1"
                                && parameterThreshold.IsNullOrEmpty())
                            {
                                uxEditRiskScore.ParameterThresholdHigh = VeraCodeSolution.DoVeraCode(
                                    Regex.Replace(parameterThreshold, "[-%$#days,()]", string.Empty).TrimEnd());
                            }
                        }

                        uxEditRiskScore.ParameterScore = VeraCodeSolution.DoVeraCode(Regex.Replace(parameterScore, "[-%$#days,()]", string.Empty).TrimEnd());
                    }
                    break;
                }
        }
    }

    //44432:  Bug #37335
    string ReplacePrecision(string input)
    {
        if (input.Contains("."))
        {
            input = input.Substring(0, input.IndexOf('.'));
        }

        return input == "0" ? string.Empty : input;

    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            //case PostBackAction.CreateRiskScore:
            //    {
            //        uxRiskScore.Visible = true;
            //        uxRiskScore.ResetForm();
            //        uxRadGrid.MasterTableView.ClearEditItems();
            //    }
            //    break;
            case PostBackAction.DeleteRiskScore:
                {
                    GridCommandEventArgs e = sender as GridCommandEventArgs;
                    GridDataItem dataItem = (GridDataItem)e.Item;

                    FilterParameterCollection parameters = new FilterParameterCollection();
                    //if (_SiteID == -1)
                    //    parameters.AddLoggedInUserRiskParams();
                    //else
                    //{
                    //    parameters.AddLoggedInUserParams(-1);
                    //    parameters.Add(new FilterParameter("@SiteID", _SiteID, DbType.Int32));
                    //}
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@RecordID", int.Parse(dataItem.GetDataKeyValue("RecordID").ToString()), DbType.Int32));
                    FilterParameterCollection parameterOut = new FilterParameterCollection();
                    WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_DeleteRiskScore", parameters, out parameterOut);

                    uxRadGrid.MasterTableView.ClearEditItems();
                    //uxRiskScore.Visible = false;
                    uxRadGrid.Rebind();
                }
                break;
            case PostBackAction.EditRiskScore:
                {
                    Telerik.Web.UI.GridCommandEventArgs e = sender as Telerik.Web.UI.GridCommandEventArgs;

                    if (e.CommandName == RadGrid.EditCommandName || e.CommandName == RadGrid.CancelCommandName)
                    {
                        //uxRiskScore.Visible = false;
                    }
                    else
                    {
                        if (e.CommandName == RadGrid.UpdateCommandName)
                        {
                            GridEditableItem editedItem = e.Item as GridEditableItem;
                            UserControls_rm_MCF_EditRiskScore editRiskScore = (UserControls_rm_MCF_EditRiskScore)editedItem.FindControl("uxEditRiskScore");

                            string isIndicatorNegative = VeraCodeSolution.DoVeraCode((editRiskScore.FindControl("uxParameterIsIndicatorNegative") as HiddenField).Value.Trim());
                            string isThresholdNegative = VeraCodeSolution.DoVeraCode((editRiskScore.FindControl("uxParameterIsThresholdNegative") as HiddenField).Value.Trim());
                            string isNullParameterIndicator = VeraCodeSolution.DoVeraCode((editRiskScore.FindControl("uxParameterIsNullIndicator") as HiddenField).Value.Trim());

                            string parameterIndicatorFrom = VeraCodeSolution.DoVeraCode((editRiskScore.FindControl("uxParameterIndicatorFrom") as TextBox).Text.Trim());
                            string parameterIndicatorTo = VeraCodeSolution.DoVeraCode((editRiskScore.FindControl("uxParameterIndicatorTo") as TextBox).Text.Trim());
                            string parameterThreshold = VeraCodeSolution.DoVeraCode((editRiskScore.FindControl("uxParameterThreshold") as TextBox).Text.Trim());

                            string parameterScore = VeraCodeSolution.DoVeraCode((editRiskScore.FindControl("uxParameterScore") as TextBox).Text.Trim());

                            PlaceHolder plcThresholdHigh = editRiskScore.FindControl("uxPlcParamThresholdHigh") as PlaceHolder;
                            int indicatorThresholdHigh = 0;
                            if (plcThresholdHigh.Visible)
                            {
                                string parameterThresholdHigh = VeraCodeSolution.DoVeraCode((editRiskScore.FindControl("uxParameterThresholdHigh") as TextBox).Text.Trim());
                                int.TryParse(parameterThresholdHigh, out indicatorThresholdHigh);
                            }


                            if (!DoValidateInput(parameterIndicatorFrom, parameterIndicatorTo, parameterThreshold, parameterScore)) return;

                            int recordID = 0;
                            decimal indicatorFrom = 0;
                            decimal indicatorTo = 0;
                            int indicatorThreshold = 0;
                            int indicatorScore = 0;
                            int indicatorNegative = isIndicatorNegative == "1" ? -1 : 1;
                            int thresholdNegative = isThresholdNegative == "1" ? -1 : 1;

                            Int32.TryParse((editRiskScore.FindControl("uxRecordID") as HiddenField).Value.Trim(), out recordID);
                            Decimal.TryParse(parameterIndicatorFrom, out indicatorFrom);
                            Decimal.TryParse(parameterIndicatorTo, out indicatorTo);

                            int.TryParse(parameterThreshold, out indicatorThreshold);
                            Int32.TryParse(parameterScore, out indicatorScore);
                            indicatorFrom = Math.Abs(indicatorFrom);
                            indicatorTo = Math.Abs(indicatorTo);
                            indicatorThreshold = Math.Abs(indicatorThreshold);
                            indicatorScore = Math.Abs(indicatorScore);

                            FilterParameterCollection parameters = new FilterParameterCollection();
                            //if (_SiteID == -1)
                            //    parameters.AddLoggedInUserRiskParams();
                            //else
                            //{
                            //    parameters.AddLoggedInUserParams(-1);
                            //    parameters.Add(new FilterParameter("@SiteID", _SiteID, DbType.Int32));
                            //}
                            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                            parameters.Add(new FilterParameter("@RecordID", recordID, DbType.Int32));
                            parameters.Add(new FilterParameter("@ParameterIndicatorFrom", indicatorFrom * indicatorNegative, DbType.Decimal));
                            parameters.Add(new FilterParameter("@ParameterIndicatorTo", indicatorTo * indicatorNegative, DbType.Decimal));
                            parameters.Add(new FilterParameter("@ParameterThreshold", indicatorThreshold == 0 ? 0 : indicatorThreshold * thresholdNegative, DbType.Int32));
                            parameters.Add(new FilterParameter("@ParameterScore", indicatorScore, DbType.Int32));
                            if (plcThresholdHigh.Visible)
                            {
                                indicatorThresholdHigh = indicatorThreshold == 0 ? 0 : indicatorThresholdHigh;
                                parameters.Add(new FilterParameter("@ParameterThresholdHigh", indicatorThresholdHigh == 0 ? 0 : indicatorThresholdHigh * thresholdNegative, DbType.Int32));
                            }
                            else
                            {
                                parameters.Add(new FilterParameter("@ParameterThresholdHigh", null, DbType.Int32));
                            }
                            FilterParameterCollection parameterOut = new FilterParameterCollection();
                            WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_UpdateRiskScore", parameters, out parameterOut);

                        }
                    }
                }
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindRiskScore:
                {
                    if (sender == uxRadGrid && uxRadGrid.Visible)
                    {
                        FilterParameterCollection parameters = new FilterParameterCollection();
                        //if (_SiteID == -1)
                        //    parameters.AddLoggedInUserRiskParams();
                        //else
                        //{
                        //    parameters.AddLoggedInUserParams(-1);
                        //    parameters.Add(new FilterParameter("@SiteID", _SiteID, DbType.Int32));
                        //}
                        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        parameters.AddLanguageID();
                        DataTable table = WebServices.RiskServices.GetReports("spa_rm_cs_GetRiskScores", parameters);
                        uxRadGrid.DataSource = _isExporting ? InitializeExport(table) : table;

                        if (table.Rows.Count > 0)
                            uxRadGrid.AllowSorting = true;
                        else
                            uxRadGrid.AllowSorting = false;
                    }
                }
                break;


            case DataBindAction.BindAttributeRiskScore:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.AddLanguageID();
                    DataTable table = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_AttributeRiskScoreList", parameters);
                    uxAttributeGrid.DataSource = table;

                    if (table.Rows.Count > 0)
                        uxAttributeGrid.AllowSorting = true;
                    else
                        uxAttributeGrid.AllowSorting = false;
                }
                break;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        uxRadGrid.SortCommand += uxRadGrid_SortCommand;
        //_SiteID = uxRiskScore.RiskSite = uxSiteIDSelector.checkRiskSiteForRiskScore();
        //uxHdRiskSite.Value = _SiteID.ToString();
        //if (_SiteID != -1)
        //{
        //    uxRiskSite.Visible = false;
        //    uxCreateMode.Visible = true;
        //    uxDivPannel.Visible = true;
        //}
        //else if (SessionManager.CurrentRiskSiteID == 0)
        //{
        //    uxCreateMode.Visible = false;
        //    uxDivPannel.Visible = false;
        //}
        //else
        //{
        //    uxCreateMode.Visible = true;
        //    uxDivPannel.Visible = true;
        //}
        // 44810 - VW - Attribute Risk Score Section - Modify Permissions to View-FE - 2019/02/11

        if (GeneralFuncsLib.CheckOnOffModule(WebSiteConstants.AttributeRiskScoreModuleName))
        {
            uxpnlAttributeRiskScore.Visible = true;
        }
        else
            uxpnlAttributeRiskScore.Visible = false;
        //46652 - AW - Multi-Currency Transaction Display
        uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorFrom").HeaderText =
            uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorFrom").HeaderText.ToCurrencySymbol();
        uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorFrom").HeaderTooltip =
            uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorFrom").HeaderTooltip.ToCurrencySymbol();
        uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorFromText").HeaderText =
            uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorFromText").HeaderText.ToCurrencySymbol();
        uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorFromText").HeaderTooltip =
            uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorFromText").HeaderTooltip.ToCurrencySymbol();
        uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorTo").HeaderText =
       uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorTo").HeaderText.ToCurrencySymbol();
        uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorTo").HeaderTooltip =
            uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorTo").HeaderTooltip.ToCurrencySymbol();
        uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorToText").HeaderText =
            uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorToText").HeaderText.ToCurrencySymbol();
        uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorToText").HeaderTooltip =
            uxRadGrid.Columns.FindByUniqueName("ParameterIndicatorToText").HeaderTooltip.ToCurrencySymbol();
    }

    void uxRadGrid_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        uxRadGrid.MasterTableView.ClearEditItems();
    }

    protected void uxRiskScore_AfterSubmit(object sender, string param, decimal from, decimal to, decimal threshold, decimal thresholdHigh, int score)
    {
        Response.Redirect(Request.RawUrl);
    }

    //protected void uxCreateMode_Click(object sender, EventArgs e)
    //{
    //    OnPostBackActions(PostBackAction.CreateRiskScore, sender);
    //}

    private DataTable InitializeExport(DataTable dt)
    {
        dt.Columns.Add("ParameterIndicatorToText");
        dt.Columns.Add("ParameterIndicatorFromText");
        dt.Columns.Add("ParameterThresholdText");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (i == 101) {
                var sa = 1;
            }
            decimal TmpDecimal = 0;
            decimal TmpDecimalHigh = 0;
            string parameterIndicatorFrom = dt.Rows[i]["ParameterIndicatorFrom"].ToString();
            string parameterIndicatorTo = dt.Rows[i]["ParameterIndicatorTo"].ToString();
            string parameterDataType = dt.Rows[i]["ParameterDataType"].ToString().ToLower();
            int parameterPrecision = int.Parse(dt.Rows[i]["ParameterPrecision"].ToString());
            string indicatorFormat = "#,##0" + (parameterPrecision > 0 ? ".".PadRight(parameterPrecision + 1, '0') : string.Empty);
            string parameterThreshold = dt.Rows[i]["ParameterThreshold"].ToString();
            string parameterThresholdHigh = dt.Rows[i]["ParameterThresholdHigh"].ToString();
            string parameterThresholdType = dt.Rows[i]["ParameterThresholdType"].ToString();
            string parameterScore = dt.Rows[i]["ParameterScore"].ToString();
            string parameterIndicator = dt.Rows[i]["ParameterIndicator"].ToString();

            string thresholdType = dt.Rows[i]["ThresholdType"].ToString().ToLower();


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
                parameterThresholdHigh = decimal.TryParse(parameterThresholdHigh, out TmpDecimalHigh) ? TmpDecimalHigh.ToString("#,###") : string.Empty;
            }

            if (parameterDataType == SessionManager.CurrencySymbol)
            {
                decimal indicatorFrom = decimal.Parse(parameterIndicatorFrom);
                decimal indicatorTo = decimal.Parse(parameterIndicatorTo);
                dt.Rows[i]["ParameterIndicatorToText"] = GetIndicator(indicatorTo, indicatorFormat, parameterDataType);
                dt.Rows[i]["ParameterIndicatorFromText"] = GetIndicator(indicatorFrom, indicatorFormat, parameterDataType);
            }
            switch (parameterDataType)
            {
                case "%":
                    dt.Rows[i]["ParameterIndicatorToText"] = parameterIndicatorTo + parameterDataType;
                    dt.Rows[i]["ParameterIndicatorFromText"] = parameterIndicatorFrom + parameterDataType;
                    break;
                //case "$":
                //    dt.Rows[i]["ParameterIndicatorToText"] = parameterDataType + (parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo);
                //    dt.Rows[i]["ParameterIndicatorFromText"] = parameterDataType + (parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom);
                //    break;
                case "#":
                    dt.Rows[i]["ParameterIndicatorToText"] = (parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo);
                    dt.Rows[i]["ParameterIndicatorFromText"] = (parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom);
                    break;
                case "days":
                    dt.Rows[i]["ParameterIndicatorToText"] = (parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo) + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString();
                    dt.Rows[i]["ParameterIndicatorFromText"] = (parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom) + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString();
                    break;
                case "":
                    dt.Rows[i]["ParameterIndicatorFromText"] = VeraCodeSolution.ValidateResponseData(parameterIndicatorFrom.Equals("0") ? "0" : parameterIndicatorFrom);
                    dt.Rows[i]["ParameterIndicatorToText"] = VeraCodeSolution.ValidateResponseData(parameterIndicatorTo.Equals("0") ? "0" : parameterIndicatorTo);
                    break;
            }
            if (string.IsNullOrEmpty(parameterIndicator))
            {
                dt.Rows[i]["ParameterIndicatorToText"] = string.Empty;
                dt.Rows[i]["ParameterIndicatorFromText"] = string.Empty;
            }
            if (String.IsNullOrEmpty(parameterThreshold) || parameterThreshold == "0")
            {
                dt.Rows[i]["ParameterThresholdText"] = string.Empty;
            }
            else
            {
                //dt.Rows[i]["ParameterThresholdText"] = (parameterThreshold.Length > 0 ?
                //   "$" + parameterThreshold : string.Empty);
                string thresholdValue = string.Empty;
                string thresholdHighValue = string.Empty;

                if (thresholdType == SessionManager.CurrencySymbol)
                {
                    thresholdValue = Double.Parse(parameterThreshold) >= 0 ? VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(parameterThreshold, SessionManager.CurrencyFortmat))
                        : VeraCodeSolution.ValidateResponseData(("(" + FormatData.FormatCurrency(parameterThreshold, SessionManager.CurrencyFortmat).Replace("-", string.Empty) + ")"));
                    if (!String.IsNullOrEmpty(parameterThresholdHigh))
                    {
                        thresholdHighValue = Double.Parse(parameterThresholdHigh) >= 0 ? VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(parameterThresholdHigh, SessionManager.CurrencyFortmat))
                            : VeraCodeSolution.ValidateResponseData(("(" + FormatData.FormatCurrency(parameterThresholdHigh, SessionManager.CurrencyFortmat).Replace("-", string.Empty) + ")").Replace("$", SessionManager.CurrencySymbol));
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
                        thresholdValue = Double.Parse(parameterThreshold) >= 0 ? VeraCodeSolution.ValidateResponseData("" + parameterThreshold + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString()) : VeraCodeSolution.ValidateResponseData("(" + parameterThreshold.Replace("-", string.Empty) + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString() + ")");
                        if (!String.IsNullOrEmpty(parameterThresholdHigh))
                        {
                            thresholdHighValue = Double.Parse(parameterThresholdHigh) >= 0 ? VeraCodeSolution.ValidateResponseData("" + parameterThresholdHigh + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString()) : VeraCodeSolution.ValidateResponseData("(" + parameterThresholdHigh.Replace("-", string.Empty) + " " + GetLocalResourceObject("rm_RiskScores_aspx_cs_Days").ToString() + ")");
                        }
                        break;

                }

                if (parameterThresholdType == "LowHigh")
                {
                    dt.Rows[i]["ParameterThresholdText"] = thresholdValue + " - " + thresholdHighValue;
                }
                else
                {
                    dt.Rows[i]["ParameterThresholdText"] = thresholdValue;
                }
            }
        }

        return dt;
    }

    protected void uxRadGrid_UpdateCommand(object source, GridCommandEventArgs e)
    {
        e.Item.Edit = false;
    }

    protected void uxRadGrid_DeleteCommand(object source, GridCommandEventArgs e)
    {
        OnPostBackActions(PostBackAction.DeleteRiskScore, e);
    }

    protected void uxRadGrid_PreRender(object sender, System.EventArgs e)
    {
        uxRadGrid.MasterTableView.PagerStyle.AlwaysVisible = uxRadGrid.Items.Count > 0;
    }

    protected void uxRadGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        OnPostBackActions(PostBackAction.EditRiskScore, e);
    }

    protected void uxRiskScore_BeforeSubmit(object sender, EventArgs e)
    {
        string min = string.Empty;
        string max = string.Empty;
        string key = (sender as UserControls_rm_MCF_Score).SelectedParameterKey;

        //Purpose: check indicator value valid min and max
        //if (this.Session["MeoMeoMeo_Parameters"] != null)
        //{
        //    DataTable table = (DataTable)Session["RiskScore_Parameters"];
        //    var selectedParam = from r in table.Rows.Cast<DataRow>()
        //                        where r["ParameterKey"].ToString() == key
        //                        select new { IndicatorMin = r["IndicatorMin"].ToString(), IndicatorMax = r["IndicatorMax"].ToString() };
        //    if (selectedParam.Count() > 0)
        //    {
        //        min = selectedParam.First().IndicatorMin;
        //        max = selectedParam.First().IndicatorMax;
        //    }
        //}

        float minVal; float? min_Val = null;
        float maxVal; float? max_Val = null;

        if (float.TryParse(min, out minVal))
            min_Val = minVal;
        if (float.TryParse(max, out maxVal))
            max_Val = maxVal;


        (sender as UserControls_rm_MCF_Score).SetIndicatorMinMax(min_Val, max_Val);
    }



    public bool DoValidateInput(string from, string to, string threshold, string score)
    {
        const string VALID_CHARS = "0123456789.,-";
        //42733 - Bug #37105
        const string NAValue = "N/A";

        if (!HasOnlyCharacters(from, VALID_CHARS))
        {
            if (from.ToUpper().Trim() != NAValue)
            {
                return false;
            }
        }

        if (!HasOnlyCharacters(to, VALID_CHARS))
        {
            if (to.ToUpper().Trim() != NAValue)
            {
                return false;
            }
        }

        if (threshold.Length > 0)
        {
            if (!HasOnlyCharacters(threshold, VALID_CHARS))
            {
                if (threshold.ToUpper().Trim() != NAValue)
                {
                    return false;
                }
            }
        }

        if (score.Length > 0)
        {
            if (!HasOnlyCharacters(score, VALID_CHARS))
            {
                return false;
            }
        }

        return true;
    }

    bool HasOnlyCharacters(string input, string valid_chars)
    {
        string validHexValue = string.Empty;

        for (int i = 0; i < valid_chars.Length; i++)
        {
            validHexValue += "\\x" + ((byte)valid_chars[i]).ToString("X").ToUpper();
        }
        MatchCollection m = Regex.Matches(input, "[" + validHexValue + "]");
        if (m != null)
        {
            return m.Count == input.Length;
        }
        return false;
    }

    [WebMethod(EnableSession = true)]
    public static string[] ValidateExisting(string commandID, string recordID, string parameterKey, string from, string to, string riskSite)
    {
        int _recordID = 0;
        int _riskSite = -1;
        decimal indicatorFrom = 0;
        decimal indicatorTo = 0;

        Int32.TryParse(recordID, out _recordID);
        Int32.TryParse(riskSite, out _riskSite);
        Decimal.TryParse(from, out indicatorFrom);
        Decimal.TryParse(to, out indicatorTo);

        FilterParameterCollection parameters = new FilterParameterCollection();
        //if (_riskSite == -1)
        //    parameters.AddLoggedInUserRiskParams();
        //else
        //{
        //    parameters.AddLoggedInUserParams(-1);
        //    parameters.Add(new FilterParameter("@SiteID", _riskSite, DbType.Int32));
        //}
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@RecordID", recordID, DbType.Int32));
        parameters.Add(new FilterParameter("@ParameterKey", parameterKey, DbType.String));
        parameters.Add(new FilterParameter("@ParameterIndicatorFrom", indicatorFrom, DbType.Decimal));
        parameters.Add(new FilterParameter("@ParameterIndicatorTo", indicatorTo, DbType.Decimal));
        parameters.Add(new FilterParameter("@RecordCount", 1, DbType.Int32, true));

        FilterParameterCollection parameterOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_ValidateRiskScore", parameters, out parameterOut);

        int result = Convert.ToInt32(parameterOut[0].ParameterValue);

        bool valid = (result == 0 ? true : false);
        return new string[] { valid.ToString().ToLower(), commandID.Replace('_', '$') };
    }
    protected void uxAttributeGrid_PreRender(object sender, EventArgs e)
    {

    }
    protected void uxAttributeGrid_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        e.Item.Edit = false;
    }
    protected void uxAttributeGrid_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.Item;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParamsWithRecId();
        parameters.Add(new FilterParameter("@RecordID", int.Parse(dataItem.GetDataKeyValue("RecordID").ToString()), DbType.Int32));
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MRS_Delete_AttributeRiskScore", parameters, out parameterOut);
        uxAttributeGrid.MasterTableView.ClearEditItems();
        uxAttributeGrid.Rebind();
    }
    protected void uxAttributeGrid_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
    protected void uxAttributeGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (GeneralFuncsLib.CheckOnOffModule(WebSiteConstants.AttributeRiskScoreModuleName))
            OnDataBindControls(DataBindAction.BindAttributeRiskScore, sender);
    }
    protected void uxEditAttributeRiskScore_Cancel(object sender, EventArgs e)
    {

    }

    protected void uxAttributeGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case GridItemType.Item:
            case GridItemType.AlternatingItem:
                {
                }
                break;
            case GridItemType.EditFormItem:
                {
                    if (e.Item.IsInEditMode)
                    {
                        GridEditFormItem dataItem = (GridEditFormItem)e.Item;
                        DataRowView dataRow = e.Item.DataItem as DataRowView;
                        UserControls_rm_MCF_EditAttributeRiskScore uxEditAttributeRiskScore = (UserControls_rm_MCF_EditAttributeRiskScore)e.Item.FindControl("uxEditAttributeRiskScore");
                        uxEditAttributeRiskScore.BindAttributeList();
                        if (!uxEditAttributeRiskScore.IsBinded)
                        {
                            uxEditAttributeRiskScore.RedId = dataRow["RecordID"].ToInt();
                            uxEditAttributeRiskScore.BindAttributeList();
                            uxEditAttributeRiskScore.SetSelectedAttributeName(dataRow["AttributeID"].ToASString());
                            uxEditAttributeRiskScore.RangeName = dataRow["RangeName"].ToASString();
                            uxEditAttributeRiskScore.Score = dataRow["Score"].ToInt();

                            if (uxEditAttributeRiskScore.fieldTypeMode == AttributeRiskScoreMode.FromTo)
                            {
                                decimal FromValue = 0;
                                decimal.TryParse(dataRow["FromValue"].ToString(), out FromValue);
                                uxEditAttributeRiskScore.FromValue = (int)Math.Round(FromValue);
                                decimal ToValue = 0;
                                decimal.TryParse(dataRow["ToValue"].ToString(), out ToValue);
                                uxEditAttributeRiskScore.ToValue = (int)Math.Round(ToValue);
                            }
                            else
                            {
                                uxEditAttributeRiskScore.OperandKey = dataRow["OperandValue"].ToInt();
                                if (uxEditAttributeRiskScore.fieldTypeMode == AttributeRiskScoreMode.OperandMetric)
                                {
                                    uxEditAttributeRiskScore.MetricFromTo = dataRow["MetricValue"].ToString();
                                }
                                else
                                {
                                    uxEditAttributeRiskScore.MetricValue = dataRow["MetricValue"].ToString();
                                }
                            }
                            uxEditAttributeRiskScore.IsBinded = true;
                        }
                        // pnlAttributeRiskScore.Attributes["class"] = "create-attribute-rs-form collapse";
                    }
                }
                break;
        }
    }

    protected void uxEditAttributeRiskScore_OnSubmit(object sender, int resultCode)
    {
        if (resultCode == 1)
        {
            foreach (GridItem item in uxAttributeGrid.MasterTableView.Items)
            {
                if (item is GridEditableItem)
                {
                    ((GridEditableItem)item).Edit = false;
                }
            }
            uxAttributeGrid.Rebind();

        }
    }

    private string MyConvert(object obj)
    {
        if (obj.IsNullOrEmpty())
            return string.Empty;
        else
        {
            return string.Format("{0:N0}", (Decimal.Parse(obj.ToString())));
        }
    }

    protected void uxExportAttributeGrid_NeedExportConfig(object sender, ExportConfig exportConfig)
    {
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExportAttributeGrid.GridHeader);
        uxExportAttributeGrid.Formatter = new Dictionary<string, Func<object, string>>();
        uxExportAttributeGrid.Formatter.Add("FromValue", new Func<object, string>(MyConvert));
        uxExportAttributeGrid.Formatter.Add("ToValue", new Func<object, string>(MyConvert));
    }

    private string GetIndicator(decimal indicator, string indicatorFormat, string parameterDataType)
    {
        string indVal = indicator.ToString();
        if (indicator == 0)
            return indicator.ToString();
        if (indicator < 0)
        {
            indVal = "-" + parameterDataType + (-1 * indicator).ToString(indicatorFormat);
        }
        else
        {
            indVal = parameterDataType + indicator.ToString(indicatorFormat);
        }
        return " " + indVal;
    }
    /// <summary>
    /// For Risk Model Score: If allow decimall will default precision = 4.
    /// Other: If allow decimall will get value from database config.
    /// </summary>
    /// <param name="key">ParameterKey</param>
    /// <param name="parameterPrecisionInt">Precision of decimal</param>
    /// <returns></returns>

    private Tuple<bool, int> GetDecimalAndPrecisionInConfig(string key, int parameterPrecisionInt)
    {
        var isDecimal = BuGeneralFuncsLib.CheckExistsInSplitToArray(WebSiteSettings.ParametersAllowDecimal, ',', key);
        if (isDecimal)
        {
            return new Tuple<bool, int>(isDecimal, parameterPrecisionInt);
        }
        var riskRiskModelScoreAllowDecimal = BuGeneralFuncsLib.CheckExistsInSplitToArray(WebSiteSettings.RiskParameterKeysAllowDecimal, ',', key);
        return new Tuple<bool, int>(riskRiskModelScoreAllowDecimal, 4);
    }
}
