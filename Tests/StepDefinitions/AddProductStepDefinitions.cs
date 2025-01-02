using BLZR.Pages;
using NUnit.Framework;
using ProductApp.Models;

namespace Tests.StepDefinitions
{
    [Binding]
    public class AddProductStepDefinitions
    {
        private Dictionary<string, object> _formData;
        private string _resultMessage;
        private List<Product> _productList;

        public AddProductStepDefinitions()
        {
            _formData = new Dictionary<string, object>();
            _resultMessage = string.Empty;
            _productList = new List<Product>();
        }

        [Given("the user is on the \"Add Product\" page")]
        public void GivenTheUserIsOnTheAddProductPage()
        {
            // Simulate navigating to the Add Product page
            _formData.Clear();
            _resultMessage = string.Empty;
        }

        [When(@"the user fills in the form with valid data \(""Name"": ""(.*)"", ""Date"": ""(.*)"", ""Price"": (.*), ""Category"": ""(.*)"", ""Quantity"": (.*)\)")]
        public void WhenTheUserFillsInTheFormWithValidData(string name, string date, decimal price, string category, int quantity)
        {
            _formData["Name"] = name;
            _formData["Date"] = DateTime.Parse(date);
            _formData["Price"] = price;
            _formData["Category"] = category;
            _formData["Quantity"] = quantity;
        }

        [When("the user submits the form with missing \"Name\" field")]
        public void WhenTheUserSubmitsTheFormWithMissingNameField()
        {
            _formData["Name"] = null;
            ValidateForm();
        }

        [When("the user submits the form with a negative \"Price\" \\((-?\\d+)\\)")]
        public void WhenTheUserSubmitsTheFormWithANegativePrice(decimal price)
        {
            _formData["Price"] = price;
            ValidateForm();
        }

        [When("the user submits the form with a negative \"Quantity\" \\((-?\\d+)\\)")]
        public void WhenTheUserSubmitsTheFormWithANegativeQuantity(int quantity)
        {
            _formData["Quantity"] = quantity;
            ValidateForm();
        }

        [When("the user submits the form with an empty \"Price\" field")]
        public void WhenTheUserSubmitsTheFormWithAnEmptyPriceField()
        {
            _formData["Price"] = null;
            ValidateForm();
        }

        [When("clicks the \"Add Product\" button")]
        public void WhenClicksTheAddProductButton()
        {
            ValidateForm();
        }

        [Then("the product \"(.*)\" with price (.*) should be added to the list of products")]
        public void ThenTheProductShouldBeAddedToTheListOfProducts(string name, decimal price)
        {
            _productList.Should().Contain(p => p.Name == name && p.Price == price);
        }

        [Then("the form should be cleared and a success message should appear")]
        public void ThenTheFormShouldBeClearedAndASuccessMessageShouldAppear()
        {
            _formData.Should().BeEmpty();
            _resultMessage.Should().Be("Product added successfully");
        }

        [Then("the form should not be submitted")]
        public void ThenTheFormShouldNotBeSubmitted()
        {
            _productList.Should().BeEmpty();
        }

        [Then("the user should see the message \"(.*)\"")]
        public void ThenTheUserShouldSeeTheMessage(string expectedMessage)
        {
            _resultMessage.Should().Be(expectedMessage);
        }

        [Then("the product list should be empty")]
        public void ThenTheProductListShouldBeEmpty()
        {
            Assert.IsEmpty(_productList, "The product list is not empty.");
        }

        private void ValidateForm()
        {
            if (_formData.TryGetValue("Name", out var name) && string.IsNullOrEmpty(name?.ToString()))
            {
                _resultMessage = "Name is required";
                return;
            }

            if (_formData.TryGetValue("Price", out var price) && (price == null || Convert.ToDecimal(price) <= 0))
            {
                _resultMessage = price == null ? "Price is required" : "Price must be greater than 0";
                return;
            }

            if (_formData.TryGetValue("Quantity", out var quantity) && Convert.ToInt32(quantity) < 0)
            {
                _resultMessage = "Quantity cannot be negative";
                return;
            }

            var product = new Product
            {
                Name = _formData["Name"].ToString(),
                Date = (DateTime)_formData["Date"],
                Price = (decimal)_formData["Price"],
                Category = _formData["Category"].ToString(),
                Quantity = (int)_formData["Quantity"]
            };

            _productList.Add(product);
            _formData.Clear();
            _resultMessage = "Product added successfully";
        }
    }
}
