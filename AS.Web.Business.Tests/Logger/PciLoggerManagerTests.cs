using AS.Common.DBManager;
using AS.Web.Business.Logger;
using log4net;
using Moq;
using System.Data;

namespace AS.Web.Business.Tests.Logger
{
    [TestClass]
    public class PciLoggerManagerTests
    {
        private Mock<ILog>? mockLog;

        [TestInitialize]
        public void Setup()
        {
            mockLog = new Mock<ILog>();
            PciLoggerManager.SetLogger(mockLog.Object);
        }

        [TestMethod]
        public void Info_CallsILogInfo()
        {
            var msg = "test";
            PciLoggerManager.Info(msg);
            mockLog?.Verify(l => l.Info(msg), Times.Once);
        }

        [TestMethod]
        public void Debug_CallsILogDebug()
        {
            var msg = "debug";
            PciLoggerManager.Debug(msg);
            mockLog?.Verify(l => l.Debug(msg), Times.Once);
        }

        [TestMethod]
        public void Error_CallsILogError()
        {
            var msg = "error";
            PciLoggerManager.Error(msg);
            mockLog?.Verify(l => l.Error(msg), Times.Once);
        }

        [TestMethod]
        public void Error_WithException_CallsILogErrorWithException()
        {
            var msg = "error";
            var ex = new Exception("ex");
            PciLoggerManager.Error(msg, ex);
            mockLog?.Verify(l => l.Error(It.Is<string>(s => s.StartsWith(msg)), ex), Times.Once);
        }

        [TestMethod]
        public void Warn_CallsILogWarn()
        {
            var msg = "warn";
            PciLoggerManager.Warn(msg);
            mockLog?.Verify(l => l.Warn(msg), Times.Once);
        }

        [TestMethod]
        public void Fatal_CallsILogFatal()
        {
            var msg = "fatal";
            PciLoggerManager.Fatal(msg);
            mockLog?.Verify(l => l.Fatal(msg), Times.Once);
        }

        [TestMethod]
        public void Info_ByParams()
        {
            string msg = "Info message for some params";
            string method = "GetReports";
            string spName = "spa_MULTI_PCI_GetMasterMerchant";
            FilterParameterCollection filterParams = new FilterParameterCollection
            {
                new FilterParameter("@PrimaryUserID", "100", DbType.AnsiString),
                new FilterParameter("@ASClient", 217, DbType.Int32)
            };
            PciLoggerManager.Info(msg, method, spName, filterParams);
        }      
       
        [TestMethod]
        public void Debug_ByParams()
        {
            string msg = "Debug message for some params";
            string method = "GetReports";
            string spName = "spa_MULTI_PCI_GetMasterMerchant";
            FilterParameterCollection filterParams = new FilterParameterCollection
            {
                new FilterParameter("@PrimaryUserID", "100", DbType.AnsiString),
                new FilterParameter("@ASClient", 217, DbType.Int32)
            };
            PciLoggerManager.Debug(msg, method, spName, filterParams);
        }
        [TestMethod]
        public void Error_ByParams()
        {
            string msg = "Error message for some params";
            string method = "GetReports";
            string spName = "spa_MULTI_PCI_GetMasterMerchant";
            FilterParameterCollection filterParams = new FilterParameterCollection
            {
                new FilterParameter("@PrimaryUserID", "100", DbType.AnsiString),
                new FilterParameter("@ASClient", 217, DbType.Int32)
            };
            PciLoggerManager.Error(msg, method, spName, filterParams);
        }
        [TestMethod]
        public void Warn_ByParams()
        {
            string msg = "Warn message for some params";
            string method = "GetReports";
            string spName = "spa_MULTI_PCI_GetMasterMerchant";
            FilterParameterCollection filterParams = new FilterParameterCollection
            {
                new FilterParameter("@PrimaryUserID", "100", DbType.AnsiString),
                new FilterParameter("@ASClient", 217, DbType.Int32)
            };
            PciLoggerManager.Warn(msg, method, spName, filterParams);
        }
        [TestMethod]
        public void Fatal_ByParams()
        {
            string msg = "Fatal message for some params";
            string method = "GetReports";
            string spName = "spa_MULTI_PCI_GetMasterMerchant";
            FilterParameterCollection filterParams = new FilterParameterCollection
            {
                new FilterParameter("@PrimaryUserID", "100", DbType.AnsiString),
                new FilterParameter("@ASClient", 217, DbType.Int32)
            };
            PciLoggerManager.Fatal(msg, method, spName, filterParams);
        }

        [TestMethod]
        public void Info_ByParams_Exception()
        {
            string msg = "Info message for some params with exception";
            string method = "GetReports";
            string spName = "spa_MULTI_PCI_GetMasterMerchant";
            FilterParameterCollection filterParams = new FilterParameterCollection
            {
                new FilterParameter("@PrimaryUserID", "100", DbType.AnsiString),
                new FilterParameter("@ASClient", 217, DbType.Int32)
            };
            var ex = new Exception("ex");
            PciLoggerManager.Info(msg, method, spName, filterParams, ex);
        }
        [TestMethod]
        public void Debug_ByParams_Exception()
        {
            string msg = "Debug message for some params with exception";
            string method = "GetReports";
            string spName = "spa_MULTI_PCI_GetMasterMerchant";
            FilterParameterCollection filterParams = new FilterParameterCollection
            {
                new FilterParameter("@PrimaryUserID", "100", DbType.AnsiString),
                new FilterParameter("@ASClient", 217, DbType.Int32)
            };
            var ex = new Exception("ex");
            PciLoggerManager.Debug(msg, method, spName, filterParams, ex);
        }
        [TestMethod]
        public void Error_ByParams_Exception()
        {
            string msg = "Error message for some params with exception";
            string method = "GetReports";
            string spName = "spa_MULTI_PCI_GetMasterMerchant";
            FilterParameterCollection filterParams = new FilterParameterCollection
            {
                new FilterParameter("@PrimaryUserID", "100", DbType.AnsiString),
                new FilterParameter("@ASClient", 217, DbType.Int32)
            };
            var ex = new Exception("ex");
            PciLoggerManager.Error(msg, method, spName, filterParams, ex);
        }
        [TestMethod]
        public void Warn_ByParams_Exception()
        {
            string msg = "Warn message for some params with exception";
            string method = "GetReports";
            string spName = "spa_MULTI_PCI_GetMasterMerchant";
            FilterParameterCollection filterParams = new FilterParameterCollection
            {
                new FilterParameter("@PrimaryUserID", "100", DbType.AnsiString),
                new FilterParameter("@ASClient", 217, DbType.Int32)
            };
            var ex = new Exception("ex");
            PciLoggerManager.Warn(msg, method, spName, filterParams, ex);
        }
        [TestMethod]
        public void Fatal_ByParams_Exception()
        {
            string msg = "Fatal message for some params with exception";
            string method = "GetReports";
            string spName = "spa_MULTI_PCI_GetMasterMerchant";
            FilterParameterCollection filterParams = new FilterParameterCollection
            {
                new FilterParameter("@PrimaryUserID", "100", DbType.AnsiString),
                new FilterParameter("@ASClient", 217, DbType.Int32)
            };
            var ex = new Exception("ex");
            PciLoggerManager.Fatal(msg, method, spName, filterParams, ex);
        }
    }
}
