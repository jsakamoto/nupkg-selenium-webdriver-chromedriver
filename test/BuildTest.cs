using System;
using System.IO;
using Selenium.WebDriver.ChromeDriver.NuPkg.Test.Lib;
using Xunit;

namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test
{
    public class BuildTest : IDisposable
    {
        private string WorkDir { get; }

        public BuildTest()
        {
            WorkDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            var srcDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Project");
            Shell.XcopyDir(srcDir, WorkDir);
        }

        public void Dispose()
        {
            Shell.DeleteDir(WorkDir);
        }

        public static object[][] Runtimes => new object[][]{
            new object[] { "win-x64", "chromedriver.exe", ExecutableFile.Format.PE },
            new object[] { "osx.10.12-x64", "chromedriver", ExecutableFile.Format.MachO },
            new object[] { "linux-x64", "chromedriver", ExecutableFile.Format.ELF },
        };

        [Theory]
        [MemberData(nameof(Runtimes))]
        public void BuildWithRuntimeIdentifier_Test(string rid, string driverFileName, ExecutableFile.Format executableFileFormat)
        {
            var exitCode = Shell.Run(WorkDir, "dotnet", "build", "-r", rid, "-o", "out");
            exitCode.Is(0);

            var driverFullPath = Path.Combine(WorkDir, "out", driverFileName);
            File.Exists(driverFullPath).IsTrue();

            ExecutableFile.DetectFormat(driverFullPath).Is(executableFileFormat);
        }

        [Theory]
        [MemberData(nameof(Runtimes))]
        public void PublishWithRuntimeIdentifier_NoPublish_Test(string rid, string driverFileName, ExecutableFile.Format _)
        {
            var exitCode = Shell.Run(WorkDir, "dotnet", "publish", "-r", rid, "-o", "out");
            exitCode.Is(0);

            var driverFullPath = Path.Combine(WorkDir, "out", driverFileName);
            File.Exists(driverFullPath).IsFalse();
        }

        [Theory]
        [MemberData(nameof(Runtimes))]
        public void PublishWithRuntimeIdentifier_with_MSBuildProp_Test(string rid, string driverFileName, ExecutableFile.Format executableFileFormat)
        {
            var exitCode = Shell.Run(WorkDir, "dotnet", "publish", "-r", rid, "-o", "out", "-p:PublishChromeDriver=true");
            exitCode.Is(0);

            var driverFullPath = Path.Combine(WorkDir, "out", driverFileName);
            File.Exists(driverFullPath).IsTrue();

            ExecutableFile.DetectFormat(driverFullPath).Is(executableFileFormat);
        }

        [Theory]
        [MemberData(nameof(Runtimes))]
        public void PublishWithRuntimeIdentifier_with_DefineConstants_Test(string rid, string driverFileName, ExecutableFile.Format executableFileFormat)
        {
            var exitCode = Shell.Run(WorkDir, "dotnet", "publish", "-r", rid, "-o", "out", "-p:DefineConstants=_PUBLISH_CHROMEDRIVER");
            exitCode.Is(0);

            var driverFullPath = Path.Combine(WorkDir, "out", driverFileName);
            File.Exists(driverFullPath).IsTrue();

            ExecutableFile.DetectFormat(driverFullPath).Is(executableFileFormat);
        }
    }
}
