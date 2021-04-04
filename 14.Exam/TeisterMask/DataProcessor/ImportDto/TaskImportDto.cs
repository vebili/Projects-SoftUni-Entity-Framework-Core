namespace TeisterMask.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Task")]
    public class TaskImportDto
    {
        [Required]
        [MaxLength(40), MinLength(2)]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [Required]
        [XmlElement(ElementName = "OpenDate")]
        public string OpenDate { get; set; }

        [Required]
        [XmlElement(ElementName = "DueDate")]
        public string DueDate { get; set; }

        [Required]
        [XmlElement(ElementName = "ExecutionType")]
        public string ExecutionType { get; set; }

        [Required]
        [XmlElement(ElementName = "LabelType")]
        public string LabelType { get; set; }
    }
}