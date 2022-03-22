﻿namespace Selenium.WebDriver.ChromeDriver.NuPkg.Test.Internals;

internal static class XProcessExtensions
{
    public static async ValueTask ExitCodeIs(this XProcess process, int expected)
    {
        using (process)
        {
            await process.WaitForExitAsync();
            process.ExitCode.Is(expected, "message: " + process.Output);
        }
    }
}
