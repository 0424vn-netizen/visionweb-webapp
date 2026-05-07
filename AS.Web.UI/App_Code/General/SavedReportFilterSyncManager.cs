using AS.Common.DBManager;
using AS.Controls.ASP.Net;
using AS.Controls.Grid;
using AS.Controls.Telerik;
using AS.VW.Share.Validation;
using AS.Web.UI.Controls;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CardTypes = WebSiteEnums.CardTypes;

namespace AS.Web.UI.AppCode.General
{

    /// <summary>
    /// Summary description for SavedReportFilterSyncManager
    /// </summary>
    public static class SavedReportFilterSyncManager
    {

        public static void SyncMerchantNumberToRiskReport()
        {
            if (SessionManager.CurrentReportFilter == null)
                return;

            var savedReportFilterValue = SessionManager.CurrentReportFilter;

            if (savedReportFilterValue.HierarchyMode.Equals(HierarchyMode.MERCHNUMBER_NR_PARTIAL, StringComparison.OrdinalIgnoreCase)                
                || savedReportFilterValue.HierarchyMode.Equals(HierarchyMode.MERCHANT_NR, StringComparison.OrdinalIgnoreCase))
            {
                SessionManager.ShareMerchantNumber = savedReportFilterValue.Value;
            }
        }

        public static void SyncMerchantNumberToMPSReportFilter()
        {
            if (SessionManager.CurrentReportFilter == null)
                return;

            var savedReportFilterValue = SessionManager.CurrentReportFilter;            

            if (savedReportFilterValue.HierarchyMode.Equals(HierarchyMode.MERCHNUMBER_NR_PARTIAL, StringComparison.OrdinalIgnoreCase))                
            {
                savedReportFilterValue.HierarchyMode = HierarchyMode.MERCHNUMBER_NR_PARTIAL;
                savedReportFilterValue.ID = GetHierarchyID(HierarchyMode.MERCHNUMBER_NR_PARTIAL);
            }
            else if(savedReportFilterValue.HierarchyMode.Equals(HierarchyMode.MERCHANT_NR, StringComparison.OrdinalIgnoreCase))
            {
                savedReportFilterValue.HierarchyMode = HierarchyMode.MERCHANT_NR;
                savedReportFilterValue.ID = GetHierarchyID(HierarchyMode.MERCHANT_NR);
            }
        }

        public static void SaveBrandFraudReportFilter(UCBrandFraudReportFilterControl searchControl, UCBrandFraudReportFilterMode searchMode, 
            ref HierarchyFilterValue reportValue, bool isSearch)
        {
            ASRadioButton uxDaily = searchControl.UxDaily;
            ASRadioButton uxMonthly = searchControl.UxMonthly;
            ASRadDatePicker uxDate = searchControl.UxDate;
            ASRadDatePicker uxFromDate = searchControl.UxFromDate;
            ASRadDatePicker uxEndDate = searchControl.UxEndDate;
            RadComboBox uxPaymentEntitieTypes = searchControl.UxPaymentEntitieTypes;
            RadComboBox uxAcquirerList = searchControl.UxAcquirerList;
            RadComboBox uxMerchantList = searchControl.UxMerchantList;
            TextBox uxMerchantNumber = searchControl.UxMerchantNumber;  

            var savedReportFilterValue = SessionManager.CurrentReportFilter;

            //Get report filter form session
            if (savedReportFilterValue != null)
                reportValue = savedReportFilterValue;
            else
                reportValue = new HierarchyFilterValue();
            // Set Date Option         
            if (uxDaily.Checked)
            {
                reportValue.DateOption = DateOptionMode.Daily;
                reportValue.DateOptionValue.From = reportValue.DateOptionValue.To = uxDate.SelectedDate.Value;
            }
            else if (uxMonthly.Checked)
            {
                reportValue.DateOption = DateOptionMode.Monthly;
                reportValue.DateOptionValue.From = uxDate.SelectedDate.Value;
            }
            else
            {
                reportValue.DateOption = DateOptionMode.DateRange;
                reportValue.DateOptionValue.From = uxFromDate.SelectedDate.Value;
                reportValue.DateOptionValue.To = uxEndDate.SelectedDate.Value;
            }

            // Set report filter
            if (isSearch)
            {
                savedReportFilterValue = reportValue;

                if (searchMode == UCBrandFraudReportFilterMode.AcquirerFraudReport)
                {
                    SessionManager.BrandFraudReportFilterPaymentEntitieType = WebSiteConstants.PaymentEntitieTypeAcquirerID;

                    if (string.IsNullOrEmpty(uxAcquirerList.SelectedValue))
                    {
                        savedReportFilterValue.Value = uxAcquirerList.Text.Trim();
                    }
                    else
                    {
                        savedReportFilterValue.Value = uxAcquirerList.SelectedValue;
                    }
                }
                if (searchMode == UCBrandFraudReportFilterMode.MerchantFraudReport)
                {
                    if (uxPaymentEntitieTypes.SelectedValue == WebSiteConstants.PaymentEntitieTypeAcquirerID)
                    {
                        SessionManager.BrandFraudReportFilterPaymentEntitieType = WebSiteConstants.PaymentEntitieTypeAcquirerID;

                        if (string.IsNullOrEmpty(uxAcquirerList.SelectedValue))
                        {
                            savedReportFilterValue.Value = uxAcquirerList.Text.Trim();
                        }
                        else
                        {
                            savedReportFilterValue.Value = uxAcquirerList.SelectedValue;
                        }
                    }
                    else if (uxPaymentEntitieTypes.SelectedValue == WebSiteConstants.PaymentEntitieTypeMerchantIDFull)
                    {
                        SessionManager.BrandFraudReportFilterPaymentEntitieType = WebSiteConstants.PaymentEntitieTypeMerchantIDFull;

                        savedReportFilterValue.HierarchyMode = HierarchyMode.MERCHANT_NR;

                        if (string.IsNullOrEmpty(uxMerchantList.SelectedValue))
                        {
                            savedReportFilterValue.Value = uxMerchantList.Text;
                        }
                        else
                        {
                            savedReportFilterValue.Value = uxMerchantList.SelectedValue;
                        }
                    }
                    else if (uxPaymentEntitieTypes.SelectedValue == WebSiteConstants.PaymentEntitieTypeMerchantIDPartial)
                    {
                        SessionManager.BrandFraudReportFilterPaymentEntitieType = WebSiteConstants.PaymentEntitieTypeMerchantIDPartial;

                        if (!string.IsNullOrEmpty(uxMerchantNumber.Text.Trim()))
                        {
                            savedReportFilterValue.HierarchyMode = HierarchyMode.MERCHNUMBER_NR_PARTIAL;
                            savedReportFilterValue.Value = uxMerchantNumber.Text.Trim();
                        }
                    }
                }
            }

            SessionManager.CurrentReportFilter = savedReportFilterValue;
        }

