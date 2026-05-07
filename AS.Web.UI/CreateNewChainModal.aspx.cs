using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI;
using AS.Security.WS.Entities;
using AS.NetCore.Api.Client.Models;
using AS.NetCore.Api.Client;
using DocumentFormat.OpenXml.Spreadsheet;

[PagePermission("AddEditChain")]
public partial class CreateNewChainModal : NonReportPage
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
                    bool isUpdateMicro = GeneralFuncsLib.GetDataOfExtendedSetting("UpdateChainToMicro") == "true";
                    if (isUpdateMicro)
                    {
                        // Create New Chain
                        FilterParameterCollection parameters = new FilterParameterCollection();
                        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.AnsiString));
                        parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
                        DataTable dt = WebServices.CsReportServices.GetReports("spa_CreateNewChainForFulton", parameters);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string chainNumber = dt.Rows[0]["ChainNumber"].ToString();

                            //insert a comment
                            InsertCommentToMerchantNote(chainNumber);

                            // Create and Update to micro DB
                            UpdateChainNumberToMicroDB(MerchantNumber, chainNumber);
                        }
                    }
                    else
                    {
                        FilterParameterCollection parameters = new FilterParameterCollection();
                        parameters.AddLoggedInUserReportingParams(true);
                        parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
                        DataTable dt = WebServices.CsReportServices.GetReports("spa_CreateNewChainForORI", parameters);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //insert a comment
                            InsertCommentToMerchantNote(dt.Rows[0]["ChainNumber"].ToString());

                            // TK26080 - WRFC - New VW Client Implementation
                            if (GeneralFuncsLib.CreateMsChainUserWhenAddingNewChain())
                            {
                                CreateChainUser(dt.Rows[0]["ChainNumber"].ToString());
                                return;
                            }
                        }
                    }
                }

                break;
        }
    }

    private void InsertCommentToMerchantNote(string chainNumber)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection tempParameters = null;
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
        parameters.Add(new FilterParameter("@Comment", GetLocalResourceObject("CreateNewChainModal_aspx_AddToNewChain").ToString() + " " + chainNumber, DbType.String));
        WebServices.MsReportServices.ExecuteNonQueryCommand("spa_cs_InsertCommentsOfMerchant", parameters, out tempParameters);
    }

    protected BaseRequestData<T> InitRequestModel<T>(T model) where T : RequestBody, new()
    {
        var request = new BaseRequestData<T>()
        {
            Data = model,
        };

        request.Header.AsClientId = SessionManager.CurrentUser.ASClient;

        return request;
    }

    private void UpdateChainNumberToMicroDB(string merchantNumber, string chainNumber)
    {
        UpdateChainNumberRequest requestItem = new UpdateChainNumberRequest()
        {
            IsCreate = true,
            MerchantNumber = merchantNumber,
            ChainNumber = chainNumber
        };

        var request = InitRequestModel(requestItem);

        var client = new FapsApiClient();
        client.UpdateChainNumber(request);
    }

    protected void CreateChain_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CreateNewChain);
        ClientScript.RegisterStartupScript(GetType(), "CloseModal", "UpdateChainSuccess();", true);
    }
    private void CreateChainUser(string chainUserId)
    {
        // If Chain User exists already, do nothing.
        if (WebServices.SecurityServices.IsExistedUserName(SessionManager.CurrentUser.ASClient, chainUserId))
        {
            return;
        }

        // Get "Hierarchy Users" role by name
        string[] roles = null;
        HierarchyCollection hierarchyUserList = WebServices.SecurityServices.GetHierarchyByName(
            GeneralFuncsLib.HIERARCHY_USERS_TEXT,
            SessionManager.CurrentClient,
            WebSiteConstants.AS_SYSTEM_MS);
        if (hierarchyUserList != null && hierarchyUserList.Count > 0)
        {
            roles = new string[1] { hierarchyUserList[0].HierarchyID.ToString() };
        }
        else
        {
            roles = new string[0];
        }

        // create new user
        User user = new User();
        user.UserID = chainUserId;
        user.UserNameFirst = chainUserId;
        user.UserNameLast = chainUserId;
        user.UserNameFull = string.Format("{0} {1}", user.UserNameFirst, user.UserNameLast);
        user.Email = SessionManager.CurrentUser.Email;
        user.Status = "1";
        user.UserPassword = WebServices.SecurityServices.GenPwd();
        user.UserPasswordType = 10;
        user.LoginQuestionIndex = 1;
        user.UserSecRole = UserSecRole.CHAINPRI;
        user.CreatedBy = SessionManager.CurrentUser.RecId;
        user.ASClient = SessionManager.CurrentUser.ASClient;
        user.SiteID = 1;
        user.EntityID = chainUserId;

        // Save new user into DB
        WebServices.SecurityServices.CreateMSUser(user, roles, HierarchyMode.WRFC_CHAIN);

        _parentPage.ASPXTrackingLog.LogData5 = GetLocalResourceObject("CreateNewChainModal_aspx_NewPassword").ToString() + ":\t" + user.UserID + "\t" + user.UserPassword;
        User newuser = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, user.UserID);
        user.RecId = newuser.RecId;

        //General password
        SessionManager.ResetPasswordUser = user;

        if (!GeneralFuncsLib.HasSSOLogin())
        {
            string queryString = BuildSecureQueryString("IsFromCreateNewChainModal=true");
            Response.Redirect(string.Format("CreateNewUser2.aspx?{0}", queryString), false);
        }
    }

}
