using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Controls.Global;
using System.Collections.Generic;
using AS.Common.DBManager;
using Org.BouncyCastle.Utilities;
using Telerik.Web.UI.com.hisoftware.api2;

public partial class UserControls_rm_MCF_Assignment_Filter_Custom : GlobalUserControl, IRiskParamFilter
{

    #region Enums
    enum DataBindAction
    {
        LoadCustomFilter,
        GetRiskLevelSelected,
        GetRiskCategorySelected,
        GetCampaignIDSelected
    }

    public Label LabelRiskLevel
    {
        get
        {
            return lblRiskLevel;
        }
    }

    protected int MaxLengthViewMore
    {
        get
        {
            return Convert.ToInt32(WebSiteSettings.RiskViewMore);
        }
    }

    #endregion


    #region Constants

    /// <summary>
    /// spa_RM_MCF_Update_AssignmentFilter_Maverick
    /// </summary>
    private const string SPA_UPDATE_ASSIGNMENT_FILTER = "spa_RM_MCF_SaveElementFilter_Maverick";
    /// <summary>
    /// spa_RM_MCF_Get_AssignmentFilter_Maverick
    /// </summary>
    private const string SPA_GET_ASSIGNMENT_FILTER = "spa_RM_MCF_GetElementFilter_Maverick";
    private string RiskLevel = "RiskLevel";
    private string RiskCategory = "RiskCategory";
    private string CampaignID = "CampaignID";

    #endregion Constants

