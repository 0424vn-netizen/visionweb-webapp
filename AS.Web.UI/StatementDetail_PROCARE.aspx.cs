using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using System.Data;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System.IO;
using System.Text;
using AS.Common.Formater;
using AS.Common;
using AS.Security.WS.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WebSupergoo.ABCpdf9;

using AS.Common.Utilities;
using System.Configuration;
using iTextSharp.text.html.simpleparser;
using System.Web.UI.HtmlControls;

namespace As.VisionWeb.Web
{
    [PagePermission("StatementRpt,MSStatementRpt")]
    public partial class StatementDetail_PROCARE : ReportPage
    {
        #region Enum

        enum DataBindAction
        {
            BindPlanSummary,
            BindDeposits,
            BindChargebacks,
            BindFees,
            Adjustments,
            IDINE,
            BWHDetail,
            BWHMonthly,
            BWHOverall,
            CashAdvance,
            ReserveFund,
            AuthFees,
            InterFees,
            TransFees,
            CardBrandFees,
            OtherFees,
            Total
        }

        enum ReportType
        {
            Deposits,
            Chargebacks,
            PlanSummary,
            Fees,
            Adjustments,
        }

        enum PostBackAction
        {
            ExportPDF
        }

        #endregion Enum

        #region Constants
        private const string FILE_NAME_EXPORT = "StatementDetails";
        private const string SPA_STATEMENT_DETAIL = "spa_stmnt_GetStatementDetails_TSYS_XML";

        private const string DASH = "-";

        #endregion Constants

        #region Properties

        protected string MerchantNumber = "";
        protected DateTime ReportDate = DateTime.Now;
        protected double TotalDiscountDue = 0;
        protected double DiscountPaid = 0;
        protected double NetDiscountDue = 0;
        protected double TotalFeesDue = 0;
        protected double FeesDue = 0;
        protected double FeesPaid = 0;
        protected double NetFeesDue = 0;
        protected double AmountDeducted = 0;

        DataSet DataSetPlanSummary
        {
            get;
            set;
        }
        DataSet DataSetDeposits
        {
            get;
            set;
        }
        DataSet DataSetAdjustments
        {
            get;
            set;
        }
        DataSet DataSetIdine
        {
            get;
            set;
        }
        DataSet DataSetChargeback
        {
            get;
            set;
        }
        DataSet DataSetBWH
        {
            get;
            set;
        }
        DataSet DataSetCashAdvance
        {
            get;
            set;
        }
        DataSet DataSetReserveFund
        {
            get;
            set;
        }
        DataSet DataSetFees
        {
            get;
            set;
        }
        protected ClientInfo ContactInformation = null;

        #endregion Properties

        #region Methods

        #region Protected Methods

        protected override void PageInitialize()
        {
            if (IsIntruderDetected) return;

            this.GridIDs.Add("uxDeposits");
            this.GridIDs.Add("uxCardSumary");
            this.GridIDs.Add("uxSettlementDiscount");
            this.GridIDs.Add("uxSurcharge");
            this.GridIDs.Add("uxOtherFee");
            base.PageInitialize();

        }

        private void ProcessQueryString()
        {
            this.MerchantNumber = SecureQueryString["MerchantNumber"];
            string _reportdate = SecureQueryString["ReportDate"];
            this.ReportDate = new DateTime(long.Parse(_reportdate));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PageType = SecurePageType.Modal;
            IsBindDataOnLoad = true;
            if (IsSecureQueryString)
            {
                ProcessQueryString();
                DisplayLogo(false);
                if (!IsPostBack)
                {
                    BindData(false);
                }
            }
            //46652 - AW - Multi-Currency Transaction Display
            Literal11.Text = Literal11.Text.ToCurrencySymbol();
            Literal13.Text = Literal13.Text.ToCurrencySymbol();
            Literal26.Text = Literal26.Text.ToCurrencySymbol();
            Literal27.Text = Literal27.Text.ToCurrencySymbol();          


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Do nothing
        }

