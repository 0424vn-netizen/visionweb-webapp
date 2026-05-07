using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rm_MCF_Filter_GauthenticateCode_Modal : NonReportPage
{
    #region Enums
    enum DataBindAction
    {
        LoadProfiles
    }
    enum PostBackAction
    {
        Close,
        SelectStatus
    }
    #endregion
    public bool IsIncluded
    {
        get
        {
            return this.uxRadioMode.Items[0].Selected;
        }
    }

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (SecureQueryString != null && SecureQueryString["mode"] != null)
        {
            int mode = Int16.Parse(SecureQueryString["mode"]);
            Mode = (WebSiteEnums.ParamFilterMode)mode;
        }
        if (SecureQueryString != null && SecureQueryString["primaryid"] != null)
            PrimaryID = SecureQueryString["primaryid"];
        
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.LoadProfiles);
        }
    }
    
    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Close:
                Save();
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
            case DataBindAction.LoadProfiles:
                BindData();
                break;
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
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
        _params.Add(new FilterParameter("@Mode", Convert.ToInt32(Mode), DbType.Int32));
        _params.Add(new FilterParameter("@AssignmentID", Convert.ToInt32(PrimaryID), DbType.Int32));
        _params.Add(new FilterParameter("@IsIncluded", isIncluded, DbType.Boolean));
        _params.Add(new FilterParameter("@ItemCode", WebSiteEnums.Filter_Extend.GauthenticateCode.ToString(), DbType.String));

        FilterParameterCollection _paramsOut = new FilterParameterCollection();
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_Update_StatusIncludeByValue", _params, out _paramsOut);
    }

    private void BindData()
    {
        if (string.IsNullOrEmpty(PrimaryID))
            return;

        DataTable listOri = null;
        DataTable listDes = null;
        if (Int32.Parse(PrimaryID) == 0)
        {
            listOri = ProcessData(WebSiteEnums.AssignmentFilterModes.All);
            listDes = listOri.Clone();
        }
        else
        {
            listOri = ProcessData(WebSiteEnums.AssignmentFilterModes.NotAssigned);
            listDes = ProcessData(WebSiteEnums.AssignmentFilterModes.Assigned, true);
        }
        uxGauthenticateCode.DataSourceOrigination = listOri;
        uxGauthenticateCode.DataSourceDestination = listDes;
    }

    private DataTable ProcessData(WebSiteEnums.AssignmentFilterModes whichMode, bool isIncluded = false)
    {
        DataSet ds = GetFilterList(whichMode);
        if (isIncluded)
        {
            if (ds.Tables[1].Rows.Count > 0)
            {
                bool isIncludedVal = ds.Tables[1].Rows[0]["IsIncluded"].ToBoolean();
                uxRadioMode.Items[0].Selected = isIncludedVal;
                uxRadioMode.Items[1].Selected = !isIncludedVal;
            }
        }
        return ds.Tables[0];
    }

    /// <summary>
    /// Get Filter List
    /// </summary>
    /// <param name="whichMode">whichMode</param>
    /// <returns>Datatable</returns>
    private DataSet GetFilterList(WebSiteEnums.AssignmentFilterModes whichMode)
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.AddLanguageID();
        paramsIn.Add(new FilterParameter("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32));
        paramsIn.Add(new FilterParameter("@Option", whichMode, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        paramsIn.Add(new FilterParameter("@ItemCode", WebSiteEnums.Filter_Extend.GauthenticateCode.ToString(), DbType.String));
        return WebServices.RiskServices.GetReportsAsDataSet("spa_RM_MCF_Get_FilterItemValue", paramsIn);
    }

    public void Save()
    {
        if (string.IsNullOrEmpty(PrimaryID))
            return;

        FilterParameterCollection paramsIn = new FilterParameterCollection();
        FilterParameterCollection paramsOut = new FilterParameterCollection();

        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32);
        paramsIn.Add("@ItemValueList", uxGauthenticateCode.GetSelectedItemsAsString(","), DbType.String);
        paramsIn.Add("@Mode", this.Mode, DbType.Int32);
        paramsIn.Add("@ItemCode", WebSiteEnums.Filter_Extend.GauthenticateCode.ToString(), DbType.String);
        paramsIn.Add("@IsIncluded", IsIncluded, DbType.Boolean);

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_Save_ItemValue", paramsIn, out paramsOut);
    }

    public event EventHandler SelectedChanged;

    protected void MultiSelector_OnMovedData(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (SelectedChanged != null)//if event register
                this.SelectedChanged(this, e);//raise event
        }
    }
}