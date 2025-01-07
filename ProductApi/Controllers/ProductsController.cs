using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(product, serviceProvider: null, items: null);
            bool isValid = Validator.TryValidateObject(product, context, validationResults, true);

            if (!isValid)
            {
                return BadRequest(validationResults);
            }

            // Jeśli dane są poprawne, dodaj produkt do bazy
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(); // Return 404 if the product doesn't exist
            }

            // Update the product details
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Date = product.Date;
            existingProduct.Category = product.Category;
            existingProduct.Quantity = product.Quantity;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception
                return StatusCode(StatusCodes.Status500InternalServerError, "A concurrency issue occurred while updating the product.");
            }

            return NoContent(); // Return 204 No Content on successful update
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
