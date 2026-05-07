using System;
using AS.Common.Logger;
using System.Data;
using AS.Common.DBManager;
using AS.WEB.UI.SamlSign;
using AS.Security.WS.Entities;
using System.Web;

namespace As.VisionWeb.Web
{
    public partial class SingleSignOn : SamlS
    {
        protected override bool ProcessUser()
        {
            LogDebug(string.Format("SAML: ProcessUser: {0}-{1}", clientID, userName));
            Session.Clear();
            SessionManager.UniqueSessionID = Guid.NewGuid().ToString();
            SessionManager.CurrentClient = clientID;

            var redirectURL = GeneralFuncsLib.GetDataOfExtendedSetting("SSO_Redirecting_To_URL", clientID.ToString());
            if (!string.IsNullOrEmpty(redirectURL))
            {
                HttpContext.Current.Response.Redirect(redirectURL, false);
                LogDebug(string.Format("SAML: SSO_Redirecting_To_URL: {0}", redirectURL));
                return true;
            }

            // Create a login context for the asserted identity.
            if ((user = WebServices.SecurityServices.GetUser(clientID, userName)) == null)
            {
                ProcessResultMessage = string.Format("(SAML: {0}-{1}) does not exist.", clientID, userName);
                LogDebug(ProcessResultMessage);
                return false;
            }
            
            /*
             *  #56312: Deactivated users can access to VisionWeb. SSO 
            */
            if (GeneralFuncsLib.ValidateSSOUser(clientID, userName) == 0)
            {
                ProcessResultMessage = string.Format("{0}-{1}: Deactivated users are not allow to access VisionWeb.", clientID, userName);
                LogDebug(ProcessResultMessage);
                return false;
            }

            return ProcessAutomatedLogin();
        }

