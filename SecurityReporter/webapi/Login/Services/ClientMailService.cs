using webapi.Login;
using webapi.Login.Models;

namespace webapi.Login.Services
{
    public class ClientMailService
    {
        public string GetClientMail(string subjectId)
        {
            User user = Users.Data.FirstOrDefault(u => u.SubjectId == subjectId);

            if (user != null)
            {
                return user.Username;
            }

            return null;
        }
    }
}

