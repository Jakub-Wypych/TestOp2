using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ProductApp.Models;
using System.Net.Http.Json;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using ProductApi;

[Binding]
public class APIOperationsForProductsStepDefinitions
{
    private readonly HttpClient _client;
    private HttpResponseMessage _response;
    private List<Product> _products;

    public APIOperationsForProductsStepDefinitions(WebApplicationFactory<ProductApi.Program> factory)
    {
        _client = factory.CreateClient();
        _products = new List<Product>();

        // Using an in-memory database for testing
        var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Switch to in-memory database for testing
        dbContext.Database.EnsureDeleted(); // Ensure the in-memory database is fresh
        dbContext.Database.EnsureCreated(); // Create a fresh in-memory database
    }

    // Scenario 1: Retrieving all products from the API
    [Given(@"there are products with IDs (.*), (.*), (.*) in the database")]
    public async Task GivenThereAreProductsWithIDsInTheDatabase(int id1, int id2, int id3)
    {
        _products = new List<Product>
        {
            new Product { Id = id1, Name = "Product1", Price = 100, Quantity = 10 },
            new Product { Id = id2, Name = "Product2", Price = 200, Quantity = 20 },
            new Product { Id = id3, Name = "Product3", Price = 300, Quantity = 30 }
        };

        foreach (var product in _products)
        {
            var response = await _client.PostAsJsonAsync("api/products", product);
            response.EnsureSuccessStatusCode();
        }
    }

    [When(@"a GET request is made to the endpoint `api/products`")]
    public async Task WhenAGETRequestIsMadeToTheEndpointApiProducts()
    {
        _response = await _client.GetAsync("api/products");
    }

    [Then(@"the response should contain all the products")]
    public async Task ThenTheResponseShouldContainAllTheProducts()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await _response.Content.ReadAsStringAsync();
        var returnedProducts = JsonConvert.DeserializeObject<List<Product>>(responseContent);

