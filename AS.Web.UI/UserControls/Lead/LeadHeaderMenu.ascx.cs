using AS.Controls.Pages;
using AS.Core.Common;
using AS.Leads.UserMaintService;
using AS.Security.WS.Entities;
using AS.VW.Share.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Core.Common.Utilities;
using System.Reflection;
using System.Linq;
using AS.Controls.ASP.Net;

public partial class UserControls_Lead_LeadHeaderMenu : HeaderMenuUserControl
{
    protected override void LoadMenu(ASMenu menu)
    {
        string logoText = string.Empty;
        string config = GeneralFuncsLib.GetDataOfExtendedSetting(WebSiteConstants.SHOW_LOGO_DESCRIPTION);
        const string BANK_NAME = "BankName";

        if (SessionManager.CurrentUser != null)
        {
            if ((SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS && config.Contains("MS"))
                || SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS && config.Contains("CS"))
            {
                UserInfo userInfo = new UserInfo();
                userInfo.AsClientId = SessionManager.CurrentUser.ASClient;
                userInfo.SiteId = SessionManager.CurrentUser.SiteID;
                userInfo.UserId = SessionManager.CurrentUser.UserID;
                userInfo.UserMode = String.Empty;
                userInfo.UserSessionId = SessionManager.UniqueSessionID;
                userInfo.RecId = SessionManager.CurrentUser.RecId;

                UserMaintBusiness userMaintBusiness = new UserMaintBusiness(userInfo);
                DataSet ds = userMaintBusiness.GetAllBanks("", "");
                if (ds.Tables.Count > 0 && ds.Tables[0].HasData() && ds.Tables[0].Columns.Contains(BANK_NAME))
                {
                    logoText = ds.Tables[0].Rows[0][BANK_NAME].ToString();
                }

                menu.LogoText = logoText;
            }
        }
        
    }
}