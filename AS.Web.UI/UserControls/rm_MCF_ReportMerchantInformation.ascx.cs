using AS.Common;
using AS.Common.DBManager;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Controls.Pages;


public partial class UserControls_rm_MCF_ReportMerchantInformation : GlobalUserControl
{

    enum DataBindAction
    {
        BindMerchantInformation,
        BindMerchantHierarchy
    }

    enum PostBackAction
    {
        SaveMerchantInfoEvent
    }

    #region properties
    private const string MERCHANT_NUMBER = "MerchantNumber";

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

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetData();
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
                    DataTable info = new DataTable();
                    parameters.Clear();
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    parameters.AddLanguageID();
                    info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetMerchantDescForRiskReport", parameters);
                    if (info.Rows.Count == 0)
                        return;

                    DataRow row = info.Rows[0];
                    string address1 = GeneralFuncsLib.NvlString(row["Address1"]);
                    string address2 = GeneralFuncsLib.NvlString(row["Address2"]);

                    parameters.Clear();
                    parameters.AddLoggedInUserReportingParams(false);
                    DataTable profileList = WebServices.RiskServices.GetReports("spa_RM_MCF_GetProfileList", parameters);
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
                    uxMerchantName.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.NvlString(row["MerchantName"]));
                    uxApprovalDate.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatDate(row["ApprovalDate"]));
                    uxLastActiveDate.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatDate(row["LastActiveDate"]));

                    uxOwner.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.NvlString(row["Owner"]));
                    uxLastStatementDate.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatDate(row["LastStatementDate"]));

                    uxStatus.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.NvlString(row["Status"]));
                    uxSIC.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.NvlString(row["SIC"]));

                    if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_MARKET_DATA").Equals("true", StringComparison.OrdinalIgnoreCase) && info.Columns.Contains("MDDescription"))
                    {
                        pnlMarketData.Visible = true;
                        uxMarketData.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.NvlString(row["MDDescription"]));
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

                    uxAddress1.Text = VeraCodeSolution.ValidateResponseData(address1);
                    uxAddress2.Text = VeraCodeSolution.ValidateResponseData(address2);

                    uxPhone.Text = (GeneralFuncsLib.NvlString(row["Phone"]).Length == 0 ?
                        string.Empty : VeraCodeSolution.ValidateResponseData(AS.Common.Formater.FormatData.FormatPhoneNumber(row["Phone"].ToString())));
                    uxFax.Text = (GeneralFuncsLib.NvlString(row["Fax"]).Length == 0 ?
                        string.Empty : VeraCodeSolution.ValidateResponseData(AS.Common.Formater.FormatData.FormatPhoneNumber(row["Fax"].ToString())));
                    
                    // TK25937 – FIS - Merchant Profile/ Risk Report Enhancement 
                    uxServicedByLabel.Visible = uxServicedByValue.Visible
                        = GeneralFuncsLib.HasServicedByFieldInRskRpt();
                    // Hide Hierarchy3
                    uxHierarchy3.Visible = uxHierarchyValue3.Visible = !uxServicedByLabel.Visible;
                    if (uxServicedByLabel.Visible)
                    {
                        uxServicedByLabel.Text = GetLocalResourceObject("RiskReportMerchantInformation_ascx_cs_ServicedBy").ToString();
                        uxServicedByValue.Text = GeneralFuncsLib.NvlString(row["ServicedBy"]);
                    }

                    // Field Email
                    pnlEmail.Visible = GeneralFuncsLib.HasEmailFieldInRskRpt();
                    if (pnlEmail.Visible)
                    {
                        uxEmail.Text = GeneralFuncsLib.NvlString(row["Email"]);
                    }

                    pnlRelationshipManager.Visible = isShowRelationshipManager;

                    if (isShowRelationshipManager && info.Columns.Contains("RelationShipManager"))
                    {
                        uxRelationshipManager.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.NvlString(row["RelationshipManager"]));
                    }


                    OnDataBindControls(DataBindAction.BindMerchantHierarchy);
                    uxWatch.Enabled = uxProfile.Enabled = uxSave.Visible = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
                }
                break;
            case DataBindAction.BindMerchantHierarchy:
                {
                    parameters.Clear();
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.AddLanguageID();
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    DataTable info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetMerchantHierarchyForRiskReport", parameters);
                    if (info == null || info.Rows.Count == 0)
                        return;
                    ResetMerchantHierarchy();
                    if (info.Rows[0].Table.Columns.Contains("Name0"))
                    {
                        uxHierarchy1.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name0"].ToString() + ":");
                        uxHierarchyValue1.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Value0"].ToString());
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name1"))
                    {
                        uxHierarchy2.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name1"].ToString() + ":");
                        uxHierarchyValue2.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Value1"].ToString());
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name2"))
                    {
                        uxHierarchy3.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name2"].ToString() + ":");
                        uxHierarchyValue3.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Value2"].ToString());
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name3"))
                    {
                        uxHierarchy4.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name3"].ToString() + ":");
                        uxHierarchyValue4.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Value3"].ToString());
                    }
                    if (info.Rows[0].Table.Columns.Contains("Name4"))
                    {
                        uxHierarchy5.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Name4"].ToString() + ":");
                        uxHierarchyValue5.Text = VeraCodeSolution.DoVeraCode(info.Rows[0]["Value4"].ToString());
                    }
                }
                break;
        }
    }
    protected void uxSave_OnClick(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SaveMerchantInfoEvent);
    }

    public void GetData()
    {
        OnDataBindControls(DataBindAction.BindMerchantInformation);
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
    }

}
