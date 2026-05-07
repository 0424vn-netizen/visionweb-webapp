using AS.Common.DBManager;
using AS.Core.Common.Log;
using AS.VW.Scheduler.Cybersource.Auth.Client;
using AS.VW.Scheduler.Cybersource.Auth.Model;
using AS.Web.Business;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace AS.VW.Scheduler.Cybersource.Auth.Business
{
    public class AuthBusiness
    {
        #region Properties
        private readonly ILog LogManager = Logger.GetLogger(typeof(CybersourceClient));
        public bool IsMock { get; set; }
        public bool IsDebug { get; set; }
        public ReportServices Service { get; set; }
        public ClientConfig ClientConfig { get; set; }
        private DateTime ReportDate { get; set; } = DateTime.Now;
        private DateTime ReportDateRun { get; set; } = DateTime.Now;
        private int ProcessedTotalNumber = 0;
        private int TotalRecords = 0;
        private readonly StringBuilder InvalidData = new StringBuilder();
        private readonly CybersourceClient Client;
        private FileLogger FileLogger { get; set; }
        #endregion

        #region Method
        public AuthBusiness(ReportServices service, ClientConfig config, bool isMock, bool isDebug)
        {
            Service = service;
            IsMock = isMock;
            IsDebug = isDebug;
            ClientConfig = config;
            FileLogger = new FileLogger(config);
            Client = new CybersourceClient(ClientConfig, LogManager);

            if (config.ReportDate.HasValue)
                ReportDate = config.ReportDate.Value;

            if (config.ReportDateRun.HasValue)
                ReportDateRun = config.ReportDateRun.Value;
        }
        public void ProcessAuthData()
        {
            LogManager.Info($"Client: {ClientConfig.ClientId} - Begin get {(IsMock ? "Mock data" : "auth api data - ReportDate")}: {ReportDate.ToString("yyyyMMdd_HH:mm:ss")}");
            var request = new RequestModel()
            {
                Limit = ClientConfig.Limit,
                Sort = ClientConfig.Sort,
                Query = ClientConfig.Query,
                Timezone = ClientConfig.Timezone
            };

            var isGetData = true;
            var currentIndex = 1;
            bool isReRun = false;
            while (isGetData)
            {
                var (authData, rawResponse) = IsMock ? GetMockData(request) : GetAuthApiData(request, ref isReRun);

                if (ClientConfig.IsReRun && !isReRun)
                    return;

                if (!authData.Any())
                {
                    isGetData = false;
                    // Process save DB to write log
                    SaveAuthData(currentIndex);
                }
                else
                {
                    if (authData.Count < request.Limit)
                    {
                        isGetData = false;
                    }

                    var detailsCache = GetAllTransactionDetails(authData, currentIndex);
                    if (!string.IsNullOrEmpty(rawResponse))
                    {
                        SaveRawDataFromCache(rawResponse, detailsCache, currentIndex);
                    }

                    SaveAuthDataFromCache(authData, detailsCache, currentIndex);

                    // Save data
                    ProcessedTotalNumber += authData.Count;
                    request.Offset += request.Limit;
                    currentIndex++;
                }
            }

            if (ClientConfig.LogSummaryFile)
            {
                //write invalid log
                FileLogger.WriteInvalidFile(InvalidData.ToString());

                //write summary log
                FileLogger.WriteSummaryFile(TotalRecords, ProcessedTotalNumber, 0);
            }

            LogManager.Info($"Total Records from Api: {TotalRecords}");
            LogManager.Info("End get auth api data");
        }

        public (List<Transactionsummary>, string rawResponse) GetAuthApiData(RequestModel data, ref bool isReRun)
        {
            var authorize = new Authorize(ClientConfig)
            {
                RequestData = data.ToJson()
            };

            var request = new RestRequest(ClientConfig.SearchApiUrl, Method.POST);
            var token = authorize.GetSignature(Client.BaseUrl.Host, request.Method.ToString().ToLower(), request.Resource);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("v-c-merchant-id", ClientConfig.AuthorizationInfo.OrganizationId);
            request.AddHeader("Date", authorize.GmtDateTime);
            request.AddHeader("Host", Client.BaseUrl.Host);
            request.AddHeader("Digest", token.Digest);
            request.AddHeader("Signature", token.Signature);
            request.AddJsonBody(authorize.RequestData);

            var response = Client.Execute(request);
            var statusCode = response.StatusCode;
            if (response.IsSuccessful)
            {
                var responseData = JsonConvert.DeserializeObject<AuthData>(response.Content);

                if (TotalRecords == 0)
                    TotalRecords = responseData.TotalCount;

                // Check data need re-Run
                if (ClientConfig.IsReRun)
                {
                    var itemSelected = ClientConfig.AuthLogs.FirstOrDefault(x => x.ReportDate == ReportDateRun);
                    isReRun = itemSelected == null || (!itemSelected.IsValid || itemSelected.TransactionCount != TotalRecords || itemSelected.TransactionCount == 0);
                }

                if (responseData?.Transactions?.TransactionSummaries?.Any() == true)
                {
                    return (responseData.Transactions.TransactionSummaries, response.Content);
                }
            }

            if ((int)statusCode < 200 && (int)statusCode > 299)
            {
                LogManager.Error($"Call GetAuthApiData error: {statusCode}{Environment.NewLine}{response.ErrorMessage}");
            }

            return (new List<Transactionsummary>(), null);
        }

        private Dictionary<string, TransactionDetailCache> GetAllTransactionDetails(List<Transactionsummary> transactions, int batchNumber)
        {
            var cache = new Dictionary<string, TransactionDetailCache>();
            LogManager.Info($"Batch {batchNumber}: Fetching details for {transactions.Count} transactions...");
            int successCount = 0;
            int errorCount = 0;
            foreach (var transaction in transactions)
            {
                try
                {
                    var authorize = new Authorize(ClientConfig)
                    {
                        RequestData = transaction.Id
                    };

                    var request = new RestRequest($"{ClientConfig.DetailApiUrl}/{transaction.Id}", Method.GET);
                    var token = authorize.GetSignature(Client.BaseUrl.Host, request.Method.ToString().ToLower(), request.Resource);

                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("v-c-merchant-id", ClientConfig.AuthorizationInfo.OrganizationId);
                    request.AddHeader("Date", authorize.GmtDateTime);
                    request.AddHeader("Host", Client.BaseUrl.Host);
                    request.AddHeader("Signature", token.Signature);
                    var response = Client.Execute(request);
                    var statusCode = response.StatusCode;

                    if ((int)statusCode < 200 || (int)statusCode > 299)
                    {
                        LogManager.Error($"Call GetTransactionDetail error for {transaction.Id}: {statusCode}{Environment.NewLine}{response.ErrorMessage}");
                        errorCount++;
                        continue;
                    }

                    if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                    {
                        cache[transaction.Id] = new TransactionDetailCache
                        {
                            RawResponse = response.Content,
                            ParsedDetail = JsonConvert.DeserializeObject<TransactionDetail>(response.Content)
                        };
                        successCount++;
                    }
                    else
                    {
                        LogManager.Warn($"Empty response for transaction {transaction.Id}");
                        errorCount++;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Error($"Error fetching detail for transaction {transaction.Id}: {ex.Message}");
                    errorCount++;
                }
            }

            LogManager.Info($"Batch {batchNumber}: Successfully fetched {successCount} details, {errorCount} errors");
            return cache;
        }

        public (List<Transactionsummary>, string rawResponse) GetMockData(RequestModel request)
        {
            var RootPath = AppDomain.CurrentDomain.BaseDirectory;
            var MockPath = "/App_Data/Mock/response.json";
            var mockFile = Path.GetFullPath($"{RootPath}{MockPath}");
            var authData = new List<Transactionsummary>();
            var response = string.Empty;

            if (File.Exists(mockFile))
            {
                response = File.ReadAllText(mockFile);
                var data = JsonConvert.DeserializeObject<AuthData>(response);

                if (TotalRecords == 0)
                    TotalRecords = data.TotalCount;

                if (data != null && data.Transactions != null && data.Transactions?.TransactionSummaries.Any() == true)
                {
                    authData = data.Transactions.TransactionSummaries;
                }
            }

            authData = authData.Skip(request.Limit * request.Offset).Take(request.Limit).ToList();

            foreach (var item in authData)
            {
                item.SubmitTimeUtc = ReportDate.AddHours(-0.5);
            }
            return (authData, response);
        }

        public void SaveAuthData(int batchNumber)
        {
            SaveDataModel result = new SaveDataModel();
            result.OriginalFileName = $"{GetOriginalFileName()}_{batchNumber}";
            result.TransactionDate = ReportDate.ToUniversalTime().ToShortDateString();
            result.IsValid = true;
            SaveData(result);
        }

        public void SaveData(SaveDataModel result)
        {
            //save to db
            if (!string.IsNullOrEmpty(ClientConfig.SpaName))
            {
                DateTime reportDate = AdjustReportDate(ClientConfig, ReportDate);
                FilterParameterCollection outputParam;
                FilterParameterCollection inputParam = new FilterParameterCollection
                    {
                        new FilterParameter("@ASClientID", ClientConfig.ClientId, DbType.Int32),
                        new FilterParameter("@ReportDate", reportDate, DbType.Date),
                        new FilterParameter("@OriginalFileName", result.OriginalFileName, DbType.String),
                        new FilterParameter("@Json", result.JsonData, DbType.String),
                        new FilterParameter("@TransactionDate", result.TransactionDate, DbType.Date),
                        new FilterParameter("@TransactionCount", result.TransactionCount, DbType.Int64),
                        new FilterParameter("@IsValid", result.IsValid, DbType.Boolean),
                        new FilterParameter("@IsReRun", ClientConfig.IsReRun, DbType.Boolean),
                    };

                Service.ExecuteNonQueryCommand(ClientConfig.SpaName, inputParam, out outputParam);
            }
        }

        private AuthViewModel ConvertAuthData(TransactionDetail source, Transactionsummary summary, DateTime reportDate)
        {
            var result = new AuthViewModel();
            result.Id = source.Id;
            result.ReportDate = reportDate.ToString("yyyy-MM-dd");
            result.ReportTime = reportDate.ToString("HH:mm:ss");
            result.MerchantId = source.MerchantId;
            result.TerminalId = source.PointOfSaleInformation?.TerminalId;
            result.CardType = source.PaymentInformation?.Card?.Type;
            result.CardMethod = source?.PaymentInformation?.PaymentType?.Method;
            result.CardMethodDescription = source?.PaymentInformation?.PaymentType?.Type;
            result.MSAccountNumber = summary.PaymentInformation?.Card?.Suffix;
            result.BinNumber = summary.PaymentInformation?.Card?.Prefix;

            var cardNumber = result.MSAccountNumber + result.BinNumber;
            if (!string.IsNullOrEmpty(cardNumber))
            {
                var accountNumber = $"{result.BinNumber}{ClientConfig.CardDummy}{result.MSAccountNumber}";
                result.AccountNumber = accountNumber;
                result.Bin8Number = accountNumber.Length >= 8 ? accountNumber.Substring(0, 8) : accountNumber;
            }

            var expirationMonth = source.PaymentInformation?.Card?.ExpirationMonth;
            var expirationYear = source.PaymentInformation?.Card?.ExpirationYear;

            if (!string.IsNullOrEmpty(expirationMonth) && !string.IsNullOrEmpty(expirationYear))
            {
                var year = expirationYear.Trim();
                year = year.Length >= 2 ? year.Substring(year.Length - 2, 2) : year;
                result.ExpirationDate = int.Parse(expirationMonth.Trim()).ToString("D2") + "/" + year;
            }

            result.TransactionDate = source.SubmitTimeUtc?.ToString("yyyy-MM-dd");
            result.TransactionTime = source.SubmitTimeUtc?.ToString("HH:mm:ss");

            if (!string.IsNullOrEmpty(source.OrderInformation?.AmountDetails?.AuthorizedAmount)
                && decimal.TryParse(source.OrderInformation?.AmountDetails?.AuthorizedAmount, out decimal amount))
                result.TotalAmount = amount;

            result.ReferenceNumber = source.ReconciliationId;
            result.AuthorizationNumber = source.ProcessorInformation?.ApprovalCode;
            result.ResponseCode = source.ProcessorInformation?.ResponseCode;
            result.EntryMode = source.PointOfSaleInformation?.EntryMode;
            result.Currency = source.OrderInformation?.AmountDetails?.Currency;
            result.CVV2ResponseCode = source.ProcessorInformation?.CardVerification?.ResultCode;
            result.AVSCode = source.ProcessorInformation?.Avs?.Code;
            result.AVSCodeRaw = source.ProcessorInformation?.Avs?.CodeRaw;
            result.TransactionID = source.ProcessorInformation?.TransactionId;
            result.ApplicationInformation = source.ApplicationInformation;
            result.IsValid = CheckValidRecord(result);

            return result;
        }

        private bool CheckValidRecord(AuthViewModel authItem)
        {
            return !string.IsNullOrEmpty(authItem.TerminalId) &&
                !string.IsNullOrEmpty(authItem.CardMethod) &&
                authItem.TotalAmount != null &&
                !string.IsNullOrEmpty(authItem.AuthorizationNumber) &&
                !string.IsNullOrEmpty(authItem.ReferenceNumber) &&
                !string.IsNullOrEmpty(authItem.BinNumber) &&
                !string.IsNullOrEmpty(authItem.MSAccountNumber) &&
                !string.IsNullOrEmpty(authItem.Currency);
        }

        private void GetHashValue(List<AuthViewModel> authData)
        {
            if (authData != null && authData.Any())
            {
                Security.Web.StatServices.StatEncryptService stat = new Security.Web.StatServices.StatEncryptService();
                var accountNumbers = authData.Where(x => !string.IsNullOrEmpty(x.AccountNumber)).Select(x => x.AccountNumber).Distinct().ToList();
                var bin8Numbers = authData.Where(x => !string.IsNullOrEmpty(x.Bin8Number)).Select(x => x.Bin8Number).Distinct().ToList();
                var hashedAccountNumbers = stat.GetHashValues(accountNumbers);
                var hashedBin8Numbers = stat.GetHashValues(bin8Numbers);
                var encryptedValues = stat.GetEncryptOldTokenValues(accountNumbers);

                foreach (var item in authData)
                {
                    if (!string.IsNullOrEmpty(item.AccountNumber))
                    {
                        var hashValue = hashedAccountNumbers.Rows.Cast<DataRow>().FirstOrDefault(x => x["RawValue"].ToString().Equals(item.AccountNumber));
                        var encryptedValue = encryptedValues.Rows.Cast<DataRow>().FirstOrDefault(x => x["RawValue"].ToString().Equals(item.AccountNumber));
                        var hashBin8Value = hashedBin8Numbers.Rows.Cast<DataRow>().FirstOrDefault(x => x["RawValue"].ToString().Equals(item.Bin8Number));

                        if (hashValue != null)
                            item.HashedAccountNumber = hashValue["Value"].ToString();

                        if (encryptedValue != null)
                            item.EncryptedAccountNumber = encryptedValue["Value"].ToString();

                        if (hashBin8Value != null)
                            item.HashedBin8Number = hashBin8Value["Value"].ToString();
                    }
                }
            }
        }

        public string GetOriginalFileName()
        {
            if (ClientConfig.IsNormal)
                return $"{ClientConfig.PrefixFileName}{ReportDate.AddHours(-1).ToString("yyyyMMddHH")}0000_{ReportDate.AddHours(-1).ToString("yyyyMMddHH")}5959";

            return $"{ClientConfig.PrefixFileName}{ReportDateRun.ToString("yyyyMMddHH")}0000_{ReportDateRun.ToString("yyyyMMddHH")}5959";
        }

        private DateTime AdjustReportDate(ClientConfig config, DateTime reportDate)
        {
            int hour = reportDate.Hour;
            if ((config.IsNormal && hour >= 17) || (config.IsRunByHour && hour >= 16))
                return ReportDate.AddDays(1);
            return reportDate;
        }

        private void SaveRawDataFromCache(string listResponse, Dictionary<string, TransactionDetailCache> detailsCache, int batchNumber)
        {
            try
            {
                if (ClientConfig.ResponseStorageConfig != null && ClientConfig.ResponseStorageConfig.EnableStorage)
                {
                    if (!Directory.Exists(ClientConfig.ResponseStorageConfig.StorageDirectory))
                    {
                        Directory.CreateDirectory(ClientConfig.ResponseStorageConfig.StorageDirectory);
                    }

                    LogManager.Info($"Start saving raw data for batch {batchNumber} with {detailsCache.Count} details");

                    var rawData = new RawDataBatch
                    {
                        List = JsonConvert.DeserializeObject(listResponse)
                    };

                    foreach (var kvp in detailsCache)
                    {
                        try
                        {
                            var transactionId = kvp.Key;
                            var rawDetailResponse = kvp.Value.RawResponse;
                            if (!string.IsNullOrEmpty(rawDetailResponse))
                            {
                                var detailObject = JsonConvert.DeserializeObject(rawDetailResponse);
                                rawData.Details.Add(new Dictionary<string, object>
                                {
                                    { $"detail_{transactionId}", detailObject }
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            LogManager.Error($"Error adding raw detail to collection: {ex.Message}");
                        }
                    }

                    var fileName = Path.Combine(ClientConfig.ResponseStorageConfig.StorageDirectory, $"{ClientConfig.ClientName}_{GetOriginalFileName()}_{DateTime.Now:yyyyMMddHHmmss}_{batchNumber}.json");

                    var jsonSettings = new JsonSerializerSettings
                    {
                        Formatting = Newtonsoft.Json.Formatting.Indented,
                        NullValueHandling = NullValueHandling.Include,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };

                    File.WriteAllText(fileName, JsonConvert.SerializeObject(rawData, jsonSettings));
                   

                    if (ClientConfig.ResponseStorageConfig.EnableEncryption)
                    {
                        FileEncryption fileEncryption = new FileEncryption(ClientConfig.ResponseStorageConfig.SaltKeyFilePath);
                        fileEncryption.EncryptFile(fileName);
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Error($"Error saving raw data batch {batchNumber}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        private AuthorizationData PrepareDataToSaveFromCache(List<Transactionsummary> data, Dictionary<string, TransactionDetailCache> detailsCache, int batchNumber)
        {
            var authData = new List<AuthViewModel>();

            if (data != null && data.Any())
            {
                DateTime reportDate = AdjustReportDate(ClientConfig, ReportDate);
                foreach (var item in data)
                {
                    if (detailsCache.ContainsKey(item.Id))
                    {
                        var transDetail = detailsCache[item.Id].ParsedDetail;
                        if (transDetail != null)
                        {
                            var authViewModel = ConvertAuthData(transDetail, item, reportDate);
                            authData.Add(authViewModel);
                        }
                    }
                }

                GetHashValue(authData);
            }

            return new AuthorizationData()
            {
                Auths = authData,
                OriginalFileName = GetOriginalFileName()
            };
        }

        public void SaveAuthDataFromCache(List<Transactionsummary> data, Dictionary<string, TransactionDetailCache> detailsCache, int batchNumber)
        {
            if (data?.Any() == true)
            {
                LogManager.Info($"Batch {batchNumber}: Preparing processed data...");
                var dataToSave = PrepareDataToSaveFromCache(data, detailsCache, batchNumber);
                dataToSave.OriginalFileName = $"{dataToSave.OriginalFileName}_{batchNumber}";

                // Save to db
                if (!string.IsNullOrEmpty(ClientConfig.SpaName))
                {
                    var jsonData = JsonConvert.SerializeObject(dataToSave);
                    SaveDataModel result = new SaveDataModel();
                    result.JsonData = jsonData;
                    result.OriginalFileName = dataToSave.OriginalFileName;
                    result.TransactionDate = dataToSave.Auths[0].TransactionDate;
                    result.TransactionCount = dataToSave.Auths.Count;
                    result.IsValid = dataToSave.Auths.All(x => x.IsValid);

                    SaveData(result);
                    LogManager.Info($"Batch {batchNumber}: Processed data saved - {result.TransactionCount} transactions");
                }
            }
        }

        #endregion   
    }
}
