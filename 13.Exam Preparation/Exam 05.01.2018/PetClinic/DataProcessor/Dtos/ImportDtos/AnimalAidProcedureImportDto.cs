namespace PetClinic.DataProcessor.Dtos.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("AnimalAid")]
    public class AnimalAidProcedureImportDto
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        [XmlElement(ElementName="Name")]
        public string Name { get; set; }
    }
}