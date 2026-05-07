using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class freeaccess_ViewNotificationGate : NonReportPage
{
    protected string Module
    {
        get
        {
            if (IsStaticQueryString && SecureQueryString["Module"].IsNotNullData())
            {
                return SecureQueryString["Module"];
            }
            return string.Empty;
        }
    }

    protected string ASClientId
    {
        get
        {
            if (IsStaticQueryString && SecureQueryString["ASClientId"].IsNotNullData())
            {
                return SecureQueryString["ASClientId"];
            }
            return string.Empty;
        }
    }

    protected string DestinationURL
    {
        get
        {
            if (IsStaticQueryString && SecureQueryString["DesURL"].IsNotNullData())
            {
                return SecureQueryString["DesURL"];
            }
            return string.Empty;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated && SessionManager.IsLoggedIn && IsStaticQueryString)
        {
            string queryString = Cryptor.DecryptText(Request["sparam"], STATIC_KEY);
            switch (Module)
            {
                case "CMS":
                    Response.Redirect(DestinationURL + "&" + queryString);
                    break;
            }
        }
        else if (!ASClientId.IsNullOrEmpty() && !Request.QueryString.IsNullOrEmpty())
        {
            Response.Redirect(System.Web.Security.FormsAuthentication.LoginUrl + "?id=" + ASClientId + "&" + Request.QueryString.ToSafeString());
        }
    }
    
}