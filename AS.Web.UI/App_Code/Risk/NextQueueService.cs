using AS.Common.DBManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using AS.WS.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using Telerik.Web.UI.Gantt;

/// <summary>
/// Summary description for NextQueueService
/// </summary>
public class NextQueueService
{
    public static void DoClearWebLoading()
    {
        DoClearWebLoading(false);
    }

    public static void DoClearWebLoading(bool isMCFRisk)
    {
        string spa = isMCFRisk ? "spa_RM_MCF_ClearWebLoading" : "spa_rq_cs_ClearWebLoading";
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection outparameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddUserSessionID();
        WebServices.RiskServices.ExecuteNonQueryCommand(spa, parameters, out outparameters);

        if (isMCFRisk)
            RiskSessionManager.MCF_RiskNextQueue_Cache = new NextQueueItemCollection(); // Fix Cache not set null value.
        else
            RiskSessionManager.RiskNextQueue = null;
    }

    public static int CheckNotWorkedMerchantCount()
    {
        return CheckNotWorkedMerchantCount(false, 0, DateTime.Now);
    }

    public static int CheckNotWorkedMerchantCount(bool isMCFRisk, int assignmentID, DateTime reportDate)
    {
        assignmentID = isMCFRisk ? assignmentID : RiskSessionManager.DetectionQueue.AssignmentID;
        reportDate = isMCFRisk ? reportDate : RiskSessionManager.DetectionQueue.ReportDate;
        string spa = isMCFRisk ? "spa_rq_MCF_CheckNotWorkedMerchantCount" : "spa_rq_cs_CheckNotWorkedMerchantCount";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
        parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
        DataTable td = WebServices.RiskServices.GetReports(spa, parameters);
        if (td.HasData())
        {
            return int.Parse(td.Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    public static string DoGetNextQueueDetail()
    {
        return DoGetNextQueueDetail(false, false, false);
    }

    public static string DoGetNextQueueDetail(bool checkQueueEmpty = false)
    {
        return DoGetNextQueueDetail(checkQueueEmpty, false, false);
    }

    public static string DoGetNextQueueDetail(bool checkQueueEmpty = false, bool isFirstLoad = false)
    {
        return DoGetNextQueueDetail(checkQueueEmpty, isFirstLoad, false);
    }

    public static string DoGetNextQueueDetail(bool checkQueueEmpty = false, bool isFirstLoad = false, bool isMCFRisk = false,
        int assignmentID = 0, string reportDate = "", string orderBy = "", string applyFilterId = "", string merchantNumber = "")
    {
        if (isMCFRisk) return DoGetNextQueueDetailV2(checkQueueEmpty, isFirstLoad, assignmentID, reportDate, orderBy, applyFilterId, merchantNumber);

        assignmentID = RiskSessionManager.DetectionQueue.AssignmentID;
        reportDate = RiskSessionManager.DetectionQueue.ReportDate.ToString();
        orderBy =  RiskSessionManager.DetectionQueue.OrderBy;
        merchantNumber = RiskSessionManager.currentMerchantNumber;

        string spa = "spa_rq_cs_GetNextQueueDetails";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddUserSessionID();
        parameters.Add(new FilterParameter("@MerchantNumber", isFirstLoad ? "" : merchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
        parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@CheckQueueEmpty", checkQueueEmpty, DbType.Boolean));
        parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
        parameters.AddDecryptDataParams("AccountNumber", false);
        parameters.Add(new FilterParameter("@PageNo", 1, DbType.Int32));
        parameters.Add(new FilterParameter("@PageSize", WebSiteSettings.DefaultRiskPageSize, DbType.Int32));
        parameters.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
        parameters.Add(new FilterParameter("@stOrder", orderBy, DbType.AnsiString));
        
        if (!checkQueueEmpty)
        {
            try
            {
                DataSet ds = WebServices.RiskServices.GetReportsAsDataSet(spa, parameters);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    NextQueueItemCollection lst = RiskSessionManager.RiskNextQueue;

                    lst.Add(new NextQueueItem()
                    {
                        MerchantNumber = ds.Tables[0].Rows[0]["MerchantNumber"].ToString(),                       
                        Data = GetDataTableFromDataSet(ds)
                    });
                   
                    RiskSessionManager.RiskNextQueue = lst;

                    return string.Empty;
                }
                else
                {                    
                    return "end";
                }
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error(string.Format("Next Queue Error: MerchantNumber={0}; AssignmentID={1}; ReportDate={2}; UserId={3}; SessionId={4}; NextQueueSessionId={5} "
                    , RiskSessionManager.currentMerchantNumber, RiskSessionManager.DetectionQueue.AssignmentID, RiskSessionManager.DetectionQueue.ReportDate
                    , SessionManager.CurrentUser.UserID, HttpContext.Current.Session.SessionID, SessionManager.CurrentUser.UserID + HttpContext.Current.Session.SessionID), ex);
                return "error";
            }
        }
        else
        {
            DataTable tb = WebServices.RiskServices.GetReports(spa, parameters);
            if (tb.HasData())
            {
                return tb.Rows[0]["InQueueMerchantCount"].ToString();
            }
            else
            {
                return "0";
            }
        }
    }

    public static FilterParameterCollection AddParameterNextQueueReport(int siteId, int assignmentID = 0, string reportDate = "", string merchantNumber = "")
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(siteId);
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
        parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));

        return parameters;
    }

    public static FilterParameterCollection AddParameterNextQueueReportDetail(int currentLanguage, int siteId
        , string reportDate = "", string orderBy = "", string merchantNumber = "")
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(siteId);
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@LanguageID", currentLanguage, DbType.Int32));
        parameters.AddDecryptDataParams("AccountNumber", false);
        parameters.Add(new FilterParameter("@PageNo", 1, DbType.Int32));
        parameters.Add(new FilterParameter("@PageSize", WebSiteSettings.DefaultRiskPageSize, DbType.Int32));
        parameters.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
        parameters.Add(new FilterParameter("@stOrder", orderBy, DbType.AnsiString));

        return parameters;
    }

