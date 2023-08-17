using webapi.Models;

namespace webapi.MyProfile.Models
{
    public class Profile
    {
        public string Role { get; set; }
        public List<ProjectData> Projects { get; set; }
    }
}
