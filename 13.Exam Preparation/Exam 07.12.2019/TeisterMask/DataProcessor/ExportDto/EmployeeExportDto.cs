namespace TeisterMask.DataProcessor.ExportDto
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class EmployeeExportDto
    {
        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Tasks")]
        public List<TaskExportDto> Tasks { get; set; }
    }
}