namespace SoftUni
{
    using Data;
    using Models;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                Console.WriteLine(RemoveTown(context));
            }
        }

        //Problem 03
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    FullName = string.Join(" ", e.FirstName, e.LastName, e.MiddleName),
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                });

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FullName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 04
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    Salary = e.Salary
                });

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 05
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Department = e.Department.Name,
                    Salary = e.Salary
                });

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 06
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var nakov = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

            nakov.Address = newAddress;

            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(e => e.Address.AddressId)
                .Select(a => a.Address.AddressText)
                .Take(10);

            foreach (var e in employees)
            {
                sb.AppendLine(e);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 07
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.EmployeesProjects.Any(s =>
                    s.Project.StartDate.Year >= 2001 && s.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName,
                    ManagerFullName = e.Manager.FirstName + " " + e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(p => new
                                                        {
                                                            p.Project.Name,
                                                            p.Project.StartDate,
                                                            p.Project.EndDate
                                                        }).ToList()
                })
                .Take(10)
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FullName} - Manager: {e.ManagerFullName}");

                foreach (var project in e.Projects)
                {
                    var start = project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    var end = project.EndDate.HasValue
                        ? project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                        : "not finished";
                    var name = project.Name;

                    sb.AppendLine($"--{name} - {start} - {end}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 08
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses
                .Select(a => new
                {
                    EmpCount = a.Employees.Count,
                    Text = a.AddressText,
                    Town = a.Town.Name
                })
                .OrderByDescending(a => a.EmpCount)
                .ThenBy(a => a.Town)
                .ThenBy(a => a.Text)
                .Take(10);

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.Text}, {a.Town} - {a.EmpCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 09
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee147 = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Projects = e.EmployeesProjects.Select(p => p.Project.Name).OrderBy(p => p).ToList()
                })
                .First();

            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            foreach (var project in employee147.Projects)
            {
                sb.AppendLine(project);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepName = d.Name,
                    Menager = d.Manager.FirstName + " " + d.Manager.LastName,
                    Employees = d.Employees.Select(e => new
                    {
                        FullName = e.FirstName + " " + e.LastName,
                        Job = e.JobTitle
                    }).ToList()
                })
                .ToList();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.DepName} - {d.Menager}");
                foreach (var e in d.Employees.OrderBy(e => e.FullName))
                {
                    sb.AppendLine($"{e.FullName} - {e.Job}");
                }
            }


            return sb.ToString().TrimEnd();
        }

        //Problem 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Select(p => new
                {
                    Name = p.Name,
                    Description = p.Description,
                    Start = p.StartDate
                })
                .Take(10);

            foreach (var p in projects.OrderBy(p => p.Name))
            {
                sb.AppendLine(String.Join(Environment.NewLine, p.Name, p.Description, p.Start.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)));
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var empToIncreaseSalary = context.Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services");

            foreach (var e in empToIncreaseSalary)
            {
                e.Salary *= 1.12m;
            }

            context.SaveChanges();

            var empToPrint = empToIncreaseSalary
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FullName);

            foreach (var e in empToPrint)
            {
                sb.AppendLine($"{e.FullName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var empoyees = context.Employees
                .Where(e => EF.Functions.Like(e.FirstName, "Sa%"))
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName,
                    Job = e.JobTitle,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FullName);

            foreach (var e in empoyees)
            {
                sb.AppendLine($"{e.FullName} - {e.Job} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projectToRemove = context.Projects.Find(2);

            var employeesWithProject2 = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(employeesWithProject2);

            context.Projects.Remove(projectToRemove);

            context.SaveChanges();

            foreach (var p in context.Projects)
            {
                sb.AppendLine(p.Name);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 15
        public static string RemoveTown(SoftUniContext context)
        {
            var seattle = context.Towns.First(t => t.Name == "Seattle");

            var addressesInSeattle = context.Addresses
                .Where(a => a.Town == seattle);

            var empInSeattle = context.Employees
                .Where(e => addressesInSeattle.Contains(e.Address));

            foreach (var e in empInSeattle)
            {
                e.AddressId = null;
            }

            var count = addressesInSeattle.Count();

            context.Addresses.RemoveRange(addressesInSeattle);
            context.Towns.Remove(seattle);

            context.SaveChanges();

            return $"{count} addresses in Seattle were deleted";
        }
    }
}