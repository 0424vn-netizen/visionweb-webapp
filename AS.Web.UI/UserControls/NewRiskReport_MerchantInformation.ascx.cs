using AS.Common;
using AS.Common.DBManager;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Controls.Pages;
using System.Text;


public partial class UserControls_NewRiskReportMerchantInformation : GlobalUserControl
{

    enum DataBindAction
    {
        BindMerchantInformation,
        BindMerchantHierarchy,
        BindMerchantClassfisication
    }

    enum PostBackAction
    {
        SaveMerchantInfoEvent
    }

    #region properties
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string BACK_END_PROCESSOR = "BackEndProcessor";

    public string MerchantNumber
    {
        get
        {
            return GeneralFuncsLib.NvlString(ViewState[MERCHANT_NUMBER]);
        }
        set
        {
            ViewState[MERCHANT_NUMBER] = value;
            if (value != string.Empty)
            {

                RiskSessionManager.RiskReportMerchantInfo = GetMerchantInfo();
            }
            else
            {
                RiskSessionManager.RiskReportMerchantInfo = null;
            }
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
        return WebServices.RiskServices.GetReports("spa_rm_cs_RiskReport_GetMerchantInfo", parameters);
    }

    protected void Page_Load(object sender, EventArgs e)
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
            GetData();
        }

        //45923 - SIGNA- VisionWeb Supplemental MIF
        if (SessionManager.CurrentClient == WebSiteConstants.SIGNAPAY_CLIENT)
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format("showSupplementalSIGNAPAY()"));
        //End


        if (GeneralFuncsLib.GetDataOfExtendedSetting("EnableAuthWorkedCheckBox").ToLower().Equals("true"))
        {
            RestyleMerchantInformationGrid();
        }

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
                    WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_SaveRiskReportMerchant", parameters, out outValue);
                    RiskSessionManager.RiskReportMerchantInfo = GetMerchantInfo();
                    GetData();
                    // Save UserActivity
                    string activityText = string.Format(GetLocalResourceObject("RiskReportMerchantInformation_ascx_cs_RiskReportProfileWatch").ToString(), this.MerchantNumber, profileType, uxWatch.Checked ? GetLocalResourceObject("RiskReportMerchantInformation_ascx_cs_Check").ToString() : GetLocalResourceObject("RiskReportMerchantInformation_ascx_cs_UnCheck").ToString());
                    GeneralFuncsLib.SaveUserActivity(this.MerchantNumber, activityText);
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

                    parameters.Clear();
                    parameters.AddLoggedInUserReportingParams(false);
                    DataTable profileList = WebServices.RiskServices.GetReports("spa_rm_cs_GetProfileList", parameters);
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

                    if (GeneralFuncsLib.GetDataOfExtendedSetting("EnableAuthWorkedCheckBox").ToLower().Equals("true"))
                    {
                        uxWorked.Checked = (string.Compare(GeneralFuncsLib.NvlString(row["IsCheckWork"]), "True") == 0);
                        uxAuthMonitorExclude.Checked = (string.Compare(GeneralFuncsLib.NvlString(row["ISCheckAuthMonitor"]), "True") == 0);
                    }

                    uxMultiWatchList.Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.NvlString(row["MultiWatchList"]).Trim().TrimEnd(','));

                    uxAddress.Text = VeraCodeSolution.DoVeraCode(
                       MerchantProfileHelper.BindAddress(row["Address1"],
                                                        row["Address2"],
                                                        row["Address3"],
                                                        row["City"],
                                                        row["State"],
                                                        row["Zip"]));
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
                    //uxWatch.Enabled = uxProfile.Enabled = uxSave.Visible = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
                }
                break;
            case DataBindAction.BindMerchantHierarchy:
                {
                    parameters.Clear();
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.AddLanguageID();
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    DataTable info = WebServices.RiskServices.GetReports("spa_rm_cs_GetMerchantHierarchyForRiskReport", parameters);
                    if (info == null || info.Rows.Count == 0)
                        return;
                    ResetMerchantHierarchy();
                    if (info.Rows[0].Table.Columns.Contains("Name0"))
                    {
                        uxHierarchy1.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name0"].ToString() + ":");
                        uxHierarchyValue1.Text = ShowData(info.Rows[0]["Value0"]);
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name1"))
                    {
                        uxHierarchy2.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name1"].ToString() + ":");
                        uxHierarchyValue2.Text = ShowData(info.Rows[0]["Value1"]);
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name2"))
                    {
                        uxHierarchy3.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name2"].ToString() + ":");
                        uxHierarchyValue3.Text = ShowData(info.Rows[0]["Value2"]);
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name3"))
                    {
                        uxHierarchy4.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name3"].ToString() + ":");
                        uxHierarchyValue4.Text = ShowData(info.Rows[0]["Value3"]);
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name4"))
                    {
                        uxHierarchy5.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name4"].ToString() + ":");
                        uxHierarchyValue5.Text = ShowData(info.Rows[0]["Value4"]);
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name5"))
                    {
                        uxHierarchy6.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name5"].ToString() + ":");
                        uxHierarchyValue6.Text = ShowData(info.Rows[0]["Value5"]);
                    }
                }
                break;
            case DataBindAction.BindMerchantClassfisication:
                {
                    parameters.Clear();
                    parameters.AddLoggedInUserParamsWithRecId();
                    parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                    _RiskScoreInfor = WebServices.RiskServices.GetReports("spa_RM_MRS_Get_MIFClassificationByMerchant", parameters);

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
                            string urlRiskScoreDetail = "rm_RiskScoreDetailModal.aspx?" + queryString;
                            var eventClick = "ShowPopupModal('" + urlRiskScoreDetail + "','auto'); return false;";

                            lnkTotalRS.Attributes.Add("onclick", eventClick);
                        }
                    }
                }
                break;
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

    private void RestyleMerchantInformationGrid()
    {
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("RestyleMerchantInformationGrid();");
    }
    protected void uxAuthMonitorExclude_CheckedChanged(object sender, EventArgs e)
    {
        FilterParameterCollection parameterList = new FilterParameterCollection();
        FilterParameterCollection outParameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.AnsiString));
        parameterList.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.AnsiString));
        parameterList.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString));
        parameterList.Add(new FilterParameter("@IsAuthMonitorExclude", uxAuthMonitorExclude.Checked, DbType.Boolean));

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_UpdateAuthMonitorExclude", parameterList, out outParameterList);
    }
    protected void uxWorked_CheckedChanged(object sender, EventArgs e)
    {
        //40706 - Show message when current merchant is worked
        if (uxWorked.Checked)
        {
            DataTable dt = GeneralFuncsLib.CheckMerchantIsWorked(DateTime.Today, this.MerchantNumber);
            if (dt.HasData())
            {
                if (dt.Rows[0]["IsWorked"].ToBoolean())
                {
                    ShowMessageBox(GeneralFuncsLib.GetMessageWorked(dt, GetLocalResourceObject("msgMerchantWorked").ToString()));
                    return;
                }
            }
        }
        FilterParameterCollection parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        WebServices.RiskServices.UpdateMerchantWorked(parameterList, DateTime.Today, this.MerchantNumber, uxWorked.Checked, null, WebSiteEnums.PAGE_CODE.RP.ToString());
    }

    public void ShowMessageBox(string message)
    {
        message = string.Format("alert('{0}')", message).ToString();
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowAlerMessage", message, true);
    }
}
