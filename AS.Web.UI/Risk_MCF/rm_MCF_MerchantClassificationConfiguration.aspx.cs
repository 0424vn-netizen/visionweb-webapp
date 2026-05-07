using System;
using System.Data;
using Telerik.Web.UI;
using AS.Common;
using System.Drawing;
using AS.Common.DBManager;
using Resources;
using AS.Controls.Exporter;
using AS.Controls.UserControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using AS.Controls.Pages;
using System.Linq;
using System.Web.Services;
using AS.Common.Formater;

[PagePermission("RskMerClass,MSRskMerClass")]
public partial class rm_MCF_MerchantClassificationConfiguration : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindDataGroups
    }

    enum PostBackAction
    {
        OpenFormCreate,
        OpenFormEdit,
        CreateAction,
        UpdateAction,
        CancelAction,
        ActivateDeactivate,
        ChangeFilter,
        OpenMultiplierSetting
    }

    #endregion

    #region Properties
    private string _urlCreateClassification = "";

    #endregion


    #region Overrides

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxGroupList");
        this.ExporterIDs.Add("uxExportTop");
        base.PageInitialize();
        IsBindDataOnLoad = true;
        this.IsSecureCSRF = true;
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxGroupList)
        {
            OnDataBindControls(DataBindAction.BindDataGroups, sender);
        }
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (sender != uxGroupList) return;
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            var activateDeactivateItem = dataItem["ActivateDeactivate"];

            var rowData = e.Item.DataItem as DataRowView;
            int groupID = int.Parse(rowData["MerchantClassificationID"].ToString());
            var groupNameItem = dataItem["MerchantClassificationName"];

            bool isActive = rowData["IsActive"].ToBoolean();
            string encodeID = CryptorServices.Current.EncryptText(rowData["MerchantClassificationID"].ToString());

            activateDeactivateItem.Text = VeraCodeSolution.DoVeraCode(string.Format("<a href=\"#\" onclick=\"ActivateDeactivate('{0}', {1}); return false;\">{2}</a>",
               encodeID, (!isActive).ToString().ToLower(), (isActive ? GetLocalResourceObject("rm_MerchantClassification_aspx_cs_Deactivate").ToString() : GetLocalResourceObject("rm_MerchantClassification_aspx_cs_Active").ToString())));

            string queryString = BuildSecureQueryString("ClassId=" + rowData["MerchantClassificationID"]);
            string urlClassDetail = "rm_MCF_MerchantClassification_CreateNewModal.aspx?" + queryString;
            string url = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return ShowPopupModal('" + urlClassDetail + "','auto');\">" + GetLocalResourceObject("LinkButton1Resource1.Text") + "</a>";

            Literal uxLink = e.Item.FindControl("btnEdit") as Literal;
            uxLink.Text = url;

            string pageView = "rm_MCF_MerchantClassificationConfiguration_ViewModal.aspx?" + queryString;
            string urlView = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return ShowPopupModal('" + pageView + "','auto');\">" + groupNameItem.Text + "</a>";
            groupNameItem.Text = urlView;

            var multiplier = dataItem["Multiplier"];
            multiplier.Text = FormatData.FormatNumber(rowData["Multiplier"], 1);

        }
    }


    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = uxExportTop.GridHeader;
        if (Request.Browser.Browser.ToUpper() == WebSiteConstants.BROWSER_INTERNETEXPLORER)
        {
            exportConfig.FileName = Server.UrlPathEncode(GeneralFuncsLib.FormatFileName(uxExportTop.GridHeader));
        }
        else
        {
            exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExportTop.GridHeader);
        }
        uxGroupList.Columns.FindByUniqueName("Edit").Visible = false;
        // Spa should return more IsActiveText for exporting
        uxGroupList.Columns.FindByUniqueName("ActivateDeactivate").Visible = false;

    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected) return;

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDataGroups:
                {
                    if (string.IsNullOrEmpty(uxGroupList.AS_SortExpression))
                        uxGroupList.AS_SortExpression = "MerchantClassificationName ASC";

                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParamsWithRecId();
                    parameters.AddLanguageID();
                    parameters.Add(new FilterParameter("@IsActive", this.FilterStatus, DbType.Int32));
                    parameters.Add(new FilterParameter("@ClassificationName", GeneralFuncsLib.ReplaceSpecialCharacter(this.uxClassificationName.Text), DbType.String));
                    parameters.Add(new FilterParameter("@AttributeList", this.GetAttributes(), DbType.String));
                    parameters.Add(new FilterParameter("@stOrder", uxGroupList.AS_SortExpression, DbType.String));

                    DataTable table = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_MerchantClassificationList", parameters);
                    uxGroupList.DataSource = table;
                    if (table.Rows.Count == 0)
                        uxGroupList.AllowSorting = false;
                    else
                        uxGroupList.AllowSorting = true;
                }
                break;
        }
    }

    private void ActivteDeactivate(int recordID, bool isActive)
    {


        FilterParameterCollection parameters = new FilterParameterCollection();        
        parameters.AddLoggedInUserParamsWithRecId();
        parameters.Add(new FilterParameter("@ClassificationID", recordID, DbType.Int32));
        parameters.Add(new FilterParameter("@Status", isActive, DbType.Boolean));
        parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_MRS_Update_StatusMIFClassification", parameters, out parameterOut);

        int result = Convert.ToInt32(parameterOut[0].ParameterValue);
        // Success
        if (result == 0)
        {

        }

        uxGroupList.Rebind();
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {

            case PostBackAction.ActivateDeactivate:
                {
                    string message = string.Empty;
                    string[] parts = uxActivateDeactivateData.Value.Split(';');
                    int recordID =CryptorServices.Current.DecryptText(parts[0]).ToInt();
                    bool isActive = parts[1].ToBoolean();

                    ActivteDeactivate(recordID, isActive);
                }
                break;
            case PostBackAction.ChangeFilter:
                {
                    uxGroupList.Rebind();
                }
                break;

        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        if (!IsPostBack)
        {
            DoBindData();
        }

    }

    private string GetAttributes()
    {

        string collection = string.Empty;
        var items = this.uxAttributeList.CheckedItems;

        if (items.Count != 0)
        {
            collection = string.Join(",", items.Select(t => t.Value));
        }

        return collection;
    }

    private void DoBindData()
    {
        FilterParameterCollection prams = new FilterParameterCollection();
        prams.AddLoggedInUserParamsWithRecId();
        uxAttributeList.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_AttributeList", prams);
        uxAttributeList.DataBind();
        uxClassificationName.EmptyMessage = GetLocalResourceObject("uxFilterClassification.Text").ToString();

    }

    [WebMethod(EnableSession = true)]
    public static string[] CheckClassificationInUsing(string classId)
    {
        string message = string.Empty;
        DataTable tb = new DataTable();
        classId = CryptorServices.Current.DecryptText(classId);
        FilterParameterCollection prams = new FilterParameterCollection();
        prams.AddLoggedInUserParamsWithRecId();
        prams.Add(new FilterParameter("@ClassificationID", classId, DbType.Int32));

        tb = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_AssignmentUseByMIFClassification", prams);
        if (tb != null && tb.Rows.Count > 0)
        {
            message = tb.Rows[0].Field<string>(0) ?? string.Empty;
        }

        return new string[] { string.IsNullOrEmpty(message).ToString().ToLower(), message };
    }

    protected void uxGroupList_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (IsIntruderDetected) return;

        if (e.CommandName == RadGrid.EditCommandName)
            OnPostBackActions(PostBackAction.OpenFormEdit, e);
        else if (e.CommandName == RadGrid.UpdateCommandName)
        {
            OnPostBackActions(PostBackAction.UpdateAction, e);
        }
    }

    protected void uxChangeFilterStatus_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeFilter);
    }

    private int FilterStatus
    {
        get
        {
            int filterStatusValue = -1;

            foreach (Control ctl in uxFilterStatusContainer.Controls)
            {
                if (ctl is RadioButton)
                {
                    RadioButton radioButton = ctl as RadioButton;

                    if (radioButton.Checked)
                    {
                        if (Int32.TryParse(radioButton.Attributes["xValue"], out filterStatusValue))
                        {
                            return filterStatusValue;
                        }
                    }
                }
            }

            return filterStatusValue;
        }

        set
        {
            int filterStatusValue = -1;

            foreach (Control ctl in uxFilterStatusContainer.Controls)
            {
                if (ctl is RadioButton)
                {
                    RadioButton radioButton = ctl as RadioButton;

                    if (Int32.TryParse(radioButton.Attributes["xValue"], out filterStatusValue))
                    {
                        if (filterStatusValue == value)
                            radioButton.Checked = true;
                        else
                            radioButton.Checked = false;
                    }
                }
            }

            if (this.FilterStatus < 0)
                uxFilterStatusAll.Checked = true;
        }
    }

    protected void uxActivateDeactivate_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ActivateDeactivate);
    }

    protected void uxSearchButton_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeFilter);
    }

    protected void litMultiplierSetting_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.OpenMultiplierSetting);
    }

    protected void uxCreateMode_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.OpenFormCreate);
    }
    protected void btnRebind_Click(object sender, EventArgs e)
    {
        uxGroupList.Rebind();
    }

}
