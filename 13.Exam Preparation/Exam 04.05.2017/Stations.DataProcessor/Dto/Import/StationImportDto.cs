﻿using System.ComponentModel.DataAnnotations;

namespace Stations.DataProcessor.Dto.Import
{
    public class StationImportDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Town { get; set; }
    }
}