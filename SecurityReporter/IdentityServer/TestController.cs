using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
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
            var htttpClient = new HttpClient();
            var token = await htttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "https://localhost:5001/connect/token",
                ClientId = "ConsoleApp_ClientId",
                ClientSecret = "nieco_nevulgarne",
                Scope = ""
            });

            Console.WriteLine(token.AccessToken);
            return Ok(token.AccessToken);
        }
    }
}
