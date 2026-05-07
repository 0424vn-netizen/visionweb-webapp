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

public partial class UserControls_Risk_Assignment_Filter_Transactional : GlobalUserControl, IRiskParamFilter
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
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.LoadTransactionalFilter);
            
        }
        VisibleControls();
        InitValidation();
    }

    private void VisibleControls()
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            cb60dayTransCnt.Enabled = false;
            cbMinimumVol.Enabled = false;
            cbContractualVol.Enabled = false;

            cbxTransCntType.OnClientDropDownOpening = "CancelDropDown";

            txtMinimumVol.ReadOnly = txtContractualVol.ReadOnly = FeatureMode == WebSiteEnums.FeatureMode.View;
            txtBetweenFrom.ReadOnly = txtBetweenTo.ReadOnly = txtGt.ReadOnly = txtLt.ReadOnly = FeatureMode == WebSiteEnums.FeatureMode.View;
        }
    }

    protected void InitializeTransactionFilterType()
    {
        cbxTransCntType.Items.Add(new RadComboBoxItem() { Value = "Between", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_Between").ToString() });
        cbxTransCntType.Items.Add(new RadComboBoxItem() { Value = "GreaterThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_GreaterThan").ToString() });
        cbxTransCntType.Items.Add(new RadComboBoxItem() { Value = "LessThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_LessThan").ToString() });
        cbxTransCntType.DataBind();
    }

    private DataTable GetDataTable()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32));
        parameters.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_rm_cs_GetTransactionalFilter", parameters);
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
                    int? TransactionalTransactionCountFrom = string.IsNullOrEmpty(dt.Rows[0]["TransactionalTransactionCountFrom"].ToString()) ? null:(int?)dt.Rows[0]["TransactionalTransactionCountFrom"];
                    int? TransactionalTransactionCountTo = string.IsNullOrEmpty(dt.Rows[0]["TransactionalTransactionCountTo"].ToString()) ? null : (int?)dt.Rows[0]["TransactionalTransactionCountTo"];
       
                    int? TransactionalContractualVolume = string.IsNullOrEmpty(dt.Rows[0]["TransactionalContractualVolume"].ToString()) ? null : (int?)dt.Rows[0]["TransactionalContractualVolume"];
                    double? TransactionalMinimumVolume = null;
                    if(!string.IsNullOrEmpty(dt.Rows[0]["TransactionalMinimumVolume"].ToString()))
                    {
                        TransactionalMinimumVolume = double.Parse(dt.Rows[0]["TransactionalMinimumVolume"].ToString());
                    }

                    if (TransactionalTransactionCountFrom != null && TransactionalTransactionCountTo != null)
                    {
                        cb60dayTransCnt.Checked = true;

                        if (TransactionalTransactionCountFrom == int.MinValue)
                        {
                            cbxTransCntType.SelectedValue = "LessThan";
                            txtLt.Value = TransactionalTransactionCountTo;
                        }
                        else if (TransactionalTransactionCountTo == int.MaxValue)
                        {
                            cbxTransCntType.SelectedValue = "GreaterThan";
                            txtGt.Value = TransactionalTransactionCountFrom;
                        }
                        else
                        {
                            cbxTransCntType.SelectedValue = "Between";
                            txtBetweenFrom.Value = TransactionalTransactionCountFrom;
                            txtBetweenTo.Value = TransactionalTransactionCountTo;
                        }
                       
                    }
                    //else if (TransactionalTransactionCountFrom != null)
                    //{
                    //    cb60dayTransCnt.Checked = true;
                    //    cbxTransCntType.SelectedValue = "GreaterThan";
                    //    txtGt.Value = TransactionalTransactionCountFrom;
                    //}
                    //else if (TransactionalTransactionCountTo != null)
                    //{
                    //    cb60dayTransCnt.Checked = true;
                    //    cbxTransCntType.SelectedValue = "LessThan";
                    //    txtLt.Value = TransactionalTransactionCountTo;
                    //}



                    if(TransactionalMinimumVolume != null)
                    {
                        cbMinimumVol.Checked = true;
                        txtMinimumVol.Value = TransactionalMinimumVolume;
                    }

                    if(TransactionalContractualVolume != null)
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
        int? TransactionalTransactionCountFrom = null;
        int? TransactionalTransactionCountTo = null;
        double? TransactionalMinimumVolume = null;
        int? TransactionalContractualVolume = null;

        if (cb60dayTransCnt.Checked)
        {
            var TransactionCountType = cbxTransCntType.SelectedValue;

            switch (TransactionCountType)
            {
                case "Between":
                    TransactionalTransactionCountFrom = (int)txtBetweenFrom.Value;
                    TransactionalTransactionCountTo = (int)txtBetweenTo.Value;
                    break;
                case "GreaterThan":
                    TransactionalTransactionCountFrom = (int)txtGt.Value;
                    TransactionalTransactionCountTo = int.MaxValue;
                    break;
                case "LessThan":
                    TransactionalTransactionCountFrom = int.MinValue;
                    TransactionalTransactionCountTo = (int)txtLt.Value;
                    break;
            }
        }

        if (cbMinimumVol.Checked)
        {
            TransactionalMinimumVolume = txtMinimumVol.Value;
        }

        if (cbContractualVol.Checked)
        {
            TransactionalContractualVolume = (int)txtContractualVol.Value;
        }


        FilterParameterCollection parameter = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameter.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameter.Add(new FilterParameter("@AssignmentID", int.Parse(this.PrimaryID), DbType.Int32));
        parameter.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));

        parameter.Add(new FilterParameter("@TransactionalTransactionCountFrom", TransactionalTransactionCountFrom, DbType.Int32));
        parameter.Add(new FilterParameter("@TransactionalTransactionCountTo", TransactionalTransactionCountTo, DbType.Int32));
        parameter.Add(new FilterParameter("@TransactionalMinimumVolume", TransactionalMinimumVolume, DbType.Double));
        parameter.Add(new FilterParameter("@TransactionalContractualVolume", TransactionalContractualVolume, DbType.Int32));

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_SaveTransactionalFilter", parameter, out parameterOut);
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion

    #region Validate

    public void InitValidation()
    {
        SixtydayTransCntMsg.ShowOnLoad = false;
        MinimumVolValidatorMessage.ShowOnLoad = false;
        ContractualVolValidatorMessage.ShowOnLoad = false;
    }

    #endregion

}
