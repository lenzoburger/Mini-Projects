
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System;

class Selectors
{

    static IWebDriver driver = new ChromeDriver();

    static IWebElement textBox;

    static IWebElement checkBox;

    static IWebElement radioButtons;

    static IWebElement dropDownMenu;

    static IAlert alertBox;
    static IWebElement image;

    static void Main()
    {
        //////Declare, Define & Initialize
        int lowWaitTime = 10;
        int highWaitTime = 2000;

        string nameUrl = "http://testing.todvachev.com/selectors/name/";
        string idUrl = "http://testing.todvachev.com/selectors/id/";
        string classUrl = "http://testing.todorvachev.com/selectors/class-name/";
        string cssPathUrl = "http://testing.todorvachev.com/selectors/css-path/";
        string xPathUrl = "http://testing.todvachev.com/about/";
        string textBoxUrl = "http://testing.todorvachev.com/special-elements/text-input-field/";
        string checkBoxUrl = "http://testing.todorvachev.com/special-elements/check-button-test-3/";
        string radioButtonUrl = "http://testing.todorvachev.com/special-elements/radio-button-test/";
        string dropDownUrl = "http://testing.todorvachev.com/special-elements/drop-down-menu-test/";
        string alertUrl = "http://testing.todorvachev.com/special-elements/alert-box/";

        string option = "1"; //'3'
        string[] radioOptions = { "1", "3", "5" };
        string[] dropDownOptions = { "1", "2", "3", "4"};
        string dropDownSelect = dropDownOptions[1];

        string alertImgCssSel = "#post-119 > div > figure > img";
        string checkBoxCssSel = "#post-33 > div > p:nth-child(8) > input[type=\"checkbox\"]:nth-child(" + option + ")";        
        string cssPath = "#post-108 > div > figure > img";
        string xPath = "//*[@id=\"recent-posts-2\"]/ul/li[3]";

        //find elements by selector types
        IWebElement nameElem = findElement(driver, By.Name("myName"), nameUrl); Thread.Sleep(lowWaitTime);
        IWebElement idElem = findElement(driver, By.Id("testImage"), idUrl); Thread.Sleep(lowWaitTime);
        IWebElement classElem = findElement(driver, By.ClassName("testClass"), classUrl); Thread.Sleep(lowWaitTime);
        ////TASKS -> ClassName text
        Console.WriteLine(classElem.Text);

        IWebElement cssSelElem = findElement(driver, By.CssSelector(cssPath), cssPathUrl); Thread.Sleep(lowWaitTime);
        IWebElement xpathElem = findElement(driver, By.XPath(xPath), xPathUrl); Thread.Sleep(lowWaitTime);


        //Perform taks with elements
        //
        //
        ////TASKS -> TextBox
        textBox = findElement(driver, By.Name("username"), textBoxUrl);
        textBox.SendKeys("Test text"); Thread.Sleep(lowWaitTime);
        Console.WriteLine("Value: " + textBox.GetAttribute("value"));
        Console.WriteLine("length: " + textBox.GetAttribute("maxlength"));
        Thread.Sleep(lowWaitTime);

        ////TASKS -> Checkbox
        checkBox = findElement(driver, By.CssSelector(checkBoxCssSel), checkBoxUrl);
        isChecked(checkBox);        Thread.Sleep(lowWaitTime);
        checkBox.Click();
        isChecked(checkBox);        Thread.Sleep(lowWaitTime);


        ////TASKS -> RadioButtons
        driver.Navigate().GoToUrl(radioButtonUrl);
        foreach (string rButton in radioOptions)
        {
            string rButtonCssSel = "#post-10 > div > form > p:nth-child(6) > input[type=\"radio\"]:nth-child("+ rButton+")";
            radioButtons = findElement(driver, By.CssSelector(rButtonCssSel));
            isChecked(radioButtons);
        }


        ////TASKS -> DropDownMenu
        dropDownMenu = findElement(driver, By.Name("DropDownTest"), dropDownUrl); Thread.Sleep(lowWaitTime);
        ColouredMessage(ConsoleColor.DarkCyan, "Selected: " + dropDownMenu.GetAttribute("value"));
        foreach (string dropDnOpt in dropDownOptions)
        {
            string dropDownOptionCss = "#post-6 > div > p:nth-child(6) > select > option:nth-child("+ dropDnOpt+")";
            IWebElement menuOption = findElement(driver, By.CssSelector(dropDownOptionCss));
            ColouredMessage(ConsoleColor.DarkMagenta, menuOption.GetAttribute("value"));
            if (dropDownSelect == dropDnOpt)
            {
                menuOption.Click();
            }            
        }
        ColouredMessage(ConsoleColor.DarkCyan, "Selected: " + dropDownMenu.GetAttribute("value"));


        ////TASKS -> AlertBoxes
        driver.Navigate().GoToUrl(alertUrl);
        alertBox = driver.SwitchTo().Alert();        alertBox.Accept(); //make go away
        image = findElement(driver, By.CssSelector(alertImgCssSel));


        //Cleanup
        Thread.Sleep(lowWaitTime);
        driver.Quit();
    }

    private static void isChecked(IWebElement buttonBox)
    {
        string attr = "checked";
        try
        {
            bool isChecked = bool.TryParse(buttonBox.GetAttribute(attr), out isChecked);
            if (isChecked)
            {
                ColouredMessage(ConsoleColor.Blue, "Checkbox/RadioBUtton is checked!");
            }
            else
            {
                ColouredMessage(ConsoleColor.Yellow, "Checkbox/RadioBUtton is NOT checked!");
            }
        }
        catch (NoSuchElementException)
        {
            ColouredMessage(ConsoleColor.Red, "Couldn't find attribute/element: "+ attr);
        }
    }

    private static IWebElement findElement(IWebDriver driver, By selectorType, String url= "")
    {
        if(url != "")
        {
            driver.Navigate().GoToUrl(url);
        }        


        try
        {
            IWebElement element = driver.FindElement(selectorType);
            if (element.Displayed)
            {
                ColouredMessage(ConsoleColor.Green, "\nYes! I can see the element [" + selectorType.ToString() + "], it's right there!!!");
                return element;
            }
        }
        catch (NoSuchElementException)
        {
            ColouredMessage(ConsoleColor.Red, "\nWell, something went wrong, I couldn't see the element [" + selectorType.ToString() + "].");
        }
        catch (UnhandledAlertException)
        {
            IAlert alertNow = driver.SwitchTo().Alert();
            ColouredMessage(ConsoleColor.Red, "\n\nThere was an unhandled/UnExpected Alert:");
            ColouredMessage(ConsoleColor.Magenta, "\n\t" + alertNow.Text + "\n");
        }


        return null;
    }

    private static void ColouredMessage(ConsoleColor colour, string message)
    {
        Console.ForegroundColor = colour;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }
}
