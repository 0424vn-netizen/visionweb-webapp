using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class CheckUserPermissionsTest
    {
        [TestMethod]
        [DataRow(null, null, null, DisplayName = "All param is null")]
        [DataRow(null, new[] { "EditRiskNote", "MSEditRiskNote" }, false, DisplayName = "Param currentUserPermissions is null")]
        [DataRow(",EditRiskNote,DeleteRiskNote,MSEditRiskNote,MSDeleteRiskNote,", null, false, DisplayName = "Param codePermissions is null")]
        [DataRow(",EditRiskNote,DeleteRiskNote,MSEditRiskNote,MSDeleteRiskNote,", new[] { "EditRiskNote", "MSEditRiskNote" }, true, DisplayName = "Result data is success")]
        public void CheckUserPermissions_ToTest(string currentUserPermissions, string[] codePermissions, bool? expected)
        {
            char separator = ',';
            var result = Business.General.GeneralFuncsLib.CheckUserPermissions(currentUserPermissions, separator, codePermissions?.ToList());
            if (expected is null)
            {
                Assert.IsFalse(result);
                return;
            }
            Assert.AreEqual(expected, result);
        }
    }
}
