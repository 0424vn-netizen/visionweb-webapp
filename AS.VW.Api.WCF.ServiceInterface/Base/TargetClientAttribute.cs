using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS.VW.Api.WCF.ServiceInterface
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class TargetClientAttribute : Attribute
    {
        public int clientId { get; set; }
    }
}