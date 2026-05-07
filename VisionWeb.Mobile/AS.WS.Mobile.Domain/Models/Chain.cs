using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Mobile.Domain.Models
{
    public class Chain
    {
        public int ChainId { get; set; }
        public string ChainName { get; set; }
        public List<Merchant> Merchants { get; set; }

    }
}