    private static string GetDataTableForNextQueueReport(int currentLanguage, int siteId,
        int assignmentID = 0, string reportDate = "", string orderBy = "", string merchantNumber = "")
    {      
        NextQueueItemCollection lst = RiskSessionManager.MCF_RiskNextQueue_Cache;
        List<string> spaNames = new List<string>();
        List<FilterParameterCollection> parameters = new List<FilterParameterCollection>();
        List<string> tableNames = new List<string>();

        spaNames.Add("spa_RM_MCF_Get_NextQueueReport_MerchantInfo");
        parameters.Add(AddParameterNextQueueReport(siteId, assignmentID, reportDate, merchantNumber));
        tableNames.Add("merchantInfo");

        spaNames.Add("spa_RM_MCF_Get_NextQueueReport_Barometer");
        parameters.Add(AddParameterNextQueueReport(siteId, assignmentID, reportDate, merchantNumber));
        tableNames.Add("barometer");

        spaNames.Add("spa_RM_MCF_GetNextQueueReportTransactionDetail");
        parameters.Add(AddParameterNextQueueReportDetail(currentLanguage, siteId, reportDate, orderBy, merchantNumber));
        tableNames.Add("transactions");

        spaNames.Add("spa_RM_MCF_GetNextQueueReportChargebackDetail");
        parameters.Add(AddParameterNextQueueReportDetail(currentLanguage, siteId, reportDate, orderBy, merchantNumber));
        tableNames.Add("chargeback");

        var results = WebServices.RiskServices.GetReportsAsDataSet(spaNames.ToArray(), parameters.ToArray(), tableNames.ToArray());

        lst.Add(new NextQueueItem()
        {
            MerchantNumber = merchantNumber,
            Data = GetDataTableFromDataSetForNextQueueReport(results, tableNames.ToArray())
        });

        RiskSessionManager.MCF_RiskNextQueue_Cache = lst;

        return string.Empty;
    }

