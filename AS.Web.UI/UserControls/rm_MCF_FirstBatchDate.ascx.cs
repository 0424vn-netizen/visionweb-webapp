using System;
using System.Data;
using AS.Common.DBManager;

public partial class UserControls_rm_MCF_FirstBatchDate : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        LoadNewMerchantAndEstablishedMerchant
    }
    enum PostBackAction { }
    #endregion

    public WebSiteEnums.FeatureMode FeatureMode { get; set; }

    public bool IsAssignmentPage { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !(Mode == WebSiteEnums.ParamFilterMode.Assignment))
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

    public void VisibleControls()
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {

            cbNewMerchantOnly1.OnClientDropDownOpening = "CancelDropDown";
            cbEstablishedMerchants1.OnClientDropDownOpening = "CancelDropDown";

            chkIsNewMerchantOnly1.Enabled = false;
            chkIsEstablishedMerchants1.Enabled = false;
        }
    }

    private DataTable GetDataTable()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32));
        parameters.Add(new FilterParameter("@FilterMode", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_Get_AssignmentFilter", parameters);
    }

    private void BindData()
    {
        //Bind data for comboxbox new merchant only
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        cbNewMerchantOnly1.DataTextField = "DataText";
        cbNewMerchantOnly1.DataValueField = "DataKey";
        parameters.Add(new FilterParameter("@ItemCode", "FirstBatchDateNewMerchantOnly", DbType.String));
        parameters.AddLanguageID();
        cbNewMerchantOnly1.DataSource = WebServices.RiskServices.GetReports("spa_REF_RM_MCF_Get_ItemValue", parameters);
        cbNewMerchantOnly1.DataBind();

        //Bind data for combobox established merchant
        FilterParameterCollection parametersx = new FilterParameterCollection();
        parametersx.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        cbEstablishedMerchants1.DataTextField = "DataText";
        cbEstablishedMerchants1.DataValueField = "DataKey";
        parametersx.Add(new FilterParameter("@ItemCode", "FirstBatchDateEstablishedMerchants", DbType.String));
        parametersx.AddLanguageID();
        cbEstablishedMerchants1.DataSource = WebServices.RiskServices.GetReports("spa_REF_RM_MCF_Get_ItemValue", parametersx);
        cbEstablishedMerchants1.DataBind();
        DataTable dt = this.GetDataTable();
        if (dt.Rows.Count > 0)
        {
            string newMerchant = dt.Rows[0]["NewMerchant1"].ToString();
            string establishedMerchant = dt.Rows[0]["EstablishedMerchant1"].ToString();

            if (!string.IsNullOrEmpty(newMerchant) && !newMerchant.Equals("0"))
            {
                this.chkIsNewMerchantOnly1.Checked = true;
                this.cbNewMerchantOnly1.Enabled = true;
                this.cbNewMerchantOnly1.SelectedValue = newMerchant;
            }
            if (!string.IsNullOrEmpty(establishedMerchant) && !establishedMerchant.Equals("0"))
            {
                this.chkIsEstablishedMerchants1.Checked = true;
                this.cbEstablishedMerchants1.Enabled = true;
                this.cbEstablishedMerchants1.SelectedValue = establishedMerchant;
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
        bool isFistBatchDate = false;
        bool isNewMerchant = false;
        bool isEstablishedMerchant = false;
        int valueNewMerchant = 0;
        int valueEstablishMerchant = 0;
        FilterParameterCollection parameter = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameter.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameter.Add(new FilterParameter("@AssignmentID", int.Parse(this.PrimaryID), DbType.Int32));
        if (chkIsNewMerchantOnly1.Checked == true || chkIsEstablishedMerchants1.Checked == true)
        {
            isFistBatchDate = true;
            if (chkIsNewMerchantOnly1.Checked == true)
            {
                isNewMerchant = true;
                valueNewMerchant = Int32.Parse(cbNewMerchantOnly1.SelectedValue.ToString());
            }
            if (chkIsEstablishedMerchants1.Checked == true)
            {
                isEstablishedMerchant = true;
                valueEstablishMerchant = Int32.Parse(cbEstablishedMerchants1.SelectedValue.ToString());
            }
        }
        parameter.Add(new FilterParameter("@IsNewMerchant1", isNewMerchant, DbType.Boolean));
        parameter.Add(new FilterParameter("@IsEstablishedMerchant1", isEstablishedMerchant, DbType.Boolean));
        parameter.Add(new FilterParameter("@NewMerchant1", valueNewMerchant, DbType.Int32));
        parameter.Add(new FilterParameter("@EstablishedMerchant1", valueEstablishMerchant, DbType.Int32));
        parameter.Add(new FilterParameter("@FilterMode", this.Mode, DbType.Int32));
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_Update_AssignmentFilter_FirstBatchDate", parameter, out parameterOut);
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion

    public void Rebind()
    {
        OnDataBindControls(DataBindAction.LoadNewMerchantAndEstablishedMerchant);
        VisibleControls();
    }
}
