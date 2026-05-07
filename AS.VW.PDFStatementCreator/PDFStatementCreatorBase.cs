using AS.VW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.PDFStatementCreator
{
    public abstract partial class PdfStatementCreatorBase : IPdfStatementCreator
    {
        public string CurrentCulture { get; set; }

        public PdfStatementResultType ForceReturnType { get; set; }

        public virtual string GetResourceString(string key) {
            var rm = new ResourceManager(this.GetType().Name+"_Resource",this.GetType().Assembly);
            return rm.GetString(key,new System.Globalization.CultureInfo(CurrentCulture));
        }

        public abstract PdfStatementCreatorResult Execute(object parameterContent, int? asClientId = null, int? siteId = null, string userId = "", string userMode = "", DateTime? reportDate = null);
    }
}
