using AS.Common.DBManager;
using AS.WS.Mobile.Domain.Models;
using AS.WS.MobileBusiness;
using System;
using System.Configuration;
using System.Data;
using System.Web.Services;
using System.Collections.Generic;

/// <summary>
/// Summary description for MsReportingServices
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[Microsoft.Web.Services3.Policy("ServerPolicy")]

public class MobileServices : AS.Common.WSE.ASWebService
{
    protected MobileService _ReportingBusiness = null;
    private int _SqlCommandTimeout = 0;

    #region Contructor
    public MobileServices()
    {
        if (Microsoft.Web.Services3.ResponseSoapContext.Current == null)
        {
            throw new UnauthorizedAccessException("Access Denied");
        }
        if (Int32.TryParse(ConfigurationManager.AppSettings["SqlCommandTimeout"], out _SqlCommandTimeout))
        {
            ASSqlDatabase.CommandTimeout = _SqlCommandTimeout;
        }
        var mbConnectionString = GeneralFuncsLib.GetConnStringSettings(RequestHeaders["ClientId"], "MB_DBCONN");
        if (string.IsNullOrEmpty(mbConnectionString))
        {
            throw new Exception("Connection string for mobile not found!");
        }
        _ReportingBusiness = new MobileService(mbConnectionString);
    }
    #endregion

    #region Methods

    [WebMethod]
    public string Hello()
    {
        return "Mobile webservice!";
    }

    #region Batches
    [WebMethod]
    public Batches GetBatchesByLastDays(MobileParameters parames)
    {
        return _ReportingBusiness.GetBatchesByLastDays(parames);
    }
    [WebMethod]
    public BatchDetails GetBatchDetail(MobileParameters parames)
    {
        return _ReportingBusiness.GetBatchDetail(parames);
    }
    [WebMethod]
    public BatchNumberDetails GetBatchsByReportDate(MobileParameters parames)
    {
        return _ReportingBusiness.GetBatchsByReportDate(parames);
    }
    #endregion

    #region Deposit
    [WebMethod]
    public Deposits GetDepositsByLastDays(MobileParameters parames)
    {
        return _ReportingBusiness.GetDepositsByLastDays(parames);
    }
    [WebMethod]
    public DepositDetail GetDepositsByReportDate(MobileParameters parames)
    {
        return _ReportingBusiness.GetDepositsByReportDate(parames);
    }

    #endregion

    #region Retrieval
    [WebMethod]
    public Retrievals GetRetrievalsByLastDays(MobileParameters parames)
    {
        return _ReportingBusiness.GetRetrievalsByLastDays(parames);
    }
    [WebMethod]
    public Retrievals GetRetrievalsByReportDate(MobileParameters parames)
    {
        return _ReportingBusiness.GetRetrievalsByReportDate(parames);
    }
    [WebMethod]
    public RetrievalDetail GetRetrievalDetails(MobileParameters parames)
    {
        return _ReportingBusiness.GetRetrievalDetails(parames);
    }
    #endregion

    #region Chargebacks
    [WebMethod]
    public Chargebacks GetChargebacksByLastDays(MobileParameters parames)
    {
        return _ReportingBusiness.GetChargebacksByLastDays(parames);
    }
    [WebMethod]
    public Chargebacks GetChargebacksByReportDate(MobileParameters parames)
    {
        return _ReportingBusiness.GetChargebacksByReportDate(parames);
    }
    [WebMethod]
    public ChargebackDetail GetChargebackDetails(MobileParameters parames)
    {
        return _ReportingBusiness.GetChargebackDetails(parames);
    }
    #endregion

    #region Statements
    [WebMethod]
    public Statements GetStatementByMonths(MobileParameters parames)
    {
        return _ReportingBusiness.GetStatementByMonths(parames);
    }

    [WebMethod]
    public StatementDetail GetStatementByReportDate(MobileParameters parames)
    {
        return _ReportingBusiness.GetStatementByReportDate(parames);
    }

    [WebMethod]
    public StatementDetail GetStatementByStatementId(MobileParameters parames)
    {
        return _ReportingBusiness.GetStatementByStatementId(parames);
    }

    [WebMethod]
    public SettlementDetailsSummary GetSettlementDetailsByStatementId(MobileParameters parames)
    {
        return _ReportingBusiness.GetSettlementDetailsByStatementId(parames);
    }

    [WebMethod]
    public SurchargeDetailsSummary GetSurchargeDetailsByStatementId(MobileParameters parames)
    {
        return _ReportingBusiness.GetSurchargeDetailsByStatementId(parames);
    }

    [WebMethod]
    public OtherFeesDetailsSummary GetOtherFeesDetailsByStatementId(MobileParameters parames)
    {
        return _ReportingBusiness.GetOtherFeesDetailsByStatementId(parames);
    }

    [WebMethod]
    public StatementDetail GetStatementTotalByStatementId(MobileParameters parames)
    {
        return _ReportingBusiness.GetStatementTotalByStatementId(parames);
    }

    [WebMethod]
    public DataTable CheckMerchantIsConvertion(MobileParameters ps, string merchantNumber)
    {
        return _ReportingBusiness.CheckMerchantIsConvertion(ps, merchantNumber);
    }

    [WebMethod]
    public bool CheckMerchantBelongToUser(MobileParameters ps, string merchantNumber)
    {
        return _ReportingBusiness.CheckMerchantBelongToUser(ps, merchantNumber);
    }

    [WebMethod]
    public DataTable UpdateStatementTracking(MobileParameters ps)
    {
        return _ReportingBusiness.UpdateStatementTracking(ps);
    }
    #endregion

    #region Merchant
    [WebMethod]
    public Merchants GetMerchantList(MobileParameters parames)
    {
        return _ReportingBusiness.GetMerchantList(parames);
    }
    #endregion

    #region Dashboard
    [WebMethod]
    public Monthly GetMonthlyVolume(MobileParameters parames)
    {
        return _ReportingBusiness.GetMonthlyVolume(parames);
    }

    [WebMethod]
    public Analysis GetVolumeAnalysis(MobileParameters parames)
    {
        return _ReportingBusiness.GetVolumeAnalysis(parames);
    }

    [WebMethod]
    public Card GetCardVolume(MobileParameters parames)
    {
        return _ReportingBusiness.GetCardVolume(parames);
    }

    [WebMethod]
    public ChartItem GetChartsVolume(MobileParameters parames)
    {
        return _ReportingBusiness.GetChartsVolume(parames);
    }

    #endregion

    [WebMethod]
    public DataTable GetAuditUserChanges(MobileParameters parames)
    {
        return _ReportingBusiness.GetAuditUserChanges(parames);
    }

    [WebMethod]
    public string GetParentEntityNumber(MobileParameters parames)
    {
        return _ReportingBusiness.GetParentEntityNumber(parames);
    }

    [WebMethod]
    public DepositItemSummary GetDepositItemByStatementId(MobileParameters parames)
    {
        return _ReportingBusiness.GetDepositItemByStatementId(parames);
    }

    [WebMethod]
    public MonthlyMessages GetMonthlyMessages(MobileParameters parames)
    {
        return _ReportingBusiness.GetMonthlyMessages(parames);
    }

    [WebMethod]
    public ChainStatement GetChainStatementByMonths(MobileParameters parames)
    {
        return _ReportingBusiness.GetChainStatementByMonths(parames);
    }
    #endregion
}

