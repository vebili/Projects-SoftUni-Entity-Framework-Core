namespace CarDealer.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("sale")]
    public class SaleExportDto
    {
        [XmlElement(ElementName="car")]
        public CarAttributesExportDto Car { get; set; }

        [XmlElement(ElementName="discount")]
        public decimal Discount { get; set; }

        [XmlElement(ElementName="customer-name")]
        public string Customername { get; set; }

        [XmlElement(ElementName="price")]
        public decimal Price { get; set; }

        [XmlElement(ElementName="price-with-discount")]
        public decimal Pricewithdiscount { get; set; }
    }
}