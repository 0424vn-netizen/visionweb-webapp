using AS.Controls.Validators;
using AS.Security.WS.Entities;
using System.Reflection;

namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class BuildPermissionCollectionWithOrderByGroupNameTests
    {
        [TestMethod]
        [DynamicData(nameof(TestnData), DynamicDataDisplayName = nameof(GetDisplayName))]
        public void BuildPermissionCollectionWithOrderByGroupName_ToTest(PermissionCollection pers, int expected)
        {
            var result = Business.General.GeneralFuncsLib.BuildPermissionCollectionWithOrderByGroupName(pers);
            Assert.AreEqual(expected, result?.Count);
        }

        public static IEnumerable<object[]> TestnData => [
            [null, 0],
            [ new PermissionCollection {
             new Permission {
                    SystemId = 1,
                    PermissionId = 679,
                    Description = "Edit Risk Note",
                    Group ="cs",
                    GroupFuncName ="RiskAccess",
                    PermissionCode ="EditRiskNote",
                    PermissionCodesRequire= "RskRP",
                    NodeOrder = 8
                },
                new Permission {
                    SystemId = 1,
                    PermissionId = 680,
                    Description = "Delete Risk Note",
                    Group ="cs",
                    GroupFuncName ="RiskAccess",
                    PermissionCode ="DeleteRiskNote",
                    PermissionCodesRequire= "RskRP",
                    NodeOrder = 10
                }

            }, 2 ]
        ];

        public static string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            var inputs = (PermissionCollection?)data[0];
            return string.Format("Test method {0} with input {1} data.", methodInfo.Name, inputs == null ?"is null":"is not null");
        }

    }
}
