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
    public partial class StatementDetail_WRFC : ReportPage
    {
        #region Enum

        enum DataBindAction
        {
            BindPlanSummary,
            BindDeposits,
            BindChargebacks,
            BindFees,
            Adjustments,
            BWHDetail,
            BWHMonthly,
            BWHOverall,
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
            Literal37.Text = Literal37.Text.ToCurrencySymbol();
            Literal38.Text = Literal38.Text.ToCurrencySymbol();
            Literal39.Text = Literal39.Text.ToCurrencySymbol();
            Literal48.Text = Literal48.Text.ToCurrencySymbol();
            Literal49.Text = Literal49.Text.ToCurrencySymbol();


        }

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
            string amountDeducted = string.Empty;
            string clientInfo = string.Empty;
            if (tbl != null && tbl.Rows.Count > 0)
            {
                // Bind AmountDeducted to the "AMOUNT DEDUCTED FROM ACCOUNT"
                amountDeducted = VeraCodeSolution.DoVeraCode(
                    FormatCurrency(tbl.Rows[0]["AmountDeducted"]));

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
                uxAmountDeducted.Text = amountDeducted;
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
            pnlBreakLineExportChargeBack.Visible = true;
            pnlBreakLineExportDeposit.Visible = true;
            pnlBreakLineExportFees.Visible = true;
            pnlBreakLineExportPlanSummary.Visible = true;
            uxExportFooter.Visible = true;
            ProcessQueryString();
            DisplayLogo(true);
            BindMerchantData(false);
            BindMessage();
            DoDataBindControls(DataBindAction.BindPlanSummary, uxPlanSummary);
            DoDataBindControls(DataBindAction.BindDeposits, uxDeposit);
            DoDataBindControls(DataBindAction.BindChargebacks, uxChargebacks);
            DoDataBindControls(DataBindAction.Adjustments, uxAdjustments);

            DoDataBindControls(DataBindAction.BWHDetail, uxBackupwithholdingDetail);
            DoDataBindControls(DataBindAction.BWHMonthly, uxBackupwithholdingMonthly);
            DoDataBindControls(DataBindAction.BWHOverall, uxBackupwithholdingOverall);
            DoDataBindControls(DataBindAction.ReserveFund, uxReserveFund);

            DoDataBindControls(DataBindAction.AuthFees, uxAuthFees);
            DoDataBindControls(DataBindAction.InterFees, uxInterchangeFees);
            DoDataBindControls(DataBindAction.TransFees, uxTransactionFees);
            DoDataBindControls(DataBindAction.CardBrandFees, uxCardBrandFees);
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
                case DataBindAction.BindChargebacks:
                    {
                        parameters.Add("@ReportType", 7, DbType.Int32);
                        DataSetChargeback = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        if (DataSetChargeback.Tables.Count > 0)
                        {
                            grid.DataSource = DataSetChargeback.Tables[0];
                            if (DataSetChargeback.Tables[0] != null && DataSetChargeback.Tables[0].Rows.Count > 0)
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
                case DataBindAction.BindFees:
                    parameters.Add("@ReportType", 11, DbType.Int32);
                    break;
                case DataBindAction.Adjustments:
                    {
                        parameters.Add("@ReportType", 5, DbType.Int32);
                        DataSetAdjustments = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        if (DataSetAdjustments.Tables.Count > 0)
                        {
                            grid.DataSource = DataSetAdjustments.Tables[0];
                            if (DataSetAdjustments.Tables[0] != null && DataSetAdjustments.Tables[0].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                        }
                    }
                    break;
                case DataBindAction.BWHDetail:
                    {
                        if (DataSetBWH == null)
                        {
                            parameters.Add("@ReportType", 8, DbType.Int32);
                            DataSetBWH = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        }
                        if (DataSetBWH.Tables.Count > 0)
                        {
                            grid.DataSource = DataSetBWH.Tables[0];
                            if (DataSetBWH.Tables[0] != null && DataSetBWH.Tables[0].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                            else
                            {
                                pnlBACKUPWITHHOLDING.Visible = false;
                            }
                        }
                    }
                    break;
                case DataBindAction.BWHMonthly:
                    {
                        if (DataSetBWH == null)
                        {
                            parameters.Add("@ReportType", 8, DbType.Int32);
                            DataSetBWH = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        }
                        if (DataSetBWH.Tables.Count > 1)
                        {
                            grid.DataSource = DataSetBWH.Tables[1];
                            if (DataSetBWH.Tables[1] != null && DataSetBWH.Tables[1].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                            else
                            {
                                pnlBACKUPWITHHOLDING.Visible = false;
                            }
                        }
                    }
                    break;
                case DataBindAction.BWHOverall:
                    {
                        if (DataSetBWH == null)
                        {
                            parameters.Add("@ReportType", 8, DbType.Int32);
                            DataSetBWH = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        }
                        if (DataSetBWH.Tables.Count > 2)
                        {
                            grid.DataSource = DataSetBWH.Tables[2];
                            if (DataSetBWH.Tables[2] != null && DataSetBWH.Tables[2].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                            else
                            {
                                pnlBACKUPWITHHOLDING.Visible = false;
                            }
                        }
                    }
                    break;
                case DataBindAction.ReserveFund:
                    {
                        parameters.Add("@ReportType", 10, DbType.Int32);
                        DataSetReserveFund = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        if (DataSetReserveFund.Tables.Count > 0)
                        {
                            grid.DataSource = DataSetReserveFund.Tables[0];
                            if (DataSetReserveFund.Tables[0] != null && DataSetReserveFund.Tables[0].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                            else
                            {
                                pnlReserveFund.Visible = false;
                            }
                        }
                    }
                    break;

                case DataBindAction.AuthFees:
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
                    }
                    break;
                case DataBindAction.InterFees:
                    {
                        if (DataSetFees == null)
                        {
                            parameters.Add("@ReportType", 11, DbType.Int32);
                            DataSetFees = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        }
                        if (DataSetFees.Tables.Count > 1)
                        {
                            grid.DataSource = DataSetFees.Tables[1];
                            if (DataSetFees.Tables[1] != null && DataSetFees.Tables[1].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                        }
                    }
                    break;
                case DataBindAction.TransFees:
                    {
                        if (DataSetFees == null)
                        {
                            parameters.Add("@ReportType", 11, DbType.Int32);
                            DataSetFees = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        }
                        if (DataSetFees.Tables.Count > 2)
                        {
                            grid.DataSource = DataSetFees.Tables[2];
                            if (DataSetFees.Tables[2] != null && DataSetFees.Tables[2].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                        }
                    }
                    break;
                case DataBindAction.CardBrandFees:
                    {
                        if (DataSetFees == null)
                        {
                            parameters.Add("@ReportType", 11, DbType.Int32);
                            DataSetFees = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        }
                        if (DataSetFees.Tables.Count > 3)
                        {
                            grid.DataSource = DataSetFees.Tables[3];
                            if (DataSetFees.Tables[3] != null && DataSetFees.Tables[3].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                        }
                    }
                    break;
                case DataBindAction.OtherFees:

                case DataBindAction.Total:
                    {
                        if (DataSetFees == null)
                        {
                            parameters.Add("@ReportType", 11, DbType.Int32);
                            DataSetFees = WebServices.CsReportServices.GetReportsAsDataSet(SPA_STATEMENT_DETAIL, parameters);
                        }
                        if (DataSetFees.Tables.Count > 4)
                        {
                            grid.DataSource = DataSetFees.Tables[4];
                            if (DataSetFees.Tables[4] != null && DataSetFees.Tables[4].Rows.Count > 0)
                            {
                                grid.DataBind();
                            }
                        }
                    }
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
                case ReportType.Adjustments:
                    parameters.Add("@ReportType", 10, DbType.Int32);
                    break;
                case ReportType.Chargebacks:
                    parameters.Add("@ReportType", 13, DbType.Int32);
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
                ((Literal)e.Item.FindControl("uxLtrDeposit_SaleCount")).Text =
                    VeraCodeSolution.ValidateResponseData(
                        FormatData.FormatInteger(data.Rows[0]["SalesCount"]));
                ((Literal)e.Item.FindControl("uxLtrDeposit_SaleAmount")).Text =
                    VeraCodeSolution.ValidateResponseData(
                        FormatData.FormatCurrency(data.Rows[0]["SalesAmount"], SessionManager.CurrencyFortmat));
                ((Literal)e.Item.FindControl("uxLtrDeposit_CreditAmount")).Text =
                    VeraCodeSolution.ValidateResponseData(
                        FormatData.FormatCurrency(data.Rows[0]["CreditAmount"], SessionManager.CurrencyFortmat));
                ((Literal)e.Item.FindControl("uxLtrDeposit_DiscountPD")).Text =
                    VeraCodeSolution.ValidateResponseData(
                        FormatData.FormatCurrency(data.Rows[0]["DiscPaiD"], SessionManager.CurrencyFortmat));
                ((Literal)e.Item.FindControl("uxLtrDeposit_NetDeposit")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat));

                DiscountPaid = double.Parse(data.Rows[0]["DiscPaiD"].ToString());
            }

        }


        protected void uxAdjustments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Footer && DataSetAdjustments != null && DataSetAdjustments.Tables.Count > 1 && DataSetAdjustments.Tables[1].Rows.Count > 0)
            {
                DataTable data = DataSetAdjustments.Tables[1];
                ((Literal)e.Item.FindControl("uxLtrAdjustment_SaleCount")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatInteger(data.Rows[0]["SalesCount"]));
                ((Literal)e.Item.FindControl("uxLtrAdjustment_SaleAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["SalesAmount"], SessionManager.CurrencyFortmat);
                ((Literal)e.Item.FindControl("uxLtrAdjustment_CreditAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["CreditsAmount"], SessionManager.CurrencyFortmat);
                ((Literal)e.Item.FindControl("uxLtrAdjustment_DiscountPD")).Text = FormatData.FormatCurrency(data.Rows[0]["DiscPaid"], SessionManager.CurrencyFortmat);
                ((Literal)e.Item.FindControl("uxLtrAdjustment_NetDeposit")).Text = FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat);
            }

        }


        protected void uxChargebacks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {


            if (e.Item.ItemType == ListItemType.Footer && DataSetChargeback != null && DataSetChargeback.Tables.Count > 1 && DataSetChargeback.Tables[1].Rows.Count > 0)
            {
                DataTable data = DataSetChargeback.Tables[1];
                ((Literal)e.Item.FindControl("uxLtrChargeback_SaleCount")).Text = FormatData.FormatInteger(data.Rows[0]["SalesCount"]);
                ((Literal)e.Item.FindControl("uxLtrChargeback_SaleAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["SalesAmount"], SessionManager.CurrencyFortmat);
                ((Literal)e.Item.FindControl("uxLtrChargeback_CreditAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["CreditsAmount"], SessionManager.CurrencyFortmat);
                ((Literal)e.Item.FindControl("uxLtrChargeback_DiscountPD")).Text = FormatData.FormatCurrency(data.Rows[0]["DiscountPaid"], SessionManager.CurrencyFortmat);
                ((Literal)e.Item.FindControl("uxLtrChargeback_NetDeposit")).Text = FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat);
            }

        }
        protected void uxReserveFund_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Footer && DataSetReserveFund != null && DataSetReserveFund.Tables.Count > 1 && DataSetReserveFund.Tables[1].Rows.Count > 0)
            {
                DataTable data = DataSetReserveFund.Tables[1];
                ((Literal)e.Item.FindControl("uxReserveFund_AmountReserved")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatInteger(data.Rows[0]["AmountReserved"]));
                ((Literal)e.Item.FindControl("uxReserveFund_AmountReleased")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatInteger(data.Rows[0]["AmountRelease"]));
                ((Literal)e.Item.FindControl("uxReserveFund_ReleasedBalance")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatInteger(data.Rows[0]["ReserveBalance"]));

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

                TotalDiscountDue = double.Parse(data.Rows[0]["DiscountDue"].ToString());
            }

        }
        protected void uxAuthFees_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Footer && DataSetFees != null && DataSetFees.Tables.Count > 0 && DataSetFees.Tables[0].Rows.Count > 0)
            {
                DataTable data = DataSetFees.Tables[0];
                ((Literal)e.Item.FindControl("txtFeeTotalFees")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["PPMTotalAuthFees"], SessionManager.CurrencyFortmat));


            }

        }
        protected void uxInterchangeFees_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Footer && DataSetFees != null && DataSetFees.Tables.Count > 1 && DataSetFees.Tables[1].Rows.Count > 0)
            {
                DataTable data = DataSetFees.Tables[1];
                ((Literal)e.Item.FindControl("txtFeeTotalFees")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["PPMTotalInterFees"], SessionManager.CurrencyFortmat));


            }
        }
        protected void uxTransactionFees_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Footer && DataSetFees != null && DataSetFees.Tables.Count > 2 && DataSetFees.Tables[2].Rows.Count > 0)
            {
                DataTable data = DataSetFees.Tables[2];
                ((Literal)e.Item.FindControl("txtFeeTotalFees")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["PPMTotalTransFees"], SessionManager.CurrencyFortmat));


            }
        }
        protected void uxCardBrandFees_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Footer && DataSetFees != null && DataSetFees.Tables.Count > 3 && DataSetFees.Tables[3].Rows.Count > 0)
            {
                DataTable data = DataSetFees.Tables[3];
                ((Literal)e.Item.FindControl("txtFeeTotalFees")).Text =
                    VeraCodeSolution.DoVeraCode(
                        FormatData.FormatCurrency(data.Rows[0]["PPMTotalCBFees"], SessionManager.CurrencyFortmat));


            }
        }
        protected void uxOtherFees_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (DataSetFees != null && DataSetFees.Tables.Count > 4 && DataSetFees.Tables[4].Rows.Count > 0)
                {
                    DataTable data = DataSetFees.Tables[4];
                    ((Literal)e.Item.FindControl("txtFeeTotalFees")).Text =
                        VeraCodeSolution.DoVeraCode(
                            FormatData.FormatCurrency(data.Rows[0]["PPMTotalOtherFees"], SessionManager.CurrencyFortmat));

                }
                if (DataSetFees != null && DataSetFees.Tables.Count > 5 && DataSetFees.Tables[5].Rows.Count > 0)
                {
                    DataTable data = DataSetFees.Tables[5];
                    ((Literal)e.Item.FindControl("uxLtrFeesDueTotal")).Text =
                        VeraCodeSolution.DoVeraCode(
                            FormatData.FormatCurrency(data.Rows[0]["Total"], SessionManager.CurrencyFortmat));

                }




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
            if (dtMessage != null && dtMessage.Rows.Count > 0)
            {
                message = dtMessage.Rows[0]["Message"].ToString();
            }
            return message;
        }


        private void BindStatementTotal()
        {
            // Statement Total
            DataTable tblStmTotal = GetStatementTotal();
            if (tblStmTotal != null && tblStmTotal.Rows.Count > 0)
            {
                txtFeeDISCOUNTDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["DiscDue"]));
                txtFeeMINDISCOUNTDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["MinDiscDue"]));
                uxLtrDiscountPaid.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["DiscPaid"]));
                uxLtrNetDiscountDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["NetDiscDue"]));
                uxLtrFeesDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["FeesDue"]));
                uxLtrFeesPaid.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["FeesPaid"]));
                uxLtrNetFeesDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["NetFeesDue"]));
                uxLtrAmountDeducted.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["AmountDeducted"]));
                txtxFeeNEXTPROCESSINGFEESDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["NetProcessingFeeDue"]));
                txtFeeAmountDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["AmountDue"]));
                txtFeeAMOUNCREDITED.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(tblStmTotal.Rows[0]["AmountCredited"]));
            }
            else
            {
                txtFeeDISCOUNTDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                txtFeeMINDISCOUNTDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                uxLtrDiscountPaid.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                uxLtrNetDiscountDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                uxLtrFeesDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                uxLtrFeesPaid.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                uxLtrNetFeesDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                uxLtrAmountDeducted.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                txtxFeeNEXTPROCESSINGFEESDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                txtFeeAmountDUE.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
                txtFeeAMOUNCREDITED.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(0));
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
            DoDataBindControls(DataBindAction.BindChargebacks, uxChargebacks);
            DoDataBindControls(DataBindAction.Adjustments, uxAdjustments);

            DoDataBindControls(DataBindAction.BWHDetail, uxBackupwithholdingDetail);
            DoDataBindControls(DataBindAction.BWHMonthly, uxBackupwithholdingMonthly);
            DoDataBindControls(DataBindAction.BWHOverall, uxBackupwithholdingOverall);
            DoDataBindControls(DataBindAction.ReserveFund, uxReserveFund);

            DoDataBindControls(DataBindAction.AuthFees, uxAuthFees);
            DoDataBindControls(DataBindAction.InterFees, uxInterchangeFees);
            DoDataBindControls(DataBindAction.TransFees, uxTransactionFees);
            DoDataBindControls(DataBindAction.CardBrandFees, uxCardBrandFees);
            DoDataBindControls(DataBindAction.OtherFees, uxOtherFees);
            BindStatementTotal();
        }

        #endregion Methods


    }
}