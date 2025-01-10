using System.Net.Http.Json;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Threading.Tasks;
using ProductApp.Models;
using ProductApp.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Linq;
using BLZR;
using Microsoft.Extensions.DependencyInjection;
using ProductApi;

namespace Tests.StepDefinitions
{
    [Binding]
    public class DeleteProductStepDefinitions
    {
        private readonly WebApplicationFactory<ProductApi.TestProgram> _factory;
        private readonly HttpClient _client;
        private readonly IProductService _productService;
        private ServiceResponse<bool> _response;
        private Product _existingProduct;

        public DeleteProductStepDefinitions(WebApplicationFactory<ProductApi.TestProgram> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();

            _productService = new ProductService(_client);

            var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            _existingProduct = new Product
            {
                Id = 1,
                Name = "Smartphone",
                Price = 500,
                Quantity = 10,
                Category = "Electronics",
                Date = DateTime.Now
            };
            dbContext.Products.Add(_existingProduct);
            dbContext.SaveChanges();
        }

        [Given(@"the product ""([^""]*)"" exists in the list of products")]
        public async Task GivenTheProductExistsInTheListOfProducts(string productName)
        {
            var productsResponse = await _productService.GetProductsAsync();
            Assert.IsTrue(productsResponse.Success);
            Assert.IsTrue(productsResponse.Data.Any(p => p.Name == productName));
        }

        [When(@"the user clicks the Delete button next to the product ""([^""]*)""")]
        public async Task WhenTheUserClicksTheDeleteButtonNextToTheProduct(string productName)
        {
            var productsResponse = await _productService.GetProductsAsync();
            var product = productsResponse.Data.FirstOrDefault(p => p.Name == productName);

            Assert.IsNotNull(product, $"Product '{productName}' not found in the list.");
            _response = await _productService.DeleteProductAsync(product.Id);
        }

        [Then(@"the service should successfully remove the product ""([^""]*)"" from the list")]
        public async Task ThenTheServiceShouldSuccessfullyRemoveTheProductFromTheList(string productName)
        {
            Assert.IsTrue(_response.Success);
            var productsResponse = await _productService.GetProductsAsync();
            Assert.IsTrue(productsResponse.Success);
            Assert.IsFalse(productsResponse.Data.Any(p => p.Name == productName));
        }

        [Then(@"a success message should appear")]
        public void ThenASuccessMessageShouldAppear()
        {
            Assert.IsTrue(_response.Success);
            Assert.AreEqual($"Product successfully deleted.", _response.Message);
        }

        [Given(@"the products ""([^""]*)"", ""([^""]*)"", and ""([^""]*)"" exist in the list of products")]
        public async Task GivenTheProductsExistInTheListOfProducts(string product1, string product2, string product3)
        {
            var dbContext = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Products.AddRange(new[]
            {
                new Product { Name = product1, Price = 100, Quantity = 5, Category = "Electronics", Date = DateTime.Now },
                new Product { Name = product2, Price = 200, Quantity = 8, Category = "Electronics", Date = DateTime.Now },
                new Product { Name = product3, Price = 300, Quantity = 10, Category = "Electronics", Date = DateTime.Now }
            });
            dbContext.SaveChanges();

            var productsResponse = await _productService.GetProductsAsync();
            Assert.IsTrue(productsResponse.Success);
            Assert.IsTrue(productsResponse.Data.Any(p => p.Name == product1));
            Assert.IsTrue(productsResponse.Data.Any(p => p.Name == product2));
            Assert.IsTrue(productsResponse.Data.Any(p => p.Name == product3));
        }

        [When(@"the user attempts to delete the product with ID (.*)")]
        public async Task WhenTheUserAttemptsToDeleteTheProductWithID(int productId)
        {
            _response = await _productService.DeleteProductAsync(productId);
        }

        [Then(@"no product should be removed from the list")]
        public async Task ThenNoProductShouldBeRemovedFromTheList()
        {
            Assert.IsFalse(_response.Success);

            var productsResponse = await _productService.GetProductsAsync();
            Assert.IsTrue(productsResponse.Success);
            Assert.AreEqual(1, productsResponse.Data.Count());
        }

        [Then(@"service should return the message ""([^""]*)""")]
        public void ThenTheServiceShouldReturnTheMessage(string expectedMessage)
        {
            Assert.AreEqual(expectedMessage, _response.Message);
        }
    }
}