    public WebSiteEnums.FeatureMode FeatureMode { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentClient != WebSiteConstants.MVRK_CLIENT) return;

        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.LoadCustomFilter);
            SetLabels();
            VisibleControls();
            InitValidation();
            GetAssignmentFilter();
        }
    }

    private void SetLabels()
    {
        SetRiskLevelInfo();

        SetRiskCategoryInfo();

        SetCampaignIDInfo();
    }

    private string BuildQueryStringForViewMoreModal(ViewMoreModalType modalType)
    {
        return Page.BuildSecureQueryString(string.Format(
                "primaryid={0}&mode={1}&typemodal={2}",
                PrimaryID,
                (int)WebSiteEnums.ParamFilterMode.Assignment,
                ((int)modalType).ToString())
            );        
    }

    private string BuildLabel(string data, ViewMoreModalType modalType)
    {
        string localQueryString = string.Empty;
        var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
        if ((temp.Length > 1 ? temp[1].Trim() : data).Length > MaxLengthViewMore)
        {
            localQueryString = BuildQueryStringForViewMoreModal(modalType);
            if (temp.Length > 1) { 
            data = temp[0].Trim() + "<br />" + temp[1].Trim().Substring(0, MaxLengthViewMore)
                + string.Format(
                    "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?{0}', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_Filters_ascx_cs_ViewMore").ToString() + "</a>",
                    localQueryString);
            } else
            {
                data = data.Substring(0, MaxLengthViewMore)
                + string.Format(
                    "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?{0}', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_Filters_ascx_cs_ViewMore").ToString() + "</a>",
                    localQueryString);
            }
        }
        return data;
    }

    private void SetRiskLevelInfo()
    {
        Dictionary<string, string> result = UserControls_rm_MCF_FilterRiskLevel.GetSelectedValuesAsString(Mode, PrimaryID, null, null, RiskLevel);

        string data = result["Label"];
        lblRiskLevel.Attributes.Add("tracking-value", result["Tracking"]);
        var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);

        if ((temp.Length > 1 ? temp[1].Trim() : data).Length > MaxLengthViewMore)
        {
            lblRiskLevel.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lblRiskLevel.Attributes.Remove("tracking-more");
        }
        lblRiskLevel.Text = BuildLabel(data, ViewMoreModalType.LoadRiskLevel);
    }

    private void SetRiskCategoryInfo()
    {
        Dictionary<string, string> result = UserControls_rm_MCF_FilterRiskCategory.GetSelectedValuesAsString(Mode, PrimaryID, null, null, RiskCategory);

        string data = result["Label"];
        lblRiskCategory.Attributes.Add("tracking-value", result["Tracking"]);

        var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
        if ((temp.Length > 1 ? temp[1].Trim() : data).Length > MaxLengthViewMore)
        {
            lblRiskCategory.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lblRiskCategory.Attributes.Remove("tracking-more");
        }
        lblRiskCategory.Text = BuildLabel(data, ViewMoreModalType.LoadRiskCategory);
    }

    private void SetCampaignIDInfo()
    {
        Dictionary<string, string> result = UserControls_rm_MCF_FilterCampaignID.GetSelectedValuesAsString(Mode, PrimaryID, null, null, CampaignID);

        string data = result["Label"];
        lblCampaignID.Attributes.Add("tracking-value", result["Tracking"]);

        var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
        if ((temp.Length > 1 ? temp[1].Trim() : data).Length > MaxLengthViewMore)
        {
            lblCampaignID.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lblCampaignID.Attributes.Remove("tracking-more");
        }
        lblCampaignID.Text = BuildLabel(data, ViewMoreModalType.LoadCampaignID);
    }

    public void VisibleControls()
    {
        bool isEditMode = FeatureMode == WebSiteEnums.FeatureMode.Edit;
        plhRiskLevel.Visible = isEditMode;
        plhRiskCategory.Visible = isEditMode;
        plhCampaignID.Visible = isEditMode;
    }

    protected void uxRebindRiskLevel_Click(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.GetRiskLevelSelected);
    }

    protected void uxRebindCampaignID_Click(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.GetCampaignIDSelected);
    }

    protected void uxRebindRiskCategory_Click(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.GetRiskCategorySelected);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (SessionManager.CurrentClient != WebSiteConstants.MVRK_CLIENT) return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadCustomFilter:
                VisibleControls();
                break;
            case DataBindAction.GetRiskLevelSelected:
                SetRiskLevelInfo();
                break;
            case DataBindAction.GetRiskCategorySelected:
                SetRiskCategoryInfo();
                break;
            case DataBindAction.GetCampaignIDSelected:
                SetCampaignIDInfo();
                break;
        }
    }

    private void setValueRadio(string value, AS.Controls.Global.RadioButton rdNA, AS.Controls.Global.RadioButton rdYes, AS.Controls.Global.RadioButton rdNo)
    {
        if (value == "Yes")
        {
            rdYes.Checked = true;
            rdNA.Checked = false;
            rdNo.Checked = false;
            return;
        }

        if (value == "No")
        {
            rdYes.Checked = false;
            rdNA.Checked = false;
            rdNo.Checked = true;
            return;
        }

        rdYes.Checked = false;
        rdNA.Checked = true;
        rdNo.Checked = false;
    }

    private string getValueRadio(AS.Controls.Global.RadioButton rdYes, AS.Controls.Global.RadioButton rdNo)
    {
        var value = "";
        if (rdYes.Checked)
        {
            value = "Yes";
        }

        if (rdNo.Checked)
        {
            value = "No";
        }

        return value;
    }

    private void GetAssignmentFilter()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(PrimaryID), DbType.Int32));
        DataTable dt = WebServices.RiskServices.GetReports(SPA_GET_ASSIGNMENT_FILTER, parameters);

        for(var i = 0; i < dt.Rows.Count; i++) {
            var row = dt.Rows[i];
            if (GeneralFuncsLib.NvlString(row["FilterName"]) == "AutoApproved")
            {
                var autoApproved = GeneralFuncsLib.NvlString(row["FilterValueString"]);
                var rdAutoApprovedIndicatorNA = uxAutoApprovedIndicator.FindControl("rdAutoApprovedIndicatorNA") as AS.Controls.Global.RadioButton;
                var rdAutoApprovedIndicatorYes = uxAutoApprovedIndicator.FindControl("rdAutoApprovedIndicatorYes") as AS.Controls.Global.RadioButton;
                var rdAutoApprovedIndicatorNo = uxAutoApprovedIndicator.FindControl("rdAutoApprovedIndicatorNo") as AS.Controls.Global.RadioButton;
                setValueRadio(autoApproved, rdAutoApprovedIndicatorNA, rdAutoApprovedIndicatorYes, rdAutoApprovedIndicatorNo);
            }
            
            if (GeneralFuncsLib.NvlString(row["FilterName"]) == "AgentSharedLiability")
            {
                var agentSharedLiability = GeneralFuncsLib.NvlString(row["FilterValueString"]);
                var rdAcqAgentSharedLiabilityNA = uxAcqAgentSharedLiability.FindControl("rdAcqAgentSharedLiabilityNA") as AS.Controls.Global.RadioButton;
                var rdAcqAgentSharedLiabilityYes = uxAcqAgentSharedLiability.FindControl("rdAcqAgentSharedLiabilityYes") as AS.Controls.Global.RadioButton;
                var rdAcqAgentSharedLiabilityNo = uxAcqAgentSharedLiability.FindControl("rdAcqAgentSharedLiabilityNo") as AS.Controls.Global.RadioButton;
                setValueRadio(agentSharedLiability, rdAcqAgentSharedLiabilityNA, rdAcqAgentSharedLiabilityYes, rdAcqAgentSharedLiabilityNo);
            }
                        
            if (GeneralFuncsLib.NvlString(row["FilterName"]) == "VisaRegistration")
            {
                var visaRegistration = GeneralFuncsLib.NvlString(row["FilterValueString"]);
                var rdVisaRegistrationNA = uxVisaRegistration.FindControl("rdVisaRegistrationNA") as AS.Controls.Global.RadioButton;
                var rdVisaRegistrationYes = uxVisaRegistration.FindControl("rdVisaRegistrationYes") as AS.Controls.Global.RadioButton;
                var rdVisaRegistrationNo = uxVisaRegistration.FindControl("rdVisaRegistrationNo") as AS.Controls.Global.RadioButton;
                setValueRadio(visaRegistration, rdVisaRegistrationNA, rdVisaRegistrationYes, rdVisaRegistrationNo);
            }

            if (GeneralFuncsLib.NvlString(row["FilterName"]) == "MCRegistration")
            {
                var mcRegistration = GeneralFuncsLib.NvlString(row["FilterValueString"]);
                var rdMCRegistrationNA = uxMCRegistration.FindControl("rdMCRegistrationNA") as AS.Controls.Global.RadioButton;
                var rdMCRegistrationYes = uxMCRegistration.FindControl("rdMCRegistrationYes") as AS.Controls.Global.RadioButton;
                var rdMCRegistrationNo = uxMCRegistration.FindControl("rdMCRegistrationNo") as AS.Controls.Global.RadioButton;
                setValueRadio(mcRegistration, rdMCRegistrationNA, rdMCRegistrationYes, rdMCRegistrationNo);
            }

            if (GeneralFuncsLib.NvlString(row["FilterName"]) == "DiscoverRegistration")
            {
                var discoverRegistration = GeneralFuncsLib.NvlString(row["FilterValueString"]);
                var rdDiscoverRegistrationNA = uxDiscoverRegistration.FindControl("rdDiscoverRegistrationNA") as AS.Controls.Global.RadioButton;
                var rdDiscoverRegistrationYes = uxDiscoverRegistration.FindControl("rdDiscoverRegistrationYes") as AS.Controls.Global.RadioButton;
                var rdDiscoverRegistrationNo = uxDiscoverRegistration.FindControl("rdDiscoverRegistrationNo") as AS.Controls.Global.RadioButton;
                setValueRadio(discoverRegistration, rdDiscoverRegistrationNA, rdDiscoverRegistrationYes, rdDiscoverRegistrationNo);
            }

            if (GeneralFuncsLib.NvlString(row["FilterName"]) == "AMEXRegistration")
            {
                var amexRegistration = GeneralFuncsLib.NvlString(row["FilterValueString"]);
                var rdAMEXRegistrationNA = uxAMEXRegistration.FindControl("rdAMEXRegistrationNA") as AS.Controls.Global.RadioButton;
                var rdAMEXRegistrationYes = uxAMEXRegistration.FindControl("rdAMEXRegistrationYes") as AS.Controls.Global.RadioButton;
                var rdAMEXRegistrationNo = uxAMEXRegistration.FindControl("rdAMEXRegistrationNo") as AS.Controls.Global.RadioButton;
                setValueRadio(amexRegistration, rdAMEXRegistrationNA, rdAMEXRegistrationYes, rdAMEXRegistrationNo);
            }

            if (GeneralFuncsLib.NvlString(row["FilterName"]) == "NonDeliveryExposure")
            {
                var highNDX = GeneralFuncsLib.NvlString(row["FilterValueString"]);
                var rdHighNDXNA = uxHighNDX.FindControl("rdHighNDXNA") as AS.Controls.Global.RadioButton;
                var rdHighNDXYes = uxHighNDX.FindControl("rdHighNDXYes") as AS.Controls.Global.RadioButton;
                var rdHighNDXNo = uxHighNDX.FindControl("rdHighNDXNo") as AS.Controls.Global.RadioButton;
                setValueRadio(highNDX, rdHighNDXNA, rdHighNDXYes, rdHighNDXNo);
            }

            if (GeneralFuncsLib.NvlString(row["FilterName"]) == "ElevatedDisputes")
            {
                var cbProducing = GeneralFuncsLib.NvlString(row["FilterValueString"]);
                var rdCBproducingNA = uxCBproducing.FindControl("rdCBproducingNA") as AS.Controls.Global.RadioButton;
                var rdCBproducingYes = uxCBproducing.FindControl("rdCBproducingYes") as AS.Controls.Global.RadioButton;
                var rdCBproducingNo = uxCBproducing.FindControl("rdCBproducingNo") as AS.Controls.Global.RadioButton;
                setValueRadio(cbProducing, rdCBproducingNA, rdCBproducingYes, rdCBproducingNo);
            }

            if (GeneralFuncsLib.NvlString(row["FilterName"]) == "EnhancedDueDiligence")
            {
                var complianceRegulatoryReputational = GeneralFuncsLib.NvlString(row["FilterValueString"]);
                var rdcomplianceRegulatoryReputationalNA = uxComplianceRegulatoryReputational.FindControl("rdcomplianceRegulatoryReputationalNA") as AS.Controls.Global.RadioButton;
                var rdcomplianceRegulatoryReputationalYes = uxComplianceRegulatoryReputational.FindControl("rdcomplianceRegulatoryReputationalYes") as AS.Controls.Global.RadioButton;
                var rdcomplianceRegulatoryReputationalNo = uxComplianceRegulatoryReputational.FindControl("rdcomplianceRegulatoryReputationalNo") as AS.Controls.Global.RadioButton;
                setValueRadio(complianceRegulatoryReputational, rdcomplianceRegulatoryReputationalNA, rdcomplianceRegulatoryReputationalYes, rdcomplianceRegulatoryReputationalNo);
            }

            if (GeneralFuncsLib.NvlString(row["FilterName"]) == "USPrincipal")
            {
                var prinusForeign = GeneralFuncsLib.NvlString(row["FilterValueString"]);
                var rdPRINUSForeignNA = uxPRINUSForeign.FindControl("rdPRINUSForeignNA") as AS.Controls.Global.RadioButton;
                var rdPRINUSForeignYes = uxPRINUSForeign.FindControl("rdPRINUSForeignYes") as AS.Controls.Global.RadioButton;
                var rdPRINUSForeignNo = uxPRINUSForeign.FindControl("rdPRINUSForeignNo") as AS.Controls.Global.RadioButton;
                setValueRadio(prinusForeign, rdPRINUSForeignNA, rdPRINUSForeignYes, rdPRINUSForeignNo);
            }

            if (GeneralFuncsLib.NvlString(row["FilterName"]) == "PersonalGuarantee")
            {
                var personalGuarantee = GeneralFuncsLib.NvlString(row["FilterValueString"]);
                var rdPersonalGuaranteeNA = uxPersonalGuarantee.FindControl("rdPersonalGuaranteeNA") as AS.Controls.Global.RadioButton;
                var rdPersonalGuaranteeYes = uxPersonalGuarantee.FindControl("rdPersonalGuaranteeYes") as AS.Controls.Global.RadioButton;
                var rdPersonalGuaranteeNo = uxPersonalGuarantee.FindControl("rdPersonalGuaranteeNo") as AS.Controls.Global.RadioButton;
                setValueRadio(personalGuarantee, rdPersonalGuaranteeNA, rdPersonalGuaranteeYes, rdPersonalGuaranteeNo);
            }
        }
    }

    public int Save()
    {        
        var rdAutoApprovedIndicatorYes = uxAutoApprovedIndicator.FindControl("rdAutoApprovedIndicatorYes") as AS.Controls.Global.RadioButton;
        var rdAutoApprovedIndicatorNo = uxAutoApprovedIndicator.FindControl("rdAutoApprovedIndicatorNo") as AS.Controls.Global.RadioButton;
        var autoApproved = getValueRadio(rdAutoApprovedIndicatorYes, rdAutoApprovedIndicatorNo);

        var rdAcqAgentSharedLiabilityYes = uxAcqAgentSharedLiability.FindControl("rdAcqAgentSharedLiabilityYes") as AS.Controls.Global.RadioButton;
        var rdAcqAgentSharedLiabilityNo = uxAcqAgentSharedLiability.FindControl("rdAcqAgentSharedLiabilityNo") as AS.Controls.Global.RadioButton;
        var agentSharedLiability = getValueRadio(rdAcqAgentSharedLiabilityYes, rdAcqAgentSharedLiabilityNo);

        var rdVisaRegistrationYes = uxVisaRegistration.FindControl("rdVisaRegistrationYes") as AS.Controls.Global.RadioButton;
        var rdVisaRegistrationNo = uxVisaRegistration.FindControl("rdVisaRegistrationNo") as AS.Controls.Global.RadioButton;
        var visaRegistration = getValueRadio(rdVisaRegistrationYes, rdVisaRegistrationNo);

        var rdMCRegistrationYes = uxMCRegistration.FindControl("rdMCRegistrationYes") as AS.Controls.Global.RadioButton;
        var rdMCRegistrationNo = uxMCRegistration.FindControl("rdMCRegistrationNo") as AS.Controls.Global.RadioButton;
        var mcRegistration = getValueRadio(rdMCRegistrationYes, rdMCRegistrationNo);

        var rdDiscoverRegistrationYes = uxDiscoverRegistration.FindControl("rdDiscoverRegistrationYes") as AS.Controls.Global.RadioButton;
        var rdDiscoverRegistrationNo = uxDiscoverRegistration.FindControl("rdDiscoverRegistrationNo") as AS.Controls.Global.RadioButton;
        var discoverRegistration = getValueRadio(rdDiscoverRegistrationYes, rdDiscoverRegistrationNo);

        var rdAMEXRegistrationYes = uxAMEXRegistration.FindControl("rdAMEXRegistrationYes") as AS.Controls.Global.RadioButton;
        var rdAMEXRegistrationNo = uxAMEXRegistration.FindControl("rdAMEXRegistrationNo") as AS.Controls.Global.RadioButton;
        var amexRegistration = getValueRadio(rdAMEXRegistrationYes, rdAMEXRegistrationNo);

        var rdHighNDXYes = uxHighNDX.FindControl("rdHighNDXYes") as AS.Controls.Global.RadioButton;
        var rdHighNDXNo = uxHighNDX.FindControl("rdHighNDXNo") as AS.Controls.Global.RadioButton;
        var highNDX = getValueRadio(rdHighNDXYes, rdHighNDXNo);

        var rdComplianceRegulatoryReputationalYes = uxComplianceRegulatoryReputational.FindControl("rdComplianceRegulatoryReputationalYes") as AS.Controls.Global.RadioButton;
        var rdComplianceRegulatoryReputationalNo = uxComplianceRegulatoryReputational.FindControl("rdComplianceRegulatoryReputationalNo") as AS.Controls.Global.RadioButton;
        var complianceRegulatoryReputational = getValueRadio(rdComplianceRegulatoryReputationalYes, rdComplianceRegulatoryReputationalNo);

        var rdCBproducingYes = uxCBproducing.FindControl("rdCBproducingYes") as AS.Controls.Global.RadioButton;
        var rdCBproducingNo = uxCBproducing.FindControl("rdCBproducingNo") as AS.Controls.Global.RadioButton;
        var cbProducing = getValueRadio(rdCBproducingYes, rdCBproducingNo);

        var rdPRINUSForeignYes = uxPRINUSForeign.FindControl("rdPRINUSForeignYes") as AS.Controls.Global.RadioButton;
        var rdPRINUSForeignNo = uxPRINUSForeign.FindControl("rdPRINUSForeignNo") as AS.Controls.Global.RadioButton;
        var prinusForeign = getValueRadio(rdPRINUSForeignYes, rdPRINUSForeignNo);

        var rdPersonalGuaranteeYes = uxPersonalGuarantee.FindControl("rdPersonalGuaranteeYes") as AS.Controls.Global.RadioButton;
        var rdPersonalGuaranteeNo = uxPersonalGuarantee.FindControl("rdPersonalGuaranteeNo") as AS.Controls.Global.RadioButton;
        var personalGuarantee = getValueRadio(rdPersonalGuaranteeYes, rdPersonalGuaranteeNo);

        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", Int32.Parse(PrimaryID), DbType.Int32));
        if (!string.IsNullOrEmpty(autoApproved)) paramsIn.Add(new FilterParameter("@AutoApproved", autoApproved, DbType.AnsiString));
        if (!string.IsNullOrEmpty(agentSharedLiability)) paramsIn.Add(new FilterParameter("@AgentSharedLiability", agentSharedLiability, DbType.AnsiString));
        if (!string.IsNullOrEmpty(visaRegistration)) paramsIn.Add(new FilterParameter("@VisaRegistration", visaRegistration, DbType.AnsiString));
        if (!string.IsNullOrEmpty(mcRegistration)) paramsIn.Add(new FilterParameter("@MCRegistration", mcRegistration, DbType.AnsiString));
        if (!string.IsNullOrEmpty(discoverRegistration)) paramsIn.Add(new FilterParameter("@DiscoverRegistration", discoverRegistration, DbType.AnsiString));
        if (!string.IsNullOrEmpty(amexRegistration)) paramsIn.Add(new FilterParameter("@AMEXRegistration", amexRegistration, DbType.AnsiString));
        if (!string.IsNullOrEmpty(highNDX)) paramsIn.Add(new FilterParameter("@NonDeliveryExposure", highNDX, DbType.AnsiString));
        if (!string.IsNullOrEmpty(cbProducing)) paramsIn.Add(new FilterParameter("@ElevatedDisputes", cbProducing, DbType.AnsiString));
        if (!string.IsNullOrEmpty(complianceRegulatoryReputational)) paramsIn.Add(new FilterParameter("@EnhancedDueDiligence", complianceRegulatoryReputational, DbType.AnsiString));
        if (!string.IsNullOrEmpty(prinusForeign)) paramsIn.Add(new FilterParameter("@USPrincipal", prinusForeign, DbType.AnsiString));
        if (!string.IsNullOrEmpty(personalGuarantee)) paramsIn.Add(new FilterParameter("@PersonalGuarantee", personalGuarantee, DbType.AnsiString));

        FilterParameterCollection paramsOut;
        WebServices.RiskServices.ExecuteNonQueryCommand(SPA_UPDATE_ASSIGNMENT_FILTER, paramsIn, out paramsOut);
        return Convert.ToInt32(PrimaryID);
    }

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    public event EventHandler SelectedChanged;

    #endregion

    #region Validate

    public void InitValidation()
    {
    }

    #endregion

    public void Rebind()
    {
        OnDataBindControls(DataBindAction.LoadCustomFilter);
        VisibleControls();
        SetLabels();
        InitValidation();
    }
}
