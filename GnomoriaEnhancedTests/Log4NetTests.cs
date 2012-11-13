using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GnomoriaEnhanced.Tests
{
    [TestClass]
    public class Log4NetTests
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Log4NetTests));

        [TestClass]
        public class Log4NetMethods
        {
            private string _message = "This is \"{0}\"-test-message";
            [TestMethod]
            public void TryLogInfo()
            {
                string message = string.Format(_message, "Info");
                Assert.IsTrue(_log.IsInfoEnabled);
                _log.Info(message);
            }

            [TestMethod]
            public void TryLogDebug()
            {
                string message = string.Format(_message, "Debug");
                Assert.IsTrue(_log.IsDebugEnabled);

                _log.Debug(message);
            }

            [TestMethod]
            public void TryLogWarn()
            {
                string message = string.Format(_message, "Warn");
                Assert.IsTrue(_log.IsWarnEnabled);

                _log.Warn(message);
            }

            [TestMethod]
            public void TryLogError()
            {
                string message = string.Format(_message, "Error");
                Assert.IsTrue(_log.IsErrorEnabled);

                _log.Error(message);
            }

            [TestMethod]
            public void TryLogFatal()
            {
                string message = string.Format(_message, "Fatal");
                Assert.IsTrue(_log.IsFatalEnabled);

                _log.Fatal(message);
            }
        }
    }
}
