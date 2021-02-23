namespace Problem1
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public const string ConnectionStringMaster = @"Server=.;;Integrated Security=True";
        public const string ConnectionStringMinionsDB = @"Server=.;Database=MinionsDB;Integrated Security=True";
        public static void Main(string[] args)
        {
            CreateDB_Minions();
            using (SqlConnection connection = new SqlConnection(ConnectionStringMinionsDB))
            {
                connection.Open();
                string createTables = @"CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))

                                        CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries   (Id))
                                        
                                        CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns (Id))
                                        
                                        CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))
                                        
                                        CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES               EvilnessFactors   (Id))
                                        
                                        CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))";
                ExecuteNonQuery(createTables, connection, "Creation of tables");
                string[] insertData =
                {
                    @"INSERT INTO Countries ([Name]) VALUES('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')",

                    @"INSERT INTO Towns([Name], CountryCode) VALUES('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)",

                    @"INSERT INTO Minions(Name, Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)",

                    @"INSERT INTO EvilnessFactors(Name) VALUES('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')",

                    @"INSERT INTO Villains(Name, EvilnessFactorId) VALUES('Gru', 2),('Victor', 1),('Jilly', 3),('Miro', 4),('Rosen', 5),('Dimityr', 1),('Dobromir', 2)",

                    @"INSERT INTO MinionsVillains(MinionId, VillainId) VALUES(4, 2),(1, 1),(5, 7),(3, 5),(2, 6),(11, 5),(8, 4),(9, 7),(7, 1),(1, 3),(7, 3),(5, 3),(4, 3),(1, 2),(2, 1),(2, 7)"
                };
                foreach (var insertCommand in insertData)
                {
                    ExecuteNonQuery(insertCommand, connection, "Insert");
                }
            }
        }
        private static void ExecuteNonQuery(string commandText, SqlConnection connection, string commandType)
        {
            SqlCommand command = new SqlCommand(commandText, connection);            
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine($"{commandType} was successful!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    command.Dispose();
                }
        }
        private static void CreateDB_Minions()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringMaster))
            {
                connection.Open();
                string createDatabase = "CREATE DATABASE MinionsDB";
                SqlCommand command = new SqlCommand(createDatabase, connection);
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Database Created Successfully!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    command.Dispose();
                }
            }
        }
    }
}