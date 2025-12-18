using Epew.Core.Helper;
using Epew.Core.Runner;
using GRYLibrary.Core.APIServer.Services.Interfaces;
using GRYLibrary.Core.Logging.GRYLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;

namespace Epew.Tests.Testcases
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void Echo()
        {
            // arrange
            string output = "test";
            string[] arguments = new string[] { "--Program", "echo2", "--Argument", output };
            ProgramStarter pe = new ProgramStarter();

            // act
            int result = pe.Main(arguments);

            // assert
            Assert.AreEqual(0, result);
            Assert.IsNotNull(pe.Result);
            Assert.IsTrue(pe.Result is RunWithArgumentsFromCLI);
            RunWithArgumentsFromCLI resultRunner = (RunWithArgumentsFromCLI)pe.Result;
            Assert.AreEqual(0, resultRunner._ExternalProgramExecutor.ExitCode);
            Assert.AreEqual(1, resultRunner._ExternalProgramExecutor.AllStdOutLines.Length);
            Assert.AreEqual(output, resultRunner._ExternalProgramExecutor.AllStdOutLines[0]);
            Assert.AreEqual(0, resultRunner._ExternalProgramExecutor.AllStdErrLines.Length);
        }

        [TestMethod]
        public void EchoWithTimestampUTC()
        {
            // arrange
            StringWriter stringWriter = new StringWriter();
            System.Console.SetOut(stringWriter);
            string output = "test2";
            DateTimeOffset testDateTime = new System.DateTimeOffset(2025, 10, 19, 00, 25, 04, TimeSpan.FromHours(2));
            GRYLog log = GRYLog.Create();
            RunWithArgumentsFromCLI pe = new RunWithArgumentsFromCLI(new ProgramStarter(log), new Core.Verbs.RunCLI()
            {
                Program = "echo2",
                Argument = output,
                AddLogOverhead = true,
                Verbosity = GRYLibrary.Core.ExecutePrograms.Verbosity.Full,
            });
            Mock<ITimeService> timeServiceMock = new Mock<ITimeService>(MockBehavior.Strict);
            timeServiceMock.Setup(t => t.GetCurrentLocalTimeAsDateTimeOffset()).Returns(testDateTime);
            log._TimeService = timeServiceMock.Object;

            // act
            int result = pe.Run();

            // assert
            Assert.AreEqual(0, result);
            string content = stringWriter.ToString().Replace("\n", string.Empty).Replace("\r", string.Empty);
            Assert.AreEqual($"[2025-10-19T00:25:04+02:00] [Information] {output}", content);
        }
    }
}