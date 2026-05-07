using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AS.Tax.Security.Web.Services.SecService
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "SecurityServicesSoap", Namespace = "http://tempuri.org/")]
    public class SecWebServicesClientProtocol : Microsoft.Web.Services3.WebServicesClientProtocol        
    {
        private int _ASClientID = 0;
        public SecWebServicesClientProtocol(int asClientID)
        {
            _ASClientID = asClientID;
        }
        public int ASClientID
        {
            get { return _ASClientID; }
            set { _ASClientID = value; }
        }

        protected override System.Net.WebRequest GetWebRequest(Uri uri)
        {
            System.Net.WebRequest request = base.GetWebRequest(uri);
            request.Headers.Add("ASClientID", _ASClientID.ToString());
            return request;
        }
    }
}
