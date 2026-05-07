using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Common.Logger;
using AS.Controls.Pages;
using AS.Web.Business.RiskReport;
using AS.Web.Business.RiskReport.Models;
using AS.Web.Business.Shared.Constants;
using AS.Web.Business.Shared.Enums;
using AS.Web.Business.Shared.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using GeneralFuncsLibBusiness = AS.Web.Business.General.GeneralFuncsLib;

public partial class UserControls_rm_MCF_NewRiskReport_ForwardDelivery : GlobalUserControl
{

    enum DataBindAction
    {
        BindGetForwardDelivery
    }

    enum PostBackAction
    {
        Calculate,
        Clear
    }

    #region properties
    private const string MERCHANT_NUMBER = "MerchantNumber";

    private EnumFWDAction FEUserAction { get; set; }
    public string MerchantNumber
    {
        get
        {
            return GeneralFuncsLib.NvlString(ViewState[MERCHANT_NUMBER]);
        }
        set
        {
            ViewState[MERCHANT_NUMBER] = value;
        }
    }

    public DataTable ForwardDeliveryInfo
    {
        get
        {
            if (RiskSessionManager.RiskReportForwardDelivery != null)
                return RiskSessionManager.RiskReportForwardDelivery;
            return null;
        }
    }

    //Contains data tables after multi-thread excuted
    public List<DataSourceParallelResponse> DataSources { get; set; }

    #endregion

