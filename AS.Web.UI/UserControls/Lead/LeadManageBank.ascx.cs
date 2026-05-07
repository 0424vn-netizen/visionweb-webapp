using AS.Controls.Pages;
using AS.Leads.UserMaintService;
using AS.VW.Share.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_Lead_LeadManageBank : GlobalUserControl
{

    string type = "Branch";
    [Browsable(true)]
    public string RoleId { get; set; }

    static DataTable destinationDataSourceBank = new DataTable();
    static DataTable originaltionDataSourceBank = new DataTable();
    static DataTable destinationDataSourceBranch = new DataTable();
    static DataTable originaltionDataSourceBranch = new DataTable();
    private bool IsMSUser = SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS;

    const string BANK_ID = "AperiaBankId";
    const string BANK_NAME = "BankDisplayName";
    const string BRANCH_ID = "AperiaBranchId";
    const string BRANCH_NAME = "BranchDisplayName";
    const string TYPE_BRANCH = "branch";
    const string TYPE_BANK = "bank";

    private string secondaryUserRecId = "00000000-0000-0000-0000-000000000000";
    private static UserMaintBusiness userMaintBusiness = null;

    protected string ViewBankDisplayField
    {
        get
        {
            if (type == TYPE_BRANCH)
                return BRANCH_NAME;

            return BANK_NAME;
        }
    }

    private static UserInfo InitUserInfoObject()
    {
        UserInfo userInfo = new UserInfo();
        userInfo.AsClientId = SessionManager.CurrentUser.ASClient;
        userInfo.SiteId = SessionManager.CurrentUser.SiteID;
        userInfo.UserId = SessionManager.CurrentUser.UserID;
        userInfo.UserMode = String.Empty;
        userInfo.UserSessionId = SessionManager.UniqueSessionID;
        userInfo.RecId = SessionManager.CurrentUser.RecId;
        return userInfo;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool isAddMode = false;
        bool isAllRole = false;

        //Switch System mode in CS page
        if (GeneralFuncsLib.HasMSUserManagementFeature)
        {
            SecurePage _parentPage = (SecurePage)this.Page;
            if (_parentPage != null)
            {
                isAddMode = bool.Parse(_parentPage.SecureQueryString["isAddMode"]);
                isAllRole = bool.Parse(_parentPage.SecureQueryString["all"]);
                secondaryUserRecId = _parentPage.SecureQueryString["recId"].ToString();
                type = _parentPage.SecureQueryString["type"].ToString();

                if (!IsMSUser)
                {
                    // Get system of this user when the Current System is CS
                    int systemId = 0;
                    int.TryParse(_parentPage.SecureQueryString["systemId"], out systemId);
                    IsMSUser = systemId == WebSiteConstants.AS_SYSTEM_MS;
                }
            }
        }

        BindDataForBank_Branch(isAddMode, isAllRole);

        if (IsMSUser)
        {
            LoadBranchResource();
        }

       // if (!Page.IsPostBack)
       // {
            uxBankListSelected.ListBoxLeft.DataBound += ListBoxLeft_DataBound;
            uxBankListSelected.ListBoxRight.DataBound += ListBoxRight_DataBound;
       // }
    }

    void ListBoxLeft_DataBound(object sender, EventArgs e)
    {
        ListBox lstBox = sender as ListBox;

        TruncateData(lstBox);
    }

    void ListBoxRight_DataBound(object sender, EventArgs e)
    {
        ListBox lstBox = sender as ListBox;
        TruncateData(lstBox);
       
    }

    private void TruncateData(ListBox lstBox)
    {
        foreach (ListItem item in lstBox.Items)
        {
            if (item.Text.Trim().Length > 37)
            {
                item.Attributes.Add("title", item.Text);
                item.Attributes.Add("class", "ellipsis");
                //item.Text = item.Text.Substring(0, 10) + "...";
            }
        }
    }

    private void BindDataForBank_Branch(bool isAddMode, bool isAllRole)
    {
        userMaintBusiness = new UserMaintBusiness(InitUserInfoObject());

        if (isAddMode)//Add Bank
        {
            SetDataMember();

            if (!IsPostBack)
            {
                if (type == TYPE_BANK)
                {
                    DataSet dsBank = userMaintBusiness.GetBanks(secondaryUserRecId, SessionManager.UniqueSessionID);

                    originaltionDataSourceBank = dsBank.Tables[0];
                    uxBankListSelected.DataSourceOrigination = originaltionDataSourceBank;
                    destinationDataSourceBank = dsBank.Tables[1];
                    uxBankListSelected.DataSourceDestination = destinationDataSourceBank;
                }
                else
                {
                    DataSet dsBranch = userMaintBusiness.GetBranchs(secondaryUserRecId, SessionManager.UniqueSessionID);

                    originaltionDataSourceBranch = dsBranch.Tables[0];
                    uxBankListSelected.DataSourceOrigination = originaltionDataSourceBranch;
                    destinationDataSourceBranch = dsBranch.Tables[1];
                    uxBankListSelected.DataSourceDestination = destinationDataSourceBranch;
                }
            }
            panelViewBank.Visible = false;
        }
        else//View Bank
        {
            SetDataMember();
            if (type == TYPE_BANK)
            {
                if (isAllRole)
                {
                    DataSet dsBank = userMaintBusiness.GetAllBanks(secondaryUserRecId, SessionManager.UniqueSessionID);
                    uxRepeaterViewBank.DataSource = dsBank.Tables[0];
                }
                else
                {
                    DataSet dsBank = userMaintBusiness.GetBanks(secondaryUserRecId, SessionManager.UniqueSessionID);
                    uxRepeaterViewBank.DataSource = dsBank.Tables[1];
                }

                uxRepeaterViewBank.DataBind();
            }
            else
            {
                if (isAllRole)
                {
                    DataSet dsBranch = userMaintBusiness.GetAllBranchs(secondaryUserRecId);
                    uxRepeaterViewBank.DataSource = dsBranch.Tables[0];
                }
                else
                {
                    DataSet dsBranch = userMaintBusiness.GetBranchs(secondaryUserRecId, SessionManager.UniqueSessionID);
                    uxRepeaterViewBank.DataSource = dsBranch.Tables[1];
                }

                uxRepeaterViewBank.DataBind();
            }
            panelAddBank.Visible = false;
        }
    }

    private void SetDisableListItem(DataTable tbl, string columnName, ListBox listBox)
    {
        foreach (ListItem item in listBox.Items)
        {
            DataRow row = tbl.FindObject(columnName, item.Value);

            if (row != null && row.Table.Columns.Contains("IsCanNotRevoke"))
            {
                if (row["IsCanNotRevoke"].ToBoolean())
                    item.Attributes.Add("disabled", "disabled");
            }
            else if (item.Value.IsNullOrEmpty())
            {
                item.Attributes.Add("disabled", "disabled");
            }
        }
    }

    

    private void LoadBranchResource()
    {
        lblBankTitle.Text = GetLocalResourceObject("Label_Assign_Branch_Title").ToString();
        lblAvailableBank.Text = GetLocalResourceObject("Label_Available_Branch_Title").ToString();
        lblSelectedBank.Text = GetLocalResourceObject("Label_Branch_Selected_Title").ToString();
        ltrViewBankTitle.Text = GetLocalResourceObject("Label_Branch_Title").ToString();
        ltrViewBankSubTitle.Text = GetLocalResourceObject("Label_Branch_Sub_Title").ToString();
    }

    private void SetDataMember()
    {
        if (type == TYPE_BANK)
        {
            uxBankListSelected.DataValueField = BANK_ID;
            uxBankListSelected.DataTextField = BANK_NAME;
        }
        else
        {
            uxBankListSelected.DataValueField = BRANCH_ID;
            uxBankListSelected.DataTextField = BRANCH_NAME;
        }
    }

    private void RemoveDataRow(DataTable table, DataRow row)
    {
        if (row != null)
            table.Rows.Remove(row);
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        string aperiaBankIds = string.Empty;

        foreach (DataRow row in uxBankListSelected.DataSourceDestination.Rows)
        {
            if (aperiaBankIds.IsNullOrEmpty())
                aperiaBankIds = row[uxBankListSelected.DataValueField].ToString();
            else
                aperiaBankIds = string.Format("{0},{1}", aperiaBankIds, row[uxBankListSelected.DataValueField].ToString());
        }


        //Save data to Staging table
        if (type == TYPE_BANK)
        {
            userMaintBusiness.SaveStagingBanks(secondaryUserRecId, SessionManager.UniqueSessionID, aperiaBankIds);
        }
        else
        {
            userMaintBusiness.SaveStagingBranchs(secondaryUserRecId, SessionManager.UniqueSessionID, aperiaBankIds);
        }

        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format("SubmitForm(" + uxBankListSelected.DataSourceDestination.Rows.Count + ")"));
    }

    protected void uxSelectAll_Click(object sender, EventArgs e)
    {
        //Get current selected + available list
        if (type == TYPE_BANK)
        {
            destinationDataSourceBank = uxBankListSelected.DataSourceDestination;
            originaltionDataSourceBank = uxBankListSelected.DataSourceOrigination;
        }
        else
        {
            destinationDataSourceBranch = uxBankListSelected.DataSourceDestination;
            originaltionDataSourceBranch = uxBankListSelected.DataSourceOrigination;
        }

        foreach (ListItem item in uxBankListSelected.ListBoxLeft.Items)
        {
            if (type == TYPE_BANK)
            {
                SelectAllItemListBox(item, uxBankListSelected.DataValueField, uxBankListSelected.DataTextField, originaltionDataSourceBank, destinationDataSourceBank);
            }
            else
            {
                SelectAllItemListBox(item, uxBankListSelected.DataValueField, uxBankListSelected.DataTextField, originaltionDataSourceBranch, destinationDataSourceBranch);
            }
        }

        this.RebindMultiSelectBank();
    }

    //Select All item in Listbox Bank/Branch
    private void SelectAllItemListBox(ListItem item, string columnId, string columnName, DataTable originaltionDataSource, DataTable destinationDataSource)
    {
        //remove at Original.
        RemoveDataRow(originaltionDataSource, originaltionDataSource.FindObject(columnId, item.Value));

        if (!item.Value.IsNullOrEmpty())
        {
            DataRow row = destinationDataSource.FindObject(columnId, item.Value);
            if (row == null)
            {
                //Add for Destination
                DataRow addRow = destinationDataSource.NewRow();
                addRow.SetField(columnId, item.Value);
                addRow.SetField(columnName, item.Text);
                destinationDataSource.Rows.Add(addRow);
            }
        }
    }

    private void RebindMultiSelectBank()
    {
        SetDataMember();
        if (type == TYPE_BANK)
        {
            LiteralSeletedBank.Text = destinationDataSourceBank.Rows.Count.ToString();
            uxBankListSelected.DataSourceOrigination = originaltionDataSourceBank;
            uxBankListSelected.DataSourceDestination = destinationDataSourceBank;
        }
        else
        {
            LiteralSeletedBank.Text = destinationDataSourceBranch.Rows.Count.ToString();
            uxBankListSelected.DataSourceOrigination = originaltionDataSourceBranch;
            uxBankListSelected.DataSourceDestination = destinationDataSourceBranch;
        }
    }

    private void SetRecordNotFound()
    {
        bool isNoRecord = true;

        if (uxBankListSelected.DataSourceOrigination.DefaultView.Count > 0)
        {
            isNoRecord = uxBankListSelected.DataSourceOrigination.DefaultView.ToTable().Rows[0][uxBankListSelected.DataValueField].ToString().IsNullOrEmpty();
        }
        if (isNoRecord)
        {
            DataTable tbl = uxBankListSelected.DataSourceOrigination.DefaultView.ToTable();

            DataRow row = tbl.FindObject(uxBankListSelected.DataValueField, "");
            if (row == null)
            {
                DataRow addRow = tbl.NewRow();
                addRow.SetField(uxBankListSelected.DataValueField, "");
                addRow.SetField(uxBankListSelected.DataTextField, GetLocalResourceObject(type == TYPE_BANK ? "NoBankFound" : "NoBranchFound"));
                tbl.Rows.Add(addRow);

                uxBankListSelected.ListBoxLeft.DataSource = tbl;
                uxBankListSelected.ListBoxLeft.DataBind();
            }
        }

    }

    private void ReBindWithFilter()
    {
        string dataTextField = type == TYPE_BANK ? BANK_NAME : BRANCH_NAME;

        uxBankListSelected.DataSourceOrigination.DefaultView.RowFilter = BuildRowFilter(uxBankListSelected, dataTextField);
        uxBankListSelected.DataSourceOrigination.DefaultView.Sort = string.Concat(uxBankListSelected.DataTextField, " ASC");
        uxBankListSelected.ListBoxLeft.DataSource = uxBankListSelected.DataSourceOrigination.DefaultView.ToTable();
        uxBankListSelected.ListBoxLeft.DataBind();
    }
    protected void uxBankListSelected_PreRender(object sender, EventArgs e)
    {
        //Rebind uxLeft with current RowFilter
        ReBindWithFilter();

        //Set Not Found record if listitem is null
        SetRecordNotFound();

        uxSubmit.Enabled = uxBankListSelected.ListBoxRight.Items.Count > 0;

        int totalSelected = uxBankListSelected.DataSourceDestination.Rows.Count;

        //Don't count No Bank/Branch Found item
        if (uxBankListSelected.SelectedItems.FindByText(GetLocalResourceObject(type == TYPE_BANK ? "NoBankFound" : "NoBranchFound").ToString()).IsNotNullData())
        {
            totalSelected--;
        }

        LiteralSeletedBank.Text = totalSelected.ToString();

        DataRow row = uxBankListSelected.DataSourceDestination.FindObject(uxBankListSelected.DataValueField, "");
        if (row != null)
            uxBankListSelected.DataSourceDestination.Rows.Remove(row);

        uxBankListSelected.DataSourceDestination = uxBankListSelected.DataSourceDestination;


        //Disable ListItem
        if (type == TYPE_BANK)
            SetDisableListItem(destinationDataSourceBank, uxBankListSelected.DataValueField, uxBankListSelected.ListBoxRight);
        else
            SetDisableListItem(destinationDataSourceBranch, uxBankListSelected.DataValueField, uxBankListSelected.ListBoxRight);

        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format("HideRightFilter()"));
    }

    private TResult GetFilterControl<TResult>(Control baseControl, string controlId)
    {
        foreach (Control control in baseControl.Controls)
        {

            if (control is TResult && control.ID.Contains(controlId))
            {
                return ((TResult)(object)control);
            }
        }
        return default(TResult);
    }
    private string BuildRowFilter(Control baseControl, string dataTextField)
    {
        string filterValue = string.Empty;
        string filterString = string.Empty;
        string filterOption = string.Empty;
        TextBox txtFilterLeft = GetFilterControl<TextBox>(baseControl, "_txtFilterLeft");
        HiddenField hddFilterOption = GetFilterControl<HiddenField>(baseControl, "_hddFilterOption");

        if (txtFilterLeft != null)
            filterValue = txtFilterLeft.Text.Trim();

        if (hddFilterOption != null)
            filterOption = hddFilterOption.Value;

        if (!filterValue.IsNullOrEmpty())
        {
            switch (filterOption)
            {
                case "Contains":
                    {
                        filterString = string.Format(string.Concat(dataTextField, " like '%{0}%'"), filterValue);
                        break;
                    }
                case "DoesNotContain":
                    {
                        filterString = string.Format(string.Concat(dataTextField, " not like '%{0}%'"), filterValue);
                        break;
                    }
                case "StartsWith":
                    {
                        filterString = string.Format(string.Concat(dataTextField, " like '{0}%'"), filterValue);
                        break;
                    }
                case "EndsWith":
                    {
                        filterString = string.Format(string.Concat(dataTextField, " like '%{0}'"), filterValue);
                        break;
                    }
                case "EqualTo":
                    {
                        filterString = string.Format(string.Concat(dataTextField, " = '{0}'"), filterValue);
                        break;
                    }
                case "NotEqualTo":
                    {
                        filterString = string.Format(string.Concat(dataTextField, " <> '{0}'"), filterValue);
                        break;
                    }
            }
        }

        return filterString;
    }
}



