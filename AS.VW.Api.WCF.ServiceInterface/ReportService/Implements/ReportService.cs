using AS.VW.Api.Business.Repository;
using System;
using System.Linq;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public partial class ReportService : SecuredService
    {
        private readonly IPaymentRepository _PaymentRepository;
        private readonly IBatchRepository _BatchRepository;
        private readonly IAuthorizationRepository _AuthorizationRepository;
        private readonly IChargebackRepository _ChargebackRepository;
        private readonly IReturnRepository _ReturnRepository;
        private readonly IRetrievalRepository _RetrievalRepository;
        private readonly IVoidsRejectRepository _VoidsRejectRepository;
        private readonly ITransactionRepository _TransactionRepository;
        private readonly IMerchantInformationRepository _MerchantInformationRepository;
        
        public ReportService(
            IPaymentRepository paymentRepository,
            IBatchRepository batchRepository,
            IAuthorizationRepository authorizationRepository,
            IChargebackRepository chargebackRepository,
            IReturnRepository returnRepository,
            IRetrievalRepository retrievalRepository,
            IVoidsRejectRepository voidsRejectRepository,
            ITransactionRepository transactionRepository,
            IMerchantInformationRepository merchantInformationRepository
            )
        {
            
            _PaymentRepository = paymentRepository;
            _PaymentRepository.ServiceAdapter = new WcfServiceAdapter(this);

            _BatchRepository = batchRepository;
            _BatchRepository.ServiceAdapter = new WcfServiceAdapter(this);
            
            _AuthorizationRepository = authorizationRepository;
            _AuthorizationRepository.ServiceAdapter = new WcfServiceAdapter(this);

            _ChargebackRepository = chargebackRepository;
            _ChargebackRepository.ServiceAdapter = new WcfServiceAdapter(this);

            _ReturnRepository = returnRepository;
            _ReturnRepository.ServiceAdapter = new WcfServiceAdapter(this);

            _RetrievalRepository = retrievalRepository;
            _RetrievalRepository.ServiceAdapter = new WcfServiceAdapter(this);

            _VoidsRejectRepository = voidsRejectRepository;
            _VoidsRejectRepository.ServiceAdapter = new WcfServiceAdapter(this);

            _TransactionRepository = transactionRepository;
            _TransactionRepository.ServiceAdapter = new WcfServiceAdapter(this);

            _MerchantInformationRepository = merchantInformationRepository;
            _MerchantInformationRepository.ServiceAdapter = new WcfServiceAdapter(this);

            string strClientId = WcfExtension.GetHttpMessageHeader("ClientID");
            ClientId = string.IsNullOrEmpty(strClientId) ? 0 : int.Parse(strClientId);
            if (ClientId != 0)
            {                
                _PaymentRepository.SetClientId(ClientId.ToString());
                _BatchRepository.SetClientId(ClientId.ToString());
                _AuthorizationRepository.SetClientId(ClientId.ToString());
                _ChargebackRepository.SetClientId(ClientId.ToString());
                _ReturnRepository.SetClientId(ClientId.ToString());
                _RetrievalRepository.SetClientId(ClientId.ToString());
                _VoidsRejectRepository.SetClientId(ClientId.ToString());
                _TransactionRepository.SetClientId(ClientId.ToString());
                _MerchantInformationRepository.SetClientId(ClientId.ToString());
            }
        }

        protected TargetClientAttribute[] Attributes { get; private set; }
    }
}
