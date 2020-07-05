using ApplicationLogs.Logs;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApplicationLogs.Tests
{
    [TestClass]
    public class ApplicationLogsTests
    {
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            // Create the test database
            var script = File.ReadAllText(@"C:\tmp\Dev\Projects\Applications\ApplicationLogs.Solution\ApplicationLogs\DomainModel\Sql\CreateTestDatabase.sql");

            ScriptRunner.Run(ConnectionManager.GetConnection("Master").ConnectionString, script);
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        [TestMethod]
        public async Task TestLogging()
        {
            var log = new CreateApplicationLogInputDto
            {
                Type = ApplicationLog.LogTypes.Error,
                Message = "Test log message",
                UserId = 1.ToString()
            };

            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers["user-agent"] = "test user agent";

            httpContext.Request.Scheme = "http";

            httpContext.Request.Host = new HostString("localhost", 8080);

            httpContext.Request.Path = "/path";

            httpContext.Request.QueryString = new QueryString("?query=some");

            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("72.43.15.36");

            httpContext.Connection.LocalIpAddress = IPAddress.Parse("192.168.1.42");

            log.AddHttpContextInformation(httpContext);

            await ApplicationLogger.Log(log);

            var (count, logs) = await ApplicationLogger.Get(null);

            var l = logs.Single();

            Assert.AreEqual(ApplicationLog.LogTypes.Error, l.Type);

            Assert.AreEqual("Test log message", l.Message);

            Assert.AreEqual("1", l.UserId);

            Assert.AreEqual("test user agent", l.UserAgent);

            Assert.AreEqual("http://localhost:8080/path?query=some", l.Url);

            Assert.AreEqual("72.43.15.36", l.UserIpAddress);

            Assert.AreEqual("192.168.1.42", l.HostIpAddress);

            await ApplicationLogger.DeleteLogs(DateTime.Now);

            (count, logs) = await ApplicationLogger.Get(null);

            // Logs are deleted for fractions of a second after created
            Assert.AreEqual(0, logs.Count());

        }
    }
}
