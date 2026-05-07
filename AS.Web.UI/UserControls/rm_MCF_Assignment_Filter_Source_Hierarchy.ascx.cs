using AS.Common;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;

public partial class UserControls_rm_MCF_Assignment_Filter_Source_Hierarchy : GlobalUserControl, IRiskParamFilter
{

    #region Enums
    enum DataBindAction
    {
        BindLeadSource,
        BindReferralSource
    }

    #endregion

    #region ---- Propeties ----
    public WebSiteEnums.FeatureMode FeatureMode { get; set; }
    public WebSiteEnums.ParamFilterMode Mode { get; set; }
    public string PrimaryID { get; set; }
    public string ParamID { get; set; }
    public string FilterID { get; set; }
    protected int MaxLengthViewMore
    {
        get
        {
            return Convert.ToInt32(WebSiteSettings.RiskViewMore);
        }
    }
    #endregion ---- Propeties ----
    #region ---- Event ----
    protected void Page_Load(object sender, EventArgs e)
    {
        VisibleControls();
        if (!IsPostBack && !(Mode == WebSiteEnums.ParamFilterMode.Assignment))
        {
            BindSupplemetal();
        }
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindLeadSource:
                Dictionary<string, string> result = UserControls_rm_MCF_Filter_Lead_Source.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID);

                string data = result["Label"];
                uxltlLeadSource.Attributes.Add("tracking-value", result["Tracking"]);

                if (data.Length > MaxLengthViewMore)
                {
                    var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
                    uxltlLeadSource.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
                }
                uxltlLeadSource.Text = BuildHierarchyStateProfileLabel(data,
                    ViewMoreModalType.LoadLeadSource, string.Empty);
                break;
            case DataBindAction.BindReferralSource:
                Dictionary<string, string> resultReferral = UserControls_rm_MCF_Filter_Referral_Source.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID);

                string dataReferralSource = resultReferral["Label"];
                uxltlReferralSource.Attributes.Add("tracking-value", resultReferral["Tracking"]);

                if (dataReferralSource.Length > MaxLengthViewMore)
                {
                    var temp = dataReferralSource.Split(new string[] { "<br />" }, StringSplitOptions.None);
                    uxltlReferralSource.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : dataReferralSource);
                }
                uxltlReferralSource.Text = BuildHierarchyStateProfileLabel(dataReferralSource,
                    ViewMoreModalType.LoadReferralSource, string.Empty);
                break;
        }
    }
    protected void btnRefreshLeadSource_Click(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.BindLeadSource, null);
    }

    protected void btnRefreshReferralSource_Click(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReferralSource, null);
    }
    #endregion ---- Event ----
    private void VisibleControls()
    {
        uxplhReferralSource.Visible = uxplhLeadSource.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            uxGroupSourceHierachyView.Visible = true;
        }
        else
            uxGroupSourceHierachyEdit.Visible = true;

        if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.LeadSource))
        {
            uxLeadSource.Visible = true;
        }

        if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.ReferralSource))
        {
            uxReferralSource.Visible = true;
        }

        if (!uxltlLeadSource.Visible && !uxReferralSource.Visible)
        {
            uxGroupSourceHierachyEdit.Visible = false;
            uxGroupSourceHierachyView.Visible = false;
        }
    }
    #region IRiskParamFilter Members
    /// <summary>
    /// If ModalType = LoadHierarchys, then we must pass hierarchyFilterMode value
    /// In other cases, we pass empty value.
    /// </summary>
    private string BuildHierarchyStateProfileLabel(string data, ViewMoreModalType modalType,
        string hierarchyFilterMode)
    {
        string localQueryString = string.Empty;
        if (data.Length > MaxLengthViewMore)
        {
            localQueryString = BuildQueryStringForViewMoreModal(modalType, hierarchyFilterMode);
            data = data.Substring(0, MaxLengthViewMore)
                + string.Format(
                    "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?{0}', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>",
                    localQueryString);
        }
        return data;
    }

    private string BuildQueryStringForViewMoreModal(ViewMoreModalType modalType,
        string hierarchyMode)
    {
        string localQueryString = string.Empty;
        localQueryString = Page.BuildSecureQueryString(string.Format(
                  "primaryid={0}&mode={1}&typemodal={2}",
                  PrimaryID,
                  Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc
                                                             : (int)WebSiteEnums.ParamFilterMode.Assignment,
                  ((int)modalType).ToString())
              );
        return localQueryString;
    }
    public void Rebind()
    {
        VisibleControls();
        BindSupplemetal();
    }
    //Bind supplemetal
    protected void BindSupplemetal()
    {
        if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.LeadSource))
        {
            OnDataBindControls(DataBindAction.BindLeadSource, null);
        }

        if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.ReferralSource))
        {
            OnDataBindControls(DataBindAction.BindReferralSource, null);
        }
    }
    public int Save()
    {
        return 0;
    }
    public event EventHandler SelectedChanged;
    #endregion
}
