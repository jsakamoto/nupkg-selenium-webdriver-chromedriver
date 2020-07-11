using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Selenium.WebDriver.ChromeDriver.NuPkg.Test.Lib;
using Xunit;

namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test
{
    public class BuildProjectABTest : IDisposable
    {
        private string WorkDir { get; }

        public BuildProjectABTest()
        {
            WorkDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            var srcDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProjectAB");
            Shell.XcopyDir(srcDir, WorkDir);
        }

        public void Dispose()
        {
            Shell.DeleteDir(WorkDir);
        }

        [Fact]
        public void Output_of_ProjectB_Contains_DriverFile_Test()
        {
            //var devenv = Environment.ExpandEnvironmentVariables(Path.Combine("%DevEnvDir%", "devenv.exe"));
            var devenv = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\Common7\IDE\devenv.exe";
            Shell.Run(WorkDir, "nuget", "restore").Is(0);
            Shell.Run(WorkDir, devenv, "ProjectAB.sln", "/Build").Is(0);

            var outDir = Path.Combine(WorkDir, "ProjectB", "bin", "Debug", "net472");
            var driverFullPath1 = Path.Combine(outDir, "chromedriver");
            var driverFullPath2 = Path.Combine(outDir, "chromedriver.exe");
            (File.Exists(driverFullPath1) || File.Exists(driverFullPath2)).IsTrue();
        }
    }
}