        public static void RestoreBrandFraudReportFilter(
            UCBrandFraudReportFilterControl searchControl, CardTypes cardType, ref HierarchyFilterValue reportValue
            )
        {

            ASRadioButton uxDaily = searchControl.UxDaily;
            ASRadioButton uxMonthly = searchControl.UxMonthly;
            ASRadioButton uxRange = searchControl.UxRange;
            ASRadDatePicker uxDate = searchControl.UxDate;
            ASRadDatePicker uxFromDate = searchControl.UxFromDate;
            ASRadDatePicker uxEndDate = searchControl.UxEndDate;
            RadComboBox uxAcquirerList = searchControl.UxAcquirerList;  

             reportValue = SessionManager.CurrentReportFilter;

            if (reportValue == null)
            {
                reportValue = new HierarchyFilterValue();
                reportValue.DateOption = DateOptionMode.Daily;
                reportValue.DateOptionValue.To = DateTime.Now;
                reportValue.DateOptionValue.From = DateTime.Now;
            }
            switch (reportValue.DateOption)
            {
                case DateOptionMode.Daily:
                    uxDaily.Checked = true;
                    uxDate.SelectedDate = reportValue.DateOptionValue.From;
                    break;
                case DateOptionMode.Monthly:
                    uxMonthly.Checked = true;
                    uxDate.SelectedDate = reportValue.DateOptionValue.From;
                    break;
                case DateOptionMode.DateRange:
                    {
                        uxRange.Checked = true;
                        uxFromDate.SelectedDate = reportValue.DateOptionValue.From;
                        uxEndDate.SelectedDate = reportValue.DateOptionValue.To;

                        // VIS-890: with Date Range option, as the Visa Merchant Fraud Report doesn't allow to search data > 31 days, so if Date range in the previous page > 31 days,
                        // populate maximum 31 days in the Visa Merchant Fraud Report.
                        if (reportValue.DateOptionValue.To.Subtract(reportValue.DateOptionValue.From).Days > 31)
                        {
                            uxFromDate.SelectedDate = reportValue.DateOptionValue.To.AddDays(-30);
                        }
                        break;
                    }
            }

            if (SessionManager.BrandFraudReportFilterPaymentEntitieType == WebSiteConstants.PaymentEntitieTypeAcquirerID)
            {
                var acquirerIdInput = reportValue.Value;

                if(string.IsNullOrEmpty(acquirerIdInput)) {
                    return;
                }

                DataTable dt = GeneralFuncsLib.GetAcquirers(cardType, acquirerIdInput);

                if (dt != null && dt.Rows.Count > 0)
                {
                    uxAcquirerList.SelectedValue = dt.Rows[0]["DataKey"].ToString();
                    uxAcquirerList.Text = dt.Rows[0]["DataText"].ToString();
                }
                else
                {
                    uxAcquirerList.Text = acquirerIdInput;
                }
            }
        }

        private static string GetHierarchyID(string hierarchyMode)
        {
            string result = string.Empty;

            var hierarchyFilter = SessionManager.HierarchyFilter;

            result = (GeneralFuncsLib.GetValueByFilters<int>(hierarchyFilter
                    , new string[1] { "HierarchyMode" }
                    , new string[1] { hierarchyMode }, "HierarchyID")).ToString();
            return result;
        }
    }

    public class UCBrandFraudReportFilterControl
    {
        public ASRadioButton UxDaily { get; set; }
        public ASRadioButton UxMonthly { get; set; }
        public ASRadioButton UxRange { get; set; }
        public ASRadDatePicker UxDate { get; set; }
        public ASRadDatePicker UxFromDate { get; set; }
        public ASRadDatePicker UxEndDate { get; set; }
        public RadComboBox UxPaymentEntitieTypes { get; set; }
        public RadComboBox UxAcquirerList { get; set; }
        public RadComboBox UxMerchantList { get; set; }
        public TextBox UxMerchantNumber { get; set; }

    }

    public enum UCBrandFraudReportFilterMode
    {
        AcquirerFraudReport = 0,
        MerchantFraudReport = 1
    }
}