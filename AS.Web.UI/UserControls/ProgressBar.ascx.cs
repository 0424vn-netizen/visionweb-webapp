using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using Telerik.Web.UI;


public partial class ProgressBar : GlobalUserControl
{
    private decimal _Value = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptBlock(GetType(), "resource", "<script src=\"" + ResolveUrl("~/res/js/risk/ProgressBar.js") + "\"></script>");
        Page.ClientScript.RegisterStartupScript(GetType(), "init", "$(function(){initAllProgressBar();});", true);
        // Register event when sorting
        if (IsPostBack)
        {
            RadAjaxManager ajx = RadAjaxManager.GetCurrent(Page);
            if (ajx != null) ajx.ResponseScripts.Add("initAllProgressBar();");
        }

    }

    public decimal Value
    {
        get { return _Value; }
        set
        { _Value = value; }
    }

    public ProgressBar()
    {
    }

    public ProgressBar(decimal value)
    {
        this._Value = value;
    }
    protected string staticClass = "ProgressBarDiv";
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //_Value = Math.Round(_Value);

        if (_Value <= 25)
            ProgressBarArea.Attributes.Add("class", staticClass + " " + "progressbar-red");
        else if (_Value >= 26 && _Value <= 74)
            ProgressBarArea.Attributes.Add("class", staticClass + " " + "progressbar-yellow");
        else
            ProgressBarArea.Attributes.Add("class", staticClass + " " + "progressbar-green");

        if(_Value > 100)
            ProgressBarArea.Style.Add("width", "100%");
        else
            ProgressBarArea.Style.Add("width", string.Format("{0}%", _Value.ToString()));

        _Value = _Value / 100;
        NumberCell.InnerHtml = string.Format("{0}", _Value.ToString("#0.00%"));

    }
}
