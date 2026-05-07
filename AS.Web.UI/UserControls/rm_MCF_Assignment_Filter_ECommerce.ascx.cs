using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Pages;
using System;
using System.Data;

public partial class UserControls_rm_MCF_Assignment_Filter_ECommerce : GlobalUserControl, IRiskParamFilter
{

    #region Enums
    enum DataBindAction
    {
        LoadDataFilter
    }

    #endregion

    #region ---- Propeties ----
    public bool Hide { get; set; }
    public WebSiteEnums.FeatureMode FeatureMode { get; set; }
    public WebSiteEnums.ParamFilterMode Mode { get; set; }
    public string PrimaryID { get; set; }
    public string ParamID { get; set; }
    public string FilterID { get; set; }
    #endregion ---- Propeties ----
    #region ---- Event ----
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowHideFieldConfig();
        if (!IsPostBack && !(Mode == WebSiteEnums.ParamFilterMode.Assignment))
        {
            if (!this.Hide)
                OnDataBindControls(DataBindAction.LoadDataFilter);
        }

        VisibleControls();
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"Assignment_Filter_ECommerce.loadControlEcomerce();"));
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadDataFilter:
                InitializeComboFilterType();

                DataTable dt = this.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    int? ECommercePercentFrom = string.IsNullOrEmpty(dt.Rows[0]["ECommercePercentFrom"].ToString()) ? null : (int?)dt.Rows[0]["ECommercePercentFrom"];
                    int? ECommercePercentTo = string.IsNullOrEmpty(dt.Rows[0]["ECommercePercentTo"].ToString()) ? null : (int?)dt.Rows[0]["ECommercePercentTo"];
                    int? KeyedPercentFrom = string.IsNullOrEmpty(dt.Rows[0]["KeyedPercentFrom"].ToString()) ? null : (int?)dt.Rows[0]["KeyedPercentFrom"];
                    int? KeyedPercentTo = string.IsNullOrEmpty(dt.Rows[0]["KeyedPercentTo"].ToString()) ? null : (int?)dt.Rows[0]["KeyedPercentTo"];
                    int? SwipedPercentFrom = string.IsNullOrEmpty(dt.Rows[0]["SwipedPercentFrom"].ToString()) ? null : (int?)dt.Rows[0]["SwipedPercentFrom"];
                    int? SwipedPercentTo = string.IsNullOrEmpty(dt.Rows[0]["SwipedPercentTo"].ToString()) ? null : (int?)dt.Rows[0]["SwipedPercentTo"];

                    //uxECommerce
                    if (uxECommerce.Visible && (ECommercePercentFrom != null || ECommercePercentTo != null))
                    {
                        uxchkEcommerce.Checked = true;
                        if (ECommercePercentFrom == null)
                        {
                            uxCboECommerceType.SelectedValue = "GreaterThan";
                            uxECommerceFrom.Value = ECommercePercentTo;
                        }
                        else if (ECommercePercentTo == null)
                        {
                            uxCboECommerceType.SelectedValue = "LessThan";
                            uxECommerceFrom.Value = ECommercePercentFrom;
                        }
                        else
                        {
                            uxCboECommerceType.SelectedValue = "Between";
                            uxECommerceFrom.Value = ECommercePercentFrom;
                            uxECommerceTo.Value = ECommercePercentTo;
                        }
                    }
                    //uxKeyed
                    if (uxKeyed.Visible && (KeyedPercentFrom != null || KeyedPercentTo != null))
                    {
                        uxchkKeyed.Checked = true;
                        if (KeyedPercentFrom == null)
                        {
                            uxCboKeyedType.SelectedValue = "GreaterThan";
                            uxKeyedFrom.Value = KeyedPercentTo;
                        }
                        else if (KeyedPercentTo == null)
                        {
                            uxCboKeyedType.SelectedValue = "LessThan";
                            uxKeyedFrom.Value = KeyedPercentFrom;
                        }
                        else
                        {
                            uxCboKeyedType.SelectedValue = "Between";
                            uxKeyedFrom.Value = KeyedPercentFrom;
                            uxKeyedTo.Value = KeyedPercentTo;
                        }
                    }
                    //uxSwiped
                    if (uxSwiped.Visible && (SwipedPercentFrom != null || SwipedPercentTo != null))
                    {
                        uxchkSwiped.Checked = true;
                        if (SwipedPercentFrom == null)
                        {
                            uxCboSwipedType.SelectedValue = "GreaterThan";
                            uxSwipedFrom.Value = SwipedPercentTo;
                        }
                        else if (SwipedPercentTo == null)
                        {
                            uxCboSwipedType.SelectedValue = "LessThan";
                            uxSwipedFrom.Value = SwipedPercentFrom;
                        }
                        else
                        {
                            uxCboSwipedType.SelectedValue = "Between";
                            uxSwipedFrom.Value = SwipedPercentFrom;
                            uxSwipedTo.Value = SwipedPercentTo;
                        }
                    }
                }
                break;
        }
    }
    #endregion ---- Event ----
    private void VisibleControls()
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            if (uxECommerce.Visible)
            {
                uxCboECommerceType.Enabled = false;
                uxchkEcommerce.Enabled = false;
                uxECommerceFrom.ReadOnly = uxECommerceTo.ReadOnly = true;
                uxCboECommerceType.OnClientDropDownOpening = "CancelDropDown";
            }
            if (uxKeyed.Visible)
            {
                uxCboKeyedType.Enabled = false;
                uxchkKeyed.Enabled = false;
                uxKeyedFrom.ReadOnly = uxKeyedTo.ReadOnly = true;
                uxCboKeyedType.OnClientDropDownOpening = "CancelDropDown";
            }
            if (uxSwiped.Visible)
            {
                uxCboSwipedType.Enabled = false;
                uxchkSwiped.Enabled = false;
                uxSwipedFrom.ReadOnly = uxSwipedTo.ReadOnly = true;
                uxCboSwipedType.OnClientDropDownOpening = "CancelDropDown";
            }
        }
    }
    protected void InitializeComboFilterType()
    {
        uxCboECommerceType.Items.Add(new RadComboBoxItem() { Value = "Between", Text = GetLocalResourceObject("Risk_Assignment_Filter_ECommerce_ascx_cs_Between").ToString() });
        uxCboECommerceType.Items.Add(new RadComboBoxItem() { Value = "GreaterThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_ECommerce_ascx_cs_GreaterThan").ToString() });
        uxCboECommerceType.Items.Add(new RadComboBoxItem() { Value = "LessThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_ECommerce_ascx_cs_LessThan").ToString() });
        uxCboECommerceType.DataBind();
        //uxCboKeyedType
        uxCboKeyedType.Items.Add(new RadComboBoxItem() { Value = "Between", Text = GetLocalResourceObject("Risk_Assignment_Filter_ECommerce_ascx_cs_Between").ToString() });
        uxCboKeyedType.Items.Add(new RadComboBoxItem() { Value = "GreaterThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_ECommerce_ascx_cs_GreaterThan").ToString() });
        uxCboKeyedType.Items.Add(new RadComboBoxItem() { Value = "LessThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_ECommerce_ascx_cs_LessThan").ToString() });
        uxCboKeyedType.DataBind();
        //uxCboSwipedType
        uxCboSwipedType.Items.Add(new RadComboBoxItem() { Value = "Between", Text = GetLocalResourceObject("Risk_Assignment_Filter_ECommerce_ascx_cs_Between").ToString() });
        uxCboSwipedType.Items.Add(new RadComboBoxItem() { Value = "GreaterThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_ECommerce_ascx_cs_GreaterThan").ToString() });
        uxCboSwipedType.Items.Add(new RadComboBoxItem() { Value = "LessThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_ECommerce_ascx_cs_LessThan").ToString() });
        uxCboSwipedType.DataBind();
    }
    private DataTable GetDataTable()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32));
        parameters.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetAssignmentValueFilter", parameters);
    }
    #region IRiskParamFilter Members
    public int Save()
    {
        if (this.Hide)
            return 0;
        int? ECommercePercentFrom = null;
        int? ECommercePercentTo = null;
        int? KeyedPercentFrom = null;
        int? KeyedPercentTo = null;
        int? SwipedPercentFrom = null;
        int? SwipedPercentTo = null;

        if (uxECommerce.Visible && uxchkEcommerce.Checked)
        {
            var ECommerceType = uxCboECommerceType.SelectedValue;

            switch (ECommerceType)
            {
                case "Between":
                    ECommercePercentFrom = (int)uxECommerceFrom.Value;
                    ECommercePercentTo = (int)uxECommerceTo.Value;
                    break;
                case "GreaterThan":
                    ECommercePercentTo = (int)uxECommerceFrom.Value;
                    break;
                case "LessThan":
                    ECommercePercentFrom = (int)uxECommerceFrom.Value;
                    break;
            }
        }
        if (uxKeyed.Visible && uxchkKeyed.Checked)
        {
            var KeyedType = uxCboKeyedType.SelectedValue;

            switch (KeyedType)
            {
                case "Between":
                    KeyedPercentFrom = (int)uxKeyedFrom.Value;
                    KeyedPercentTo = (int)uxKeyedTo.Value;
                    break;
                case "GreaterThan":
                    KeyedPercentTo = (int)uxKeyedFrom.Value;
                    break;
                case "LessThan":
                    KeyedPercentFrom = (int)uxKeyedFrom.Value;
                    break;
            }
        }
        if (uxSwiped.Visible && uxchkSwiped.Checked)
        {
            var Swiped = uxCboSwipedType.SelectedValue;

            switch (Swiped)
            {
                case "Between":
                    SwipedPercentFrom = (int)uxSwipedFrom.Value;
                    SwipedPercentTo = (int)uxSwipedTo.Value;
                    break;
                case "GreaterThan":
                    SwipedPercentTo = (int)uxSwipedFrom.Value;
                    break;
                case "LessThan":
                    SwipedPercentFrom = (int)uxSwipedFrom.Value;
                    break;
            }
        }


        FilterParameterCollection parameter = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameter.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameter.Add(new FilterParameter("@AssignmentID", int.Parse(this.PrimaryID), DbType.Int32));
        parameter.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));

        parameter.Add(new FilterParameter("@ECommercePercentFrom", ECommercePercentFrom, DbType.Int32));
        parameter.Add(new FilterParameter("@ECommercePercentTo", ECommercePercentTo, DbType.Int32));
        parameter.Add(new FilterParameter("@KeyedPercentFrom", KeyedPercentFrom, DbType.Double));
        parameter.Add(new FilterParameter("@KeyedPercentTo", KeyedPercentTo, DbType.Int32));
        parameter.Add(new FilterParameter("@SwipedPercentFrom", SwipedPercentFrom, DbType.Int32));
        parameter.Add(new FilterParameter("@SwipedPercentTo", SwipedPercentTo, DbType.Int32));

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveAssignmentValueFilter", parameter, out parameterOut);
        return 0;
    }
    public void ShowHideFieldConfig()
    {
        if (!GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.ECommerce))
        {
            uxECommerce.Visible = false;
        }
        if (!GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.Keyed))
        {
            uxKeyed.Visible = false;
        }
        if (!GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.Swiped))
        {
            uxSwiped.Visible = false;
        }
        if (!uxECommerce.Visible && !uxKeyed.Visible && !uxSwiped.Visible)
        {
            Hide = true;
        }
    }
    public void Rebind()
    {
        OnDataBindControls(DataBindAction.LoadDataFilter);
        VisibleControls();
    }
    public event EventHandler SelectedChanged;
    #endregion
}
