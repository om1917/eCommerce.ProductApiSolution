using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProduct productInterface): ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            //Get All Product from repo
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
                return NotFound("No Product found in Database");

            //Convert data from entity to DTO and return
            var (_, list) = ProductConversion.FromEntity(null!, products);
            return list.Any() ? Ok(list) : NotFound("No product found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            //Get single product from the repo 
            var product = await productInterface.FindByIdAsync(id);
            if (product is null)
                return NotFound("Product not found");

            //Convert from entity to DTO and return 
            var (_product, list) = ProductConversion.FromEntity(product, null!);
            return _product is not null? Ok(list) : NotFound($"Product {product.Name} not found.");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            //check model state if all data annotations are passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Convert to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);   
            return response.flag is true? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            //check model state if all data annotations are passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Convert to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);
            return response.flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product)
        {
            //Convert to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.DeleteAsync(getEntity);
            return response.flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
