using Microsoft.AspNetCore.Mvc;

namespace PrintShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        public ProductController()
        {
            
        }

        [HttpGet]
        public Task<ActionResult<Guid>> Get()
        {

        }
    }
}
