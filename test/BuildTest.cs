using NUnit.Framework;
using Selenium.WebDriver.ChromeDriver.NuPkg.Test.Lib;
using static Selenium.WebDriver.ChromeDriver.NuPkg.Test.Lib.ExecutableFile;

namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test;

[Parallelizable(ParallelScope.All)]
public class BuildTest
{
    public static object[][] Runtimes => new object[][]{
            new object[] { "win-x64", "chromedriver.exe", Format.PE },
            new object[] { "osx.10.12-x64", "chromedriver", Format.MachO },
            new object[] { "linux-x64", "chromedriver", Format.ELF },
        };

    [Test]
    [TestCaseSource(nameof(Runtimes))]
    public void BuildWithRuntimeIdentifier_Test(string rid, string driverFileName, Format executableFileFormat)
    {
        using var workSpace = new WorkSpace(copyFrom: "Project");

        var exitCode = Shell.Run(workSpace, "dotnet", "build", "-r", rid, "-o", "out");
        exitCode.Is(0);

        var driverFullPath = Path.Combine(workSpace, "out", driverFileName);
        File.Exists(driverFullPath).IsTrue();

        DetectFormat(driverFullPath).Is(executableFileFormat);
    }

    [Test]
    [TestCaseSource(nameof(Runtimes))]
    public void PublishWithRuntimeIdentifier_NoPublish_Test(string rid, string driverFileName, Format _)
    {
        using var workSpace = new WorkSpace(copyFrom: "Project");

        var exitCode = Shell.Run(workSpace, "dotnet", "publish", "-r", rid, "-o", "out");
        exitCode.Is(0);

        var driverFullPath = Path.Combine(workSpace, "out", driverFileName);
        File.Exists(driverFullPath).IsFalse();
    }

    [Test]
    [TestCaseSource(nameof(Runtimes))]
    public void PublishWithRuntimeIdentifier_with_MSBuildProp_Test(string rid, string driverFileName, Format executableFileFormat)
    {
        using var workSpace = new WorkSpace(copyFrom: "Project");

        var exitCode = Shell.Run(workSpace, "dotnet", "publish", "-r", rid, "-o", "out", "-p:PublishChromeDriver=true");
        exitCode.Is(0);

        var driverFullPath = Path.Combine(workSpace, "out", driverFileName);
        File.Exists(driverFullPath).IsTrue();

        DetectFormat(driverFullPath).Is(executableFileFormat);
    }

    [Test]
    [TestCaseSource(nameof(Runtimes))]
    public void PublishWithRuntimeIdentifier_with_DefineConstants_Test(string rid, string driverFileName, Format executableFileFormat)
    {
        using var workSpace = new WorkSpace(copyFrom: "Project");

        var exitCode = Shell.Run(workSpace, "dotnet", "publish", "-r", rid, "-o", "out", "-p:DefineConstants=_PUBLISH_CHROMEDRIVER");
        exitCode.Is(0);

        var driverFullPath = Path.Combine(workSpace, "out", driverFileName);
        File.Exists(driverFullPath).IsTrue();

        DetectFormat(driverFullPath).Is(executableFileFormat);
    }

    [Test]
    public void Publish_with_SingleFileEnabled_Test()
    {
        var rid = "win-x64";
        var driverFileName = "chromedriver.exe";
        var executableFileFormat = Format.PE;

        using var workSpace = new WorkSpace(copyFrom: "Project");
        var publishCommand = new[] {
                "dotnet", "publish", "-r", rid, "-o", "out",
                "-c:Release",
                "-p:PublishChromeDriver=true",
                "-p:PublishSingleFile=true",
                "-p:SelfContained=false"
            };

        // IMPORTANT: 2nd time of publishing, sometimes lost driver file in the published folder, so we have to validate it..
        for (var i = 0; i < 2; i++)
        {
            var exitCode = Shell.Run(workSpace, publishCommand);
            exitCode.Is(0);

            var driverFullPath = Path.Combine(workSpace, "out", driverFileName);
            File.Exists(driverFullPath).IsTrue();

            DetectFormat(driverFullPath).Is(executableFileFormat);
        }
    }
}
