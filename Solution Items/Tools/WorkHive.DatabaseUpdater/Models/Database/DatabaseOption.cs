using Dapper;
using ProjectPlanner.DatabaseUpdater.Services;

namespace ProjectPlanner.DatabaseUpdater.Models.Database
{
    public class DatabaseOption
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public static DatabaseOption GetDatabaseOption(string name, string defaultValue)
        {
            CreateTableIfNotExists();

            return DatabaseService.Connection.QueryFirst<DatabaseOption>(
                @$"
                    IF NOT EXISTS (SELECT 1 FROM [Option] WHERE [Name] = '{name}')
                    BEGIN
	                    INSERT INTO [Option] ([Name], [Value]) VALUES ('{name}', '{defaultValue}')
                    END

                    SELECT [Value] FROM [Option] WHERE [Name] = '{name}'
                "
            );
        }

        private static void CreateTableIfNotExists()
        {
            DatabaseService.Connection.Execute(
                @"
                    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Option')
                    BEGIN
	                    CREATE TABLE [Option] (
		                    [Name] NVARCHAR(50) NOT NULL,
		                    [Value] NVARCHAR(MAX) NOT NULL
	                    )
                    END
                "
            );
        }
    }
}
