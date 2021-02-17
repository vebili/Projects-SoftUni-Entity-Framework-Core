using Microsoft.Data.SqlClient;
using MyClasses;
using System;

namespace CSharpDbDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new SqlConnection("");
            connection.Open();
            var query = new SqlCommand("SELECT COUNT(*) FROM Employees");
        }
    }
}