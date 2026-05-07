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
using AS.Common.DBManager;

public partial class UserControls_Risk_ApprovalDate : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        LoadNewMerchantAndEstablishedMerchant
    }
    enum PostBackAction { }
    #endregion

    public WebSiteEnums.FeatureMode FeatureMode { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //BindData();
            OnDataBindControls(DataBindAction.LoadNewMerchantAndEstablishedMerchant);
            VisibleControls();
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadNewMerchantAndEstablishedMerchant:
                BindData();
                break;
        }
    }

    private void VisibleControls()
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {

            cbNewMerchantOnly.OnClientDropDownOpening = "CancelDropDown";
            cbEstablishedMerchants.OnClientDropDownOpening = "CancelDropDown";

            chkIsNewMerchantOnly.Enabled = false;
            chkIsEstablishedMerchants.Enabled = false;
        }
    }

    private DataTable GetDataTable()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32));
        parameters.Add(new FilterParameter("@FilterMode", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_rm_cs_GetApprovalDateByFilter", parameters);
    }

    private void BindData()
    {
        //Bind data for comboxbox new merchant only
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        cbNewMerchantOnly.DataTextField = "RefTblCol1";
        cbNewMerchantOnly.DataValueField = "RefTblKey";
        parameters.Add(new FilterParameter("@ApprovalDateType", true, DbType.Boolean));
        parameters.AddLanguageID();
        cbNewMerchantOnly.DataSource = WebServices.RiskServices.GetReports("spa_rm_cs_GetApprovalDate", parameters);
        cbNewMerchantOnly.DataBind();

        //Bind data for combobox established merchant
        FilterParameterCollection parametersx = new FilterParameterCollection();
        parametersx.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        cbEstablishedMerchants.DataTextField = "RefTblCol1";
        cbEstablishedMerchants.DataValueField = "RefTblKey";
        parametersx.Add(new FilterParameter("@ApprovalDateType", false, DbType.Boolean));
        parametersx.AddLanguageID();
        cbEstablishedMerchants.DataSource = WebServices.RiskServices.GetReports("spa_rm_cs_GetApprovalDate", parametersx);
        cbEstablishedMerchants.DataBind();
        DataTable dt = this.GetDataTable();
        if (dt.Rows.Count > 0)
        {
            string newMerchant = dt.Rows[0]["NewMerchant"].ToString();
            string establishedMerchant = dt.Rows[0]["EstablishedMerchant"].ToString();

            if (!string.IsNullOrEmpty(newMerchant) && !newMerchant.Equals("0"))
            {
                this.chkIsNewMerchantOnly.Checked = true;
                this.cbNewMerchantOnly.Enabled = true;
                this.cbNewMerchantOnly.SelectedValue = newMerchant;
            }
            if (!string.IsNullOrEmpty(establishedMerchant) && !establishedMerchant.Equals("0"))
            {
                this.chkIsEstablishedMerchants.Checked = true;
                this.cbEstablishedMerchants.Enabled = true;
                this.cbEstablishedMerchants.SelectedValue = establishedMerchant;
            }
        }
    }

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    public int Save()
    {
        bool isApprovalDate = false;
        bool isNewMerchant = false;
        bool isEstablishedMerchant = false;
        int valueNewMerchant = 0;
        int valueEstablishMerchant = 0;
        FilterParameterCollection parameter = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameter.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameter.Add(new FilterParameter("@PrimaryID", int.Parse(this.PrimaryID), DbType.Int32));
        if (chkIsNewMerchantOnly.Checked == true || chkIsEstablishedMerchants.Checked == true)
        {
            isApprovalDate = true;
            if (chkIsNewMerchantOnly.Checked == true)
            {
                isNewMerchant = true;
                valueNewMerchant = Int32.Parse(cbNewMerchantOnly.SelectedValue.ToString());
            }
            if (chkIsEstablishedMerchants.Checked == true)
            {
                isEstablishedMerchant = true;
                valueEstablishMerchant = Int32.Parse(cbEstablishedMerchants.SelectedValue.ToString());
            }
        }
        parameter.Add(new FilterParameter("@IsNewMerchant", isNewMerchant, DbType.Boolean));
        parameter.Add(new FilterParameter("@IsEstablishedMerchant", isEstablishedMerchant, DbType.Boolean));
        parameter.Add(new FilterParameter("@Value_NewMerchant", valueNewMerchant, DbType.Int32));
        parameter.Add(new FilterParameter("@Value_EstablishedMerchant", valueEstablishMerchant, DbType.Int32));
        parameter.Add(new FilterParameter("@isApprovalDate", isApprovalDate, DbType.Boolean));
        parameter.Add(new FilterParameter("@FilterMode", this.Mode, DbType.Int32));
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_InsertUpdateApprovalDate", parameter, out parameterOut);
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
}
