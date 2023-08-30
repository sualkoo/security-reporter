using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using webapi.Login.Controllers;
using webapi.Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using webapi.Service;

namespace webapi.Login.Utils.Authorization
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string[] RequiredRoles { get; }

        public RoleRequirement(params string[] requiredRoles)
        {
            RequiredRoles = requiredRoles;
        }
    }

    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        private HttpClient _httpClient;
        private IHttpContextAccessor _httpContextAccessor;
        private ICosmosService cosmosService;

        public RoleAuthorizationHandler(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ICosmosService cosmosService)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            this.cosmosService = cosmosService;
        }
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            UserRole result;
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            var subClaim = httpContext.User.FindFirst("sub");
            if (subClaim != null)
            {
                string userId = subClaim.Value;
                result = await cosmosService.GetUserRole(userId);

                if (requirement.RequiredRoles.Contains(result.Role))
                {
                    context.Succeed(requirement);
                }
            }


        }
        }
    }
