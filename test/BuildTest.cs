using System.IO;
using NUnit.Framework;
using Selenium.WebDriver.ChromeDriver.NuPkg.Test.Lib;

namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test
{
    [Parallelizable(ParallelScope.All)]
    public class BuildTest
    {
        public static object[][] Runtimes => new object[][]{
            new object[] { "win-x64", "chromedriver.exe", ExecutableFile.Format.PE },
            new object[] { "osx.10.12-x64", "chromedriver", ExecutableFile.Format.MachO },
            new object[] { "linux-x64", "chromedriver", ExecutableFile.Format.ELF },
        };

        [Test]
        [TestCaseSource(nameof(Runtimes))]
        public void BuildWithRuntimeIdentifier_Test(string rid, string driverFileName, ExecutableFile.Format executableFileFormat)
        {
            using var workSpace = new WorkSpace(copyFrom: "Project");

            var exitCode = Shell.Run(workSpace, "dotnet", "build", "-r", rid, "-o", "out");
            exitCode.Is(0);

            var driverFullPath = Path.Combine(workSpace, "out", driverFileName);
            File.Exists(driverFullPath).IsTrue();

            ExecutableFile.DetectFormat(driverFullPath).Is(executableFileFormat);
        }

        [Test]
        [TestCaseSource(nameof(Runtimes))]
        public void PublishWithRuntimeIdentifier_NoPublish_Test(string rid, string driverFileName, ExecutableFile.Format _)
        {
            using var workSpace = new WorkSpace(copyFrom: "Project");

            var exitCode = Shell.Run(workSpace, "dotnet", "publish", "-r", rid, "-o", "out");
            exitCode.Is(0);

            var driverFullPath = Path.Combine(workSpace, "out", driverFileName);
            File.Exists(driverFullPath).IsFalse();
        }

        [Test]
        [TestCaseSource(nameof(Runtimes))]
        public void PublishWithRuntimeIdentifier_with_MSBuildProp_Test(string rid, string driverFileName, ExecutableFile.Format executableFileFormat)
        {
            using var workSpace = new WorkSpace(copyFrom: "Project");

            var exitCode = Shell.Run(workSpace, "dotnet", "publish", "-r", rid, "-o", "out", "-p:PublishChromeDriver=true");
            exitCode.Is(0);

            var driverFullPath = Path.Combine(workSpace, "out", driverFileName);
            File.Exists(driverFullPath).IsTrue();

            ExecutableFile.DetectFormat(driverFullPath).Is(executableFileFormat);
        }

        [Test]
        [TestCaseSource(nameof(Runtimes))]
        public void PublishWithRuntimeIdentifier_with_DefineConstants_Test(string rid, string driverFileName, ExecutableFile.Format executableFileFormat)
        {
            using var workSpace = new WorkSpace(copyFrom: "Project");

            var exitCode = Shell.Run(workSpace, "dotnet", "publish", "-r", rid, "-o", "out", "-p:DefineConstants=_PUBLISH_CHROMEDRIVER");
            exitCode.Is(0);

            var driverFullPath = Path.Combine(workSpace, "out", driverFileName);
            File.Exists(driverFullPath).IsTrue();

            ExecutableFile.DetectFormat(driverFullPath).Is(executableFileFormat);
        }
    }
}
