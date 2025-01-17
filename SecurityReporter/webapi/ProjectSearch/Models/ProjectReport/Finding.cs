﻿using System.ComponentModel.DataAnnotations;
using webapi.ProjectSearch.Enums;

namespace webapi.ProjectSearch.Models.ProjectReport;

public class Finding : IEntity
{
    private List<FileData>? imagesList = new();

    [Required(ErrorMessage = "FindingAuthor is required.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "FindingAuthor cannot contain numbers or special characters!")]
    public string? FindingAuthor { get; set; } = "";

    [Required(ErrorMessage = "FindingName is required.")]
    public string? FindingName { get; set; } = "";

    [MinLength(1, ErrorMessage = "Location must have at least one item.")]
    public List<string>? Location { get; set; } = new List<string>();

    public string? Component { get; set; } = "";
    public string? FoundWith { get; set; } = "";
    public string? TestMethod { get; set; } = "";
    public string? CVSS { get; set; } = "";
    public string? CVSSVector { get; set; } = "";
    public int? CWE { get; set; }

    [EnumDataType(typeof(Criticality), ErrorMessage = "Criticality must be a valid value from the Criticality enum.")]
    public Criticality Criticality { get; set; }

    [EnumDataType(typeof(Exploitability),
        ErrorMessage = "Exploitability must be a valid value from the Exploitability enum.")]
    public Exploitability Exploitability { get; set; }

    [EnumDataType(typeof(Category), ErrorMessage = "Category must be a valid value from the Category enum.")]
    public Category Category { get; set; }

    [EnumDataType(typeof(Detectability),
        ErrorMessage = "Detectability must be a valid value from the Detectability enum.")]
    public Detectability Detectability { get; set; }

    public string? SubsectionDetails { get; set; } = "";
    public string? UnformattedSubsectionDetails { get; set; } = "";
    public string? SubsectionImpact { get; set; } = "";
    public string? UnformattedSubsectionImpact { get; set; } = "";
    public string? SubsectionRepeatability { get; set; } = "";
    public string? UnformattedSubsectionRepeatability { get; set; } = "";
    public string? SubsectionCountermeasures { get; set; } = "";
    public string? UnformattedSubsectionCountermeasures { get; set; } = "";
    public string? SubsectionReferences { get; set; } = "";
    public string? UnformattedSubsectionReferences { get; set; } = "";
    public string? FolderName { get; set; } = "";

    public List<FileData>? GetImages()
    {
        return imagesList;
    }

    public void AddImage(string fileName, byte[] contents)
    {
        var newImage = new FileData();
        newImage.FileName = fileName;
        newImage.Content = contents;
        imagesList.Add(newImage);
    }

    public void ClearImageList()
    {
        imagesList = new List<FileData>();
    }
}