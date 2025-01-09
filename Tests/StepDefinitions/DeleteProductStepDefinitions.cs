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
    public class DeleteProductStepDefinitions
    {
        private readonly Mock<IProductService> _mockProductService;
        private ServiceResponse<bool> _response;
        private Product _existingProduct;

        public DeleteProductStepDefinitions()
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

            _mockProductService.Setup(service => service.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => id == _existingProduct.Id
                    ? new ServiceResponse<Product> { Success = true, Data = _existingProduct }
                    : new ServiceResponse<Product> { Success = false, Message = "Product does not exist" });

            _mockProductService.Setup(service => service.DeleteProductAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    if (id == _existingProduct.Id)
                        return new ServiceResponse<bool> { Success = true, Message = $"Product {id} deleted successfully" };
                    else
                        return new ServiceResponse<bool> { Success = false, Message = "Product does not exist" };
                });
        }

        [Given(@"the product ""([^""]*)"" exists in the list of products")]
        public void GivenTheProductExistsInTheListOfProducts(string productName)
        {
            _mockProductService.Setup(service => service.GetProductsAsync())
                .ReturnsAsync(new ServiceResponse<IEnumerable<Product>>
                {
                    Success = true,
                    Data = new[] { _existingProduct }
                });
        }

        [When(@"the user clicks the Delete button next to the product ""([^""]*)""")]
        public async Task WhenTheUserClicksTheDeleteButtonNextToTheProduct(string productName)
        {
            var productId = _existingProduct.Id;
            _response = await _mockProductService.Object.DeleteProductAsync(productId);
        }

        [Then(@"the service should successfully remove the product ""([^""]*)"" from the list")]
        public void ThenTheServiceShouldSuccessfullyRemoveTheProductFromTheList(string productName)
        {
            Assert.IsTrue(_response.Success);
            Assert.AreEqual($"Product {_existingProduct.Id} deleted successfully", _response.Message);
        }

        [Then(@"a success message should appear")]
        public void ThenASuccessMessageShouldAppear()
        {
            Assert.IsTrue(_response.Success);
            Assert.AreEqual("Product 1 deleted successfully", _response.Message);
        }

        [Given(@"the products ""([^""]*)"", ""([^""]*)"", and ""([^""]*)"" exist in the list of products")]
        public void GivenTheProductsExistInTheListOfProducts(string product1, string product2, string product3)
        {
            _mockProductService.Setup(service => service.GetProductsAsync())
                .ReturnsAsync(new ServiceResponse<IEnumerable<Product>>
                {
                    Success = true,
                    Data = new[]
                    {
                        _existingProduct,
                        new Product { Id = 2, Name = product2, Price = 100, Quantity = 5, Category = "Electronics", Date = DateTime.Now },
                        new Product { Id = 3, Name = product3, Price = 300, Quantity = 8, Category = "Electronics", Date = DateTime.Now }
                    }
                });
        }

        [When(@"the user attempts to delete the product with ID (.*)")]
        public async Task WhenTheUserAttemptsToDeleteTheProductWithID(int productId)
        {
            _response = await _mockProductService.Object.DeleteProductAsync(productId);
        }

        [Then(@"no product should be removed from the list")]
        public void ThenNoProductShouldBeRemovedFromTheList()
        {
            Assert.IsFalse(_response.Success);
            Assert.AreEqual("Product does not exist", _response.Message);
        }


        [Then(@"service should return the message ""([^""]*)""")]
        public void ThenTheServiceShouldReturnTheMessage(string expectedMessage)
        {
            Assert.AreEqual(expectedMessage, _response.Message);
        }
    }
}
