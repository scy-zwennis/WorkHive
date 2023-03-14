namespace ProjectPlanner.DatabaseUpdater.Models.Database
{
    public class DatabaseScript : IComparable<DatabaseScript>
    {
        public string FileName { get; set; }
        public DatabaseVersion Version { get; set; }

        public DatabaseScript(string fileName)
        {
            FileName = fileName;
            Version = new DatabaseVersion(Path.GetFileName(fileName));
        }

        public int CompareTo(DatabaseScript? other)
        {
            if (other == null) return 1;
            return Version.CompareTo(other.Version);
        }
    }
}
