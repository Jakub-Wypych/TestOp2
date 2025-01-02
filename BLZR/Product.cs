namespace ProductApp.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public int Quantity { get; set; }
}