namespace Problem6
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public const string ConnectionStringMinionsDB = @"Server=.;Database=MinionsDB;Integrated Security=True";
        public static void Main(string[] args)
        {
            string selectName = @"SELECT Name FROM Villains WHERE Id = @villainId";
            string deleteFromMinions = @"DELETE FROM MinionsVillains 
                                         WHERE VillainId = @villainId";
            string deleteFromVillains = @"DELETE FROM Villains
                                          WHERE Id = @villainId";
            int villainId = int.Parse(Console.ReadLine());
            using (SqlConnection connection = new SqlConnection(ConnectionStringMinionsDB))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = selectName;
                    command.Parameters.AddWithValue("@villainId", villainId);
                    string villainName = (string)command.ExecuteScalar();

                    if (villainName == null)
                    {
                        throw new NullReferenceException("No such villain was found.");
                    }
                    command.CommandText = deleteFromMinions;
                    int deletedMinions = command.ExecuteNonQuery();
                    command.CommandText = deleteFromVillains;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine($"{villainName} was deleted.");
                    Console.WriteLine($"{deletedMinions} minions were released.");
                }
                catch (NullReferenceException nre)
                {
                    Console.WriteLine(nre.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"Rollback Exception Type: {ex2.GetType()}");
                        Console.WriteLine($"  Message: {ex2.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Commit Exception Type: {ex.GetType()}");
                    Console.WriteLine($"  Message: {ex.Message}");
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"Rollback Exception Type: {ex2.GetType()}");
                        Console.WriteLine($"  Message: {ex2.Message}");
                    }
                }
                finally
                {
                    command.Dispose();
                }
            }
        }
    }
}