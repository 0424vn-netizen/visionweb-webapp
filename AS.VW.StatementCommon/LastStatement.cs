using AS.Common;
using AS.Common.DataProtection;
using AS.Controls.Pages;
using AS.LoneStar.Client.StatementApi;
using AS.LoneStar.Client.StatementApi.AccessOneApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS.VW.StatementCommon
{
    public class LastStatement
    {
        public const string HTML_EM_DASH = "&mdash;";
        const string CellHTMLEmDash = "<span data='{0}'>{1}</span>";
        public void LastStatementFormat(SecurePage page, Literal ltViewStatement, LinkButton lnkBt, DataRow data, LastStatementFormatInfoModel lastStatementFormatInfo)
        {
            this.LastStatementFormat(page, ltViewStatement, lnkBt, data, lastStatementFormatInfo, false);
        }

        public void LastStatementFormat(SecurePage page, Literal ltViewStatement, LinkButton lnkBt, DataRow data, LastStatementFormatInfoModel lastStatementFormatInfo, bool isConvertEmdash)
        {
            var isConvertion = data.Table.Columns.Contains("IsConvertion") 
                                        && !string.IsNullOrEmpty(data["IsConvertion"].ToString())
                                        && Convert.ToBoolean(data["IsConvertion"].ToString());
            // 43842 - FD to TSYS Merchant Migration
            if (isConvertion)
            {
                FormatStatementWithConvertion(page, ltViewStatement, lnkBt, data, lastStatementFormatInfo, isConvertEmdash);
            }
            else
            {
                FormatStatement(page, ltViewStatement, lnkBt, data, lastStatementFormatInfo, isConvertEmdash);
            }
        }

        private void FormatStatementWithConvertion(SecurePage page, Literal ltViewStatement, LinkButton lnkBt, DataRow data, LastStatementFormatInfoModel lastStatementFormatInfo, bool isConvertEmdash)
        {
            if (ltViewStatement != null)
            {
                bool hasEmDashName = isConvertEmdash && string.IsNullOrEmpty(data["LastStatementExport"].ToString().Trim());
                ltViewStatement.Visible = true;
                lnkBt.Visible = false;
                if (!string.IsNullOrEmpty(data["LastStatement"].ToString()) &&
                    !string.IsNullOrEmpty(data["StatementID"].ToString()))
                {
                    string[] statementIDs = data["StatementID"].ToString().Split(',');
                    DateTime reportDate = DateTime.Parse(data["LastStatement"].ToString());
                    string fileName = string.Format("{0}-{3}-{1}{2}.pdf",
                        data["Entity"], reportDate.ToString(lastStatementFormatInfo.ReportDateFormat),
                        statementIDs.Length > 1 ? "-[1]" : string.Empty, lastStatementFormatInfo.StmtResource);

                    string secureString = page.BuildSecureQueryString(
                        string.Format("StmtId={0}&dl=1&fileName={1}&tracking=true&reportDate={2}&merchantNumber={3}", statementIDs[0], fileName, reportDate, data["Entity"]));

                    var cellHTML = "<span style=\"display:inline-block; position:relative; \">"
                        + "<a class=\"link\" href=\"{0}\" style=\"cursor:pointer\">{1}</a></span>";


                    //44353 - MPS Conversion - Phase 5 -  9/13/2018
                    ltViewStatement.Text = VeraCodeSolution.GetOutputHtmlString(
                    string.Format(hasEmDashName ? CellHTMLEmDash : cellHTML
                        , lastStatementFormatInfo.StatementPageUrl + secureString
                        , hasEmDashName ? HTML_EM_DASH : data["LastStatementExport"].ToString()));
                }
                else
                {
                    string merchantNumber = Cryptophy.EncryptText(data["Entity"].ToString());
                    string secureString = page.BuildSecureQueryString(
                        string.Format("dlAPI=1&merchantNumber={0}", merchantNumber));

                    var cellHTML = "<span style=\"display:inline-block; position:relative; \">"
                        + "<a class=\"link\" href=\"{0}\" style=\"cursor:pointer\">{1}</a></span>";

                    ltViewStatement.Text = VeraCodeSolution.GetOutputHtmlString(
                    string.Format(hasEmDashName ? CellHTMLEmDash : cellHTML
                        , lastStatementFormatInfo.StatementPageUrl + secureString
                        , hasEmDashName ? HTML_EM_DASH : data["LastStatementExport"].ToString()));
                }
            }
        }
        private void FormatStatement(SecurePage page, Literal ltViewStatement, LinkButton lnkBt, DataRow data, LastStatementFormatInfoModel lastStatementFormatInfo, bool isConvertEmdash)
        {
            bool hasEmDashName = isConvertEmdash && string.IsNullOrEmpty(data["LastStatementExport"].ToString().Trim());           
            string selectedDate = data["LastStatement"].ToString();

            if (string.IsNullOrEmpty(selectedDate))
            {
                ltViewStatement.Visible = hasEmDashName;
                ltViewStatement.Text = (hasEmDashName ? HTML_EM_DASH : string.Empty);
                HideLinkButton(lnkBt);
                return;
            }
            // Save User Activity
            DateTime tempDate;
            DateTime.TryParse(selectedDate, out tempDate);

            if (data["BackEndProcessor"] != DBNull.Value)
            {
                lastStatementFormatInfo.StmURL = string.IsNullOrEmpty(lastStatementFormatInfo.StmURL) ? "StatementDetail_MPS.aspx" : lastStatementFormatInfo.StmURL;
            }

            if (data["FileSource"] != DBNull.Value && !string.IsNullOrEmpty(data["FileSource"].ToString()))
            {
                BuildStatementFromFileSource(page, data, lnkBt, isConvertEmdash, tempDate);                    
            }
            else
            {
                if (data["StatementID"] == DBNull.Value || string.IsNullOrEmpty(data["StatementID"].ToString()))
                {
                    string queryString = page.BuildSecureQueryString(string.Format("ReportDate={0}&MerchantNumber={1}", tempDate.Ticks.ToString(), data["Entity"]));

                    if (lnkBt != null)
                    {
                        //44353 - MPS Conversion - Phase 5 -  9/13/2018
                        lnkBt.Text = hasEmDashName ? HTML_EM_DASH : data["LastStatementExport"].ToString();
                        // CommandArgument : 0 - queryString, 1 - MerchantNumber, 2 - StatementDetail Page
                        lnkBt.CommandArgument = queryString + "," + data["Entity"] + "," + lastStatementFormatInfo.StmURL;
                        lnkBt.CommandName = "ViewStatement";
                    }
                }
                else
                {
                    BuildStatementLinkFromStatementId(page, ltViewStatement, data, lastStatementFormatInfo, isConvertEmdash);
                    HideLinkButton(lnkBt);
                }
            }
        }
        private void HideLinkButton(LinkButton lnkBt)
        {
            if (lnkBt != null)
            {
                lnkBt.Visible = false;
            }
        }

        private void BuildStatementFromFileSource(SecurePage page, DataRow data, LinkButton lnkBt, bool isConvertEmdash, DateTime tempDate)
        {
            string cellHTML = "<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"ShowPopupModal('{0}'); return false;\">{1}</a>";
            bool hasEmDashName = isConvertEmdash && string.IsNullOrEmpty(data["LastStatementExport"].ToString().Trim());
            string fullURL = "StatementDetail_FIS.aspx?"
                       + page.BuildSecureQueryString(string.Format("ReportDate={0}&MerchantNumber={1}", tempDate.Ticks.ToString(), data["Entity"]));

            if (lnkBt != null)
            {
                lnkBt.Text = VeraCodeSolution.GetOutputHtmlString(
                    string.Format(hasEmDashName ? CellHTMLEmDash : cellHTML, fullURL, hasEmDashName ? HTML_EM_DASH : data["LastStatementExport"].ToString()));
            }
        }
        private void BuildStatementLinkFromStatementId(SecurePage page, Literal ltViewStatement, DataRow data, LastStatementFormatInfoModel lastStatementFormatInfo, bool isConvertEmdash)
        {
            bool hasEmDashName = isConvertEmdash && string.IsNullOrEmpty(data["LastStatementExport"].ToString().Trim());            
            string[] statementIDs = data["StatementID"].ToString().Split(',');
            DateTime reportDate = DateTime.Parse(data["LastStatement"].ToString());
            string fileName = string.Format("{0}-{3}-{1}{2}.pdf",
                data["Entity"], reportDate.ToString(lastStatementFormatInfo.ReportDateFormat),
                statementIDs.Length > 1 ? "-[1]" : string.Empty, lastStatementFormatInfo.StmtResource);
            string secureString = page.BuildSecureQueryString(
                string.Format("StmtId={0}&dl=1&fileName={1}&tracking=true&reportDate={2}&merchantNumber={3}", statementIDs[0], fileName, reportDate, data["Entity"]));

            var cellHTML = "<span style=\"display:inline-block; position:relative; \">"
                + "<a class=\"link\" href=\"{0}\" style=\"cursor:pointer\">{1}</a>";


            if (ltViewStatement != null)
            {
                ltViewStatement.Visible = true;
                //44353 - MPS Conversion - Phase 5 -  9/13/2018
                //43842 - FD to TSYS Merchant Migration
                ltViewStatement.Text = VeraCodeSolution.GetOutputHtmlString(
                string.Format(hasEmDashName ? CellHTMLEmDash : cellHTML
                    , lastStatementFormatInfo.StatementPageUrl + secureString
                    , hasEmDashName ? HTML_EM_DASH : data["LastStatementExport"].ToString()));
            }
        }
        public Dictionary<string, byte[]>  ViewStatementApi(string merchantNumber, bool isMerchantBelongToUser, string stmFileNameResource)
        {
            Dictionary<string, byte[]> data = null;
            if (isMerchantBelongToUser)
            {
                //Get statement list with order
                var items = GetOmahaMerchantStatementList(merchantNumber);

                if (items != null && items.Any())
                {
                    MerchantStatementItem item = items[0];
                    DateTime reportDate = ParseDateTime(item.StatementDate);
                    if (reportDate != DateTime.MinValue)
                    {
                        data = DownloadStatementApi(merchantNumber
                            , item.MerchantNumber, reportDate, item.StatementId, stmFileNameResource);
                    }
                }
            }

            return data;
        }
        private List<MerchantStatementItem> GetOmahaMerchantStatementList(string realMerchant)
        {
            StatementApi statementApi = new StatementApi();
            var items = statementApi.GetOmahaMerchantStatementList(realMerchant);

            if (items != null && items.Any())
            {
                return items.OrderByDescending(o => ParseDateTime(o.StatementDate)).ToList();
            }
            return new List<MerchantStatementItem>();
        }
        private DateTime ParseDateTime(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr)) return DateTime.MinValue;

            DateTime date;
            DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return date;
        }
        private Dictionary<string, byte[]> DownloadStatementApi(string currMerchantNumber, string realMerchantNumber, DateTime reportDate, string statementId, string defaultFileName)
        {
            StatementApi statementApi = new StatementApi();
            MerchantStatement statement = statementApi.GetOmahaMerchantStatement(realMerchantNumber, statementId);
            byte[] bytes = Convert.FromBase64String(statement.StatementData);
            string fileName = string.Format(defaultFileName, currMerchantNumber, reportDate.Month.ToString("0#"), reportDate.Year.ToString());
            string dataKey = string.Format("{0}|{1}|{2}", reportDate, fileName, statementId);
            Dictionary<string, byte[]> data = new Dictionary<string, byte[]>();
            data.Add(dataKey, bytes);

            return data;
        }
        public void TransferFileOrNot(bool isTranfer, Page page, byte[] buffer = null, string fileName = "")
        {
            if (isTranfer)
                TransferFileToClient(page, buffer, fileName);
            else
                page.ClientScript.RegisterStartupScript(GetType(), "ShowMessage", "showMessageError();", true);
        }
        private void TransferFileToClient(Page page, byte[] buffer, string fileName)
        {
            page.Response.ClearContent();
            try
            {
                page.Response.ClearHeaders();
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error("ClearHeaders: Last Statement - TransferFileToClient:\n" + ex.ToString());
            }
            page.Response.ContentType = "application/octet-stream";
            page.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            page.Response.BinaryWrite(buffer);
            page.Response.End();
        }
    }
}
