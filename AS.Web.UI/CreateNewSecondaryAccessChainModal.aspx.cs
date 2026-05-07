using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI;
using AS.Security.WS.Entities;
[PagePermission("AddEditAccessChain")]
public partial class CreateNewSecondaryAccessChainModal : NonReportPage
{
    enum PostBackAction
    {
        CreateNewChain
    }
    private SecurePage _parentPage = null;
    protected string MerchantNumber
    {
        get
        {
            if (IsSecureQueryString)
            {
                return SecureQueryString["MerchNum"].ToString();
            }
            else
                return string.Empty;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        _parentPage = (SecurePage)this.Page;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    protected override void OnPostBackActions(Enum type, object sender)
    {
        base.OnPostBackActions(type, sender);
        switch ((PostBackAction)type)
        {
            case PostBackAction.CreateNewChain:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    FilterParameterCollection paramOuts = new FilterParameterCollection();
                    parameters.AddLoggedInUserReportingParams(true);
                    parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
                    DataTable dt = WebServices.CsReportServices.GetReports("spa_cs_CreateNewSecondaryAccessChain", parameters);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //insert a comment
                        parameters = new FilterParameterCollection();
                        FilterParameterCollection tempParameters = null;
                        parameters.AddLoggedInUserReportingParams();
                        parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
                        parameters.Add(new FilterParameter("@Comment", GetLocalResourceObject("CreateNewChainModal_aspx_AddToNewChain").ToString() + " " + dt.Rows[0]["ChainNumber"].ToString(), DbType.String));
                        WebServices.MsReportServices.ExecuteNonQueryCommand("spa_cs_InsertCommentsOfMerchant", parameters, out tempParameters);
                    }
                }

                break;
        }
    }
    protected void CreateChain_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CreateNewChain);
        ClientScript.RegisterStartupScript(GetType(), "CloseModal", "UpdateChainSuccess();", true);
    }

}
