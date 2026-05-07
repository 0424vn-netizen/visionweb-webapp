using Microsoft.Web.Services3;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace AS.Web.Business.PCI.WebServices
{
    [System.Web.Services.WebServiceBinding(Name = "PCIServicesSoap", Namespace = "http://tempuri.org/")]
    public class PciWebServicesClientProtocol : WebServicesClientProtocol
    {
        private readonly NameValueCollection _headers;
        public PciWebServicesClientProtocol()
        {
            _headers = new NameValueCollection();
        }
        public void AddRequestHeader(string name, string value)
        {
            _headers.Add(name, value);
        }
        public void AddRequestHeaders(Dictionary<string, string> dictHeader)
        {
            foreach (var item in dictHeader)
            {
                _headers.Add(item.Key, item.Value);
            }
        }
        public void InsertOrUpdateRequestHeader(string name, string value)
        {
            var dataHearder = _headers.Get(name);
            if (dataHearder == null)
            {
                _headers.Add(name, value);
                return;
            }
            if (dataHearder == value)
            {
                return;
            }
            _headers.Set(name, value);
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest webRequest = base.GetWebRequest(uri);
            for (int i = 0; i < _headers.Count; i++)
            {
                var dataHearder = webRequest.Headers.Get(_headers.Keys[i]);
                if (dataHearder == null)
                {
                    webRequest.Headers.Add(_headers.Keys[i], _headers[i]);
                    continue;
                }
                if (dataHearder == _headers[i])
                {
                    continue;
                }
                _headers.Set(_headers.Keys[i], _headers[i]);
            }
            if (webRequest is HttpWebRequest request)
            {
                request.AutomaticDecompression = DecompressionMethods.GZip;
            }
            return webRequest;
        }
    }
}
