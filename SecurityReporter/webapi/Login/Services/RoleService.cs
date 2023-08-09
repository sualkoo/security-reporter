using webapi.Login;
using webapi.Login.Models;

namespace webapi.Login.Services
{
    public class RoleService
    {
        public string GetUserRoleBySubjectId(string subjectId)
        {
            User user = Users.Data.FirstOrDefault(u => u.SubjectId == subjectId);

            if (user != null)
            {
                return user.Role;
            }

            return null;
        }
    }
}
