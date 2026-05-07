using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditASUserConfirmModal : NonReportPage
{

    WebSiteEnums.ASUserAction action
    {
        get
        {
            if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["action"]))
                return (WebSiteEnums.ASUserAction)Enum.Parse(typeof(WebSiteEnums.ASUserAction), SecureQueryString["action"].ToString());
            else
                return WebSiteEnums.ASUserAction.None;
        }
    }

    string EditUserName
    {
        get
        {
            if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["u"]))
                return SecureQueryString["u"].Trim();
            else
                return string.Empty;
        }
    }
    string ControlID
    {
        get
        {
            if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["ControlID"]))
                return SecureQueryString["ControlID"].Trim();
            else
                return string.Empty;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        switch (action)
        {
            case WebSiteEnums.ASUserAction.Edit:
                {
                    Page.Title = (GetLocalResourceObject("EditModalTitleResource").ToString());
                    ltAreYouSure.Text = string.Format(GetLocalResourceObject("ltAreYouSureResource.Text").ToString(), EditUserName);
                    uxSave.OnClientClick = "parent.SaveChanged(); return false;";
                }
                break;
            case WebSiteEnums.ASUserAction.Delete:
                {
                    Page.Title = (GetLocalResourceObject("DeleteModalTitleResource").ToString());
                    ltAreYouSure.Text = string.Format(GetLocalResourceObject("ltAreYouSureDeleteResource.Text").ToString(), EditUserName);
                    uxSave.OnClientClick = "parent.DeleteUser(); return false;";
                    uxSave.Text = GetLocalResourceObject("uxSaveResourceDelete.Text").ToString();
                }
                break;
            case WebSiteEnums.ASUserAction.Active:
                {
                    Page.Title = (GetLocalResourceObject("ActivateUserModalTitleResource").ToString());
                    ltAreYouSure.Text = string.Format(GetLocalResourceObject("ltAreYouSureActiveResource.Text").ToString(), EditUserName);
                    uxSave.OnClientClick = string.Format("parent.validateActive('{0}'); return false;", ControlID);
                    uxSave.Text = GetLocalResourceObject("uxSaveResourceActive.Text").ToString();
                }
                break;
            case WebSiteEnums.ASUserAction.Deactive:
                {
                    Page.Title = (GetLocalResourceObject("DeactivateUserModalTitleResource").ToString());
                    ltAreYouSure.Text = string.Format(GetLocalResourceObject("ltAreYouSureDeactiveResource.Text").ToString(), EditUserName);
                    uxSave.OnClientClick = string.Format("parent.validateActive('{0}'); return false;", ControlID);
                    uxSave.Text = GetLocalResourceObject("uxSaveResourceDeactive.Text").ToString();
                }
                break;
        }

    }
}