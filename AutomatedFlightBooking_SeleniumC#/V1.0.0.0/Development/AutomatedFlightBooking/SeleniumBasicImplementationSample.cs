using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Threading;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;
using WebDriverWrapper;
using WebDriverWrapper.Extensions;

namespace UdemyAutomationProject
{
    /// <summary>
    /// Summary description for SeleniumBasicImplementationSample
    /// </summary>
    [TestClass]

    [DeploymentItem("IEDriverServer.exe")]
    [DeploymentItem("chromedriver.exe")]
    [DeploymentItem("geckodriver.exe")]
    [DeploymentItem("MicrosoftWebDriver.exe")]
    public class SeleniumBasicImplementationSample:SeleniumHandler
    {
        public SeleniumBasicImplementationSample()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        //private static IWebDriver driver = null;       

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        // Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext) {
        //    driver = new FirefoxDriver();
        //    driver.Navigate().GoToUrl("https://www.lastminutetravel.com/flights");
        //    driver.Manage().Window.Maximize();
        //}

        //// Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup() {
        //    Thread.Sleep(4000);
        //    driver.Dispose();
        //}
        
        /*TESTS
         ***/
        [TestMethod]
        public void JsonObjectSample()
        {
            var jsonSample = new SeleniumWrapper();
            jsonSample.parseJson("{\"FirstName\":\"Lencho\",\"LastName\":\"Burka\",\"Age\":34,\"LikeThisCourse\":\"true\"}");
        }

        [TestMethod]
        public void SeleniumHandlerSamples()
        {
            var webDs = new List<string>();
            webDs.Add("{ \"Driver\":\"Chrome\" }");
            webDs.Add("{ \"Driver\":\"Firefox\" }");
            webDs.Add("{ \"Driver\":\"IE\" }");


            webDs.ForEach(_webD =>
            {
                var handler = new SeleniumHandler();
                handler.WebDriverParams = _webD;
                var webD = handler.WebDriver;

                try
                {
                    webD.Navigate().GoToUrl("https://www.lastminutetravel.com/flights");
                    webD.Manage().Window.Maximize();
                    webD.FindElement(By.Id("productsNavHotels")).Click();
                }
                catch (Exception)
                {                    
                    throw;
                }
                finally
                {
                    webD.Dispose();
                }
            });

        }

        [TestMethod]
        public void SearchFlightsWithWrapper()
        {
            WebDriverParams = "{ \"Driver\":\"Chrome\" }";
            GoToUrl("https://www.lastminutetravel.com/flights");
                      
            /*
            Textbox Tests
            //Set FROM and TO destinations
            //FROM
            */
            var fromDest = FindElement(By.Id("autosuggest-flightsFrom"));
            fromDest.SendKeys("auckland");
            fromDest.Click();
            FindElement(By.Id("react-autowhatever-1-section-0-item-2")).Click();

            ////TO
            var toDest = FindElement(By.Id("autosuggest-flightsTo"));
            toDest.SendKeys("sydney");
            toDest.Click();
            FindElement(By.Id("react-autowhatever-1-section-0-item-1")).Click();
            //Click Background - hide date popup
            FindElement(By.ClassName("bannerTitle")).Click();
            ////ASSERT
            Assert.AreEqual("Auckland - Aukland International Airport, New Zealand", fromDest.GetAttribute("value"));
            Assert.AreEqual("Sydney - All Airports, NS, Australia", toDest.GetAttribute("value"));

            /*
            Radio Buttons
            //Select One Way OR Round Trip Flight
            */
            var radioButtonLabel = FindElement(By.CssSelector("div.radioGroup:nth-child(3) > label:nth-child(2)"), 500, 2000);
            radioButtonLabel.Click();
            Assert.AreEqual(true, FindElement(By.Id(radioButtonLabel.GetAttribute("for"))).Selected);


            /*
            Datepicker Buttons
            //Select Flight date 
            */
            var roundTrip = FindElement(By.Id("radio1"));
            roundTrip.FindElementIn(By.XPath("following-sibling::*")).Click();

            var DateboxDepart = FindElement(By.Id("checkInDateInput"));
            DateboxDepart.Click();
            
            var days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(2) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));
            var flyday = DateTime.Now.AddDays(5).Day;

