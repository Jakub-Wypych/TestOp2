using System.Net.Http.Json;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Threading.Tasks;
using ProductApp.Models;
using ProductApp.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using BLZR;
using Microsoft.Extensions.DependencyInjection;
using ProductApi;

namespace Tests.StepDefinitions
{
    [Binding]
    public class EditProductStepDefinitions
    {
        private readonly WebApplicationFactory<ProductApi.TestProgram> _factory;
        private readonly HttpClient _client;
        private readonly IProductService _productService;
        private Product _existingProduct;
        private Product _updatedProduct;
        private ServiceResponse<bool> _response;

        public EditProductStepDefinitions(WebApplicationFactory<ProductApi.TestProgram> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _productService = new ProductService(_client);
        }

        [BeforeScenario]
        public async Task Setup()
        {
            var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            _existingProduct = new Product
            {
                Name = "Smartphone",
                Price = 500,
                Quantity = 10,
                Category = "Electronics",
                Date = DateTime.Now
            };
            dbContext.Products.Add(_existingProduct);
            await dbContext.SaveChangesAsync();
        }

        [Given(@"the product ""([^""]*)"" with ID (.*) exists in the database")]
        public async Task GivenTheProductWithIDExistsInTheDatabase(string productName, int productId)
        {
            var product = await _productService.GetProductAsync(productId);
            Assert.IsTrue(product.Success);
            Assert.AreEqual(productName, product.Data.Name);
        }

        [When(@"the user updates the product with name ""([^""]*)"" and price (.*)")]
        public async Task WhenTheUserUpdatesTheProductWithNameAndPrice(string productName, decimal price)
        {
            _updatedProduct = new Product
            {
                Id = _existingProduct.Id,
                Name = productName,
                Price = price,
                Quantity = _existingProduct.Quantity,
                Category = _existingProduct.Category,
                Date = _existingProduct.Date
            };

            _response = await _productService.EditProductAsync(_updatedProduct);
        }

        [Then(@"the service should successfully save the product ""([^""]*)"" with price (.*)")]
        public async Task ThenTheServiceShouldSuccessfullySaveTheProductWithPrice(string productName, decimal price)
        {
            Assert.IsTrue(_response.Success);

            var updatedProduct = await _productService.GetProductAsync(_updatedProduct.Id);
            Assert.IsTrue(updatedProduct.Success);
            Assert.AreEqual(productName, updatedProduct.Data.Name);
            Assert.AreEqual(price, updatedProduct.Data.Price);
        }

        [Given(@"the product with ID (.*) does not exist")]
        public async Task GivenTheProductWithIDDoesNotExistInTheDatabase(int productId)
        {
            var product = await _productService.GetProductAsync(productId);
            Assert.IsFalse(product.Success);
        }

        [When(@"the user attempts to edit the product with ID (.*)")]
        public async Task WhenTheUserAttemptsToEditTheProductWithID(int productId)
        {
            _updatedProduct = new Product
            {
                Id = productId,
                Name = "Invalid Product",
                Price = 100,
                Quantity = 5
            };
            _response = await _productService.EditProductAsync(_updatedProduct);
        }

        [Then(@"the service should return the message ""([^""]*)""")]
        public void ThenTheServiceShouldReturnTheMessage(string expectedMessage)
        {
            Assert.AreEqual(expectedMessage, _response.Message);
        }

        [Then(@"the product should not be updated")]
        public async Task ThenTheProductShouldNotBeUpdated()
        {
            var originalProduct = await _productService.GetProductAsync(_existingProduct.Id);
            Assert.AreEqual(_existingProduct.Name, originalProduct.Data.Name);
            Assert.AreEqual(_existingProduct.Price, originalProduct.Data.Price);
            Assert.AreEqual(_existingProduct.Quantity, originalProduct.Data.Quantity);
        }

        [When(@"the user submits the product update with missing Name field")]
        public async Task WhenTheUserSubmitsTheProductUpdateWithMissingNameField()
        {
            _updatedProduct = new Product
            {
                Id = _existingProduct.Id,
                Name = string.Empty,
                Price = _existingProduct.Price,
                Quantity = _existingProduct.Quantity
            };

            _response = await _productService.EditProductAsync(_updatedProduct);
        }

        [When(@"the user submits the product update with negative Price \((.*)\)")]
        public async Task WhenTheUserSubmitsTheProductUpdateWithNegativePrice(decimal price)
        {
            _updatedProduct = new Product
            {
                Id = _existingProduct.Id,
                Name = _existingProduct.Name,
                Price = price,
                Quantity = _existingProduct.Quantity
            };

            _response = await _productService.EditProductAsync(_updatedProduct);
        }

        [When(@"the user submits the product update with negative Quantity \((.*)\)")]
        public async Task WhenTheUserSubmitsTheProductUpdateWithNegativeQuantity(int quantity)
        {
            _updatedProduct = new Product
            {
                Id = _existingProduct.Id,
                Name = _existingProduct.Name,
                Price = _existingProduct.Price,
                Quantity = quantity
            };

            _response = await _productService.EditProductAsync(_updatedProduct);
        }
    }
}
