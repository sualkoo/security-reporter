using System.ComponentModel.DataAnnotations;

namespace webapi.ProjectSearch.Models.ProjectReport;


    public class TestingMethodology : IEntity
    {
    private string toolsUsed = "";
    private string attackVectors = "";

        [Required(ErrorMessage = "ToolsUsed is required!")]
    public string? ToolsUsed {
        get { return toolsUsed; }
        set { toolsUsed = value; }
    }
        [Required(ErrorMessage = "AttackVectors is required!")]
        public string? AttackVectors {
            get { return attackVectors; }
            set { attackVectors = value; }
        }
    }