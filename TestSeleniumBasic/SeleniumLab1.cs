using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;


namespace SeleniumWDTestProject
{
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public bool isElementPresent(By locator)
        {
            try
            {
                driver.FindElement(locator);
            }
            catch (NoSuchElementException)
            {
                return true;
            }
            return false;
        }

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:5000/");
            driver.Manage().Window.Maximize();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Timeout = TimeSpan.FromSeconds(5);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            string login = "user";
            string password = "user";

            driver.FindElement(By.Id("Name")).SendKeys(login);
            driver.FindElement(By.Id("Password")).SendKeys(password);
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();

            Assert.AreEqual(driver.FindElement(By.XPath("//h2[text()=\"Home page\"]")).Text, "Home page");
        }

        [Test]
        public void AddNewProduct()
        {
            driver.FindElement(By.XPath("//div/a[@href=\"/Product\"]")).Click();
            driver.FindElement(By.XPath("//a[@class=\"btn btn-default\"]")).Click();
            driver.FindElement(By.XPath("//input[@id=\"ProductName\"]")).SendKeys("Borovichok Mushroom");
            new SelectElement(driver.FindElement(By.XPath("//select[@id=\"CategoryId\"]"))).SelectByText("Produce");
            new SelectElement(driver.FindElement(By.XPath("//select[@id=\"SupplierId\"]"))).SelectByText("Grandma Kelly's Homestead");
            driver.FindElement(By.XPath("//input[@id=\"UnitPrice\"]")).SendKeys("4");
            driver.FindElement(By.XPath("//input[@id=\"QuantityPerUnit\"]")).SendKeys("10 boxes x 20 bags");
            driver.FindElement(By.XPath("//input[@id=\"UnitsInStock\"]")).SendKeys("2");
            driver.FindElement(By.XPath("//input[@id=\"UnitsOnOrder\"]")).SendKeys("2");
            driver.FindElement(By.XPath("//input[@id=\"ReorderLevel\"]")).SendKeys("2");

            driver.FindElement(By.XPath("//input[@type=\"submit\"]")).Click();

            Assert.IsTrue(isElementPresent(By.XPath("//input[@id=\"ProductName\"]")));
        }

        [Test]
        public void CheckNewProduct()
        {
            driver.FindElement(By.XPath("//div/a[@href=\"/Product\"]")).Click();
            driver.FindElement(By.XPath("//a[contains (text(), 'Bor')]")).Click();

            Assert.AreEqual(driver.FindElement(By.XPath("//input[@id=\"ProductName\"]")).GetAttribute("value"), "Borovichok Mushroom");
            Assert.AreEqual(driver.FindElement(By.XPath("//select[@id=\"CategoryId\"]/option[@value=\"7\"]")).Text, "Produce");
            Assert.AreEqual(driver.FindElement(By.XPath("//select[@id=\"SupplierId\"]/option[@value=\"3\"]")).Text, "Grandma Kelly's Homestead");
            Assert.AreEqual(driver.FindElement(By.XPath("//input[@id=\"UnitPrice\"]")).GetAttribute("value"), "4,0000");
            Assert.AreEqual(driver.FindElement(By.XPath("//input[@id=\"QuantityPerUnit\"]")).GetAttribute("value"), "10 boxes x 20 bags");
            Assert.AreEqual(driver.FindElement(By.XPath("//input[@id=\"UnitsInStock\"]")).GetAttribute("value"), "2");
            Assert.AreEqual(driver.FindElement(By.XPath("//input[@id=\"UnitsOnOrder\"]")).GetAttribute("value"), "2");
            Assert.AreEqual(driver.FindElement(By.XPath("//input[@id=\"ReorderLevel\"]")).GetAttribute("value"), "2");
        }

        [Test]
        public void DeleteProduct()
        {
            driver.FindElement(By.XPath("//div/a[@href=\"/Product\"]")).Click();
            driver.FindElement(By.XPath("(//a[contains(text(), 'Rem')])[last()]")).Click();

            driver.SwitchTo().Alert().Accept();
        }

        [TearDown]
        public void TearDown()
        {
            driver.FindElement(By.LinkText("Logout")).Click();
            Assert.AreEqual(driver.FindElement(By.XPath("//h2")).Text, "Login");
            driver.Quit();
        }
    }
}