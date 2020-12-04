using System;
using System.IO;

namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test.Lib
{
    public class WorkSpace : IDisposable
    {
        public string WorkDir { get; }

        public WorkSpace(string copyFrom)
        {
            this.WorkDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            var srcDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, copyFrom);
            Shell.XcopyDir(srcDir, WorkDir);
        }

        public void Dispose()
        {
            try { Shell.DeleteDir(WorkDir); } catch { }
        }

        public static implicit operator string(WorkSpace workSpace) => workSpace.WorkDir;
    }
}
