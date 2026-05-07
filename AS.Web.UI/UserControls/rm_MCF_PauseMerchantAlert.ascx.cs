using AS.Common.DBManager;
using AS.Common.Logger;
using AS.VW.Audit;
using AS.VW.Audit.Models;
using AS.VW.Entities.PauseMerchantAlert;
using AS.Web.Business.PauseMerchantAlert.Impl;
using AS.Web.Business.PauseMerchantAlert.Interfaces;
using AS.Web.Business.PauseMerchantAlert.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using HiddenField = AS.Controls.Global.HiddenField;
using LinkButton = AS.Controls.Global.LinkButton;
using RadDatePicker = AS.Controls.Global.RadDatePicker;

public partial class UserControls_rm_MCF_PauseMerchantAlert : GlobalUserControl
{
    private readonly IPauseMerchantAlertBusiness _pauseMerchantAlertBusiness;

    public UserControls_rm_MCF_PauseMerchantAlert() : this(new PauseMerchantAlertBusiness(WebServices.RiskServices))
    {
    }

    public UserControls_rm_MCF_PauseMerchantAlert(IPauseMerchantAlertBusiness pauseMerchantAlertBusiness)
    {
        _pauseMerchantAlertBusiness = pauseMerchantAlertBusiness;
    }

    public string PrimaryID { get; set; }

    enum DataBindAction
    {
        BindPauseAlertFilterList
    }

    public WebSiteEnums.FeatureMode FeatureMode { get; set; }

    public bool IsCreateNewAssignment { get; set; }

    public DataTable CurrentPauseAlertFilterDataTable
    {
        get
        {
            return (DataTable)Session["CurrentPauseAlertFilterDataTable"];
        }
        set
        {
            Session["CurrentPauseAlertFilterDataTable"] = value;
        }
    }

    public DataTable OriginalPauseAlertFilterDataTable
    {
        get
        {
            return (DataTable)Session["OriginalPauseAlertFilterDataTable"];
        }
        set
        {
            Session["OriginalPauseAlertFilterDataTable"] = value;
        }
    }

    private void UpdateCurrentPauseAlertFilterDataTable()
    {
        foreach (RepeaterItem item in uxPauseMerchantAlertFilterRepeater.Items)
        {
            RadDatePicker uxFromDate = item.FindControl("uxFromDate") as RadDatePicker;
            RadDatePicker uxToDate = item.FindControl("uxToDate") as RadDatePicker;
            RadMultiSelect radMultiSelect = item.FindControl("RadMultiSelect1") as RadMultiSelect;
            HiddenField hddRowGuid = item.FindControl("hddRowGuid") as HiddenField;

            var values = radMultiSelect.Value;

            DataRow dr = GetDataRow(CurrentPauseAlertFilterDataTable, "RowGUID", hddRowGuid.Value);

            if (uxFromDate.SelectedDate != null)
            {
                dr["FromDate"] = uxFromDate.SelectedDate;
            }
            else
            {
                dr["FromDate"] = DBNull.Value;
            }


            if (uxToDate.SelectedDate != null)
            {
                dr["ToDate"] = uxToDate.SelectedDate;
            }
            else
            {
                dr["ToDate"] = DBNull.Value;
            }

            if (values != null)
            {
                dr["MerchantID"] = string.Join(",", values);
            }

        }
    }

    private static DataRow GetDataRow(DataTable table, string columnName, string value)
    {
        if (table.Columns.Contains(columnName))
        {
            foreach (DataRow row in table.Rows)
            {
                if (row[columnName] != DBNull.Value && row[columnName].ToString() == value)
                {
                    return row;
                }
            }
        }

        return null;
    }

    private void AddNewRow()
    {
        var dt = CurrentPauseAlertFilterDataTable;
        var dr = dt.NewRow();

        var guid = Guid.NewGuid().ToString();
        var fromDate = DateTime.Now;

        dr["FromDate"] = fromDate;
        dr["MerchantID"] = string.Empty;
        dr["RowGUID"] = guid;

        dt.Rows.Add(dr);
        CurrentPauseAlertFilterDataTable = dt;
    }

    private void DeleteRow(string rowGuid, string assignmentID)
    {
        var dt = CurrentPauseAlertFilterDataTable;

        DataRow dr = GetDataRow(CurrentPauseAlertFilterDataTable, "RowGUID", rowGuid);
        dt.Rows.Remove(dr);

        CurrentPauseAlertFilterDataTable = dt;

        if (!string.IsNullOrEmpty(assignmentID))
        {
            _pauseMerchantAlertBusiness.DeleteAssignmentPauseDateRange(GeneralFuncsLib.GetUserMode(), SessionManager.CurrentUser, assignmentID, rowGuid);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ClearSessionData();
            InitData();
            VisibleControls();
        }
    }

