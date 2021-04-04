namespace TeisterMask.DataProcessor.ExportDto
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("Project")]
    public class ProjectExportDto
    {
        [XmlElement(ElementName= "ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement(ElementName = "HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlArray(ElementName = "Tasks")]
        public List<TaskProjectExportDto> Tasks { get; set; }


        [XmlAttribute(AttributeName = "TasksCount")]
        public int TasksCount { get; set; }
    }
}