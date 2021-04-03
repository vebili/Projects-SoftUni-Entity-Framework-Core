namespace CarDealer.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("customer")]
    public class CustomerExportDto
    {
        [XmlAttribute(AttributeName="full-name")]
        public string Fullname { get; set; }

        [XmlAttribute(AttributeName="bought-cars")]
        public int Boughtcars { get; set; }

        [XmlAttribute(AttributeName="spent-money")]
        public decimal Spentmoney { get; set; }
    }
}