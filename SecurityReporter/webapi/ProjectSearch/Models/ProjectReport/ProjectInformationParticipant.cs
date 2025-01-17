﻿using System.ComponentModel.DataAnnotations;

namespace webapi.ProjectSearch.Models.ProjectReport;

public class ProjectInformationParticipant : IEntity
{
    [Required(ErrorMessage = "Name of ProjectReportParticipant is require!")]
    [RegularExpression(@"^[a-zA-Z\s]+$",
        ErrorMessage = "Name of ProjectInformationParticipant cannot contain numbers or special characters!")]
    public string? Name { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Department cannot exceed 50 characters.")]
    public string? Department { get; set; } = string.Empty;

    public string? Contact { get; set; } = string.Empty;
}