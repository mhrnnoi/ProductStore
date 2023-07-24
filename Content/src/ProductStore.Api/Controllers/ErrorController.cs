using Microsoft.AspNetCore.Mvc;

namespace ProductStore.Api.Controllers
{
    [Route("/error")]
    public class ErrorController : ApiController
    {
        [HttpGet]
        public IActionResult Error()
        {

            return Problem();
        }
    }
}