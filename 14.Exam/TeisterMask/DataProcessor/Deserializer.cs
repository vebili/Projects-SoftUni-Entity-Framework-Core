namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var attr = new XmlRootAttribute("Projects");
            var serializer = new XmlSerializer(typeof(List<ProjectImportDto>), attr);

            StringBuilder sb = new StringBuilder();
            var validProjects = new List<Project>();

            using (StringReader reader = new StringReader(xmlString))
            {
                var projectsDto = (List<ProjectImportDto>)serializer.Deserialize(reader);

                foreach (var dto in projectsDto)
                {
                    if (!IsValid(dto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var project  = new Project
                    {
                        Name = dto.Name,
                        OpenDate = DateTime.ParseExact(dto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DueDate = string.IsNullOrEmpty(dto.DueDate)
                            ? (DateTime?)null
                            : DateTime.ParseExact(dto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                    };

                    var projectTasks = new List<Task>();

                    foreach (var task in dto.Tasks)
                    {
                        if (!IsValid(task))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }
                        
                        var taskOpDate = DateTime.ParseExact(task.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var taskDueDate = DateTime.ParseExact(task.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        
                        bool isTaskOpDateValid = true;
                        if (taskOpDate < project.OpenDate)
                        {
                            isTaskOpDateValid = false;
                        }

                        bool isTaskDueDateValid = true;
                        if (taskDueDate > project.DueDate)
                        {
                            isTaskDueDateValid = false;
                        }

                        if (!isTaskDueDateValid || !isTaskOpDateValid)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        var taskToAdd = new Task
                        {
                            Name = task.Name,
                            OpenDate = taskOpDate,
                            DueDate = taskDueDate,
                            ExecutionType = Enum.Parse<ExecutionType>(task.ExecutionType),
                            LabelType = Enum.Parse<LabelType>(task.LabelType),
                            Project = project
                        };

                        projectTasks.Add(taskToAdd);
                    }

                    project.Tasks = projectTasks;

                    validProjects.Add(project);

                    sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
                }
            }

            context.Projects.AddRange(validProjects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeesDto = JsonConvert.DeserializeObject<List<EmployeeImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();
            var validEmployees = new List<Employee>();

            foreach (var dto in employeesDto)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var employee = new Employee
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Phone = dto.Phone
                };

                var validTasks = new List<EmployeeTask>();

                foreach (var taskNUmber in dto.Tasks.Distinct())
                {
                    var task = context.Tasks.FirstOrDefault(t => t.Id == taskNUmber);

                    if (task == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var taskEmployee = new EmployeeTask()
                    {
                        Employee = employee,
                        Task = task
                    };

                    validTasks.Add(taskEmployee);
                }
                
                employee.EmployeesTasks = validTasks;

                validEmployees.Add(employee);

                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }

            context.Employees.AddRange(validEmployees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}