using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestDrive;

var options = new ChromeOptions();
if (Environment.OSVersion.Platform == PlatformID.Win32NT && ChromeDriverVersionInfo.VersionText.EndsWith("-beta"))
{
    options.BinaryLocation = @"C:\Program Files\Google\Chrome Beta\Application\chrome.exe";
}

using var driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory, options);

driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
driver.Navigate().GoToUrl("https://www.nuget.org/");
await Task.Delay(1000);

driver.FindElement(By.Id("search")).SendKeys("Selenium WebDriver");
driver.FindElement(By.Id("search")).SendKeys(Keys.Enter);

Console.WriteLine("OK");
Console.ReadKey(intercept: true);
