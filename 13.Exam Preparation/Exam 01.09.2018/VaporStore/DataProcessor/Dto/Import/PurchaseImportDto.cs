namespace VaporStore.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Data.Models.Enums;

    [XmlType("Purchase")]
    public class PurchaseImportDto
    {
        [Required]
        [XmlElement(ElementName="Type")]
        public string Type { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        [XmlElement(ElementName="Key")]
        public string ProductKey { get; set; }

        [Required]
        [RegularExpression(@"\b\d{4} \d{4} \d{4} \d{4}\b")]
        [XmlElement(ElementName="Card")]
        public string Card { get; set; }

        [Required]
        [XmlElement(ElementName="Date")]
        public string Date { get; set; }

        [Required]
        [XmlAttribute(AttributeName="title")]
        public string Title { get; set; }
    }
}