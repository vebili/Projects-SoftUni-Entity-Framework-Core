namespace Problem7
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class StartUp
    {
        public const string ConnectionStringMinionsDB = @"Server=.;Database=MinionsDB;Integrated Security=True";
        public static void Main(string[] args)
        {
            string selectSQL = @"SELECT Name FROM Minions";
            List<string> names = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConnectionStringMinionsDB))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectSQL, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            names.Add((string)reader[0]);
                        }
                    }
                }
            }
            for (int i = 0; i < names.Count/2; i++)
            {
                Console.WriteLine(names[i]);
                Console.WriteLine(names[names.Count - 1 - i]);
            }
            if (names.Count % 2 != 0)
            {
                Console.WriteLine(names[names.Count / 2]);
            }
        }
    }
}