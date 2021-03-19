namespace MusicHub.DataProcessor.ImportDtos
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Performer")]
    public class PerformerDto
    {
        [Required]
        [MaxLength(20), MinLength(3)]
        [XmlElement(ElementName="FirstName")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20), MinLength(3)]
        [XmlElement(ElementName="LastName")]
        public string LastName { get; set; }

        
        [Range(18,70)]
        [XmlElement(ElementName="Age")]
        public int Age { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [XmlElement(ElementName="NetWorth")]
        public decimal NetWorth { get; set; }

        [XmlArray(ElementName="PerformersSongs")]
        public List<SongPerformerDto> PerformersSongs { get; set; }
    }
}