using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TestUnit.Pages
{
    public class HomePage : DriverHelper
    {
        IWebElement currency => driver.FindElement(By.CssSelector(".bui-button__text"));
        IWebElement language => driver.FindElement(By.XPath("//div[@class = 'bui-avatar bui-avatar--small']"));
        IWebElement avia => driver.FindElement(By.XPath("//ul[@class = 'bui-tab__nav']/li[2]"));
        IWebElement aviaPage => driver.FindElement(By.XPath("//html[@class]"));
        IWebElement loginLink => driver.FindElement(By.XPath("//div[@class = 'bui-group bui-button-group bui-group--inline bui-group--align-end bui-group--vertical-align-middle']/div[position() = 5]"));
        IWebElement calendar  => driver.FindElement(By.XPath("//div[@class = 'xp__dates xp__group']"));
        IWebElement accountLink => driver.FindElement(By.Id("profile-menu-trigger--title"));
        IWebElement manageAccount => driver.FindElement(By.XPath("//ul[@class = 'bui-dropdown-menu__items']/li[position() = 1]"));
        IWebElement city => driver.FindElement(By.Id("ss"));
        IWebElement tickets => driver.FindElement(By.XPath("//div[@class = 'xp__input-group xp__guests']"));
        IWebElement searchButton => driver.FindElement(By.XPath("//button[@class = 'sb-searchbox__button ']"));
        IWebElement listOfCurrencies;
        IWebElement listOfLanguages;
        public IWebElement actualCurrency => driver.FindElement(By.XPath("//span[@class = 'bui-button__text']/span[@aria-hidden = 'true'][1]"));
        public IWebElement actualLanguage => driver.FindElement(By.XPath("//div[@class = 'bui-avatar bui-avatar--small']/img[@src]"));
        public void ChangeCurrency(string value)
        {
            currency.Click();
            Thread.Sleep(2000);
            listOfCurrencies = driver.FindElement(By.XPath($"//ul[@class = 'bui-grid bui-grid--size-small']/li/a/div/div[text() = '\n{value}\n'][1]"));
            listOfCurrencies.Click();
        }
        public void ChangeLanguage(string value)
        {
            language.Click();
            Thread.Sleep(1000);
            listOfLanguages = driver.FindElement(By.XPath($"//ul[@class = 'bui-grid bui-grid--size-small']/li/a/div/div[text() = '\n{value}\n'][1]"));
            listOfLanguages.Click();
        }
        public bool isLanguageChanged(string value)
        {
            if (actualLanguage.GetAttribute("src").Contains("gb"))
            {
                return true;
            }
            else return false;
        }
        public void goToAvia() => avia.Click();
        public bool isAvia()
        {
            if (aviaPage.GetAttribute("class").Contains("Flights"))
            {
                return true;
            }
            else return false;
        }

        public void toLogin() => loginLink.Click();
        public void toAccount()
        {
            accountLink.Click();
            manageAccount.Click();
        }
        public bool isManage()
        {
            if (driver.FindElement(By.CssSelector(".ltr")).Displayed)
            {
                return true;
            }
            else return false;
        }
        public void SelectDate()
        {
            //city
            city.SendKeys("Minsk");
            /*IWebElement exactCity = driver.FindElement(By.XPath("//ul[@class = 'c-autocomplete__list sb-autocomplete__list sb-autocomplete__list-with_photos']/li[position() = 1]"));
            exactCity.Click();*/

            //date
            calendar.Click();
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var week = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
            IWebElement dateCheckIn = driver.FindElement(By.XPath($"//td[@data-date = '{today}']"));
            IWebElement dateCheckOut = driver.FindElement(By.XPath($"//td[@data-date = '{week}']"));
            dateCheckIn.Click();
            dateCheckOut.Click();

            //tickets
            tickets.Click();
            IWebElement adult = driver.FindElement(By.XPath("//div[@class = 'sb-group__field sb-group__field-adults']/div/div[2]/span[@class = 'bui-stepper__display']"));
            IWebElement add = driver.FindElement(By.XPath("//div[@class = 'sb-group__field sb-group__field-adults']/div/div/button[@class = 'bui-button bui-button--secondary bui-stepper__add-button ']"));
            var adultAmount = int.Parse(adult.Text);
            addSome(adultAmount, 2, add);
           
            IWebElement kids = driver.FindElement(By.XPath("//div[@class = 'sb-group__field sb-group-children ']/div/div[2]/span[@class = 'bui-stepper__display']"));
            IWebElement addKids = driver.FindElement(By.XPath("//div[@class = 'sb-group__field sb-group-children ']/div/div/button[@class = 'bui-button bui-button--secondary bui-stepper__add-button ']"));
            var kidsAmount = int.Parse(kids.Text);
            addSome(kidsAmount, 1, addKids);


            IWebElement rooms = driver.FindElement(By.XPath("//div[@class = 'sb-group__field sb-group__field-rooms']/div/div[2]/span[@class = 'bui-stepper__display']"));
            IWebElement addRoom = driver.FindElement(By.XPath("//div[@class = 'sb-group__field sb-group__field-rooms']/div/div/button[@class = 'bui-button bui-button--secondary bui-stepper__add-button ']"));
            var roomsAmount = int.Parse(rooms.Text);
            addSome(roomsAmount, 1, addRoom);

            searchButton.Click();
        }

        public void addSome(int amount, int neededAmount, IWebElement button)
        {
            if (amount < neededAmount)
            {
                button.Click();
                amount++;
                
                addSome(amount, neededAmount, button);
            }
        }
        public bool isSearched()
        {
            if (driver.FindElement(By.Id("b2searchresultsPage")).Displayed)
            {
                return true;
            }
            else return false;
        }
    }
}
