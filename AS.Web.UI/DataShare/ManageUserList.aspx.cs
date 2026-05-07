using AS.Common.DBManager;
using AS.Controls.Global;
using System;
using System.Data;
using System.Text;
using System.Web.UI;

public partial class ManageUserList : NonReportPage
{
    #region Enum
    enum DataBindAction
    {
        BindSelector,
    }
    enum PostbackAction
    {
        Submit = 1,
    }
    #endregion

    #region Properties
    protected int UserGroupID
    {
        get
        {
            if (ViewState["UserGroupID"] == null)
            {
                if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["UserGroupID"]))
                    ViewState["UserGroupID"] = int.Parse(SecureQueryString["UserGroupID"]);
                else
                    ViewState["UserGroupID"] = 0;
            }
            return int.Parse(ViewState["UserGroupID"].ToString());
        }
    }

    DataTable OriginalSourceUserList
    {
        get
        {

            return GetSourceUserList();
        }
    }

    DataTable _OriginalDestinationUserList;
    DataTable OriginalDestinationUserList
    {
        get
        {
            if (!_OriginalDestinationUserList.HasData())
                _OriginalDestinationUserList = GetDestinationUserList();
            return _OriginalDestinationUserList;
        }
    }
    #endregion

    #region Page Life Cycle
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;

        if (!IsPostBack)
            OnDataBindControls(DataBindAction.BindSelector, this);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindSelector:
                BindFirstLoadData();
                break;
        }
    }

    private void BindFirstLoadData()
    {
        uxUserSelector.DataSourceOrigination = RemoveDuplicate(OriginalSourceUserList, OriginalDestinationUserList, "UserName");
        uxUserSelector.DataSourceDestination = OriginalDestinationUserList;
    }

    protected DataTable GetSourceUserList()
    {
        FilterParameterCollection parasI = new FilterParameterCollection();
        parasI.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        return WebServices.CsReportServices.GetReports("spa_cs_DataShare_GetUserList", parasI);
    }

    protected DataTable GetDestinationUserList()
    {
        FilterParameterCollection parasI = new FilterParameterCollection();
        parasI.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parasI.Add(new FilterParameter("@GroupID", UserGroupID, DbType.Int32));
        parasI.Add(new FilterParameter("@Mode", true, DbType.Boolean));
        return WebServices.CsReportServices.GetReports("spa_cs_DataShare_GetUserList", parasI);
    }
    #endregion

    #region Events
    protected void uxSelector_DoubleClick(object sender, EventArgs e)
    {
        MultiSelector uxSelector = (MultiSelector)sender;
        if (Page.Request["__EVENTTARGET"].Contains("uxLeft"))
            uxSelector.MoveLeftToRight();
        else
            uxSelector.MoveRightToLeft();
    }

    protected void uxSave_Click(object sender, EventArgs e)
    {
        // Update DB here
        FilterParameterCollection parasI = new FilterParameterCollection();
        FilterParameterCollection outparam = new FilterParameterCollection();
        parasI.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parasI.Add(new FilterParameter("@GroupID", UserGroupID, DbType.Int32));
        parasI.Add(new FilterParameter("@UserIDList", ParseUserString(uxUserSelector.DataSourceDestination, "UserID"), DbType.String));
        WebServices.CsReportServices.ExecuteNonQueryCommand("spa_cs_DataShare_UpdateUserGroupUserList", parasI, out outparam);

        AjaxAddResponseScript("SaveAndExit();");
    }
    #endregion

    #region Ulti
    protected DataTable RemoveDuplicate(DataTable source, DataTable dest, string fieldCompare)
    {
        string destString = ParseUserString(dest, fieldCompare);
        if (source.HasData())
        {
            DataTable result = source.Clone();
            foreach (DataRow sRow in source.Rows)
            {
                if (!destString.Contains(sRow[fieldCompare].ToString())) result.ImportRow(sRow);
            }
            return result;
        }
        else return source;
    }

    protected DataTable AddAdditionalRows(DataTable source, DataTable dest)
    {
        if (dest.HasData())
        {
            foreach (DataRow dRow in dest.Rows)
            {
                source.ImportRow(dRow);
            }
        }
        return source;
    }

    protected string ParseUserString(DataTable source, string dataField, string defaultValue = "")
    {
        StringBuilder sb = new StringBuilder();
        if (source.HasData())
        {
            foreach (DataRow row in source.Rows)
            {
                sb.AppendFormat("{0},", row[dataField].ToString());
            }
            if (source.Rows.Count > 0) sb.Remove(sb.Length - 1, 1);
            else sb.Append(defaultValue);
        }
        else sb.Append(defaultValue);

        return sb.ToString();
    }
    #endregion
}