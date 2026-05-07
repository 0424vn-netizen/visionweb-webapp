using AS.VW.Api.Utility.Utils;
using AS.WCF;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace AS.VW.Api.WCF.ServiceInterface
{
    static class WcfExtension
    {
        public static void AddHttpMessageHeader(string key, string value)
        {
            HttpContext.Current.Request.Headers[key] = value;
        }
        public static string GetHttpMessageHeader(string key)
        {
            return HttpContext.Current.Request.Headers[key];

        }
    }

    class ASWcfServiceAttribute : Attribute, IContractBehavior
    {
        #region IContractBehavior Members

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            // empty method
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {

            ClientCredentials credentials = null;
            var listCredential = endpoint.EndpointBehaviors.Where(x => x is ClientCredentials);
            
            if (listCredential != null && listCredential.Any())
            {
                credentials = (ClientCredentials)listCredential.FirstOrDefault();

                clientRuntime.MessageInspectors.Add(new ClientMessageHeaders(credentials?.UserName?.UserName, credentials?.UserName?.Password));
            }
            else
            {
                clientRuntime.MessageInspectors.Add(new ClientMessageHeaders());
            }
            //this.ins
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.MessageInspectors.Add(new ASMessageInspector());
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            // empty method
        }

        #endregion
    }

    class NonceCollection
    {
        class NonceItem
        {
            public string Nonce { get; set; }
            public DateTime ExpiredTime { get; set; }
        }
        List<NonceItem> _nonces = null;
        public NonceCollection()
        {
            _nonces = new List<NonceItem>();
        }
        public bool AddNonce(string nonce, DateTime created)
        {
            try
            {
                if (_nonces == null)
                    _nonces = new List<NonceItem>();
                int foundIndex = -1;
                for (int i = 0; i < _nonces.Count; i++)
                {
                    if (_nonces[i] != null)
                    {
                        if (_nonces[i].ExpiredTime <= DateTime.Now.ToUniversalTime())
                        {
                            _nonces.RemoveAt(i);
                        }
                        else if (_nonces[i].Nonce == nonce)
                        {
                            foundIndex = i;
                        }
                    }
                    else
                    {
                        AS.Common.Logger.LoggerManager.Info(string.Format("_nonces.Count = {0}; _nonces[{1}]=NULL", _nonces.Count, i));
                        _nonces.RemoveAt(i);
                    }
                }

                if (foundIndex == -1)
                {
                    _nonces.Add(new NonceItem()
                    {
                        Nonce = nonce,
                        ExpiredTime = created.Add(new TimeSpan(0, 5, 0))//five minutes to expired 
                    });
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Info(string.Format("AddNonce(nonce={0}, created={1}) - Error={2}", nonce, created, ex.Message));
                return false;
            }
        }
    }

    class ASMessageInspector : IDispatchMessageInspector
    {
        public ASMessageInspector()
        {
        }

        public string _errorCode = string.Empty;
        public string _faultErrorMessage = string.Empty;
        bool _isRestService { get; set; }
        public bool _accessDenied { get; set; }

        public object AfterReceiveRequest(ref Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            //Reset info
            this._errorCode = string.Empty;
            this._accessDenied = false;

            _isRestService = request.Properties.Keys.Contains("HttpOperationName");
            UsernameToken token = null;

            if (_isRestService)
            {
                //if request for help page, allow to access
                if (request.Properties["HttpOperationName"].ToString() == "HelpPageInvoke") return null;
                IncomingWebRequestContext webRequest = WebOperationContext.Current.IncomingRequest;
                var user = UserHelper.GetUser(webRequest);
                token = new UsernameToken(user.Name, user.Password);
            }
            else
            {
                const string nsWsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
                int headerSecurity = request.Headers.FindHeader("Security", nsWsse);

                if (headerSecurity != -1)
                {
                    XmlElement xml = request.Headers.GetHeader<XmlElement>("Security", nsWsse);
                    token = new UsernameToken(xml);
                }
            }

            if (token == null)
            {
                _errorCode += "User or Password is invalid.";
                OnUnauthorizedAccess(token);
            }
            else
            {
                if (string.IsNullOrEmpty(token.Password))
                {
                    _errorCode += "User or Password is invalid.";
                    OnUnauthorizedAccess(token);
                }

                // Set ClientId
                WcfExtension.AddHttpMessageHeader("ClientID", GetClientId(token.Password));

                object currentServiceInstant = instanceContext.GetServiceInstance();
                string loginAction = ((SecuredService)currentServiceInstant).GetValidateUser(token.Username, token.Password);
                if (loginAction == "1")//invalid user
                {
                    _errorCode += "User or Password is invalid.";
                    OnUnauthorizedAccess(token);
                }
            }

            return null;//valid request
        }

        protected void OnUnauthorizedAccess(UsernameToken token)
        {
            this._accessDenied = true;
            throw new UnauthorizedAccessException("Access denied");
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            bool isRequestFault = false;

            if (this._isRestService)
            {
                HttpResponseMessageProperty responseMessagePropterty = (HttpResponseMessageProperty)reply.Properties["httpResponse"];
                HttpResponseMessage responseMessage = new HttpResponseMessage(responseMessagePropterty.StatusCode);
                if (!responseMessage.IsSuccessStatusCode)
                {
                    isRequestFault = true;
                }
            }
            else
            {
                isRequestFault = reply.IsFault;
            }

            if (isRequestFault)
            {
                var xmlDoc = new XmlDocument() { XmlResolver = null };
                xmlDoc.Load(reply.GetReaderAtBodyContents());
                var xDoc = XDocument.Load(new XmlNodeReader(xmlDoc));
                XElement faultString = GetElement(xDoc, "faultstring");
                XElement faultMessage = GetElement(xDoc, "Message");
                string reason;

                //keep real faulr message in _error code 
                _faultErrorMessage = _errorCode;

                if (string.IsNullOrEmpty(_errorCode) && faultString != null)
                    _errorCode = faultString.Value;
                else
                    _errorCode = "Generic error.";

                if (faultMessage != null)
                    reason = faultMessage.Value;
                else
                    reason = "Generic error.";

                reply = Message.CreateMessage(reply.Version, new FaultException(reason).CreateMessageFault(), _errorCode);
            }
        }

        private XElement GetElement(XDocument xDoc, string elementName)
        {
            foreach (XNode node in xDoc.DescendantNodes().Where(x => x is XElement))
            {
                XElement element = (XElement)node;
                if (element.Name.LocalName.Equals(elementName))
                    return element;
            }
            return null;
        }

        private string GetClientId(string password)
        {
            string id = password.Trim().Substring(password.Length - 4);
            int.TryParse(id, out int clientId);

            return clientId.ToString();
        }
    }
}