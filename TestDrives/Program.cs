﻿using OpenQA.Selenium;

using var driver = new OpenQA.Selenium.Chrome.ChromeDriver(AppDomain.CurrentDomain.BaseDirectory);

driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
driver.Navigate().GoToUrl("https://www.bing.com/");
await Task.Delay(1000);

driver.FindElement(By.Id("sb_form_q")).SendKeys("Selenium WebDriver");
driver.FindElement(By.Id("sb_form_q")).SendKeys(Keys.Enter);

Console.WriteLine("OK");
Console.ReadKey(intercept: true);
