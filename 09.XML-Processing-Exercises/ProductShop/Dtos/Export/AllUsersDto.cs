namespace ProductShop.Dtos.Export
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="Users")]
    public class AllUsersDto
    {
        [XmlElement(ElementName="count")]
        public int Count { get; set; }

        [XmlArray(ElementName="users")]
        public List<UserWithProductSExportDto> Users { get; set; }
    }
}