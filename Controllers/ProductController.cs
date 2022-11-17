using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace ProductApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }



        // GET: api/<ProducController>
        [HttpGet]
        public IEnumerable<Product> Get() => _productService.GetProducts();

        // GET api/<ProducController>/5
        [HttpGet("{id}")]
        public Product Get(int id) => _productService.GetProduct(id);

        // POST api/<ProducController>
        [HttpPost]
        public void Post([FromBody] Product value)
        {
            _productService.UpdateProduct(value);
        }

        // PUT api/<ProducController>/5
        [HttpPut]
        public void Put([FromBody] Product value)
        {
            _productService.CreateProduct(value);
        }

        // DELETE api/<ProducController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _productService.DeleteProduct(id);
        }
    }
}
