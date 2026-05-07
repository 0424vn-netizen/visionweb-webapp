using System;
using System.Web.UI;
using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.Security.WS.Entities;
using AS.Common.Logger;
using AS.Web.Business.PCI;
using AS.Web.Business.PCI.Models;
using AS.Web.Business.Shared.Constants;

[PagePermission("SiteAccessPCIAdmin,HierarchySiteAccessPCIAdmin,MerchantSiteAccessPCIAdmin")]
public partial class PCISecure : NonReportPage
{
    private enum PCIMaintenanceMode
    {
        Offline = 1,
        Online = 0,
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (PCIInMaintenanceMode())
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PCIMaintenanceModal",
                    String.Format("openPopupWindow('PCIMaintenanceModal.aspx', 'PCIMaintenanceModal', 500, 225);"), true);
            return;
        }
        bool isShowMsg = false;
        FilterParameterCollection _params = GeneralFuncsLib.GetMasterMerchant(out isShowMsg);
        if (isShowMsg)
        {
            string message = GetLocalResourceObject("PCISecure_aspx_cs_CannotAccess").ToString();
            uxMessage.Text = message;
            //Writer log info to PCI SSO log file
            PciLogInfo(SessionManager.CurrentUser.ASClient, Guid.NewGuid(), message);
            return;
        }

        var pciUserInfo = GetUserInfoInPci(_params);
        var errMsg = pciUserInfo != null ? SsoToPCIForExistUser(pciUserInfo) : SsoToPCIForNonUser(_params);
        uxMessage.Text = (string.IsNullOrEmpty(errMsg) || errMsg == PciConstants.SSO_SUCCESS) ? string.Empty: errMsg;
    }

    private void JumpToPCI(int asClient, Guid requestId, string pciUserName)
    {
        //Writer log info to PCI SSO log file
        PciLogInfo(asClient, requestId, pciUserName, null);
        //Send SSO request to PCI
        SendSsoRequest(asClient, requestId, pciUserName);
    }

    private bool PCIInMaintenanceMode()
    {
        bool isOffline = false;
        AS.Security.WS.Entities.RefTableValueCollection refs = WebServices.SecurityServices.GetRefTableValues(
          SessionManager.CurrentClient, "PCIMaintenanceMode", "ENG", SessionManager.CurrentLanguage);
        if (refs != null && refs.Count > 0)
        {
            string value = refs[0].RefTblCols[0];
            isOffline = !value.IsNullOrEmpty() && value.Equals(((int)PCIMaintenanceMode.Offline).ToString());
        }
        return isOffline;
    }


    private string SsoToPCIForNonUser(FilterParameterCollection paramRequest)
    {
        Guid requestId = Guid.NewGuid();
        // #26064: If user does not exist in PCI database, we have to create it and let him jump to PCI feature
        try
        {
            //  Create account for CS or Hierarchy Secondary users if not exist in PCI
            bool isSynchedToPci = SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS
                || (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Hierarchy && SessionManager.CurrentUser.UserSecRole.Contains("SEC"));

            if (!isSynchedToPci)
            {
                string message = GetLocalResourceObject("PCISecure_aspx_cs_CannotAccessHierarchy").ToString();
                //Writer log info to PCI SSO log file
                PciLogInfo(SessionManager.CurrentUser.ASClient, requestId, string.Format("{0} - Cannot Access PCI SsoToPCIForNonUser :UserHierarchyMode is not access - CurrentUserType={1}; UserSecRole={2}.", message, SessionManager.CurrentUserType, SessionManager.CurrentUser.UserSecRole));
                return message;
            }

            User currentUser = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID);
            if (currentUser == null)
            {
                string message = GetLocalResourceObject("PCISecure_aspx_cs_CannotAccessUserVWIsNull").ToString();
                //Writer log info to PCI SSO log file
                PciLogInfo(SessionManager.CurrentUser.ASClient, requestId, message);
                return message;
            }

            //  Create user on PCI
            GeneralFuncsLib.SyncUserWithPCI(currentUser, GeneralFuncsLib.GeneratePassword());
            var pciUserInfo = GetUserInfoInPci(paramRequest);
            if (pciUserInfo == null)
            {
                string message = GetLocalResourceObject("PCISecure_aspx_cs_CannotAccessNotUserWithPCI").ToString();
                //Writer log info to PCI SSO log file
                PciLogInfo(SessionManager.CurrentUser.ASClient, requestId, message);
                return message;
            }

            //SSO to PCI
            JumpToPCI(pciUserInfo.AsClientId, requestId, pciUserInfo.UserName);

            return PciConstants.SSO_SUCCESS;
        }
        catch (Exception ex)
        {
            LoggerManager.Error(string.Format("PCI Access SsoToPCIForNonUser: Client={0}; UserId={1}; RequestId={2}. \n", SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, requestId), ex);
            string message = GetLocalResourceObject("PCISecure_aspx_cs_CannotAccessException").ToString();
            //Writer log info to PCI SSO log file
            PciLogInfo(SessionManager.CurrentUser.ASClient, requestId, message);
            return message;
        }
    }
    private string SsoToPCIForExistUser(GetUserInPciInfo pciUserInfo)
    {
        Guid requestId = Guid.NewGuid();
        try
        {
            // UPdate PCI Role
            GeneralFuncsLib.SynchUserToPCI(pciUserInfo.UserInfo, pciUserInfo.UserId, pciUserInfo.AsClientId, pciUserInfo.UserName);
            //SSO to PCI
            JumpToPCI(pciUserInfo.AsClientId, requestId, pciUserInfo.UserName);

            return PciConstants.SSO_SUCCESS;
        }
        catch (Exception ex)
        {
            LoggerManager.Error(string.Format("PCI Access SsoToPCIForExistUser: Client={0}; UserId={1}; RequestId={2}. \n", SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, requestId), ex);
            string message = GetLocalResourceObject("PCISecure_aspx_cs_CannotAccessException").ToString();
            //Writer log info to PCI SSO log file
            PciLogInfo(SessionManager.CurrentUser.ASClient, requestId, pciUserInfo.UserName, message);
            return message;
        }
    }

    private void SendSsoRequest(int asClient, Guid requestId, string pciUserName)
    {
        var rawSaml = SsoPciFuncsLib.GenerateSamlContent(new GenerateSamlContentRequest
        {
            RequestId = requestId,
            CurrentUserId = pciUserName,
            ClientId = GetCurrentClientId(),
            AsClientId = asClient.ToString(),
            FullPathPciCertificateUrl = Server.MapPath(WebSiteSettings.PCI_Certificate_URL),
            PciCertificatePass = WebSiteSettings.PCI_Certificate_Pass
        });
        string html = SsoPciFuncsLib.GetSsoRequest(WebSiteSettings.PCI_SSO_URL, rawSaml);
        Response.Write(html);
    }
    private void PciLogInfo(int asClient, Guid requestId, string message = null)
    {
        SsoPciFuncsLib.PciLogInfo(new PciLogInfoRequest()
        {
            RequestId = requestId,
            CurrentUserId = GetCurrentUserId(),
            ClientId = GetCurrentClientId(),
            AsClientId = asClient.ToString(),
            PciSsoUrl = WebSiteSettings.PCI_SSO_URL,
            Message = message
        });
    }
    private void PciLogInfo(int asClient, Guid requestId, string pciUserName, string message)
    {
        SsoPciFuncsLib.PciLogInfo(new PciLogInfoRequest()
        {
            RequestId = requestId,
            CurrentUserId = pciUserName,
            ClientId = GetCurrentClientId(),
            AsClientId = asClient.ToString(),
            PciSsoUrl = WebSiteSettings.PCI_SSO_URL,
            Message = message
        });
    }

    public GetUserInPciInfo GetUserInfoInPci(FilterParameterCollection paramRequest)
    {
        var dtUser = PciWebServices.PciReportServices.GetReports("spa_SEC_GetUsers", paramRequest);
        if (dtUser == null || dtUser.Rows.Count <= 0)
        {
            return null;
        }
        Guid userId = new Guid(dtUser.Rows[0]["RecId"].ToString());
        int asClient = int.Parse(dtUser.Rows[0]["ASClient"].ToString());
        string userName = dtUser.Rows[0]["UserId"].ToString();

        return new GetUserInPciInfo
        {
            AsClientId = asClient,
            UserId = userId,
            UserName = userName,
            UserInfo = dtUser
        };
    }

    private static string GetCurrentClientId()
    {
        return GeneralFuncsLib.GetCurrentASClientId();
    }
    private static string GetCurrentUserId()
    {
        return SessionManager.CurrentUser.UserID;
    }
}
