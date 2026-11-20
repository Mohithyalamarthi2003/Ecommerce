using Ecommerce.Data;
using Ecommerce.Dtos;
using Ecommerce.Services.Implementation;
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

        private readonly ICreateProduct _createproduct;
        private readonly IProductEdit _productedit;

        private readonly ISingleproduct _singleproduct;

        private readonly IProductDelete _productdelete;

        public Productapi(ApplicationDbContext context, IProductView productview, ICreateProduct createproduct, IProductEdit productedit, ISingleproduct singleproduct, IProductDelete productdelete)
        {
            _context = context;
            _productview = productview;
            _createproduct = createproduct;
            _productedit = productedit;
            _singleproduct = singleproduct;
            _productdelete = productdelete;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {

            var products = await _productview.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> product(int id)
        {
            var product = await _singleproduct.product(id);
            return Ok(product);
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> EditProduct(int id, ProductUpdateDto dto)
        {
            var product = await _productedit.EditProduct(id, dto);

            return Ok(product);
        }


        [HttpDelete("{id:int}")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productdelete.DeleteProduct(id);

            return Ok(product);
        }

        [HttpPost]

        public async Task<IActionResult> CreateProduct([FromForm]Productcreatedto dto)
        {
            var product = await _createproduct.CreateProduct(dto);
            return Ok(product);
        }
        
    }
}
