using System;
using System.ComponentModel;
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
using System.Reflection;
using DocumentFormat.OpenXml.Math;

public class WebSiteEnums
{
    public enum ProductEnvironment
    {
        MS,
        CS,
        RISK,
        MS_CS,
        MS_CS_RISK,
        MS_RISK,
        CS_RISK,
    }

    public enum UserHierarchyMode
    {
        Unknown,
        CS,
        AS,
        Site,
        Hierarchy,
        Merchant,
        Headquarter


    }

    public enum PCIHierarchyType
    {
        PCIClientAdmin = 99,
        PCIAdmin = 7,
        PCIAdvanced = 8,
        PCIBasic = 9,
        None = 0
    }

    public enum SecViewMode
    {
        CC = 0x1,
        DDA = 0x2,
        TAX = 0x4
    }

    #region RISK MANAGEMENT

    public enum WatchFilterTypes
    {
        ALL,
        MERCHANT,
        NONE,
        SELECT_ONE,
        HIERARCHY
    }

    public enum AssignmentActiveTypes
    {
        All = 0,
        Active = 1,
        Expired = 2
    }

    public enum AssignmentFilterTypes
    {
        All = 0,
        User = 1,
        Group = 2,
        Assignment = 3,
        Subsite = 4
    }

    public enum FeatureMode
    {
        Edit = 0,
        View = 1,
        Create = 2
    }

    public enum AutoQueueMode
    {
        Edit = 0,
        Create = 1
    }

    public enum AssignmentFilterModes
    {
        All = 0,
        Assigned = 1,
        NotAssigned = 2,
        Other = 3
    }

    /// <summary>
    /// Summary description for IRiskParamFilter
    /// </summary>
    public enum ParamFilterMode
    {
        Assignment = 1,
        ParameterDefine = 2,
        Adhoc = 3

    }

    public enum MerchantWorkedType
    {
        All = -1,
        NotWorked = 0,
        Worked = 1,
        Requeued = 2
    }

    public enum MerchantReportType
    {
        WorkedDetail,
        WorkedSummary,
        AssignmentVolumeSummary,
        AssignmentAlertSummary
    }

    public enum WatchStatus
    {
        Off = 0,
        On = 1,
        NA = 2
    }

    public enum FutureDeliveryIndicator
    {
        All = -1,
        Yes = 1,
        No = 0
    }

    #endregion

    #region Detection Queues

    public enum RiskReportType
    {
        RainbowReport = 1,
        FlatReport = 2,
        NextQReport = 3
    }

    public enum WorkTypes
    {
        All = 0,
        NotWorked = 1,
        Worked = 2
    }

    public enum DetectionQueueExcludeType
    {
        NotExclude = 0,
        Exclude = 1
    }

    public enum WorkStatus
    {
        Work = 0,
        WorkInProgress = 1,
        Worked = 2,
        WorkInProgressByOther = 3
    }

    public enum AssignmentType
    {
        [Description("Detection Queue")]
        DetectionQueue = 0,
        [Description("Work Queue")]
        WorkQueue = 1,
        [Description("Detection Queue Distinct")]
        DetectionQueueDistinct = 2,
        [Description("Aggregate Queue")]
        AggregateQueue = 3,
        [Description("All")]
        All = -1,
        [Description("Subsite")]
        Subsite = 4
    }

