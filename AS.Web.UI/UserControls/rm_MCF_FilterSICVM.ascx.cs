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
using Telerik.Web.UI;
using AS.Controls.Grid;
using AS.Web.Business;

public partial class UserControls_rm_MCF_FilterSICVM : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        BindStatus,
        BindSICs
    }
    #endregion

    #region Const
    const string FILTER_TYPE = "4";
    const int MAX_ITEMS = 7;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            OnDataBindControls(DataBindAction.BindStatus);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindStatus:
                BindStatus();
                break;
            case DataBindAction.BindSICs:
                FilterParameterCollection _params = new FilterParameterCollection();
                _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                _params.AddLanguageID();
                _params.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
                _params.Add(new FilterParameter("@AssignmentID", Convert.ToInt32(PrimaryID), DbType.Int32));

                string spaName = "spa_RM_MCF_Get_AssignmentFilter_SIC";
                this.uxGridSIC.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME,
                    new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(_params) });
                break;
        }
    }

    #region ControlEvent
    protected void uxGridSIC_Init(object sender, EventArgs e)
    {
        uxGridSIC.PageSize = 10;
    }

    protected void uxGridSIC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindSICs);
    }

    #endregion 

    private void BindStatus()
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(PrimaryID), DbType.Int32));
        parames.Add(new FilterParameter("@IsAllSIC", false, DbType.Boolean, true));
        parames.Add(new FilterParameter("@IsIncluded", false, DbType.Boolean, true));

        FilterParameterCollection paramesOut = new FilterParameterCollection();

        // Get OutParams value
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_GetFilterSICCode_Info", parames, out paramesOut);

        FilterParameter isIncluded = paramesOut.FindFilterParameterByName("@IsIncluded", true);

        if (isIncluded != null)
        {
            bool value = Convert.ToBoolean(isIncluded.ParameterValue);
            if (value)
            {
                uxInclude.Text = GetLocalResourceObject("Risk_FilterSICVM_ascx_cs_Include").ToString();
            }
            else
            {
                uxInclude.Text = GetLocalResourceObject("Risk_FilterSICVM_ascx_cs_Exclude").ToString();
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
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
}
