using OpenQA.Selenium;

using var driver = new OpenQA.Selenium.Chrome.ChromeDriver(AppDomain.CurrentDomain.BaseDirectory);

driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
driver.Navigate().GoToUrl("https://www.nuget.org/");
await Task.Delay(1000);

driver.FindElement(By.Id("search")).SendKeys("Selenium WebDriver");
driver.FindElement(By.Id("search")).SendKeys(Keys.Enter);

Console.WriteLine("OK");
Console.ReadKey(intercept: true);