    private void InitData()
    {
        DataTable dt = CurrentPauseAlertFilterDataTable;
        if (dt == null || dt.Rows.Count == 0)
        {
            dt = GetPauseMerchantAlertFilters();
        }
        CurrentPauseAlertFilterDataTable = dt;
        OriginalPauseAlertFilterDataTable = dt.Copy();

        uxPauseMerchantAlertFilterRepeater.DataSource = dt;
        uxPauseMerchantAlertFilterRepeater.DataBind();

        GetMerchantsSelectedCallback(dt);
    }

    private void GetMerchantsSelectedCallback(DataTable dt)
    {
        List<List<MerchantRefModel>> result = new List<List<MerchantRefModel>>();
        foreach (DataRow row in dt.Rows)
        {
            var merchants = new List<MerchantRefModel>();
            var merchantIds = row["MerchantID"].ToString();

            LoggerManager.Info(string.Format("GetMerchantsSelectedCallback - Start Find dtMerchantFind - search key: {0}", merchantIds));
            var dtMerchantFind = _pauseMerchantAlertBusiness.GetAllMerchants(merchantIds, GeneralFuncsLib.GetUserMode(), SessionManager.CurrentUser);
            if (dtMerchantFind != null && dtMerchantFind.Rows.Count > 0)
            {
                foreach (DataRow merchant in dtMerchantFind.Rows)
                {
                    merchants.Add(new MerchantRefModel
                    {
                        DataKey = merchant["DataKey"].ToString(),
                        DataText = merchant["DataText"].ToString()
                    });
                }
            }

            if (merchants.Any())
            {
                result.Add(merchants);
            }
        }

        LoggerManager.Info(string.Format("GetMerchantsSelectedCallback - selected merchants: {0}", JsonConvert.SerializeObject(result)));

        string script = "var valueStoreServer = " + JsonConvert.SerializeObject(result);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DataScript", script, true);
    }

