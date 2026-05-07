using AS.Web.Business.Shared.Enums;

namespace AS.Web.Business.Shared.Models
{
    public class BuildToHtmlModel: MappingTemplateByOrderModel
    {
        public EnumToHtml EnumToHtml { get; set; }
    }

    public class MappingTemplateByOrderModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
    }
}
