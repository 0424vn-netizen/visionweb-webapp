using AS.Web.SharedSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class freeaccess_ManageSessionTimeout : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static bool IsActive()
    {
        return SharedProvider.IsActive();
    }
}