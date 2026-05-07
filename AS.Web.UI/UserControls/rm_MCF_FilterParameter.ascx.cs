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
using Telerik.Web.UI;
using AS.Common.DBManager;
using AS.Common;
using System.Collections.Generic;

public partial class UserControls_rm_MCF_FilterParameter : GlobalUserControl, IRiskParamFilter
{
    DataTable source;
    /// <summary>
    /// Contain the RiskParameter informations for generate to client browser
    /// </summary>
    private List<string> ParameterInfo
    {
        get
        {

            if (Session[this.ClientID + "_ParameterInfo"] == null)
                Session[this.ClientID + "_ParameterInfo"] = new List<string>();

            return (List<string>)Session[this.ClientID + "_ParameterInfo"];
        }
        set
        {
            Session[this.ClientID + "_ParameterInfo"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            source = new DataTable();
            source = GetParameterList();
            if (source != null && source.Rows.Count > 0)
            {
                uxCheckAll.Attributes["onclick"] = "chkAllParameters(this)";
                ParameterInfo.Clear();
                uxParameterRepeater.DataSource = source;
                uxParameterRepeater.DataBind();

            }
            else
            {
                uxCheckAll.Visible = false;
                uxMessage.Visible = true;
                pnlMessage.Visible = true;
                uxMessage.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("Risk_FilterParameter_ascx_cs_NoDataFound").ToString());
            }
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        string paramInfo = "ParameterInfo = [];";
        foreach (string info in this.ParameterInfo)
        {
            paramInfo += "ParameterInfo.push(" + info + ");";
        }

        this.Page.ClientScript.RegisterStartupScript(
            this.GetType(), "jsParameterInfo", paramInfo, true);

    }

    private DataTable GetParameterList()
    {
        FilterParameterCollection paramIns = new FilterParameterCollection();
        paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramIns.Add(new FilterParameter("@PrimaryID", this.PrimaryID, DbType.Int32));
        paramIns.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        paramIns.AddLanguageID();
        source = WebServices.RiskServices.GetReports("spa_RM_MCF_GetFilterParameterList", paramIns);
        return source;
    }

    protected void rptParameterRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Literal ltIndex = e.Item.FindControl("ltIndex") as Literal;
        Literal ltParameterName = e.Item.FindControl("ltParameterName") as Literal;
        Literal ltParameterDescription = e.Item.FindControl("ltParameterDescription") as Literal;
        Literal ltGroupName = e.Item.FindControl("ltGroupName") as Literal;
        CheckBox checkBox = e.Item.FindControl("checkBoxID") as CheckBox;
        HiddenField hdParameterKey = e.Item.FindControl("hdParameterKey") as HiddenField;
        PlaceHolder uxSpecialPanel = e.Item.FindControl("uxSpecialPanel") as PlaceHolder;

        switch (e.Item.ItemType)
        {
            case ListItemType.Item:
            case ListItemType.AlternatingItem:
                DataRowView row = e.Item.DataItem as DataRowView;
                string group = row["GroupName"].ToString();
                if (!group.Equals(previousGroupName.Value))
                {
                    ltGroupName.Text = VeraCodeSolution.DoVeraCode(row["GroupName"].ToString());
                    previousGroupName.Value = VeraCodeSolution.DoVeraCode(row["GroupName"].ToString());
                    uxSpecialPanel.Visible = true;
                }
                else
                {
                    uxSpecialPanel.Visible = false;
                }

                hdParameterKey.Value = VeraCodeSolution.DoVeraCode(row["ParameterKey"].ToString());
                ltIndex.Text = VeraCodeSolution.DoVeraCode(row["ParameterKey"].ToString());
                ltParameterName.Text = VeraCodeSolution.DoVeraCode(row["ParameterName"].ToString());
                ltParameterDescription.Text= VeraCodeSolution.DoVeraCode(row["ParameterDescription"].ToString());

                checkBox.Attributes["onclick"] = "checkBoxClick(this," + '"' + row["ParameterKey"].ToString() + '"' + ")";
                this.ParameterInfo.Add(string.Format("{{ CbClientId:\"{0}\", ParamKey:\"{1}\" }}", checkBox.ClientID, hdParameterKey.Value));
                break;
        }
    }

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    /// <summary>
    /// save list parameterkeys into db
    /// </summary>
    /// <returns>0 if success</returns>
    public int Save()
    {
        string paramkey = hdParamSelected.Value;
        FilterParameterCollection paramIns = new FilterParameterCollection();
        paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramIns.Add(new FilterParameter("@PrimaryID", this.PrimaryID, DbType.Int32));
        paramIns.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        paramIns.Add(new FilterParameter("@FilterValue", paramkey, DbType.String));
        FilterParameterCollection paramOuts = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveFilterParameterList", paramIns, out paramOuts);
        return 0;
    }

    public event EventHandler SelectedChanged;

  
    #endregion
}
