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
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;

[PagePermission("RskManAss,RskAdhoc,MSRskManAss,MSRskAdhoc")]
public partial class rm_MCF_ParameterFilter_ACHReturnCodeModal : NonReportPage
{
    #region Enums
    enum DataBindAction { BindStatus }
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
    public bool IsView
    {
        get
        {
            var isvew = SecureQueryString["IsView"];
            if (string.IsNullOrEmpty(isvew) || !Convert.ToBoolean(isvew))
                return false;
            return true;
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        uxHierarchy.PrimaryID = SecureQueryString["PrimaryID"];
        int mode = Int16.Parse(SecureQueryString["Mode"]);
        uxHierarchy.Mode = (WebSiteEnums.ParamFilterMode)mode;
        uxHierarchy.ParamID = SecureQueryString["ParamID"];
        uxClose.Enabled = uxRadioMode.Enabled =uxHierarchy.Enabled = !IsView;
        this.Title = GetLocalResourceObject("rm_ParameterFilter_ACHReturnCodeModal_aspx_cs").ToString();
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
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.Close:
                uxHierarchy.IsInclude = IsIncluded;
                uxHierarchy.Save();
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
        OnPostBackActions(PostBackAction.Close, sender);
    }
    protected void uxRadioMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SelectStatus);
    }
    private int UpdateIncludeStatus(bool isIncluded)
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _params.Add(new FilterParameter("@AssignmentID", Convert.ToInt32(uxHierarchy.PrimaryID), DbType.Int32));
        _params.Add(new FilterParameter("@IsIncluded", isIncluded, DbType.Boolean));
        _params.Add(new FilterParameter("@ItemID", 105, DbType.Int32));
        _params.Add(new FilterParameter("@ParameterCode ", uxHierarchy.ParamID, DbType.String));
        _params.Add(new FilterParameter("@PageID", Convert.ToInt32(uxHierarchy.Mode), DbType.Int32));

        FilterParameterCollection _paramsOut = new FilterParameterCollection();
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_Save_AssignmentParameterFilter", _params, out _paramsOut);
    }

    private void BindStatus()
    {
        var included = false;
        string code = VeraCodeSolution.ValidateResponseData(UserControls_rm_MCF_ParameterFilter_ACHReturnCode.GetSelectedValuesAsString(uxHierarchy.Mode, uxHierarchy.PrimaryID, uxHierarchy.ParamID, ref included));
        uxRadioMode.Items[0].Selected = included;
        uxRadioMode.Items[1].Selected = !uxRadioMode.Items[0].Selected;
    }
}
