namespace Problem9
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public const string ConnectionStringMinionsDB = @"Server=.;Database=MinionsDB;Integrated Security=True";
        public static void Main(string[] args)
        {
            string selectNameAndAge = @"SELECT Name, Age FROM Minions WHERE Id = @Id";
            string uspGetOlderProc = @"EXEC usp_GetOlder @id";
            int id = int.Parse(Console.ReadLine());
            using (SqlConnection connection = new SqlConnection(ConnectionStringMinionsDB))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(uspGetOlderProc, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                using (SqlCommand command = new SqlCommand(selectNameAndAge, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = (string)reader[0];
                            int age = (int)reader[1];

                            Console.WriteLine($"{name} – {age} years old");
                        }
                    }
                }
            }
        }
    }
}