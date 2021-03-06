﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Projection")]
    public class ProjectionDto
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        public string DateTime { get; set; }
    }
}
