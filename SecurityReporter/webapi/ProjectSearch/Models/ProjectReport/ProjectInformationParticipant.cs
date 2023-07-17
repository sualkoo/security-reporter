using System.ComponentModel.DataAnnotations;
using webapi.Enums;
using webapi.ProjectSearch.Models;

namespace webapi.Models.ProjectReport
{
    public class ProjectInformationParticipant : IEntity
    {
        [Required(ErrorMessage = "Name of ProjectReportParticipant is require!")]
        public string? Name { get; set; }
        [EnumDataType(typeof(ProjectParticipantRole), ErrorMessage = "Role must be a valid ProjectParticipantRole!")]
        public ProjectParticipantRole Role { get; set; }
        [StringLength(50, ErrorMessage = "Department cannot exceed 50 characters.")]
        public string? Department { get; set; }
        public string? Contact { get; set; }
    }
}
