using NUnit.Framework;
using ProductApp.Models;
using ProductApp.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using BLZR;
using Microsoft.Extensions.DependencyInjection;
using ProductApi;

namespace Tests.StepDefinitions
{
    [Binding]
    public class AddProductStepDefinitions
    {
        private readonly HttpClient _client;
        private readonly IProductService _productService;
        private ServiceResponse<bool> _response;
        private Product _newProduct;

        public AddProductStepDefinitions(WebApplicationFactory<ProductApi.TestProgram> factory)
        {
            _client = factory.CreateClient();

            _productService = new ProductService(_client);

            var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        [BeforeScenario]
        public void SetupNewProduct()
        {
            _newProduct = new Product
            {
                Name = "Product",
                Price = 1,
                Quantity = 1,
                Category = "TestCategory",
                Date = DateTime.Now
            };
        }

        [When(@"clicks the Add Product button")]
        public async Task WhenClicksTheButton()
        {
            _response = await _productService.AddProductAsync(_newProduct);
        }

        [Then(@"the product ""([^""]*)"" with price (.*) should be added to the list of products")]
        public async Task ThenTheProductWithPriceShouldBeAddedToTheListOfProducts(string productName, decimal price)
        {
            Assert.IsTrue(_response.Success);
            Assert.AreEqual($"Product '{productName}' added successfully", _response.Message);

            var productsResponse = await _productService.GetProductsAsync();
            Assert.IsTrue(productsResponse.Success);
            Assert.IsTrue(productsResponse.Data.Any(p => p.Name == productName && p.Price == price));
        }

        [When(@"user submits the form with the Name field missing")]
        public void WhenUserSubmitsTheFormWithTheFieldMissing()
        {
            _newProduct.Name = string.Empty;
        }

        [Then(@"the product should not be added")]
        public async Task ThenTheProductShouldNotBeAdded()
        {
            Assert.IsFalse(_response.Success);

            var productsResponse = await _productService.GetProductsAsync();
            Assert.IsFalse(productsResponse.Data.Any(p => string.IsNullOrWhiteSpace(p.Name)));
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
