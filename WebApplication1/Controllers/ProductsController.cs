using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Entities;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _dbContext.Set<Product>().ToList();

            return Ok(products);
        }

        [HttpGet]
        [Route("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
                return NotFound($"product with id = {id} is not found");

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest productRequest)
        {
            if (productRequest is not null)
            {
                if (string.IsNullOrWhiteSpace(productRequest.Name))
                    return BadRequest("product name must be passed");

                var product = new Product
                {
                    Name = productRequest.Name,
                    Price = productRequest.Price,
                    CategoryId = productRequest.CategoryId
                };
                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                return Created();
            }


            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductRequest request)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

            if (product is null)
                return NotFound();

            product.Name = request.Name;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;

            _dbContext.Products.Update(product);

            await _dbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
                return NotFound();

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return Ok(product);
        }

    }
}
