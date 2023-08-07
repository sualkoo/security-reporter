using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using webapi.Login;

namespace webapi.Login.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(string name, string password)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Already signed in!");
            }

            var testUsers = Users.Data;

            var user = testUsers.SingleOrDefault(u => u.Username == name);

            if (user == null || user.Password != password)
            {
                return BadRequest("Invalid credentials");
            }

            var token = new IdentityServerUser(user.SubjectId);
            await HttpContext.SignInAsync(token);

            return Ok("Signed in!");

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
    }
}
