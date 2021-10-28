using System.IO;
using NUnit.Framework;
using Selenium.WebDriver.ChromeDriver.NuPkg.Test.Lib;

namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test
{
    [Parallelizable(ParallelScope.All)]
    public class BuildProjectABTest
    {
        [Test]
        public void Output_of_ProjectB_Contains_DriverFile_Test()
        {
            using var workSpace = new WorkSpace(copyFrom: "ProjectAB");

            //var devenv = Environment.ExpandEnvironmentVariables(Path.Combine("%DevEnvDir%", "devenv.exe"));
            var devenv = @"C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\IDE\devenv.exe";
            Shell.Run(workSpace, "nuget", "restore").Is(0);
            Shell.Run(workSpace, devenv, "ProjectAB.sln", "/Build").Is(0);

            var outDir = Path.Combine(workSpace, "ProjectB", "bin", "Debug", "net472");
            var driverFullPath1 = Path.Combine(outDir, "chromedriver");
            var driverFullPath2 = Path.Combine(outDir, "chromedriver.exe");
            (File.Exists(driverFullPath1) || File.Exists(driverFullPath2)).IsTrue();
        }
    }
}
