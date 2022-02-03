namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test;

[Parallelizable(ParallelScope.All)]
public class BuildTest
{
    public static object[][] Runtimes => new object[][]{
            new object[] { "win-x64", "chromedriver.exe", Format.PE32 },
            new object[] { "osx.10.12-x64", "chromedriver", Format.MachO },
            new object[] { "linux-x64", "chromedriver", Format.ELF },
        };

    [Test]
    [TestCaseSource(nameof(Runtimes))]
    public async Task BuildWithRuntimeIdentifier_Test(string rid, string driverFileName, Format executableFileFormat)
    {
        var unitTestProjectDir = FileIO.FindContainerDirToAncestor("*.csproj");
        using var workDir = WorkDirectory.CreateCopyFrom(Path.Combine(unitTestProjectDir, "Project"), predicate: item => item.Name is not "obj" and not "bin");

        using var dotnet = await XProcess.Start("dotnet", $"build -r {rid} -o out", workDir).WaitForExitAsync();
        dotnet.ExitCode.Is(0);

        var driverFullPath = Path.Combine(workDir, "out", driverFileName);
        File.Exists(driverFullPath).IsTrue();

        DetectFormat(driverFullPath).Is(executableFileFormat);
    }

    [Test]
    [TestCaseSource(nameof(Runtimes))]
    public async Task PublishWithRuntimeIdentifier_NoPublish_Test(string rid, string driverFileName, Format _)
    {
        var unitTestProjectDir = FileIO.FindContainerDirToAncestor("*.csproj");
        using var workDir = WorkDirectory.CreateCopyFrom(Path.Combine(unitTestProjectDir, "Project"), predicate: item => item.Name is not "obj" and not "bin");

        using var dotnet = await XProcess.Start("dotnet", $"publish -r {rid} -o out", workDir).WaitForExitAsync();
        dotnet.ExitCode.Is(0);

        var driverFullPath = Path.Combine(workDir, "out", driverFileName);
        File.Exists(driverFullPath).IsFalse();
    }

    [Test]
    [TestCaseSource(nameof(Runtimes))]
    public async Task PublishWithRuntimeIdentifier_with_MSBuildProp_Test(string rid, string driverFileName, Format executableFileFormat)
    {
        var unitTestProjectDir = FileIO.FindContainerDirToAncestor("*.csproj");
        using var workDir = WorkDirectory.CreateCopyFrom(Path.Combine(unitTestProjectDir, "Project"), predicate: item => item.Name is not "obj" and not "bin");

        using var dotnet = await XProcess.Start("dotnet", $"publish -r {rid} -o out -p:PublishChromeDriver=true", workDir).WaitForExitAsync();
        dotnet.ExitCode.Is(0);

        var driverFullPath = Path.Combine(workDir, "out", driverFileName);
        File.Exists(driverFullPath).IsTrue();

        DetectFormat(driverFullPath).Is(executableFileFormat);
    }

    [Test]
    [TestCaseSource(nameof(Runtimes))]
    public async Task PublishWithRuntimeIdentifier_with_DefineConstants_Test(string rid, string driverFileName, Format executableFileFormat)
    {
        var unitTestProjectDir = FileIO.FindContainerDirToAncestor("*.csproj");
        using var workDir = WorkDirectory.CreateCopyFrom(Path.Combine(unitTestProjectDir, "Project"), predicate: item => item.Name is not "obj" and not "bin");

        using var dotnet = await XProcess.Start("dotnet", $"publish -r {rid} -o out -p:DefineConstants=_PUBLISH_CHROMEDRIVER", workDir).WaitForExitAsync();
        dotnet.ExitCode.Is(0);

        var driverFullPath = Path.Combine(workDir, "out", driverFileName);
        File.Exists(driverFullPath).IsTrue();

        DetectFormat(driverFullPath).Is(executableFileFormat);
    }

    [Test]
    public async Task Publish_with_SingleFileEnabled_Test()
    {
        var rid = "win-x64";
        var driverFileName = "chromedriver.exe";
        var executableFileFormat = Format.PE32;

        var unitTestProjectDir = FileIO.FindContainerDirToAncestor("*.csproj");
        using var workDir = WorkDirectory.CreateCopyFrom(Path.Combine(unitTestProjectDir, "Project"), predicate: item => item.Name is not "obj" and not "bin");

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
            using var dotnet = await XProcess.Start(
               filename: publishCommand.First(),
               arguments: String.Join(' ', publishCommand.Skip(1)),
               workDir).WaitForExitAsync();
            dotnet.ExitCode.Is(0);

            var driverFullPath = Path.Combine(workDir, "out", driverFileName);
            File.Exists(driverFullPath).IsTrue();

            DetectFormat(driverFullPath).Is(executableFileFormat);
        }
    }
}
