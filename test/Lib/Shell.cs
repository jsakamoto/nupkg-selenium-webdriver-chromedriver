using System.Diagnostics;

namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test.Lib;

public static class Shell
{
    public static int ErrorLevel { get; set; }

    public static bool Exists(string dir, string wildCard)
    {
        return Directory.GetFiles(dir, wildCard, SearchOption.TopDirectoryOnly).Any();
    }

    public static void DeleteDir(string dir)
    {
        if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true);
    }

    public static void XcopyDir(string srcDir, string dstDir)
    {
        Directory.CreateDirectory(dstDir);

        var srcFileNames = Directory.GetFiles(srcDir);
        foreach (var srcFileName in srcFileNames)
        {
            var dstFileName = Path.Combine(dstDir, Path.GetFileName(srcFileName));
            File.Copy(srcFileName, dstFileName);
        }

        var srcSubDirs = Directory.GetDirectories(srcDir);
        foreach (var srcSubDir in srcSubDirs)
        {
            var dstSubDir = Path.Combine(dstDir, Path.GetFileName(srcSubDir));
            XcopyDir(srcSubDir, dstSubDir);
        }
    }

    public static int Run(string workDir, params string[] args)
    {
        var pi = new ProcessStartInfo
        {
            WorkingDirectory = workDir,
            FileName = args.First(),
            Arguments = string.Join(" ", args.Skip(1)),
            UseShellExecute = false,
        };
        var process = Process.Start(pi);
        if (process == null) throw new Exception($"The process could not be started. ({pi.FileName})");
        process.WaitForExit();
        ErrorLevel = process.ExitCode;
        return process.ExitCode;
    }
}
