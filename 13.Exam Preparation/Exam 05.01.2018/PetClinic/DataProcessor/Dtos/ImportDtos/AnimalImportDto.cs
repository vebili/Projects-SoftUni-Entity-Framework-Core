namespace PetClinic.DataProcessor.Dtos.ImportDtos
{
    using System.ComponentModel.DataAnnotations;

    public class AnimalImportDto
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Type { get; set; }

        [Range(1, int.MaxValue)]
        public int Age { get; set; }

        public PassportImportDto Passport { get; set; }
    }
}