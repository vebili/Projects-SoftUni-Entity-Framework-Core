using Microsoft.Data.SqlClient;
using MyClasses;
using System;

namespace CSharpDbDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //.
            //localhost
            //127.0.0.1
            //name of the PC
            //Integrated Security=true;
            //string connectionString = "Server=.;Integrated Security=true;Database=SoftUni";
            //var connection = new SqlConnection(connectionString);
            //connection.Open();
            //var query = new SqlCommand("SELECT COUNT(*) FROM Employees");
            //connection.Close();
            using (var connection = new SqlConnection("Server=.;Integrated Security=true;Database=SoftUni"))
            {
                connection.Open();

            }
        }
    }
}