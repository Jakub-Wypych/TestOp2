//using Moq;
//using NUnit.Framework;
//using BLZR;
//using ProductApp.Models;


//namespace Tests.StepDefinitions
//{
//    [Binding]
//    public class DeleteProductStepDefinitions
//    {
//        private readonly Mock<IProductService> _mockProductService;
//        private List<Product> _products;
//        private string _resultMessage;

//        public DeleteProductStepDefinitions()
//        {
//            _mockProductService = new Mock<IProductService>();
//            _products = new List<Product>();
//            _resultMessage = string.Empty;
//        }

//        [Given(@"the user sees a list of products, including the product ""(.*)""")]
//        public void GivenTheUserSeesAListOfProductsIncludingTheProduct(string productName)
//        {
//            _products = new List<Product>
//            {
//                new Product { Id = 1, Name = "Smartphone", Price = 599.99m, Category = "Electronics", Quantity = 10 },
//                new Product { Id = 2, Name = "Tablet", Price = 399.99m, Category = "Electronics", Quantity = 15 },
//                new Product { Id = 3, Name = "Laptop", Price = 899.99m, Category = "Electronics", Quantity = 5 }
//            };

//            _mockProductService.Setup(service => service.GetProducts()).Returns(Task.FromResult(_products.AsEnumerable()));

//            // Ensure the product we're targeting exists
//            var product = _products.FirstOrDefault(p => p.Name == productName);
//            Assert.NotNull(product, $"Product '{productName}' not found in the list.");
//        }

//        [Given(@"the user sees a list of products, including the products ""(.*)"", ""(.*)"", and ""(.*)""")]
//        public void GivenTheUserSeesAListOfProductsIncludingTheProducts(string product1, string product2, string product3)
//        {
//            _products = new List<Product>
//    {
//        new Product { Id = 1, Name = product1, Price = 599.99m, Category = "Electronics", Quantity = 10 },
//        new Product { Id = 2, Name = product2, Price = 399.99m, Category = "Electronics", Quantity = 15 },
//        new Product { Id = 3, Name = product3, Price = 899.99m, Category = "Electronics", Quantity = 5 }
//    };

//            _mockProductService.Setup(service => service.GetProducts()).Returns(Task.FromResult(_products.AsEnumerable()));

//            // Ensure all specified products exist
//            foreach (var productName in new[] { product1, product2, product3 })
//            {
//                var product = _products.FirstOrDefault(p => p.Name == productName);
//                Assert.NotNull(product, $"Product '{productName}' not found in the list.");
//            }
//        }


//        [When(@"the user clicks the ""Delete"" button next to ""(.*)""")]
//        public async Task WhenTheUserClicksTheDeleteButtonNextTo(string productName)
//        {
//            var productToDelete = _products.FirstOrDefault(p => p.Name == productName);
//            Assert.NotNull(productToDelete, $"Product '{productName}' not found for deletion.");

//            // Simulate deletion
//            await _mockProductService.Object.DeleteProduct(productToDelete.Id);
//            _products.Remove(productToDelete);

//            // Success message
//            _resultMessage = $"Product '{productName}' deleted successfully";
//        }

//        [Then(@"the product ""(.*)"" should be removed from the list of products")]
//        public void ThenTheProductShouldBeRemovedFromTheListOfProducts(string productName)
//        {
//            var product = _products.FirstOrDefault(p => p.Name == productName);
//            Assert.Null(product, $"Product '{productName}' is still in the list.");
//        }

//        [Then(@"a success message should appear")]
//        public void ThenASuccessMessageShouldAppear()
//        {
//          //  Assert.AreEqual(_resultMessage, $"Product '{Name}' deleted successfully");
//        }
//    }
//}
