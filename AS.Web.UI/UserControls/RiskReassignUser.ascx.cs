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
using Telerik.Web.UI;
using Resources;
using Unit = System.Web.UI.WebControls.Unit;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System.ComponentModel;


public partial class UserControls_RiskReassignUser : GlobalUserControl
{
    protected bool _UpdateMode = false;
    public bool UpdateMode { get { return _UpdateMode; } set { _UpdateMode = value; } }
    public event AfterSubmitHandler AfterSubmit;
    public event AfterCancelHandler AfterCancel;
    public delegate void AfterSubmitHandler(object sender, string userName, string userReassign, DateTime fromDate, DateTime toDate);
    public delegate void AfterCancelHandler(object sender);
     
    [Category("Behavior"), DefaultValue(""), Description("Container css for outer div"), NotifyParentProperty(true),]
    public string ContainerCss { get; set; }

    //protected override void OnInit(EventArgs e)
    //{
    //    base.OnInit(e);
    //    string curCulture = GeneralFuncsLib.GetCurrentCulture();
    //    uxSDate.Culture = uxEDate.Culture = new System.Globalization.CultureInfo(curCulture);
    //}

    protected override void OnPreRender(EventArgs e)
    {
        if ((ContainerCss != null) && (ContainerCss != String.Empty))
        {
            dvOuter.Attributes.Add("class", String.Format("{0}", ContainerCss));
        }

        base.OnPreRender(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page.IsIntruderDetected) return;
        if (!IsPostBack)
        {
            DoBindData(null, null, null, null, null);
        }

        string mode = "";
        string userId = "";
        string validateTemplate = "return ValidateData('{0}','{1}','{2}','{3}','{4}');";
        if (_UpdateMode)
        {
            mode = "update";
            //RadAjaxManager1.EnableAJAX = false;
            uxUserList.Visible = false;
            userId = uxUserName.ClientID;
        }
        else
        {
            uxUserList.Visible = true;
            //RadAjaxManager1.EnableAJAX = true;
            mode = "add";
            uxSDate.MinDate = uxEDate.MinDate = DateTime.Now;
            userId = uxUserList.ClientID;
        }

        uxUpdate.OnClientClick = String.Format(validateTemplate, uxSDate.ClientID, uxEDate.ClientID, userId, uxReassignList.ClientID, mode);
         
    }

    protected void uxCancel_Click(object sender, EventArgs e)
    {
        if (!_UpdateMode)//create mode
        {
            this.Visible = false;

        }
        if (AfterCancel != null)
        {
            AfterCancel(this);
        }
    }


    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        uxReassignListErrMsg.Message = String.Empty;
        uxUserListErrMsg.Message = String.Empty;

        if (this.Page.IsIntruderDetected) return;
        if (!DoValidateInput()) return;

        if (!validateComboBox())
        {
            if (uxReassignListErrMsg.Message != String.Empty)
            { uxReassignListlabel.CssClass = "control-label label-error"; }
            else
            { uxReassignListlabel.CssClass = "control-label"; }

            if (uxUserListErrMsg.Message != String.Empty)
            { uxUserListLabel.CssClass = "control-label label-error"; }
            else
            { uxUserListLabel.CssClass = "control-label"; }

            return;
        } 

        int result = 0;

        if (uxReassignList.SelectedIndex < 0)
        {
            RadComboBoxItem item = uxReassignList.Items.FindItemByText(uxReassignList.Text.ToUpper());
            if (item == null)
            {                               
                ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(String.Format("alert('{0}')", MessageManager.Generic_InvalidUser));
                return;
            }
            uxReassignList.SelectedValue = item.Value.ToUpper();
        }
        if (uxUserList.SelectedValue.Trim().ToUpper().Equals(uxReassignList.SelectedValue.Trim().ToUpper()))
        {
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(String.Format("setTimeout(\"alert('{0}');\", 100);", MessageManager.ReassignUser_ReassignToEqualUserName));
            return;
        }

