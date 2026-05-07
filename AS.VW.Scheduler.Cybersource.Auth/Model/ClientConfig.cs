using System;
using System.Collections.Generic;

namespace AS.VW.Scheduler.Cybersource.Auth.Model
{
    public class ClientConfig: ICloneable
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public string BaseApiUrl { get; set; }

        public string SearchApiUrl { get; set; }    

        public string DetailApiUrl { get; set; }

        public int Limit { get; set; }

        public string Timezone { get; set; }

        public string Sort { get; set; }

        public string Query { get; set; }

        public string CardDummy { get; set; }

        public string PrefixFileName { get; set; }

        public string SpaName { get; set; }

        public bool LogSummaryFile { get; set; }

        public AuthorizationInfo AuthorizationInfo { get; set; }

        public string CredentialPath { get; set; }

        public DateTime? ReportDate { get; set; }

        public DateTime? ReportDateRun { get; set; }

        public DateTime? TransactionDate { get; set; }

        public bool IsReRun { get; set; }

        public bool IsNormal { get; set; }

        public bool IsRunByHour { get; set; }

        public List<AuthLogModel> AuthLogs { get; set; }

        public ResponseStorageConfig ResponseStorageConfig { get; set; }

        public object Clone()
        {
            return new ClientConfig
            {
                ClientId = ClientId,
                ClientName = ClientName,
                BaseApiUrl = BaseApiUrl,
                SearchApiUrl = SearchApiUrl,
                DetailApiUrl = DetailApiUrl,
                Limit = Limit,
                Timezone = Timezone,
                Sort = Sort,
                Query = Query,
                CardDummy = CardDummy,
                PrefixFileName = PrefixFileName,
                SpaName = SpaName,
                LogSummaryFile = LogSummaryFile,
                CredentialPath = CredentialPath,
                ReportDate = ReportDate,
                IsReRun = IsReRun,
                IsNormal = IsNormal,
                AuthLogs = AuthLogs,
                AuthorizationInfo = AuthorizationInfo,
                TransactionDate = TransactionDate,
                ReportDateRun = ReportDateRun,
                IsRunByHour = IsRunByHour,
                ResponseStorageConfig = ResponseStorageConfig
            };
        }
    }

    public class AuthorizationInfo
    {
        public int ClientId { get; set; }

        public string OrganizationId { get; set; }

        public string Key { get; set; }

        public string SecretKey { get; set; }
    }
}
