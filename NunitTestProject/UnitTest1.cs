using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TUTAAutomation
{
    public class Tests
    {
        private IWebDriver driver;
        private string validEmail = "tuta@test.lt";
        private string validPassword = "testing123";
        private string itemName = "Blouse";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "http://automationpractice.com/index.php";

        }

        [Test]
        public void Purchase()
        {
            Login(validEmail, validPassword);

            IWebElement accountInfo = driver.FindElement(By.CssSelector("#header > div.nav > div > div > nav > div:nth-child(1) > a"));
            Assert.AreEqual("Testa Tester", accountInfo.Text, "Account info is incorrect");

            SearchItem(itemName);
            IWebElement searchInfo = driver.FindElement(By.CssSelector("#center_column > ul.product_list > li.ajax_block_product > div.product-container > div.right-block > h5 > a"));
            Assert.AreEqual("Blouse", searchInfo.Text, "No results were found for your search");

            BuyItem();
            IWebElement orderInfo = driver.FindElement(By.CssSelector("#center_column > h1"));
            Assert.AreEqual("ORDER CONFIRMATION", orderInfo.Text, "Something went wrong with your order");

        }

        public void SearchItem(string itemName)
        {
            driver.FindElement(By.Id("search_query_top")).SendKeys(itemName);
            driver.FindElement(By.CssSelector("#searchbox > button.button-search")).Click();
        }

        public void BuyItem()
        {
            // Click on item name to open product page
            driver.FindElement(By.CssSelector("#center_column > ul.product_list > li.ajax_block_product > div.product-container > div.right-block > h5 > a")).Click();

            // Click on "Add to cart" button
            driver.FindElement(By.CssSelector("#add_to_cart > button")).Click();

            // Click on "Proceed to checkout" button in pop up window
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            IWebElement element = driver.FindElement(By.CssSelector("#layer_cart > div.clearfix > div.layer_cart_cart > div.button-container > a"));
            executor.ExecuteScript("arguments[0].click();", element);

            // Reload page to see the item in the cart
            driver.Navigate().Refresh();

            // Summary tab
            driver.FindElement(By.CssSelector("#center_column > p.cart_navigation > a.standard-checkout")).Click();

            // Address tab
            driver.FindElement(By.CssSelector("#center_column > form > p.cart_navigation > button")).Click();

            // Shipping tab
            driver.FindElement(By.CssSelector("#uniform-cgv > span > input")).Click();
            driver.FindElement(By.CssSelector("#form > p.cart_navigation > button ")).Click();

            // Payment tab
            driver.FindElement(By.CssSelector("#HOOK_PAYMENT > div:first-child > div > p.payment_module > a")).Click();

            // Order confirmation
            driver.FindElement(By.CssSelector("#cart_navigation > button")).Click();
        }


        public void Login(string email, string password)
        {
            driver.FindElement(By.CssSelector("#header > div.nav > div > div > nav > div.header_user_info > a")).Click();

            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("passwd")).SendKeys(password);
            driver.FindElement(By.Id("SubmitLogin")).Click();
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}