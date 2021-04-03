namespace CarDealer.Dtos.Import
{
    using System.Xml.Serialization;

    [XmlType("partId")]
    public class PartIdDto
    {
        [XmlAttribute(AttributeName="id")]
        public int Id { get; set; }
    }
}