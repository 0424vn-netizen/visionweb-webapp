using AS.Common;
using AS.Common.DBManager;
using AS.Common.Logger;
using AS.Controls.Pages;
using AS.Web.Business.Shared.Constants;
using AS.Web.Business.Shared.Enums;
using AS.Web.Business.Shared.Models;
using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using BuGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;

public partial class UserControls_rm_MCF_NewRiskReport_MerchantInformation : GlobalUserControl
{

    enum DataBindAction
    {
        BindMerchantInformation,
        BindMerchantHierarchy,
        BindMerchantClassfisication,
        BindGetMerchantInfo,
        BindProfileList,
        BindDataElement
    }

    enum PostBackAction
    {
        SaveMerchantInfoEvent
    }

    #region properties
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string BACK_END_PROCESSOR = "BackEndProcessor";
    private const string IS_SWITCH = "IsSwitch";
    private const string TSYS_MID = "TSYS_MID";
    private const string FDR_MID = "FDR_MID";
    private const string SPA_GET_MERCHANT_INFO = "spa_RM_MCF_Get_RiskReport_MerchantInfo";
    private const string SPA_GET_MERCHANT_INFO_V2 = "spa_RM_MCF_Get_RiskReport_MerchantInfo_V2";
    private const string SPA_GET_MERCHANT_HIERARCHY_FOR_RISK = "spa_RM_MCF_GetMerchantHierarchyForRiskReport";
    private const string SPA_GET_MIF_CLASSIFICATION = "spa_RM_MCF_MRS_Get_MIFClassificationByMerchant";
    private const string SPA_PROFILE_LIST = "spa_RM_MCF_GetProfileList";
    private const string SPA_MVRK_DATA_ELEMENT = "spa_RM_MCF_Get_RiskReport_MerchantInfo_Maverick";

    //private bool _isBinData = false;
    //public bool IsBindData { get { return _isBinData; } set { _isBinData = value; } }

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

    public DataTable MerchantInfo
    {
        get
        {
            if (RiskSessionManager.RiskReportMerchantInfo != null)
                return RiskSessionManager.RiskReportMerchantInfo;
            return null;
        }
    }

    //Contains data tables after multi-thread excuted
    public List<DataSourceParallelResponse> DataSources { get; set; }

    #endregion
    private DataTable _RiskScoreInfor;

    public string BindValue(string colName)
    {
        if (_RiskScoreInfor == null || _RiskScoreInfor.Rows.Count <= 0)
            return WebSiteConstants.HTML_EM_DASH_ENCODE;
        DataRow dr = _RiskScoreInfor.Rows[0];
        return dr[colName] == DBNull.Value || dr[colName].ToString().IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : dr[colName].ToString();

    }

