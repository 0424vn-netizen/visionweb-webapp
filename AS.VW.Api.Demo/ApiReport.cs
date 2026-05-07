using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AS.VW.Api.WCF.ServiceInterface;
using AS.VW.Api.Business.Repository;

namespace AS.VW.Api.WCF
{
    public class ApiReport : ReportService
    {
        public ApiReport(IPaymentRepository paymentRepository,
            IBatchRepository batchRepository,
            IAuthorizationRepository authorizationRepository,
            IChargebackRepository chargebackRepository,
            IReturnRepository returnRepository,
            IRetrievalRepository retrievalRepository,
            IVoidsRejectRepository voidsRejectRepository,
            ITransactionRepository transactionRepository,
            IMerchantInformationRepository merchantInformationRepository)
            : base(paymentRepository,
            batchRepository,
            authorizationRepository,
            chargebackRepository,
            returnRepository,
            retrievalRepository,
            voidsRejectRepository,
            transactionRepository,
            merchantInformationRepository)
        { }

    }
}