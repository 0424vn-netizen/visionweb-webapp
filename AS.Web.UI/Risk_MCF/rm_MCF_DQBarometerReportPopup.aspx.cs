using AS.Controls.Pages;
using System;
using System.Web.UI;

namespace As.VisionWeb.Web
{
    [PagePermission("RskQueue,MSRskQueue")]
    public partial class RiskMangementBaorometerReportModal : ReportPage
    {
        #region ---- Variable & Enum ----

        private WebSiteEnums.AssignmentType _AssignmentType
        {
            get
            {
                if (!string.IsNullOrEmpty(this.SecureQueryString["AssignmentType"]))
                {
                    return (WebSiteEnums.AssignmentType)Enum.Parse(typeof(WebSiteEnums.AssignmentType), this.SecureQueryString["AssignmentType"]);
                }
                else
                {
                    return WebSiteEnums.AssignmentType.All;
                }
            }
        }

        private static int RequeueSessionID
        {
            get
            {
                return RiskSessionManager.MCF_DQTemporaryBarometerReport.RequeueSessionID;
            }
        }

        protected string ApplyFilterId
        {
            get
            {

                if (!string.IsNullOrEmpty(SecureQueryString["ApplyFilterId"]))
                {
                    return Convert.ToString(SecureQueryString["ApplyFilterId"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #endregion ---- Variable & Enum ----

        #region ---- Events Handle ----

        protected void Page_Load(object sender, EventArgs e)
        {
            //load data when open modal call needDataSource
            IsBindDataOnLoad = true;
            SetInfo();
        }

        #endregion ---- Events Handle ----

        #region ---- Public Method ----

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string[] MerchantNumberClick(string merchantNumber, string status, string reportDate, string assignmentID)
        {
            bool reloadRainbowReport = false;
            ReportPage page = new ReportPage();
            string riskReportIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(page.ID, new string[] { "MerchantNumber" });
            string url = "rm_MCF_RiskReport.aspx?" + page.BuildSecureQueryString(
                        string.Format("merchantnumber={0}&IsPopup={1}{2}", merchantNumber, true, riskReportIntruderQuery));

            return new string[] { url, reloadRainbowReport.ToString() };
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string[] UpdateRequeuedMerchant(string status, string merchantNumber, string cycleId, string parentCycleID, string applyFilteredId)
        {
            string[] data = RM_MCF_GeneralFuncsLib.UpdateRequeuedMerchant(RequeueSessionID, status, merchantNumber, cycleId, parentCycleID, WebSiteEnums.PAGE_CODE.DQ, applyFilteredId);
            return new string[] { data[0], data[1], data[2], data[3] };
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string[] UpdateAllRequeuedMerchant(string status, int filterWorkingStatus, string applyFilteredId)
        {
            string[] data = RM_MCF_GeneralFuncsLib.UpdateAllRequeuedMerchant(RequeueSessionID, status, filterWorkingStatus, WebSiteEnums.PAGE_CODE.DQ, applyFilteredId);
            return new string[] { data[0], data[1], data[2] };
        }

        #endregion ---- Public Method ----

        #region Private Method

        private void SetInfo()
        {
            string pageTitle = string.Empty;
            string headerTitle = string.Empty;
            switch (_AssignmentType)
            {
                case WebSiteEnums.AssignmentType.DetectionQueue:
                case WebSiteEnums.AssignmentType.WorkQueue:
                    pageTitle = headerTitle = GetLocalResourceObject("BarometerReportTitle").ToString();
                    break;
                case WebSiteEnums.AssignmentType.AggregateQueue:
                    pageTitle = headerTitle = GetLocalResourceObject("AggregateQueueTitle").ToString();
                    break;
                case WebSiteEnums.AssignmentType.DetectionQueueDistinct:
                    pageTitle = headerTitle = GetLocalResourceObject("DistinctQueueTitle").ToString();
                    break;
            }

            Page.Title = pageTitle;
            uxPageTitle.ReportTitle = headerTitle;
        }
        #endregion
    }
}
