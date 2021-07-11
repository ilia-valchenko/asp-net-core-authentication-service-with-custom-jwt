using System;
using AuthenticationService.Filters;
using AuthenticationServiceWithCustomJwt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServiceWithCustomJwt.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [TokenAuthenticationFilter]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new Product[] {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "The first product"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "The second product"
                }
            });
        }
    }
}