    public static string GetEnumAssTypeDescription(WebSiteEnums.AssignmentType value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());
        object[] attributes = fi.GetCustomAttributes(true);
        if (attributes != null && attributes.Length > 0)
        {
            return ((DescriptionAttribute)attributes[0]).Description;
        }
        else
        {
            return value.ToString();
        }
    }

    #endregion

    public enum MgmtReportType
    {
        WorkedDetail,
        WorkedSummary,
        AssignmentVolumeSummary,
        AssignmentAlertSummary,
        MerchantDetail
    }
    public enum DefaultPasswordValidation
    {
        Min = 8,
        Max = 50,
        NumberUpper = 1,
        NumberLower = 1,
        NumberSpecial = 0,
        MinPasswordHistory = 10,
        PasswordUserBeforeExpiredDays = 10,
        PasswordSystemExpiredDays = 10,
        PasswordAdminResetExpiredDays = 10
    }

    public enum LanguageCode
    {
        English = 1,
        Spanish = 2
    }

    public enum UserType
    {
        CS = 1,
        MS = 2,
        ALL = 0
    }

    public enum OrganizationMode
    {
        ALL,
        BYUSER,
        BYSALESREP
    }

    public enum LOGOUT_MODE
    {
        JUMPSITE = 3,
        SSO = 4
    }

    public enum PAGE_CODE
    {
        // Include page: Risk report, DectectionQueue, SecurityReport, RetCb, NextQueue, Adhoc
        RP,
        DQ,
        RC,
        NQ,
        SR,
        AR
    }

    public enum SUPPLEMENTAL_FEATURE
    {
        MerchantRank,
        ACHHoldDays,
        OwnerLastName,
        Zip3,
        MerchantFundingStatus,
        CreaditCore,
        ECommerce,
        Keyed,
        Swiped,
        LeadSource,
        ReferralSource
    }

    public enum Filter_Extend
    {
        GverifyCode,
        GauthenticateCode,
        G2Compass,
        ActualDeliveryDays,
        DaystoFund,
        FutureDeliveryIndicator,
        FutureDeliveryDayMaximum,
        FICOScore,
        SeasonalIndicator,
        SeasonalActiveMonths,
        SeasonalIndicatorStandard,
        FICOScoreStandard,
        DaystoFundStandard,
        RiskRatingStandard
    }

    public enum ManageCustomView
    {
        FullView = 0,
        Public = 1,
        Private = 2
    }

    //42782 VW – CAYAN - Implement New TSYS Processing Platform - add new
    public enum StatementDetailReportType
    {
        MerchantInfo = 1,
        GetMessage = 2,
        ActivitySummary = 3,
        ActivitySumaryTotal = 4,
        Deposits = 5,
        Chargebacks = 6,
        Fees = 7,
        FeesTotal = 8,
        Adjustments = 9,
        AdjustmentsTotal = 10,
        StatementTotal = 11,
        DepositTotal = 12,
        ChargebackTotal = 13
    }


    public enum ASUserAction
    {
        Edit = 1,
        Delete = 2,
        Active = 3,
        Deactive = 4,
        None = 5
    }

    public enum MCF_MerchantWorkingStatus
    {
        All = -1,
        ReadyToWork = 0,
        Worked = 1,
        WIPByme = 2,
        WIPbyOther = 3,
        Requeued = 4
    }

    public enum PageModeEnums
    {
        Assignment,
        DetectionQueue,
        RiskReport,
        TransactionHistory
    }

    public enum PageSectionEnums
    {
        BarometerReport,
        SecurityReport,
        NextQueue,
        Assignment
    }

    public enum ReviewedTypes
    {
        All = 0,
        NotReviewed = 1,
        Reviewed = 2
    }

    public enum ExtractDateRange
    {
        Daily = 1,
        Mothly = 2,
        Range = 3
    }

    public enum CardTypes
    {
        Visa,
        MasterCard
    }

    public enum AutoApprovedIndicator
    {
        All = -1,
        Yes = 1,
        No = 0
    }

    public enum AcqAgentSharedLiability
    {
        All = -1,
        Yes = 1,
        No = 0
    }

    public enum VisaRegistration
    {
        All = -1,
        Yes = 1,
        No = 0
    }

    public enum MCRegistration
    {
        All = -1,
        Yes = 1,
        No = 0
    }

    public enum DiscoverRegistration
    {
        All = -1,
        Yes = 1,
        No = 0
    }

    public enum AMEXRegistration
    {
        All = -1,
        Yes = 1,
        No = 0
    }

    public enum HighNDX
    {
        All = -1,
        Yes = 1,
        No = 0
    }

    public enum CBproducing
    {
        All = -1,
        Yes = 1,
        No = 0
    }

    public enum PRINUSForeign
    {
        All = -1,
        Yes = 1,
        No = 0
    }

    public enum ComplianceRegulatoryReputational
    {
        All = -1,
        Yes = 1,
        No = 0
    }

    public enum PersonalGuarantee
    {
        All = -1,
        Yes = 1,
        No = 0
    }
}