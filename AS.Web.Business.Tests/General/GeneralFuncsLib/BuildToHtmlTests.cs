using AS.Controls.Pages;
using AS.Web.Business.Shared.Enums;
using AS.Web.Business.Shared.Models;

namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class BuildToHtmlTests
    {
        [TestMethod]
        public void BuildToHtml_ReturnHasItem1()
        {
            List<BuildToHtmlModel>? lstData = null;
            string? template = null;
            var result = Business.General.GeneralFuncsLib.BuildToHtml(lstData, template);
            Assert.AreEqual(result.Item1, template);
        }
        [TestMethod]
        public void SplitToArray_ReturnSuccess()
        {
            string templateHtml = "<a class=\"risk-merchant-detail\" href=\"#\" actionModal ='GetMerchantProfile' onclick=\"OpenDetailModal(this);\">[MerchantNumber]</a>";
            var lstData = new List<BuildToHtmlModel>()
        {
            new() { Key = "MerchantNumber" ,  Value = "123456789", EnumToHtml = EnumToHtml.All, Order = 1  },
            new() {Key = "IsHideMenu", Value= "true", Order = 2 }
        };
            var result = Business.General.GeneralFuncsLib.BuildToHtml(lstData, templateHtml);
            Assert.IsNotNull(result.Item1);
            Assert.IsNotNull(result.Item2);
        }
    }
}
