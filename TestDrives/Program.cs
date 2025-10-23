using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestDrive;
using Toolbelt.Diagnostics;

var options = new ChromeOptions();
if (Environment.OSVersion.Platform == PlatformID.Win32NT && ChromeDriverVersionInfo.VersionText.EndsWith("-beta"))
{
    options.BinaryLocation = @"C:\Program Files\Google\Chrome Beta\Application\chrome.exe";
}

var driverVersion = await XProcess.Start("chromedriver", "--version", AppDomain.CurrentDomain.BaseDirectory).WaitForExitAsync();
Console.WriteLine(driverVersion.Output);

using var driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory, options);

driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
driver.Navigate().GoToUrl("https://www.nuget.org/");
await Task.Delay(1000);

driver.FindElement(By.Id("search")).SendKeys("Selenium WebDriver");
driver.FindElement(By.Id("search")).SendKeys(Keys.Enter);

Console.WriteLine("OK");
Console.ReadKey(intercept: true);