    public static string DoGetNextQueueDetailV2(bool checkQueueEmpty = false, bool isFirstLoad = false,
        int assignmentID = 0, string reportDate = "", string orderBy = "", string applyFilterId = "", string merchantNumber = "")
    {
        merchantNumber = RiskSessionManager.MCF_CurrentNextQueue.MerchantNumber;
       
        string spa = "spa_RM_MCF_Get_NextQueueReport";
        var user = SessionManager.CurrentUser;
        var siteId = user.SiteID;
        var currentLanguage = SessionManager.CurrentLanguage;

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(siteId);
        parameters.AddUserSessionID();
        parameters.Add(new FilterParameter("@MerchantNumber", isFirstLoad ? "" : merchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
        parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@CheckQueueEmpty", checkQueueEmpty, DbType.Boolean));
        parameters.Add(new FilterParameter("@LanguageID", currentLanguage, DbType.Int32));
        parameters.AddDecryptDataParams("AccountNumber", false);
        parameters.Add(new FilterParameter("@PageNo", 1, DbType.Int32));
        parameters.Add(new FilterParameter("@PageSize", WebSiteSettings.DefaultRiskPageSize, DbType.Int32));
        parameters.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
        parameters.Add(new FilterParameter("@stOrder", orderBy, DbType.AnsiString));        
        parameters.Add(new FilterParameter("@ApplyFilterId", applyFilterId, DbType.String));
        
        if (!checkQueueEmpty)
        {
            try
            {
                DataSet ds = WebServices.RiskServices.GetReportsAsDataSet(spa, parameters);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var mid = ds.Tables[0].Rows[0]["MerchantNumber"].ToString();

                    return GetDataTableForNextQueueReport(currentLanguage, siteId, assignmentID, reportDate, "TransactionAmount DESC", mid);
                }
                else
                {                   
                    return "end";
                }
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error(string.Format("Next Queue Error: MerchantNumber={0}; AssignmentID={1}; ReportDate={2}; UserId={3}; SessionId={4}; NextQueueSessionId={5} "
                    , RiskSessionManager.currentMerchantNumber, RiskSessionManager.DetectionQueue.AssignmentID, RiskSessionManager.DetectionQueue.ReportDate
                    , SessionManager.CurrentUser.UserID, HttpContext.Current.Session.SessionID, SessionManager.CurrentUser.UserID + HttpContext.Current.Session.SessionID), ex);
                return "error";
            }
        }
        else
        {
            DataTable tb = WebServices.RiskServices.GetReports(spa, parameters);
            if (tb.HasData())
            {
                return tb.Rows[0]["InQueueMerchantCount"].ToString();
            }
            else
            {
                return "0";
            }
        }
    }

    public static void UpdateMerchantWorked(string merchantNumber, bool status = true)
    {
        FilterParameterCollection parameterList = new FilterParameterCollection();
        FilterParameterCollection outParameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.Add(new FilterParameter("@ReportDate", RiskSessionManager.DetectionQueue.ReportDate, DbType.DateTime));
        parameterList.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.AnsiString));
        parameterList.Add(new FilterParameter("@AssignmentID", RiskSessionManager.DetectionQueue.AssignmentID, DbType.Int32));
        parameterList.Add(new FilterParameter("@ReturnValue", 0, DbType.Int32, true));
        parameterList.Add(new FilterParameter("@Status", status, DbType.Boolean));
        parameterList.Add(new FilterParameter("@WorkSource", WebSiteEnums.PAGE_CODE.NQ.ToString(), DbType.String));
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rq_cs_UpdateMerchantWorked", parameterList, out outParameterList);
    }

    public static NextQueueItem GetCurrentMerchantWorked()
    {
        return GetCurrentMerchantWorked(false, string.Empty, 0, DateTime.Now);
    }

    public static NextQueueItem GetCurrentMerchantWorked(bool isMCFRisk, string merchantNumber, int assignmentID, DateTime reportDate)
    {
        merchantNumber = isMCFRisk ? merchantNumber : RiskSessionManager.currentMerchantNumber;
        assignmentID = isMCFRisk ? assignmentID : RiskSessionManager.MerchantWorkedDetectionQueue.AssignmentID;
        reportDate = isMCFRisk ? reportDate : RiskSessionManager.MerchantWorkedDetectionQueue.ReportDate;

        string spa = isMCFRisk ? "spa_RM_MCF_Get_NextQueueReport_Merchant" : "spa_rq_cs_GetNextQueueDetailsByMerchant";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
        parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
        parameters.AddDecryptDataParams("AccountNumber", false);
        parameters.Add(new FilterParameter("@PageNo", 1, DbType.Int32));
        parameters.Add(new FilterParameter("@PageSize", WebSiteSettings.DefaultRiskPageSize, DbType.Int32));
        parameters.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
        DataSet ds = WebServices.RiskServices.GetReportsAsDataSet(spa, parameters);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            return new NextQueueItem()
            {
                MerchantNumber = ds.Tables[0].Rows[0]["MerchantNumber"].ToString(),
                //42598 + Enhance code
                Data = GetDataTableFromDataSet(ds)
            };
        }
        else
            return null;
    }

    //42598 + Enhance code - Build table array from dataset
    public static DataTable[] GetDataTableFromDataSet(DataSet ds)
    {
        DataTable[] arrTables = new DataTable[ds.Tables.Count];
        for (int i = 0; i < ds.Tables.Count; i++)
        {
            arrTables[i] = ds.Tables[i];
        }
        return arrTables;
    }

    public static DataTable[] GetDataTableFromDataSetForNextQueueReport(DataSet ds, string[] tableName)
    {
        DataTable[] arrTables = new DataTable[ds.Tables.Count];
        for (int i = 0; i < ds.Tables.Count; i++)
        {
            arrTables[i] = ds.Tables[tableName[i]];
        }
        return arrTables;
    }

    public static void KeepSessionActive()
    {
        KeepSessionActive(false);
    }

    public static void KeepSessionActive(bool isMCFRisk)
    {
        string spa = isMCFRisk ? "spa_rq_MCF_KeepSessionActive" : "spa_rq_cs_KeepSessionActive";
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection outParameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddUserSessionID();
        WebServices.RiskServices.ExecuteNonQueryCommand(spa, parameters, out outParameters);
    }
}