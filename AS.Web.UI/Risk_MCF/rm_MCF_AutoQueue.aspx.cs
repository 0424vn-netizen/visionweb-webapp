using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.Services;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

[PagePermission("RskAutoQueue,MSRskAutoQueue")]
public partial class rm_MCF_AutoQueue : ReportPage
{
    #region Fields
    private const string FmDateTime = "MM/dd/yyyy hh:mm tt";
    protected RiskAutoQueueModel AutoQueueItem
    {
        get
        {
            if (RiskSessionManager.AutoQueue != null)
            {
                return RiskSessionManager.AutoQueue;
            }

            return new RiskAutoQueueModel();
        }
    }
    protected bool IsActiveSelectAq { get; set; }
    private const string TextRunningNow = "Running Now";
    private const string TextNow = "Now";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ucDetail.Visible = RiskSessionManager.AutoQueue == null ? false : true;

        if (!IsPostBack)
        {
            IsActiveSelectAq = true;
            RiskSessionManager.AutoQueue = null;
        }
    }

    #region Function
    protected DataRow GetAutoQueueItem(long queueId)
    {
        DataTable data = Rm_AutoQueueBusiness.GetAutoQueueList(null, queueId, true);
        if (data.HasData())
        {
            return data.Rows[0];
        }

        return null;
    }

    protected DataRow GetCurSelectedQueueItem(long queueID)
    {
        if (RiskSessionManager.AutoQueueList.HasData())
        {
            foreach (DataRow row in RiskSessionManager.AutoQueueList.Rows)
            {
                if (row["AutoQueueID"].ToInt() == queueID)
                {
                    return row;
                }
            }
        }

        return null;
    }

    protected DataTable CreateDataForListQueue(bool isActive)
    {
        DataTable dataList = Rm_AutoQueueBusiness.GetAutoQueueList(isActive, null, true);
        dataList.Columns.Add("ClassUI", typeof(string));
        dataList.Columns.Add("lastRunText", typeof(string));
        foreach (DataRow row in dataList.Rows)
        {
            if (row["IsIncludedAggregateQueue"].ToBoolean())
            {
                row["ClassUI"] = "check-green";
            }
            else
            {
                row["ClassUI"] = "check";
            }

            string textRun = row["LastRun"].ToString();
            if (textRun.Equals(TextRunningNow)) textRun = GetLocalResourceObject("textRunning").ToString();
            if (textRun.Equals(TextNow)) textRun = GetLocalResourceObject("textNow").ToString();
            string LastEnd = row["LastEndDTS"].ToString();
            string lastRun = string.Format("<span class=\"font-weight-bold\">{0}</span>{1}", textRun,
                Rm_AutoQueueBusiness.ConvertDateTimeAQ(LastEnd, FmDateTime));
            if (string.IsNullOrEmpty(textRun) && string.IsNullOrEmpty(LastEnd)) lastRun = "--";
            row["lastRunText"] = lastRun;
        }

        dataList.DefaultView.Sort = "ProcessingOrder";
        dataList = dataList.DefaultView.ToTable();
        return dataList;
    }

    protected void SetSelectItem(DataRow item, bool isNew = false)
    {
        RiskAutoQueueModel selectItem = new RiskAutoQueueModel();
        if (item != null)
        {
            selectItem.ID = item["AutoQueueID"].ToInt();
            selectItem.Index = item["ProcessingOrder"].ToInt();
            selectItem.IsActive = item["IsActived"].ToBoolean();
            selectItem.Name = item["AutoQueueName"].ToString();
            selectItem.lastRun = item["LastRun"].ToString();
            selectItem.lastRunDts = item["LastEndDTS"].ToString();
            selectItem.lastRunText = selectItem.lastRun;
            selectItem.CreateOn = Rm_AutoQueueBusiness.ConvertDateTimeAQ(item["CreatedDTS"].ToString(), FmDateTime);
            selectItem.Description = item["AutoQueueDesc"].ToString();
            string textRun = item["LastRun"].ToString();
            if (textRun.Equals(TextRunningNow)) textRun = GetLocalResourceObject("textRunning").ToString();
            if (textRun.Equals(TextNow)) textRun = GetLocalResourceObject("textNow").ToString();
            string lastRunText = (string.IsNullOrEmpty(textRun) && string.IsNullOrEmpty(selectItem.lastRunDts)) ? "--"
                : string.Format("<span class=\"font-weight-bold\">{0}</span>{1}", textRun, Rm_AutoQueueBusiness.ConvertDateTimeAQ(selectItem.lastRunDts, FmDateTime));

            selectItem.lastRunText = isNew
                ? lastRunText
                : item["lastRunText"].ToString();
        }

        RiskSessionManager.AutoQueueStatus = SetCurQueueStatus(selectItem.lastRun, selectItem.lastRunDts);
        RiskSessionManager.AutoQueue = selectItem;
        ucDetail.Visible = RiskSessionManager.AutoQueue == null ? false : true;
    }

    public static string SetCurQueueStatus(string lastRun, string LastEndDTS)
    {
        string aqStatus = "";
        if (lastRun.Equals("--")) lastRun = "";
        if (string.IsNullOrEmpty(LastEndDTS) && string.IsNullOrEmpty(lastRun)) aqStatus = "start";
        if (!string.IsNullOrEmpty(lastRun))
        {
            if (lastRun.Equals(TextRunningNow)) aqStatus = "now";
            if (lastRun.Equals(TextNow)) aqStatus = "last";
        }
        if (!string.IsNullOrEmpty(LastEndDTS) && string.IsNullOrEmpty(lastRun)) aqStatus = "stop";

        return aqStatus;
    }

    private bool CompareData(DataRow oldData, DataRow newData)
    {
        return (oldData["LastRun"].ToString().Equals(newData["LastRun"].ToString()) &&
            oldData["LastEndDTS"].ToString().Equals(newData["LastEndDTS"].ToString()));
    }

    protected bool UpdateOrderAutoQueue(DataRow queueData, long newIndex)
    {
        return Rm_AutoQueueBusiness.UpdateOrderAutoQueue(queueData, newIndex, true);
    }

    protected void ReLoadActiveGrid()
    {
        DataTable dataList = CreateDataForListQueue(true);
        ActiveAQGrid.DataSource = dataList;
        ActiveAQGrid.Rebind();
        RiskSessionManager.AutoQueueList = dataList;

        if (dataList.HasData())
        {
            // ReSelect item 
            ReSelectGrid(ActiveAQGrid, RiskSessionManager.AutoQueue.ID.ToString());
            SetSelectItem(GetAutoQueueItem(RiskSessionManager.AutoQueue.ID), true);

        }
    }

    protected void ReLoadPerOrder(bool isActived, long gridID)
    {
        DataTable dataList = CreateDataForListQueue(true);
        ActiveAQGrid.DataSource = dataList;
        ActiveAQGrid.Rebind();
        RiskSessionManager.AutoQueueList = dataList;

        DataTable dataListInactive = CreateDataForListQueue(false);
        InActiveAQGrid.DataSource = dataListInactive;
        InActiveAQGrid.Rebind();

        if (isActived)
        {
            if (dataList.HasData())
            {
                // ReSelect item 
                if (!ReSelectGrid(ActiveAQGrid, gridID.ToString()))
                {
                    ReSelectGrid(InActiveAQGrid, gridID.ToString());
                }

                SetSelectItem(GetAutoQueueItem(gridID), true);
            }
        }
        else
        {
            if (dataListInactive.HasData())
            {
                // ReSelect item 
                if (!ReSelectGrid(InActiveAQGrid, gridID.ToString()))
                {
                    ReSelectGrid(ActiveAQGrid, gridID.ToString());
                }

                SetSelectItem(GetAutoQueueItem(gridID), true);
            }
        }
    }

    protected void ReLoadAllData()
    {
        long CurQueueId = RiskSessionManager.AutoQueue.ID;
        DataRow newData = GetAutoQueueItem(CurQueueId);
        if (newData == null) return;
        bool isActive = newData["IsActived"].ToBoolean();

        DataTable dataList = CreateDataForListQueue(true);
        ActiveAQGrid.DataSource = dataList;
        ActiveAQGrid.Rebind();
        RiskSessionManager.AutoQueueList = dataList;

        DataTable dataListInactive = CreateDataForListQueue(false);
        InActiveAQGrid.DataSource = dataListInactive;
        InActiveAQGrid.Rebind();

        if (isActive)
        {
            if (dataList.HasData())
            {
                InActiveAQGrid.SelectedIndexes.Clear();

                // ReSelect item 
                ReSelectGrid(ActiveAQGrid, CurQueueId.ToString());
            }
        }
        else
        {
            if (dataListInactive.HasData())
            {
                ActiveAQGrid.SelectedIndexes.Clear();

                // ReSelect item 
                ReSelectGrid(InActiveAQGrid, CurQueueId.ToString());
            }
        }

        AjaxAddResponseScript("aqModule.HideBoder('0');");
        SetSelectItem(newData, true);
        ucDetail.BindDataDetail(CurQueueId);

    }

    private bool ReSelectGrid(ASGrid gridControl, string gridId)
    {
        foreach (GridDataItem item in gridControl.MasterTableView.Items)
        {
            if (item["AutoQueueID"].Text.Equals(gridId))
            {
                item.Selected = true;
                return true;
            }
        }

        return false;
    }

    #endregion

    #region Page events
    protected void OnRowDrop(object sender, GridDragDropEventArgs e)
    {
        if (e.DestDataItem != null)
        {
            var pos = e.DropPosition;

            // Item from
            var draItem = e.DraggedItems[0];
            long draID = draItem["AutoQueueID"].Text.ToLong();

            // Item to
            var destItem = e.DestDataItem;
            string destIsActived = destItem["IsActived"].Text.ToString();
            if (!destIsActived.Equals("Yes")) return;
            int destIndex = 0;
            if (Int32.TryParse(destItem["ProcessingOrder"].Text, out destIndex))
            {
                DataRow itemUpdate = GetCurSelectedQueueItem(draID);

                // Update
                if (!UpdateOrderAutoQueue(itemUpdate, destIndex))
                {
                    string msgOutUpdateAQ = GetLocalResourceObject("textAqreLoadMessage")
                   .ToString()
                   .Replace("[AutoQueueName]", itemUpdate["AutoQueueName"].ToString());

                    AjaxAddResponseScript("aqModule.OpenAqMsg('" + msgOutUpdateAQ + "')");
                    ReLoadPerOrder(true, draID);
                    ucDetail.BindDataDetail(RiskSessionManager.AutoQueue.ID);
                    return;
                }

                DataTable data = CreateDataForListQueue(true);
                ActiveAQGrid.DataSource = data;
                RiskSessionManager.AutoQueueList = data;
                ActiveAQGrid.Rebind();

                // ReSelect item 
                foreach (GridDataItem item in ActiveAQGrid.MasterTableView.Items)
                {
                    if (item["AutoQueueID"].Text.Equals(itemUpdate["AutoQueueID"].ToString()))
                    {
                        item.Selected = true;
                        break;
                    }
                }
                InActiveAQGrid.SelectedIndexes.Clear();
                SetSelectItem(itemUpdate);
                ucDetail.BindDataDetail(RiskSessionManager.AutoQueue.ID);
            }

        }
    }

    protected void ActiveAQGrid_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "RowClick")
        {
            IsActiveSelectAq = true;
            bool IsReload = false;
            DataRow selectData = null;
            foreach (GridDataItem item in ActiveAQGrid.SelectedItems)
            {
                if (item != null)
                {
                    DataRow newData = GetAutoQueueItem(item["AutoQueueID"].Text.ToInt());
                    selectData = GetCurSelectedQueueItem(item["AutoQueueID"].Text.ToInt());

                    if (selectData != null && newData != null)
                    {
                        IsReload = !CompareData(selectData, newData);
                    }

                    SetSelectItem(newData, true);
                    break;
                }
            }

            if (IsReload)
            {
                ReLoadActiveGrid();
            }

            InActiveAQGrid.SelectedIndexes.Clear();
            ucDetail.BindDataDetail(RiskSessionManager.AutoQueue.ID);
        }
    }

    protected void ActiveAQGrid_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dataList = CreateDataForListQueue(true);

        ActiveAQGrid.DataSource = dataList;
        RiskSessionManager.AutoQueueList = dataList;

        if (dataList.HasData())
        {
            if (RiskSessionManager.AutoQueue == null)
            {
                ActiveAQGrid.SelectedIndexes.Add(0);
                SetSelectItem(RiskSessionManager.AutoQueueList.Rows[0]);
            }
            else
            {
                foreach (GridDataItem item in ActiveAQGrid.MasterTableView.Items)
                {
                    if (item["AutoQueueID"].Text.Equals(AutoQueueItem.ID.ToString()))
                    {
                        item.Selected = true;
                        break;
                    }
                }

                SetSelectItem(GetCurSelectedQueueItem(AutoQueueItem.ID));
            }

            ucDetail.BindDataDetail(RiskSessionManager.AutoQueue.ID);
        }
    }

    protected void InActiveAQGrid_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dataList = CreateDataForListQueue(false);
        InActiveAQGrid.DataSource = dataList;
        if (RiskSessionManager.AutoQueueList == null || RiskSessionManager.AutoQueueList.Rows.Count == 0)
        {
            if (dataList.HasData())
            {
                InActiveAQGrid.SelectedIndexes.Add(0);
                SetSelectItem(dataList.Rows[0]);
                ucDetail.BindDataDetail(RiskSessionManager.AutoQueue.ID);
            }
        }

    }

    protected void ActiveAQGrid_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //Sprint 2 - TK44078 - Remove Code
            DataTable dtb = ActiveAQGrid.DataSource as DataTable;
            if (dtb.Rows.Count > 5)
            {
                ActiveAQGrid.ClientSettings.Scrolling.AllowScroll = true;
                ActiveAQGrid.ClientSettings.Scrolling.ScrollHeight = Unit.Pixel(250);
            }
            else
            {
                ActiveAQGrid.ClientSettings.Scrolling.AllowScroll = false;
            }
        }
    }

    protected void InActiveAQGrid_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //Sprint 2 - TK44078 - Remove Code
            DataTable dtb = InActiveAQGrid.DataSource as DataTable;
            if (dtb.Rows.Count > 5)
            {
                InActiveAQGrid.ClientSettings.Scrolling.AllowScroll = true;
                InActiveAQGrid.ClientSettings.Scrolling.ScrollHeight = Unit.Pixel(250);
            }
            else
            {
                InActiveAQGrid.ClientSettings.Scrolling.AllowScroll = false;
            }
        }
    }

    protected void InActiveAQGrid_OnRowDrop(object sender, GridDragDropEventArgs e)
    {
        if (e.DestDataItem != null)
        {
            var pos = e.DropPosition;

            // Item from
            var draItem = e.DraggedItems[0];
            int draID = draItem["AutoQueueID"].Text.ToInt();

            // Item to
            var destItem = e.DestDataItem;
            string destIsActived = destItem["IsActived"].Text.ToString();
            if (!destIsActived.Equals("No")) return;
            int destIndex = 0;
            if (Int32.TryParse(destItem["ProcessingOrder"].Text, out destIndex))
            {
                DataRow itemUpdate = GetAutoQueueItem(draID);
                itemUpdate["ProcessingOrder"] = draItem["ProcessingOrder"].Text;

                // Update
                if (!UpdateOrderAutoQueue(itemUpdate, destIndex))
                {
                    string msgOutUpdateAQ = GetLocalResourceObject("textAqreLoadMessage")
                   .ToString()
                   .Replace("[AutoQueueName]", itemUpdate["AutoQueueName"].ToString());

                    AjaxAddResponseScript("aqModule.OpenAqMsg('" + msgOutUpdateAQ + "')");
                    ReLoadPerOrder(false, draID);
                    ucDetail.BindDataDetail(RiskSessionManager.AutoQueue.ID);
                    return;
                }

                DataTable data = CreateDataForListQueue(false);
                InActiveAQGrid.DataSource = data;
                InActiveAQGrid.Rebind();

                // ReSelect item 
                foreach (GridDataItem item in InActiveAQGrid.MasterTableView.Items)
                {
                    if (item["AutoQueueID"].Text.Equals(itemUpdate["AutoQueueID"].ToString()))
                    {
                        item.Selected = true;
                        break;
                    }
                }
                ActiveAQGrid.SelectedIndexes.Clear();
                SetSelectItem(itemUpdate, true);
                ucDetail.BindDataDetail(RiskSessionManager.AutoQueue.ID);
            }

        }
    }

    protected void InActiveAQGrid_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        IsActiveSelectAq = false;
        foreach (GridDataItem item in InActiveAQGrid.SelectedItems)
        {
            if (item != null)
            {
                DataRow row = GetAutoQueueItem(item["AutoQueueID"].Text.ToInt());
                SetSelectItem(row, true);
                break;
            }
        }

        ActiveAQGrid.SelectedIndexes.Clear();
        ucDetail.BindDataDetail(RiskSessionManager.AutoQueue.ID);
    }

    protected void btnDoReloadGrid_OnClick(object sender, EventArgs e)
    {
        ReLoadActiveGrid();
    }

    protected void btnDoReloadData_OnClick(object sender, EventArgs e)
    {
        ReLoadAllData();
    }
    #endregion

    [WebMethod(EnableSession = true)]
    public static string UpdatelastRunStatus()
    {
        string lastRun = "";
        if (RiskSessionManager.AutoQueue == null) return string.Empty;
        long autoQueueId = RiskSessionManager.AutoQueue.ID;
        if (autoQueueId == 0) return lastRun;

        string spaName = "spa_RM_MCF_AQ_Get_RequestList";
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paras.Add(new FilterParameter("@AutoQueueID", autoQueueId, DbType.Int32));
        DataTable data = WebServices.RiskServices.GetReports(spaName, paras);
        if (data.HasData())
        {
            lastRun = data.Rows[0]["LastRun"].ToString();
            string lastEndDTS = data.Rows[0]["LastEndDTS"].ToString();

            string classStatus = SetCurQueueStatus(lastRun, lastEndDTS);
            if (classStatus.Equals(RiskSessionManager.AutoQueueStatus) && !classStatus.Equals("stop")) return string.Empty;
            RiskSessionManager.AutoQueueStatus = classStatus;
            if (!string.IsNullOrEmpty(lastEndDTS))
            {
                lastRun += lastEndDTS;
            }
            if (string.IsNullOrEmpty(lastRun)) lastRun = "--";
            lastRun += ";" + classStatus;
        }

        return lastRun;
    }
}