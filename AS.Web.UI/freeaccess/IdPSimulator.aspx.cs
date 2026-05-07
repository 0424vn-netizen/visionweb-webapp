using System;
using System.Xml;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Protocols;
using System.IO;
using AS.Common.Logger;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Text;




    public partial class freeaccess_IdPSimulator : System.Web.UI.Page
    {

        #region Vars
        
        protected string responseId = "uuid-";
        protected string assertionId = "uuid-";
        protected const string SAMLEndpoint = "";
        protected const string Target = "";
        protected const string pubCertFileName = "sso-testing.pfx";
        protected const string pfxFileName = "sso-testing.pfx";
        protected const string pfxPassword = "password1";
        protected const string certificateDir = "~/App_Data/SAML/";
        protected SAMLResponse _samlResponse = null;
        protected SAMLAssertion samlAssertion = null;
        protected EncryptedAssertion encryptedSamlAssertion = null;
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
        }


        protected void sendResponse_Click(object sender, EventArgs e)
        {
            ProcessSSORespoonse();
        }

        private void TestSignedResponse()
        {
            byte[] data = Convert.FromBase64String("PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPHNhbWwycDpSZXNwb25zZSBJRD0iXzZhNTgzNDA3NzY5N2YxYTZlZDNhNTdkMzY0MWFjMWI4IiBJc3N1ZUluc3RhbnQ9IjIwMTUtMDUtMTRUMTY6NTQ6NTQuNzg5WiIgVmVyc2lvbj0iMi4wIiB4bWxuczpzYW1sMnA9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDpwcm90b2NvbCI+PHNhbWwyOklzc3VlciB4bWxuczpzYW1sMj0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmFzc2VydGlvbiI+VE5CQ0k8L3NhbWwyOklzc3Vlcj48c2FtbDI6QXNzZXJ0aW9uIElEPSJfYmU5MjdiZTkyMTFmZmVjNGYyZTc4M2Q2YTY0OWI5NzEiIElzc3VlSW5zdGFudD0iMjAxNS0wNS0xNFQxNjo1NDo1NC43ODlaIiBWZXJzaW9uPSIyLjAiIHhtbG5zOnNhbWwyPSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXNzZXJ0aW9uIj48ZHM6U2lnbmF0dXJlIHhtbG5zOmRzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjIj4KPGRzOlNpZ25lZEluZm8geG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPgo8ZHM6Q2Fub25pY2FsaXphdGlvbk1ldGhvZCBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvMTAveG1sLWV4Yy1jMTRuIyIgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiLz4KPGRzOlNpZ25hdHVyZU1ldGhvZCBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNyc2Etc2hhMSIgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiLz4KPGRzOlJlZmVyZW5jZSBVUkk9IiNfYmU5MjdiZTkyMTFmZmVjNGYyZTc4M2Q2YTY0OWI5NzEiIHhtbG5zOmRzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjIj4KPGRzOlRyYW5zZm9ybXMgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPgo8ZHM6VHJhbnNmb3JtIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnI2VudmVsb3BlZC1zaWduYXR1cmUiIHhtbG5zOmRzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjIi8+CjxkczpUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzEwL3htbC1leGMtYzE0biMiIHhtbG5zOmRzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjIi8+CjwvZHM6VHJhbnNmb3Jtcz4KPGRzOkRpZ2VzdE1ldGhvZCBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNzaGExIiB4bWxuczpkcz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnIyIvPgo8ZHM6RGlnZXN0VmFsdWUgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPjdMMTNVTzNQV0JpTHpTaWRqWnhDQzFTbEY2OD08L2RzOkRpZ2VzdFZhbHVlPgo8L2RzOlJlZmVyZW5jZT4KPC9kczpTaWduZWRJbmZvPgo8ZHM6U2lnbmF0dXJlVmFsdWUgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPgpnTi8zc2Z3UC9jc1ZTT0xUL2RWODdhb1h5cG56aGI5Wm1yZzg0dGtHcmpJREJkeFMwV3JmNTg2R08yZmg4TDNGZkhkVzFYMmtXK2pUCnZnUjNUTjI1SWtpVzRDWUVWTWg3Z0pTdHJHeUlkdXYzeElnanVWRlI2bDRVcDFoTzNjSVVEQzRoWWhMR1M2ZjhIQzJhclhKeEFMdDkKWHZORDlWWGxYMThLdG8xekpDVTRyQ1VKbXFhc1E4aG9Pd25PSUpyZDF1MlZCbUhRSTJsUFFPcmVYR3A2Q004V1VHOU9UaVVhc2l0cAp5OE4veUY3ZHpCUXAvcnZiMnh4RzZCOVpwOW9SK3JwQnZEN002Ujc5RjBXUXphRlZ1d2hHS0VNU3VvdE9XZVl5U1lUR1h2RDdKOFZnClRqcFNqWHFSZzNEeVFuZkVISjAvRFBOSFVXVXZHVTF1WFl1RmxRPT0KPC9kczpTaWduYXR1cmVWYWx1ZT4KPC9kczpTaWduYXR1cmU+PHNhbWwyOkF0dHJpYnV0ZVN0YXRlbWVudD48c2FtbDI6QXR0cmlidXRlIE5hbWU9InVzZXJuYW1lIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDI6QXR0cmlidXRlVmFsdWU+ODc4ODg1MDAxOTAwNjwvc2FtbDI6QXR0cmlidXRlVmFsdWU+PC9zYW1sMjpBdHRyaWJ1dGU+PHNhbWwyOkF0dHJpYnV0ZSBOYW1lPSJwZXJtaXNzaW9uTGV2ZWwiIE5hbWVGb3JtYXQ9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDphdHRybmFtZS1mb3JtYXQ6YmFzaWMiPjxzYW1sMjpBdHRyaWJ1dGVWYWx1ZT4yPC9zYW1sMjpBdHRyaWJ1dGVWYWx1ZT48L3NhbWwyOkF0dHJpYnV0ZT48L3NhbWwyOkF0dHJpYnV0ZVN0YXRlbWVudD48L3NhbWwyOkFzc2VydGlvbj48L3NhbWwycDpSZXNwb25zZT4=");            

            string decodedString = Encoding.UTF8.GetString(data);

            //parse xml
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            xmlDocument.LoadXml(decodedString);

            //extract assertion

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");
            namespaceManager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            namespaceManager.AddNamespace("xenc", "http://www.w3.org/2001/04/xmlenc#");
            XmlElement assertion = (XmlElement)xmlDocument.SelectSingleNode("/samlp:Response/saml:Assertion", namespaceManager);

            SignedXml signedXml = new SignedXml(xmlDocument);
            // Find the "Signature" node and create a new            
            // Load the signature node.
            XmlElement s = (XmlElement)assertion.ChildNodes[0];
            signedXml.LoadXml(s);

            //x509
            X509Certificate2 x = new X509Certificate2(MapPath(@"~\App_Data\SAML\TNBCI_Cert.cer"));
            //validate
            Boolean isValidToken = signedXml.CheckSignature(x, true);
        }

        protected void uxSendResponseFromSample_Click(object sender, EventArgs e)
        {
            TestSignedResponse();
            string sampleSamlXml = string.Empty;

            System.IO.StreamReader testXml = new System.IO.StreamReader(MapPath(@"~\App_Data\SAML\TestSamlResponse.xml"));
            sampleSamlXml = testXml.ReadToEnd();
            testXml.Close();

            PostSamlSample(sampleSamlXml);
        }

        protected void ProcessSSORespoonse()
        {
            
            if (!ConstructResponse())
                return;

            SendResponse();
        }


        protected bool ConstructResponse()
        {

            try
            {

                _samlResponse = new SAMLResponse();

                responseId += Guid.NewGuid().ToString().ToUpper();  // Set ResponseID

                SetResponseAttributes();

                assertionId += Guid.NewGuid().ToString().ToUpper();  // Set AssertionID
             
                samlAssertion = new SAMLAssertion();

                SetAssertionAttributes();

                if (RadioButtonListAssertionSignedEncrypt.SelectedValue == "Signed")
                {
                    if (!SignAssertion())
                        return false;
                }
                else if (RadioButtonListAssertionSignedEncrypt.SelectedValue == "Encrypted")
                {
                    if (!EncryptAssertion())
                        return false;

                    _samlResponse.Assertions.Add(encryptedSamlAssertion);
                }
                else
                    _samlResponse.Assertions.Add(samlAssertion);


                if (ResponseSigned.Checked == true)
                {
                    if (!SignResponse())
                        return false;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Error("ConstructRespoonse", ex);
                throw ex;
            }

            return true;
        }


        protected bool SendResponse()
        {
            return PostHtml();
        }


        protected void SetResponseAttributes()
        {
            Issuer partner = new Issuer(partnerName.Text);
            _samlResponse.Issuer = partner;
         }

        protected bool SetAssertionAttributes()
        {
            bool ret = false;

            try
            {

                Issuer partner = new Issuer(partnerName.Text);

                //Assertion Attributes
                samlAssertion.Issuer = partner;
                samlAssertion.SetAttributeValue("AssertionID", assertionId);
                samlAssertion.SetAttributeValue("IssueInstant", DateTime.Now.ToUniversalTime().ToString("s") + "Z");
                samlAssertion.SetAttributeValue("username", userName.Text);
                samlAssertion.SetAttributeValue("destinationid", drpApplication.SelectedValue);
                
                if(CheckBoxSendSecurityLevel.Checked == true)
                    samlAssertion.SetAttributeValue("permissionLevel", RadioButtonListSecurityLevel.SelectedValue); 

                ret = true;
            }
            catch (Exception ex)
            {
                LoggerManager.Error("SetAssertionAttributes", ex);
            }

            return ret;
        }

            

        protected bool SignAssertion()
        {
            {
                bool ret = false;

                try
                {
                    XmlElement assertionXMLElement = samlAssertion.ToXml();

                     //Retrieve the Key
                    AsymmetricAlgorithm key = GetPrivateKey();

                    if (key == null)
                        return true;

                    SAMLAssertionSignature.Generate(assertionXMLElement, key);  

                    _samlResponse.Assertions.Add(assertionXMLElement);

                    ret = true;
                }
                catch (Exception ex)
                {
                    LoggerManager.Error("SignAssertion", ex);
                }
                return ret;
            }
        }



        protected bool SignResponse()
        {
            bool ret = false;

            try
            {
                XmlElement responseXMLElement = _samlResponse.ToXml();

                //Retrieve the Key
                AsymmetricAlgorithm key = GetPrivateKey();

                SAMLMessageSignature.Generate(responseXMLElement, key);

                _samlResponse = new SAMLResponse(responseXMLElement);

                ret = true;           
            }
            catch (Exception ex)
            {
                LoggerManager.Error("SignResponse", ex);
            }
            return ret;
        }




        

          protected bool EncryptAssertion()
          {
                bool ret = false;
              
                try
                {

                    // Load the certificate for the encryption.
                    X509Certificate2 encryptingCert = LoadCertificate(Server.MapPath(certificateDir + pubCertFileName), pfxPassword);
    
                    // Create an encrypted SAML assertion from the SAML assertion we have created.
                    encryptedSamlAssertion = new EncryptedAssertion(samlAssertion, encryptingCert, new EncryptionMethod(EncryptedXml.XmlEncTripleDESUrl));
                    ret = true;

                }
                catch (Exception ex)
                {
                    LoggerManager.Error("EncryptAssertion" + Server.MapPath(certificateDir + pubCertFileName), ex);
                }
                return ret;
          }


          protected bool PostSamlSample(string samlResponseXml)
          {
              bool ret = false;

              try
              {

                  byte[] SAML = System.Text.Encoding.UTF8.GetBytes(samlResponseXml);

                  string Target = "_self";
                  string SAMLEndpoint;
                  
                  string postedSAML = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPHNhbWwycDpSZXNwb25zZSBJRD0iXzZhNTgzNDA3NzY5N2YxYTZlZDNhNTdkMzY0MWFjMWI4IiBJc3N1ZUluc3RhbnQ9IjIwMTUtMDUtMTRUMTY6NTQ6NTQuNzg5WiIgVmVyc2lvbj0iMi4wIiB4bWxuczpzYW1sMnA9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDpwcm90b2NvbCI+PHNhbWwyOklzc3VlciB4bWxuczpzYW1sMj0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmFzc2VydGlvbiI+VE5CQ0k8L3NhbWwyOklzc3Vlcj48c2FtbDI6QXNzZXJ0aW9uIElEPSJfYmU5MjdiZTkyMTFmZmVjNGYyZTc4M2Q2YTY0OWI5NzEiIElzc3VlSW5zdGFudD0iMjAxNS0wNS0xNFQxNjo1NDo1NC43ODlaIiBWZXJzaW9uPSIyLjAiIHhtbG5zOnNhbWwyPSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXNzZXJ0aW9uIj48ZHM6U2lnbmF0dXJlIHhtbG5zOmRzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjIj4KPGRzOlNpZ25lZEluZm8geG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPgo8ZHM6Q2Fub25pY2FsaXphdGlvbk1ldGhvZCBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvMTAveG1sLWV4Yy1jMTRuIyIgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiLz4KPGRzOlNpZ25hdHVyZU1ldGhvZCBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNyc2Etc2hhMSIgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiLz4KPGRzOlJlZmVyZW5jZSBVUkk9IiNfYmU5MjdiZTkyMTFmZmVjNGYyZTc4M2Q2YTY0OWI5NzEiIHhtbG5zOmRzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjIj4KPGRzOlRyYW5zZm9ybXMgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPgo8ZHM6VHJhbnNmb3JtIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnI2VudmVsb3BlZC1zaWduYXR1cmUiIHhtbG5zOmRzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjIi8+CjxkczpUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzEwL3htbC1leGMtYzE0biMiIHhtbG5zOmRzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjIi8+CjwvZHM6VHJhbnNmb3Jtcz4KPGRzOkRpZ2VzdE1ldGhvZCBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNzaGExIiB4bWxuczpkcz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnIyIvPgo8ZHM6RGlnZXN0VmFsdWUgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPjdMMTNVTzNQV0JpTHpTaWRqWnhDQzFTbEY2OD08L2RzOkRpZ2VzdFZhbHVlPgo8L2RzOlJlZmVyZW5jZT4KPC9kczpTaWduZWRJbmZvPgo8ZHM6U2lnbmF0dXJlVmFsdWUgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPgpnTi8zc2Z3UC9jc1ZTT0xUL2RWODdhb1h5cG56aGI5Wm1yZzg0dGtHcmpJREJkeFMwV3JmNTg2R08yZmg4TDNGZkhkVzFYMmtXK2pUCnZnUjNUTjI1SWtpVzRDWUVWTWg3Z0pTdHJHeUlkdXYzeElnanVWRlI2bDRVcDFoTzNjSVVEQzRoWWhMR1M2ZjhIQzJhclhKeEFMdDkKWHZORDlWWGxYMThLdG8xekpDVTRyQ1VKbXFhc1E4aG9Pd25PSUpyZDF1MlZCbUhRSTJsUFFPcmVYR3A2Q004V1VHOU9UaVVhc2l0cAp5OE4veUY3ZHpCUXAvcnZiMnh4RzZCOVpwOW9SK3JwQnZEN002Ujc5RjBXUXphRlZ1d2hHS0VNU3VvdE9XZVl5U1lUR1h2RDdKOFZnClRqcFNqWHFSZzNEeVFuZkVISjAvRFBOSFVXVXZHVTF1WFl1RmxRPT0KPC9kczpTaWduYXR1cmVWYWx1ZT4KPC9kczpTaWduYXR1cmU+PHNhbWwyOkF0dHJpYnV0ZVN0YXRlbWVudD48c2FtbDI6QXR0cmlidXRlIE5hbWU9InVzZXJuYW1lIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDI6QXR0cmlidXRlVmFsdWU+ODc4ODg1MDAxOTAwNjwvc2FtbDI6QXR0cmlidXRlVmFsdWU+PC9zYW1sMjpBdHRyaWJ1dGU+PHNhbWwyOkF0dHJpYnV0ZSBOYW1lPSJwZXJtaXNzaW9uTGV2ZWwiIE5hbWVGb3JtYXQ9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDphdHRybmFtZS1mb3JtYXQ6YmFzaWMiPjxzYW1sMjpBdHRyaWJ1dGVWYWx1ZT4yPC9zYW1sMjpBdHRyaWJ1dGVWYWx1ZT48L3NhbWwyOkF0dHJpYnV0ZT48L3NhbWwyOkF0dHJpYnV0ZVN0YXRlbWVudD48L3NhbWwyOkFzc2VydGlvbj48L3NhbWwycDpSZXNwb25zZT4=";
                  postedSAML = Convert.ToBase64String(SAML);

                  if (RadioButtonSSO_SLO.SelectedValue == "SSO")
                      SAMLEndpoint = "sso.aspx";
                  else
                      SAMLEndpoint = "slo.aspx";

                  string htmlPosted = string.Format(
                       @"<html>" +
                       @"<body onload=""javascript:document.Form.submit()"">" +
                       @"<form action=""{0}"" method=""post"" name=""Form"" target=""_blank"">" +
                       @"<input  type=""hidden"" name=""TARGET"" value=""{1}"" />" +
                      //@"<input  type=""hidden"" name=""clientID"" value=""{2}"" />" +
                       @"<input  type=""hidden"" name=""SAMLResponse"" value=""{3}"" />" +
                       @"<noscript><div>" +
                       @"You do not have JavaScript enabled in your browser, or you are " +
                       @"using a browser without JavaScript support." +
                       @"</div><div>" +
                       @"To continue the single sign-on process, click the button below</div>" +
                       @"<button type=""submit"">Continue Authentication</button>" +
                       @"</noscript>" +
                       @"</form></body></html>",
                       System.Web.HttpUtility.HtmlEncode(SAMLEndpoint),
                       System.Web.HttpUtility.HtmlEncode(Target),
                       clientID.Text,
                       postedSAML);

                  LoggerManager.Debug(samlResponseXml);
                  LoggerManager.Debug(htmlPosted);

                  Response.Write(htmlPosted);

                  ret = true;
              }
              catch (Exception ex)
              {
                  LoggerManager.Error("Error posting response", ex);
              }
              return ret;
          }


        protected bool PostHtml()
        {
            bool ret = false;

            try
            {
                
                byte[] SAML = System.Text.Encoding.UTF8.GetBytes(_samlResponse.ToXml().OuterXml);
                                
                string Target = "_self";
                string SAMLEndpoint = System.Configuration.ConfigurationManager.AppSettings.Get("SSO_SAMLEndpoint_Url");
                
                string postedSAML = Convert.ToBase64String(SAML);

                if (RadioButtonSSO_SLO.SelectedValue == "SSO")
                    SAMLEndpoint += "sso.aspx";
                else
                    SAMLEndpoint += "slo.aspx";

                string htmlPosted = string.Format(
                     @"<html>" +
                     @"<body onload=""javascript:document.Form.submit()"">" +
                     @"<form action=""{0}"" method=""post"" name=""Form"" target=""_self"">" +
                     @"<input  type=""hidden"" name=""TARGET"" value=""{1}"" />" +
                    //@"<input  type=""hidden"" name=""clientID"" value=""{2}"" />" +
                     @"<input  type=""hidden"" name=""SAMLResponse"" value=""{3}"" />" +
                     @"<noscript><div>" +
                     @"You do not have JavaScript enabled in your browser, or you are " +
                     @"using a browser without JavaScript support." +
                     @"</div><div>" +
                     @"To continue the single sign-on process, click the button below</div>" +
                     @"<button type=""submit"">Continue Authentication</button>" +
                     @"</noscript>" +
                     @"</form></body></html>",
                     System.Web.HttpUtility.HtmlEncode(SAMLEndpoint),
                     System.Web.HttpUtility.HtmlEncode(Target),
                     clientID.Text,
                     postedSAML);

                LoggerManager.Debug(_samlResponse);
                LoggerManager.Debug(htmlPosted);

                Response.Write(htmlPosted);
                
                ret = true;
            }
            catch (Exception ex)
            {
                LoggerManager.Error("Error posting response", ex);
            }
            return ret;

        }


        

        private System.Security.Cryptography.AsymmetricAlgorithm GetPrivateKey()
        {
            
            string filename = Server.MapPath(certificateDir + pfxFileName);
            X509Certificate2 cert = null;
            AsymmetricAlgorithm privateKey = null;
            try
            {
                cert = new X509Certificate2(filename, pfxPassword);                
            }
            catch (Exception ex)
            {
                LoggerManager.Error(string.Format("Error loading certificate->({0}), password->({1})", filename, pfxPassword), ex); 
            }


            if (cert == null)
            {
                throw new ArgumentException
                    ("unable to create certificate from .pfx file: unknown error");
            }
            if (cert.HasPrivateKey)
            {
                //throw new ArgumentException
                //    ("certificate file does not contain a private key " + filename);
                privateKey = cert.PrivateKey;
            }           

            return privateKey;
        }


        protected X509Certificate2 LoadClientCertificate(string certPath)
        {
            X509Certificate2 cert = null;

            try
            {
                cert = LoadCertificate(certPath, null);
            }
            catch (Exception ex)
            {
                LoggerManager.Error(string.Format("SAML: Error in LoadCertificate for certPath ({0}).", certPath), ex);
            }

            return cert;
       
        }



        protected X509Certificate2 LoadCertificate(string certPath, string password)
        {
            X509Certificate2 cert = null;
            
            try
            {
                 cert = new X509Certificate2(certPath, password);
            }
            catch (Exception ex)
            {
                LoggerManager.Error(string.Format("SAML: Error in LoadCertificate for filename ({0}) and password ({1}).", certPath, password), ex);
            }

            return cert;  
        }


     

              
}

