namespace PetClinic.DataProcessor.Dtos.ImportDtos
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Procedure")]
    public class ProcedureImportDto
    {
        [Required]
        [MinLength(3), MaxLength(40)]
        [XmlElement(ElementName="Vet")]
        public string Vet { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        [XmlElement(ElementName="Animal")]
        public string Animal { get; set; }
        
        [XmlElement(ElementName="DateTime")]
        public string DateTime { get; set; }

        [XmlArray(ElementName="AnimalAids")]
        public List<AnimalAidProcedureImportDto> AnimalAids { get; set; }
    }
}