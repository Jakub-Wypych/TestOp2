using System;
using TechTalk.SpecFlow;
using Moq;
using ProductApp.Models;
using ProductApp.Services;
using NUnit.Framework;
using System.Threading.Tasks;
using BLZR;

namespace Tests.StepDefinitions
{
    [Binding]
    public class AddProductStepDefinitions
    {
        private readonly Mock<IProductService> _mockProductService;
        private ServiceResponse<bool> _response;
        private Product _newProduct;
        private Product _existingProduct;

        public AddProductStepDefinitions()
        {
            _mockProductService = new Mock<IProductService>();

            _existingProduct = new Product
            {
                Id = 1,
                Name = "Smartphone",
                Price = 500,
                Quantity = 10,
                Category = "Electronics",
                Date = DateTime.Now
            };

            _mockProductService.Setup(service => service.AddProductAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product product) =>
                {
                    if (string.IsNullOrWhiteSpace(product.Name))
                        return new ServiceResponse<bool> { Success = false, Message = "Name is required" };

                    if (product.Price <= 0)
                        return new ServiceResponse<bool> { Success = false, Message = "Price must be greater than 0" };

                    if (product.Quantity < 0)
                        return new ServiceResponse<bool> { Success = false, Message = "Quantity cannot be negative" };

                    return new ServiceResponse<bool> { Success = true, Message = $"Product {product.Name} with price {product.Price} added successfully" };
                });
        }

        [BeforeScenario]
        public void SetupNewProduct()
        {
            _newProduct = new Product();
            _newProduct.Name = "Product";
            _newProduct.Date = DateTime.Now;
            _newProduct.Price = 1;
            _newProduct.Quantity = 1;
        }


        [When(@"clicks the Add Product button")]
        public async Task WhenClicksTheButton()
        {
            _response = await _mockProductService.Object.AddProductAsync(_newProduct);
        }

        [Then(@"the product ""([^""]*)"" with price (.*) should be added to the list of products")]
        public void ThenTheProductWithPriceShouldBeAddedToTheListOfProducts(string productName, decimal price)
        {
            Assert.IsTrue(_response.Success);
            Assert.AreEqual($"Product {productName} with price {price} added successfully", _response.Message);
        }


        [When(@"user submits the form with the Name field missing")]
        public void WhenUserSubmitsTheFormWithTheFieldMissing()
        {
            _newProduct.Name = string.Empty;
        }

        [Then(@"the product should not be added")]
        public void ThenTheProductShouldNotBeAdded()
        {
            Assert.IsFalse(_response.Success);
        }

        [Then(@"the user should see the message ""([^""]*)""")]
        public void ThenTheUserShouldSeeTheMessage(string expectedMessage)
        {
            Assert.AreEqual(expectedMessage, _response.Message);
        }

        [When(@"user submits the form with a negative ""([^""]*)"" \((.*)\)")]
        public void WhenUserSubmitsTheFormWithANegative(string field, int value)
        {
            if (field == "Price")
            {
                _newProduct.Price = value;
            }
            else if (field == "Quantity")
            {
                _newProduct.Quantity = value;
            }
        }
    }
}
