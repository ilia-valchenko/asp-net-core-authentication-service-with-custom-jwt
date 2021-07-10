using System;
using AuthenticationService.Filters;
using AuthenticationService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
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
            return Ok(new[]
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "RTX"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "QPA"
                }
            });
        }
    }
}