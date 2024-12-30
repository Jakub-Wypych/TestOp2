using ProductApp.Models;

namespace ProductApp;

public class ProductService
{
    private readonly List<Product> _products = new();

    public IEnumerable<Product> GetProducts() => _products;

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }
}