using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupergoo.ABCpdf9;

namespace AS.VW.Common
{
    public class AbcpdfDoc : Doc
    {
        public AbcpdfDoc(bool engineGecko = true)
            : base()
        {
            if (engineGecko)
            {
                base.HtmlOptions.Engine = EngineType.Gecko;
            }
        }
    }
}
