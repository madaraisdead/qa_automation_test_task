
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TestUnit.JsonModel;
using TestUnit.Pages;

namespace TestUnit
{
    public class Tests : DriverHelper
    {

      
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://booking.com");
        }
        //UI TESTING
        [Test]
        public void Test1()
        {
            HomePage homePage = new HomePage();
            homePage.ChangeCurrency("Евро");
            Thread.Sleep(2000);
            Assert.That(homePage.actualCurrency.Text, Is.EqualTo("EUR"), "Currency has not changed");

            homePage.ChangeLanguage("English (UK)");
            Thread.Sleep(2000);
            Assert.That(homePage.isLanguageChanged("gb"), Is.True, "Language has not changed");

            homePage.goToAvia();
            Thread.Sleep(2000);
            Assert.That(homePage.isAvia(), Is.True, "Not avia page");
        }

        [Test]
        public void LoginTest()
        {
            HomePage homePage = new HomePage();
            LoginPage loginPage = new LoginPage();
            homePage.toLogin();
            Thread.Sleep(2000);
            loginPage.EnterUsername("33aprelya@mail.ru");
            loginPage.ClickLogin();

            Thread.Sleep(2000);
            loginPage.EnterPassword("kGXhKA7Unh7tJJX");
            loginPage.ClickLogin();

            Thread.Sleep(2000);
            homePage.toAccount();
            Thread.Sleep(2000);
            Assert.That(homePage.isManage(), Is.True, "Can not manage account");
        }
        [Test]
        public void FilterTest()
        {
            HomePage homePage = new HomePage();
            homePage.SelectDate();
            Thread.Sleep(2000);
            Assert.That(homePage.isSearched(), Is.True, "No search");
        }

        //API TESTING
        [Test]
        public void getMinsk()
        {
            string url = "https://www.metaweather.com/api/location/search/?query=min";

            HttpRequestMessage req = new HttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.Headers.Add("Accept", "application/json");
            req.RequestUri = new Uri(url);
            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(req);
            HttpResponseMessage message = httpResponse.Result;

            HttpContent responseContent = message.Content;
            var data = responseContent.ReadAsStringAsync();
            List<RootObject> root = JsonConvert.DeserializeObject<List<RootObject>>(data.Result);
            var result = root.Find(x => x.title == "Minsk" && x.location_type == "City");
            if (result.latt_long.Contains("53.9") && result.latt_long.Contains("27.56"))
            {
                Console.WriteLine("Latt Long matches real data");
            }
            else
            {
                Console.WriteLine("Latt long doesn't match real data");
            }
            httpClient.Dispose();
        }
        [Test]
        public void getForecast()
        {
            string url = "https://www.metaweather.com/api/location/834463/";
            HttpRequestMessage req = new HttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.Headers.Add("Accept", "application/json");
            req.RequestUri = new Uri(url);
            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(req);
            HttpResponseMessage message = httpResponse.Result;


            HttpContent responseContent = message.Content;
            var data = responseContent.ReadAsStringAsync();
            Root forecastToday = JsonConvert.DeserializeObject<Root>(data.Result);
      
            FieldInfo[] fields = forecastToday.consolidated_weather[0].GetType().GetFields(BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance);

            //forecast for today for minsk
            Console.WriteLine("Today for Minsk: ");
            for (int i=0; i < fields.Length; i++)
            {
                Console.WriteLine("{0} : {1}", fields[i].Name, fields[i].GetValue(forecastToday.consolidated_weather[0]));
            }


            //temperature in interval 
            for (int i = 0; i < forecastToday.consolidated_weather.Count; i++)
            {
                if (forecastToday.consolidated_weather[i].the_temp < 15 && forecastToday.consolidated_weather[i].the_temp > 8)
                {
                    Console.WriteLine("For {0} day temperature is between 8 and 15", i + 1);
                }
            }
            httpClient.Dispose();
        }
        [Test]
        public void getDataFromArchive()
        {
            DateTime fiveYearsAgo = new DateTime(DateTime.Now.Year - 3, DateTime.Now.Month, DateTime.Now.Day);

            string url = $"https://www.metaweather.com/api/location/834463/{fiveYearsAgo.Year}/{fiveYearsAgo.Month}/{fiveYearsAgo.Day}";
            HttpRequestMessage req = new HttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.Headers.Add("Accept", "application/json");
            req.RequestUri = new Uri(url);
            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(req);
            HttpResponseMessage message = httpResponse.Result;


            HttpContent responseContent = message.Content;
            var data = responseContent.ReadAsStringAsync();
            List<ArchiveObject> archive = JsonConvert.DeserializeObject<List<ArchiveObject>>(data.Result);


            string urlToday = "https://www.metaweather.com/api/location/834463/";
            HttpRequestMessage reqToday = new HttpRequestMessage();
            reqToday.Method = HttpMethod.Get;
            reqToday.Headers.Add("Accept", "application/json");
            reqToday.RequestUri = new Uri(urlToday);
            Task<HttpResponseMessage> httpRes = httpClient.SendAsync(reqToday);
            HttpResponseMessage messageRes = httpRes.Result;


            HttpContent resContent = messageRes.Content;
            var dataToday = resContent.ReadAsStringAsync();
            Root forecastToday = JsonConvert.DeserializeObject<Root>(dataToday.Result);

            int count = 0;
            for (int i=0; i< archive.Count; i++)
            {
               
               if (forecastToday.consolidated_weather[0].weather_state_name.ToString() == archive[i].weather_state_name.ToString()){
                    count++;
                }
            }
            Console.WriteLine("Number of matches: {0} ", count);
        }
    }

}