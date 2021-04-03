namespace CarDealer.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("car")]
    public class CarDetailsExportDto
    {
        [XmlArray(ElementName="parts")]
        public PartExportDto[] Parts { get; set; }

        [XmlAttribute(AttributeName="make")]
        public string Make { get; set; }

        [XmlAttribute(AttributeName="model")]
        public string Model { get; set; }

        [XmlAttribute(AttributeName="travelled-distance")]
        public long Travelleddistance { get; set; }
    }
}