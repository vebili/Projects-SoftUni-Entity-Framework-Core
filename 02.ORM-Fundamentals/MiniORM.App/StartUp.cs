namespace MiniORM.App
{
    using System.Linq;
    using Data;
    using Data.Entities;

    class StartUp
    {
        static void Main(string[] args)
        {
            var connectionString = @"Server=.;Database=MiniORM;Trusted_Connection=True";

            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Pesho",
                LastName = "Peshev",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true,
            });

            var employee = context.Employees.Last();
            employee.FirstName = "Modified";

            context.SaveChanges();
        }
    }
}