        protected void DisplayLogo(bool isExport)
        {
            string themeName = string.Empty;

            if (SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.Merchant)
            {
                ASThemeCollection themes = WebServices.SecurityServices.GetASThemsByUser(
                    SessionManager.CurrentUser.ASClient, MerchantNumber, 0);
                if (themes.Count > 0)
                {
                    themeName = themes[0].ThemeName;
                }
            }
            else
            {
                themeName = SessionManager.CurrentUserTheme.ThemeName;
            }

            string domain = string.Format("file:///{0}", Server.MapPath("~/"));
            string logoURL = string.Format("/App_Themes/{0}/img/logo.png", themeName);

            string logoUrl;
            if (isExport)
            {
                logoUrl = string.Format("<img src='{0}' alt='logo' />", String.Format("{0}{1}", domain, logoURL));
            }
            else
            {
                logoUrl = string.Format("<img src='{0}' alt='logo' />", logoURL);
            }
            uxReportTitle.ReportTitle = logoUrl;
            uxLogo.Text = logoUrl;
        }

        protected void BindMerchantData(bool isExportPdf)
        {
            // Merchant Information
            DataTable tbl = GetMerchantInfomation();          
            string clientInfo = string.Empty;
            if (tbl != null && tbl.Rows.Count > 0)
            {                
                //Client Information
                clientInfo = string.Format("{0}<br />{1}",
                    tbl.Rows[0]["ClientName"].ToString(),
                    FormatAddress(tbl.Rows[0]["ClientAddress1"],
                                  tbl.Rows[0]["ClientAddress2"],
                                  tbl.Rows[0]["ClientAddress3"],
                                  tbl.Rows[0]["ClientAddress4"]));
            }


            uxMerchantInfo.DataSource = tbl;
            uxMerchantInfo.DataBind();
            if (tbl != null && tbl.Rows.Count > 0)
            {                
                uxClientInfo.Text = VeraCodeSolution.GetOutputHtmlString(clientInfo);
            }
        }

