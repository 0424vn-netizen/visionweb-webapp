using Aperia.Core.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Windows.Input;

/// <summary>
/// Summary description for RiskSessionManager
/// </summary>
public static class RiskSessionManager
{
    private static string RISK_REPORT_REFERRER = "RiskReportReferrer";
    private static string RISK_REPORT_REFERRER_INFO = "RiskReportReferrerInfo";
    private static string RISK_REPORT_MERCHANT_INFO = "RiskReportMerchantInfo";

    private static string ESCALATION_QUEUE_REFERRER = "EscalationQueueReferrer";
    private static string ESCALATION_QUEUE_REFERRER_INFO = "EscalationQueueReferrerInfo";
    private static string IS_SCROLL = "IS_SCROLL";
    private static string PORTFOLIO_FILTERING_OPTIONS = "PortCreditsFilteringOptions";
    private static string RISK_MGMT_REPORT_FILTER = "RiskMgmtReportFilter";
    //private static string RISK_MCF_MERCHARTALERTHISTORY = "MCF_MerchantAlertHistory";
    //private static string MERCHANT_PROFILE_REFERRER = "RiskReportReferrer";
    private static string MERCHANT_PROFILE_REFERRER_INFO = "RiskReportReferrerInfo";
    private static string DETECTION_QUEUE_REQUEUE_INFO = "DetectionQueueRequeueInfo";

    private static string RISK_NEXT_QUEUE = "RiskNextQueue";
    private static string CURRENT_RISK_NEXT_QUEUE = "CurrentRiskNextQueue";
    private static string MCF_RISK_NEXT_QUEUE = "MCF_RiskNextQueue";
    private static string MCF_CURRENT_RISK_NEXT_QUEUE = "MCF_CurrentRiskNextQueue";
    //private static string RISK_NEXT_QUEUE_MERCHANT_INDEX = "RiskNextQueueMerchantIndex";
    //private static string RISK_NEXT_QUEUE_END = "RiskNextQueueEnd";
    //private static string RISK_NEXT_QUEUE_MERCHANT_COUNT = "RiskNextQueueMerchantCount";
    //private static string RISK_NEXT_QUEUE_MERCHANT_CURRENT_COUNT = "RiskNextQueueMerchantCurrentCount";
    private static string RISK_REPORT_VOLUME = "RiskReportVolume";
    private static string RISK_REPORT_TRANSACTION_HISTORY = "RiskReportTransactionHistory";
    private static string RISKREPORT_DETECTIONQUEUE_COLUMNS = "RISKREPORT_DETECTIONQUEUE_COLUMNS";
    private static string RISK_AQ_LIST = "RiskAutoQueueList";
    private static string RISK_AQ_ITEM = "RiskAutoQueueItem";
    private static string RISK_DQNEXTQREPORT_HIGHEST_TRANSACTION_AMOUNT_TOOLTIP = "DQNextQReportCS_Text_HighestTransactionAmount";
    private static string RISK_DQNEXTQREPORT_TEXT_DUPLICATE_ACCOUNT_NUMBER_TOOLTIP = "DQNextQReportCS_Text_DuplicateAccountNumber";
    private static string RISK_DQNEXTQREPORT_TEXT_TOTAL = "DQNextQReportCS_Text_Total";
    private static string RISK_MCF_DQRAINBOWREPORT = "RiskMCFDQRainbowReport";
    private static string RISK_MCF_DISPOSITIONLIST = "RiskMCFDispositionList";
    private static string RISK_MCF_DQ_CURRENTASSIGNMENTID = "RISK_MCF_DQ_CURRENTASSIGNMENTID";
    private static string RISK_REPORT_FORWARD_DELIVERY = "RiskReportForwardDelivery";

    public static NextQueueItemCollection RiskNextQueue
    {
        get
        {
            if (HttpContext.Current.Session[RISK_NEXT_QUEUE] != null)
                return (NextQueueItemCollection)HttpContext.Current.Session[RISK_NEXT_QUEUE];
            else
                return new NextQueueItemCollection();
        }
        set
        {
            HttpContext.Current.Session[RISK_NEXT_QUEUE] = value;
        }
    }
    public static NextQueueItem CurrentRiskNextQueue
    {
        get
        {
            if (HttpContext.Current.Session[CURRENT_RISK_NEXT_QUEUE] != null)
                return (NextQueueItem)HttpContext.Current.Session[CURRENT_RISK_NEXT_QUEUE];
            else
                return new NextQueueItem();
        }
        set
        {
            HttpContext.Current.Session[CURRENT_RISK_NEXT_QUEUE] = value;
        }
    }

