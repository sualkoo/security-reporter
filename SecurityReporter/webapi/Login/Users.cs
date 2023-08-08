using webapi.Login.Models;

namespace webapi.Login
{
    public static class Users
    {
        public static List<User> Data { get; } = new List<User>
        {
            new User { SubjectId = "1", Username = "admin", Password = "admin", Role = "admin" },
            new User { SubjectId = "2", Username = "pentester", Password = "pentester", Role = "pentester" },
            new User { SubjectId = "3", Username = "coordinator", Password = "coordinator", Role = "coordinator" },
            new User { SubjectId = "4", Username = "client", Password = "client", Role = "client" },
            new User { SubjectId = "5", Username = "default", Password = "default", Role = "default" },
        };
    }
}
