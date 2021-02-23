namespace Problem8
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class StartUp
    {
        public const string ConnectionStringMinionsDB = @"Server=.;Database=MinionsDB;Integrated Security=True";
        public static void Main(string[] args)
        {
            string updateMinions = @"UPDATE Minions
                                     SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                     WHERE Id = @Id";
            string selectName = @"SELECT Name, Age FROM Minions";

            int[] ids = Console.ReadLine().Split().Select(int.Parse).ToArray();

            using (SqlConnection connection = new SqlConnection(ConnectionStringMinionsDB))
            {
                connection.Open();
                for (int i = 0; i < ids.Length; i++)
                {
                    using (SqlCommand command = new SqlCommand(updateMinions, connection))
                    {
                        command.Parameters.AddWithValue("@Id", ids[i]);
                        command.ExecuteNonQuery();
                    }
                }
                using (SqlCommand command = new SqlCommand(selectName, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = (string) reader[0];
                            int age = (int) reader[1];

                            Console.WriteLine($"{name} {age}");
                        }
                    }
                }
            }
        }
    }
}