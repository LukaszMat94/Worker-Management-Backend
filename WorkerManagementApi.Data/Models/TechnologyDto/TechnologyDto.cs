﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WorkerManagementAPI.Entities.Enums;

namespace WorkerManagementAPI.Models.TechnologyDtos
{
    public class TechnologyDto
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TechnologyLevelEnum TechnologyLevel { get; set; } = TechnologyLevelEnum.None;
    }
}
