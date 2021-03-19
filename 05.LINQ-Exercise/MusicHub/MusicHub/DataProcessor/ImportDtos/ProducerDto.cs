namespace MusicHub.DataProcessor.ImportDtos
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;

    public class ProducerDto
    {
        [Required]
        [MaxLength(30), MinLength(3)]
        public string Name { get; set; }

        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        public string Pseudonym { get; set; }

        [RegularExpression(@"\+359 \d{3} \d{3} \d{3}")]
        public string PhoneNumber { get; set; }

        public List<AlbumDto> Albums { get; set; }
    }
}