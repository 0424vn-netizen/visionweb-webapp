using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Xml.Serialization;
using AS.Controls.Pages;
using AS.SSO;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Profiles.SSOBrowser;
using ComponentSpace.SAML2.Protocols;

public static class SamlHelper
{
    private static readonly string SsoConfigFile = @"~/App_Data/SAML/SsoConfig.xml";

    public static bool IsSsoMode(SecurePage page)
    {
        if (!"1".Equals(page.Request["sso"]))
            return false;

        if (SessionManager.IsLoggedIn)
        {
            if (GeneralFuncsLib.IsSSOUser(SessionManager.CurrentClient, SessionManager.CurrentUser.UserID))
            {
                // go to dashboard if current login user is SSO user.
                page.Response.Redirect("~/Dashboard.aspx");
            }
            else
            {
                page.Response.Redirect(page.LoginUrl);
            }
        }

        return true;
    }

    public static void SendAuthRequest(HttpContext context)
    {
        var clientConfig = LoadConfig(context.Server.MapPath(SsoConfigFile)).Clients.FirstOrDefault(x => x.Id == SessionManager.CurrentClient);

        if (clientConfig == null)
            throw new Exception("Client does not exists.");

        var certificate = LoadCertificate(
            context.Server.MapPath(clientConfig.Setting.CertificatePath),
            clientConfig.Setting.CertificatePassword
        );

        var authRequest = new AuthnRequest
        {
            AssertionConsumerServiceIndex = int.Parse(clientConfig.Setting.GetExtendSetting("AuthnRequest.AssertionConsumerServiceIndex")),
            AttributeConsumingServiceIndex = int.Parse(clientConfig.Setting.GetExtendSetting("AuthnRequest.AttributeConsumingServiceIndex")),
            Issuer = new Issuer(clientConfig.Setting.GetExtendSetting("AuthnRequest.Issuer.NameId")),
            NameIDPolicy = new NameIDPolicy
            {
                Format = clientConfig.Setting.GetExtendSetting("AuthnRequest.NameIdPolicy.Format"),
                AllowCreate = "1".Equals(clientConfig.Setting.GetExtendSetting("AuthnRequest.NameIdPolicy.AllowCreate")),
                SPNameQualifier = clientConfig.Setting.GetExtendSetting("AuthnRequest.NameIdPolicy.SpNameQualifier")
            }
        };
        ServiceProvider.SendAuthnRequestByHTTPRedirect(
            context.Response,
            clientConfig.Setting.GetExtendSetting("AuthnRequest.IdpUrl"),
            authRequest.ToXml(),
            clientConfig.Setting.GetExtendSetting("AuthnRequest.RelayState"),
            certificate.PrivateKey
        );
    }

    private static SsoConfig LoadConfig(string configFile)
    {
        SsoConfig result;
        var deserializer = new XmlSerializer(typeof(SsoConfig));
        using (var reader = new StreamReader(configFile))
        {
            var obj = deserializer.Deserialize(reader);
            result = obj as SsoConfig;
        }
        return result;
    }

    private static X509Certificate2 LoadCertificate(string certFilePath, string password)
    {
        var result = new X509Certificate2(
            certFilePath,
            password,
            X509KeyStorageFlags.MachineKeySet
        );
        return result;
    }
}