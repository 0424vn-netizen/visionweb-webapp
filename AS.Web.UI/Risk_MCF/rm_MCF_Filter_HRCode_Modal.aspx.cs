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

public partial class rm_MCF_Filter_HRCode_Modal : NonReportPage
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
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (SecureQueryString != null && SecureQueryString["mode"] != null)
        {
            int mode = Int16.Parse(SecureQueryString["mode"]);
            uxHRCode.Mode = (WebSiteEnums.ParamFilterMode)mode;
        }
        if (SecureQueryString != null && SecureQueryString["primaryid"] != null)
            uxHRCode.PrimaryID = SecureQueryString["primaryid"];
        if (SecureQueryString != null && SecureQueryString["paramid"] != null)
            uxHRCode.ParamID = SecureQueryString["paramid"];
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindStatus);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Close:
                uxHRCode.IsInclude = IsIncluded;
                uxHRCode.Save();
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
        _params.Add("@PrimaryID", Convert.ToInt32(uxHRCode.PrimaryID), DbType.Int32);
        _params.Add("@Mode", Convert.ToInt32(uxHRCode.Mode), DbType.Int32);
        _params.Add("@isIncluded", isIncluded, DbType.Boolean);

        FilterParameterCollection _paramsOut = new FilterParameterCollection();
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_UpdateIncludeStatusHRCode", _params, out _paramsOut);
    }
    private void BindStatus()
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(uxHRCode.PrimaryID), DbType.Int32));
        parames.Add(new FilterParameter("@Mode", Convert.ToInt32(uxHRCode.Mode), DbType.Int32));
        parames.Add(new FilterParameter("@IsIncluded", false, DbType.Boolean, true));
        
        FilterParameterCollection paramesOut = new FilterParameterCollection();

        // Get OutParams value
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_GetFilterHRCode_Info", parames, out paramesOut);

        FilterParameter isIncluded = paramesOut.FindFilterParameterByName("@IsIncluded", true);

        if (isIncluded != null)
        {
            uxRadioMode.Items[0].Selected = Convert.ToBoolean(isIncluded.ParameterValue);
            uxRadioMode.Items[1].Selected = !uxRadioMode.Items[0].Selected;
        }

    }
}