    public static NextQueueItemCollection MCF_RiskNextQueue_Cache
    {
        get
        {
            var item = CacheManager.GetCache<NextQueueItemCollection>(MCF_RISK_NEXT_QUEUE + HttpContext.Current.Session.SessionID);
            if (item == null)
                return new NextQueueItemCollection();
            else
                return item;
        }
        set
        {
            CacheManager.SetCache<NextQueueItemCollection>(MCF_RISK_NEXT_QUEUE + HttpContext.Current.Session.SessionID, value);
        }
    }

    public static NextQueueItem MCF_CurrentNextQueue
    {
        get
        {
            if (HttpContext.Current.Session[MCF_CURRENT_RISK_NEXT_QUEUE] != null)
                return (NextQueueItem)HttpContext.Current.Session[MCF_CURRENT_RISK_NEXT_QUEUE];
            else
                return new NextQueueItem();
        }
        set
        {
            HttpContext.Current.Session[MCF_CURRENT_RISK_NEXT_QUEUE] = value;
        }
    }

    public static string PortfolioReferer
    {
        get
        {
            if (HttpContext.Current.Session[PORTFOLIO_FILTERING_OPTIONS] != null)
            {
                return (string)HttpContext.Current.Session[PORTFOLIO_FILTERING_OPTIONS];
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            HttpContext.Current.Session[PORTFOLIO_FILTERING_OPTIONS] = value;
        }
    }

    public static string RiskReportReferrer
    {
        get
        {
            if (HttpContext.Current.Session[RISK_REPORT_REFERRER] != null)
            {
                return (string)HttpContext.Current.Session[RISK_REPORT_REFERRER];
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                HttpContext.Current.Session.Remove(RISK_REPORT_REFERRER);
            }
            else
            {
                HttpContext.Current.Session[RISK_REPORT_REFERRER] = value;
            }
        }
    }
    public static ReferrerInfo RiskReportReferrerInfo
    {
        get
        {
            if (HttpContext.Current.Session[RISK_REPORT_REFERRER_INFO] != null)
            {
                return HttpContext.Current.Session[RISK_REPORT_REFERRER_INFO] as ReferrerInfo;
            }
            else
            {
                return new ReferrerInfo();
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(RISK_REPORT_REFERRER_INFO);
            }
            else
            {
                HttpContext.Current.Session[RISK_REPORT_REFERRER_INFO] = value;
            }
        }
    }
    public static string MerchantProfileReferrer
    {
        get
        {
            if (HttpContext.Current.Session[RISK_REPORT_REFERRER] != null)
            {
                return (string)HttpContext.Current.Session[RISK_REPORT_REFERRER];
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                HttpContext.Current.Session.Remove(RISK_REPORT_REFERRER);
            }
            else
            {
                HttpContext.Current.Session[RISK_REPORT_REFERRER] = value;
            }
        }
    }
    public static ReferrerInfo MerchantProfileReferrerInfo
    {
        get
        {
            if (HttpContext.Current.Session[MERCHANT_PROFILE_REFERRER_INFO] != null)
            {
                return HttpContext.Current.Session[MERCHANT_PROFILE_REFERRER_INFO] as ReferrerInfo;
            }
            else
            {
                return new ReferrerInfo();
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(MERCHANT_PROFILE_REFERRER_INFO);
            }
            else
            {
                HttpContext.Current.Session[MERCHANT_PROFILE_REFERRER_INFO] = value;
            }
        }
    }

    public static string EscalationQueueReferrer
    {
        get
        {
            if (HttpContext.Current.Session[ESCALATION_QUEUE_REFERRER] != null)
            {
                return GeneralFuncsLib.NvlString(HttpContext.Current.Session[ESCALATION_QUEUE_REFERRER]);
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                HttpContext.Current.Session.Remove(ESCALATION_QUEUE_REFERRER);
            }
            else
            {
                HttpContext.Current.Session[ESCALATION_QUEUE_REFERRER] = value;
            }
        }
    }
    public static RiskQueue RiskQueue
    {
        get
        {
            return HttpContext.Current.Session["RiskQueue"] as RiskQueue;
        }
        set
        {
            HttpContext.Current.Session["RiskQueue"] = value;
        }
    }

    //Risk Report use to scroll if user click Investigation Button
    public static int IsSCroll
    {
        get
        {
            if (HttpContext.Current.Session[IS_SCROLL] != null)
            {
                return (int)HttpContext.Current.Session[IS_SCROLL];
            }
            else
            {
                return 0;
            }
        }
        set
        {
            HttpContext.Current.Session[IS_SCROLL] = value;
        }
    }

    public static ReferrerInfo EscalationQueueReferrerInfo
    {
        get
        {
            if (HttpContext.Current.Session[ESCALATION_QUEUE_REFERRER_INFO] != null)
            {
                return HttpContext.Current.Session[ESCALATION_QUEUE_REFERRER_INFO] as ReferrerInfo;
            }
            else
            {
                return new ReferrerInfo();
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(ESCALATION_QUEUE_REFERRER_INFO);
            }
            else
            {
                HttpContext.Current.Session[ESCALATION_QUEUE_REFERRER_INFO] = value;
            }
        }
    }

    public static DetectionQueue DetectionQueue
    {
        get
        {
            if (HttpContext.Current.Session["DetectionQueue"] != null)
                return (DetectionQueue)HttpContext.Current.Session["DetectionQueue"];
            return null;
        }
        set
        {
            HttpContext.Current.Session["DetectionQueue"] = value;
        }
    }

    public static DetectionQueue MerchantWorkedDetectionQueue
    {
        get
        {
            if (HttpContext.Current.Session["MerchantWorkedDetectionQueue"] != null)
                return (DetectionQueue)HttpContext.Current.Session["MerchantWorkedDetectionQueue"];
            return null;
        }
        set
        {
            HttpContext.Current.Session["MerchantWorkedDetectionQueue"] = value;
        }
    }

    public static bool IsUsingMarketData
    {
        get
        {
            if (HttpContext.Current.Session["ASIsUsingMarketData"] != null)
                return bool.Parse(HttpContext.Current.Session["ASIsUsingMarketData"].ToString());
            else
                return false;
        }
        set
        {
            HttpContext.Current.Session["ASIsUsingMarketData"] = value;
        }
    }

    public static bool IsAutoCheckWork
    {
        get
        {
            if (HttpContext.Current.Session["IsAutoCheckWork"] != null)
            {
                return bool.Parse(HttpContext.Current.Session["IsAutoCheckWork"].ToString());
            }
            else
            {
                return false;
            }
        }
        set
        {
            HttpContext.Current.Session["IsAutoCheckWork"] = value;
        }
    }

    public static MgmtReportFilter RiskMgmtReportFilter
    {
        get
        {
            if (HttpContext.Current.Session[RISK_MGMT_REPORT_FILTER] == null)
            {
                MgmtReportFilter reportFileValue = new MgmtReportFilter();
                HttpContext.Current.Session[RISK_MGMT_REPORT_FILTER] = reportFileValue;
            }
            return (MgmtReportFilter)HttpContext.Current.Session[RISK_MGMT_REPORT_FILTER];
        }
        set
        {
            HttpContext.Current.Session[RISK_MGMT_REPORT_FILTER] = value;
        }
    }

    public static List<string> ListMerchantNumber
    {
        get
        {
            if (HttpContext.Current.Session["ListMerchantNumber"] != null)
                return (List<string>)HttpContext.Current.Session["ListMerchantNumber"];
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["ListMerchantNumber"] = value;
        }
    }
    public static string currentMerchantNumber
    {
        get
        {
            if (HttpContext.Current.Session["currentMerchantNumber"] != null)
                return (string)HttpContext.Current.Session["currentMerchantNumber"];
            return null;
        }
        set
        {
            HttpContext.Current.Session["currentMerchantNumber"] = value;
        }
    }
    public static RetrievalsChargebacks RetrievalsChargebacksReferer
    {
        get
        {
            if (HttpContext.Current.Session["RetrievalsChargebacksReferer"] != null)
            {
                return (RetrievalsChargebacks)HttpContext.Current.Session["RetrievalsChargebacksReferer"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["RetrievalsChargebacksReferer"] = value;
        }
    }
    public static string TransactionHistorySortOrder
    {
        get
        {
            if (HttpContext.Current.Session["TransactionHistorySortOrder"] != null)
            {
                return HttpContext.Current.Session["TransactionHistorySortOrder"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            HttpContext.Current.Session["TransactionHistorySortOrder"] = value;
        }

    }
    public static int TransDateRangeModal
    {
        get
        {
            if (HttpContext.Current.Session["TransDateRangeModal"] != null)
            {
                return (int)HttpContext.Current.Session["TransDateRangeModal"];
            }
            else
            {
                return 3;
            }
        }
        set
        {
            HttpContext.Current.Session["TransDateRangeModal"] = value;
        }
    }

    public static int BatchDateRangeModal
    {
        get
        {
            if (HttpContext.Current.Session["BatchDateRangeModal"] != null)
            {
                return (int)HttpContext.Current.Session["BatchDateRangeModal"];
            }
            else
            {
                return 3;
            }
        }
        set
        {
            HttpContext.Current.Session["BatchDateRangeModal"] = value;
        }
    }

    public static int ACHReturnDateRangeModal
    {
        get
        {
            if (HttpContext.Current.Session["ACHReturnDateRangeModal"] != null)
            {
                return (int)HttpContext.Current.Session["ACHReturnDateRangeModal"];
            }
            else
            {
                return 30;
            }
        }
        set
        {
            HttpContext.Current.Session["ACHReturnDateRangeModal"] = value;
        }
    }

    public static DetectionQueue_RequeuedAssignment DetectionQueueTemporaryRequeueInfo
    {
        get
        {
            if (HttpContext.Current.Session[DETECTION_QUEUE_REQUEUE_INFO] == null)
            {
                DetectionQueue_RequeuedAssignment requeuedSession = new DetectionQueue_RequeuedAssignment();
                HttpContext.Current.Session[DETECTION_QUEUE_REQUEUE_INFO] = requeuedSession;
            }
            return (DetectionQueue_RequeuedAssignment)HttpContext.Current.Session[DETECTION_QUEUE_REQUEUE_INFO];
        }
        set
        {
            HttpContext.Current.Session[DETECTION_QUEUE_REQUEUE_INFO] = value;
        }
    }

    public static IEnumerable<ColumnDisplayedConfigurationItem> RiskReportTransactionVolumeColumn
    {
        get
        {
            if (HttpContext.Current.Session[RISK_REPORT_VOLUME] != null)
            {
                return (IEnumerable<ColumnDisplayedConfigurationItem>)HttpContext.Current.Session[RISK_REPORT_VOLUME];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(RISK_REPORT_VOLUME);
            }
            else
            {
                HttpContext.Current.Session[RISK_REPORT_VOLUME] = value;
            }
        }
    }

    public static IEnumerable<ColumnDisplayedConfigurationItem> RiskReportTransactionHistoryColumn
    {
        get
        {
            if (HttpContext.Current.Session[RISK_REPORT_TRANSACTION_HISTORY] != null)
            {
                return (IEnumerable<ColumnDisplayedConfigurationItem>)HttpContext.Current.Session[RISK_REPORT_TRANSACTION_HISTORY];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(RISK_REPORT_TRANSACTION_HISTORY);
            }
            else
            {
                HttpContext.Current.Session[RISK_REPORT_TRANSACTION_HISTORY] = value;
            }
        }
    }

    public static IEnumerable<ColumnDisplayedConfigurationItem> RiskReportDetectionQueueColumns
    {
        get
        {
            if (HttpContext.Current.Session[RISKREPORT_DETECTIONQUEUE_COLUMNS] != null)
            {
                return (IEnumerable<ColumnDisplayedConfigurationItem>)HttpContext.Current.Session[RISKREPORT_DETECTIONQUEUE_COLUMNS];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(RISKREPORT_DETECTIONQUEUE_COLUMNS);
            }
            else
            {
                HttpContext.Current.Session[RISKREPORT_DETECTIONQUEUE_COLUMNS] = value;
            }
        }
    }

    public static DataTable AutoQueueList
    {
        get
        {
            if (HttpContext.Current.Session[RISK_AQ_LIST] != null)
            {
                return (DataTable)HttpContext.Current.Session[RISK_AQ_LIST];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(RISK_AQ_LIST);
            }
            else
            {
                HttpContext.Current.Session[RISK_AQ_LIST] = value;
            }
        }
    }

    public static RiskAutoQueueModel AutoQueue
    {
        get
        {
            if (HttpContext.Current.Session[RISK_AQ_ITEM] != null)
                return (RiskAutoQueueModel)HttpContext.Current.Session[RISK_AQ_ITEM];
            return null;
        }
        set
        {
            HttpContext.Current.Session[RISK_AQ_ITEM] = value;
        }
    }

    public static string AutoQueueStatus
    {
        get
        {
            if (HttpContext.Current.Session["AutoQueueitemStatus"] != null)
            {
                return HttpContext.Current.Session["AutoQueueitemStatus"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            HttpContext.Current.Session["AutoQueueitemStatus"] = value;
        }

    }

    public static string UserViewSelected
    {
        get
        {
            if (HttpContext.Current.Session["UserViewSelected"] != null)
            {
                return HttpContext.Current.Session["UserViewSelected"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            HttpContext.Current.Session["UserViewSelected"] = value;
        }

    }

    public static string TransactionHistoryUserViewSelected
    {
        get
        {
            if (HttpContext.Current.Session["TransactionHistoryUserViewSelected"] != null)
            {
                return HttpContext.Current.Session["TransactionHistoryUserViewSelected"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            HttpContext.Current.Session["TransactionHistoryUserViewSelected"] = value;
        }

    }

    public static string TransactionHistoryNewWindowUserViewSelected
    {
        get
        {
            if (HttpContext.Current.Session["TransactionHistoryNewWindowUserViewSelected"] != null)
            {
                return HttpContext.Current.Session["TransactionHistoryNewWindowUserViewSelected"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            HttpContext.Current.Session["TransactionHistoryNewWindowUserViewSelected"] = value;
        }

    }
    public static DataTable RiskReportMerchantInfo
    {
        get
        {
            if (HttpContext.Current.Session[RISK_REPORT_MERCHANT_INFO] != null)
            {
                return (DataTable)HttpContext.Current.Session[RISK_REPORT_MERCHANT_INFO];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(RISK_REPORT_MERCHANT_INFO);
            }
            else
            {
                HttpContext.Current.Session[RISK_REPORT_MERCHANT_INFO] = value;
            }
        }
    }

    public static DataTable RiskReportForwardDelivery
    {
        get
        {
            if (HttpContext.Current.Session[RISK_REPORT_FORWARD_DELIVERY] != null)
            {
                return (DataTable)HttpContext.Current.Session[RISK_REPORT_FORWARD_DELIVERY];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(RISK_REPORT_FORWARD_DELIVERY);
            }
            else
            {
                HttpContext.Current.Session[RISK_REPORT_FORWARD_DELIVERY] = value;
            }
        }
    }

    public static string DQNextQReportCS_Text_HighestTransactionAmount_Tooltip
    {
        get
        {
            if (HttpContext.Current.Session[RISK_DQNEXTQREPORT_HIGHEST_TRANSACTION_AMOUNT_TOOLTIP] != null)
            {
                return (string)HttpContext.Current.Session[RISK_DQNEXTQREPORT_HIGHEST_TRANSACTION_AMOUNT_TOOLTIP];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(RISK_DQNEXTQREPORT_HIGHEST_TRANSACTION_AMOUNT_TOOLTIP);
            }
            else
            {
                HttpContext.Current.Session[RISK_DQNEXTQREPORT_HIGHEST_TRANSACTION_AMOUNT_TOOLTIP] = value;
            }
        }
    }

    public static string DQNextQReportCS_Text_DuplicateAccountNumber_Tooltip
    {
        get
        {
            if (HttpContext.Current.Session[RISK_DQNEXTQREPORT_TEXT_DUPLICATE_ACCOUNT_NUMBER_TOOLTIP] != null)
            {
                return (string)HttpContext.Current.Session[RISK_DQNEXTQREPORT_TEXT_DUPLICATE_ACCOUNT_NUMBER_TOOLTIP];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(RISK_DQNEXTQREPORT_TEXT_DUPLICATE_ACCOUNT_NUMBER_TOOLTIP);
            }
            else
            {
                HttpContext.Current.Session[RISK_DQNEXTQREPORT_TEXT_DUPLICATE_ACCOUNT_NUMBER_TOOLTIP] = value;
            }
        }
    }

    public static string DQNextQReportCS_Text_Total
    {
        get
        {
            if (HttpContext.Current.Session[RISK_DQNEXTQREPORT_TEXT_TOTAL] != null)
            {
                return (string)HttpContext.Current.Session[RISK_DQNEXTQREPORT_TEXT_TOTAL];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(RISK_DQNEXTQREPORT_TEXT_TOTAL);
            }
            else
            {
                HttpContext.Current.Session[RISK_DQNEXTQREPORT_TEXT_TOTAL] = value;
            }
        }
    }

    public static Dictionary<int, string> RiskMCFDQRainbowReport
    {
        get
        {
            var key = RISK_MCF_DQRAINBOWREPORT + "_" + HttpContext.Current.Session.SessionID;
            if (HttpContext.Current.Cache[key] != null)
            {
                return (Dictionary<int, string>)HttpContext.Current.Cache[key];
            }
            else
            {
                return new Dictionary<int, string>();
            }
        }
        set
        {
            var key = RISK_MCF_DQRAINBOWREPORT + "_" + HttpContext.Current.Session.SessionID;
            HttpContext.Current.Cache[key] = value;
        }
    }

    public static AdvancedFilterConfig AdvancedFilterConfigs
    {
        get
        {
            if (HttpContext.Current.Session["AdvancedFilterConfig"] != null)
            {
                return (AdvancedFilterConfig)HttpContext.Current.Session["AdvancedFilterConfig"];
            }
            else
            {
                return new AdvancedFilterConfig();
            }
        }
        set
        {
            HttpContext.Current.Session["AdvancedFilterConfig"] = value;
        }
    }

    public static DateTime? DateAvailableFilter
    {
        get
        {
            if (HttpContext.Current.Session["DateAvailableFilter"] != null)
            {
                return (DateTime)HttpContext.Current.Session["DateAvailableFilter"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["DateAvailableFilter"] = value;
        }
    }

    public static MCF_DetectionQueue_RequeuedAssignment MCF_DQTemporaryBarometerReport
    {
        get
        {
            var key = "MCF_DQTemporaryBarometerReport_" + HttpContext.Current.Session.SessionID;
            if (HttpContext.Current.Cache[key] == null)
            {
                MCF_DetectionQueue_RequeuedAssignment requeuedSession = new MCF_DetectionQueue_RequeuedAssignment();
                HttpContext.Current.Cache[key] = requeuedSession;
            }
            return (MCF_DetectionQueue_RequeuedAssignment)HttpContext.Current.Cache[key];
        }
        set
        {
            HttpContext.Current.Cache["MCF_DQTemporaryBarometerReport_" + HttpContext.Current.Session.SessionID] = value;
        }
    }

    public static DetectionQueue MCF_BarometerReport
    {
        get
        {
            var key = "MCF_BarometerReport_" + HttpContext.Current.Session.SessionID;
            if (HttpContext.Current.Cache[key] != null)
                return (DetectionQueue)HttpContext.Current.Cache[key];
            return null;
        }
        set
        {
            HttpContext.Current.Cache["MCF_BarometerReport_" + HttpContext.Current.Session.SessionID] = value;
        }
    }

    public static MCF_RetrievalsChargebacks MCF_RetrievalsChargebacksReferer
    {
        get
        {
            if (HttpContext.Current.Session["MCF_RetrievalsChargebacksReferer"] != null)
            {
                return (MCF_RetrievalsChargebacks)HttpContext.Current.Session["MCF_RetrievalsChargebacksReferer"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["MCF_RetrievalsChargebacksReferer"] = value;
        }
    }

    public static Dictionary<string, string> ResourceTitleMultiChoose
    {
        get
        {
            if (HttpContext.Current.Session["ResourceTitleMultiChoose"] != null)
            {
                return (Dictionary<string, string>)HttpContext.Current.Session["ResourceTitleMultiChoose"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["ResourceTitleMultiChoose"] = value;
        }
    }

    public static MCF_DetectionQueue_RequeuedAssignment MCF_DQTemporarySecurityReport
    {
        get
        {
            var key = "MCF_DQTemporarySecurityReport_" + HttpContext.Current.Session.SessionID;
            if (HttpContext.Current.Cache[key] == null)
            {
                MCF_DetectionQueue_RequeuedAssignment requeuedSession = new MCF_DetectionQueue_RequeuedAssignment();
                HttpContext.Current.Cache[key] = requeuedSession;
            }
            return (MCF_DetectionQueue_RequeuedAssignment)HttpContext.Current.Cache[key];
        }
        set
        {
            var key = "MCF_DQTemporarySecurityReport_" + HttpContext.Current.Session.SessionID;
            HttpContext.Current.Cache[key] = value;
        }
    }

    public static MCF_DetectionQueue_RequeuedAssignment MCF_DQTemporaryNextQueue
    {
        get
        {
            var key = "MCF_DQTemporaryNextQueue_" + HttpContext.Current.Session.SessionID;
            if (HttpContext.Current.Cache[key] == null)
            {
                MCF_DetectionQueue_RequeuedAssignment requeuedSession = new MCF_DetectionQueue_RequeuedAssignment();
                HttpContext.Current.Cache[key] = requeuedSession;
            }
            return (MCF_DetectionQueue_RequeuedAssignment)HttpContext.Current.Cache[key];
        }
        set
        {
            HttpContext.Current.Cache["MCF_DQTemporaryNextQueue_" + HttpContext.Current.Session.SessionID] = value;
        }
    }

    public static int Risk_ModeMCF
    {
        get
        {
            if (HttpContext.Current.Session["Risk_ModeMCF"] == null)
            {
                HttpContext.Current.Session["Risk_ModeMCF"] = (int)ShowRiskMCF.MCFRisk;
                return (int)HttpContext.Current.Session["Risk_ModeMCF"];
            }

            return (int)HttpContext.Current.Session["Risk_ModeMCF"];
        }
        set
        {
            HttpContext.Current.Session["Risk_ModeMCF"] = value;
        }
    }

    public static DataTable Risk_MCF_DispositionList
    {
        get
        {
            var key = RISK_MCF_DISPOSITIONLIST + "_" + HttpContext.Current.Session.SessionID;
            if (HttpContext.Current.Cache[key] != null)
            {
                return (DataTable)HttpContext.Current.Cache[key];
            }
            else
            {
                return null;
            }
        }
        set
        {
            var key = RISK_MCF_DISPOSITIONLIST + "_" + HttpContext.Current.Session.SessionID;
            if (value == null)
            {
                HttpContext.Current.Cache.Remove(key);
            }
            else
            {
                HttpContext.Current.Cache[key] = value;
            }
        }
    }

    public static string MCF_Security_CurrentMerchant
    {
        get
        {
            if (HttpContext.Current.Session["MCF_Security_CurrentMerchant"] != null)
            {
                return HttpContext.Current.Session["MCF_Security_CurrentMerchant"].ToString();
            }

            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["MCF_Security_CurrentMerchant"] = value;
        }
    }
    public static int? MCF_DQ_CurrentAssignmentID
    {
        get
        {
            if (HttpContext.Current.Session[RISK_MCF_DQ_CURRENTASSIGNMENTID] != null)
            {
                return HttpContext.Current.Session[RISK_MCF_DQ_CURRENTASSIGNMENTID].ASConverter<int>();
            }

            return null;
        }
        set
        {
            HttpContext.Current.Session[RISK_MCF_DQ_CURRENTASSIGNMENTID] = value;
        }
    }
}
