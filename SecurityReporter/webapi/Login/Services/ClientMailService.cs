using webapi.Login.Models;

namespace webapi.Login.Services
{
    public class ClientMailService
    {
        private readonly Users users;

        public ClientMailService(Users users)
        {
            this.users = users;
        }

        public string GetClientMail(string subjectId)
        {
            User user = users.Data.FirstOrDefault(u => u.SubjectId == subjectId);

            if (user != null)
            {
                return user.Username;
            }

            return null;
        }
    }
}

