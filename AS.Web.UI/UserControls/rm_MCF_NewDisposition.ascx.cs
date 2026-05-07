using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI;
using System.Linq;
using System.Text.RegularExpressions;

public partial class UserControls_rm_MCF_NewDisposition : GlobalUserControl
{
    #region ---- Variable & Enum ----
    enum PostBackAction
    {
        CreateAction,
        EditAction
    }

    enum MsgType
    {
        Name,
        Default
    }

    private string _DisName { get; set; }
    private bool _Status { get; set; }
    private bool _Default { get; set; }
    private int _NewID { get; set; }
    private int _NewPosition { get; set; }

    private string DispositionID
    {
        get
        {
            if (Page.IsSecureQueryString && Page.SecureQueryString["DispositionID"] != null)
                return Page.SecureQueryString["DispositionID"].ToString();
            return string.Empty;
        }
    }

    private static DataTable _dispositionList
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
    #endregion ---- Variable & Enum ----

    #region ---- protected Methods -----
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(DispositionID))
            {
                uxSubmit.Text = GetLocalResourceObject("btnSubmitResource2.Text").ToString();
                EditDispositionByID(int.Parse(DispositionID));
            }
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.CreateAction:
                int createResponse = CreateDisposition();
                MessageResponse(createResponse, PostBackAction.CreateAction);
                break;
            case PostBackAction.EditAction:
                int editResponse = EditDisposition();
                MessageResponse(editResponse, PostBackAction.EditAction);
                break;
            default:
                break;
        }
    }

    protected int CreateDisposition()
    {
        var parameters = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@DispositionName", _DisName, DbType.String));
        parameters.Add(new FilterParameter("@IsActive", _Status, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsDefault", _Default, DbType.Boolean));
        DataTable response = WebServices.RiskServices.GetReports("spa_RM_MCF_AddDisposition", parameters);
        var reStatus = Convert.ToInt32(response.Rows[0]["ReturnValue"].ToString());
        if (reStatus == 1)
        {
            _NewID = Convert.ToInt32(response.Rows[0]["DispositionID"].ToString());
            _NewPosition = Convert.ToInt32(response.Rows[0]["SortIndex"].ToString());
        }
        return reStatus;
    }

    protected int EditDisposition()
    {
        var parameters = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();

        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@DispositionID", int.Parse(DispositionID), DbType.Int32));
        parameters.Add(new FilterParameter("@DispositionName", _DisName, DbType.String));
        parameters.Add(new FilterParameter("@IsActive", _Status, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsDefault", _Default, DbType.Boolean));
        parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_UpdateDisposition", parameters, out parameterOut);
        return Convert.ToInt32(parameterOut[0].ParameterValue);
    }

    protected void EditDispositionByID(int DispositionID)
    {
        var data = GetDispositionByID(DispositionID);
        uxDispositionName.Text = data.Rows[0]["DispositionName"].ToString();
        if (data.Rows[0]["IsActive"].ToBoolean())
        {
            uxRabAction.Checked = true;
            uxRabInactive.Checked = false;
        }
        else
        {
            uxRabInactive.Checked = true;
            uxRabAction.Checked = false;
        }

        if (data.Rows[0]["IsDefault"].ToBoolean())
        {
            uxRabDefault.Checked = true;
            uxRabNotDefault.Checked = false;
        }
        else
        {
            uxRabNotDefault.Checked = true;
            uxRabDefault.Checked = false;
        }

    }

    protected DataTable GetDispositionByID(int DispositionID)
    {
        var parameters = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();

        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@DispositionID", DispositionID, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetDispositionByID", parameters);
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        _DisName = uxDispositionName.Text.Trim();
        _Status = uxRabAction.Checked;
        _Default = uxRabDefault.Checked;

        if (Validation())
        {
            if (string.IsNullOrEmpty(DispositionID))
            {
                OnPostBackActions(PostBackAction.CreateAction);
            }
            else
            {
                OnPostBackActions(PostBackAction.EditAction);
            }
        }
    }
    #endregion ---- protected Methods ----

    #region ---- Private Methods -----

    private bool Validation()
    {
        // Check special characters
        string strRegex = WebSiteConstants.REG_SPECIAL_CHARACTERS;
        Regex re = new Regex(strRegex);

        if (string.IsNullOrEmpty(_DisName) || !re.IsMatch(_DisName))
        {
            return false;
        }
        return true;
    }

    private void MessageResponse(int result, Enum type)
    {
        switch (result)
        {
            case 1:
                UpdateSessionDispositionList(type);
                ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ClosePopupModal();");
                ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("parent.refreshData();");
                break;

            case 2:
                ShowMessageError(GetLocalResourceObject("ValidationMessages_DuplicateFullView.Message").ToString(), MsgType.Name);
                ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("AdjustModalSize();");

                break;
            case 3:
                string message = !_Status ? "uxDefaultActiveMessage" : "uxDefaultMessage.Message";
                ShowMessageError(GetLocalResourceObject(message).ToString(), MsgType.Default);

                ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("AdjustModalSize();");
                break;
            default:
                break;
        }
    }

    private void ShowMessageError(string message, Enum type)
    {
        switch ((MsgType)type)
        {
            case MsgType.Name:
                txtNameErrMsg.Message = VeraCodeSolution.DoVeraCode(message);
                txtNameErrMsg.ShowOnLoad = true;
                break;
            case MsgType.Default:
                uxDefaultMessage.Message = VeraCodeSolution.DoVeraCode(message);
                uxDefaultMessage.ShowOnLoad = true;
                break;
            default:
                break;
        }
    }

    private DataTable GetDataSource()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@IsActive", _Status, DbType.String));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetDisposition", parameters);
    }

    private void UpdateSessionDispositionList(Enum type)
    {
        if (_dispositionList != null)
        {
            switch ((PostBackAction)type)
            {
                case PostBackAction.CreateAction:
                    if (!_dispositionList.Columns.Contains("Checked"))
                    {
                        _dispositionList.Columns.Add("Checked", typeof(Boolean));
                    }
                    DataRow row = _dispositionList.NewRow();
                    row["DispositionID"] = _NewID;
                    row["DispositionName"] = _DisName;
                    row["IsActive"] = _Status;
                    row["IsDefault"] = _Default;
                    row["SortIndex"] = _NewPosition;
                    row["Checked"] = false;
                    _dispositionList.Rows.Add(row);
                    break;
                case PostBackAction.EditAction:
                    DataRow item = _dispositionList.AsEnumerable().Where(x => x["DispositionID"].ToString().Equals(DispositionID)).FirstOrDefault();
                    item["DispositionName"] = _DisName;
                    item["IsActive"] = _Status;
                    item["IsDefault"] = _Default;
                    break;
                default:
                    break;
            }
        }
    }

    #endregion ---- private Methods ----
}