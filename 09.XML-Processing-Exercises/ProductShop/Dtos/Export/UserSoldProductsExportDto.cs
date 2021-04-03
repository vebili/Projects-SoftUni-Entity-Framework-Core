namespace ProductShop.Dtos.Export
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Models;

    [XmlType("User")]
    public class UserSoldProductsExportDto
    {
        [XmlElement(ElementName="firstName")]
        public string FirstName { get; set; }

        [XmlElement(ElementName="lastName")]
        public string LastName { get; set; }

        [XmlArray(ElementName="soldProducts")]
        public List<ProductSoldDto> SoldProducts { get; set; }
    }
}