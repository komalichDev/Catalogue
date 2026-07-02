using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Backend.Adapter
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Product>> GetProduct() {
            return NotFound();
        }
    }
}
