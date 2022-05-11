using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WorkerManagementAPI.Models.TechnologyDtos
{
    public class CreateTechnologyDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required]
        [MaxLength(50)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TechnologyLevelEnum TechnologyLevel { get; set; } = TechnologyLevelEnum.None;
    }
}
