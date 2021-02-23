namespace Problem3
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public const string ConnectionStringMinionsDB = @"Server=.;Database=MinionsDB;Integrated Security=True";
        public static void Main(string[] args)
        {
            int villianId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(ConnectionStringMinionsDB))
            {
                connection.Open();
                string selectVillainName = @"SELECT Name FROM Villains WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(selectVillainName, connection))
                {
                    command.Parameters.AddWithValue("@Id", villianId);

                    string villianName = (string)command.ExecuteScalar();

                    if (villianName == null)
                    {
                        Console.WriteLine($"No villain with ID {villianId} exists in the database.");
                        return;
                    }
                    Console.WriteLine($"Villain: {villianName}");
                }
                string selectMinions = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                                    m.Name, 
                                                    m.Age
                                            FROM MinionsVillains AS mv
                                            JOIN Minions As m ON mv.MinionId = m.Id
                                            WHERE mv.VillainId = @Id
                                            ORDER BY m.Name";

                using (SqlCommand command = new SqlCommand(selectMinions, connection))
                {
                    command.Parameters.AddWithValue("@Id", villianId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("(no minions)");
                        }

                        while (reader.Read())
                        {
                            long rowNum = (long)reader[0];
                            string name = (string)reader[1];
                            int age = (int)reader[2];

                            Console.WriteLine($"{rowNum}. {name} {age}");
                        }
                    }
                }
            }
        }
    }
}