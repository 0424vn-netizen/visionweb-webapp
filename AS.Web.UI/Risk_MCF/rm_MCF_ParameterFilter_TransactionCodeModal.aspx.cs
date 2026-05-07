using System;
using AS.Controls.Pages;
using BuGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;

[PagePermission("RskManAss,RskAdhoc,MSRskManAss,MSRskAdhoc")]
public partial class rm_MCF_ParameterFilter_TransactionCodeModal : NonReportPage
{
    #region Enums
    enum DataBindAction { }
    enum PostBackAction
    {
        Close
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (SecureQueryString["MarketData"] != null)
        {
            uxHierarchy.MarketData = SecureQueryString["MarketData"];
        }
        else
        {
            uxHierarchy.PrimaryID = SecureQueryString["PrimaryID"];
            int mode = Int16.Parse(SecureQueryString["Mode"]);
            uxHierarchy.Mode = (WebSiteEnums.ParamFilterMode)mode;
        }
        uxHierarchy.ParamID = SecureQueryString["ParamID"];

        this.Title = GetTitleParameter(uxHierarchy.ParamID);
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
                if (SecureQueryString["MarketData"] != null)
                {
                    uxHierarchy.Save_MarketData();
                    ClientScript.RegisterStartupScript(GetType(), "startupscript", "CloseModal();", true);
                }
                else
                {
                    uxHierarchy.Save();
                    ClientScript.RegisterStartupScript(GetType(), "startupscript", "parent.ClosePopupModal(1);", true);
                } break;
        }
    }

    protected void uxClose_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Close, sender);
    }
    private string GetTitleParameter(string paramKey)
    {
        var isModelTypeInConfig = BuGeneralFuncsLib.CheckExistsInSplitToArray(WebSiteSettings.ParametersAllowDecimal, ',', paramKey);
        if (isModelTypeInConfig)
        {
            return GetLocalResourceObject("rm_ParameterFilter_TransactionCodeModal_aspx_cs_Title_ModelType").ToString();
        }
        return GetLocalResourceObject("rm_ParameterFilter_TransactionCodeModal_aspx_cs").ToString(); ;
    }
}
