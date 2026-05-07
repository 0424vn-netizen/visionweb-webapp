using AS.Web.Business.General;
using AS.Web.Business.Logger;
using AS.Web.Business.PCI.Models;
using AS.Web.Business.Shared.Constants;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Protocols;
using Org.BouncyCastle.Security;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AS.Web.Business.PCI
{
    public static partial class SsoPciFuncsLib
    {
        public static void PciLogInfo(PciLogInfoRequest request)
        {
            if (request == null)
            {
                PciLoggerManager.Info(string.Format("SSO Log: {0} with request is null. {1}"
                , DateTime.Now.ToString()
                , Environment.NewLine));
                return;
            }
            //log
            string message = string.IsNullOrEmpty(request.Message) ? string.Empty : string.Concat("- Message: ", request.Message);
            string strLog = string.Format("SSO Log: {0} RequestId: {1} - Username: {2} - Info: {3}-{4} - AsClientId: {5} - URL: {6} {7} {8}"
                , DateTime.Now.ToString()
                , request.RequestId
                , request.CurrentUserId
                , request.ClientId
                , request.ClientName
                , request.AsClientId
                , request.PciSsoUrl
                , message
                , Environment.NewLine
             );
            PciLoggerManager.Info(strLog);
        }
        public static string GetSsoRequest(string url, SAMLResponse saml)
        {
            if (string.IsNullOrEmpty(url) || saml == null)
                return null;
            var samlContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(saml.ToXml().OuterXml));
            string resultHtml = string.Format(
            @"<html>
              <body onload=""javascript:document.Form.submit()"">
                <form action=""{0}"" method=""post"" name=""Form"" target=""_blank"">
	              <input  type=""hidden"" name=""TARGET"" value=""_self"" />
	              <input  type=""hidden"" name=""SAMLResponse"" value=""{1}"" />
	            </form>
              </body>
            </html>",
            url,
            samlContent);
            return resultHtml;
        }
        public static SAMLResponse GenerateSamlContent(GenerateSamlContentRequest request)
        {
            // generate raw SAML content
            var saml = new SAMLResponse
            {
                ID = BuildSamlId(),
                Issuer = new Issuer { NameIdentifier = request.ClientName, NameQualifier = request.ClientName }
            };

            var samlAssertion = new SAMLAssertion
            {
                ID = BuildSamlId(),
                Issuer = saml.Issuer,
                IssueInstant = DateTime.UtcNow,
                Subject = new Subject(new NameID(request.AssertionSubject))
            };

            samlAssertion.SetAttributeValue(request.AssertionKeyUserName, request.CurrentUserId);
            samlAssertion.SetAttributeValue(request.AssertionKeyAsClientId, request.AsClientId);

            var xmlAssertion = samlAssertion.ToXml();
            var certificatePass = GeneralFuncsLib.DecryptText(request.PciCertificatePass, request.RequestId);
            var signAssertionCert = LoadCertificate(request.FullPathPciCertificateUrl, certificatePass);

            SAMLAssertionSignature.Generate(xmlAssertion, signAssertionCert.PrivateKey, null, null, signAssertionCert.PrivateKey.SignatureAlgorithm);

            saml.Assertions.Add(xmlAssertion);

            return saml;
        }
        private static X509Certificate2 LoadCertificate(string fullPathCertificateFile, string certificatePassword)
        {
            X509Certificate2 result;

            try
            {
                result = new X509Certificate2(fullPathCertificateFile, certificatePassword, X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            }
            catch (Exception ex)
            {
                throw new InvalidParameterException("Cannot load certificate " + fullPathCertificateFile, ex);
            }

            return result;
        }

        private static string BuildSamlId(object id = null)
        {
            if (id == null || id.ToString() == PciConstants.ID_ZERO.ToString())
            {
                return string.Concat("_", Guid.NewGuid());
            }
            return string.Concat("_", id);
        }
    }
}
