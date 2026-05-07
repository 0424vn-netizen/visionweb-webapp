using AS.Common.DataProtection;
using AS.Common.DBManager;
using AS.LoneStar.Client.StatementApi;
using AS.LoneStar.Client.StatementApi.AccessOneApi;
using AS.Web.Mobile.Business.Interfaces;
using AS.Web.Mobile.Common;
using AS.Web.Mobile.Common.Helpers;
using AS.Web.Mobile.Common.Models;
using AS.Web.Mobile.Common.MVC;
using AS.Web.Mobile.Security;
using AS.Web.Mobile.UI.Models;
using AS.WS.Mobile.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace AS.Web.Mobile.UI.Controllers
{
    [MobileAuthorize(PermissionCodes.MBStatementReport)]
    public class StatementController : MobileController
    {
        private readonly IStatementBusiness _business;
        protected IStatementBusiness Business
        {
            get { return _business; }
        }

        public StatementController(IMobileSecurityService mobileSecurity, IStatementBusiness business)
            : base(mobileSecurity)
        {
            this._business = business;

        }

        //
        // GET: /Statement/
        public ActionResult Index()
        {
            var model = LoadData(1);

            ViewBag.PageTitle = MobileResources.Constants.MonthlyStatementPageHeader;

            if (Params.StatementType == (int)StatementReportType.TSYSProcessor)
            {
                return View("TSYSStatementIndex", model);
            }
            if (Params.StatementType == (int)StatementReportType.GlobalProcessor)
            {
                return View("GlobalWRFCStatementIndex", model);
            }
            if (Params.StatementType == (int)StatementReportType.WRFC)
            {
                return View("GlobalWRFCStatementIndex", model);
            }
            if (Params.StatementType == (int)StatementReportType.NoType)
            {
                // In case StatementType = StatementReportType.NoType & have data then get processor of a merchant and apply for the rest                
                return View("GenericStatementIndex", model);
            }
            if (Params.StatementType == (int)StatementReportType.API)
            {
                return View("TSYSStatementIndex", model);
            }

            return View(model);
        }

        /// <summary>
        /// Load more statement
        /// </summary>
        /// <param name="loadCount">Page number</param>
        /// <returns></returns>
        public PartialViewResult LoadMoreStatement(int loadCount)
        {

            var model = LoadData(loadCount);
            if (Params.StatementType == (int)StatementReportType.TSYSProcessor ||
                Params.StatementType == (int)StatementReportType.API)
            {
                return PartialView("_TSYSStatementListView", model);
            }
            else if (Params.StatementType == (int)StatementReportType.NoType)
            {
                return PartialView("_GenericStatementListView", model);
            }
            return PartialView("_GlobalWRFCStatementListView", model);
        }

        /// <summary>
        /// Get statement details
        /// </summary>
        /// <param name="detail">The model</param>
        /// <returns>
        /// Details
        /// </returns>
        public ActionResult Detail(ObjectDetailViewModel detail)
        {
            var ps = Params;
            ps.Day = Global.NumDay;
            ps.IsPaging = false;

            var detailParam = DetailsParams;
            if (detailParam != null)
            {
                ps.ReportDate = detailParam.ReportDate;
                ps.HierarchyFilterValue = detailParam.Mid;
            }

            // Get statement detail
            var statementViewModels = new StatementDetailViewModel()
            {
                Date = ps.ReportDate,
                Mid = detailParam.Mid
            };

            statementViewModels.HeaderModel = new HeaderReportViewModel
            {
                MerchantNumber = detailParam.Mid,
                DateDisplay = ps.ReportDate.ToString(Constants.DATE_FORMAT_FULL_MONTH_AND_YEAR)
            };

            #region GlobalProcessor
            // If Global -> Get Deposit, card, settlement, surcharge, other fees
            if (ps.StatementType == (int)StatementReportType.GlobalProcessor || ps.StatementType == (int)StatementReportType.NoType)
            {
                statementViewModels.StatementReportType = (int)StatementReportType.GlobalProcessor;
                // Get settlement, surcharge, other fees
                decimal amountOtherFees = 0;
                decimal amountSettlementDiscountAmount = 0;
                var statementDetails = _business.GetStatementByReportDate(ps);
                if (statementDetails != null)
                {
                    //statementViewModels.SurchargeSummaryViewModel = new SurchargeSummaryViewModel
                    //{
                    //    Amount = statementDetails.SurchargeSummary.SurchargeAmount
                    //};

                    amountSettlementDiscountAmount = statementDetails.SettlementSummary.SettlementDiscountAmount;

                    amountOtherFees = statementDetails.OtherFeesSummary.OtherFees;
                }

                // Get Deposit
                ps.StatementDetailType = (int)StatementDetailType.Deposit;
                var statementDepositDetails = _business.GetStatementByStatementId(ps);
                if (statementDepositDetails != null)
                {
                    statementViewModels.DepositSummaryViewModel = new DepositSummaryViewModel
                    {
                        Sales = statementDepositDetails.DepositSummary.Sales,
                        SaleCount = statementDepositDetails.DepositSummary.SaleCount,
                        Credits = statementDepositDetails.DepositSummary.Credits,
                        AdjustedTotal = statementDepositDetails.DepositSummary.AdjustedTotal,
                        SubTotal = statementDepositDetails.DepositSummary.SubTotal,
                    };
                }

                //Get Deposit Item Summary
                ps.StatementDetailType = (int)StatementDetailType.DepositItemSummary;
                var depositItemSummary = _business.GetDepositItemByStatementId(ps);
                if (depositItemSummary != null)
                {
                    foreach (var item in depositItemSummary.Items)
                    {
                        statementViewModels.DepositItemSummaryViewModel.Add(new DepositItemSummaryViewModel
                        {
                            CreditAdjCount = item.CreditAdjCount,
                            CreditAdjustAmount = item.CreditAdjustAMount,
                            DebitAdjustAmount = item.DebitAdjustAmount,
                            DebitAdjustCount = item.DebitAdjustCount,
                            ReturnAmount = item.ReturnAmount,
                            ReturnCount = item.ReturnCount,
                            SaleAmount = item.SaleAmount,
                            SaleCount = item.SaleCount
                        });
                    }
                }

                // Get card
                ps.StatementDetailType = (int)StatementDetailType.Card;
                var statementCardDetails = _business.GetStatementByStatementId(ps);
                if (statementCardDetails != null)
                {
                    statementViewModels.CardSummaryViewModel = new CardSummaryViewModel
                    {
                        MasterCard = statementCardDetails.CardSummary.MasterCard,
                        Discover = statementCardDetails.CardSummary.Discover,
                        Visa = statementCardDetails.CardSummary.Visa,
                        AMEX = statementCardDetails.CardSummary.AMEX,
                        Diners = statementCardDetails.CardSummary.Diners,
                        DEBIT = statementCardDetails.CardSummary.DEBIT,
                        Others = statementCardDetails.CardSummary.Others,
                    };
                }

                // Get Settlement
                ps.StatementDetailType = (int)StatementDetailType.SettlementDiscount;
                var statementSettlementDiscountDetails = _business.GetSettlementDetailsByStatementId(ps);
                if (statementSettlementDiscountDetails != null)
                {
                    List<SettlementDetails> settlementDetails = new List<SettlementDetails>();
                    //TK: 42609 - Update Settlement/Discount and Other Fees Tabs on VW Mobile
                    for (int i = 0; i < statementSettlementDiscountDetails.Items.Count; i++)
                    {
                        SettlementDetails settlementDetail = new SettlementDetails();
                        settlementDetail.Description = statementSettlementDiscountDetails.Items[i].Description;
                        settlementDetail.FeeAmount = statementSettlementDiscountDetails.Items[i].FeeAmount;
                        settlementDetail.Amount = statementSettlementDiscountDetails.Items[i].Amount;
                        settlementDetail.Interchange = statementSettlementDiscountDetails.Items[i].Interchange;
                        settlementDetail.Items = statementSettlementDiscountDetails.Items[i].Items;
                        settlementDetails.Add(settlementDetail);
                    }

                    statementViewModels.SettlementSummaryViewModel = new SettlementSummaryViewModel
                    {
                        Amount = amountSettlementDiscountAmount,
                        SettlementDetails = settlementDetails,
                        //TK: 42609 - Update Settlement/Discount and Other Fees Tabs on VW Mobile
                        TotalAmount = statementSettlementDiscountDetails.TotalAmount,
                        TotalItems = statementSettlementDiscountDetails.TotalItems
                    };
                }

                // Get Surcharge
                ps.StatementDetailType = (int)StatementDetailType.Surcharge;
                var statementSurchargeDetails = _business.GetSurchargeDetailsByStatementId(ps);
                if (statementSurchargeDetails != null)
                {
                    List<SurchargeDetails> surchargeDetails = new List<SurchargeDetails>();
                    for (int i = 0; i < statementSurchargeDetails.Items.Count; i++)
                    {
                        SurchargeDetails surchargeDetail = new SurchargeDetails();
                        surchargeDetail.Description = statementSurchargeDetails.Items[i].Description;
                        surchargeDetail.SurchargeAmount = statementSurchargeDetails.Items[i].SurchargeAmount;
                        surchargeDetail.Items = statementSurchargeDetails.Items[i].Items;
                        surchargeDetails.Add(surchargeDetail);
                    }

                    statementViewModels.SurchargeSummaryViewModel = new SurchargeSummaryViewModel
                    {
                        Amount = statementDetails.SurchargeSummary.SurchargeAmount,
                        SurchargeDetails = surchargeDetails,
                        TotalAmount = statementSurchargeDetails.TotalAmount,
                        TotalItems = statementSurchargeDetails.TotalItems
                    };
                }



                // Get OtherFees
                ps.StatementDetailType = (int)StatementDetailType.OtherFees;
                var statementOtherFeesDiscountDetails = _business.GetOtherFeesDetailsByStatementId(ps);
                if (statementOtherFeesDiscountDetails != null)
                {
                    List<OtherFeesDetails> otherFeesDetails = new List<OtherFeesDetails>();
                    //TK: 42609 - Update Settlement/Discount and Other Fees Tabs on VW Mobile
                    List<string> cardTypeCodes = statementOtherFeesDiscountDetails.Items.GroupBy(g => g.CardTypeCode).Select(s => s.First()).Select(i => i.CardTypeCode).ToList();

                    for (int i = 0; i < statementOtherFeesDiscountDetails.Items.Count; i++)
                    {
                        OtherFeesDetails otherFeesDetail = new OtherFeesDetails();
                        otherFeesDetail.CardTypeCode = statementOtherFeesDiscountDetails.Items[i].CardTypeCode;
                        otherFeesDetail.ShortDescription = statementOtherFeesDiscountDetails.Items[i].ShortDescription;
                        otherFeesDetail.Amount = statementOtherFeesDiscountDetails.Items[i].Amount;
                        otherFeesDetail.Tickets = statementOtherFeesDiscountDetails.Items[i].Tickets;
                        otherFeesDetails.Add(otherFeesDetail);
                    }

                    statementViewModels.OtherFeesSummaryViewModel = new OtherFeesSummaryViewModel
                    {
                        Amount = amountOtherFees,
                        OtherFeesDetails = otherFeesDetails,
                        //TK: 42609 - Update Settlement/Discount and Other Fees Tabs on VW Mobile
                        TotalAmount = statementOtherFeesDiscountDetails.TotalAmount,
                        CardTypeCodes = cardTypeCodes
                    };
                }

                // Get More
                ps.StatementDetailType = (int)StatementDetailType.More;
                var statementMoreDetails = _business.GetStatementByStatementId(ps);
                if (statementMoreDetails != null)
                {
                    statementViewModels.MoreSummaryViewModel = new MoreSummaryViewModel
                    {
                        TotalAmount = statementMoreDetails.MoreSummary.TotalAmount,
                        MinBillAdjustment = statementMoreDetails.MoreSummary.MinBillAdjustment
                    };
                }

                //Get MonthlyMessages
                ps.StatementDetailType = (int)StatementDetailType.MonthlyMessages;
                var monthlyMessages = _business.GetMonthlyMessages(ps);
                statementViewModels.MonthlyMessagesViewModel = new MonthlyMessagesViewModel();
                if (monthlyMessages.Messages != null)
                {
                    List<MonthlyMessagesItemViewModel> monthlyMsg = new List<MonthlyMessagesItemViewModel>();
                    foreach (var item in monthlyMessages.Messages)
                    {
                        var msg = new MonthlyMessagesItemViewModel();
                        msg.Messages = item.Messages;

                        monthlyMsg.Add(msg);
                    }

                    statementViewModels.MonthlyMessagesViewModel.Messages = monthlyMsg;
                }
            }
            #endregion

            #region WRFC
            // If WRFC -> Get Plan, Deposit, Adjuested, chargeback, fees
            if (ps.StatementType == (int)StatementReportType.WRFC)
            {
                statementViewModels.StatementReportType = (int)StatementReportType.WRFC;
                // Get Fee
                var statementWrfcFeesDetails = _business.GetStatementByReportDate(ps);
                if (statementWrfcFeesDetails != null)
                {
                    statementViewModels.WRFCFeesSummaryViewModel = new WRFCFeesSummaryViewModel
                    {
                        Amount = statementWrfcFeesDetails.WRFCFeesSummary.OtherFees,
                    };
                }

                // Get Plan
                ps.StatementDetailType = (int)StatementDetailType.Plan;
                var statementPlanDetails = _business.GetStatementByStatementId(ps);
                if (statementPlanDetails != null)
                {
                    statementViewModels.PlanSummaryViewModel = new PlanSummaryViewModel
                    {
                        Sales = statementPlanDetails.PlanSummary.Sales,
                        SaleCount = statementPlanDetails.PlanSummary.SaleCount,
                        Credits = statementPlanDetails.PlanSummary.Credits,
                        CreditCount = statementPlanDetails.PlanSummary.CreditCount,
                        Net = statementPlanDetails.PlanSummary.NetSales,
                    };
                }

                // Get DepositWRFC Summary
                ps.StatementDetailType = (int)StatementDetailType.Deposit;
                var statementDepositDetails = _business.GetStatementByStatementId(ps);
                if (statementDepositDetails != null)
                {
                    statementViewModels.DepositWRFCSummaryViewModel = new DepositWRFCSummaryViewModel
                    {
                        Sales = statementDepositDetails.DepositWRFCSummary.Sales,
                        SaleCount = statementDepositDetails.DepositWRFCSummary.SaleCount,
                        Credits = statementDepositDetails.DepositWRFCSummary.Credits,
                        Net = statementDepositDetails.DepositWRFCSummary.NetSales,
                    };
                }

                // Get Chargeback
                ps.StatementDetailType = (int)StatementDetailType.Chargeback;
                var statementChargebackDetails = _business.GetStatementByStatementId(ps);
                if (statementChargebackDetails != null)
                {
                    statementViewModels.ChargebackSummaryViewModel = new ChargebackSummaryViewModel
                    {
                        Sales = statementChargebackDetails.ChargebackSummary.Sales,
                        SaleCount = statementChargebackDetails.ChargebackSummary.SaleCount,
                        Credits = statementChargebackDetails.ChargebackSummary.Credits,
                        Net = statementChargebackDetails.ChargebackSummary.NetSales,
                    };
                }

                // Get Adjustment
                ps.StatementDetailType = (int)StatementDetailType.Adjustment;
                var statementAdjustmentDetails = _business.GetStatementByStatementId(ps);
                if (statementAdjustmentDetails != null)
                {
                    statementViewModels.AdjustmentWRFCSummaryViewModel = new AdjustmentWRFCSummaryViewModel
                    {
                        Sales = statementAdjustmentDetails.AdjustmentWRFCSummary.Sales,
                        SaleCount = statementAdjustmentDetails.AdjustmentWRFCSummary.SaleCount,
                        Credits = statementAdjustmentDetails.AdjustmentWRFCSummary.Credits,
                        Net = statementAdjustmentDetails.AdjustmentWRFCSummary.NetSales,
                    };
                }

                // Get More
                ps.StatementDetailType = (int)StatementDetailType.More;
                var statementMoreDetails = _business.GetStatementTotalByStatementId(ps);
                if (statementMoreDetails != null)
                {
                    statementViewModels.MoreWRFCSummaryViewModel = new MoreWRFCSummaryViewModel
                    {
                        DiscountPaid = statementMoreDetails.MoreWRFCSummary.DiscountPaid,
                        NetDiscountDue = statementMoreDetails.MoreWRFCSummary.NetDiscountDue,
                        FeesDue = statementMoreDetails.MoreWRFCSummary.FeesDue,
                        FeePaid = statementMoreDetails.MoreWRFCSummary.FeePaid,
                        NetFeesDue = statementMoreDetails.MoreWRFCSummary.NetFeesDue,
                        AmountDeducted = statementMoreDetails.MoreWRFCSummary.AmountDeducted
                    };
                }
            }
            #endregion

            return View("GlobalWRFCStatementDetail", statementViewModels);
        }

        /// <summary>
        /// Download fis statement
        /// </summary>
        /// <returns>The file</returns>
        public ActionResult GetFileDownloadStatement()
        {
            var queryString = EncryptedQueryString;
            if (queryString != null)
            {
                var historicalStatement = queryString["historicalStatement"];
                var docId = queryString["docId"];
                var docName = queryString["fileName"];
                var reportTickDate = queryString["date"];
                var mid = queryString["mid"];
                var reportDate = !string.IsNullOrWhiteSpace(reportTickDate) ? new DateTime(long.Parse(reportTickDate)) : DateTime.MinValue;
                var fileNameDefault = string.Format("Statement_{0}_{1}-{2}.pdf", mid, reportDate.Month, reportDate.Year);
                var bEProcessor = queryString["BEProcessor"];
                byte[] responseFile = null;

                if (GetEnabledStatementAPI())
                {
                    bool isConvertion = false;
                    var checkData = _business.CheckMerchantIsConvertion(Params, mid);
                    if (checkData != null && checkData.Rows.Count > 0)
                    {
                        isConvertion = bool.Parse(checkData.Rows[0]["IsConvertion"].ToString());
                    }
                    if (!isConvertion)
                    {
                        if (!string.IsNullOrWhiteSpace(docId))
                        {
                            var fileName = !string.IsNullOrWhiteSpace(docName) ? docName : fileNameDefault;
                            responseFile = DocServerHelper.DownloadDocument(long.Parse(docId));

                            if (responseFile == null)
                            {
                                return RedirectToNotFound();
                            }
                            return File(responseFile, MediaTypeNames.Application.Pdf, fileName);
                        }
                    }
                    else
                    {
                        StatementApi statementApi = new StatementApi();
                        if (string.IsNullOrEmpty(docId))
                        {
                            string merchantNum = mid;
                            if (CheckMerchantBelongToUser(merchantNum))
                            {
                                var items = GetOmahaMerchantStatementList(merchantNum);
                                if (items != null && items.Any())
                                {
                                    MerchantStatementItem item = items[0];
                                    DateTime reportDateApi = ParseDateTime(item.StatementDate);
                                    if (reportDateApi != DateTime.MinValue)
                                    {
                                        return DownloadStatementApi(merchantNum, reportDate, item.StatementId, fileNameDefault);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (bEProcessor == BEProcessor.FD.ToString())
                            {
                                string realMerchant = GetRealMerchantNum(mid);
                                return DownloadStatementApi(realMerchant, reportDate, docId, fileNameDefault);
                            }
                            else
                            {
                                var fileName = !string.IsNullOrWhiteSpace(docName) ? docName : fileNameDefault;
                                responseFile = DocServerHelper.DownloadDocument(long.Parse(docId));

                                if (responseFile == null)
                                {
                                    return RedirectToNotFound();
                                }
                                return File(responseFile, MediaTypeNames.Application.Pdf, fileName);
                            }
                        }
                    }
                }
                else
                {
                    // Site jump to full site if historicalStatement
                    if (historicalStatement.ToLower() == "true")
                    {
                        var ps = Params;
                        ps.HierarchyFilterValue = mid;

                        // Create jumpsite ticket
                        string k = MobileSecurity.CreateJumpSiteTicket(new UserSecurityModel()
                        {
                            ClientId = ps.ASClientID,
                            RecId = ps.RecId,
                        });

                        string msGateUrl = Global.MSGATEURL;

                        if (k != null)
                        {
                            var userIdEncrypt = HttpUtility.UrlEncode(Encrypt(ps.UserID));
                            var recIdEncrypt = HttpUtility.UrlEncode(Encrypt(ps.RecId.ToString()));
                            var clientIdEncrypt = HttpUtility.UrlEncode(AS.Common.DataProtection.Cryptophy.EncryptText(ps.ASClientID.ToString()));
                            msGateUrl += (string.Format("?u={0}&j={1}&k={2}&f={3}&c={4}", userIdEncrypt, recIdEncrypt, k, ps.UserID, clientIdEncrypt));

                            var request = new MobileWebClient();
                            ServicePointManager.Expect100Continue = false;
                            ServicePointManager.ServerCertificateValidationCallback =
                            delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                            {
                                return true;
                            };
                            request.DownloadData(msGateUrl);

                            var msMobileStatementUrl = Global.MS_STATEMENT_FOR_MOBILE + string.Format("?merchantNumber={0}&reportDate={1}&hierarchyValue={2}", mid, reportTickDate, ps.HierarchyFilterValue);
                            responseFile = request.DownloadData(msMobileStatementUrl);
                            return File(responseFile, MediaTypeNames.Application.Pdf, fileNameDefault);
                        }
                        return RedirectToNotFound();
                    }
                    else if (!string.IsNullOrWhiteSpace(docId))
                    {
                        var fileName = !string.IsNullOrWhiteSpace(docName) ? docName : fileNameDefault;
                        responseFile = DocServerHelper.DownloadDocument(long.Parse(docId));

                        if (responseFile == null)
                        {
                            return RedirectToNotFound();
                        }

                        return File(responseFile, MediaTypeNames.Application.Pdf, fileName);
                    }
                }
            }

            return RedirectToNotFound();
        }
        ///
        private bool CheckProcessorHasDownload(string beProcessor)
        {
            bool result = false;
            beProcessor = string.IsNullOrEmpty(beProcessor) ? string.Empty : beProcessor.ToLower();
            string[] processors = User.Processors.Split(';').Select(t => t.ToLower()).ToArray();
            result = processors.Contains(beProcessor);

            return result;

        }

        private PagingViewModel<MonthlyStatementViewModels> LoadData(int loadCount)
        {
            var lstMonthlyStatement = new List<MonthlyStatementViewModel>();
            var ps = Params;
            ps.PageNo = loadCount;
            ps.PageSize = Global.PageSize;
            ps.IsPaging = true;
            bool isChainHQ = User.UserData.IsChainHQ;
            bool hasMultipleMerchant = (string.IsNullOrWhiteSpace(ps.HierarchyFilterValue) && User.UserData.UserMobileType != UserTypes.Merchant) && isChainHQ;

            ps.HierarchyFilterValue = !isChainHQ && string.IsNullOrEmpty(SelectedMerchant) ? User.UserData.EntityID : SelectedMerchant;
            if (GetEnabledStatementAPI())
            {
                if (hasMultipleMerchant || !EnableStatementApi(ps.HierarchyFilterValue))
                {
                    var monthlyStatements = _business.GetStatementByMonths(ps);

                    foreach (var item in monthlyStatements.Items)
                    {
                        bool _hasDownload = true;

                        var detailAction = "GetFileDownloadStatement";

                        var queryString = BuildEncryptedQueryString(new
                        {
                            mid = item.Mid,
                            date = item.ReportDate.Ticks,
                            docId = item.DocID,
                            fileName = item.FileName,
                            historicalStatement = item.HistoricalStatement,
                            BEProcessor = item.BackEndProcessor.ToLower().Trim().Equals("omaha") ? BEProcessor.FD.ToString() : BEProcessor.TSYS.ToString()
                        });
                        lstMonthlyStatement.Add(new MonthlyStatementViewModel
                        {
                            hasDownload = _hasDownload,
                            Date = item.ReportDate,
                            FileIndex = item.FileIndex,
                            Mid = item.Mid,
                            NetDeposit = item.NetDeposit,
                            DetailUrl = string.Format("{0}?{1}", Url.Action(detailAction), queryString),
                            BEProcessor = item.BackEndProcessor.ToLower().Trim().Equals("omaha") ? BEProcessor.FD.ToString() : BEProcessor.TSYS.ToString(),
                            MerchantName = string.Format("{0} - {1}{2}", item.ReportDate.ToString("MM/dd/yyyy"), item.BackEndProcessor.ToLower().Trim().Equals("omaha") ? BEProcessor.FD.ToString() : BEProcessor.TSYS.ToString(),
                            item.FileIndex != 0 ? string.Format(" - {0}", item.FileIndex) : string.Empty)
                        });
                    }
                }
                else
                {
                    lstMonthlyStatement = BindStatementApi(ps);
                }
            }
            else
            {
                var monthlyStatements = _business.GetStatementByMonths(ps);

                foreach (var item in monthlyStatements.Items)
                {
                    bool _hasDownload = Params.StatementType == (int)StatementReportType.TSYSProcessor ||
                                (Params.StatementType == (int)StatementReportType.NoType && CheckProcessorHasDownload(item.BackEndProcessor));

                    var detailAction = _hasDownload ? "GetFileDownloadStatement" : "Detail";

                    var queryString = BuildEncryptedQueryString(new
                    {
                        mid = item.Mid,
                        date = item.ReportDate.Ticks,
                        docId = item.DocID,
                        fileName = item.FileName,
                        historicalStatement = item.HistoricalStatement,
                        BEProcessor = item.BackEndProcessor.ToLower().Trim().Equals("omaha") ? BEProcessor.FD.ToString() : BEProcessor.TSYS.ToString()
                    });
                    lstMonthlyStatement.Add(new MonthlyStatementViewModel
                    {
                        hasDownload = _hasDownload,
                        Date = item.ReportDate,
                        FileIndex = item.FileIndex,
                        Mid = item.Mid,
                        NetDeposit = item.NetDeposit,
                        DetailUrl = string.Format("{0}?{1}", Url.Action(detailAction), queryString),
                        BEProcessor = item.BackEndProcessor.ToLower().Trim().Equals("omaha") ? BEProcessor.FD.ToString() : BEProcessor.TSYS.ToString()
                    });

                }
            }

            return new PagingViewModel<MonthlyStatementViewModels>()
            {
                Items = new List<MonthlyStatementViewModels>
                {
                    new MonthlyStatementViewModels
                    {
                        hasMultipleMerchant = hasMultipleMerchant,
                        ListMonthlyStatementViewModels = lstMonthlyStatement
                    }
                }
            };
        }

        private DateTime ParseDateTime(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr)) return DateTime.MinValue;

            DateTime date = DateTime.MinValue;
            DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return date;
        }

        private string GetRealMerchantNum(string currMerchant)
        {
            DataTable merchant = _business.CheckMerchantIsConvertion(Params, currMerchant);
            if (merchant != null && merchant.Rows.Count > 0)
            {
                string realMerchant = merchant.Rows[0]["FDR_MID"].ToString();
                return string.IsNullOrEmpty(realMerchant) ? currMerchant : realMerchant;
            }

            return currMerchant;
        }

        //  FD to TSYS Merchant Migration
        private List<MonthlyStatementViewModel> BindStatementApi(AS.WS.Mobile.Domain.Models.MobileParameters ps)
        {
            var lstMonthlyStatement = new List<MonthlyStatementViewModel>();
            string merchantNumber = ps.HierarchyFilterValue;
            int pageNo = ps.PageNo;
            int pageSize = ps.PageSize;
            if (CheckMerchantBelongToUser(merchantNumber))
            {
                //Get statement list with order
                var items = GetOmahaMerchantStatementList(merchantNumber);
                bool isLastStatement = CheckHasLastStatementTSYS(merchantNumber);
                if (isLastStatement)
                {
                    foreach (var item in items)
                    {
                        bool _hasDownload = true;
                        var detailAction = "GetFileDownloadStatement";
                        var queryString = BuildEncryptedQueryString(new
                        {
                            mid = item.MerchantNumber,
                            date = DateTime.Parse(item.StatementDate).Ticks,
                            docId = item.StatementId,
                            fileName = string.Empty,
                            BEProcessor = BEProcessor.FD.ToString()
                        });
                        lstMonthlyStatement.Add(new MonthlyStatementViewModel
                        {
                            hasDownload = _hasDownload,
                            Date = DateTime.Parse(item.StatementDate),
                            Mid = item.MerchantNumber,
                            DetailUrl = string.Format("{0}?{1}", Url.Action(detailAction), queryString),
                            BEProcessor = BEProcessor.FD.ToString(),
                            MerchantName = string.Format("{0} - {1}", DateTime.Parse(item.StatementDate).ToString("MM/yyyy"), BEProcessor.FD.ToString())
                        });
                    }

                    var monthlyStatements = new AS.WS.Mobile.Domain.Models.Statements();
                    for (int i = 1; i <= pageNo; i++)
                    {
                        ps.PageNo = i;
                        var getmonthlyStatement = _business.GetStatementByMonths(ps);
                        if (getmonthlyStatement.Items.Count() > 0)
                        {
                            monthlyStatements.Items.AddRange(getmonthlyStatement.Items);
                        }
                        else
                        {
                            break;
                        }
                    }
                    foreach (var item in monthlyStatements.Items)
                    {
                        bool _hasDownload = true;

                        var detailAction = "GetFileDownloadStatement";

                        var queryString = BuildEncryptedQueryString(new
                        {
                            mid = item.Mid,
                            date = item.ReportDate.Ticks,
                            docId = item.DocID,
                            fileName = item.FileName,
                            historicalStatement = item.HistoricalStatement,
                            BEProcessor = item.BackEndProcessor.ToLower().Trim().Equals("omaha") ? BEProcessor.FD.ToString() : BEProcessor.TSYS.ToString()
                        });
                        lstMonthlyStatement.Add(new MonthlyStatementViewModel
                        {
                            hasDownload = _hasDownload,
                            Date = item.ReportDate,
                            FileIndex = item.FileIndex,
                            Mid = item.Mid,
                            NetDeposit = item.NetDeposit,
                            DetailUrl = string.Format("{0}?{1}", Url.Action(detailAction), queryString),
                            BEProcessor = item.BackEndProcessor.ToLower().Trim().Equals("omaha") ? BEProcessor.FD.ToString() : BEProcessor.TSYS.ToString(),
                            MerchantName = string.Format("{0} - {1}{2}", item.ReportDate.ToString("MM/yyyy"), item.BackEndProcessor.ToLower().Trim().Equals("omaha") ? BEProcessor.FD.ToString() : BEProcessor.TSYS.ToString(),
                                      item.FileIndex != 0 ? string.Format(" - {0}", item.FileIndex) : string.Empty)
                        });

                    }

                    if (lstMonthlyStatement != null && lstMonthlyStatement.Any())
                    {
                        //43842: Bug #38060 - Sort order is incorrectly in Statements page
                        lstMonthlyStatement = lstMonthlyStatement.OrderByDescending(o => o.Date)
                            .ThenBy(o => o.BEProcessor).ToList();
                    }
                }
                else
                {
                    if (items != null && items.Any())
                    {
                        foreach (var item in items)
                        {
                            bool _hasDownload = true;
                            var detailAction = "GetFileDownloadStatement";
                            var queryString = BuildEncryptedQueryString(new
                            {
                                mid = item.MerchantNumber,
                                date = DateTime.Parse(item.StatementDate).Ticks,
                                docId = item.StatementId,
                                fileName = string.Empty,
                                BEProcessor = BEProcessor.FD.ToString()
                            });
                            lstMonthlyStatement.Add(new MonthlyStatementViewModel
                            {
                                hasDownload = _hasDownload,
                                Date = DateTime.Parse(item.StatementDate),
                                Mid = item.MerchantNumber,
                                DetailUrl = string.Format("{0}?{1}", Url.Action(detailAction), queryString),
                                BEProcessor = BEProcessor.FD.ToString(),
                                MerchantName = string.Format("{0} - {1}", DateTime.Parse(item.StatementDate).ToString("MM/yyyy"), BEProcessor.FD.ToString())
                            });
                        }
                    }
                }
            }
            lstMonthlyStatement = lstMonthlyStatement.Take(pageNo * pageSize).ToList();
            lstMonthlyStatement.RemoveRange(0, (pageNo - 1) * pageSize);
            return lstMonthlyStatement;
        }

        private bool CheckHasLastStatementTSYS(string currMerchant)
        {
            DataTable merchant = _business.CheckMerchantIsConvertion(Params, currMerchant);
            if (merchant != null && merchant.Rows.Count > 0)
            {
                return !string.IsNullOrEmpty(merchant.Rows[0]["LastStatementReportDate"].ToString()) &&
                    !string.IsNullOrEmpty(merchant.Rows[0]["STMT_DocIDList"].ToString());
            }

            return false;
        }

        private bool CheckMerchantBelongToUser(string merchantNumber)
        {
            return _business.CheckMerchantBelongToUser(Params, merchantNumber);
        }

        private List<MerchantStatementItem> GetOmahaMerchantStatementList(string merchantNumber)
        {
            //43842 - FD to TSYS Merchant Migration
            string realMerchant = GetRealMerchantNum(merchantNumber);

            StatementApi statementApi = new StatementApi();
            var items = statementApi.GetOmahaMerchantStatementList(realMerchant);

            if (items != null && items.Any())
            {
                return items.OrderByDescending(o => ParseDateTime(o.StatementDate)).ToList();
            }
            return null;
        }

        private bool EnableStatementApi(string merchantNumber)
        {
            if (GetEnabledStatementAPI())
            {
                var checkData = _business.CheckMerchantIsConvertion(Params, merchantNumber);
                if (checkData != null && checkData.Rows.Count > 0)
                {
                    return bool.Parse(checkData.Rows[0]["IsConvertion"].ToString());
                }
            }
            return false;
        }

        private bool GetEnabledStatementAPI()
        {
            var statApi = SettingHelper.GetDataOfExtendedSetting("EnableStatementAPI");
            return !string.IsNullOrEmpty(statApi) && statApi.ToLower() == "true";
        }

        private FileContentResult DownloadStatementApi(string realMerchantNumber, DateTime reportDate, string statementId, string fileName)
        {
            StatementApi statementApi = new StatementApi();
            MerchantStatement statement = statementApi.GetOmahaMerchantStatement(realMerchantNumber, statementId);
            byte[] bytes = Convert.FromBase64String(statement.StatementData);
            return File(bytes, MediaTypeNames.Application.Pdf, fileName);
        }
    }
}