    private void VisibleControls()
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            hddIsViewMode.Value = "true";
            foreach (RepeaterItem item in uxPauseMerchantAlertFilterRepeater.Items)
            {
                RadDatePicker uxFromDate = item.FindControl("uxFromDate") as RadDatePicker;
                RadDatePicker uxToDate = item.FindControl("uxToDate") as RadDatePicker;
                LinkButton btnDelete = item.FindControl("btnDelete") as LinkButton;

                uxFromDate.Enabled = false;
                uxToDate.Enabled = false;
                btnDelete.Visible = false;
            }
        }
        else
        {
            hddIsViewMode.Value = "false";
        }
    }

    private void ClearSessionData()
    {
        CurrentPauseAlertFilterDataTable = null;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindPauseAlertFilterList:
                {
                    DataTable dt = CurrentPauseAlertFilterDataTable;
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        dt = GetPauseMerchantAlertFilters();
                    }
                    CurrentPauseAlertFilterDataTable = dt;

                    uxPauseMerchantAlertFilterRepeater.DataSource = dt;
                    uxPauseMerchantAlertFilterRepeater.DataBind();
                    break;
                }
        }
    }

    private DataTable GetPauseMerchantAlertFilters()
    {
        DataTable dt = _pauseMerchantAlertBusiness.GetPauseMerchantAlertFilters(GeneralFuncsLib.GetUserMode(), SessionManager.CurrentUser, PrimaryID);

        if (dt != null && dt.Rows != null)
        {
            foreach (DataRow row in dt.Rows)
            {
                object merchantIdsObj = row["MerchantID"];
                if (merchantIdsObj == null || string.IsNullOrEmpty(merchantIdsObj.ToString()))
                {
                    continue;
                }
                string merchantIds = merchantIdsObj.ToString();
                if (!merchantIds.Contains(","))
                {
                    continue;
                }
                string sortedMerchantIds = string.Join(",", merchantIds.Split(',').OrderBy(id => id));
                row["MerchantID"] = sortedMerchantIds;
            }
        }

        return dt;
    }

    protected void uxPauseMerchantAlertFilterRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = e.Item.DataItem as DataRowView;

            RadDatePicker uxFromDate = e.Item.FindControl("uxFromDate") as RadDatePicker;
            if (drv["FromDate"] != DBNull.Value)
            {
                uxFromDate.SelectedDate = (DateTime)drv["FromDate"];
            }
            else
            {
                uxFromDate.SelectedDate = null;
            }

            RadDatePicker uxToDate = e.Item.FindControl("uxToDate") as RadDatePicker;
            if (drv["ToDate"] != DBNull.Value)
            {
                uxToDate.SelectedDate = (DateTime)drv["ToDate"];
            }
            else
            {
                uxToDate.SelectedDate = null;
            }

            HiddenField hddRowGuid = e.Item.FindControl("hddRowGuid") as HiddenField;
            hddRowGuid.Value = drv["RowGUID"].ToString();

            HiddenField hddMerchantsSelected = e.Item.FindControl("hddMerchantsSelected") as HiddenField;
            hddMerchantsSelected.Value = drv["MerchantID"].ToString();
        }
    }

    public void AddFilter()
    {
        UpdateCurrentPauseAlertFilterDataTable();
        AddNewRow();
        OnDataBindControls(DataBindAction.BindPauseAlertFilterList);
    }

    protected void uxPauseMerchantAlertFilterRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete" && e.CommandArgument.ToString() != "")
        {
            var args = GeneralFuncsLib.ConvertStringToList(e.CommandArgument.ToString(), ';', false);
            var rowGuid = args[0];
            var assignmentID = args[1];

            UpdateCurrentPauseAlertFilterDataTable();
            DeleteRow(rowGuid, assignmentID);
            OnDataBindControls(DataBindAction.BindPauseAlertFilterList);
        }
    }

    private int GetSubstract2Days(DateTime dtFromDate, DateTime dtToDate)
    {
        int days = dtFromDate.Subtract(dtToDate).Days;
        return days;
    }

    private bool IsValid()
    {
        bool isValid = true;
        foreach (RepeaterItem item in uxPauseMerchantAlertFilterRepeater.Items)
        {
            RadDatePicker uxFromDate = item.FindControl("uxFromDate") as RadDatePicker;
            RadDatePicker uxToDate = item.FindControl("uxToDate") as RadDatePicker;

            RadMultiSelect radMultiSelect = item.FindControl("RadMultiSelect1") as RadMultiSelect;
            var values = radMultiSelect.Value;

            // FromDate, ToDate
            if (uxFromDate.SelectedDate.HasValue && uxToDate.SelectedDate.HasValue)
            {
                DateTime fromDate = uxFromDate.SelectedDate.Value;
                DateTime toDate = uxToDate.SelectedDate.Value;

                var currentDate = DateTime.Now;

                // Check two flags including !IsCreateNewAssignment because FeatureMode = 'Edit' in both Create and Edit modes
                var isEditMode = FeatureMode == WebSiteEnums.FeatureMode.Edit && !IsCreateNewAssignment;

                if (!isEditMode && (GetSubstract2Days(fromDate, currentDate) < 0 || GetSubstract2Days(toDate, currentDate) < 0))
                {
                    isValid = false;
                }
                else
                {
                    if (GetSubstract2Days(toDate, fromDate) < 0)
                    {
                        isValid = false;
                    }
                }
            }
            else
            {
                isValid = false;
            }

            //Merchants
            if (values == null || !values.Any())
            {
                isValid = false;
            }
        }

        return isValid;
    }

    public int Save()
    {
        if (!ValidatePauseMerchantAlert())
        {
            LoggerManager.Info("pauseMerchantAlert - Save - ValidatePauseMerchantAlert() = false");
            return -1;
        }

        var list = new List<PauseMerchantAlertModel>();
        foreach (DataRow row in CurrentPauseAlertFilterDataTable.Rows)
        {
            var item = new PauseMerchantAlertModel
            {
                FromDate = (DateTime)row["FromDate"],
                ToDate = (DateTime)row["ToDate"],
                MerchantId = row["MerchantID"].ToString(),
                RowGuid = row["RowGUID"].ToString()
            };
            list.Add(item);
        }

        var data = JsonConvert.SerializeObject(list, Formatting.Indented);

        _pauseMerchantAlertBusiness.SaveAssignmentPauseDateRange(GeneralFuncsLib.GetUserMode(), SessionManager.CurrentUser, PrimaryID, data);

        return 0;
    }

    private bool ValidatePauseMerchantAlert()
    {
        UpdateCurrentPauseAlertFilterDataTable();
        var isValid = IsValid();
        return isValid;
    }

    public string GetAuditEntities(string auditEntities)
    {
        var isEditMode = FeatureMode == WebSiteEnums.FeatureMode.Edit && !IsCreateNewAssignment;
        if (!isEditMode)
        {
            return auditEntities;
        }

        var result = string.Empty;

        List<AuditEntityLegacyModel> entities = new List<AuditEntityLegacyModel>();

        if (!string.IsNullOrEmpty(auditEntities))
        {
            entities = JsonConvert.DeserializeObject<List<AuditEntityLegacyModel>>(auditEntities);
        }

        var columsFormat = new Dictionary<string, string>
        {
            { "FromDate", "MM/dd/yyyy" },
            { "ToDate", "MM/dd/yyyy" },
            { "Merchants", "{0}" }
        };

        var dtComparison = new DataTableOneLineAudit("{1} - {2}: {3}", columsFormat);

        var pauseMerchantAlertsAudit = dtComparison.CompareEntities(OriginalPauseAlertFilterDataTable, CurrentPauseAlertFilterDataTable, "RowGUID", new string[] { "RowGUID", "FromDate", "ToDate", "MerchantID" });
        LoggerManager.Info(string.Format("pauseMerchantAlertsAuditLog: {0}", JsonConvert.SerializeObject(pauseMerchantAlertsAudit)));

        foreach (var item in pauseMerchantAlertsAudit)
        {
            entities.Add(new AuditEntityLegacyModel
            {
                Key = "PauseMerchantAlert",
                KeyLang = "PauseMerchantAlert",
                NewValues = item.NewValues,
                OldValues = item.OldValues
            });
        }

        result = JsonConvert.SerializeObject(entities);

        return result;
    }
}