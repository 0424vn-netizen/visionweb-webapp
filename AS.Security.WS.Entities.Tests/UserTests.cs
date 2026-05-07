using AS.Security.WS.Entities;
namespace AS.Security.WS.Entities.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void User_Properties_ReturnValues()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var user = new User()
            {
                RecId = guid,
                IsChangeUserID = true,
                UserPasswordType = 1,
                UserPasswordTmpType = 1,
                LoginAttempts = 1,
                LoginQuestionIndex = 1,
                LoginsM01 = 1,
                LoginsM02 = 1,
                LoginsM03 = 1,
                LoginsM04 = 1,
                LoginsM05 = 1,
                LoginsM06 = 1,
                LoginsM07 = 1,
                LoginsM08 = 1,
                LoginsM09 = 1,
                LoginsM10 = 1,
                LoginsM11 = 1,
                LoginsM12 = 1,
                LoginsYTD = 1,
                LoginsPriorYear = 1,
                Theme_id = 1,
                Status = "Status",
                UserType = 1,
                CreatedBy = guid,
                ReportType = 1,
                UpdatedBy = guid
            };

            // Assert
            Assert.AreEqual(guid, user.RecId);
            Assert.AreEqual(true, user.IsChangeUserID);
            Assert.AreEqual(1, user.UserPasswordType);
            Assert.AreEqual(1, user.UserPasswordTmpType);
            Assert.AreEqual(1, user.LoginAttempts);
            Assert.AreEqual(1, user.LoginQuestionIndex);
            Assert.AreEqual(1, user.LoginsM01);
            Assert.AreEqual(1, user.LoginsM02);
            Assert.AreEqual(1, user.LoginsM03);
            Assert.AreEqual(1, user.LoginsM04);
            Assert.AreEqual(1, user.LoginsM05);
            Assert.AreEqual(1, user.LoginsM06);
            Assert.AreEqual(1, user.LoginsM07);
            Assert.AreEqual(1, user.LoginsM08);
            Assert.AreEqual(1, user.LoginsM09);
            Assert.AreEqual(1, user.LoginsM10);
            Assert.AreEqual(1, user.LoginsM11);
            Assert.AreEqual(1, user.LoginsM12);
            Assert.AreEqual(1, user.LoginsYTD);
            Assert.AreEqual(1, user.LoginsPriorYear);
            Assert.AreEqual(1, user.Theme_id);
            Assert.AreEqual("Status", user.Status);
            Assert.AreEqual(1, user.UserType);
            Assert.AreEqual(guid, user.CreatedBy);
            Assert.AreEqual(1, user.ReportType);
            Assert.AreEqual(guid, user.UpdatedBy);
        }

        [TestMethod]
        public void User_StatusCharProperty_ReturnYes()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var user = new User()
            {
                ActvStat = "1"
            };

            // Assert
            Assert.AreEqual('Y', user.StatusChar);
        }

        [TestMethod]
        public void User_StatusCharProperty_ReturnNo()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var user = new User()
            {
                ActvStat = "0"
            };

            // Assert
            Assert.AreEqual('N', user.StatusChar);
        }
    }
}