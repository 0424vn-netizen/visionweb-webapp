using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[PagePermission("RskManAss,RskAdhoc,MSRskManAss,MSRskAdhoc")]
public partial class rm_MCF_Filter_MerchantClassifications_Modal : NonReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindStatus
    }
    enum PostBackAction
    {
        Close
        , SelectStatus
    }
    #endregion

    public string HierarchyFilterText { get; set; }
    public string ClientIDbtn { get; set; }
    public string HierarchyFilterMode { get; set; }
    public bool IsIncludeExcludeItem
    {
        get
        {
            string isInExItem = SecureQueryString["isInExItem"];
            if (isInExItem == "1")
            {
                return true;
            }
            return false;
        }
    }
    public bool GetItemFromSession
    {
        get
        {
            string InS = Request.QueryString["InS"];
            if (InS == "1")
            {
                return true;
            }
            return false;
        }
    }
    protected bool IsIncluded
    {
        get
        {
            return this.uxRadioMode.Items[0].Selected;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (SecureQueryString != null && SecureQueryString["mode"] != null)
        {
            int mode = Int16.Parse(SecureQueryString["mode"]);
            uxMerchantClassifications.Mode = (WebSiteEnums.ParamFilterMode)mode;
        }
        if (SecureQueryString != null && SecureQueryString["primaryid"] != null)
            uxMerchantClassifications.PrimaryID = SecureQueryString["primaryid"];
        if (SecureQueryString != null && SecureQueryString["paramid"] != null)
            uxMerchantClassifications.ParamID = SecureQueryString["paramid"];
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindStatus);
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Close:
                uxMerchantClassifications.IsInclude = IsIncluded;
                uxMerchantClassifications.Save();
                ClientScript.RegisterStartupScript(GetType(), "startupscript", "parent.ClosePopupModal(1);", true);
                break;
            case PostBackAction.SelectStatus:
                UpdateIncludeStatus(IsIncluded);
                break;
        }
    }


    public void BindIncludeExcludeStatus()
    {
        if (GetItemFromSession)
        {
            if (SessionManager.IsIncludeItem)
            {
                uxRadioMode.Items[0].Selected = true;
            }
            else
            {
                uxRadioMode.Items[1].Selected = true;
            }
        }
        else
        {
            uxRadioMode.Items[0].Selected = true;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindStatus:
                BindStatus();
                break;
        }
    }

    protected void uxRadioMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SelectStatus);
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Close);
    }
    private int UpdateIncludeStatus(bool isIncluded)
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _params.Add("@PrimaryID", Convert.ToInt32(uxMerchantClassifications.PrimaryID), DbType.Int32);
        _params.Add("@Mode", Convert.ToInt32(uxMerchantClassifications.Mode), DbType.Int32);
        _params.Add("@isIncluded", isIncluded, DbType.Boolean);

        FilterParameterCollection _paramsOut = new FilterParameterCollection();
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_MRS_Update_IncludeStatusMIFClassification", _params, out _paramsOut);
    }
    private void BindStatus()
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(uxMerchantClassifications.PrimaryID), DbType.Int32));
        parames.Add(new FilterParameter("@Mode", Convert.ToInt32(uxMerchantClassifications.Mode), DbType.Int32));
        parames.Add(new FilterParameter("@IsIncluded", false, DbType.Boolean, true));

        FilterParameterCollection paramesOut = new FilterParameterCollection();

        // Get OutParams value
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_MRS_Get_StatusInclueMIFClassification", parames, out paramesOut);

        FilterParameter isIncluded = paramesOut.FindFilterParameterByName("@IsIncluded", true);

        if (isIncluded != null)
        {
            uxRadioMode.Items[0].Selected = Convert.ToBoolean(isIncluded.ParameterValue);
            uxRadioMode.Items[1].Selected = !uxRadioMode.Items[0].Selected;
        }

    }

}