using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Controls.Pages;

/// <summary>
/// Summary description for SecuredUserControl
/// </summary>
public class GlobalUserControl: UserControl
{
    public GlobalUserControl()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public SecurePage Page
    {
        get
        {
            return (SecurePage)base.Page;
        }
    }
    public ReportPage ReportPage
    {
        get
        {
            return (ReportPage)base.Page;
        }
    }

    #region Common functions

    protected virtual void OnDataBindControls(Enum type, object sender) { }
    protected void OnDataBindControls(Enum type) { OnDataBindControls(type, null); }
    protected virtual void OnPostBackActions(Enum type, object sender) { }
    protected void OnPostBackActions(Enum type) { OnPostBackActions(type, null); }
    protected virtual bool OnValidateInputs(Enum type, object sender) { return true; }
    protected bool OnValidateInputs(Enum type) { return OnValidateInputs(type, null); }

    #endregion
}
