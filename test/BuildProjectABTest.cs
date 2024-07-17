namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test;

[Parallelizable(ParallelScope.All)]
public class BuildProjectABTest
{
    [Test, Platform("Win")]
    public async Task Output_of_ProjectB_Contains_DriverFile_Test()
    {
        var vsAppDir = Environment.GetEnvironmentVariable("VSAPPIDDIR");
        if (vsAppDir == null) Assert.Inconclusive(@"This test requires Visual Studio and the definition of the ""VSAPPDIR"" environment variable to point out the directory where Visual Studio ""devenv.exe"" exists. (ex: VSAPPDIR=C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\)");

        var unitTestProjectDir = FileIO.FindContainerDirToAncestor("*.csproj");
        using var workDir = WorkDirectory.CreateCopyFrom(Path.Combine(unitTestProjectDir, "ProjectAB"), item => item.Name is not "obj" and not "bin");

        var nuget = Path.Combine(unitTestProjectDir, "nuget.exe");
        var devenv = Path.Combine(vsAppDir, "devenv.com");
        await XProcess.Start(nuget, "restore", workDir).ExitCodeIs(0);
        await XProcess.Start(devenv, "ProjectAB.sln /Build", workDir).ExitCodeIs(1);

        var outDir = Path.Combine(workDir, "ProjectB", "bin", "Debug", "net472");
        var driverFullPath1 = Path.Combine(outDir, "chromedriver");
        var driverFullPath2 = Path.Combine(outDir, "chromedriver.exe");
        (File.Exists(driverFullPath1) || File.Exists(driverFullPath2)).IsTrue();
    }
}
