using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WorkerManagementAPI.Data.Entities.Enums;

namespace WorkerManagementAPI.Data.Models.TechnologyDtos
{
    public class CreateTechnologyDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [RegularExpression("None|Basic|Medium|Advanced", ErrorMessage = "Only number from range 0-3")]
        public TechnologyLevelEnum TechnologyLevel { get; set; } = TechnologyLevelEnum.None;
    }
}