    #region ctor
    private readonly IForwardDeliveryBussiness _forwardDeliveryBussiness;
    public UserControls_rm_MCF_NewRiskReport_ForwardDelivery() : this(new ForwardDeliveryBussiness(WebServices.RiskServices))
    {
    }
    public UserControls_rm_MCF_NewRiskReport_ForwardDelivery(IForwardDeliveryBussiness forwardDeliveryBussiness)
    {
        _forwardDeliveryBussiness = forwardDeliveryBussiness;
    }
    #endregion
    private DataTable GetForwardDelivery()
    {
        return _forwardDeliveryBussiness.GetForwardDelivery(new GetForwardDeliveryRequest
        {
            UserMode = GeneralFuncsLib.GetUserMode(),
            MerchantNumber = this.MerchantNumber
        }, SessionManager.CurrentUser);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //GetData();
            }
        }
        catch (ViewStateException)
        {
            LoggerManager.Error(string.Format("UserControls_rm_MCF_NewRiskReport_ForwardDelivery - ViewStateException - Request={0}; SessionID={1}", Context.Request.Url, Session.SessionID));
            throw;
        }


    }

    protected override void OnPostBackActions(Enum type, object param)
    {
        try
        {
            if (Page.IsIntruderDetected) return;

            switch ((PostBackAction)type)
            {
                case PostBackAction.Calculate:
                    {
                        var userAction = EnumFWDAction.Calc;
                        _forwardDeliveryBussiness.CalculateForwardDelivery(new CalculateForwardDeliveryRequest
                        {
                            UserMode = GeneralFuncsLib.GetUserMode(),
                            MerchantNumber = MerchantNumber,
                            CreditTimeliness = uxDays.Text.ToInt(),
                            NDX = uxNDX.Text.ToInt(),
                            NDXPercent = GetNDXPercentValue().ToInt(),
                            EnumFWDAction = userAction
                        }, SessionManager.CurrentUser);

                        RiskSessionManager.RiskReportForwardDelivery = GetForwardDelivery();
                        FEUserAction = userAction;
                        GetData();
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "registerInputEvent", "registerInputEvent()", true);
                    }
                    break;
                case PostBackAction.Clear:
                    {
                        var userAction = EnumFWDAction.Clear;
                        _forwardDeliveryBussiness.CalculateForwardDelivery(new CalculateForwardDeliveryRequest
                        {
                            UserMode = GeneralFuncsLib.GetUserMode(),
                            MerchantNumber = MerchantNumber,
                            EnumFWDAction = userAction
                        }, SessionManager.CurrentUser);

                        RiskSessionManager.RiskReportForwardDelivery = null;
                        FEUserAction = userAction;
                        GetData();
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "registerInputEvent", "registerInputEvent()", true);
                    }
                    break;
            }
        }
        catch (ViewStateException)
        {
            LoggerManager.Error(string.Format("UserControls_rm_MCF_NewRiskReport_ForwardDelivery - ViewStateException - Request={0}; SessionID={1}", Context.Request.Url, Session.SessionID));
            throw;
        }
    }

    protected void uxCalculate_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Calculate, sender);
    }

    protected void uxClear_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Clear, sender);
    }


    protected override void OnDataBindControls(Enum type, object sender)
    {   
        DataTable info = this.ForwardDeliveryInfo;
        if (info == null || info.Rows.Count == 0)
        {
            GetDefaultOrClearForwardDelivery(FEUserAction);
            return;
        }

        DataRow row = info.Rows[0];

        EnumFWDAction enumFWDAction;
        var uerAction = GeneralFuncsLibBusiness.GetValueDataRow(row, ForwardDeliveryConstants.COLUMN_NAME_FE_USER_ACTION);
        Enum.TryParse(uerAction, out enumFWDAction);
        FEUserAction = enumFWDAction;
        if (enumFWDAction == EnumFWDAction.Clear)
        {
            GetDefaultOrClearForwardDelivery(EnumFWDAction.Clear);
            return;
        }
        uxCreditTimelinessDays.InnerText = ShowData(row[ForwardDeliveryConstants.COLUMN_NAME_CREDIT_TIME_LINESS]);
        var tooltipDays = HistoryTooltip(ShowData(row["LastCreditTimelinessUpdatedBy"]), row["LastCreditTimelinessUpdatedOn"]);
        RM_MCF_GeneralFuncsLib.AddTooltip(uxCreditTimelinessDays, tooltipDays);

        //Get DNX,% DNX to input texbox, lable detail and tooltip
        GetInfoDNXByUserOrSupMIF(row);
        uxDays.Text = GeneralFuncsLibBusiness.GetValueDataRow(row, ForwardDeliveryConstants.COLUMN_NAME_CREDIT_TIME_LINESS);
        uxPeriodFrom.Text = GeneralFuncsLib.FormatDate(row[ForwardDeliveryConstants.COLUMN_NAME_FROM_DATE]);  
        uxPeriodTo.Text = GeneralFuncsLib.FormatDate(row[ForwardDeliveryConstants.COLUMN_NAME_TO_DATE]);
        uxActualSaleVolume.Text = ShowData(FormatCurrency(row["ActualSaleVolume"]));
        uxMTDCreditRatio.Text = ShowData(GeneralFuncsLib.FormatPercent(row["CreditRatio"]));
        uxMTDChargebackRatio.Text = ShowData(GeneralFuncsLib.FormatPercent(row["ChargebackRatio"]));
        uxCreditRiskAmount.Text = ShowData(FormatCurrency(row["CreditRiskAmount"]));
        uxChargebackRiskAmount.Text = ShowData(FormatCurrency(row["ChargebackRiskAmount"]));
        uxNDXRiskAmount.Text = ShowData(FormatCurrency(row["NDXRiskAmount"]));
        uxTotalRiskAmount.Text = ShowData(FormatCurrency(row["TotalRiskAmount"]));
        var updatedBy = row["UpdatedBy"];
        var msg = string.Empty;
        if (updatedBy == DBNull.Value || updatedBy.IsNullOrEmpty())
            msg = HistoryTooltip(ShowData(row["CreatedBy"]), row["CreatedOn"], true);
        else
            msg = HistoryTooltip(ShowData(row["UpdatedBy"]), row["UpdatedOn"], true);
        Literal9.Text = msg;
    }

    protected string ShowValidateResponseData(object value)
    {
        return (value == DBNull.Value || value.IsNullOrEmpty()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : VeraCodeSolution.ValidateResponseData(value.ToString());
    }

    protected string ShowData(object value)
    {
        return (value == DBNull.Value || value.IsNullOrEmpty()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : VeraCodeSolution.DoVeraCode(value.ToString());
    }

    private static string HistoryTooltipBySystemOrUser(string user, object date, bool isMsg = false)
    {
        user = (string.IsNullOrEmpty(user) || user == GlobalConstants.HTML_EM_DASH_ENCODE) ? GlobalConstants.SYSTEM_USER_DEFAULT.ToLower() : user;
        return HistoryTooltip(user, date, isMsg);
    }
    private static string HistoryTooltip(string user, object date, bool isMsg = false)
    {
        string result = string.Empty;
        string hour = string.Format("{0:hh:mm tt}", date);
        result = string.Format("Last updated by {0} on {1} at {2}", user, GeneralFuncsLib.FormatDate(date), hour);
        if (isMsg)
        {
            result = string.Format("The Total Risk Amount was last updated on {0} at {1}", GeneralFuncsLib.FormatDate(date), hour);
        }
        return result;
    }

    public void GetData()
    {
        OnDataBindControls(DataBindAction.BindGetForwardDelivery);
    }

    protected string FormatCurrency(object data)
    {
        if (data != null && !string.IsNullOrEmpty(data.ToString()))
            return GeneralFuncsLib.FormatCurrency(data, "C");
        return string.Empty;
    }

    #region Enhance Performance  
    public void BindDataFromThread(List<DataSourceParallelResponse> dataSources)
    {
        DataSources = dataSources;
        DataTable binData = null;
        if (dataSources.IsNotNullData())
        {
            var data = DataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindGetForwardDelivery.ToString());
            if (data.IsNotNullData())
                binData = data.DataSource;
        }

        RiskSessionManager.RiskReportForwardDelivery = binData.IsNotNullData() ? binData : GetForwardDelivery();

        GetData();
    }

    #endregion

    private string GetNDXPercentValue()
    {
        return string.IsNullOrEmpty(uxNDXPercent.Text) ? ForwardDeliveryConstants.DEFAULT_NDX_PERCENT_VALUE : uxNDXPercent.Text.Trim();
    }
    private void GetInfoDNXByUserOrSupMIF(DataRow row)
    {
        string txtNDXValue = GeneralFuncsLibBusiness.GetValueDataRow(row, ForwardDeliveryConstants.COLUMN_NAME_NDX);
        string txtNDXPercentValue = GeneralFuncsLibBusiness.GetValueDataRow(row, ForwardDeliveryConstants.COLUMN_NAME_NDX_PERCENT);
        bool isUserPercentNDX = IsUserUpdated(row, ForwardDeliveryConstants.COLUMN_NAME_LAST_NDX_PER_UPDATED_BY);

        uxNDX.Text = txtNDXValue;
        uxNDXPercent.Text = string.IsNullOrEmpty(txtNDXPercentValue) && !isUserPercentNDX ? ForwardDeliveryConstants.DEFAULT_NDX_PERCENT_VALUE : txtNDXPercentValue;

        var LastNDXUpdatedBy = GeneralFuncsLibBusiness.GetValueDataRow(row, ForwardDeliveryConstants.COLUMN_NAME_LAST_NDX_UPDATED_BY, GlobalConstants.HTML_EM_DASH_ENCODE);
        var LastNDXUpdatedOn = row[ForwardDeliveryConstants.COLUMN_NAME_LAST_NDX_UPDATED_ON];
        var tooltipNDX = HistoryTooltip(LastNDXUpdatedBy, LastNDXUpdatedOn);
        uxNDXValue.InnerText = ShowData(txtNDXValue);
        RM_MCF_GeneralFuncsLib.AddTooltip(uxNDXValue, tooltipNDX);


        var LastNDXPerUpdatedBy = GeneralFuncsLibBusiness.GetValueDataRow(row, ForwardDeliveryConstants.COLUMN_NAME_LAST_NDX_PER_UPDATED_BY, GlobalConstants.HTML_EM_DASH_ENCODE);
        var LastNDXPerUpdatedOn = row[ForwardDeliveryConstants.COLUMN_NAME_LAST_NDX_PER_UPDATED_ON];
        var tooltipPercentNDX = HistoryTooltipBySystemOrUser(LastNDXPerUpdatedBy, LastNDXPerUpdatedOn);
        uxNDXPercentValue.InnerText = ShowData(txtNDXPercentValue);
        RM_MCF_GeneralFuncsLib.AddTooltip(uxNDXPercentValue, tooltipPercentNDX);
    }

    private static bool IsUserUpdated(DataRow row, string columnName)
    {
        var data = GeneralFuncsLibBusiness.GetValueDataRow(row, columnName);
        if (string.IsNullOrEmpty(data) || IsSystemUser(data))
        {
            return false;
        }
        return true;
    }

    private void GetDefaultOrClearForwardDelivery(EnumFWDAction action = EnumFWDAction.System)
    {
        string ndxPercentValue = action == EnumFWDAction.Clear ? string.Empty : ForwardDeliveryConstants.DEFAULT_NDX_PERCENT_VALUE;
        var tooltipPercentNDX = action == EnumFWDAction.Clear ? string.Empty : HistoryTooltipBySystemOrUser(null, DateTime.Now);
        uxCreditTimelinessDays.InnerText = ShowData(string.Empty);
        RM_MCF_GeneralFuncsLib.AddTooltip(uxCreditTimelinessDays, string.Empty);
        uxNDXValue.InnerText = ShowData(string.Empty);
        RM_MCF_GeneralFuncsLib.AddTooltip(uxNDXValue, string.Empty);
        uxNDXPercentValue.InnerText = ShowData(ndxPercentValue);       
        RM_MCF_GeneralFuncsLib.AddTooltip(uxNDXPercentValue, tooltipPercentNDX);
        uxDays.Text = string.Empty;
        uxNDX.Text = string.Empty;
        uxNDXPercent.Text = ndxPercentValue;
        uxPeriodFrom.Text = string.Empty;
        uxPeriodTo.Text = string.Empty;
        uxActualSaleVolume.Text = ShowData(string.Empty);
        uxMTDCreditRatio.Text = ShowData(string.Empty);
        uxMTDChargebackRatio.Text = ShowData(string.Empty);
        uxCreditRiskAmount.Text = ShowData(string.Empty);
        uxChargebackRiskAmount.Text = ShowData(string.Empty);
        uxNDXRiskAmount.Text = ShowData(string.Empty);
        uxTotalRiskAmount.Text = ShowData(string.Empty);
        Literal9.Text = string.Empty;
    }

    private static bool IsSystemUser(string user)
    {
        if (string.IsNullOrEmpty(user))
        {
            return false;
        }
        var lstUserSystem = GetSystemUser();
        return lstUserSystem.Any(x => x.Equals(user, StringComparison.OrdinalIgnoreCase));
    }
    private static List<string> GetSystemUser()
    {
        //Get system user from config xml
        return GeneralFuncsLibBusiness.SplitToArray(WebSiteSettings.SYSTEM_USER, ',');
    }    
}
