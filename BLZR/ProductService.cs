using ProductApp.Models;

namespace ProductApp;

public class ProductService
{
    private readonly List<Product> _products = new();
    private int _nextId = 0; // Zmienna do generowania unikalnych ID

    public IEnumerable<Product> GetProducts() => _products;

    public void AddProduct(Product product)
    {
        product.Id = _nextId++; // Przypisanie unikalnego ID
        _products.Add(product);
    }

    public void EditProduct(Product updatedProduct)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == updatedProduct.Id);
        if (existingProduct != null)
        {
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Date = updatedProduct.Date;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Category = updatedProduct.Category;
            existingProduct.Quantity = updatedProduct.Quantity;
        }
    }

    public void DeleteProduct(int productId)
    {
        var productToRemove = _products.FirstOrDefault(p => p.Id == productId);
        if (productToRemove != null)
        {
            _products.Remove(productToRemove);
        }
    }
}
