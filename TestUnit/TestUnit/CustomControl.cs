using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestUnit
{
    class CustomControl : DriverHelper
    {
        public static void ComboBox(string name, string value)
        {
            IWebElement control = driver.FindElement(By.XPath($"//input[@id = '{name}-awed']"));
            control.Clear();
            control.SendKeys(value);

            driver.FindElement(By.XPath($"//div[@id = '{name}-dropmenu']//li[text()='{value}']")).Click();
        }
        public static void EnterText(IWebElement element, string value) => element.SendKeys(value);
        public static void ClickElement(IWebElement element) => element.Click();

        public static void SelectByValue(IWebElement element, string value)
        {
            SelectElement select = new SelectElement(element);
            select.SelectByValue(value);
        }
    }
}
