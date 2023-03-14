using Dapper;
using ProjectPlanner.DatabaseUpdater.Services;

namespace ProjectPlanner.DatabaseUpdater.Models.Database
{
    public struct DatabaseVersion : IComparable<DatabaseVersion>
    {
        public string Version;
        public int RolloutNumber;
        public int ScriptNumber;

        public DatabaseVersion(string version)
        {
            var versions = version.Split("_");

            Version = $"{versions[0]}_{versions[1]}";
            RolloutNumber = int.Parse(versions[0]);
            ScriptNumber = int.Parse(versions[1]);
        }

        public static DatabaseVersion GetDatabaseVersion()
        {
            return new DatabaseVersion(DatabaseOption.GetDatabaseOption("Version", "0000_000").Value);
        }

        public static void UpdateDatabaseVersion(DatabaseVersion version)
        {
            DatabaseService.Connection.Execute(
                $"UPDATE [Option] SET [Value] = '{version.Version}' WHERE [Name] = 'Version'"
            );
        }

        public int CompareTo(DatabaseVersion other)
        {
            var result = RolloutNumber.CompareTo(other.RolloutNumber);
            if (result == 0)
                result = ScriptNumber.CompareTo(other.ScriptNumber);
            return result;
        }

        public override string ToString()
        {
            return Version;
        }
    }
}
