using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Dtos.ProductDtos;
using WebApplication1.Entities;
using WebApplication1.Results;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        //private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;
        public ProductsController(IProductService service, ILogger<ProductsController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        #region Previous Sesssion
        //public ProductsController(ApplicationDbContext dbContext, ILogger<ProductsController> logger, IMapper mapper)
        //{
        //    _dbContext = dbContext;
        //    _logger = logger;
        //    _mapper = mapper;
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllProducts(int pageNumber, int pageSize, string? searchTerm)
        //{
        //    List <Product> products;
        //    if (searchTerm is not null)
        //    {

        //         products = _dbContext.Set<Product>()
        //            .Where(p => p.Name.Contains(searchTerm))
        //            .Include(p => p.Category)
        //            .Skip((pageNumber - 1) * pageSize)
        //            .Take(pageSize)
        //            .ToList();
        //    }
        //    else
        //    {
        //        products = _dbContext.Set<Product>()
        //            .Include(p => p.Category)
        //            .Skip((pageNumber - 1) * pageSize)
        //            .Take(pageSize)
        //            .ToList();
        //    }



        //    var totalCount = await _dbContext.Products.CountAsync(p => p.Name.Contains(searchTerm));
        //    _logger.LogInformation("Get All Products Endpint", products);

        //    //List<GetProductResponse> getProducts = new();

        //    //foreach (var product in products)
        //    //{
        //    //    var productResponse = new GetProductResponse
        //    //    {
        //    //        Id = product.Id,
        //    //        Name = product.Name,
        //    //        Price = product.Price,
        //    //        CategoryId = product.CategoryId,
        //    //        CategoryName = product.Category.Name
        //    //    };

        //    //    getProducts.Add(productResponse);
        //    //}

        //    var getProducts = _mapper.Map<List<GetProductResponse>>(products);
        //    var paginationedList = new PaginationedList<GetProductResponse>()
        //    {
        //        PageSize = pageSize,
        //        PageNumber = pageNumber,
        //        TotalRecords = totalCount,
        //        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
        //        Data = getProducts
        //    };
        //    var result = GenericResult<PaginationedList<GetProductResponse>>.Success(paginationedList);
        //    return Ok(result);
        //}

        //[HttpGet]
        //[Route("GetById/{id:int}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

        //    if (product is null)
        //        return NotFound($"product with id = {id} is not found");
        //    var result = GenericResult<Product>.Success(product);
        //    return Ok(result);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateProductRequest productRequest)
        //{
        //    GenericResult<bool>? result;
        //    if (productRequest is not null)
        //    {

        //        if (string.IsNullOrWhiteSpace(productRequest.Name))
        //        {
        //            result = GenericResult<bool>.Failure("product name must be passed");
        //            return BadRequest(result);
        //        }

        //        var product = new Product
        //        {
        //            Name = productRequest.Name,
        //            Price = productRequest.Price,
        //            CategoryId = productRequest.CategoryId
        //        };
        //        await _dbContext.Products.AddAsync(product);
        //        await _dbContext.SaveChangesAsync();
        //        result = GenericResult<bool>.Success(true);
        //        return Ok(result);
        //    }


        //    return BadRequest();
        //}

        //[HttpPut]
        //public async Task<IActionResult> Update(UpdateProductRequest request)
        //{
        //    var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

        //    if (product is null)
        //        return NotFound();

        //    product.Name = request.Name;
        //    product.Price = request.Price;
        //    product.CategoryId = request.CategoryId;

        //    _dbContext.Products.Update(product);

        //    await _dbContext.SaveChangesAsync();

        //    return Ok(product);
        //}

        //[HttpDelete("Delete/{id:int}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

        //    if (product is null)
        //        return NotFound();

        //    _dbContext.Products.Remove(product);
        //    await _dbContext.SaveChangesAsync();

        //    return Ok(product);
        //} 
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _service.GetAllAsync();

            if (!products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet]
        [Route("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product is null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest productRequest)
        {
            var product = new Product
            {
                Name = productRequest.Name,
                Price = productRequest.Price,
                CategoryId = productRequest.CategoryId
            };

            var result = await _service.CreateAsync(product);

            if (result is null)
                return BadRequest();

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductRequest request)
        {
            var product = await _service.GetByIdAsync(request.Id);

            if (product is null)
                return NotFound();

            product.Name = request.Name;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;

            var result = await _service.UpdateAsync(product);

            if (result is false)
                return BadRequest();

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (result is false)
                return BadRequest();

            return Ok(result);
        }
    }
}
