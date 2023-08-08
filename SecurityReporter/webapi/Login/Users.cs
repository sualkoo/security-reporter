using Microsoft.OpenApi.Any;
using System.Collections.Generic;
using webapi.Login.Models;
using webapi.Login.Services;

namespace webapi.Login
{
    public class Users
    {
        public List<User> Data { get; } = new List<User>();

        public async Task AssignRoles(RoleService roleService)
        {
            Data.Add(new User { SubjectId = "1", Username = "admin@admin.sk", Password = "admin", Role = await roleService.GetUserRoleBySubjectId("1") });
            Data.Add(new User { SubjectId = "2", Username = "pentester@pentester.sk", Password = "pentester", Role = await roleService.GetUserRoleBySubjectId("2") });
            Data.Add(new User { SubjectId = "3", Username = "coordinator@coordinator.sk", Password = "coordinator", Role = await roleService.GetUserRoleBySubjectId("3") });
            Data.Add(new User { SubjectId = "4", Username = "client@client.sk", Password = "client", Role = await roleService.GetUserRoleBySubjectId("4") });
            Data.Add(new User { SubjectId = "5", Username = "default@default.sk", Password = "default", Role = await roleService.GetUserRoleBySubjectId("5") });
        }
    }
}