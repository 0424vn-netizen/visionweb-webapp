using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace AS.VW.Api.Business.WCF
{
    public delegate NameValueCollection MessageHeaderHanlder();
    public delegate void CreateHanlder();

    public class CustomMessageHeader : IClientMessageInspector
    {
        public CustomMessageHeader()

        {
           
        }
        public CustomMessageHeader(MessageHeaderHanlder headerCreator)
        {
            // Method intentionally left empty.
        }
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // Method intentionally left empty.
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            return null;
        }
    }
}