using System;
using OpenQA.Selenium;

using var driver = new OpenQA.Selenium.Chrome.ChromeDriver();

driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
driver.Navigate().GoToUrl("https://www.bing.com/");
driver.FindElement(By.Id("sb_form_q")).SendKeys("Selenium WebDriver");
driver.FindElement(By.ClassName("search")).Click();

Console.WriteLine("OK");
Console.ReadKey(intercept: true);
