using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebDriverWrapper
{
    public class SeleniumHandler
    {
        public string WebDriverParams
        {
            get
            {
                return webDriverParams;
            }
            set
            {
                webDriverParams = value;
            }
        }

        public IWebDriver WebDriver
        {
            get
            {
                if (webDriver == null)
                {
                    webDriver = SetWebDriver();
                }
                return webDriver;
            }
        }

        private string webDriverParams = "{ \"Driver\":\"Firefox\" }";

        private IWebDriver webDriver = null;

        private IWebDriver SetWebDriver()
        {
            try
            {
                var driverParams = JObject.Parse(WebDriverParams);
                if (driverParams["Driver"].ToString()=="Firefox")
                {
                    return setFirefoxDriver();
                }
                else if (driverParams["Driver"].ToString() == "IE")
                {
                    return setInternetExplorerDriver();
                }
                else if (driverParams["Driver"].ToString() == "Chrome")
                {
                    return setChromeDriver();
                }

                return setFirefoxDriver();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IWebDriver setFirefoxDriver()
        {
            try
            {                
                var options = new FirefoxOptions();
                options.Profile = new FirefoxProfile();
                //options.AcceptInsecureCertificates = true;
                options.Profile.AcceptUntrustedCertificates = true;
                options.Profile.DeleteAfterUse = true;

                return new FirefoxDriver(options);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IWebDriver setChromeDriver()
        {
            try
            {
                return new ChromeDriver();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IWebDriver setInternetExplorerDriver()
        {
            try
            {
                var options = new InternetExplorerOptions();
                options.EnsureCleanSession = true;
                //options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;                
                options.UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss;
                options.PageLoadStrategy = PageLoadStrategy.Normal;

                return new InternetExplorerDriver(options);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GoToUrl(string url)
        {
            try
            {
                WebDriver.Navigate().GoToUrl(url);
                WebDriver.Manage().Window.Maximize();
                WebDriver.SwitchTo().ActiveElement();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IWebElement FindElement(By by, int interval = 500, int timeout = 15000)
        {
            IWebElement webElement = null;
            var tick = 0;
            try
            {
                do
                {
                    try
                    {
                        webElement = WebDriver.FindElement(by);
                    }
                    catch
                    {
                        Thread.Sleep(interval);
                        tick += interval;
                    }

                } while (webElement == null && tick < timeout);

                if(webElement == null)
                {
                    throw new TimeoutException(string.Format("Element(s) were not found within {0}sec.",(timeout/1000).ToString()));
                }
                return webElement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IWebElement> FindElements(By by, int interval = 500, int timeout = 15000)
        {
            var elements = new List<IWebElement>();
            var tick = 0;
            try
            {
                do
                {
                    try
                    {
                        elements = WebDriver.FindElements(by).ToList();
                        if (elements.Count == 0)
                        {
                            Thread.Sleep(interval);
                            tick += interval;
                        }
                    }
                    catch
                    {
                        Thread.Sleep(interval);
                        tick += interval;
                    }
                } while (elements.Count==0 && tick < timeout);
                return elements;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IWebElement GetDisplayedElement(By by, int interval = 500, int timeout = 15000)
        {
            try
            {
                var elements = FindElements(by, interval, timeout);

                foreach (IWebElement element in elements)
                {
                    if (element.Displayed)
                    {
                        return element;
                    }
                }
                throw new ElementNotVisibleException();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<IWebElement> GetDisplayedElements(By by, int interval = 500, int timeout = 15000)
        {
            try
            {
                var elements = FindElements(by, interval, timeout);
                var DisplayedElements = new List<IWebElement>();

                elements.ForEach(element =>
                {
                    if (element.Displayed)
                    {
                        DisplayedElements.Add(element);
                    }
                });
                return DisplayedElements;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IWebElement WaitForDisplayedElement(By by, int interval = 500, int timeout = 15000)
        {
            try
            {
                IWebElement webElement = null;
                var tick = 0;
                do
                {
                    try
                    {
                        webElement = FindElement(by, interval, timeout);
                        if (!webElement.Displayed)
                        {
                            throw new ElementNotVisibleException();
                        }
                        return webElement;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(interval);
                        tick += interval;
                    }

                } while (tick < timeout);

                throw new TimeoutException(string.Format("Element(s) were not found within {0}sec.", (timeout / 1000).ToString()));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
