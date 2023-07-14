using System.ComponentModel.DataAnnotations;
using webapi.Enums;
using webapi.ProjectSearch.Models;

namespace webapi.Models.ProjectReport
{
    public class ProjectInformation : IEntity
    {
        //[Required(ErrorMessage = "ApplicationManager is required.")]
        public ProjectInformationParticipant? ApplicationManager { get; set; }
        public ProjectInformationParticipant? BusinessOwner { get; set; }
        public ProjectInformationParticipant? BusinessRepresentative { get; set; }
        public List<ProjectInformationParticipant>? TechnicalContacts { get; set; }
        public ProjectInformationParticipant? PentestLead { get; set; }
        public ProjectInformationParticipant? PentestCoordinator { get; set; }
        public List<ProjectInformationParticipant>? PentestTeam { get; set; }
        //ProjectReportName in DocumentInformation
        [StringLength(50, ErrorMessage = "TargetInfoVersion cannot exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z0-9.]*$", ErrorMessage = "TargetInfoVersion can only contain alphanumeric characters and periods.")]
        public string? TargetInfoVersion { get; set; }
        //AssetType in DocumentInformation
        public string? TargetInfoEnvironment { get; set; }
        [Range(typeof(bool), "false", "true", ErrorMessage = "TargetInfoInternetFacing must be either true or false!")]
        public bool TargetInfoInternetFacing { get; set; }

        [Range(typeof(bool), "false", "true", ErrorMessage = "TargetInfoSNXConnectivity must be either true or false!")]
        public bool TargetInfoSNXConnectivity { get; set; }
        [StringLength(100, ErrorMessage = "TargetInfoHostingConnection cannot exceed 100 characters.")]
        public string? TargetInfoHostingConnection { get; set; }

        [StringLength(100, ErrorMessage = "TargetInfoHostingProvider cannot exceed 100 characters!")]
        public string? TargetInfoHostingProvider { get; set; }

        [StringLength(100, ErrorMessage = "TargetInfoLifeCyclePhase cannot exceed 100 characters!")]
        public string? TargetInfoLifeCyclePhase { get; set; }

        [StringLength(100, ErrorMessage = "TargetInfoCriticality cannot exceed 100 characters!")]
        public string? TargetInfoCriticality { get; set; }

        [StringLength(100, ErrorMessage = "TargetInfoAssetID cannot exceed 100 characters!")]
        public string? TargetInfoAssetID { get; set; }

        [StringLength(100, ErrorMessage = "TargetInfoSHARPUUID cannot exceed 100 characters!")]
        public string? TargetInfoSHARPUUID { get; set; }

        [StringLength(5000, ErrorMessage = "TargetInfoDescription cannot exceed 5000 characters!")]
        public string? TargetInfoDescription { get; set; }
        [Range(0, 1000, ErrorMessage = "TimeFrameTotal must be a non-negative integer not exceeding 1000!")]
        public int TimeFrameTotal { get; set; }
        [DataType(DataType.Date)]
        public DateTime TimeFrameStart { get; set; }

        [DataType(DataType.Date)]
        [Compare(nameof(TimeFrameStart), ErrorMessage = "TimeFrameEnd must be a date that is later than or equal to TimeFrameStart!")]
        public DateTime TimeFrameEnd { get; set; }
        [DataType(DataType.Date)]
        [Compare(nameof(TimeFrameStart), ErrorMessage = "TimeFrameReportDue must be a date that is later than or equal to TimeFrameStart!")]
        public DateOnly TimeFrameReportDue { get; set; }
        public string? TimeFrameComment { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "FindingsCountCriticalTBD must be greater than or equal to 0!")]
        public int FindingsCountCriticalTBD { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "FindingsCountCritical must be greater than or equal to 0!")]
        public int FindingsCountCritical { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "FindingsCountHigh must be greater than or equal to 0!")]
        public int FindingsCountHigh { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "FindingsCountMedium must be greater than or equal to 0!")]
        public int FindingsCountMedium { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "FindingsCountLow must be greater than or equal to 0!")]
        public int FindingsCountLow { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "FindingsCountInfo must be greater than or equal to 0!")]
        public int FindingsCountInfo { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "FindingsCountTotal must be greater than or equal to 0!")]
        public int FindingsCountTotal { get; set; }

    }
}
