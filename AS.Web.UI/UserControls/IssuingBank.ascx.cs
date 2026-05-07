using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AS.Common.DBManager;

public partial class UserControls_IssuingBank : GlobalUserControl
{
    public long BinNumber { get; set; }
    public string BinNumberText
    {
        get
        {
            if (ViewState["BinNumberText"] != null)
                return ViewState["BinNumberText"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["BinNumberText"] = value;
        }
    }
        
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void Search_Click()
    {
        DataTable tbl = new DataTable();

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@BinNumber", BinNumber, DbType.String));
        tbl = WebServices.CsReportServices.GetReports("spa_GetIssuingBank", parameters);
        if (tbl == null || tbl.Rows.Count == 0)
        {
            ux4Report.Visible = false;
            uxMessage.Visible = true;
            return;
        }        
        if ( tbl.Rows.Count !=0)
        {
            uxMessage.Visible = false;
            ux4Report.Visible = true;
            uxIssuingBank4.DataSource = tbl;
            uxIssuingBank4.DataBind();
        }
    }
        

    protected string ProcessingText(object str)
    {
        string text = str.ToString();
        if (text != string.Empty)
        {
            text = text.Replace("\r\n", "<br />");
            if (text.Contains("Phone:"))
            {
                int phonefaxIndex = text.IndexOf("Phone:");
                text = text.Substring(0, phonefaxIndex);
            }
            else if (text.Contains("phone:"))
            {
                int phonefaxIndex = text.IndexOf("phone:");
                text = text.Substring(0, phonefaxIndex);
            }
        }
        return text;
    }

    protected string ProcessingPhone(object str)
    {
        string text = str.ToString();
        if (text != string.Empty)
        {
            string phonefax = "";
            if (text.Contains("Phone:"))
            {
                int phonefaxIndex = text.IndexOf("Phone:");
                phonefax = text.Substring(phonefaxIndex, text.Length - phonefaxIndex);
                phonefax = phonefax.Replace("\r\n", "<br />");

            }
            else if (text.Contains("phone:"))
            {
                int phonefaxIndex = text.IndexOf("phone:");
                phonefax = text.Substring(phonefaxIndex, text.Length - phonefaxIndex);
                phonefax = phonefax.Replace("\r\n", "<br />");
            }
            return phonefax;
        }
        return text;
    }
}
