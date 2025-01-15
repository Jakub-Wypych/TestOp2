using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

[TestFixture]
public class AddProductPageTests
{
    private IWebDriver driver;
    private WebDriverWait wait;

    [SetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        driver.Navigate().GoToUrl("https://localhost:7086/add-product");
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }


    [Test]
    public void ShouldShowErrorMessageForProductName()
    {
        var addButton = driver.FindElement(By.CssSelector("button.btn.btn-primary"));
        addButton.Click();

        var nameError = wait.Until(driver => driver.FindElement(By.CssSelector("div.invalid-feedback")));
        Assert.IsTrue(nameError.Text.Contains("Product Name is required."), "Komunikat o błędzie dla nazwy produktu nie został wyświetlony.");
    }

    [Test]
    public void ShouldShowErrorMessageForProductCategory()
    {
        var nameInput = driver.FindElement(By.Id("productName"));
        nameInput.SendKeys("New Product");

        var dateInput = driver.FindElement(By.Id("productDate"));
        dateInput.SendKeys("2025-01-01");

        var priceInput = driver.FindElement(By.Id("productPrice"));
        priceInput.SendKeys("10");

        var quantityInput = driver.FindElement(By.Id("stockQuantity"));
        quantityInput.SendKeys("100");

        var addButton = driver.FindElement(By.CssSelector("button.btn.btn-primary"));
        addButton.Click();

        var categoryError = wait.Until(driver => driver.FindElement(By.CssSelector("div.invalid-feedback")));
        Assert.IsTrue(categoryError.Text.Contains("Product Category is required."), "Komunikat o błędzie dla kategorii produktu nie został wyświetlony." + categoryError.Text);
    }


    [Test]
    public void ShouldAddProductSuccessfully()
    {
        var nameInput = driver.FindElement(By.Id("productName"));
        nameInput.SendKeys("Test");

        var dateInput = driver.FindElement(By.Id("productDate"));
        dateInput.SendKeys("2024-11-01");

        var priceInput = driver.FindElement(By.Id("productPrice"));
        priceInput.SendKeys("10");

        var categoryInput = driver.FindElement(By.Id("productCategory"));
        categoryInput.SendKeys("Category A");

        var quantityInput = driver.FindElement(By.Id("stockQuantity"));
        quantityInput.SendKeys("1000");

        var addButton = driver.FindElement(By.CssSelector("button.btn.btn-primary"));
        addButton.Click();

        var successMessage = wait.Until(driver => driver.FindElement(By.CssSelector("div.alert.alert-success")));
        Assert.IsNotNull(successMessage, "Komunikat o sukcesie nie został wyświetlony.");
        Assert.IsTrue(successMessage.Text.Contains("Product 'Test' added successfully"), "Komunikat o sukcesie nie zawiera oczekiwanego tekstu.");
    }
}