        protected override void OnDataBindControls(Enum type, object sender)
        {
            ASGrid grid = (ASGrid)sender;
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams();
            parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
            parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
            switch ((DataBindAction)type)
            {
                case DataBindAction.BindPlanSummary:
                    {
                        parameters.Add("@ReportType", 3, DbType.Int32);
                        grid.DataSourceInvoker = new ASFuncInvoker(
                            WebServices.CsReportServices,
                            WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] {
                            SPA_STATEMENT_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                    }
                    break;
                case DataBindAction.BindDeposits:
                    {
                        parameters.Add("@ReportType", 4, DbType.Int32);
                        grid.DataSourceInvoker = new ASFuncInvoker(
                            WebServices.CsReportServices,
                            WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] {
                            SPA_STATEMENT_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                    }
                    break;
                case DataBindAction.BindChargebacks:
                    {
                        parameters.Add("@ReportType", 7, DbType.Int32);
                        grid.DataSourceInvoker = new ASFuncInvoker(
                            WebServices.CsReportServices,
                            WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] {
                            SPA_STATEMENT_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                    }
                    break;
                case DataBindAction.BindFees:
                    {
                        parameters.Add("@ReportType", 11, DbType.Int32);
                        grid.DataSourceInvoker = new ASFuncInvoker(
                            WebServices.CsReportServices,
                            WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] {
                            SPA_STATEMENT_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                    }
                    break;
                case DataBindAction.Adjustments:
                    {
                        parameters.Add("@ReportType", 5, DbType.Int32);
                        grid.DataSourceInvoker = new ASFuncInvoker(
                            WebServices.CsReportServices,
                            WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] {
                            SPA_STATEMENT_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                    }
                    break;
                case DataBindAction.IDINE:
                    {
                        parameters.Add("@ReportType", 6, DbType.Int32);
                        grid.DataSourceInvoker = new ASFuncInvoker(
                            WebServices.CsReportServices,
                            WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] {
                            SPA_STATEMENT_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                    }
                    break;

            }
        }

        protected override void OnPostBackActions(Enum type, object sender)
        {
            switch ((PostBackAction)type)
            {
                case PostBackAction.ExportPDF:
                    {                       
                        ExportPDF();
                        break;
                    }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["index"]))
            {
                uxClose.OnClientClick = "return parent.ClosePopupModal(" + SecureQueryString["index"] + ");";
            }
            else
            {
                uxClose.OnClientClick = "return parent.HidePopupModal();";
            }
        }

        protected void ButtonExcel_Click(object sender, EventArgs e)
        {
            this.IsNoCache = false;
            uxPlaceHolderStyleExcel.Visible = true;
            pnlBreakLineExportAdustments.Visible = true;
         
            pnlBreakLineExportDeposit.Visible = true;
          
            pnlBreakLineExportPlanSummary.Visible = true;

            uxExportFooter.Visible = true;
            ProcessQueryString();
            DisplayLogo(true);
            BindMerchantData(false);
            BindMessage();
            DoDataBindControls(DataBindAction.BindPlanSummary, uxPlanSummary);
            DoDataBindControls(DataBindAction.BindDeposits, uxDeposit);                    
            DoDataBindControls(DataBindAction.OtherFees, uxOtherFees);
            BindStatementTotal();

            // Response.Clear(); //this clears the Response of any headers or previous output

            Response.Buffer = true; //make sure that the entire output is rendered simultaneously
            Response.ContentType = "application/vnd.ms-excel";

            HttpContext.Current.Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}.xls", FILE_NAME_EXPORT));


            StringWriter sw = new StringWriter();
            HtmlTextWriter textWriter = new HtmlTextWriter(sw);
            File.WriteAllText(Server.MapPath("~/App_Data/" + FILE_NAME_EXPORT + ".xls"), textWriter.ToString(), Encoding.UTF8);

            uxPlaceHolderStyleExcel.RenderControl(textWriter);
            uxContentExport.RenderControl(textWriter);
            Response.Write(sw.ToString());
            sw.Close();
            textWriter.Close();
            Response.Flush();
            Response.End();
        }

        protected void ButtonPDF_Click(object sender, EventArgs e)
        {
            DisplayLogo(true);
            uxPlaceHolderStylePDF.Visible = true;
            uxPnlClientLogo.Visible = true;
            uxExportFooter.Visible = true;
            OnPostBackActions(PostBackAction.ExportPDF);
        }

        protected string FormatCurrency(object data)
        {
            return FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);
        }

        protected string FormatCurrencyHasZeroValue(object data)
        {
            string value = FormatInteger(data);
            if (value.IsNullOrEmpty() || value.Equals("0"))
            {
                return DASH;
            }
            return FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);
        }

        protected string FormatInteger(object data)
        {
            return FormatData.FormatInteger(data);
        }

        protected string FormatInteger2Digits(object data)
        {
            string number = FormatInteger(data);
            if (number.IsNullOrEmpty() || number.Trim().Equals("0"))
            {
                number = DASH;
            }
            else if (number.Length < 2)
            {
                number = number.PadLeft(2, '0');
            }
            return number;
        }

        protected string FormatNumber4Digits(object data)
        {
            return FormatData.FormatNumber(data, 4);
        }

        protected string FormatDate(object data)
        {
            if (data is DateTime)
            {
                return (DateTime.Parse(data.ToString())).ToString(WebSiteConstants.DATE_FORMAT);
            }
            return data.ToString();
        }

        protected string FormatDate2(object data)
        {
            if (data is DateTime)
            {
                return (DateTime.Parse(data.ToString())).ToString("MM/dd");
            }
            return data.ToString();
        }

        protected string FormatAddress(object addr1, object addr2, object addr3, object addr4)
        {
            StringBuilder result = new StringBuilder();
            if (addr1 != null && !addr1.ToString().IsNullOrEmpty())
            {
                result.AppendFormat("{0}<br />", addr1.ToString());
            }
            if (addr2 != null && !addr2.ToString().IsNullOrEmpty())
            {
                result.AppendFormat("{0}<br />", addr2.ToString());
            }
            if (addr3 != null && !addr3.ToString().IsNullOrEmpty())
            {
                result.AppendFormat("{0}<br />", addr3.ToString());
            }
            if (addr4 != null && !addr4.ToString().IsNullOrEmpty())
            {
                result.AppendFormat("{0}<br />", addr4.ToString());
            }
            return result.ToString();
        }

        protected string FormatAddress(object addr1, object addr2, object addr3, int index)
        {
            string address = string.Empty;
            object[] listAddress = new object[] { addr1, addr2, addr3 };
            int countAddressIsNotNull = 0;
            for (int i = 0; i < listAddress.Length; i++)
            {
                if (listAddress[i] != null && !listAddress[i].ToString().IsNullOrEmpty())
                {
                    countAddressIsNotNull++;
                    if (countAddressIsNotNull == index)
                    {
                        address = listAddress[i].ToString();
                        break;
                    }
                }
            }
            return address;
        }

        protected void DoDataBindControls(Enum type, object sender)
        {
            Repeater grid = (Repeater)sender;
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams();
            parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
            parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
            switch ((DataBindAction)type)
            {
                case DataBindAction.BindDeposits:
                    {
                        parameters.Add("@ReportType", 4, DbType.Int32);
                        DataSetDeposits = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        if (DataSetDeposits.Tables.Count > 0)
                        {
                            grid.DataSource = DataSetDeposits.Tables[0];
                            if (DataSetDeposits.Tables[0] != null && DataSetDeposits.Tables[0].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                        }
                    }
                    break;
               


                case DataBindAction.BindPlanSummary:
                    {
                        parameters.Add("@ReportType", 3, DbType.Int32);
                        DataSetPlanSummary = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        if (DataSetPlanSummary.Tables.Count > 0)
                        {
                            grid.DataSource = DataSetPlanSummary.Tables[0];
                            if (DataSetPlanSummary.Tables[0] != null && DataSetPlanSummary.Tables[0].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                        }
                    }
                    break;

                case DataBindAction.OtherFees:
                    {
                        if (DataSetFees == null)
                        {
                            parameters.Add("@ReportType", 11, DbType.Int32);
                            DataSetFees = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        }
                        if (DataSetFees.Tables.Count > 0)
                        {
                            grid.DataSource = DataSetFees.Tables[0];
                            if (DataSetFees.Tables[0] != null && DataSetFees.Tables[0].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                        }
                        break;
                    }
                case DataBindAction.Total:
                    break;              
            }
        }

        protected DataTable GetTotalData(Enum type)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams();
            parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
            parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
            //Get Total
            switch ((ReportType)type)
            {
                case ReportType.Deposits:
                    parameters.Add("@ReportType", 12, DbType.Int32);
                    break;
               
                case ReportType.PlanSummary:
                    parameters.Add("@ReportType", 4, DbType.Int32);
                    break;
                case ReportType.Fees:
                    parameters.Add("@ReportType", 8, DbType.Int32);
                    break;
            }
            return WebServices.CsReportServices.GetReports(SPA_STATEMENT_DETAIL, parameters);
        }

        protected void uxDeposits_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer && DataSetDeposits != null && DataSetDeposits.Tables.Count > 1 && DataSetDeposits.Tables[1].Rows.Count > 0)
            {
                DataTable data = DataSetDeposits.Tables[1];

                ((Literal)e.Item.FindControl("uxLtrDeposit_FeePaid")).Text =
                   VeraCodeSolution.DoVeraCode(
                       FormatData.FormatCurrency(data.Rows[0]["FeePaid"], SessionManager.CurrencyFortmat));
                ((Literal)e.Item.FindControl("uxLtrDeposit_NetDeposit")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat));               
            }

        }     
        protected void uxPlanSummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Footer && DataSetPlanSummary != null && DataSetPlanSummary.Tables.Count > 1 && DataSetPlanSummary.Tables[1].Rows.Count > 0)
            {
                DataTable data = DataSetPlanSummary.Tables[1];
                ((Literal)e.Item.FindControl("uxLtrPlan_SaleCount")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatInteger(data.Rows[0]["SalesCount"]));
                ((Literal)e.Item.FindControl("uxLtrPlan_SaleAmount")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["SalesAmount"], SessionManager.CurrencyFortmat));
                ((Literal)e.Item.FindControl("uxLtrPlan_CreditCount")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatInteger(data.Rows[0]["CreditsCount"]));
                ((Literal)e.Item.FindControl("uxLtrPlan_CreditAmount")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["CreditsAmount"], SessionManager.CurrencyFortmat));
                ((Literal)e.Item.FindControl("uxLtrPlan_NetAmount")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["NetAmount"], SessionManager.CurrencyFortmat));
                ((Literal)e.Item.FindControl("uxLtrPlan_AvgTkt")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["AverageTicket"], SessionManager.CurrencyFortmat));
                ((Literal)e.Item.FindControl("uxLtrPlan_DiscountDue")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["DiscountDue"], SessionManager.CurrencyFortmat));
                ((Literal)e.Item.FindControl("Literal66")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["TransactionCount"], SessionManager.CurrencyFortmat));

                TotalDiscountDue = double.Parse(data.Rows[0]["DiscountDue"].ToString());
            }

        }        
        protected void uxOtherFees_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer && DataSetFees != null && DataSetFees.Tables.Count > 0 && DataSetFees.Tables[1].Rows.Count > 0)
            {
                DataTable data = DataSetFees.Tables[1];
                ((Literal)e.Item.FindControl("txtFeeTotalFees")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["Total_ODT"], SessionManager.CurrencyFortmat));

           }           
        }


        #endregion Protected Methods

        #region get SPAs

        private DataTable GetMerchantInfomation()
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams();
            parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
            parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
            parameters.Add("@ReportType", 1, DbType.Int32);
            if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA)
                || IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA_MS))
            {
                parameters.Add(new FilterParameter("@IsViewFull", true, DbType.Boolean));
                parameters.AddDecryptDataParams("DDANumber");
            }
            return WebServices.CsReportServices.GetReports(SPA_STATEMENT_DETAIL, parameters);
        }

        private DataTable GetStatementTotal()
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams();
            parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
            parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
            parameters.Add("@ReportType", 12, DbType.Int32);
            return WebServices.CsReportServices.GetReports(SPA_STATEMENT_DETAIL, parameters);
        }

        private string GetMessage()
        {
            string message = string.Empty;

            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams();
            parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
            parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
            parameters.Add("@ReportType", 2, DbType.Int32);
            DataTable dtMessage = WebServices.CsReportServices.GetReports(SPA_STATEMENT_DETAIL, parameters);
            if (dtMessage != null && dtMessage.Rows.Count > 0 && dtMessage.Rows.Count < 7)
            {
                message = dtMessage.Rows[0]["Message"].ToString().ToUpper() +
                            dtMessage.Rows[0]["BANK_MSG1"].ToString().ToUpper() +
                             dtMessage.Rows[0]["BANK_MSG2"].ToString().ToUpper() +
                              dtMessage.Rows[0]["BANK_MSG3"].ToString().ToUpper() +
                               dtMessage.Rows[0]["BANK_MSG4"].ToString().ToUpper() +
                                dtMessage.Rows[0]["BANK_MSG5"].ToString().ToUpper() +
                                 dtMessage.Rows[0]["BANK_MSG6"].ToString().ToUpper();
            }
            return message;
        }

        private void BindStatementTotal()
        {
            // Statement Total
            DataTable tblStmTotal = GetStatementTotal();
            if (tblStmTotal != null && tblStmTotal.Rows.Count > 0)
            {
                txtFeeDISCOUNTDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["ProcessFees"]));
                txtFeeMINDISCOUNTDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["AuthFees"]));
                uxLtrNetDiscountDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["OtherFees"]));
                uxLtrFeesDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["Total_ProcessFees"]));

            }
            else
            {
                txtFeeDISCOUNTDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                txtFeeMINDISCOUNTDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                uxLtrNetDiscountDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                uxLtrFeesDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
            }
        }

        private void BindMessage()
        {
            uxMessage.Text = VeraCodeSolution.GetOutputHtmlString(GetMessage());
        }

        #endregion get SPAs

        private string BuildContentForPDFExport()
        {
            string result;
            this.IsNoCache = false;
            ProcessQueryString();
            BindData(true);

            StringWriter sw = new StringWriter();
            HtmlTextWriter textWriter = new HtmlTextWriter(sw);           

            uxContentExport.RenderControl(textWriter);

            result = string.Format("<!DOCTYPE html><html lang='en'><body>{0}</body></html>", sw.ToString());
            sw.Close();

            return result;
        }

        private void ExportPDF()
        {
            PdfFactory pdfCreator = new PdfFactory();
            pdfCreator.CreateContent += new PdfFactory.PdfFactoryHanlder(pdfCreator_CreateContent);
            pdfCreator.Create(Response.OutputStream);

            Response.AppendHeader("content-disposition", "attachment; filename=" + FILE_NAME_EXPORT + ".pdf");
            Response.ContentType = "application/pdf";
            Response.Flush();
            Response.End();
        }

        private void pdfCreator_CreateContent(PdfFactory.PdfAgent agent, XRect rect)
        {
            agent.AddBrowserHtml(BuildContentForPDFExport());
        }

        private void BindData(bool isExportPdf)
        {
            BindMerchantData(isExportPdf);
            BindMessage();
            DoDataBindControls(DataBindAction.BindPlanSummary, uxPlanSummary);
            DoDataBindControls(DataBindAction.BindDeposits, uxDeposit);           
            DoDataBindControls(DataBindAction.OtherFees, uxOtherFees);
            BindStatementTotal();
        }

        #endregion Methods


    }
}