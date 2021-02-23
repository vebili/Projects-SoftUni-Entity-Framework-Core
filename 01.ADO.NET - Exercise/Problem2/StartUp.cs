namespace Problem2
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public const string ConnectionStringMinionsDB = @"Server=.;Database=MinionsDB;Integrated Security=True";
        public static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringMinionsDB))
            {
                connection.Open();
                string selectString = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                        FROM Villains AS v 
                                        JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                        GROUP BY v.Id, v.Name 
                                        HAVING COUNT(mv.VillainId) > 3 
                                        ORDER BY COUNT(mv.VillainId)";
                using (SqlCommand command = new SqlCommand(selectString, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                string name = (string)reader[0];
                                int age = (int)reader[1];

                                Console.WriteLine($"{name} - {age}");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }
    }
}