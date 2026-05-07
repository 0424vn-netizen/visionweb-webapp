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
using AS.Controls.Pages;
using AS.Common.DBManager;

[PagePermission("RskManAss,RskAdhoc,MSRskManAss,MSRskAdhoc")]
public partial class rm_MCF_Filter_Hierarchy_Modal : NonReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindStatus
    }
    enum PostBackAction
    {
        Close
        ,SelectStatus
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

        HierarchyFilterMode = SecureQueryString["hierarchyMode"];
        uxHierarchy.HierarchyFilterMode = HierarchyFilterMode;
        uxHierarchy.IsIncludeExcludeItem = IsIncludeExcludeItem;
        uxHierarchy.GetItemFromSession = GetItemFromSession;
        if (IsIncludeExcludeItem)
        {
            uxRadioMode.AutoPostBack = false;
            //uxHierarchy._bindIncludeExcludeStatus += new EventHandler(BindIncludeExcludeStatus);
            this.Title = GetLocalResourceObject("rm_Filter_Hierarchy_Modal_aspx_cs_Select").ToString();
        }
        else
        {
            uxRadioMode.AutoPostBack = true;
            HierarchyFilterText = GeneralFuncsLib.GetRiskHierarchyFilterText(HierarchyFilterMode);
            uxHierarchy.HierarchyFilterText = HierarchyFilterText;
            ClientIDbtn = SecureQueryString["clientID"];
            uxHierarchy.PrimaryID = SecureQueryString["primaryid"];
            int mode = Int16.Parse(SecureQueryString["mode"]);
            uxHierarchy.Mode = (WebSiteEnums.ParamFilterMode)mode;
            uxHierarchy.ParamID = SecureQueryString["paramid"];
            this.Title = GetLocalResourceObject("rm_Filter_Hierarchy_Modal_aspx_cs_Select").ToString() + " " + HierarchyFilterText;
        }

        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindStatus);

        }        
    }

    //public void BindIncludeExcludeStatus(object sender, EventArgs e)
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

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.Close:
                if (IsIncludeExcludeItem)
                {
                    string result = uxHierarchy.SaveIncludeExcludeISONumber();
                    SessionManager.IsIncludeItem = IsIncluded;
                    ClientScript.RegisterStartupScript(GetType(), "startupscript", "parent.RefreshIncludeExcludeItems('" + result + "','" + IsIncluded + "'); ClosePopupModal();", true);
                }
                else
                {
                    uxHierarchy.IsInclude = IsIncluded;
                    uxHierarchy.Save();
                    ClientScript.RegisterStartupScript(GetType(), "startupscript", "parent.ClosePopupModal(1);", true);
                }
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
        OnPostBackActions(PostBackAction.Close, sender);
    }
    protected void uxRadioMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!IsIncludeExcludeItem)
        {
            //SessionManager.IsIncludeItem = uxRadioMode.Items[0].Selected ? true : false;
            OnPostBackActions(PostBackAction.SelectStatus);
        }
    }
    private int UpdateIncludeStatus(bool isIncluded)
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _params.Add(new FilterParameter("@Mode", Convert.ToInt32(uxHierarchy.Mode), DbType.Int32));
        _params.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(uxHierarchy.PrimaryID), DbType.Int32));
        _params.Add(new FilterParameter("@IsIncluded", isIncluded, DbType.Boolean));
        _params.Add(new FilterParameter("@HierarchyFilterMode", uxHierarchy.HierarchyFilterMode, DbType.String));

        FilterParameterCollection _paramsOut = new FilterParameterCollection();
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_UpdateIncludeStatusHierarchy", _params, out _paramsOut);
    }
    private void BindStatus()
    {
        if (IsIncludeExcludeItem)
        {
            BindIncludeExcludeStatus();
        }
        else
        {
            FilterParameterCollection parames = new FilterParameterCollection();
            parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parames.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(uxHierarchy.PrimaryID), DbType.Int32));
            parames.Add(new FilterParameter("@Mode", Convert.ToInt32(uxHierarchy.Mode), DbType.Int32));
            parames.Add(new FilterParameter("@IsIncluded", false, DbType.Boolean, true));
            parames.Add(new FilterParameter("@HierarchyFilterMode", uxHierarchy.HierarchyFilterMode, DbType.String));

            FilterParameterCollection paramesOut = new FilterParameterCollection();

            // Get OutParams value
            WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_GetFilterHierarchy_Info", parames, out paramesOut);

            FilterParameter isIncluded = paramesOut.FindFilterParameterByName("@IsIncluded", true);

            if (isIncluded != null)
            {
                uxRadioMode.Items[0].Selected = Convert.ToBoolean(isIncluded.ParameterValue);
                uxRadioMode.Items[1].Selected = !uxRadioMode.Items[0].Selected;
            }
        }

    }
    
}
