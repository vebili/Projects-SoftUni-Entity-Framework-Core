namespace PetClinic.DataProcessor.Dtos.ExportDtos
{
    using System.Xml.Serialization;

    [XmlType("AnimalAid")]
    public class AnimalAidExportDto
    {
        [XmlElement(ElementName="Name")]
        public string Name { get; set; }

        [XmlElement(ElementName="Price")]
        public decimal Price { get; set; }
    }
}