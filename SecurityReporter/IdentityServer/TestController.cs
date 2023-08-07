﻿using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("identity")]
        public IActionResult ProtectedResource()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok($"Protected resource accessed by user with ID: id");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string name, string password)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Already signed in!");
            }

            var testUsers = Config.GetTestUsers();

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
