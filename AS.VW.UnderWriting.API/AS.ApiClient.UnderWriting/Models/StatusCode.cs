using System.ComponentModel;

namespace AS.ApiClient.UnderWriting.Models
{
    public enum StatusCode
    {
        [Description("Request success.")]
        Success = 1,

        [Description("Request fail.")]
        Fail = 0,

        [Description("Unknown error.")]
        UnknownError = 999,

        [Description("Authentication fail.")]
        AuthenticationFail = 11,

        [Description("Request expired.")]
        RequestExpired = 12,

        [Description("IP not allowed.")]
        IPNotAllowed = 13,

        [Description("Invalid request.")]
        InvalidRequest = 14,

        [Description("Bad request.")]
        BadRequest = 15,

        [Description("Send email failed.")]
        SendEmailFailed = 22,

        [Description("Fiserv validate failed.")]
        FiservValidateFailed = 23,

        [Description("BE api is error.")]
        BEError = 24,

        [Description("Action Failure: Database ADD / UPDATE / DELETE failed")]
        DbFailed = 30,

        [Description("Data input or data state not passing business rule validation")]
        ValidationFailed = 31,

        [Description("No data found.")]
        NoDataFound = 32,

        [Description("API Format Failure.")]
        APIFormatError = 33,

        [Description("Resource Limited.")]
        ResourceLimited = 34,

        [Description("Server validation failed.")]
        CustomValidationFailed = 35
    }
}
