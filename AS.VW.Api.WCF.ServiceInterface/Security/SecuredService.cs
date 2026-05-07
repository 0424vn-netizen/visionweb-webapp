using AS.VW.Api.Model.Security;
using AS.VW.Api.Business.Repository;
using AS.VW.Api.Business.Repository.WebService;
using System.ServiceModel.Dispatcher;
using System.Data;
using AS.Common.DataProtection;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public class SecuredService : BaseService, IAuthenticatedService, AS.WCF.IPasswordProvider
    {
        #region Public Properties
        /// <summary>
        /// Access to logged-in user information
        /// </summary>
        public User LoggedInUser { get; set; }

        public ServiceRequestLog LogTracking { get; set; }

        public ApiAuthToken AuthToken { get;  set; }

        public int ClientId { get; set; }
        
        #endregion

        public string GetUserPassword(string userName)
        {
            // This declare is not reach to ReportRepositoryBase
            ISecurityRepository securityRepository = WcfServiceModule.Instance.Resolve<ISecurityRepository>();
            // TO DO:  parse userName to get ClientId 
            securityRepository.AddRequestHeader("ClientId", ClientId.ToString());
            string password = securityRepository.GetUserPassword(new Model.Security.ApiAuthToken() { ApiClientId = userName, AsClientId = ClientId });
            AuthToken = new ApiAuthToken() { ApiClientId = userName, ApiKey = password, AsClientId = ClientId };
            LoggedInUser = new User();
            LoggedInUser.AsClientId = ClientId;
            LoggedInUser.UserId = userName;
            
            return password;
        }

        public string GetValidateUser(string userName, string password)
        {
            // This declare is not reach to ReportRepositoryBase
            ISecurityRepository securityRepository = WcfServiceModule.Instance.Resolve<ISecurityRepository>();
            securityRepository.AddRequestHeader("ClientId", ClientId.ToString());

            string passwordEnc = Cryptophy.EncryptText(password);

            AuthToken = new ApiAuthToken() { ApiClientId = userName, ApiKey = passwordEnc, AsClientId = ClientId };
            LoggedInUser = new User();
            LoggedInUser.AsClientId = ClientId;
            LoggedInUser.UserId = userName;

            DataTable userlogin = securityRepository.GetValidateUser(AuthToken);
            if (userlogin != null && userlogin.Rows.Count > 0)
            {
                return userlogin.Rows[0]["LoginAction"].ToString();
            }

            return "1";
        }

        public void SetUsername(string userName)
        {
            LoggedInUser.UserId = userName;
        }
    }
}
