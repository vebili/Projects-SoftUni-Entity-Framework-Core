namespace ProductShop.Dtos.Export
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class SoldProductsForUserExportDto
    {
        [XmlElement(ElementName="count")]
        public int Count { get; set; }

        [XmlArray(ElementName="products")]
        public List<ProductSoldDto> Products { get; set; }
    }
}