using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestUnit.Pages
{
    public class LoginPage : DriverHelper
    {
        IWebElement username => driver.FindElement(By.Id("username"));
        IWebElement password => driver.FindElement(By.Id("password"));
        IWebElement loginButton => driver.FindElement(By.XPath("//button[@type = 'submit']"));



        public void EnterUsername(string usernameTxt) => username.SendKeys(usernameTxt);

        public void EnterPassword(string passwordTxt) => password.SendKeys(passwordTxt);
        public void ClickLogin() => loginButton.Click();
        
    }
}
