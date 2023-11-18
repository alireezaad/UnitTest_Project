using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models.Entities;
using WebAppMVC.Models.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAppMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly HttpClient _client;

        //public ProductApiController(IProductService service)
        //{
        //    _service = service;
        //}
        public ProductApiController(IProductService service, HttpClient client = null)
        {
            _service = service;
            _client = client;
        }

        // GET: api/<ProductApiController>
        [HttpGet]
        public async Task<List<Product>> Get()
        {
            var response = await _client.GetAsync("https://example.com");
            if (!response.IsSuccessStatusCode)
            {
                return (new List<Product> { });
            }
            //return Ok(_service.GetProducts());
            var responseContent = await response.Content.ReadFromJsonAsync<List<Product>>();
            return responseContent;

        }

        // GET api/<ProductApiController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _service.GetProductById(id);
            if (product == null)
            {
                //return NotFound("I didnt find it"); // --> NotFoundObjectResult
                return NotFound(); // --> NotFoundResult
            }
            else
                return Ok(product);
        }

        // POST api/<ProductApiController>
        [HttpPost]
        public IActionResult Post([FromBody] Product value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            _service.Add(value);
            return CreatedAtAction("Get", new { value.Id});
        }

        // PUT api/<ProductApiController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            return Ok(_service.Update(value));
        }

        // DELETE api/<ProductApiController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _service.GetProductById(id);
            if (product == null)
                return NotFound();
            _service.Delete(id);
            return Ok(true);
        }
    }
}
