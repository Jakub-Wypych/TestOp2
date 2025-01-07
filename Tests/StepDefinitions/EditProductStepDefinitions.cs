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
    public class EditProductStepDefinitions
    {
        private readonly Mock<IProductService> _mockProductService;
        private Product _existingProduct;
        private Product _updatedProduct;
        private ServiceResponse<bool> _response;

        public EditProductStepDefinitions()
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

            _mockProductService.Setup(service => service.EditProductAsync(It.IsAny<Product>()))
           .ReturnsAsync((Product product) =>
           {
               if (product.Id == -1) 
                   return new ServiceResponse<bool> { Success = false, Message = "Product does not exist" };
               if (string.IsNullOrWhiteSpace(product.Name))
                   return new ServiceResponse<bool> { Success = false, Message = "Name is required" };
               if (product.Price <= 0)
                   return new ServiceResponse<bool> { Success = false, Message = "Price must be greater than 0" };
               if (product.Quantity < 0)
                   return new ServiceResponse<bool> { Success = false, Message = "Quantity cannot be negative" };
               return new ServiceResponse<bool> { Success = true, Message = $"Product {product.Name} with price {product.Price} saved successfully" };
           });

        }

        [BeforeScenario]
        public void Setup() 
        {            
            _updatedProduct = new Product
            {
                Id = 1,
                Name = "Smartphone",
                Price = 1000,
                Quantity = 10
            };        
        }

        [Given(@"the product ""([^""]*)"" with ID (.*) exists in the database")]
        public void GivenTheProductWithIDExistsInTheDatabase(string productName, int productId)
        {
            _mockProductService.Setup(service => service.GetProductAsync(productId))
                .ReturnsAsync(new ServiceResponse<Product>
                {
                    Success = true,
                    Data = _existingProduct
                });
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

            _response = await _mockProductService.Object.EditProductAsync(_updatedProduct);
        }

        [Then(@"the service should successfully save the product ""([^""]*)"" with price (.*)")]
        public void ThenTheServiceShouldSuccessfullySaveTheProductWithPrice(string productName, decimal price)
        {
            Assert.IsTrue(_response.Success);
            Assert.AreEqual($"Product {productName} with price {price} saved successfully", _response.Message);
        }

        [Given(@"the product with ID (.*) does not exist")]
        public void GivenTheProductWithIDDoesNotExistInTheDatabase(int productId)
        {
            _mockProductService.Setup(service => service.GetProductAsync(productId))
                .ReturnsAsync(new ServiceResponse<Product>
                {
                    Success = false,
                    Message = "Product does not exist"
                });
        }

        [When(@"the user attempts to edit the product with ID (.*)")]
        public async Task WhenTheUserAttemptsToEditTheProductWithID(int productId)
        {
            _updatedProduct.Id = productId;
            _response = await _mockProductService.Object.EditProductAsync(_updatedProduct);
        }

        [Then(@"the service should return the message ""([^""]*)""")]
        public void ThenTheServiceShouldReturnTheMessage(string expectedMessage)
        {
            Assert.AreEqual(expectedMessage, _response.Message);
        }

        [Then(@"the product should not be updated")]
        public void ThenTheProductShouldNotBeUpdated()
        {
            Assert.IsFalse(_response.Success);
        }

        [When(@"the user submits the product update with missing Name field")]
        public async Task WhenTheUserSubmitsTheProductUpdateWithMissingNameField()
        {
            _updatedProduct.Name = string.Empty; 
            _response = await _mockProductService.Object.EditProductAsync(_updatedProduct);
        }

        [When(@"the user submits the product update with negative Price \((.*)\)")]
        public async Task WhenTheUserSubmitsTheProductUpdateWithNegativePrice(decimal price)
        {
            _updatedProduct.Price = price;
            _response = await _mockProductService.Object.EditProductAsync(_updatedProduct);
        }

        [When(@"the user submits the product update with negative Quantity \((.*)\)")]
        public async Task WhenTheUserSubmitsTheProductUpdateWithNegativeQuantity(int quantity)
        {
            _updatedProduct.Quantity = quantity;
            _response = await _mockProductService.Object.EditProductAsync(_updatedProduct);
        }
    }
}
