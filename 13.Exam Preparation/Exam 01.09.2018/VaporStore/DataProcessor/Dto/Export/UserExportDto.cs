namespace VaporStore.DataProcessor.Dto.Export
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("User")]
    public class UserExportDto
    {
        [XmlArray(ElementName="Purchases")]
        public List<PurchaseExportDto> Purchases { get; set; }

        [XmlElement(ElementName="TotalSpent")]
        public string TotalSpent { get; set; }

        [XmlAttribute(AttributeName="username")]
        public string Username { get; set; }
    }
}