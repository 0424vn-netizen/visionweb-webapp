using AS.VW.Scheduler.Tasks.Entities;
using Moq;
using System.Data;
using System.IO.Compression;
using System.Reflection;

namespace AS.VW.Scheduler.Tasks.Tests
{
    [TestClass]
    public class ExtractReportTests
    {
        private string _testDirectory = string.Empty;
        private string _zipFilePath = string.Empty;

        private Mock<ITaskManager>? _mockTaskManager;
        private ExtractReport? _extractReport;

        [TestInitialize]
        public void Setup()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDirectory);
            _zipFilePath = Path.Combine(_testDirectory, "output.zip");

            _mockTaskManager = new Mock<ITaskManager>();
            _extractReport = new ExtractReport();
            var taskManagerField = typeof(ExtractReport).GetField("_taskManager", BindingFlags.NonPublic | BindingFlags.Instance);
            taskManagerField?.SetValue(_extractReport, _mockTaskManager.Object);

            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }

        #region ZipFiles

        [TestMethod]
        public void ZipFiles_WithValidFiles_ShouldReturnTrueAndCreateZip()
        {
            var file1 = Path.Combine(_testDirectory, "file1.txt");
            var file2 = Path.Combine(_testDirectory, "file2.txt");
            File.WriteAllText(file1, "File 1");
            File.WriteAllText(file2, "File 2");
            var files = new List<string> { file1, file2 };

            var report = new ExtractReport();
            var result = report.ZipFiles(files, _zipFilePath);

            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(_zipFilePath));
            using (var zip = ZipFile.OpenRead(_zipFilePath))
            {
                Assert.AreEqual(2, zip.Entries.Count);
            }
        }

        [TestMethod]
        public void ZipFiles_WithNonExistentFile_ShouldReturnFalse()
        {
            var files = new List<string> { Path.Combine(_testDirectory, "notfound.txt") };
            var report = new ExtractReport();
            var result = report.ZipFiles(files, _zipFilePath);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ZipFiles_WithEmptyList_ShouldReturnTrueAndCreateEmptyZip()
        {
            var report = new ExtractReport();
            var result = report.ZipFiles(new List<string>(), _zipFilePath);
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(_zipFilePath));
            using (var zip = ZipFile.OpenRead(_zipFilePath))
            {
                Assert.AreEqual(0, zip.Entries.Count);
            }
        }

        [TestMethod]
        public void ZipFiles_WithInvalidDestinationPath_ShouldReturnFalse()
        {
            var file = Path.Combine(_testDirectory, "file.txt");
            File.WriteAllText(file, "Test");
            var report = new ExtractReport();
            var invalidPath = Path.Combine("Z:\\InvalidFolder", "output.zip");
            var result = report.ZipFiles(new List<string> { file }, invalidPath);
            Assert.IsFalse(result);
        }

        #endregion

    }
}