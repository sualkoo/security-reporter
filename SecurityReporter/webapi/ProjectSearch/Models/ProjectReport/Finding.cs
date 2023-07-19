﻿using System.ComponentModel.DataAnnotations;
using webapi.Enums;
using webapi.ProjectSearch.Models;

namespace webapi.Models.ProjectReport
{
    public class Finding : IEntity
    {
        [Required(ErrorMessage = "FindingAuthor is required.")]
        public string? FindingAuthor { get; set; }
        [Required(ErrorMessage = "FindingName is required.")]
        public string? FindingName { get; set; }
        [MinLength(1, ErrorMessage = "Location must have at least one item.")]
        public List<string>? Location { get; set; }
        public string? Component { get; set; }
        public string? FoundWith { get; set; }
        public string? TestMethod { get; set; }
        public string? CVSS { get; set; }
        public string? CVSSVector { get; set; }
        public string? CWE { get; set; }
        [EnumDataType(typeof(Criticality), ErrorMessage = "Criticality must be a valid value from the Criticality enum.")]
        public Criticality Criticality { get; set; }

        [EnumDataType(typeof(Exploitability), ErrorMessage = "Exploitability must be a valid value from the Exploitability enum.")]
        public Exploitability Exploitability { get; set; }

        [EnumDataType(typeof(Category), ErrorMessage = "Category must be a valid value from the Category enum.")]
        public Category Category { get; set; }

        [EnumDataType(typeof(Detectability), ErrorMessage = "Detectability must be a valid value from the Detectability enum.")]
        public Detectability Detectability { get; set; }
        public string? SubsectionDetails { get; set; }
        public string? SubsectionImpact { get; set; }
        public string? SubsectionRepeatability { get; set; }
        public string? SubsectionCountermeasures { get; set; }
        [MinLength(1, ErrorMessage = "SubsectionReferences must have at least one item.")]
        public List<string>? SubsectionReferences { get; set; }
    }
}
