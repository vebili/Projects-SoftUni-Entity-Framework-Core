namespace ProductShop.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class UserWithProductSExportDto
    {
        [XmlElement(ElementName="firstName")]
        public string FirstName { get; set; }

        [XmlElement(ElementName="lastName")]
        public string LastName { get; set; }

        [XmlElement(ElementName="age")]
        public int? Age { get; set; }

        [XmlElement(ElementName="SoldProducts")]
        public SoldProductsForUserExportDto SoldProducts { get; set; }
    }
}