using System;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Protocols;
using ComponentSpace.SAML2.Profiles.SSOBrowser;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using AS.Common.Logger;
using ASUser = AS.Security.WS.Entities.User;
using System.Linq;
using System.Configuration;
using System.Threading;
using System.Web;
using As.VisionWeb.Web;
using As.VisionWeb.Web.Helper;
using System.IO;
using System.Text.RegularExpressions;

namespace AS.WEB.UI.SamlSign
{
    public abstract class SamlS : NonReportPage
    {
        #region vars
        protected bool signedResponse = false;
        protected bool signedAssertion = false;
        protected bool encryptedAssertion = false;
        protected bool permissionLevelIsPassed = false;
        protected int clientID = 0;
        protected Security.Web.SecurityServices.SecService.Partner partner = null;
        protected SAMLResponse samlResponse = null;
        protected SAMLAssertion samlAssertion = null;
        protected ASUser user = null;
        protected string userName = string.Empty;
        protected int permissionLevel = 0;
        protected int destinationID = 0;
        protected string relayState = null;
        protected string partnerName = null;
        protected int partnerID = 0;
        protected string clientCertificateFilename = string.Empty;
        protected string privateCertificateFilename = string.Empty;
        protected string privateCertificatePassword = string.Empty;
        protected const string certificateDir = "~/App_Data/SAML/";
        protected string ProcessResultMessage = string.Empty;
        protected SSO.SsoConfiguration SsoConfiguration { get; set; }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            SsoConfiguration = SsoHelper.GetSsoConfiguration();
            Process();
        }
        protected void Process()
        {
            try
            {
                LogDebug("SAML: Begin Process SAMLResponse.");
                if (!ReceiveSAMLResponse())
                {
                    LogSSOTracking(clientID, userName, false, "ReceiveSAMLResponse: Fail");
                    ProcessSamlResponseFail();
                    return;
                }

                if (!GetPartnerName())
                {
                    LogSSOTracking(clientID, userName, false, "GetPartnerName: Fail");
                    ProcessSamlResponseFail();
                    return;
                }

                if (!GetPartnerConfig())
                {
                    LogSSOTracking(clientID, userName, false, "GetPartnerConfig: Fail");
                    ProcessSamlResponseFail();
                    return;
                }

                if (!CheckSAMLResponse())
                {
                    LogSSOTracking(clientID, userName, false, "CheckSAMLResponse: Fail");
                    ProcessSamlResponseFail();
                    return;
                }

                // Process the SAML response.
                if (!ProcessSAMLResponse())
                {
                    LogSSOTracking(clientID, userName, false, "ProcessSAMLResponse: Fail");
                    ProcessSamlResponseFail();
                    return;
                }

                if (!ProcessUser())
                {
                    LogSSOTracking(clientID, userName, false, ProcessResultMessage);
                    ProcessSamlResponseFail();
                    return;
                }
                LogDebug(string.Format("SAML: SSO is successful: {0}-{1}", clientID, userName));
                LogSSOTracking(clientID, userName, true, string.Empty);
            }
            catch (Exception ex)
            {
                LogSSOTracking(clientID, userName, false, ex.Message);
                ProcessSamlResponseFail();
                LoggerManager.Error("SAML: Error in assertion consumer service.", ex);
            }
        }
        protected bool GetPartnerName()
        {
            bool ret = false;

            if (samlResponse == null)
            {
                LogDebug("SAML: Error getting parter Name from response. Response was null.");
                throw new ArgumentException("SAML: Error getting parter Name from response. Response was null.");
            }

            try
            {
                partnerName = samlResponse.Issuer.NameIdentifier;
                ret = true;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(string.Format("SAML: Error getting partnerName from response ({0}).", samlResponse.Issuer.NameIdentifier), ex);
            }

            return ret;
        }
        // Receive the SAML response from the identity provider.
        private bool ReceiveSAMLResponse()
        {
            // Receive the SAML response.
            XmlElement samlResponseXml = null;
            var timeLog = DateTime.Now.Ticks.ToString();
            LogRawRequest(timeLog);
            ServiceProvider.ReceiveSAMLResponseByHTTPPost(Request, out samlResponseXml, out relayState);
            samlResponse = new SAMLResponse(samlResponseXml);
            LogRequest(timeLog, samlResponseXml.OuterXml);
            return true;
        }
        protected bool CheckSAMLResponse()
        {

            bool ret = false;

            if (signedResponse)
            {
                if (CheckResponseSignature())
                    ret = true;
            }
            else
                if (!CheckIfResponseSigned())
                ret = true;

            return ret;
        }
        protected bool CheckIfResponseSigned()
        {
            bool ret = true;

            XmlElement samlResponseXML = samlResponse.ToXml();

            if (SAMLMessageSignature.IsSigned(samlResponseXML))
            {
                LogDebug("SAML: Response was not supposed to be signed.");

                throw new ArgumentException("SAML: Response was not supposed to be signed.");
            }
            else
                ret = false;


            return ret;
        }
        protected bool CheckResponseSignature()
        {
            bool ret = false;

            XmlElement samlResponseXML = samlResponse.ToXml();
            LogDebug("SAML: Verifying response signature.");


            if (!SAMLMessageSignature.IsSigned(samlResponseXML))
            {
                LogDebug("SAML: The SAML response was not signed.");

                throw new ArgumentException("The SAML response was not signed.");
            }
            else
            {
                X509Certificate2 clientCertificate = LoadClientCertificate(Server.MapPath(certificateDir + partner.ResponseSignedCertPath));

                if (clientCertificate == null || !SAMLMessageSignature.Verify(samlResponseXML, clientCertificate))
                {
                    LogDebug("SAML: The SAML response signature failed to verify.");

                    throw new ArgumentException("The SAML response signature failed to verify.");
                }
                else
                    ret = true;
            }

            return ret;
        }
        // Process the SAML response.
        protected bool ProcessSAMLResponse()
        {
            LogDebug("SAML: Processing SAML response.");

            if (!ExtractDataFromResponse())
                return false;

            // Extract the asserted identity from the SAML response.
            if (!GetAssertion())
                return false;

            if (!ExtractDataFromAssertion())
                return false;

            LogDebug("SAML: Processed successful SSO SAML response.");
            return true;
        }
        protected bool ExtractDataFromResponse()
        {
            bool ret = true;

            if (partnerName == string.Empty)
            {
                LogDebug(string.Format("SAML: Missing Issuer ({0})", samlResponse.Issuer));
                ret = false;
            }

            return ret;
        }
        protected bool GetPartnerConfig()
        {
            // note that clientID is needed for the correct db connection string 
            if ((partner = WebServices.SecurityServices.GetPartners(partnerName, clientID.ToString())) != null)
            {
                partnerID = partner.PartnerID;
                signedResponse = partner.ResponseSigned;
                signedAssertion = partner.AssertionSigned;
                encryptedAssertion = partner.AssertionEncrypted;
                permissionLevelIsPassed = partner.PermissionLevelIsPassed;

                clientCertificateFilename = partner.AssertionSignedCertPath;
                privateCertificateFilename = partner.AperiaCertPath;
                privateCertificatePassword = partner.AperiaCertPassword;
                clientID = partner.ASClientID;

                return true;
            }
            else
                LogDebug(string.Format("SAML: Failed to retrieve partner configuration for ({0}).", partnerName));


            return false;
        }
        protected virtual bool HandleUsername(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                LogDebug(string.Format("SAML: Missing Attribute - username ({0})", samlAssertion.GetAttributeValue("username")));
                return false;
            }
            return true;
        }
        protected bool ExtractDataFromAssertion()
        {
            bool ret = true;

            if (samlAssertion != null)
            {
                userName = samlAssertion.GetAttributeValue("username");

                if (string.IsNullOrEmpty(userName))
                {
                    userName = samlAssertion.GetAttributeValue("UserName");
                }

                if(!string.IsNullOrEmpty(userName))
                    userName = Regex.Replace(userName, @"\s+", "");

                string strdestinationID = string.IsNullOrEmpty(samlAssertion.GetAttributeValue("destinationid")) ? "1" : samlAssertion.GetAttributeValue("destinationid");

                ret = HandleUsername(userName);

                if (!int.TryParse(strdestinationID, out destinationID))
                {
                    LogDebug(string.Format("SAML: Missing Attribute - destinationid ({0})", samlAssertion.GetAttributeValue("destinationid")));
                    ret = false;
                }

                if (permissionLevelIsPassed)
                {
                    string strPermissionLevel = samlAssertion.GetAttributeValue("permissionLevel");

                    if (string.IsNullOrEmpty(strPermissionLevel))
                    {
                        LogDebug(string.Format("SAML: Missing Attribute - permissionLevel ({0})", strPermissionLevel));
                        ret = false;
                    }

                    if (!int.TryParse(strPermissionLevel, out permissionLevel))
                    {
                        LogDebug(string.Format("SAML: Unable to convert permissionLevel ({0}) to a valid number", strPermissionLevel));
                        ret = false;
                    }
                }
                else
                {
                    string strPermissionLevel = samlAssertion.GetAttributeValue("permissionLevel");

                    if (!string.IsNullOrEmpty(strPermissionLevel))
                    {
                        LogDebug(string.Format("SAML: Error. A permission level attribute was passed and was unexpected - permissionLevel ({0})", strPermissionLevel));
                        ret = false;
                    }
                }
            }
            else
            {
                LogDebug("SAML: Assertion is null");
                throw new ArgumentException("Assertion is null");
            }

            return ret;
        }
        public static TEnum GetMaxValue<TEnum>() where TEnum : IComparable, IConvertible, IFormattable
        {
            Type type = typeof(TEnum);

            if (!type.IsSubclassOf(typeof(Enum)))
                throw new
                    InvalidCastException("Cannot cast '" + type.FullName + "' to System.Enum.");

            return (TEnum)Enum.ToObject(type, Enum.GetValues(type).Cast<int>().Last());
        }
        protected abstract bool ProcessUser();
        protected abstract void ProcessSamlResponseFail();

        #region GetAssertionMethods
        protected bool GetAssertion()
        {
            //If encrypted I need to get the assertion and unencrypt it
            if (encryptedAssertion)
                return GetEncryptedAssertion();
            else if (signedAssertion)
            {
                if (samlAssertion == null && !GetSignedAssertion())
                    return false;

                return CheckAssertionSignature();
            }
            else  // assertion is neither encrypted nor signed
                return GetUnencryptedUnsignedAssertion();
        }
        protected bool GetEncryptedAssertion()
        {
            bool result = false;

            try
            {
                EncryptedAssertion[] encryptedAssertions = samlResponse.GetEncryptedAssertions().ToArray<EncryptedAssertion>();

                if (encryptedAssertions.Length > 0)
                {
                    X509Certificate2 privateCertificate = LoadCertificate(Server.MapPath(certificateDir + partner.AperiaCertPath), partner.AperiaCertPassword);
                    EncryptedAssertion firstAssertion = encryptedAssertions[0];
                    samlAssertion = firstAssertion.Decrypt(privateCertificate, null);

                    if (samlAssertion == null)
                        LogDebug("SAML: Requiring encrypted assertions, but none found in response.");
                    else
                        result = true;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Error("SAML: Error getting encrypted assertion.", ex);
            }

            return result;
        }
        protected bool GetSignedAssertion()
        {
            if (samlResponse.GetSignedAssertions().Count > 0)
                samlAssertion = new SAMLAssertion(samlResponse.GetSignedAssertions()[0]);

            if (samlAssertion == null)
                return false;

            return true;
        }
        protected bool CheckAssertionSignature()
        {
            bool ret = false;

            XmlElement samlAssertionXML = samlAssertion.ToXml();
            XmlDocument xmlDocument = new XmlDocument() { XmlResolver = null };
            xmlDocument.LoadXml(samlResponse.ToXml().OuterXml);
            SignedXml signedXml = new SignedXml(xmlDocument);
            signedXml.LoadXml(samlAssertion.Signature);

            LogDebug("SAML: Verifying assertion signature.");

            if (!SAMLAssertionSignature.IsSigned(samlAssertionXML))
            {
                LogDebug("SAML: The SAML assertion was not signed.");

                throw new ArgumentException("The SAML assertion was not signed.");
            }
            else
            {
                X509Certificate2 clientCertificate = LoadClientCertificate(Server.MapPath(certificateDir + partner.AssertionSignedCertPath));

                LogDebug(string.Format("CertificateDir = {0}, AssertionCertPath = {1}, FullPath = {2}", certificateDir, partner.AssertionSignedCertPath, Server.MapPath(certificateDir + partner.AssertionSignedCertPath)));

                if (clientCertificate == null || !SAMLAssertionSignature.Verify(samlAssertionXML, clientCertificate) && !signedXml.CheckSignature(clientCertificate, true))
                {
                    LogDebug("SAML: The SAML assertion signature failed to verify.");

                    throw new ArgumentException("The SAML assertion signature failed to verify.");
                }
                else
                    ret = true;
            }

            return ret;
        }
        protected bool GetUnencryptedUnsignedAssertion()
        {
            if ((samlResponse.GetAssertions().Count > 0) && ((samlAssertion = samlResponse.GetAssertions()[0]) != null))
                return true;
            else
                LogDebug("SAML: Assertion not found in Response.");

            return false;
        }
        #endregion

        #region LoadCertificateMethods
        protected X509Certificate2 LoadClientCertificate(string certPath)
        {
            return LoadCertificate(certPath, privateCertificatePassword);

        }
        protected X509Certificate2 LoadCertificate(string certPath, string password)
        {
            X509Certificate2 cert = null;

            try
            {
                cert = new X509Certificate2(certPath, password, X509KeyStorageFlags.MachineKeySet);
            }
            catch (Exception ex)
            {
                LoggerManager.Error(string.Format("SAML: Error in LoadCertificate for filename ({0}) and password ({1})  error - ({2}) - ({3}).", certPath, string.Empty, ex.Message, ex.InnerException));
            }

            return cert;
        }
        #endregion

        protected void LogDebug(object log)
        {
            if (SsoConfiguration != null && SsoConfiguration.IsDebug)
                LoggerManager.Debug(log);
        }
        private void LogSSOTracking(int clientId, string userId, bool isSucess, string message)
        {
            if (IsLogSSOTracking(clientId))
            {
                var clientIp = GeneralFuncsLib.GetRemoteIpAddress;
                var hostIp = HttpContext.Current.Server.MachineName;
                var sessionId = SessionManager.UniqueSessionID;
                var apId = Session.SessionID;

                AS.Web.LogServices.TrackingLogService.SsoTracking ssoTracking = new Web.LogServices.TrackingLogService.SsoTracking()
                {
                    AppId = apId,
                    ClientId = clientId,
                    UserId = userId,
                    Status = isSucess,
                    Message = message,
                    ClientIp = clientIp,
                    HostIp = hostIp,
                    SessionId = sessionId
                };
                WebServices.LogServices.InsertSSOTrackingLog(ssoTracking);
            }
        }
        private bool IsLogSSOTracking(int clientId)
        {
            if (SsoConfiguration != null && SsoConfiguration.SsoTrackingForClient != null && SsoConfiguration.SsoTrackingForClient.Any())
            {
                if (SsoConfiguration.SsoTrackingForClient.Any(x => x.Equals("All", StringComparison.OrdinalIgnoreCase)))
                    return true;

                var config = SsoConfiguration.SsoTrackingForClient.FirstOrDefault(x => x == clientId.ToString());
                return config != null;
            }

            return false;
        }
        private string GetLogFolder()
        {
            var path = ConfigurationManager.AppSettings["RequestLogFolder"] ?? "~/App_Data/SAML/RequestLogs/";
            path = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd"));
            var physicalPath = Server.MapPath(path);
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
            }

            return physicalPath;
        }
        private void LogRawRequest(string time)
        {
            if (SsoConfiguration != null && SsoConfiguration.IsLogRequest)
            {
                var folder = GetLogFolder();
                var fileName = Path.Combine(folder, "Request_" + time + "_Raw");
                Request.SaveAs(fileName, true);
            }
        }
        private void LogRequest(string time, string sAMLResponse)
        {
            if (SsoConfiguration != null && SsoConfiguration.IsLogRequest)
            {
                var folder = GetLogFolder();
                var fileName = Path.Combine(folder, "Request_" + time);
                File.WriteAllText(fileName, sAMLResponse);
            }
        }
    }
}