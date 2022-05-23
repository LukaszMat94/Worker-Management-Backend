using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WorkerManagementAPI.Data.Entities.Enums;

namespace WorkerManagementAPI.Data.Models.RoleDtos
{
    public class RoleDto
    {
        public long Id { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [RegularExpression("USER|MANAGER|ADMIN", ErrorMessage = "Only number from range 0-2")]
        public RoleEnum RoleName { get; set; } 
    }
}
