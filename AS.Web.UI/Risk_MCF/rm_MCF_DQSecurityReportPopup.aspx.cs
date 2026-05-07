using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace As.VisionWeb.Web
{
    [PagePermission("RskQueue,MSRskQueue")]
    public partial class RiskManagementSecurityReportModal : ReportPage
    {

        #region ---- Variable & Enum ----

        private static int RequeueSessionID
        {
            get
            {
                return RiskSessionManager.MCF_DQTemporarySecurityReport.RequeueSessionID;
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
            uxPageTitle.ReportTitle = GetLocalResourceObject("PageResource1.Title").ToString();
            uxTooltipDupeCount.Text = string.Format("<span class='tooltip-text1'>{0}</span>", GetLocalResourceObject("DupeCount.HeaderDescription").ToString());
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
            string[] data = RM_MCF_GeneralFuncsLib.UpdateRequeuedMerchant(RequeueSessionID, status, merchantNumber, cycleId, parentCycleID, WebSiteEnums.PAGE_CODE.SR, applyFilteredId);
            return new string[] { data[0], data[1], data[2], data[3] };
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string[] UpdateAllRequeuedMerchant(string status, int filterWorkingStatus, string applyFilteredId)
        {
            string[] data = RM_MCF_GeneralFuncsLib.UpdateAllRequeuedMerchant(RequeueSessionID, status, filterWorkingStatus, WebSiteEnums.PAGE_CODE.SR, applyFilteredId);
            return new string[] { data[0], data[1] };
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string[] GetWorkQueueAssignment()
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameters.Add(new FilterParameter("@IsNotExpire", 1, DbType.Boolean));
            DataTable assignTable = WebServices.RiskServices.GetReports("spa_RM_MCF_wq_GetWorkQueueAssignmentList", parameters);
            return new string[] { assignTable.Rows.Count.ToString() };
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string[] GetTransaction(int pageIndex, DateTime reportDate, bool isCSViewFullCard)
        {
            using (var sw = new StringWriter())
            {
                var page = new Page();
                var scriptManager = new Telerik.Web.UI.RadScriptManager();
                page.Controls.Add(scriptManager);
                Control control = page.LoadControl("~/UserControls/rm_MCF_Transaction.ascx");
                UserControls_rm_MCF_Transaction userControl = ((UserControls_rm_MCF_Transaction)control);
                userControl.MerchantList = RiskSessionManager.MCF_Security_CurrentMerchant;
                userControl.ReportDate = reportDate;
                userControl.IsCSViewFullCard = isCSViewFullCard;
                userControl.PageIndex = pageIndex;

                page.Controls.Add(userControl);
                HttpContext.Current.Server.Execute(page, sw, false);
                var html = sw.ToString();
                string reg = "(<!--{0}-->((.|\n)*)<!--/{0}-->)";
                string regHeader = string.Format(reg, "TransactionDetail");
                var transHtml = string.Empty;
                var pagingHtml = string.Empty;

                Match headerMatch = Regex.Match(html, regHeader);
                if(headerMatch.Success)
                    transHtml = headerMatch.Value;

                string regPaging = string.Format(reg, "TransactionPaging");
                Match PagingMatch = Regex.Match(html, regPaging);
                if (PagingMatch.Success)
                    pagingHtml = PagingMatch.Value;

                return new string[] { transHtml , pagingHtml };                            
            }
        }
        #endregion ---- Public Method ----
    }
}
