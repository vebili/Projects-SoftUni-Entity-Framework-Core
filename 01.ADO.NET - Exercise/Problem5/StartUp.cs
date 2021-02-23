namespace Problem5
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class StartUp
    {
        public const string ConnectionStringMinionsDB = @"Server=.;Database=MinionsDB;Integrated Security=True";
        public static void Main(string[] args)
        {
            string country = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(ConnectionStringMinionsDB))
            {
                connection.Open();

                string updateSQL = @"UPDATE Towns
                                     SET Name = UPPER(Name)
                                     WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                using (SqlCommand command = new SqlCommand(updateSQL, connection))
                {
                    command.Parameters.AddWithValue("@countryName", country);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} town names were affected. ");
                }

                string selectSQL = @"SELECT t.Name 
                                     FROM Towns as t
                                     JOIN Countries AS c ON c.Id = t.CountryCode
                                     WHERE c.Name = @countryName";
                List<string> towns = new List<string>();

                using (SqlCommand command = new SqlCommand(selectSQL, connection))
                {
                    command.Parameters.AddWithValue("@countryName", country);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            towns.Add((string)reader[0]);
                        }
                    }
                }

                Console.WriteLine("[" + string.Join(", ", towns) + "]");
            }
        }
    }
}
