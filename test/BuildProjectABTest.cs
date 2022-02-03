using NUnit.Framework;
using Toolbelt;
using Toolbelt.Diagnostics;

namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test;

[Parallelizable(ParallelScope.All)]
public class BuildProjectABTest
{
    [Test]
    public async Task Output_of_ProjectB_Contains_DriverFile_Test()
    {
        var unitTestProjectDir = FileIO.FindContainerDirToAncestor("*.csproj");
        using var workDir = WorkDirectory.CreateCopyFrom(Path.Combine(unitTestProjectDir, "ProjectAB"), item => item.Name is not "obj" and not "bin");

        //var devenv = Environment.ExpandEnvironmentVariables(Path.Combine("%DevEnvDir%", "devenv.exe"));
        var devenv = @"C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\IDE\devenv.exe";
        using var nugetProcess = await XProcess.Start("nuget", "restore", workDir).WaitForExitAsync();
        nugetProcess.ExitCode.Is(0);
        using var devenvProcess = await XProcess.Start(devenv, "ProjectAB.sln /Build", workDir).WaitForExitAsync();
        devenvProcess.ExitCode.Is(0);

        var outDir = Path.Combine(workDir, "ProjectB", "bin", "Debug", "net472");
        var driverFullPath1 = Path.Combine(outDir, "chromedriver");
        var driverFullPath2 = Path.Combine(outDir, "chromedriver.exe");
        (File.Exists(driverFullPath1) || File.Exists(driverFullPath2)).IsTrue();
    }
}
