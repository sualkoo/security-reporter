using webapi.Enums;

namespace webapi.Models.ProjectReport
{
    public class ProjectInformationParticipant
    {
        public string? Name { get; set; }
        public ProjectParticipantRole Role { get; set; }
        public string? Department { get; set; }
        public string? Contact { get; set; }
    }
}
