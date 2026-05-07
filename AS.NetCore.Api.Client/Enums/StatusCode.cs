using System;
using System.ComponentModel;

namespace AS.NetCore.Api.Client.Enums
{
    public enum StatusCode
    {
        [Description("Request success.")]
        Success = 1,

        [Description("Request fail.")]
        Fail = 0,
    }
}
