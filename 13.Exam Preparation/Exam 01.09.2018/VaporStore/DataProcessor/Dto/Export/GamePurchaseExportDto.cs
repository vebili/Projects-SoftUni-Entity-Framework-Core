namespace VaporStore.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("Game")]
    public class GamePurchaseExportDto
    {
        [XmlElement(ElementName="Genre")]
        public string Genre { get; set; }

        [XmlElement(ElementName="Price")]
        public string Price { get; set; }

        [XmlAttribute(AttributeName="title")]
        public string Title { get; set; }
    }
}