namespace PetClinic.DataProcessor.Dtos.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Vet")]
    public class VetImportDto
    {
        [Required]
        [MinLength(3), MaxLength(40)]
        [XmlElement(ElementName="Name")]
        public string Name { get; set; }

        [Required]
        [MinLength(3), MaxLength(50)]
        [XmlElement(ElementName="Profession")]
        public string Profession { get; set; }

        [Range(22, 65)]
        [XmlElement(ElementName="Age")]
        public int Age { get; set; }

        [Required]
        [RegularExpression(@"^(\+359|0)[0-9]{9}$")]
        [XmlElement(ElementName="PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}