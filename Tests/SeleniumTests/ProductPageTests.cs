using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ProductApp.Tests.SeleniumTests
{
    [TestFixture]
    public class ProductPageTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl("https://localhost:7086/products");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void ShouldDisplayProductList()
        {
            var productTable = driver.FindElement(By.TagName("table"));
            Assert.IsNotNull(productTable, "Tabela produktów nie została znaleziona.");
        }

        [Test]
        public void ShouldNavigateToEditPage()
        {
            var editButton = driver.FindElement(By.XPath("//table/tbody/tr[1]/td/button[contains(text(), 'Edit')]"));
            editButton.Click();
            Assert.IsTrue(driver.Url.Contains("/edit-product/"));
        }

        [Test]
        public void ShouldDeleteProduct()
        {
            var productRowsBeforeDelete = driver.FindElements(By.XPath("//table/tbody/tr"));
            var countBeforeDelete = productRowsBeforeDelete.Count;

            var deleteButton = driver.FindElement(By.XPath("//table/tbody/tr[1]/td/button[contains(text(), 'Delete')]"));
            deleteButton.Click();


            driver.SwitchTo().Alert().Accept();


            wait.Until(driver =>
            {
                var productRowsAfterDelete = driver.FindElements(By.XPath("//table/tbody/tr"));
                return productRowsAfterDelete.Count == countBeforeDelete - 1;
            });


            var productRowsAfterDelete = driver.FindElements(By.XPath("//table/tbody/tr"));
            var countAfterDelete = productRowsAfterDelete.Count;

            Assert.AreEqual(countBeforeDelete - 1, countAfterDelete, "Produkt nie został usunięty.");
        }

        [Test]
        public void ShouldLoadProductsCorrectly()
        {
            var productRows = driver.FindElements(By.XPath("//table/tbody/tr"));
            Assert.IsTrue(productRows.Count > 0, "Nie załadowano produktów.");
        }

        [Test]
        public void ShouldNavigateToProductPageAndCheckDetails()
        {
            var productRow = driver.FindElement(By.XPath("//table/tbody/tr[1]"));
            var productName = productRow.FindElement(By.XPath("./td[1]")).Text;

            var editButton = productRow.FindElement(By.XPath("./td/button[contains(text(), 'Edit')]"));
            editButton.Click();

            var editUrl = driver.Url;
            Assert.IsTrue(editUrl.Contains("/edit-product/"), "Nie przekierowano do strony edycji produktu.");

            var nameField = wait.Until(driver => driver.FindElement(By.Id("productName")));
            Assert.AreEqual(productName, nameField.GetAttribute("value"), "Nazwa produktu na stronie edycji nie jest zgodna.");
        }



        [Test]
        public void ShouldSortByNameAscending()
        {
            var sortByNameButton = driver.FindElement(By.CssSelector("button.btn.btn-secondary"));
            sortByNameButton.Click();

            var productNames = driver.FindElements(By.CssSelector("table tbody tr td:first-child"))
                .Select(element => element.Text)
                .ToList();

            var sortedNames = productNames.OrderBy(name => name).ToList();

            Assert.AreEqual(sortedNames, productNames, "Produkty nie zostały posortowane rosnąco po nazwie.");
        }

        [Test]
        public void ShouldSortByDateAscending()
        {
            var sortByDateButton = driver.FindElement(By.CssSelector("button.btn.btn-secondary:nth-child(2)"));
            sortByDateButton.Click();

            var productDates = driver.FindElements(By.CssSelector("table tbody tr td:nth-child(2)"))
                .Select(element =>
                {
                    return DateTime.TryParseExact(
                        element.Text,
                        "MM-dd-yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var date) ? date : DateTime.MinValue;
                })
                .ToList();

            var sortedDates = productDates.OrderBy(date => date).ToList();

            Assert.AreEqual(sortedDates, productDates, "Produkty nie zostały posortowane rosnąco po dacie.");
        }

        [Test]
        public void ShouldSortByQuantityAscending()
        {
            var sortByQuantityButton = driver.FindElement(By.CssSelector("button.btn.btn-secondary:nth-child(4)"));
            sortByQuantityButton.Click();

            var productQuantities = driver.FindElements(By.CssSelector("table tbody tr td:nth-child(5)"))
                .Select(element => int.TryParse(element.Text, out var quantity) ? quantity : 0)
                .ToList();

            var sortedQuantities = productQuantities.OrderBy(quantity => quantity).ToList();

            Assert.AreEqual(sortedQuantities, productQuantities, "Produkty nie zostały posortowane rosnąco po ilości.");
        }


        [Test]
        public void ShouldSortByNameDescending()
        {
            var sortByNameButton = driver.FindElement(By.CssSelector("button.btn.btn-secondary"));
            sortByNameButton.Click(); 
            sortByNameButton.Click(); 

            var productNames = driver.FindElements(By.CssSelector("table tbody tr td:first-child"))
                .Select(element => element.Text)
                .ToList();

            var sortedNames = productNames.OrderByDescending(name => name).ToList();

            Assert.AreEqual(sortedNames, productNames, "Produkty nie zostały posortowane malejąco po nazwie.");
        }

        [Test]
        public void ShouldSortByPriceDescending()
        {
            var sortByPriceButton = driver.FindElement(By.CssSelector("button.btn.btn-secondary:nth-child(3)"));
            sortByPriceButton.Click(); 
            sortByPriceButton.Click();

            var productPrices = driver.FindElements(By.CssSelector("table tbody tr td:nth-child(3)"))
                .Select(element =>
                {
                    var priceText = element.Text.TrimStart('$');
                    return decimal.TryParse(priceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var price) ? price : 0m;
                })
                .ToList();

            var sortedPrices = productPrices.OrderByDescending(price => price).ToList();

            Assert.AreEqual(sortedPrices, productPrices, "Produkty nie zostały posortowane malejąco po cenie.");
        }

        [Test]
        public void ShouldSortByDateDescending()
        {
            var sortByDateButton = driver.FindElement(By.CssSelector("button.btn.btn-secondary:nth-child(2)"));
            sortByDateButton.Click(); 
            sortByDateButton.Click();

            var productDates = driver.FindElements(By.CssSelector("table tbody tr td:nth-child(2)"))
                .Select(element =>
                {
                    return DateTime.TryParseExact(
                        element.Text,
                        "MM-dd-yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var date) ? date : DateTime.MinValue;
                })
                .ToList();

            var sortedDates = productDates.OrderByDescending(date => date).ToList();

            Assert.AreEqual(sortedDates, productDates, "Produkty nie zostały posortowane malejąco po dacie.");
        }

        [Test]
        public void ShouldSortByQuantityDescending()
        {
            var sortByQuantityButton = driver.FindElement(By.CssSelector("button.btn.btn-secondary:nth-child(4)"));
            sortByQuantityButton.Click();
            sortByQuantityButton.Click(); 

            var productQuantities = driver.FindElements(By.CssSelector("table tbody tr td:nth-child(5)"))
                .Select(element => int.TryParse(element.Text, out var quantity) ? quantity : 0)
                .ToList();

            var sortedQuantities = productQuantities.OrderByDescending(quantity => quantity).ToList();

            Assert.AreEqual(sortedQuantities, productQuantities, "Produkty nie zostały posortowane malejąco po ilości.");
        }
    }
}
