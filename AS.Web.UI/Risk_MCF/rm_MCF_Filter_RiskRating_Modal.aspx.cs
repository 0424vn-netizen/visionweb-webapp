using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;

[PagePermission("RskManAss,RskAdhoc,MSRskManAss,MSRskAdhoc")]
public partial class rm_MCF_Filter_RiskRating_Modal : NonReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindStatus
    }
    enum PostBackAction
    {
        Close,
        SelectStatus
    }
    #endregion

    protected bool IsIncluded
    {
        get
        {
            return this.uxRadioMode.Items[0].Selected;
        }
    }

    private string FilterName = "RiskRating";
    protected string RiskRatingLabel = "Risk Rating";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        uxFilterRiskRating.Mode = (WebSiteEnums.ParamFilterMode)(int.Parse(SecureQueryString["mode"]));
        uxFilterRiskRating.PrimaryID = SecureQueryString["primaryid"];
        uxFilterRiskRating.ParamID = SecureQueryString["paramid"];


        RiskRatingLabel = GetLocalResourceObject("Filter_RiskRating_Modal_aspx_cs_State").ToString();

        this.Page.Title = GetLocalResourceObject("Filter_RiskRating_Modal_aspx_cs_Select").ToString() + " " + (GetLocalResourceObject("Filter_RiskRating_Modal_aspx_cs_State").ToString());
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindStatus);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Close:
                uxFilterRiskRating.IsInclude = IsIncluded;
                uxFilterRiskRating.Save();
                ClientScript.RegisterStartupScript(GetType(), "startupscript", "parent.ClosePopupModal(1);", true);
                break;
            case PostBackAction.SelectStatus:
                UpdateIncludeStatus(IsIncluded);
                break;
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

    protected void uxClose_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Close);
    }

    protected void uxRadioMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SelectStatus);
    }

    private int UpdateIncludeStatus(bool isIncluded)
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _params.Add(new FilterParameter("@Mode", Convert.ToInt32(uxFilterRiskRating.Mode), DbType.Int32));
        _params.Add(new FilterParameter("@AssignmentID", Convert.ToInt32(uxFilterRiskRating.PrimaryID), DbType.Int32));
        _params.Add(new FilterParameter("@FilterName", FilterName, DbType.AnsiString));
        _params.Add(new FilterParameter("@IsIncluded", isIncluded, DbType.Boolean));

        FilterParameterCollection _paramsOut = new FilterParameterCollection();
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_UpdateIncludeElementFilter", _params, out _paramsOut);
    }
    private void BindStatus()
    {
        var isIncluded = true;
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@AssignmentID", Convert.ToInt32(uxFilterRiskRating.PrimaryID), DbType.Int32));
        parames.Add(new FilterParameter("@Mode", Convert.ToInt32(uxFilterRiskRating.Mode), DbType.Int32));
        parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        parames.Add(new FilterParameter("@FilterName", FilterName, DbType.AnsiString));

        // Get OutParams value
        DataSet ds = WebServices.RiskServices.GetReportsAsDataSet("spa_RM_MCF_GetElementFilter", parames);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
                isIncluded = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsIncluded"]);
        }

        uxRadioMode.Items[0].Selected = isIncluded;
        uxRadioMode.Items[1].Selected = !uxRadioMode.Items[0].Selected;
    }
}
