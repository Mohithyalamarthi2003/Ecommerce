using Ecommerce.Data;
using Ecommerce.Dtos;
using Ecommerce.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Productapi : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductView _productview;

        private readonly IProductEdit _productedit;

        private readonly ISingleproduct _singleproduct;

        public Productapi(ApplicationDbContext context, IProductView productview, IProductEdit productedit, ISingleproduct singleproduct)
        {
            _context = context;
            _productview = productview;
            _productedit = productedit;
            _singleproduct = singleproduct; 
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {

            var products = await _productview.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:int}")]

        public async Task <IActionResult> product(int id)
        {
            var product = await _singleproduct.product(id);
            return Ok(product);
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> EditProduct(int id, ProductUpdateDto dto)
        {
            var product = await _productedit.EditProduct( id, dto);

            return Ok(product);
        }
    }
}
