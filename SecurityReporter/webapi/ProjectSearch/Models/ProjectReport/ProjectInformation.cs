using System.ComponentModel.DataAnnotations;
using webapi.Enums;

namespace webapi.Models.ProjectReport
{
    public class ProjectInformation
    {
        //[Required(ErrorMessage = "ApplicationManager is required.")]
        public ProjectInformationParticipant? ApplicationManager { get; set; }
        public ProjectInformationParticipant? BusinessOwner { get; set; }
        public ProjectInformationParticipant? BusinessRepresentative { get; set; }
        public List<ProjectInformationParticipant>? TechnicalContacts { get; set; }
        public ProjectInformationParticipant? PentestLead { get; set; }
        public ProjectInformationParticipant? PentestCoordinator { get; set; }
        public List<ProjectInformationParticipant>? PentestTeam { get; set; }
        //ProjectReportName in DocumentInfoinformation
        public string? TargetInfoVersion { get; set; }
        //AssetType in DocumentInformation
        public string? TargetInfoEnviroment { get; set; }
        public bool TargetInfoInternetFacing { get; set; }
        public bool TargetInfoSNXConectivity { get; set; }
        public string? TargetInfoHostingConnection { get; set; }
        public string? TargetInfoHostingProvider { get; set; }
        public string? TargetInfoLifeCyclePhase { get; set; }
        public string? TargetInfoCriticality { get; set; }
        public string? TargetInfoAssetID { get; set; }
        public string? TargetInfoSHARPUUID { get; set; }
        public string? TargetInfoDescription { get; set; }
        public int TimeFrameTotal { get; set; }
        public DateOnly TimeFrameStart { get; set; }
        public DateOnly TimeFrameEnd { get; set; }
        public DateOnly TimeFrameReportDue { get; set; }
        public string? TimeFrameComment { get; set; }
        public int FindingsCountCriticalTBD { get; set; }
        public int FindingsCountCritical { get; set; }
        public int FindingsCountCriticalHigh { get; set; }
        public int FindingsCountCriticalMedium { get; set; }
        public int FindingsCountCriticalLow { get; set; }
        public int FindingsCountCriticalInfo { get; set; }
        public int FindingsCountCriticalTotal { get; set; }

    }
}