        returnedProducts.Should().HaveCount(_products.Count);
        foreach (var product in _products)
        {
            returnedProducts.Should().ContainEquivalentOf(product);
        }
    }

    [Then(@"the products should have correct data like name, price, and quantity")]
    public async Task ThenTheProductsShouldHaveCorrectDataLikeNamePriceAndQuantity()
    {
        var responseContent = await _response.Content.ReadAsStringAsync();
        var returnedProducts = JsonConvert.DeserializeObject<List<Product>>(responseContent);

        foreach (var product in _products)
        {
            var matchedProduct = returnedProducts.FirstOrDefault(p => p.Id == product.Id);
            matchedProduct.Should().NotBeNull();
            matchedProduct.Should().BeEquivalentTo(product);
        }
    }

    // Scenario 2: Adding a product via the API
    [Given(@"the product ""([^""]*)"" does not exist in the database")]
    public async Task GivenTheProductDoesNotExistInTheDatabase(string name)
    {
        var response = await _client.GetAsync("api/products");
        var products = JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());

        products.Should().NotContain(p => p.Name == name);
    }

    [When(@"a POST request is made to the endpoint `api/products` with data ""([^""]*)"": ""([^""]*)"", ""([^""]*)"": (.*)")]
    public async Task WhenAPOSTRequestIsMadeToTheEndpointApiProductsWithData(string key1, string name, string key2, decimal price)
    {
        var newProduct = new Product { Name = name, Price = price, Quantity = 5 }; // Assuming Quantity is constant for this test
        _response = await _client.PostAsJsonAsync("api/products", newProduct);
    }

    [Then(@"the response should have a status code (.*) Created")]
    public void ThenTheResponseShouldHaveAStatusCodeCreated(int statusCode)
    {
        _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
    }

    [Then(@"the product ""([^""]*)"" should be added to the database")]
    public async Task ThenTheProductShouldBeAddedToTheDatabase(string name)
    {
        var response = await _client.GetAsync("api/products");
        var products = JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());

        products.Should().Contain(p => p.Name == name);
    }

    // Scenario 3: Editing a product via the API
    [Given(@"the product with ID (.*) exists in the database with the name ""([^""]*)""")]
    public async Task GivenTheProductWithIDExistsInTheDatabaseWithTheName(int id, string name)
    {
        var product = new Product { Id = id, Name = name, Price = 500, Quantity = 5 };
        _response = await _client.PostAsJsonAsync("api/products", product);
        _response.EnsureSuccessStatusCode();
    }

    [When(@"a PUT request is made to the endpoint `api/products/(.*)` with new data \(""([^""]*)"": ""([^""]*)"", ""([^""]*)"": (.*)\)")]
    public async Task WhenAPUTRequestIsMadeToTheEndpointApiProductsWithNewData(int id, string key1, string newName, string key2, decimal newPrice)
    {
        var updatedProduct = new Product { Id = id, Name = newName, Price = newPrice, Quantity = 5 };
        _response = await _client.PutAsJsonAsync($"api/products/{id}", updatedProduct);
    }

    [Then(@"the response to the DELETE request should have a status code 204 No Content")]
    public void ThenTheResponseToDeleteRequestShouldHaveAStatusCode204NoContent()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.NoContent);

    }

    [Then(@"the response to the PUT request should have a status code 204 No Content")]
    public void ThenTheResponseToPutRequestShouldHaveAStatusCode204NoContent()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }



    [Then(@"the product ""([^""]*)"" should be updated to ""([^""]*)"" with a new price of (.*)")]
    public async Task ThenTheProductShouldBeUpdatedToWithANewPriceOf(string oldName, string newName, decimal newPrice)
    {
        var response = await _client.GetAsync("api/products");
        var products = JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());

        var updatedProduct = products.FirstOrDefault(p => p.Name == newName && p.Price == newPrice);
        updatedProduct.Should().NotBeNull();
    }

    // Scenario 4: Deleting a product via the API
    [Given(@"the product with ID (.*) exists in the database")]
    public async Task GivenTheProductWithIDExistsInTheDatabase(int id)
    {
        var product = new Product { Id = id, Name = "TempProduct", Price = 100, Quantity = 1 };
        _response = await _client.PostAsJsonAsync("api/products", product);
        _response.EnsureSuccessStatusCode();
    }

    [When(@"a DELETE request is made to the endpoint `api/products/(.*)`")]
    public async Task WhenADELETERequestIsMadeToTheEndpointApiProducts(int id)
    {
        _response = await _client.DeleteAsync($"api/products/{id}");
    }

    [Then(@"the response should have a status code (.*) No Content")]
    public void ThenTheResponseShouldHaveAStatusCodeNoContentDelete(int statusCode)
    {
        _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
    }

    [Then(@"the product with ID (.*) should be deleted from the database")]
    public async Task ThenTheProductWithIDShouldBeDeletedFromTheDatabase(int id)
    {
        var response = await _client.GetAsync("api/products");
        var products = JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());

        products.Should().NotContain(p => p.Id == id);
    }

    // Scenario 5: Handling errors when deleting a non-existing product
    [Given(@"the product with ID (.*) does not exist in the database")]
    public void GivenTheProductWithIDDoesNotExistInTheDatabase(int id)
    {
        // No action needed; we assume a clean database or explicit deletion of the product
    }

    [Then(@"the response should have a status code (.*) Not Found")]
    public void ThenTheResponseShouldHaveAStatusCodeNotFound(int statusCode)
    {
        _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
    }

    [Then(@"the product with ID (.*) should not be deleted")]
    public async Task ThenTheProductWithIDShouldNotBeDeleted(int id)
    {
        var response = await _client.GetAsync("api/products");
        var products = JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());

        products.Should().NotContain(p => p.Id == id);
    }

    [Then(@"the response should have a status code 400 Bad Request")]
    public void ThenTheResponseShouldHaveAStatusCode400BadRequest()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Then(@"the product should not be added to the database")]
    public async Task ThenTheProductShouldNotBeAddedToTheDatabase()
    {
        var response = await _client.GetAsync("api/products");
        var products = JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());
        products.Should().NotContain(p => p.Name == "Monitor");
    }

    [Then(@"the product should not be modified")]
    public async Task ThenTheProductShouldNotBeModified()
    {
        var response = await _client.GetAsync("api/products");
        var products = JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());
        products.Should().NotContain(p => p.Name == "Non-existent");
    }

    // Scenario: Trying to retrieve a non-existing product
    [When(@"a GET request is made to the endpoint `api/products/(\d+)`")]
    public async Task WhenAGETRequestIsMadeToTheEndpointApiProductsWithNonExistingID(int id)
    {
        _response = await _client.GetAsync($"api/products/{id}");
    }

    [Then(@"the response body should contain an error message")]
    public async Task ThenTheResponseBodyShouldContainAnErrorMessage()
    {
        var responseContent = await _response.Content.ReadAsStringAsync();
        responseContent.IndexOf("not found", StringComparison.OrdinalIgnoreCase).Should().BeGreaterThan(-1);
    }

    // Scenario: Retrieving products when the database is empty
    [Given(@"the database is empty")]
    public void GivenTheDatabaseIsEmpty()
    {
        // Assumes database is already empty or will be reset for testing
    }

    [Then(@"the response should contain an empty list")]
    public async Task ThenTheResponseShouldContainAnEmptyList()
    {
        var responseContent = await _response.Content.ReadAsStringAsync();
        var products = JsonConvert.DeserializeObject<List<Product>>(responseContent);
        products.Should().BeEmpty();
    }

    // Scenario: Trying to delete a product that was already deleted
    [When(@"a DELETE request is made to the endpoint `api/products/(\d+)` again")]
    public async Task WhenADELETERequestIsMadeToTheEndpointApiProductsAgain(int id)
    {
        _response = await _client.DeleteAsync($"api/products/{id}");
    }

    [When(@"a PUT request is made to the endpoint `api/products/(\d+)` with invalid data \(Name: ""([^""]+)"", Price: ""([^""]+)"", Quantity: (\d+)\)")]
    public async Task WhenAPUTRequestIsMadeToTheEndpointApiProductsWithInvalidData(int id, string name, string invalidPrice, int quantity)
    {
        // Attempt to update product with invalid price (string instead of decimal)
        var invalidProductData = new { Name = name, Price = invalidPrice, Quantity = quantity };
        _response = await _client.PutAsJsonAsync($"api/products/{id}", invalidProductData);
    }

}
