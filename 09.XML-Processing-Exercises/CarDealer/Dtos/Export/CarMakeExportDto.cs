namespace CarDealer.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("car")]
    public class CarMakeExportDto
    {
        [XmlAttribute(AttributeName="id")]
        public int Id { get; set; }

        [XmlAttribute(AttributeName="model")]
        public string Model { get; set; }

        [XmlAttribute(AttributeName="travelled-distance")]
        public long Travelleddistance { get; set; }
    }
}