            if (flyday < DateTime.Now.Day)
                days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(3) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));

            selectDate(days, flyday);

            if (roundTrip.Selected)
            {
                var returnDay = DateTime.Now.AddDays(23).Day;
                if (returnDay < flyday)
                    days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(3) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));

                selectDate(days, returnDay);
            }


            /*
            Combox Tests
            ////Select Class and Number of Passengers 
            */

            //Flight Class
            FindElement(By.ClassName("selectBox")).ComboBox().SelectByValue("businessClass");

            //Open Passengers Dialog
            FindElement(By.Id("occupancyField")).Click();
            //adults
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(1) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(1);
            //Children
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(2) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(2);
            //Seniors
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(3) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(1);
            //Infants
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(4) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(2);            
            //Click OK
            FindElement(By.ClassName("okBtn")).Click();


            //Search Flights
            FindElement(By.Id("findFlights")).Click();

            var results = FindElements(By.ClassName("flight_res_table"), 1000, 30000);
            Console.WriteLine(results.Count);
            Assert.AreNotEqual(0, results.Count);


            WebDriver.Dispose();
        }

        [TestMethod]
        public void FindElementSamples()
        {
            WebDriverParams = "{ \"Driver\":\"Chrome\" }";
            GoToUrl("https://www.lastminutetravel.com/packages");

            var elements = FindElements(By.XPath("//input[@type='radio']"));

            elements.ForEach(element => {
                element.FindElement(By.XPath("following-sibling::*")).Click();
            });
        }

        [TestMethod]
        public void GetDisplayedElementsSample()
        {

            WebDriverParams = "{ \"Driver\":\"Chrome\" }";
            GoToUrl("https://www.lastminutetravel.com/packages");

            var elements = GetDisplayedElements(By.XPath("//input[@type='radio']"));

            elements.ForEach(element => {
                element.FindElement(By.XPath("following-sibling::*")).Click();
            });
        }

        [TestMethod]
        public void WaitForDisplayedElementSample()
        {
            WebDriverParams = "{ \"Driver\":\"Firefox\" }";
            GoToUrl("https://www.lastminutetravel.com/flights");

            /*
            Textbox Tests
            //Set FROM and TO destinations
            //FROM
            */
            var fromDest = FindElement(By.Id("autosuggest-flightsFrom"));
            fromDest.SendKeys("auckland");
            fromDest.Click();
            FindElement(By.Id("react-autowhatever-1-section-0-item-2")).Click();

            ////TO
            var toDest = FindElement(By.Id("autosuggest-flightsTo"));
            toDest.SendKeys("sydney");
            toDest.Click();
            FindElement(By.Id("react-autowhatever-1-section-0-item-1")).Click();
            //Click Background - hide date popup
            FindElement(By.ClassName("bannerTitle")).Click();
            ////ASSERT
            Assert.AreEqual("Auckland - Aukland International Airport, New Zealand", fromDest.GetAttribute("value"));
            Assert.AreEqual("Sydney - All Airports, NS, Australia", toDest.GetAttribute("value"));

            /*
            Radio Buttons
            //Select One Way OR Round Trip Flight
            */
            var radioButtonLabel = FindElement(By.CssSelector("div.radioGroup:nth-child(3) > label:nth-child(2)"), 500, 2000);
            radioButtonLabel.Click();
            Assert.AreEqual(true, FindElement(By.Id(radioButtonLabel.GetAttribute("for"))).Selected);


            /*
            Datepicker Buttons
            //Select Flight date 
            */
            var roundTrip = FindElement(By.Id("radio1"));
            roundTrip.FindElementIn(By.XPath("following-sibling::*")).Click();

            var DateboxDepart = FindElement(By.Id("checkInDateInput"));
            DateboxDepart.Click();

            var days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(2) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));
            var flyday = DateTime.Now.AddDays(5).Day;

            if (flyday < DateTime.Now.Day)
                days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(3) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));

            selectDate(days, flyday);

            if (roundTrip.Selected)
            {
                var returnDay = DateTime.Now.AddDays(23).Day;
                if (returnDay < flyday)
                    days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(3) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));

                selectDate(days, returnDay);
            }


            /*
            Combox Tests
            ////Select Class and Number of Passengers 
            */

            //Flight Class
            FindElement(By.ClassName("selectBox")).ComboBox().SelectByValue("businessClass");

            //Open Passengers Dialog
            FindElement(By.Id("occupancyField")).Click();
            //adults
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(1) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(1);
            //Children
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(2) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(2);
            //Seniors
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(3) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(1);
            //Infants
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(4) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(2);
            //Click OK
            FindElement(By.ClassName("okBtn")).Click();


            //Search Flights
            FindElement(By.Id("findFlights")).Click();

            var temp = WaitForDisplayedElement(By.CssSelector("#matrixTD1 > div:nth-child(1) > div:nth-child(2)")).Text;

            Console.WriteLine(temp);

            WebDriver.Dispose();
        }

        [TestMethod]
        public void ScrollPageSample()
        {
            WebDriverParams = "{ \"Driver\":\"Firefox\" }";
            GoToUrl("https://www.lastminutetravel.com/flights");

            /*
            Textbox Tests
            //Set FROM and TO destinations
            //FROM
            */
            var fromDest = FindElement(By.Id("autosuggest-flightsFrom"));
            fromDest.SendKeys("auckland");
            fromDest.Click();
            FindElement(By.Id("react-autowhatever-1-section-0-item-2")).Click();

            ////TO
            var toDest = FindElement(By.Id("autosuggest-flightsTo"));
            toDest.SendKeys("sydney");
            toDest.Click();
            FindElement(By.Id("react-autowhatever-1-section-0-item-1")).Click();
            //Click Background - hide date popup
            FindElement(By.ClassName("bannerTitle")).Click();
            ////ASSERT
            Assert.AreEqual("Auckland - Aukland International Airport, New Zealand", fromDest.GetAttribute("value"));
            Assert.AreEqual("Sydney - All Airports, NS, Australia", toDest.GetAttribute("value"));

            /*
            Radio Buttons
            //Select One Way OR Round Trip Flight
            */
            var radioButtonLabel = FindElement(By.CssSelector("div.radioGroup:nth-child(3) > label:nth-child(2)"), 500, 2000);
            radioButtonLabel.Click();
            Assert.AreEqual(true, FindElement(By.Id(radioButtonLabel.GetAttribute("for"))).Selected);


            /*
            Datepicker Buttons
            //Select Flight date 
            */
            var roundTrip = FindElement(By.Id("radio1"));
            roundTrip.FindElementIn(By.XPath("following-sibling::*")).Click();

            var DateboxDepart = FindElement(By.Id("checkInDateInput"));
            DateboxDepart.Click();

            var days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(2) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));
            var flyday = DateTime.Now.AddDays(5).Day;

            if (flyday < DateTime.Now.Day)
                days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(3) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));

            selectDate(days, flyday);

            if (roundTrip.Selected)
            {
                var returnDay = DateTime.Now.AddDays(23).Day;
                if (returnDay < flyday)
                    days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(3) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));

                selectDate(days, returnDay);
            }


            /*
            Combox Tests
            ////Select Class and Number of Passengers 
            */

            //Flight Class
            FindElement(By.ClassName("selectBox")).ComboBox().SelectByValue("businessClass");

            //Open Passengers Dialog
            FindElement(By.Id("occupancyField")).Click();
            //adults
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(1) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(1);
            //Children
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(2) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(2);
            //Seniors
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(3) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(1);
            //Infants
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(4) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(2);
            //Click OK
            FindElement(By.ClassName("okBtn")).Click();


            //Search Flights
            FindElement(By.Id("findFlights")).Click();

            var results = FindElements(By.ClassName("flight_res_table"), 1000, 30000);
            Console.WriteLine(results.Count);
            Assert.AreNotEqual(0, results.Count);

            for (int i = 0; i < 3; i++)
            {
                WebDriver.ScrollBowserPage(2000 * i);
                Thread.Sleep(500);
            }

            WebDriver.Dispose();
        }

        [TestMethod]
        public void ActionsSample()
        {
            WebDriverParams = "{ \"Driver\":\"Firefox\" }";
            GoToUrl("https://www.lastminutetravel.com/flights");

            /*
            Right Click on element
            */
            FindElement(By.CssSelector("div.radioGroup:nth-child(3) > label:nth-child(2)")).Actions().ContextClick().Perform();

            Thread.Sleep(5000);
            WebDriver.Dispose();
        }

        [TestMethod]
        public void BannersSample()
        {
            WebDriverParams = "{ \"Driver\":\"Firefox\" }";
            GoToUrl("https://www.lastminutetravel.com/flights");

            WebDriver.BannersListner(By.CssSelector("i.icon-delete:nth-child(2)")); // May have to to clear cookies for banner to appear

            /*
            Textbox Tests
            //Set FROM and TO destinations
            //FROM
            */
            var fromDest = FindElement(By.Id("autosuggest-flightsFrom"));
            fromDest.SendKeys("auckland");
            fromDest.Click();
            FindElement(By.Id("react-autowhatever-1-section-0-item-2")).Click();

            ////TO
            var toDest = FindElement(By.Id("autosuggest-flightsTo"));
            toDest.SendKeys("sydney");
            toDest.Click();
            FindElement(By.Id("react-autowhatever-1-section-0-item-1")).Click();
            //Click Background - hide date popup
            FindElement(By.ClassName("bannerTitle")).Click();
            ////ASSERT
            Assert.AreEqual("Auckland - Aukland International Airport, New Zealand", fromDest.GetAttribute("value"));
            Assert.AreEqual("Sydney - All Airports, NS, Australia", toDest.GetAttribute("value"));

            /*
            Radio Buttons
            //Select One Way OR Round Trip Flight
            */
            var radioButtonLabel = FindElement(By.CssSelector("div.radioGroup:nth-child(3) > label:nth-child(2)"), 500, 2000);
            radioButtonLabel.Click();
            Assert.AreEqual(true, FindElement(By.Id(radioButtonLabel.GetAttribute("for"))).Selected);


            /*
            Datepicker Buttons
            //Select Flight date 
            */
            var roundTrip = FindElement(By.Id("radio1"));
            roundTrip.FindElementIn(By.XPath("following-sibling::*")).Click();

            var DateboxDepart = FindElement(By.Id("checkInDateInput"));
            DateboxDepart.Click();

            var days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(2) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));
            var flyday = DateTime.Now.AddDays(5).Day;

            if (flyday < DateTime.Now.Day)
                days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(3) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));

            selectDate(days, flyday);

            if (roundTrip.Selected)
            {
                var returnDay = DateTime.Now.AddDays(23).Day;
                if (returnDay < flyday)
                    days = FindElement(By.CssSelector("div.CalendarMonth:nth-child(3) > table:nth-child(1) > tbody:nth-child(2)")).FindElementsIn(By.TagName("td"));

                selectDate(days, returnDay);
            }


            /*
            Combox Tests
            ////Select Class and Number of Passengers 
            */

            //Flight Class
            FindElement(By.ClassName("selectBox")).ComboBox().SelectByValue("businessClass");

            //Open Passengers Dialog
            FindElement(By.Id("occupancyField")).Click();
            //adults
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(1) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(1);
            //Children
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(2) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(2);
            //Seniors
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(3) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(1);
            //Infants
            FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(4) > div:nth-child(2) > select:nth-child(1)")).ComboBox().SelectByIndex(2);
            //Click OK
            FindElement(By.ClassName("okBtn")).Click();


            //Search Flights
            FindElement(By.Id("findFlights")).Click();

            var results = FindElements(By.ClassName("flight_res_table"), 1000, 30000);
            Console.WriteLine(results.Count);
            Assert.AreNotEqual(0, results.Count);


            WebDriver.Dispose();
        }

       [TestMethod]
        public void testTest()
        {
            var today = DateTime.Now;

            var dayToday = DateTime.Now.AddDays(25).Day; //var dayToday = today.Day;
            string monthToday = today.ToString("MMMM");
            var yearToday = DateTime.Today.Year;

            Console.WriteLine(dayToday + ", " + monthToday + ", " + yearToday);
        }


        /***
         * Supporting Functions
         ***/
        public void selectDate(List<IWebElement> days, int desiredDay)
        {




            foreach (IWebElement day in days)
            {
                if (day.Text == desiredDay.ToString())
                {
                    day.Click();
                    break;
                }
            }
        }
        

        //public void initializeDriver(IWebDriver driverType, string url = "")
        //{
        //    if (driver == null)
        //    {
        //        driver = driverType;
        //    }
        //    else
        //    {
        //        driverType.Dispose();
        //    }

        //    if (url != "" && driver.Url != url)
        //    {
        //        driver.Navigate().GoToUrl(url);
        //    }

        //    driver.Manage().Window.Maximize();
        //}

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        ///// <summary>
        ///// OBSELETE Tests
        ///// </summary>
        ////Enter From and TO destinations
        //[TestMethod]
        //public void TextBoxTest()
        //{
        //    ////FROM
        //    //var fromDest = driver.FindElement(By.Id("autosuggest-flightsFrom"));
        //    //fromDest.SendKeys("auckland");
        //    //fromDest.Click();
        //    //Thread.Sleep(1500);
        //    //var fromDropDown = driver.FindElement(By.Id("react-autowhatever-1-section-0-item-1"));
        //    //fromDropDown.Click();
        //    //Thread.Sleep(1000);

        //    ////TO
        //    //var toDest = driver.FindElement(By.Id("autosuggest-flightsTo"));
        //    //toDest.SendKeys("dunedin");
        //    //toDest.Click();
        //    //Thread.Sleep(1500);
        //    //var toDropDown = driver.FindElement(By.Id("react-autowhatever-1-section-0-item-0"));
        //    //toDropDown.Click();

        //    ////ASSERT
        //    //Thread.Sleep(1000);
        //    //Assert.AreEqual("Auckland - All Airports, New Zealand", fromDest.GetAttribute("value"));
        //    //Assert.AreEqual("Dunedin - Dunedin Airport, New Zealand", toDest.GetAttribute("value"));
        //}

        ////Select One Way OR Round Trip Flight
        //[TestMethod]
        //public void RadioButtonTest()
        //{
        //    //var radioButtonLabel = driver.FindElement(By.CssSelector("div.radioGroup:nth-child(3) > label:nth-child(2)"));

        //    //var radioButton = driver.FindElement(By.Id(radioButtonLabel.GetAttribute("for")));

        //    //radioButtonLabel.Click();

        //    //Thread.Sleep(1000);

        //    //Assert.AreEqual(true, radioButton.Selected);
        //}

        ////Select Flight date
        //[TestMethod]
        //public void DatepickerTest()
        //{
        //    //var DateboxDepart = driver.FindElement(By.Id("checkInDateInput")); DateboxDepart.Click();
        //    //Thread.Sleep(1000);

        //    //ReadOnlyCollection<IWebElement> days = driver.FindElement(By.CssSelector("div.CalendarMonth:nth-child(2) > table:nth-child(1) > tbody:nth-child(2)")).FindElements(By.TagName("td"));
        //    //var dayToday = DateTime.Now.Day;
        //    //selectDate(days, dayToday);

        //    //var roundTrip = driver.FindElement(By.Id("radio1"));

        //    //if (roundTrip.Selected)
        //    //{
        //    //    var returnDay = DateTime.Now.AddDays(23).Day;
        //    //    if (returnDay < dayToday)
        //    //        days = driver.FindElement(By.CssSelector("div.CalendarMonth:nth-child(3) > table:nth-child(1) > tbody:nth-child(2)")).FindElements(By.TagName("td"));

        //    //    selectDate(days, returnDay);
        //    //}

        //}

        ////Select Class and Number of Passengers
        //[TestMethod]
        //public void comboBoxTest()
        //{
        //    //var flightClass = new SelectElement(driver.FindElement(By.ClassName("selectBox")));
        //    //flightClass.SelectByIndex(1);

        //    //var passengers = driver.FindElement(By.Id("occupancyField"));
        //    //passengers.Click();
        //    //Thread.Sleep(500);

        //    //var adults = new SelectElement(driver.FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(1) > div:nth-child(2) > select:nth-child(1)")));
        //    //var children = new SelectElement(driver.FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(2) > div:nth-child(2) > select:nth-child(1)")));
        //    //var seniors = new SelectElement(driver.FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(3) > div:nth-child(2) > select:nth-child(1)")));
        //    //var infants = new SelectElement(driver.FindElement(By.CssSelector("div.tr-row:nth-child(2) > div:nth-child(4) > div:nth-child(2) > select:nth-child(1)")));

        //    //adults.SelectByIndex(4);
        //    //children.SelectByIndex(3);
        //    //seniors.SelectByIndex(1);
        //    //infants.SelectByIndex(2);

        //    //Thread.Sleep(1500);

        //    //var okButton = driver.FindElement(By.ClassName("okBtn"));
        //    //okButton.Click();
        //}


        //[TestMethod]
        //public void multipleBrowserTest()
        //{
        //    //IWebDriver webDriver = new FirefoxDriver();
        //    //Thread.Sleep(1000);
        //    //webDriver.Dispose();

        //    //webDriver = new InternetExplorerDriver();
        //    //Thread.Sleep(1000);
        //    //webDriver.Dispose();

        //    //webDriver = new ChromeDriver();
        //    //Thread.Sleep(1000);
        //    //webDriver.Dispose();
        //}

    }
}