        if (_UpdateMode)
        {
            result = UpdateReassignment(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, int.Parse(uxUpdate.CommandArgument), uxUserList.SelectedValue, uxReassignList.SelectedValue, uxSDate.SelectedDate.Value, uxEDate.SelectedDate.Value);
        }
        else
        {
            if (uxUserList.SelectedIndex < 1)
            {
                RadComboBoxItem item = uxUserList.Items.FindItemByText(uxUserList.Text.ToUpper());
                if (item == null)
                {
                    ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(String.Format("alert('{0}');", MessageManager.Generic_InvalidUser));
                    return;
                }
                uxUserList.SelectedValue = item.Value.ToUpper();
            }
            result = AddReassignment(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, uxUserList.SelectedValue, uxReassignList.SelectedValue, uxSDate.SelectedDate.Value, uxEDate.SelectedDate.Value);
        }
        if (result == 1)
        {
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(String.Format("setTimeout(\"alert('{0}');\", 100);", MessageManager.Generic_ProcessingFailed));
        }
        else if (result == 2)
        {
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(String.Format("setTimeout(\"alert('{0}');\", 100);", MessageManager.Reassign_Overlap));
        }

        if (AfterSubmit != null)
        {
            AfterSubmit(this, uxUserList.SelectedValue, uxReassignList.SelectedValue, uxSDate.SelectedDate.Value, uxEDate.SelectedDate.Value);
        }
    }


    protected bool validateComboBox()
    {
        ReportPage page = (ReportPage)this.Page;
        if (uxReassignList.SelectedIndex < 0)
        {
            RadComboBoxItem item = uxReassignList.Items.FindItemByText(uxReassignList.Text);
            if (item == null)
            {
                page.ShowServerErrorMessage(uxReassignListErrMsg, UserMaintenanceMessage.ReassignUser_InvalidUser);
                return false;
            }
            uxReassignList.SelectedValue = item.Value;
        }
        if (uxUserList.SelectedValue.Trim().ToUpper().Equals(uxReassignList.SelectedValue.Trim().ToUpper()))
        {
            page.ShowServerErrorMessage(uxReassignListErrMsg, UserMaintenanceMessage.ReassignUser_ReassignToEqualUserName);
            return false;
        }

        if (!_UpdateMode)
        {
            if (uxUserList.SelectedIndex < 1)
            {
                RadComboBoxItem item = uxUserList.Items.FindItemByText(uxUserList.Text);
                if (item == null)
                { 
                    page.ShowServerErrorMessage(uxUserListErrMsg, UserMaintenanceMessage.ReassignUser_InvalidUser);
                    return false;
                }
                uxUserList.SelectedValue = item.Value;
            }
        }
        return true;
    }

    
    public bool DoValidateInput()
    {
        if (uxUserList.SelectedValue != string.Empty && uxReassignList.SelectedValue != string.Empty && uxUserList.SelectedValue.Equals(uxReassignList.SelectedValue))
        {
            this.Page.IsIntruderDetected = true;
            return false;
        }
        if (!uxSDate.SelectedDate.HasValue)
        {
            this.Page.IsIntruderDetected = true;
            return false;
        }
        if (!uxEDate.SelectedDate.HasValue)
        {
            this.Page.IsIntruderDetected = true;
            return false;
        }
        if (uxEDate.SelectedDate.Value < uxSDate.SelectedDate.Value)
        {
            this.Page.IsIntruderDetected = true;
            return false;
        }
        return true;
    }

    public void DoBindData(string reassignId, string selectUserName, string selectReassign, DateTime? fromDate, DateTime? toDate)
    {
        if (this.Page.IsIntruderDetected) return;
        if (_UpdateMode)
        {
            BindDataReassignment(selectUserName, selectReassign);
        }
        
        uxUpdate.CommandArgument = reassignId;
        uxUserList.DataSource = this.RiskUsersData;
        uxUserList.DataBind();
        uxUserList.Items.Insert(0, new RadComboBoxItem(""));
    
        if (uxUserList.Items.Count > 10)
            uxUserList.Height = Unit.Pixel(220);
        //uxUserList.SelectedIndex = uxReassignList.SelectedIndex = 0; 

        if (selectUserName != null)
        {

            uxUserList.SelectedValue = selectUserName;
            RadComboBoxItem item = uxUserList.Items.FindItemByValue(selectUserName);

            if (_UpdateMode && item != null)
            {
                uxUserName.Visible = true;

                uxUserName.Text = AS.Common.VeraCodeSolution.DoVeraCode("<b>" + AS.Common.VeraCodeSolution.ValidateResponseData(item.Text) + "</b>");
                uxUserList.Enabled = false;
            }
        }

        if (fromDate.HasValue)
        {
            uxSDate.SelectedDate = fromDate;
            if (DateTime.Compare(fromDate.Value, DateTime.Now.Date) < 0)
                uxSDate.Enabled = false;
            else
            {
                uxSDate.Enabled = true;
                uxSDate.MinDate = DateTime.Now;
            }
        }
        if (toDate.HasValue)
        {
            uxEDate.SelectedDate = toDate;
            if (_UpdateMode)
                uxEDate.MinDate = DateTime.Now;
        }
    }
    
    public void ResetForm()
    {
        uxUserList.DataBind();
        uxUserList.SelectedIndex = -1;
        uxUserList.Text = uxReassignList.Text = ""; 
        uxReassignList.Items.Clear();
        uxSDate.Clear();
        uxEDate.Clear();
    }

    protected void uxUserList_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (uxUserList.SelectedIndex < 0 || uxUserList.Text == string.Empty)
        {
            uxReassignList.Items.Clear();
        }
        else
        {
            BindDataReassignment(uxUserList.Text, "");
            uxReassignList.SelectedIndex = 0;
        }
        //uxUpdate.Enabled = true;
    }

    private void BindDataReassignment(string selectUserName, string selectReassign)
    {
        if (uxUserList.SelectedIndex < 0 && !_UpdateMode)
        {
            BindDataReassignment();
            return;
        }
        BindDataReassignment();
        if (uxReassignList.Items.Count > 10)
            uxReassignList.Height = Unit.Pixel(220);
        RadComboBoxItem item = new RadComboBoxItem();

        if (selectReassign != string.Empty) //Edit
        {
            item = uxReassignList.Items.FindItemByValue(selectUserName.ToUpper());
            uxReassignList.SelectedValue = selectReassign;
        }
        else //Create
        {
            item = uxReassignList.Items.FindItemByText(selectUserName);
        }
        if (uxReassignList.Items.Count > 0 && item != null)
        {
            uxReassignList.Items.Remove(item);
        }
    }
    
    private void BindDataReassignment()
    {
        uxReassignList.DataSource = this.RiskUsersData;
        uxReassignList.DataBind();
    }

    private DataTable _RiskUsersData = null;
    private DataTable RiskUsersData
    {
        get {
            if (_RiskUsersData == null || _RiskUsersData.Rows.Count == 0)
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams(false);
                _RiskUsersData = WebServices.RiskServices.GetReports("spa_rm_cs_GetRiskUsers", parameters);
            }
            return _RiskUsersData;
        }
    }
    #region Data
    public int AddReassignment(int dDSClient, string userID, string fromUser, string toUser, DateTime from, DateTime to)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@FromUserID", fromUser, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ToUserID", toUser, DbType.AnsiString));
        parameters.Add(new FilterParameter("@FromDate", from, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ToDate", to, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ReturnValue", 0, DbType.Int32,true));
        FilterParameterCollection outparameters = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_AddReassignment", parameters, out outparameters);
        return (int)outparameters[0].ParameterValue;
    }
    public int UpdateReassignment(int dDSClient, string userID, int reassignId, string fromUser, string toUser, DateTime from, DateTime to)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@ReassignmentID",  reassignId,DbType.Int32));
        parameters.Add(new FilterParameter("@FromUserID", fromUser, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ToUserID", toUser, DbType.AnsiString));
        parameters.Add(new FilterParameter("@FromDate", from, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ToDate", to, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ReturnValue", 0, DbType.Int32,true));
        FilterParameterCollection outparameters = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_UpdateReassignment", parameters, out outparameters);
        return (int)outparameters[0].ParameterValue;
    }
    #endregion Data
}
