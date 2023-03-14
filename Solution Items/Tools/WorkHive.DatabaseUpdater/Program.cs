using Dapper;
using ProjectPlanner.DatabaseUpdater.Models.Database;
using ProjectPlanner.DatabaseUpdater.Services;
using System.Data.SqlClient;

namespace ProjectPlanner.DatabaseUpdater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ArgumentService.Initialize(args);

                WriteInfo($"UPDATING DATABASE {ArgumentService.Instance.Database}");

                var currentVersion = DatabaseVersion.GetDatabaseVersion();
                var latestVersion = new DatabaseVersion(currentVersion.Version);

                WriteInfo($"CURRENT VERSION: {currentVersion}");

                var files = Directory.GetFiles(ArgumentService.Instance.ScriptPath, "*.sql");
                var databaseScripts = files
                    .Select(f => new DatabaseScript(f))
                    .Where(s => s.Version.CompareTo(currentVersion) == 1)
                    .ToList();

                if (databaseScripts.Count == 0)
                {
	                WriteInfo($"DATABASE {ArgumentService.Instance.Database} NOT UPDATED - NO NEW SCRIPTS TO RUN");
                }

                foreach (var databaseScript in databaseScripts)
                {
                    WriteInfo($"RUNNING SCRIPT - {databaseScript.FileName}");

                    latestVersion = databaseScript.Version;
                    var scriptContent = File.ReadAllText(databaseScript.FileName);
                    DatabaseService.Connection.Execute(scriptContent);

                    DatabaseVersion.UpdateDatabaseVersion(databaseScript.Version);
                }

                WriteInfo($"UPDATED DATABASE {ArgumentService.Instance.Database} TO VERSION {latestVersion}");
            }
            catch (Exception ex)
            {
                WriteInfo("UNABLE TO UPDATE DATABASE VERSION: " + ex.Message);
            }
        }

        private static void WriteInfo(string info)
        {
            Console.WriteLine("");
            Console.WriteLine(info);
        }
    }
}