    private DataTable GetMerchantInfo()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
        parameters.AddLanguageID();
        var spa = SessionManager.CurrentClient == WebSiteConstants.ESQR_CLIENT ? SPA_GET_MERCHANT_INFO_V2 : SPA_GET_MERCHANT_INFO;
        return WebServices.RiskServices.GetReports(spa, parameters);
    }

    private DataTable GetMVRKDataElement()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
        return WebServices.RiskServices.GetReports(SPA_MVRK_DATA_ELEMENT, parameters);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (GeneralFuncsLib.GetDataOfExtendedSetting("Show_HR").ToLower().Equals("true"))
            {
                divHrCode.Visible = true;
            }
            if (GeneralFuncsLib.GetDataOfExtendedSetting("Show_ACH").ToLower().Equals("true"))
            {
                divACHDelay.Visible = true;
            }
            if (GeneralFuncsLib.GetDataOfExtendedSetting("DISABLE_SYSPRINAGENT_FIELD").ToLower().Equals("true"))
            {
                pnlSysPrinAgent.Visible = false;
            }
            if (!IsPostBack)
            {
                //GetData();
            }

            //45923 - SIGNA- VisionWeb Supplemental MIF
            if (SessionManager.CurrentClient == WebSiteConstants.SIGNAPAY_CLIENT)
                ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format("showSupplementalSIGNAPAY()"));
            //End

            if (GeneralFuncsLib.CheckOnOffModule(WebSiteConstants.AttributeRiskScoreModuleName) &&
               (Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_MSRISK_INFO) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RISK_INFO)))
            {
                pnlClassificationRiskScore.Visible = true;
                OnDataBindControls(DataBindAction.BindMerchantClassfisication);
            }
            else
            {
                pnlClassificationRiskScore.Visible = false;
            }

            if (Page.IsUserWithPermission("ViewWebsites"))
            {
                uxLinkButtonViewWebSites.Visible = true;
            }
            else
            {
                uxLinkButtonViewWebSites.Visible = false;
            }
            BindRiskExtendInfo();
        }
        catch (ViewStateException)
        {
            LoggerManager.Error(string.Format("UserControls_rm_MCF_NewRiskReport_MerchantInformation - ViewStateException - Request={0}; SessionID={1}", Context.Request.Url, Session.SessionID));
            throw;
        }
    }

    protected void BindRiskExtendInfo()
    {
        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.GverifyCode))
        {
            uxGVerifyCode.Visible = true;
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.GauthenticateCode))
        {
            uxGAuthenticateCode.Visible = true;
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.G2Compass))
        {
            uxG2CompassAutoApprovalIndicator.Visible = true;
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.DaystoFund))
        {
            uxDaystoFund.Visible = true;
        }
        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.FICOScore))
        {
            uxFICOScore.Visible = true;
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.FutureDeliveryIndicator))
        {
            uxFutureDeliveryIndicator.Visible = true;
        }
        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.FutureDeliveryDayMaximum))
        {
            uxFutureDeliveryDayMaximum.Visible = true;
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.ActualDeliveryDays))
        {
            uxActualDeliveryDaysItem.Visible = true;
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.SeasonalIndicator))
        {
            uxSeasonalIndicator.Visible = true;
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.SeasonalActiveMonths))
        {
            uxSeasonalActiveMonths.Visible = true;
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.SeasonalIndicatorStandard))
        {
            uxSeasonalIndicatorStandard.Visible = true;
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.RiskRatingStandard))
        {
            uxRiskRatingStandard.Visible = true;
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.FICOScoreStandard))
        {
            uxFICOScoreStandard.Visible = true;
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.DaystoFundStandard))
        {
            uxDaystoFundStandard.Visible = true;
        }

        if (SessionManager.CurrentClient == WebSiteConstants.ESQR_CLIENT)
        {
            uxStatusDateItem.Visible = true;
            uxRiskLevelMIItem.Visible = true;
            uxDescGoodServiceItem.Visible = true;
            uxSubMCCItem.Visible = true;
            uxAdvDepositItem.Visible = true;
            uxStatusNameItem.Visible = true;
            uxThirdCol.Visible = true;
            uxExpeditedFundingItem.Visible = true;
            uxAdvDepositsDaysItem.Visible = true;
            uxApprovedNDXDaysItem.Visible = true;
        }

        if (SessionManager.CurrentClient == WebSiteConstants.MVRK_CLIENT)
        {
            dataEmlemets.Visible = true;
        }

        if (SessionManager.CurrentClient == WebSiteConstants.WFNB_CLIENT)
        {
            uxRiskRatingItem.Visible = true;
        }

        if (GetIsShowOriginallyContractValuesInConfig())
        {
            uxOriginallyBoardedAnnualVolumeItem.Visible = true;
            uxOriginallyBoardedAverageTicketItem.Visible = true;
            uxOriginallyBoardedHighTicketItem.Visible = true;
        }
    }

    protected override void OnPostBackActions(Enum type, object param)
    {
        if (Page.IsIntruderDetected) return;

        switch ((PostBackAction)type)
        {
            case PostBackAction.SaveMerchantInfoEvent:
                {
                    int profileType = -1;
                    if (GeneralFuncsLib.GetDataOfExtendedSetting("DISABLE_MIF_PROFILE_EDIT_MAINTENANCE") == "true")
                    {
                        Int32.TryParse(uxProfileReadOnly.Text, out profileType);
                    }
                    else
                    {
                        Int32.TryParse(uxProfile.SelectedValue, out profileType);
                    }
                    Int32.TryParse(uxProfile.SelectedValue, out profileType);
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    FilterParameterCollection outValue = new FilterParameterCollection();
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    if (profileType != -1)
                        parameters.Add(new FilterParameter("@Profile", profileType, DbType.Int32));
                    parameters.Add(new FilterParameter("@Watch", uxWatch.Checked, DbType.Boolean));
                    parameters.Add(new FilterParameter("@UpdatedByUserId", SessionManager.CurrentUser.RecId.ToString(), DbType.AnsiString));
                    WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveRiskReportMerchant", parameters, out outValue);
                    RiskSessionManager.RiskReportMerchantInfo = GetMerchantInfo();
                    GetData();
                    // Save UserActivity
                    string activityText = string.Format(GetLocalResourceObject("RiskReportMerchantInformation_ascx_cs_RiskReportProfileWatch").ToString(), this.MerchantNumber, profileType, uxWatch.Checked ? GetLocalResourceObject("RiskReportMerchantInformation_ascx_cs_Check").ToString() : GetLocalResourceObject("RiskReportMerchantInformation_ascx_cs_UnCheck").ToString());
                    GeneralFuncsLib.SaveUserActivity(this.MerchantNumber, activityText, true);
                }
                break;
        }
    }


    bool isShowRelationshipManager = GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RELATIONSHIP_MANAGER").Equals("true") ? true : false;

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;

        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMerchantInformation:
                {
                    DataTable info = this.MerchantInfo;
                    if (info == null || info.Rows.Count == 0)
                        return;

                    DataRow row = info.Rows[0];
                    DataTable profileList = null;
                    DataTable bindProfile = null;
                    if (DataSources.IsNotNullData())
                    {
                        var data = DataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindProfileList.ToString());
                        if (data.IsNotNullData())
                        {
                            bindProfile = data.DataSource;
                        }
                    }

                    if (bindProfile.IsNotNullData())
                    {
                        profileList = bindProfile;
                    }
                    else
                    {
                        parameters.Clear();
                        parameters.AddLoggedInUserReportingParams(false);
                        profileList = WebServices.RiskServices.GetReports(SPA_PROFILE_LIST, parameters);
                    }
                    RadComboBoxItem item = new RadComboBoxItem(string.Empty, "-1");
                    item.Height = Unit.Pixel(12);
                    if (GeneralFuncsLib.GetDataOfExtendedSetting("DISABLE_MIF_PROFILE_EDIT_MAINTENANCE") == "true")
                    {
                        uxProfile.Visible = false;
                        uxProfileReadOnly.Visible = true;
                    }
                    else
                    {
                        uxProfile.Visible = true;
                        uxProfileReadOnly.Visible = false;

                        uxProfile.DataSource = profileList;
                        uxProfile.DataBind();
                        uxProfile.Items.Insert(0, item);
                    }
                    uxMerchantName.Text = ShowData(row["MerchantName"]);
                    uxHrCode.Text = ShowData(row["HRCode"]);
                    uxACHDelay.Text = ShowData(row["ACHDelay"]);
                    uxSYSPRINAgent.Text = ShowData(row["SYSPRINAGENT"]);
                    uxApprovalDate.Text = ShowData(GeneralFuncsLib.FormatDate(row["ApprovalDate"]));
                    uxLastActiveDate.Text = ShowData(GeneralFuncsLib.FormatDate(row["LastActiveDate"]));
                    if (SessionManager.CurrentClient == WebSiteConstants.ALLIEDWALLET_CLIENT)
                    {
                        uxLastStatement.Visible = false;
                        uxPlhRiskLevel.Visible = true;
                        uxRiskLevel.Text = ShowValidateResponseData(row["RiskLevel"]);
                        uxOwner2.Visible = uxOwner3.Visible = uxOwner4.Visible = uxOwner5.Visible = false;
                        if (row["Owner_1"] != DBNull.Value && !row["Owner_1"].IsNullOrEmpty())
                        {
                            uxOwner.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.NvlString(row["Owner_1"]));
                            if (row["Owner_2"] != DBNull.Value && !row["Owner_2"].IsNullOrEmpty())
                            {
                                uxOwner2.Visible = true;
                                uxOwner2.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.NvlString(row["Owner_2"]));
                                if (row["Owner_3"] != DBNull.Value && !row["Owner_3"].IsNullOrEmpty())
                                {
                                    uxOwner3.Visible = true;
                                    uxOwner3.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.NvlString(row["Owner_3"]));
                                    if (row["Owner_4"] != DBNull.Value && !row["Owner_4"].IsNullOrEmpty())
                                    {
                                        uxOwner4.Visible = true;
                                        uxOwner4.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.NvlString(row["Owner_4"]));
                                        if (row["Owner_5"] != DBNull.Value && !row["Owner_5"].IsNullOrEmpty())
                                        {
                                            uxOwner5.Visible = true;
                                            uxOwner5.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.NvlString(row["Owner_5"]));
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            uxOwner.Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
                        }

                    }
                    else
                    {
                        uxOwner.Text = ShowValidateResponseData(row["Owner"]);
                    }

                    //42397 - VW - MCPS - Add new Parameter P176 for First Batch Rule 
                    uxFirstBatchAmount.Text = string.IsNullOrEmpty(row["FirstBatchAmount"].ToString()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatCurrency(row["FirstBatchAmount"], "C"));
                    uxFirstBatchDate.Text = string.IsNullOrEmpty(row["FirstBatchDate"].ToString()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatDate(row["FirstBatchDate"]));
                    var firstBatch = GeneralFuncsLib.GetClientExtendedSetting("VIEW_FIRST_BATCH_INFO");
                    uxPlFirstBatch.Visible = firstBatch.Data != null && firstBatch.Data.ToLower().Equals("true");

                    uxLastStatementDate.Text = ShowValidateResponseData(GeneralFuncsLib.FormatDate(row["LastStatementDate"]));

                    uxStatus.Text = ShowValidateResponseData(row["Status"]);
                    uxFundingStatus.Text = ShowValidateResponseData(row["FundingStatus"]);
                    uxSIC.Text = ShowData(row["SIC"]);
                    uxCreditScore.Text = ShowValidateResponseData(row["CreditScore"]);
                    uxUrl.Text = ShowValidateResponseData(row["MerchantURL"]);

                    if (GeneralFuncsLib.EnabledTSYSConversion() && row[BACK_END_PROCESSOR].ToString().Equals(BackEndProcessor.TSYS, StringComparison.OrdinalIgnoreCase))
                    {
                        uxPlhConversionDate.Visible = true;
                        uxConversionDate.Text = ShowValidateResponseData(GeneralFuncsLib.FormatDate(row["ActualConversionDate"]));
                    }
                    else
                    {
                        uxPlhConversionDate.Visible = false;
                    }

                    if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_MARKET_DATA").Equals("true", StringComparison.OrdinalIgnoreCase) && info.Columns.Contains("MDDescription"))
                    {
                        pnlMarketData.Visible = true;
                        uxMarketData.Text = ShowValidateResponseData(row["MDDescription"]);
                    }
                    else
                    {
                        pnlMarketData.Visible = false;
                    }

                    if (!string.IsNullOrEmpty(row["ProfileType"].ToString()))
                    {
                        if (GeneralFuncsLib.GetDataOfExtendedSetting("DISABLE_MIF_PROFILE_EDIT_MAINTENANCE") == "true")
                        {
                            if (profileList != null && profileList.Rows.Count > 0)
                            {
                                string profileText = string.Empty;
                                for (int i = 0; i < profileList.Rows.Count; i++)
                                {
                                    if (GeneralFuncsLib.NvlString(profileList.Rows[i]["DataKey"]) == GeneralFuncsLib.NvlString(row["ProfileType"]))
                                    {
                                        profileText = GeneralFuncsLib.NvlString(profileList.Rows[i]["DataText"]);
                                        continue;
                                    }
                                }
                                uxProfileReadOnly.Text = profileText;
                            }
                            else
                            {
                                uxProfileReadOnly.Text = string.Empty;
                            }
                        }
                        else
                        {
                            if (uxProfile.FindItemByValue(row["ProfileType"].ToString()) != null)
                                uxProfile.SelectedValue = GeneralFuncsLib.NvlString(row["ProfileType"]);
                            else
                                uxProfile.SelectedIndex = 0;
                        }
                    }

                    uxWatch.Checked = (string.Compare(GeneralFuncsLib.NvlString(row["Watch"]), "True") == 0);

                    uxMultiWatchList.Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.NvlString(row["MultiWatchList"]).Trim().TrimEnd(','));

                    uxAddress.Text = VeraCodeSolution.DoVeraCode(
                       MerchantProfileHelper.BindAddress(row["Address1"],
                                                        row["Address2"],
                                                        row["Address3"],
                                                        row["City"],
                                                        row["State"],
                                                        row["Zip"]));
                    if (SessionManager.CurrentClient == WebSiteConstants.AVIDIA_CLIENT && row[BACK_END_PROCESSOR].ToString().Equals(BackEndProcessor.REPAY, StringComparison.OrdinalIgnoreCase))
                    {
                        uxAddress.Text = VeraCodeSolution.DoVeraCode(
                           MerchantProfileHelper.BindAddress(row["Address1"],
                                                            row["Address2"],
                                                            row["Address3"],
                                                            row["Corp_City"],
                                                            row["Corp_state"],
                                                            row["Corp_Zip"]));
                    }

                    uxPhone.Text = (GeneralFuncsLib.NvlString(row["Phone"]).Length == 0 ?
                        WebSiteConstants.HTML_EM_DASH : VeraCodeSolution.ValidateResponseData(AS.Common.Formater.FormatData.FormatPhoneNumber(row["Phone"].ToString())));

                    // TK25937 – FIS - Merchant Profile/ Risk Report Enhancement 
                    uxServicedByLabel.Visible = uxServicedByValue.Visible = GeneralFuncsLib.HasServicedByFieldInRskRpt();
                    // Hide Hierarchy3
                    uxHierarchy3.Visible = uxHierarchyValue3.Visible = !uxServicedByLabel.Visible;
                    if (uxServicedByLabel.Visible)
                    {
                        uxServicedByLabel.Text = GetLocalResourceObject("RiskReportMerchantInformation_ascx_cs_ServicedBy").ToString();
                        uxServicedByValue.Text = ShowData(row["ServicedBy"]);
                    }

                    // Field Email
                    pnlEmail.Visible = GeneralFuncsLib.HasEmailFieldInRskRpt();
                    if (pnlEmail.Visible)
                    {
                        uxEmail.Text = ShowData(row["Email"]);
                    }

                    pnlRelationshipManager.Visible = isShowRelationshipManager;

                    if (isShowRelationshipManager && info.Columns.Contains("RelationShipManager"))
                    {
                        uxRelationshipManager.Text = ShowValidateResponseData(row["RelationshipManager"]);
                    }

                    uxWKD30.Text = ShowValidateResponseData(row["TotalOfWorked"]);

                    OnDataBindControls(DataBindAction.BindMerchantHierarchy);

                    if (GeneralFuncsLib.EnabledTSYSConversion())
                    {
                        uxPlhSwitchMID.Visible = true;
                        string proccessor = row[BACK_END_PROCESSOR].ToString();
                        BindSwitchMID(this.MerchantNumber, proccessor);
                    }
                    else
                    {
                        uxPlhSwitchMID.Visible = false;
                    }

                    //has referrer, has referred merchant, and merchant is the same
                    if (RiskSessionManager.RiskReportReferrer.Length > 0 && RiskSessionManager.RiskReportReferrerInfo.Key.Length > 0
                        && string.Compare(RiskSessionManager.RiskReportReferrerInfo.Key, this.MerchantNumber) == 0
                        && RiskSessionManager.RiskReportReferrerInfo.Url.Length > 0)
                    {
                        var url = string.Format("/risk_MCF/{0}", RiskSessionManager.RiskReportReferrerInfo.Url);
                        uxGoBack.NavigateUrl = VeraCodeSolution.ValidateResponseData(url);
                        uxGoBack.Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("rm_RiskReport_aspx_cs_BackTo").ToString() + " " + RiskSessionManager.RiskReportReferrerInfo.Title);
                        uxGoBack.Visible = true;
                    }
                    else
                    {
                        uxGoBack.Visible = false;
                        RiskSessionManager.RiskReportReferrer = null;
                        RiskSessionManager.RiskReportReferrerInfo = null;
                    }

                    string merchantNumber = VeraCodeSolution.ValidateResponseData(this.MerchantNumber);
                    string buildMerchantNumberLink = BuildMerchantNumberLink(merchantNumber);
                    string buildDetectionQueueLink = BuildDetectionQueueLink(merchantNumber);
                    uxMerchantName.Text = VeraCodeSolution.DoVeraCode(string.Format("{0}: {1} {2}", buildMerchantNumberLink, uxMerchantName.Text, buildDetectionQueueLink));

                    uxWorked.Text = ConvertData(GeneralFuncsLib.NvlString(row["Worked"]));
                    uxParameterWorked.Text = ConvertData(GeneralFuncsLib.NvlString(row["ParametersWorked"]));

                    uxActualDeliveryDays.Text = ConvertData(GeneralFuncsLib.NvlString(row["ActualDeliveryDays"]));
                    lblDaystoFund.Text = ConvertData(GeneralFuncsLib.NvlString(row["DaystoFund"]));
                    lblFICOScore.Text = ConvertData(GeneralFuncsLib.NvlString(row["CreditScore"]));
                    lblFutureDeliveryIndicator.Text = ConvertData(GeneralFuncsLib.NvlString(row["FutureDeliveryIndicator"]));
                    lblFutureDeliveryDayMaximum.Text = ConvertData(GeneralFuncsLib.NvlString(row["FutureDeliveryDayMaximum"]));
                    lblSeasonalIndicator.Text = ConvertData(GeneralFuncsLib.NvlString(row["SeasonalIndicator"]));
                    lblSeasonalActiveMonths.Text = ConvertData(GeneralFuncsLib.NvlString(row["SeasonalActiveMonths"]));
                    if (SessionManager.CurrentClient == WebSiteConstants.ESQR_CLIENT)
                    {
                        uxStatusDate.Text = ShowValidateResponseData(GeneralFuncsLib.FormatDate(row["StatusDate"]));
                        uxRiskLevelMI.Text = ShowData(row["RiskLevel"]);
                        uxDescGoodService.Text = ShowData(row["DescriptionOfGoodsOrServices"]);
                        uxSubMCC.Text = ShowData(row["SubMCC"]);
                        uxAdvDeposit.Text = FormatTemplateHelper.FormatPercent(row["AdvDepositPercentage"]);
                        uxStatusName.Text = ShowData(row["StatusName"]);
                        uxExpeditedFunding.Text = ShowData(row["ExpeditedFunding"]);
                        uxAdvDepositsDays.Text = ShowData(row["AdvDepositDays"]);
                        uxApprovedNDXDays.Text = ShowData(row["ApprovedNDXDays"]);

                        uxReserveIndicator.Text = ShowData(row["ReserveIndicator"]);
                        uxReserveTarget.Text = FormatTemplateHelper.FormatCurrency(row["ReserveTarget"]);
                        uxReservePercent.Text = FormatTemplateHelper.FormatPercent(row["ReservePercentage"]);
                        uxEsquireDirectAcct.Text = ShowData(row["EsquireDirectAcct"]);
                        uxAgentName.Text = ShowData(row["AgentName"]);
                        uxHighRiskRegistration.Text = ShowData(row["HighRiskRegistration"]);
                        uxNegativeDatabase.Text = ShowData(row["NegativeDatabase"]);
                        uxApprovedMCC.Text = ShowData(row["ApprovedMCC"]);
                    }
                    if (SessionManager.CurrentClient == WebSiteConstants.WFNB_CLIENT)
                    {
                        uxRiskRating.Text = ShowValidateResponseData(row["RiskRating"]);
                    }

                    if (GetIsShowOriginallyContractValuesInConfig())
                    {
                        uxOriginallyBoardedAnnualVolume.Text = AS.Web.Business.Risk.RM_MCF_GeneralFuncsLib.FormatCurrencyText(row["OriginallyBoardedAnnualVolume"], SessionManager.CurrencyFortmat);
                        uxOriginallyBoardedAverageTicket.Text = AS.Web.Business.Risk.RM_MCF_GeneralFuncsLib.FormatCurrencyText(row["OriginallyBoardedAverageTicket"], SessionManager.CurrencyFortmat);
                        uxOriginallyBoardedHighTicket.Text = AS.Web.Business.Risk.RM_MCF_GeneralFuncsLib.FormatCurrencyText(row["OriginallyBoardedHighTicket"], SessionManager.CurrencyFortmat);
                    }

                    lblDaystoFundStandard.Text = ConvertData(GeneralFuncsLib.NvlString(row["DaystoFund"]));
                    lblFICOScoreStandard.Text = ConvertData(GeneralFuncsLib.NvlString(row["CreditScore"]));
                    lblSeasonalIndicatorStandard.Text = ConvertData(GeneralFuncsLib.NvlString(row["SeasonalIndicator"]));
                    lblRiskRatingStandard.Text = ShowValidateResponseData(row["RiskRating"]);
                }
                break;
            case DataBindAction.BindMerchantHierarchy:
                {
                    DataTable info = null;
                    DataTable bindHierarchy = null;
                    if (DataSources.IsNotNullData())
                    {
                        var data = DataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindMerchantHierarchy.ToString());
                        if (data.IsNotNullData())
                            bindHierarchy = data.DataSource;
                    }

                    if (bindHierarchy.IsNotNullData())
                    {
                        info = bindHierarchy;
                    }
                    else
                    {
                        parameters.Clear();
                        parameters.AddLoggedInUserReportingParams(false);
                        parameters.AddLanguageID();
                        parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                        info = WebServices.RiskServices.GetReports(SPA_GET_MERCHANT_HIERARCHY_FOR_RISK, parameters);
                    }
                    ResetMerchantHierarchy();
                    if (info == null || info.Rows.Count == 0)
                        return;

                    if (info.Rows[0].Table.Columns.Contains("Name0"))
                    {
                        uxHierarchy1Item.Visible = true;
                        uxHierarchy1.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name0"].ToString() + ":");
                        uxHierarchyValue1.Text = ShowData(info.Rows[0]["Value0"]);
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name1"))
                    {
                        uxHierarchy2Item.Visible = true;
                        uxHierarchy2.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name1"].ToString() + ":");
                        uxHierarchyValue2.Text = ShowData(info.Rows[0]["Value1"]);
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name2"))
                    {
                        uxHierarchy3Item.Visible = true;
                        uxHierarchy3.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name2"].ToString() + ":");
                        uxHierarchyValue3.Text = ShowData(info.Rows[0]["Value2"]);
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name3"))
                    {
                        uxHierarchy4Item.Visible = true;
                        uxHierarchy4.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name3"].ToString() + ":");
                        uxHierarchyValue4.Text = ShowData(info.Rows[0]["Value3"]);
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name4"))
                    {
                        uxHierarchy5Item.Visible = true;
                        uxHierarchy5.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name4"].ToString() + ":");
                        uxHierarchyValue5.Text = ShowData(info.Rows[0]["Value4"]);
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name5"))
                    {
                        uxHierarchy6Item.Visible = true;
                        uxHierarchy6.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name5"].ToString() + ":");
                        uxHierarchyValue6.Text = ShowData(info.Rows[0]["Value5"]);
                    }
                }
                break;
            case DataBindAction.BindMerchantClassfisication:
                {
                    if (string.IsNullOrEmpty(this.MerchantNumber))
                        break;

                    DataTable bindClassfisication = null;
                    if (DataSources != null)
                    {
                        var data = DataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindMerchantClassfisication.ToString());
                        if (data.IsNotNullData())
                            bindClassfisication = data.DataSource;
                    }

                    if (bindClassfisication.IsNotNullData())
                    {
                        _RiskScoreInfor = bindClassfisication;
                    }
                    else
                    {
                        parameters.Clear();
                        parameters.AddLoggedInUserParamsWithRecId();
                        parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                        _RiskScoreInfor = WebServices.RiskServices.GetReports(SPA_GET_MIF_CLASSIFICATION, parameters);
                    }

                    uxTextTotalRS.Visible = true;
                    uxLinkTotalRS.Visible = false;
                    uxTextTotalRS.Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
                    if (_RiskScoreInfor != null && _RiskScoreInfor.Rows.Count > 0)
                    {
                        DataRow dr = _RiskScoreInfor.Rows[0];
                        if (!GeneralFuncsLib.NvlString(dr["TotalRS"]).Equals("0") && dr["TotalRS"] != DBNull.Value
                         && !dr["TotalRS"].ToString().IsNullOrEmpty())
                        {
                            uxTextTotalRS.Visible = false;
                            uxLinkTotalRS.Visible = true;
                            string queryString = this.Page.BuildSecureQueryString("MerchantNumber=" + this.MerchantNumber + "&ReportDate=" + dr["ReportDate"]);
                            string urlRiskScoreDetail = "rm_MCF_RiskScoreDetailModal.aspx?" + queryString;
                            var eventClick = "ShowPopupModal('" + urlRiskScoreDetail + "','auto'); return false;";

                            lnkTotalRS.Attributes.Add("onclick", eventClick);
                        }
                    }
                }
                break;
            case DataBindAction.BindDataElement:
                {
                    if (SessionManager.CurrentClient == WebSiteConstants.MVRK_CLIENT)
                    {
                        var dataElement = GetMVRKDataElement();
                        if (dataElement != null && dataElement.Rows.Count > 0)
                        {
                            DataRow row = dataElement.Rows[0];
                            reserveVal.Text = ShowData(row["ReservePercentage"], string.Empty, "%");
                            reserveDollarAmountVal.Text = ShowData(row["ReserveBalance"], "$", string.Empty);
                            RDRVal.Text = ShowData(row["RDRResolutions"]);
                            AppVal.Text = ShowData(row["AppID"]);
                            LegalVal.Text = ShowData(row["LegalName"]);
                            ProductorServiceVal.Text = ShowData(row["ProductOrService"]);
                            salesVal.Text = ShowData(row["SalesMethod"]);
                            B2BVal.Text = ShowData(row["CustomerProfile"]);
                            GovernmentVal.Text = ShowData(row["CustomerLocation"]);
                        }
                    }
                    break;
                }
        }
    }
    protected void uxSelected_OnClick(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        OnPostBackActions(PostBackAction.SaveMerchantInfoEvent);
    }

    protected void uxChecked_OnClick(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SaveMerchantInfoEvent);
    }

    protected string ShowData(object value)
    {
        return (value == DBNull.Value || value.IsNullOrEmpty()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : VeraCodeSolution.DoVeraCode(value.ToString());
    }

    protected string ShowValidateResponseData(object value)
    {
        return (value == DBNull.Value || value.IsNullOrEmpty()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : VeraCodeSolution.ValidateResponseData(value.ToString());
    }

    public void GetData()
    {
        OnDataBindControls(DataBindAction.BindMerchantInformation);
        OnDataBindControls(DataBindAction.BindMerchantClassfisication);
        OnDataBindControls(DataBindAction.BindDataElement);
    }

    private void ResetMerchantHierarchy()
    {
        uxHierarchy1.Text = string.Empty;
        uxHierarchyValue1.Text = string.Empty;
        uxHierarchy2.Text = string.Empty;
        uxHierarchyValue2.Text = string.Empty;
        uxHierarchy3.Text = string.Empty;
        uxHierarchyValue3.Text = string.Empty;
        uxHierarchy4.Text = string.Empty;
        uxHierarchyValue4.Text = string.Empty;
        uxHierarchy5.Text = string.Empty;
        uxHierarchyValue5.Text = string.Empty;
        uxHierarchy6.Text = string.Empty;
        uxHierarchyValue6.Text = string.Empty;
    }

    private void BindSwitchMID(string merchantNumber, string proccessor)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.AnsiString));
        parameters.Add(new FilterParameter("@Processor", proccessor, DbType.String));

        DataTable data = WebServices.CsReportServices.GetReports("spa_cs_CheckIsSwitchByMerchant", parameters);
        if (data != null && data.Rows.Count > 0 && (bool)data.Rows[0][IS_SWITCH])
        {
            uxPlhSwitchMID.Visible = true;
            string mid = "";
            if (proccessor.Equals(BackEndProcessor.FDR, StringComparison.OrdinalIgnoreCase))
            {
                uxSwitchMID.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("rm_RiskReport_aspx_cs_SwitchToTSYS").ToString());
                mid = data.Rows[0][TSYS_MID].ToString();
            }
            else
            {
                uxSwitchMID.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("rm_RiskReport_aspx_cs_SwitchToFDR").ToString());
                mid = data.Rows[0][FDR_MID].ToString();
            }
            uxSwitchMID.NavigateUrl = "";
            uxSwitchMID.Attributes.Add("onclick", string.Format("return switchMID('{0}');", mid));
            uxSwitchMID.Style.Add("cursor", "pointer");
        }
        else
        {
            uxPlhSwitchMID.Visible = false;
        }
    }

    private string ConvertData(string data)
    {
        return string.IsNullOrEmpty(data) ? WebSiteConstants.HTML_EM_DASH_ENCODE : data;
    }

    private string ShowData(object value, string prefix, string suffix)
    {
        return (value == DBNull.Value || value.IsNullOrEmpty()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : prefix + VeraCodeSolution.DoVeraCode(value.ToString()) + suffix;
    }

    private bool GetIsShowOriginallyContractValuesInConfig()
    {
        return BuGeneralFuncsLib.CheckExistsInSplitToArray(WebSiteSettings.RiskReportShowOriginallyContractValues, ',', SessionManager.CurrentClient.ToString());
    }
    #region Enhance Performance
    private FilterParameterCollection GetMerchantInfoParameters(DataBindAction action)
    {
        //DataBindAction actiontemp = (DataBindAction)action; 
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        switch (action)
        {
            case DataBindAction.BindGetMerchantInfo:
                {
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    parameters.AddLanguageID();
                    //return WebServices.RiskServices.GetReports(SPA_GET_MERCHANT_INFO, parameters);
                    break;
                }
            case DataBindAction.BindMerchantHierarchy:
                {
                    parameters.AddLanguageID();
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    //DataTable info = WebServices.RiskServices.GetReports(SPA_GET_MERCHANT_HIERARCHY_FOR_RISK, parameters);
                    break;
                }
            case DataBindAction.BindMerchantClassfisication:
                {
                    parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                    //_RiskScoreInfor = WebServices.RiskServices.GetReports(SPA_GET_MIF_CLASSIFICATION, parameters);
                    break;
                }
            case DataBindAction.BindProfileList:
                {

                    // WebServices.RiskServices.GetReports(SPA_PROFILE_LIST, parameters)
                    break;
                }
        }
        return parameters;
    }
    public void BindDataFromThread(List<DataSourceParallelResponse> dataSources)
    {
        DataSources = dataSources;
        DataTable binData = null;
        if (dataSources.IsNotNullData())
        {
            var data = DataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindGetMerchantInfo.ToString());
            if (data.IsNotNullData())
                binData = data.DataSource;
        }

        RiskSessionManager.RiskReportMerchantInfo = binData.IsNotNullData() ? binData : GetMerchantInfo();

        GetData();
    }

    private string BuildDetectionQueueLink(string merchantNumber)
    {
        string templateHtml = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\"openPopupWindowOnMenu(this,'" + RiskReportConstants.KEY_SECURE_PARAM_HTML + "','DQMCFWindow2'); return false;\">" +
           "<img src='" + ResolveUrl("~/") + "res/img/information.png' border='0' alt='[TextResourceValue]' />" +
           "</a>";
        var lstData = new List<BuildToHtmlModel>()
        {
            new BuildToHtmlModel{ Key = "MerchantNumber",Value = merchantNumber  },
            new BuildToHtmlModel{ Key ="ReportDate", Value =  DateTime.Now.ToString()  },
            new BuildToHtmlModel{ Key ="TextResourceValue", Value =  RM_MCF_GeneralFuncsLib.GetResourceValue("DetectionQueueRainbowReport_ascx_PV").ToString(), EnumToHtml  = EnumToHtml.Template  }
        };
        var rsp = GeneralFuncsLib.BuildToHtmlWithSecurePage(lstData, templateHtml, EnumPage.RM_MCF_DQReasonModal, this.Page);

        return rsp.Item1;
    }
    private string BuildMerchantNumberLink(string merchantNumber)
    {
        string templateHtml = "<a class=\"risk-merchant-detail\" href=\"#\" actionModal ='GetMerchantProfile' onclick=\"OpenDetailModal(this);\">[MerchantNumber]</a>";
        var lstData = new List<BuildToHtmlModel>()
        {
            new BuildToHtmlModel{ Key = "MerchantNumber" ,  Value = merchantNumber, EnumToHtml = EnumToHtml.All, Order = 1  },
            new BuildToHtmlModel{Key = "IsHideMenu", Value= "true", Order = 2 }
        };
        var rsp = GeneralFuncsLib.BuildToHtmlWithSecurePage(lstData, templateHtml, EnumPage.Redirect, this.Page);
        hdShowDetailModal.Value = rsp.Item2;

        return rsp.Item1;
    }
    protected void UxShowModalDetail_Click(object sender, EventArgs e)
    {
        string paramValue = hdShowDetailModal.Value;
        if (string.IsNullOrEmpty(paramValue))
        {
            var msgError = GetLocalResourceObject("UxShowModalDetailError");
            ((BaseMasterPage)Page.Master).AjaxAddResponseScript(string.Format("ShowMsg('{0}');", msgError));
            return;
        }

        EnumAction enumAction;
        Enum.TryParse(hdActionModal.Value, out enumAction);
        string funcScript = string.Empty;
        switch (enumAction)
        {
            case EnumAction.GetMerchantProfile:
                funcScript = string.Format("openPopupWindow('/MerchantProfile.aspx?{0}','auto'); hideLoading(); return false;", paramValue);
                break;
            default:
                break;
        }
        ((BaseMasterPage)Page.Master).AjaxAddResponseScript(funcScript);
    }
    #region SPA INFO
    public SpaInfo SpaGetMerchantInfo
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindGetMerchantInfo.ToString(),
                SpaName = SessionManager.CurrentClient == WebSiteConstants.ESQR_CLIENT ? SPA_GET_MERCHANT_INFO_V2 : SPA_GET_MERCHANT_INFO,
                Parameters = GetMerchantInfoParameters(DataBindAction.BindGetMerchantInfo)
            };
        }
    }

    public SpaInfo SpaGetMerchantHierarchyForRiskReport
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindMerchantHierarchy.ToString(),
                SpaName = SPA_GET_MERCHANT_HIERARCHY_FOR_RISK,
                Parameters = GetMerchantInfoParameters(DataBindAction.BindMerchantHierarchy)
            };
        }
    }

    public SpaInfo SpaGetMIFClassificationByMerchant
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindMerchantClassfisication.ToString(),
                SpaName = SPA_GET_MIF_CLASSIFICATION,
                Parameters = GetMerchantInfoParameters(DataBindAction.BindMerchantClassfisication)
            };
        }
    }

    public SpaInfo SpaGetProfileList
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindProfileList.ToString(),
                SpaName = SPA_PROFILE_LIST,
                Parameters = GetMerchantInfoParameters(DataBindAction.BindProfileList)
            };
        }
    }

    #endregion
    #endregion
}
