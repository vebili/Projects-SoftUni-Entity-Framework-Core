namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;

    public class ImportWritersDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }

        [RegularExpression(@"^[A-Z]{1}[a-z]+[ ][A-Z]{1}[a-z]+$")]
        public string Pseudonym { get; set; }

    }
}
