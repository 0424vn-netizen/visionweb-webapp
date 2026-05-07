using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Mobile.Domain.Models
{
    public class Merchant
    {
        public string Entity { get; set; }
        public string EntityName { get; set; }
    }

    public class Merchants
    {
        public List<Merchant> Items { get; set; }

        public Merchants()
        {
            Items = new List<Merchant>();
        }
    }
}
