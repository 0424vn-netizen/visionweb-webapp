using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Configuration;


using AS.Security.WS.Entities;
using AS.Security.WS.Business;
using AS.Common.Logger;
using AS.Security.WS.Mobile;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[Microsoft.Web.Services3.Policy("ServerPolicy")]
public partial class SecurityService : AS.Common.WSE.ASWebService //temporaryly inherite from ASWebServices to use _BasicPager
{
    readonly GenericServices _GenericService;
    readonly MembershipServices _MembershipService;
    readonly PermissionHierarchyServices _PermHierService;
    readonly SsoPermissionXlatServices _SSOPermissionXlatService;
    readonly JumpSiteServices _JumpSiteService;
    readonly SiteMapServices _SiteMapService;
    readonly int AsClientId;
    //Mobile security
    private readonly SecMobileService _secMobileService;
    
    public SecurityService()
    {
        if (Microsoft.Web.Services3.ResponseSoapContext.Current == null)
        {
            throw new UnauthorizedAccessException("Access Denied");
        }

        AsClientId = int.Parse(RequestHeaders["ClientId"]);
        _GenericService = new GenericServices(GeneralFuncsLib.GetConnStringSettings(AsClientId.ToString(), "SEC_DBCONN"));
        _secMobileService = new SecMobileService(GeneralFuncsLib.GetConnStringSettings(AsClientId.ToString(), "SEC_DBCONN"));
        _MembershipService = new MembershipServices(GeneralFuncsLib.GetConnStringSettings(AsClientId.ToString(), "SEC_DBCONN"));
        _PermHierService = new PermissionHierarchyServices(GeneralFuncsLib.GetConnStringSettings(AsClientId.ToString(), "SEC_DBCONN"));
        _SSOPermissionXlatService = new SsoPermissionXlatServices(GeneralFuncsLib.GetConnStringSettings(AsClientId.ToString(), "SEC_DBCONN"));
        _JumpSiteService = new JumpSiteServices(GeneralFuncsLib.GetConnStringSettings(AsClientId.ToString(), "SEC_DBCONN"));
        _SiteMapService = new SiteMapServices(GeneralFuncsLib.GetConnStringSettings(AsClientId.ToString(), "SEC_DBCONN"));
    }

    [WebMethod(Description = "Create a jump site ticket. Return a temp password for client")]
    public string CreateJumpSiteTicket(Guid userId, string remoteIP, int desSysId, Guid jumper)
    {
        try
        {
            
            long newId = 0;
            string jump_password = GeneratePassword(userId.ToString());
            _JumpSiteService.InsertJumpSite(userId, remoteIP, desSysId, jump_password, out newId, jumper);
            return jump_password;
        }
        catch (Exception ex)
        {
            LoggerManager.Error("CreateJumpSiteTicket:\n" + ex.ToString());
            return null;
        }
    }
    
    [WebMethod(Description = "Create a jump site ticket. Return a temp password for client")]
    public bool CheckJumpSiteTicket(Guid userId, Guid jumper, string tempPassword, string remoteIP, int desSysId, int clientId, string hostIP, string browser)
    {
        try
        {
            var jumsiteModel = new JumpSiteModel()
            {
                UserId = userId,
                Jumper = jumper,
                TempPassword = tempPassword,
                RemoteIp = remoteIP,
                DesSysId = desSysId,
                ClientId = clientId,
                HostIp = hostIP,
                Browser = browser,
            };
            bool ret = _JumpSiteService.CheckJumpSite(jumsiteModel);
            LoggerManager.Info("CheckJumpJumpSiteTicket:\r\n" + userId + "\t" + remoteIP + "\t" + desSysId + "\t" + tempPassword + "\t" + jumper);
            return ret;
            
        }
        catch (Exception ex)
        {
            LoggerManager.Error("CheckJumpJumpSiteTicket:\n" + ex.ToString() + "\r\n" + userId + "\t" + remoteIP + "\t" + desSysId + "\t" + tempPassword + "\t" + jumper);
            return false;
        }
    }

    [WebMethod(Description = "Retrieve the Partner records associated with PartnerName")]
    public Partner GetPartners(string partnerName, string clientID)
    {

        try
        {
            PartnerServices _PartnerServices= new PartnerServices(GeneralFuncsLib.GetConnStringSettings(clientID, "SEC_DBCONN"));
            PartnerCollection _PartnerCollection = _PartnerServices.GetPartners(partnerName);

            return _PartnerCollection[0];
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetPartners:\n" + ex.ToString());
            return null;
        }
    }

    [WebMethod(Description = "Retrieve the SSOPermissionXlat records associated with PartnerID")]
    public SsoPermissionXlatCollection GetSSOPermissionXlat(int partnerID, int clientID, int permissionlevel)
    {
        try
        {
            return _SSOPermissionXlatService.GetSSOPermissionXlat(partnerID, clientID, permissionlevel);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetSSOPermissionXlat:\n" + ex.ToString());
            return new SsoPermissionXlatCollection();
        }
    }

    string GeneratePassword(string salt)
    {
        return AS.Common.DataProtection.Cryptophy.SHA1(DateTime.Now.Ticks.ToString() + salt);
    }

    #region Mobile
    [WebMethod(Description = "Get mobile user information")]
    public MobileUser GetMobileUser(int clientId, string userId)
    {
        return _secMobileService.GetMobileUser(clientId, userId);
    }
    #endregion
}
