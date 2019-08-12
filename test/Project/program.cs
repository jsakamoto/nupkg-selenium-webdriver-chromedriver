using System;

class Program
{
    static void Main()
    {
        using (var driver = new OpenQA.Selenium.Chrome.ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
        {
            driver.Navigate().GoToUrl("https://www.bing.com/");
            driver.FindElementById("sb_form_q").SendKeys("Selenium WebDriver");
            driver.FindElementById("sb_form_go").Click();

            Console.WriteLine("OK");
            Console.ReadKey(intercept: true);
        }
    }
}