        // process the automated login
        protected bool ProcessAutomatedLogin()
        {
            var result = false;

            //Add mobile redirect check first
            var redirectMobileUrl = GetMobileRedirect(user.ASClient, user.UserID);
            if (!string.IsNullOrEmpty(redirectMobileUrl))
            {
                HttpContext.Current.Response.Redirect(redirectMobileUrl, false);
                LogDebug(string.Format("(SAML: Redirect to mobile: {0}", redirectMobileUrl));
                return true;
            }
            
            SessionManager.SingleSignOnMenuMod = true;
            SessionManager.SSOSecurityLevel = permissionLevel;
            if (GeneralFuncsLib.SaveLoginUserData(user.UserID, this))
            {
                switch ((SSODestinationType)Enum.ToObject(typeof(SSODestinationType), destinationID))
                {
                    case SSODestinationType.MS:
                        {
                            //Check Use primary prefix
                            SessionManager.UsePrimaryPrefix = CheckUsePrimaryPrefix();
                            string redirectUrl = GeneralFuncsLib.GetDefaultPageOfLoggedInUser((AS.Controls.Pages.SecurePage)this.Page);

                            if (SessionManager.CurrentUserType.ToString() == WebSiteEnums.UserHierarchyMode.Hierarchy.ToString())
                                GetHierarchy();                          
                            
                            //Save Timezone
                            if (!GeneralFuncsLib.GetCookie("ClientTimezone").IsNullOrEmpty() 
                                && !GeneralFuncsLib.GetCookie("DayLightSaving").IsNullOrEmpty())
                            {
                                TimeZoneHandler.SaveTimeZone(null, string.Empty, GeneralFuncsLib.GetCookie("ClientTimezone"), GeneralFuncsLib.GetCookie("DayLightSaving").ToInt());
                            }
                            else
                            {
                                WriteLogTimeZone(user.UserID, user.ASClient);
                            }

                            //Reload header menu 
                            SessionManager.GetHeaderMenu();
                            //Sprint 6 - 46652 - AW Multi-currency Transaction Display
                            GeneralFuncsLib.SetDefaultCurrency();
                            HttpContext.Current.Response.Redirect(redirectUrl, false);
                            result = true;
                        }
                        break;
                    case SSODestinationType.PCI:
                        result = ProcessPCIApplication();
                        break;
                    case SSODestinationType.Total:
                        ProcessTotalSsoApplication();
                        result = true;
                        break;
                    default:
                        ProcessResultMessage = string.Format("Destination Application was invalid. DestinationID={0}", destinationID);
                        LogDebug(ProcessResultMessage);
                        result = false;
                        break;
                }
            }
            else
            {
                ProcessResultMessage = string.Format("Username was invalid. UserId={0}; ClientId={1}", user.UserID, user.ASClient);
                LogDebug(ProcessResultMessage);
                result = false;
            }

            return result;
        }
        private bool ProcessPCIApplication()
        {
            if (this.IsUserWithPermission("SiteAccessPCIAdmin") 
                || this.IsUserWithPermission("HierarchySiteAccessPCIAdmin") 
                || this.IsUserWithPermission("MerchantSiteAccessPCIAdmin"))
            {
                if (PCIInMaintenanceMode())
                {
                    ClientScript.RegisterStartupScript(GetType(), "CloseModal", "<script language=\"javascript\">openPopupWindow('PCIMaintenanceModal.aspx','PCI Maintenance Modal', 450, 225);</script>", true);
                    return true;
                }

                bool isShowMsg = false;
                FilterParameterCollection _params = GeneralFuncsLib.GetMasterMerchant(out isShowMsg);
                if (isShowMsg)
                {
                    ProcessResultMessage = string.Format("Your Master Merchant Account is inactive, therefore PCI site jump is not available. Entity={0}", SessionManager.CurrentUser.EntityID);
                    LogDebug(ProcessResultMessage);
                    return false;
                }

                DataTable dtUser = PciWebServices.PciReportServices.GetReports("spa_SEC_GetUsers", _params);
                if (dtUser.Rows.Count > 0)
                {
                    Guid userid = new Guid(dtUser.Rows[0]["RecId"].ToString());
                    int asclient = int.Parse(dtUser.Rows[0]["ASClient"].ToString());
                    string username = dtUser.Rows[0]["UserId"].ToString();
                    string temppassword = GeneralFuncsLib.GeneratePassword();

                    // UPdate PCI Role
                    GeneralFuncsLib.SynchUserToPCI(dtUser, userid, asclient, username);
                    //Save Timezone
                    if (!GeneralFuncsLib.GetCookie("ClientTimezone").IsNullOrEmpty() && !GeneralFuncsLib.GetCookie("DayLightSaving").IsNullOrEmpty())
                    {
                        TimeZoneHandler.SaveTimeZone(null, string.Empty, GeneralFuncsLib.GetCookie("ClientTimezone"), GeneralFuncsLib.GetCookie("DayLightSaving").ToInt());
                    }
                    else
                    {
                        WriteLogTimeZone(username, asclient);
                    }
                    return JumpToPCI(userid, asclient, username, temppassword);
                }
            }
            else
            {
                ProcessResultMessage = string.Format("{0}-{1}: User does not have PCI permission.", SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID);
                LogDebug(ProcessResultMessage);
                return false;
            }
            return true;
        }
        private bool PCIInMaintenanceMode()
        {
            bool isOffline = false;
            RefTableValueCollection refs = WebServices.SecurityServices.GetRefTableValues(SessionManager.CurrentClient,
                "PCIMaintenanceMode", "ENG",
                SessionManager.CurrentLanguage);

            if (refs != null && refs.Count > 0)
            {
                string value = refs[0].RefTblCols[0];
                isOffline = !value.IsNullOrEmpty() && value.Equals(((int)PCIMaintenanceMode.Offline).ToString());
            }
            return isOffline;
        }
        private bool JumpToPCI(Guid userid, int asclient, string username, string temppassword)
        {
            FilterParameterCollection _param = new FilterParameterCollection();
            _param.Add(new FilterParameter("@UserId", userid, DbType.Guid));
            _param.Add(new FilterParameter("@RemoteIP", Request.UserHostAddress, DbType.AnsiString)); //"192.168.119.148"
            _param.Add(new FilterParameter("@DesSysId", 2, DbType.Int32));
            _param.Add(new FilterParameter("@TempPassword", temppassword, DbType.AnsiString));
            _param.Add(new FilterParameter("@Jumper", SessionManager.CurrentUser.RecId, DbType.Guid));
            _param.Add(new FilterParameter("@RedId", 0, DbType.Int64, true));
            FilterParameterCollection _outParam = new FilterParameterCollection();
            _outParam.Add(new FilterParameter("@RedId", 0, DbType.Int64, true));
            PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_InsJumpSite", _param, out _outParam);
            string k = _outParam[0].ParameterValue.ToString();

            if (k != null)
            {
                string gateUrl = WebSiteSettings.NPC_PCI_JumpUrl + "?u=" + Server.UrlEncode(AS.Common.DataProtection.Cryptophy.EncryptText(username))
                    + "&cs=1&j=" + Server.UrlEncode(AS.Common.DataProtection.Cryptophy.EncryptText(SessionManager.CurrentUser.RecId.ToString())) + "&k=" + temppassword
                    + "&type=13" + "&cl=" + asclient;
                HttpContext.Current.Response.Redirect(gateUrl, false);
                return true;
            }
            else
            {
                ProcessResultMessage = string.Format("{0}-{1}: Can not create PCI ticket. Password={2}", asclient, userid, temppassword);
                LogDebug(ProcessResultMessage);
                return false;
            }
        }
        private void ProcessTotalSsoApplication()
        {
            //Save Timezone
            if (!GeneralFuncsLib.GetCookie("ClientTimezone").IsNullOrEmpty() && !GeneralFuncsLib.GetCookie("DayLightSaving").IsNullOrEmpty())
            {
                TimeZoneHandler.SaveTimeZone(null, string.Empty, GeneralFuncsLib.GetCookie("ClientTimezone"), GeneralFuncsLib.GetCookie("DayLightSaving").ToInt());
            }
            else
            {
                WriteLogTimeZone();
            }

            //Reload header menu 
            SessionManager.GetHeaderMenu();

            HttpContext.Current.Response.Redirect("~/Dashboard.aspx", false);
        }
        private void WriteLogTimeZone(string userid = null, int? clientId = null)
        {

            string clientTimezone = GeneralFuncsLib.GetCookie("ClientTimezone").IsNotNullData() ? GeneralFuncsLib.GetCookie("ClientTimezone") : "";
            string dayLightSaving = GeneralFuncsLib.GetCookie("DayLightSaving").IsNotNullData() ? GeneralFuncsLib.GetCookie("DayLightSaving") : "";

            LoggerManager.Warn(string.Format("Cookie clienttimezone is null! UserId={0}; ClientId={1}; ClientTimezone={2}; DayLightSaving={3}"
                , userid != null ? userid : SessionManager.CurrentUser.UserID, clientId != null ? clientId : SessionManager.CurrentUser.ASClient, clientTimezone, dayLightSaving));
           
        }
        protected void ModifySpecificUserPermission(string permissionStr, bool IsExcluded)
        {
            if (string.IsNullOrEmpty(permissionStr))
                return;

            if (IsExcluded)  // subtract the permission if it is there
            {
                if (SessionManager.CurrentUserPermissions.Contains(permissionStr))
                    SessionManager.CurrentUserPermissions = SessionManager.CurrentUserPermissions.Replace("," + permissionStr + ",", ",");

            }
            else    // add the permission if it's not already there
            {
                if (!(SessionManager.CurrentUserPermissions.Contains(permissionStr)))
                    SessionManager.CurrentUserPermissions += permissionStr + ",";
            }
        }
        protected bool CheckUsePrimaryPrefix()
        {
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
            paras.Add(new FilterParameter("@UserName", SessionManager.CurrentUser.UserID, DbType.AnsiString));
            paras.Add(new FilterParameter("@SystemId", SessionManager.CurrentSystem, DbType.Int32));
            DataTable tb = WebServices.SecurityServices.GetReports("spa_SEC_GetHierarchysForUser", paras);
            if (tb != null && tb.Rows.Count > 0)
            {
                if (tb.Rows[0]["UsePrimaryPrefix"].ToString() == "True")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        protected void GetHierarchy()
        {
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.AddLoggedInUserReportingParams(false);
            paras.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.AnsiString));
            DataTable dt = WebServices.MsReportServices.GetReports("spa_ms_GetHierarchyName", paras);

            if (dt != null && dt.Rows.Count > 0
                && dt.Rows[0]["EntityName"] != DBNull.Value && !dt.Rows[0]["EntityName"].ToString().IsNullOrEmpty())
            {
                SessionManager.HierarchyName = dt.Rows[0]["EntityName"].ToString();
            }
        }
        protected override void ProcessSamlResponseFail()
        {
            uxPlhLoading.Visible = false;
            uxPlhInfo.Visible = true;
        }
        private string GetMobileRedirect(int clientId, string userName)
        {
            if (MobileRedirectHandler.CheckMobileRedirect(clientId, WebSiteSettings.DefaultSystem, userName))
            {
                return MobileRedirectHandler.GetMobileRedirectUrlForSSO(clientId, userName);
            }
            return string.Empty;
        }
        public enum SSODestinationType
        {
            MS = 1,
            PCI = 2,
            None = 3,
            Total = 4,
        }
        private enum PCIMaintenanceMode
        {
            Offline = 1,
            Online = 0,
        }
    }
}