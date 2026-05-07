using System;
using System.Xml;
using ComponentSpace.SAML2.Profiles.SSOBrowser;

public partial class freeaccess_SPSimulator : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        XmlElement samlRequestXml;
        try
        {
            string relayState = null;
            bool signed = false;
            IdentityProvider.ReceiveAuthnRequestByHTTPRedirect(Request, out samlRequestXml, out relayState, out signed, null);
        }
        catch (Exception)
        {
            samlRequestXml = null;
        }

        if (samlRequestXml != null)
            txtRawAuthnRequest.Text = System.Xml.Linq.XDocument.Parse(samlRequestXml.OuterXml).ToString();
    }
}