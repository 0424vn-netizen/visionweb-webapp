using AS.VW.Scheduler.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnTest_Click(object sender, EventArgs e)
    {
        ExtractMerchant t = new ExtractMerchant();
        t.DoGenerateFiles();
    }
}