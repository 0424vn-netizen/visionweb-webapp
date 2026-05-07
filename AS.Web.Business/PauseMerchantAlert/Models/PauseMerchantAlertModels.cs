using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Web.Business.PauseMerchantAlert.Models
{
    public class MerchantRefModel
    {
        public string DataKey { get; set; }

        public string DataText { get; set; }
    }

    public class PauseMerchantAlertResponse
    {
        public Array Data { get; set; }

        public int Count { get; set; }

        public string Errors { get; set; }

        public PauseMerchantAlertResponse(Array data, int count)
        {
            this.Data = data;
            this.Count = count;
        }

        public PauseMerchantAlertResponse(string errors)
        {
            this.Errors = errors;
        }

        public PauseMerchantAlertResponse()
        {

        }
    }

    public class Filter
    {
        public string Value { get; set; }

        public string Field { get; set; }

        public string Operator { get; set; }

        public bool IgnoreCase { get; set; }
    }

    public class FilterContainer
    {
        public string Logic { get; set; }

        public List<Filter> Filters { get; set; }
    }

    public class RequestObject
    {
        public int Take { get; set; }

        public int Skip { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public FilterContainer Filter { get; set; }

        public string[] SelectedDataKeys { get; set; }
    }
}
