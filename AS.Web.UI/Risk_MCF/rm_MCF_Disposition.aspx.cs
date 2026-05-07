using AS.Common.DBManager;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Linq;

//[PagePermission("")]
public partial class rm_MCF_Disposition : NonReportPage
{
    enum DataBindAction
    {
        BindUserListGrid,
        RebindUserListAfterCreateClick,
        RebindUserListAfterCancelClick
    }

    enum PostBackAction
    {
        CreateClick
    }

    public enum DispositionStatus
    {
        All,
        Active,
        Inactive
    }

    public int Status;

    private DataTable _dispositionList
    {
        get
        {
            return RiskSessionManager.Risk_MCF_DispositionList;
        }
        set
        {
            RiskSessionManager.Risk_MCF_DispositionList = value;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DispositionStatus)type)
        {
            case DispositionStatus.All:
                Status = -1;
                GetDispositionList();
                break;
            case DispositionStatus.Active:
                Status = 1;
                GetDispositionList();
                break;
            case DispositionStatus.Inactive:
                Status = 0;
                GetDispositionList();
                break;
            default:
                break;
        }
    }

    protected void uxCreateMode_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CreateClick);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {

        switch ((PostBackAction)type)
        {
            case PostBackAction.CreateClick:
                AjaxAddResponseScript("ShowPopupModal('rm_MCF_CreateNewDispositionModal.aspx','auto');");
                break;
            default:
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OnDataBindControls(DispositionStatus.All);
        }
    }

    protected void GetDispositionList()
    {
        var dt = GetDataSource();

        if (dt.Rows.Count > 0)
        {
            uxDispositionGrid.Visible = true;
            uxDispositionGrid.DataSource = dt;
            uxDispositionGrid.DataBind();
            AjaxAddResponseScript("setNoDataFound(false);");
        }
        else
        {
            uxDispositionGrid.Visible = false;
            AjaxAddResponseScript("setNoDataFound(true);");
        }
    }

    protected void btnReFresh_Click(object sender, EventArgs e)
    {
        if (uxRabAll.Checked)
        {
            OnDataBindControls(DispositionStatus.All);
        }
        else if (uxRabActive.Checked)
        {
            OnDataBindControls(DispositionStatus.Active);
        }
        else
        {
            OnDataBindControls(DispositionStatus.Inactive);
        }
    }

    protected void uxDispositionGrid_ItemDataBound(object sender, RadListBoxItemEventArgs e)
    {
        DataRowView dataSourceRow = (DataRowView)e.Item.DataItem;
        LinkButton urlEdit = e.Item.FindControl("btnEdit") as LinkButton;
        string queryString = BuildSecureQueryString("DispositionID=" + dataSourceRow["DispositionID"]);
        urlEdit.OnClientClick = "ShowPopupModal('rm_MCF_CreateNewDispositionModal.aspx?" + queryString + "','auto');";
    }

    protected void uxUpdatePosition_Click(object sender, EventArgs e)
    {

        UpdatePosition();
        UpdatePositionSession();
    }

    private void UpdatePosition()
    {
        var parameters = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@DispositionList", hdListDisposition.Value, DbType.String));
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_UpdateSortIndexDisposition", parameters, out parameterOut);
    }

    public void UpdatePositionSession()
    {
        if (_dispositionList != null)
        {
            DataTable listDisposition = GetAllData();

            foreach (DataRow item in _dispositionList.Rows)
            {
                DataRow dispoItem = listDisposition.AsEnumerable().Where(x => x.Field<int>("DispositionID") == Int32.Parse(item["DispositionID"].ToString())).FirstOrDefault();
                if (dispoItem != null)
                {
                    item["SortIndex"] = dispoItem["SortIndex"];
                }
            }
            _dispositionList.DefaultView.Sort = "SortIndex asc";
            _dispositionList = _dispositionList.DefaultView.ToTable();
        }
    }

    private DataTable GetDataSource()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@IsActive", Status, DbType.String));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetDisposition", parameters);
    }
    private DataTable GetAllData()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@IsActive", -1, DbType.String));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetDisposition", parameters);
    }
}