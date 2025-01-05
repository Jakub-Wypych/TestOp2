//using BLZR;
//using Moq;
//using NUnit.Framework;
//using ProductApp.Models;
//using System;
//using TechTalk.SpecFlow;

//namespace Tests.StepDefinitions
//{
//    [Binding]
//    public class EditProductStepDefinitions
//    {
//        private readonly Mock<IProductService> _mockProductService;
//        private List<Product> _products;
//        private string _resultMessage;

//        public EditProductStepDefinitions()
//        {
//            _mockProductService = new Mock<IProductService>();
//            _products = new List<Product>
//            {
//                new Product { Id = 1, Name = "Smartphone", Price = 599.99m, Quantity = 10 },
//                new Product { Id = 2, Name = "Tablet", Price = 399.99m, Quantity = 15 }
//            };

//            _mockProductService.Setup(service => service.GetProducts()).Returns(Task.FromResult(_products.AsEnumerable()));
//        }


//        [Given("the user is on the \"Edit Product\" page")]
//        public void GivenTheUserIsOnTheEditProductPage()
//        {
//            _resultMessage = string.Empty;
//        }

//        [When("the user changes the product name to \"(.*)\" and the price to (.*)")]
//        public async Task WhenTheUserChangesTheProductNameToAndThePriceTo(string newName, decimal newPrice)
//        {
//            var productToEdit = _products.FirstOrDefault(p => p.Name == "Smartphone");
//            Assert.NotNull(productToEdit, "Product to edit not found.");

//            productToEdit.Name = newName;
//            productToEdit.Price = newPrice;

//            await _mockProductService.Object.EditProduct(productToEdit);
//        }

//        [When("clicks the \"Save\" button")]
//        public void WhenClicksTheSaveButton()
//        {
//            _resultMessage = "Product saved successfully";
//        }

//        [Then("the product \"(.*)\" with price (.*) should be saved")]
//        public void ThenTheProductWithPriceShouldBeSaved(string productName, decimal price)
//        {
//            var product = _products.FirstOrDefault(p => p.Name == productName && p.Price == price);
//            Assert.NotNull(product, "Product was not saved correctly.");
//        }


//        [Given("the user wants to edit a product with ID (.*) that does not exist in the database")]
//        public void GivenTheUserWantsToEditAProductWithIDThatDoesNotExist(int productId)
//        {
//            Assert.IsFalse(_products.Any(p => p.Id == productId), "Product exists in the database.");
//        }


//        [When("the user submits the edit form")]
//        public void WhenTheUserSubmitsTheEditForm()
//        {
//            _resultMessage = "Product does not exist";
//        }
//        [Then("the user should see the error message \"(.*)\"")]

//        public void ThenTheUserShouldSeeTheErrorMessage(string errorMessage)
//        {
//            Assert.AreEqual(errorMessage, _resultMessage);
//        }

//        [Then("the product should not be updated")]
//        public void ThenTheProductShouldNotBeUpdated()
//        {
//            _mockProductService.Verify(service => service.EditProduct(It.IsAny<Product>()), Times.Never);
//        }
//    }
//}
