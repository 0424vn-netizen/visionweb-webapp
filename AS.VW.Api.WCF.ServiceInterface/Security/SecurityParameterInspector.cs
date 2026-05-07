using AS.VW.Api.Model.Security;
using AS.VW.Api.Business.Repository;
using AS.VW.Api.Business.Repository.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Web;
using Ninject;
using System.ServiceModel.Channels;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public class SecurityParameterInspector : IParameterInspector
    {
        private readonly ISecurityRepository _SecurityRepository;

        private readonly string _PermissionCode;

        public SecurityParameterInspector(string permissionCode)
        {
            _PermissionCode = permissionCode;
            _SecurityRepository = WcfServiceModule.Instance.Resolve<ISecurityRepository>();           
 
            
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            if (WebApiSettings.ApiEnableRequestTracking)
            {
                ServiceLogger.Log.Debug(string.Format("Responding {0} with result: {1}.", operationName, returnValue));            

                var serviceIntance = (OperationContext.Current.InstanceContext.GetServiceInstance() as IAuthenticatedService);
                ServiceRequestLog logTracking = serviceIntance.LogTracking;
                TimeSpan durationRequest = DateTime.Now.Subtract(logTracking.LogRequestStartDts);
                logTracking.LogWebServerDts = DateTime.Now;
                logTracking.LogSessionId = OperationContext.Current.SessionId;
                logTracking.LogElapsedTime = (int)durationRequest.TotalMilliseconds;
                logTracking.LogWebSiteName = OperationContext.Current.Host.Description.Name;
                if (serviceIntance.LoggedInUser != null)
                {
                    logTracking.LogId1 = serviceIntance.LoggedInUser.UserId;
                    logTracking.LogFullName = serviceIntance.LoggedInUser.UserFullName;
                    _SecurityRepository.AddRequestHeader("ClientId", serviceIntance.LoggedInUser.AsClientId.ToString());
                }
                //Get IP Client
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name];
                logTracking.LogClientIpAddress = prop.Address;
                
                _SecurityRepository.InsertServiceRequestLog(logTracking);
            }
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            if (WebApiSettings.ApiEnableRequestTracking && inputs.Length > 0)
            {
                ServiceLogger.Log.Debug(string.Format("Executing {0} with parameters: {1}.", operationName, OperationContext.Current.RequestContext.RequestMessage.ToString()));
            }
            User user = null;
            var serviceIntance = (OperationContext.Current.InstanceContext.GetServiceInstance() as IAuthenticatedService);

            _SecurityRepository.AddRequestHeader("ClientId", serviceIntance.AuthToken.AsClientId.ToString());
            // Get corresponding user
            AuthorizationResult authenResult = _SecurityRepository.ValidateApiTokenAndLoadUser(serviceIntance.AuthToken);

            if (authenResult.Result == LoginActions.Success)
            {
                //Check permission of user
                if (authenResult.Permissions.Any(s => s.Equals(_PermissionCode, StringComparison.OrdinalIgnoreCase)))
                {
                    user = authenResult.AuthorizedUser;
                }
                else
                {
                    throw new FaultException("Invalid Permission!");
                }
            }
            else
            {
                throw new FaultException("Invalid Access!");
            }
             
            // Save above user to current service instance
            
            serviceIntance.LoggedInUser = user;
            serviceIntance.LogTracking = new ServiceRequestLog() { LogRequestStartDts = DateTime.Now };
            return null;
        }
    }
}