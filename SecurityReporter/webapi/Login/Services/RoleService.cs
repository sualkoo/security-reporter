using Microsoft.AspNetCore.Http;
using webapi.Login;
using webapi.Login.Models;
using webapi.Service;

namespace webapi.Login.Services
{
    public class RoleService
    {
        private readonly ClientMailService clientMailService;
        private readonly CosmosRolesService cosmosRolesService;

        public RoleService(ClientMailService clientMailService, CosmosRolesService cosmosRolesService)
        {
            this.clientMailService = clientMailService;
            this.cosmosRolesService = cosmosRolesService;
        }
        public async Task<string> GetUserRoleBySubjectId(string subjectId)
        {

            // REMOVE ME IN PRODUCTION VERSION
            cosmosRolesService.AddRole("client@client.sk", "client");
            cosmosRolesService.AddRole("admin@admin.sk", "admin");
            cosmosRolesService.AddRole("pentester@pentester.sk", "pentester");
            cosmosRolesService.AddRole("coordinator@coordinator.sk", "coordinator");
            cosmosRolesService.AddRole("default@default.sk", "default");


            var mail = clientMailService.GetClientMail(subjectId);

            if (mail != null)
            {
                var role = await cosmosRolesService.GetRole(mail);

                if (role != null)
                {
                    return role;
                }
            }

            return "default";
        }
    }
}
