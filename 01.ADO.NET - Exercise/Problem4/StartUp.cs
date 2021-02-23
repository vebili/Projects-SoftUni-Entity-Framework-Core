namespace Problem4
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public const string ConnectionStringMinionsDB = @"Server=.;Database=MinionsDB;Integrated Security=True";
        public static void Main(string[] args)
        {
            string[] minionsData = Console.ReadLine().Split();
            string[] villainData = Console.ReadLine().Split();

            string minionName = minionsData[1];
            int age = int.Parse(minionsData[2]);
            string townName = minionsData[3];

            string villainName = villainData[1];

            using (SqlConnection connection = new SqlConnection(ConnectionStringMinionsDB))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    int? townId = GetTownByName(connection, townName, transaction);
                    if (townId == null)
                    {
                        AddTown(connection, townName, transaction);
                    }
                    townId = GetTownByName(connection, townName, transaction);


                    AddMinion(connection, minionName, age, townId, transaction);


                    int? villainId = GetVillainByName(connection, villainName, transaction);
                    if (villainId == null)
                    {
                        AddVillain(connection, villainName, transaction);
                    }
                    villainId = GetVillainByName(connection, villainName, transaction);

                    int minionId = GetMinionByName(connection, minionName, transaction);

                    AddMinionVillain(connection, villainId, minionId, villainName, minionName, transaction);

                    transaction.Commit();

                    Console.WriteLine();
                    Console.WriteLine("Problem 4.Add Minion Completed!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Exception Type Returned From Problem 4: {ex.GetType()}");
                    Console.WriteLine($"  Message For Problem 4 Exception: {ex.Message}");

                   try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Rollback Exception Type For Problem 4: {ex2.GetType()}");
                        Console.WriteLine($"  Message For Problem 4 Rollback Exception: {ex2.Message}");
                    }
                }
            }
        }

        private static void AddMinionVillain(SqlConnection connection, int? villainId, int minionId, string villainName, string minionName, SqlTransaction transaction)
        {
            string insertMinionVillain = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

            using (SqlCommand command = new SqlCommand(insertMinionVillain, connection, transaction))
            {
                command.Parameters.AddWithValue("@villainId", villainId);
                command.Parameters.AddWithValue("@minionId", minionId);

                command.ExecuteNonQuery();
            }

            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
        }

        private static int GetMinionByName(SqlConnection connection, string minionName, SqlTransaction transaction)
        {
            string selectMinionId = @"SELECT Id FROM Minions WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(selectMinionId, connection, transaction))
            {
                command.Parameters.AddWithValue("@Name", minionName);
                return (int)command.ExecuteScalar();
            }
        }

        private static void AddVillain(SqlConnection connection, string villainName, SqlTransaction transaction)
        {
            string insertVillain = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

            using (SqlCommand command = new SqlCommand(insertVillain, connection, transaction))
            {
                command.Parameters.AddWithValue("@villainName", villainName);

                command.ExecuteNonQuery();
            }

            Console.WriteLine($"Villain {villainName} was added to the database.");
        }

        private static int? GetVillainByName(SqlConnection connection, string villainName, SqlTransaction transaction)
        {
            string selectVillainId = @"SELECT Id FROM Villains WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(selectVillainId, connection, transaction))
            {
                command.Parameters.AddWithValue("@Name", villainName);
                return (int?)command.ExecuteScalar();
            }
        }

        private static void AddMinion(SqlConnection connection, string minionName, int age, int? townId, SqlTransaction transaction)
        {
            string insertMinion = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

            using (SqlCommand command = new SqlCommand(insertMinion, connection, transaction))
            {
                command.Parameters.AddWithValue("@name", minionName);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@townId", townId);

                command.ExecuteNonQuery();
            }
        }

        private static int? GetTownByName(SqlConnection connection, string townName, SqlTransaction transaction)
        {
            string selectTownId = @"SELECT Id FROM Towns WHERE Name = @townName";

            using (SqlCommand command = new SqlCommand(selectTownId, connection, transaction))
            {
                
                command.Parameters.AddWithValue("@townName", townName);
                return (int?)command.ExecuteScalar();
            }
        }

        private static void AddTown(SqlConnection connection, string townName, SqlTransaction transaction)
        {
            string insertTown = @"INSERT INTO Towns (Name) VALUES (@townName)";
            using (SqlCommand command = new SqlCommand(insertTown, connection, transaction))
            {
                command.Parameters.AddWithValue("@townName", townName);

                command.ExecuteNonQuery();
            }

            Console.WriteLine($"Town {townName} was added to the database.");
        }
    }
}
