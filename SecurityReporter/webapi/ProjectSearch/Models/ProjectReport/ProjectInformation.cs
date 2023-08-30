using System.ComponentModel.DataAnnotations;

namespace webapi.ProjectSearch.Models.ProjectReport;

public class ProjectInformation : IEntity
{
    [Required(ErrorMessage = "ApplicationManager is required!")]
    public ProjectInformationParticipant? ApplicationManager { get; set; } = new ProjectInformationParticipant();

    [Required(ErrorMessage = "BusinessOwner is required!")]
    public ProjectInformationParticipant? BusinessOwner { get; set; } = new ProjectInformationParticipant();

    [Required(ErrorMessage = "BusinessRepresentative is required!")]
    public ProjectInformationParticipant? BusinessRepresentative { get; set; } = new ProjectInformationParticipant();

    [MinLength(1, ErrorMessage = "TechnicalContacts must have at least one participant.")]
    public List<ProjectInformationParticipant>? TechnicalContacts { get; set; } = new List<ProjectInformationParticipant>();

    [Required(ErrorMessage = "PentestLead is required!")]
    public ProjectInformationParticipant? PentestLead { get; set; } = new ProjectInformationParticipant();

    [Required(ErrorMessage = "PentestCoordinator is required!")]
    public ProjectInformationParticipant? PentestCoordinator { get; set; } = new ProjectInformationParticipant();

    [MinLength(1, ErrorMessage = "PentestTeam must have at least one participant.")]
    public List<ProjectInformationParticipant>? PentestTeam { get; set; } = new List<ProjectInformationParticipant>();

    //ProjectReportName in DocumentInformation
    [StringLength(50, ErrorMessage = "TargetInfoVersion cannot exceed 50 characters.")]
    [RegularExpression("^[a-zA-Z0-9.]*$",
        ErrorMessage = "TargetInfoVersion can only contain alphanumeric characters and periods.")]
    public string? TargetInfoVersion { get; set; } = string.Empty;

    //AssetType in DocumentInformation
    public string? TargetInfoEnvironment { get; set; } = string.Empty;

    [Range(typeof(bool), "false", "true", ErrorMessage = "TargetInfoInternetFacing must be either true or false!")]
    public bool TargetInfoInternetFacing { get; set; }

    [Range(typeof(bool), "false", "true", ErrorMessage = "TargetInfoSNXConnectivity must be either true or false!")]
    public bool TargetInfoSNXConnectivity { get; set; }

    [StringLength(100, ErrorMessage = "TargetInfoHostingLocation cannot exceed 100 characters.")]
    public string? TargetInfoHostingLocation { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "TargetInfoHostingProvider cannot exceed 100 characters!")]
    public string? TargetInfoHostingProvider { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "TargetInfoLifeCyclePhase cannot exceed 100 characters!")]
    public string? TargetInfoLifeCyclePhase { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "TargetInfoCriticality cannot exceed 100 characters!")]
    public string? TargetInfoCriticality { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "TargetInfoAssetID cannot exceed 100 characters!")]
    public string? TargetInfoAssetID { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "TargetInfoSHARPUUID cannot exceed 100 characters!")]
    public string? TargetInfoSHARPUUID { get; set; } = string.Empty;

    [StringLength(5000, ErrorMessage = "TargetInfoDescription cannot exceed 5000 characters!")]
    public string? TargetInfoDescription { get; set; } = string.Empty;

    [Range(0, 1000, ErrorMessage = "TimeFrameTotal must be a non-negative integer not exceeding 1000!")]
    public int TimeFrameTotal { get; set; }

    [DataType(DataType.Date)] public DateTime TimeFrameStart { get; set; }

    [DataType(DataType.Date)]
    [DateOrderValidation(nameof(TimeFrameStart), ErrorMessage = "TimeFrameEnd must be later than TimeFrameStart.")]
    public DateTime TimeFrameEnd { get; set; }

    [DataType(DataType.Date)]
    [DateOrderValidation(nameof(TimeFrameStart),
        ErrorMessage = "TimeFrameReportDue must be later than TimeFrameStart.")]
    public DateTime TimeFrameReportDue { get; set; }

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

public class DateOrderValidationAttribute : ValidationAttribute
{
    private readonly string _earlierDatePropertyName;

    public DateOrderValidationAttribute(string earlierDatePropertyName)
    {
        _earlierDatePropertyName = earlierDatePropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var earlierDateProperty = validationContext.ObjectType.GetProperty(_earlierDatePropertyName);

        if (earlierDateProperty == null) throw new ArgumentException($"Property {_earlierDatePropertyName} not found.");

        var earlierDateValue = (DateTime)earlierDateProperty.GetValue(validationContext.ObjectInstance);
        var laterDateValue = (DateTime)value;

        if (earlierDateValue > laterDateValue) return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}