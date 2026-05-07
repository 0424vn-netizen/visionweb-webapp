using System;
using System.Globalization;
using AS.Common.DBManager;
using System.Data;
using System.Text.RegularExpressions;

/// <summary>
/// Rm_AutoQueue Business
/// </summary>
public class Rm_AutoQueueBusiness
{
    private const string FmDateTime = "MM/dd/yyyy hh:mm tt";

    public static DataTable GetAutoQueueList(bool? isActive)
    {
        return GetAutoQueueList(isActive, null);
    }

    public static DataTable GetAutoQueueList(bool? isActive, long? aqId)
    {
        return GetAutoQueueList(isActive, aqId, false);
    }

    public static DataTable GetAutoQueueList(bool? isActive, long? aqId, bool isMCFRisk)
    {
        string spaName = !isMCFRisk ? "spa_RM_AutoQueue_Get_RequestList" : "spa_RM_MCF_AQ_Get_RequestList";

        FilterParameterCollection paras = new FilterParameterCollection();
        paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        if (isActive.HasValue)
        {
            paras.Add(new FilterParameter("@IsActived", isActive, DbType.Boolean));
        }

        if (aqId.HasValue)
        {
            paras.Add(new FilterParameter("@AutoQueueID", aqId, DbType.Int64));
        }

        return WebServices.RiskServices.GetReports(spaName, paras);
    }

    public static bool UpdateOrderAutoQueue(DataRow queueData, long newIndex)
    {
        return UpdateOrderAutoQueue(queueData, newIndex, false);
    }

    public static bool UpdateOrderAutoQueue(DataRow queueData, long newIndex, bool isMCFRisk)
    {
        string spaName = !isMCFRisk ? "spa_RM_AutoQueue_Update_Request_ProcessingOrder" : "spa_RM_MCF_AQ_Update_Request_ProcessingOrder";
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paras.Add(new FilterParameter("@AutoQueueID", queueData["AutoQueueID"].ToInt(), DbType.Int64));
        paras.Add(new FilterParameter("@OldProcessingOrder", queueData["ProcessingOrder"].ToInt(), DbType.Int64));
        paras.Add(new FilterParameter("@NewProcessingOrder", newIndex, DbType.Int64));
        paras.Add(new FilterParameter("@IsActived", queueData["IsActived"].ToBoolean(), DbType.Boolean));

        DataTable dataResult = WebServices.CsReportServices.GetReports(spaName, paras);

        if (dataResult.Rows[0]["IsAvailable"].ToInt() > 0)
        {
            return true;
        }

        return false;
    }

    public static DataTable GetDataAssignments(int type, long queueId)
    {
        return GetDataAssignments(type, queueId, false);
    }

    public static DataTable GetDataAssignments(int type, long queueId, bool isMCFRisk)
    {
        FilterParameterCollection paras = new FilterParameterCollection();
        string spaName = !isMCFRisk ? "spa_RM_AutoQueue_Get_AssignmentList" : "spa_RM_MCF_AQ_Get_AssignmentList";

        paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paras.Add(new FilterParameter("@AutoQueueID", queueId, DbType.Int32));
        paras.Add(new FilterParameter("@AssignmentTypeID", type, DbType.Int32));

        return WebServices.CsReportServices.GetReports(spaName, paras);
    }

    public static DataTable GetDataChangeLog(long queueId)
    {
        return GetDataChangeLog(queueId, true);
    }

    public static DataTable GetDataChangeLog(long queueId, bool isAllData)
    {
        return GetDataChangeLog(queueId, isAllData, false);
    }

    public static DataTable GetDataChangeLog(long queueId, bool isAllData, bool isMCFRisk)
    {
        FilterParameterCollection paras = new FilterParameterCollection();
        string spaName = !isMCFRisk ? "spa_RM_AutoQueue_Get_ChangeLogList" : "spa_RM_MCF_AQ_Get_ChangeLogList";
        paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID).AddLanguageID();
        paras.Add(new FilterParameter("@AutoQueueID", queueId, DbType.Int32));
        if (!isAllData)
        {
            paras.Add(new FilterParameter("@IsPaging", 1, DbType.Boolean));
            paras.Add(new FilterParameter("@PageSize", 5, DbType.Int32));
        }

        DataTable dataList = WebServices.CsReportServices.GetReports(spaName, paras);

        DataTable newdata = new DataTable();
        newdata.Columns.Add(new DataColumn("RowNumber", typeof(Int64)));
        newdata.Columns.Add(new DataColumn("TotalRows", typeof(Int64)));
        newdata.Columns.Add(new DataColumn("CreatedDTS", typeof(string)));
        newdata.Columns.Add(new DataColumn("ActionTypeDesc", typeof(string)));

        foreach (DataRow row in dataList.Rows)
        {
            string textActived = row["ActionTypeDesc"].ToString();
            string textValue = string.Empty;
            string columnName = string.Empty;
            string replaceName = string.Empty;
            var tempDatas = Regex.Matches(textActived, @"{\w*}");
            if (tempDatas.Count > 0)
            {
                foreach (var item in tempDatas)
                {
                    columnName = item.ToString().Replace("{", "").Replace("}", "");
                    replaceName = item.ToString();
                    if (!string.IsNullOrEmpty(columnName))
                    {
                        textValue = row["" + columnName + ""].ToString();
                        if (Regex.Matches(columnName, @"Date").Count > 0)
                        {
                            DateTime dt = DateTime.MinValue;
                            if (DateTime.TryParse(textValue, out dt)) textValue = dt.ToShortDateString();
                        }
                        textValue = string.Format("<span class=\"text-color-default\">{0}</span>", textValue);
                    }

                    textActived = textActived.Replace(replaceName, textValue);
                }
            }

            DataRow rowData = newdata.NewRow();
            if (!isAllData)
            {
                rowData["RowNumber"] = row["RowNumber"];
                rowData["TotalRows"] = row["TotalRows"];
            }

            rowData["CreatedDTS"] = ConvertDateTimeAQ(row["CreatedDTS"].ToString(), FmDateTime);
            rowData["ActionTypeDesc"] = textActived;
            newdata.Rows.Add(rowData);
        }

        return newdata;
    }

    public static string ConvertDateTimeAQ(string dateTime, string formatString = null)
    {
        DateTime dt = DateTime.MinValue;
        if (DateTime.TryParse(dateTime, out dt))
        {
            if (string.IsNullOrEmpty(formatString)) formatString = FmDateTime;
            return dt.ToString(formatString);
        }

        return dateTime;
    }
}