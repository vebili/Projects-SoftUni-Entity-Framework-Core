namespace PetClinic.DataProcessor.Dtos.ImportDtos
{
    using System.ComponentModel.DataAnnotations;

    public class PassportImportDto
    {
        [Required]
        [RegularExpression(@"^([A-Za-z]{7}[0-9]{3})$")]
        public string SerialNumber { get; set; }

        [Required]
        [MinLength(3), MaxLength(30)]
        public string OwnerName { get; set; }
        
        [Required]
        [RegularExpression(@"^(\+359|0)[0-9]{9}$")]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        public string RegistrationDate { get; set; }
    }
}