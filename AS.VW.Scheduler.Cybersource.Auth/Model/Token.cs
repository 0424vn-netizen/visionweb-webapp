using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Cybersource.Auth.Model
{
    public class Authorize
    {
        public ClientConfig ClientConfig { get; set; }
        public string RequestData { get; set; }
        public string GmtDateTime { get; set; } = DateTime.Now.ToUniversalTime().ToString("r"); 
        public Authorize(ClientConfig config) 
        {
            ClientConfig = config;
        }
        private string GenerateDigest()
        {
            using (var sha = SHA256.Create())
            {
                var text = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(RequestData)));
                return $"SHA-256={text}";
            }
        }
        public Token GetSignature(string hostName, string method, string resource)
        {
            var requestTarget = $"{method.ToLower()} {resource}";
            StringBuilder header = new StringBuilder();
            StringBuilder signature = new StringBuilder();
            var digest = GenerateDigest();
            var getHeader = "host date (request-target) v-c-merchant-id";
            var postHeader = "host date (request-target) digest v-c-merchant-id";
            var isPost = method.Equals("post", StringComparison.OrdinalIgnoreCase)
                || method.Equals("put", StringComparison.OrdinalIgnoreCase)
                || method.Equals("patch", StringComparison.OrdinalIgnoreCase);

            //Make sure header is order by: 
            //host
            //date
            //(request-target)
            //digest
            //v-c-merchant-id
            header.Append("\nhost: " + hostName)
                .Append("\ndate: " + GmtDateTime)
                .Append("\n(request-target): " + requestTarget);

            if (isPost)
            {
                header.Append("\ndigest: " + digest);
            }

            header.Append("\nv-c-merchant-id: " + ClientConfig.AuthorizationInfo.OrganizationId);

            header.Remove(0, 1);
            byte[] bytes = Encoding.UTF8.GetBytes(header.ToString());
            string text = Convert.ToBase64String(new HMACSHA256(Convert.FromBase64String(ClientConfig.AuthorizationInfo.SecretKey)).ComputeHash(bytes));
            signature.Append("keyid=\"" + ClientConfig.AuthorizationInfo.Key + "\"").Append(", algorithm=\"HmacSHA256\"").Append($", headers=\"{(isPost? postHeader: getHeader)}\"")
                .Append(", signature=\"" + text + "\"");

            return new Token()
            {
                RequestData = this.RequestData,
                Digest = digest,
                Signature = signature.ToString(),
                RawSignature = header.ToString()
            };
        }
    }

    public class Token
    {
        public string Digest { get; set; }
        public string Signature { get; set; }
        public string RequestData { get; set; }
        public string RawSignature { get; set; }
    }    
}
