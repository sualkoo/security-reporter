using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using webapi.Login.Services;

namespace webapi.Login.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly RoleService roleService;

        public LoginController(RoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string name, string password)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Conflict("Already signed in!");
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

        [HttpGet("role")]
        public async Task<IActionResult> GetUserRole()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok(this.roleService.GetUserRoleBySubjectId(HttpContext.User?.FindFirst("sub")?.Value));
            }

            return Ok("Not signed in!");

        }
    }
}
