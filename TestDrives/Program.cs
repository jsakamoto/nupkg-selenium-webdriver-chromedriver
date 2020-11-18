using System;

using var driver = new OpenQA.Selenium.Chrome.ChromeDriver();

driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
driver.Navigate().GoToUrl("https://www.bing.com/");
driver.FindElementById("sb_form_q").SendKeys("Selenium WebDriver");
driver.FindElementByClassName("search").Click();

Console.WriteLine("OK");
Console.ReadKey(intercept: true);
