using System;
using OpenQA.Selenium;

class Program
{
    static void Main()
    {
        using (var driver = new OpenQA.Selenium.Chrome.ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
        {
            driver.Navigate().GoToUrl("https://www.bing.com/");
            driver.FindElement(By.Id("sb_form_q")).SendKeys("Selenium WebDriver");
            driver.FindElement(By.Id("sb_form_go")).Click();

            Console.WriteLine("OK");
            Console.ReadKey(intercept: true);
        }
    }
}
