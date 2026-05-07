using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_CampaignID : GlobalUserControl, IRiskParamFilter
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
    }

    public void VisibleControls()
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            cbCompaignID.Enabled = false;
            txtCompaignID.ReadOnly = FeatureMode == WebSiteEnums.FeatureMode.View;
        }
    }
    public void DisableControls()
    {
        cbCompaignID.Enabled = false;
        txtCompaignID.ReadOnly = true;
    }


    protected override void OnDataBindControls(Enum type, object sender)
    {
    }

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }


    public event EventHandler SelectedChanged;

    #endregion

    #region Validate

    #endregion

    public void Rebind()
    {
        OnDataBindControls(DataBindAction.LoadTransactionalFilter);
        VisibleControls();
    }

    public int Save()
    {
        return 0;
    }
}