using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using webapi.Login.Services;

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
        private readonly RoleService roleService;

        public RoleAuthorizationHandler(RoleService roleService)
        {
            this.roleService = roleService;
        }
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (requirement.RequiredRoles.Contains(await roleService.GetUserRoleBySubjectId(context.User?.FindFirst("sub")?.Value)))
            {
                context.Succeed(requirement);
            }
        }
    }
}
