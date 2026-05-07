using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace AS.VW.Api.WCF
{
    public static class WebApiSettings
    {
        public static string ApiWsdlServiceNamespace
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiWsdlServiceNamespace"];
            }
        }

        public static string ApiServiceName
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiServiceName"];
            }
        }

        public static string ApiAuthenticationRedirectUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiAuthenticationRedirectUrl"];
            }
        }

        public static string ApiDefaultVersion
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiDefaultVersion"];
            }
        }

        public static bool ApiEnableRequestTracking
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiEnableRequestTracking"] == "1";
            }
        }

        public static int DefaultAsClientId
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["DefaultAsClientId"]);
            }
        }

        public const string PERMISSION_REPORTING = "Reports";
        public const string PERMISSION_AUTHOR = "AuthLogRpt";
        public const string PERMISSION_BATCH = "BatchRpt";
        public const string PERMISSION_CHARGEBACK = "ChbRpt";
        public const string PERMISSION_RETURN = "ReturnRpt";
        public const string PERMISSION_RETRIEVAL = "RetRpt";
        public const string PERMISSION_TRANSACTION = "CardSearchRpt";

        public const string PERMISSION_PAYMENT = "PaymentRpt";

        public const string PERMISSION_MIF = "MerInf";
        public const string PERMISSION_STATEMENTREPORT = "StatementReport";
    }
}