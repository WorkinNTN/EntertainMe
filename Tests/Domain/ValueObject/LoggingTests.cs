using System;
using System.IO;

using NUnit.Framework;

using EntertainMe.Domain.ValueObjects;
using System.Linq;

namespace EntertainMeTests.Domain.ValueObject
{
    public class LoggingTests
    {
        public string logPath = Path.GetTempPath();
        public string logFile = "loggingTest";
        public string logFileDate = DateTime.Now.ToString("yyyyMMdd");

        [SetUp]
        public void Setup()
        {
            string file1 = logPath + logFile + ".log";
            string file2 = logPath + logFile + "." + logFileDate + ".log";

            File.Delete(file1);
            File.Delete(file2);
        }

        [Test]
        public void Create()
        {
            var logging = new Logging();

            Assert.IsNotNull(logging);
        }

        [Test]
        public void CreateWithFileInfo()
        {
            var logging = new Logging(logPath, logFile);

            Assert.IsNotNull(logging);
        }

        [Test]
        public void CreateWithFileInfoStandardFileName()
        {
            string fullLogFilename = logPath + logFile + ".log";
            var logging = new Logging(logPath, logFile);
            Assert.IsNotNull(logging);

            logging.Log("Test");
            Assert.IsTrue(File.Exists(fullLogFilename));
        }

        [Test]
        public void CreateWithAddedFileInfoStandardFileName()
        {
            string fullLogFilename = logPath + logFile + ".log";
            var logging = new Logging();
            logging.SetLoggingInfo(logPath, logFile);
            Assert.IsNotNull(logging);

            logging.Log("Test");
            Assert.IsTrue(File.Exists(fullLogFilename));
        }

        [Test]
        public void CreateWithFileInfoDatedFileName()
        {
            string fullLogFilename = logPath + logFile + "." + logFileDate + ".log";
            var logging = new Logging(logPath, logFile);
            logging.DateStamp = true;
            Assert.IsNotNull(logging);

            logging.Log("Test");
            Assert.IsTrue(File.Exists(fullLogFilename));
        }

        [Test]
        public void CreateWithAddedFileInfoDatedFileName()
        {
            string fullLogFilename = logPath + logFile + "." + logFileDate + ".log";
            var logging = new Logging();
            logging.DateStamp = true;
            logging.SetLoggingInfo(logPath, logFile);
            Assert.IsNotNull(logging);

            logging.Log("Test");
            Assert.IsTrue(File.Exists(fullLogFilename));
        }

        public static Array GetLoggingTypes()
        {
            return Enum.GetValues(typeof(LoggingType));
        }

        [TestCaseSource("GetLoggingTypes")]
        public void CreateWithFileInfoAllLoggingTypes(LoggingType lt)
        {
            string fullLogFilename = logPath + logFile + ".log";
            var logging = new Logging(logPath, logFile);

            Assert.IsNotNull(logging);

            logging.Log("Test", lt);
            Assert.IsTrue(File.Exists(fullLogFilename));

            string line1 = File.ReadLines(fullLogFilename).First();
            Assert.IsTrue(line1.Contains(lt.ToString() + "::"));
        }
    }
}