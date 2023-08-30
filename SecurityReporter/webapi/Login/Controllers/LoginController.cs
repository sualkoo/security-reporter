using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using webapi.Login.Models;
using webapi.Service;

namespace webapi.Login.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        public ICosmosService CosmosService { get; }

        public LoginController(ICosmosService cosmosService)
        {
            CosmosService = cosmosService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(string name, string password)
        {
            if (name == "Username")
            {
                return BadRequest("Invalid credentials");

            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Conflict("Already signed in!");
            }

            var user = await CosmosService.GetUserRole(name);


            if (user.id == name && user.id == password)
            {
                var token = new IdentityServerUser(user.id);
                await HttpContext.SignInAsync(token);

                return Ok("Signed in!");
            }
            return BadRequest("Invalid credentials");

        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                return Ok("Signed out!");
            }
            return BadRequest("Not signed in!");

        }

        [HttpGet("role")]
        public async Task<IActionResult> GetUserRole()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                UserRole result = await CosmosService.GetUserRole(HttpContext.User?.FindFirst("sub")?.Value);
                return Ok(result.Role);
            }

            return StatusCode(401, "Not signed in!");

        }
    }
}
