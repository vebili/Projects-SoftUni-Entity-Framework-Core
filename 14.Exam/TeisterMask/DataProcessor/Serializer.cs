namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using ExportDto;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projects = context
                .Projects
                .Where(p => p.Tasks.Count > 0)
                .Select(p => new ProjectExportDto
                {
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate != null ? "Yes" : "No",
                    Tasks = p.Tasks
                        .Select(t => new TaskProjectExportDto
                        {
                            Name = t.Name,
                            Label = t.LabelType.ToString()
                        })
                        .OrderBy(t => t.Name)
                        .ToList(),
                    TasksCount = p.Tasks.Count
                })
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.ProjectName)
                .ToList();

            var attr = new XmlRootAttribute("Projects");
            var serializer = new XmlSerializer(typeof(List<ProjectExportDto>), attr);

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty
            });

            serializer.Serialize(new StringWriter(sb), projects, namespaces);

            return sb.ToString().TrimEnd();
        }


        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context
                .Employees
                .Where(e => e.EmployeesTasks.Count(t => t.Task.OpenDate >= date) > 0)
                .OrderByDescending(e => e.EmployeesTasks.Count(t => t.Task.OpenDate >= date))
                .ThenBy(e => e.Username)
                .Select(e => new EmployeeExportDto
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                        .Where(t => t.Task.OpenDate >= date)
                        .Select(t => new TaskExportDto
                        {
                            TaskName = t.Task.Name,
                            OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                            DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                            LabelType = t.Task.LabelType.ToString(),
                            ExecutionType = t.Task.ExecutionType.ToString()
                        })
                        .OrderByDescending(t => DateTime.ParseExact(t.DueDate, "d", CultureInfo.InvariantCulture))
                        .ThenBy(t => t.TaskName)
                        .ToList()
                })
                .Take(10)
                .ToList();

            var employeesJson = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return employeesJson;
        }
    }
}