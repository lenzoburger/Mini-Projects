using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebDriverWrapper.Extensions
{
    public static class IWebElementExtensions
    {
        public static SelectElement ComboBox(this IWebElement webElement)
        {
            try
            {
                return new SelectElement(webElement);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IWebElement FindElementIn(this IWebElement parentElement, By by, int interval = 500, int timeout = 15000)
        {
            IWebElement webElement = null;
            var tick = 0;
            try
            {
                do
                {
                    try
                    {
                        webElement = parentElement.FindElement(by);
                    }
                    catch
                    {
                        Thread.Sleep(interval);
                        tick += interval;
                    }

                } while (webElement == null && tick < timeout);

                if (webElement == null)
                {
                    throw new TimeoutException(string.Format("Element(s) were not found within {0}sec.", (timeout / 1000).ToString()));
                }
                return webElement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<IWebElement> FindElementsIn(this IWebElement parentElement, By by, int interval = 500, int timeout = 15000)
        {
            var elements = new List<IWebElement>();
            var tick = 0;
            try
            {
                do
                {
                    try
                    {
                        elements = parentElement.FindElements(by).ToList();
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
                } while (elements.Count == 0 && tick < timeout);
                return elements;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Actions Actions(this IWebElement webElement)
        {
            try
            {
                var driver = ((IWrapsDriver)webElement).WrappedDriver;
                var actions = new Actions(driver);
                actions.MoveToElement(webElement);
                return actions;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
