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
using AS.Controls.Global;
using System.Collections.Generic;
using AS.Common.DBManager;

public partial class UserControls_rm_MCF_Assignment_Filter_Transactional : GlobalUserControl, IRiskParamFilter
{

    #region Enums
    enum DataBindAction
    {
        LoadTransactionalFilter
    }
    enum PostBackAction { }
    #endregion

    public WebSiteEnums.FeatureMode FeatureMode { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !(Mode == WebSiteEnums.ParamFilterMode.Assignment))
        {
            OnDataBindControls(DataBindAction.LoadTransactionalFilter);
        }

        VisibleControls();
        InitValidation();
    }

    public void VisibleControls()
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            cb60dayTransCnt.Enabled = false;
            chbTodaySaleAmount.Enabled = false;
            cbContractualVol.Enabled = false;

            cbxTransCntType.OnClientDropDownOpening = "CancelDropDown";
            cbTodaySaleAmount.OnClientDropDownOpening = "CancelDropDown";
            txtContractualVol.ReadOnly = FeatureMode == WebSiteEnums.FeatureMode.View;
            txtBetweenFrom.ReadOnly = txtBetweenTo.ReadOnly = txtGt.ReadOnly = txtLt.ReadOnly = FeatureMode == WebSiteEnums.FeatureMode.View;
            txtBetweenFrom1.ReadOnly = txtBetweenTo1.ReadOnly = txtGreaterThan1.ReadOnly = txtLessThan1.ReadOnly = FeatureMode == WebSiteEnums.FeatureMode.View;
        }
    }
    public void DisableControls()
    {
        cb60dayTransCnt.Enabled = false;
        chbTodaySaleAmount.Enabled = false;
        cbContractualVol.Enabled = false;

        cbxTransCntType.OnClientDropDownOpening = "CancelDropDown";
        cbTodaySaleAmount.OnClientDropDownOpening = "CancelDropDown";
        txtContractualVol.ReadOnly = true;
        txtBetweenFrom.ReadOnly = txtBetweenTo.ReadOnly = txtGt.ReadOnly = txtLt.ReadOnly = true;
        txtBetweenFrom1.ReadOnly = txtBetweenTo1.ReadOnly = txtGreaterThan1.ReadOnly = txtLessThan1.ReadOnly = true;
    }

    protected void InitializeTransactionFilterType()
    {
        cbxTransCntType.Items.Add(new RadComboBoxItem() { Value = "Between", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_Between").ToString() });
        cbxTransCntType.Items.Add(new RadComboBoxItem() { Value = "GreaterThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_GreaterThan").ToString() });
        cbxTransCntType.Items.Add(new RadComboBoxItem() { Value = "LessThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_LessThan").ToString() });
        cbxTransCntType.DataBind();

        cbTodaySaleAmount.Items.Add(new RadComboBoxItem() { Value = "Between", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_Between").ToString() });
        cbTodaySaleAmount.Items.Add(new RadComboBoxItem() { Value = "GreaterThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_GreaterThan").ToString() });
        cbTodaySaleAmount.Items.Add(new RadComboBoxItem() { Value = "LessThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_LessThan").ToString() });
        cbTodaySaleAmount.DataBind();
    }

    private DataTable GetDataTable()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32));
        parameters.Add(new FilterParameter("@FilterMode", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_Get_AssignmentFilter", parameters);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadTransactionalFilter:
                InitializeTransactionFilterType();

                DataTable dt = this.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    int? transactionalTransactionCountFrom = string.IsNullOrEmpty(dt.Rows[0]["TransactionalTransactionCountFrom"].ToString()) ? null:(int?)dt.Rows[0]["TransactionalTransactionCountFrom"];
                    int? transactionalTransactionCountTo = string.IsNullOrEmpty(dt.Rows[0]["TransactionalTransactionCountTo"].ToString()) ? null : (int?)dt.Rows[0]["TransactionalTransactionCountTo"];

                    int? transactionalMinimumVolume = string.IsNullOrEmpty(dt.Rows[0]["TransactionalMinimumVolume"].ToString()) ? null : (int?)dt.Rows[0]["TransactionalMinimumVolume"];
                    int? transactionalMaximumVolume = string.IsNullOrEmpty(dt.Rows[0]["TransactionalMaximumVolume"].ToString()) ? null : (int?)dt.Rows[0]["TransactionalMaximumVolume"];

                    int? TransactionalContractualVolume = string.IsNullOrEmpty(dt.Rows[0]["TransactionalContractualVolume"].ToString()) ? null : (int?)dt.Rows[0]["TransactionalContractualVolume"];

                    if (transactionalTransactionCountFrom != null && transactionalTransactionCountTo != null)
                    {
                        cb60dayTransCnt.Checked = true;

                        if (transactionalTransactionCountFrom == int.MinValue)
                        {
                            cbxTransCntType.SelectedValue = "LessThan";
                            txtLt.Value = transactionalTransactionCountTo;
                        }
                        else if (transactionalTransactionCountTo == int.MaxValue)
                        {
                            cbxTransCntType.SelectedValue = "GreaterThan";
                            txtGt.Value = transactionalTransactionCountFrom;
                        }
                        else
                        {
                            cbxTransCntType.SelectedValue = "Between";
                            txtBetweenFrom.Value = transactionalTransactionCountFrom;
                            txtBetweenTo.Value = transactionalTransactionCountTo;
                        }
                    }

                    if (transactionalMinimumVolume != null && transactionalMaximumVolume != null)
                    {
                        chbTodaySaleAmount.Checked = true;
                        if (transactionalMinimumVolume == int.MinValue)
                        {
                            cbTodaySaleAmount.SelectedValue = "LessThan";
                            txtLessThan1.Value = transactionalMaximumVolume;
                        }
                        else if (transactionalMaximumVolume == int.MaxValue)
                        {
                            cbTodaySaleAmount.SelectedValue = "GreaterThan";
                            txtGreaterThan1.Value = transactionalMinimumVolume;
                        }
                        else
                        {
                            cbTodaySaleAmount.SelectedValue = "Between";
                            txtBetweenFrom1.Value = transactionalMinimumVolume;
                            txtBetweenTo1.Value = transactionalMaximumVolume;
                        }
                    }

                    if (TransactionalContractualVolume != null)
                    {
                        cbContractualVol.Checked = true;
                        txtContractualVol.Value = TransactionalContractualVolume;
                    }
                }
                break;
        }
    }

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    public int Save()
    {
        int? transactionalTransactionCountFrom = null;
        int? transactionalTransactionCountTo = null;
        int? transactionalMinimumVolume = null;
        int? transactionalMaximumVolume = null;
        int? transactionalContractualVolume = null;

        if (cb60dayTransCnt.Checked)
        {
            var transactionCountType = cbxTransCntType.SelectedValue;

            switch (transactionCountType)
            {
                case "Between":
                    transactionalTransactionCountFrom = (int)txtBetweenFrom.Value;
                    transactionalTransactionCountTo = (int)txtBetweenTo.Value;
                    break;
                case "GreaterThan":
                    transactionalTransactionCountFrom = (int)txtGt.Value;
                    transactionalTransactionCountTo = int.MaxValue;
                    break;
                case "LessThan":
                    transactionalTransactionCountFrom = int.MinValue;
                    transactionalTransactionCountTo = (int)txtLt.Value;
                    break;
            }
        }

        if ( chbTodaySaleAmount.Checked)
        {
            var todaySaleAmountType = cbTodaySaleAmount.SelectedValue;
            switch (todaySaleAmountType)
            {
                case "Between":
                    transactionalMinimumVolume = (int)txtBetweenFrom1.Value;
                    transactionalMaximumVolume = (int)txtBetweenTo1.Value;
                    break;
                case "GreaterThan":
                    transactionalMinimumVolume = (int)txtGreaterThan1.Value;
                    transactionalMaximumVolume = int.MaxValue;
                    break;
                case "LessThan":
                    transactionalMinimumVolume = int.MinValue;
                    transactionalMaximumVolume = (int)txtLessThan1.Value;
                    break;
            }
        }

        if (cbContractualVol.Checked)
        {
            transactionalContractualVolume = (int)txtContractualVol.Value;
        }

        FilterParameterCollection parameter = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameter.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameter.Add(new FilterParameter("@AssignmentID", int.Parse(this.PrimaryID), DbType.Int32));
        parameter.Add(new FilterParameter("@FilterMode", this.Mode, DbType.Int32));
        parameter.Add(new FilterParameter("@TransactionalTransactionCountFrom", transactionalTransactionCountFrom, DbType.Int32));
        parameter.Add(new FilterParameter("@TransactionalTransactionCountTo", transactionalTransactionCountTo, DbType.Int32));
        parameter.Add(new FilterParameter("@TransactionalMinimumVolume", transactionalMinimumVolume, DbType.Int32));
        parameter.Add(new FilterParameter("@TransactionalMaximumVolume", transactionalMaximumVolume, DbType.Int32));
        parameter.Add(new FilterParameter("@TransactionalContractualVolume", transactionalContractualVolume, DbType.Int32));

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_Update_AssignmentFilter_Transactional", parameter, out parameterOut);
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion

    #region Validate

    public void InitValidation()
    {
        SixtydayTransCntMsg.ShowOnLoad = false;
        vlTodaySaleAmount.ShowOnLoad = false;
        ContractualVolValidatorMessage.ShowOnLoad = false;
    }

    #endregion

    public void Rebind()
    {
        OnDataBindControls(DataBindAction.LoadTransactionalFilter);
        VisibleControls();
        InitValidation();
    }
}
