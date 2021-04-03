namespace ProductShop.Dtos.Import
{
    using System.Xml.Serialization;

    [XmlType("Category")]
    public class CategoryImportDto
    {
        [XmlElement(ElementName="name")]
        public string Name { get; set; }
    }
}