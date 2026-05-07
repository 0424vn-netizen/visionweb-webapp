using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Filter
{
    [DataContract]
    public class GenericReportFilter : GenericReportFilterNoViewLevel,IDataViewLevelFilter
    {
        [DataMember]
        public string ViewLevel {get;set;}

        public DataViewLevel ViewLevelValidate
        {
            get
            {
                if(string.IsNullOrEmpty(ViewLevel))
                    return DataViewLevel.None;

                if (ViewLevel.ToLower().Equals("merchant"))
                    return DataViewLevel.Merchant;
                if (ViewLevel.ToLower().Equals("hierarchy"))
                    return DataViewLevel.Hierarchy;
                return DataViewLevel.None;

            }
            set { ViewLevel = value.ToString(); }
        }
    }
}
