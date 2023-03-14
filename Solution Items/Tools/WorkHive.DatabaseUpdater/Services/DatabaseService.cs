using System.Data.SqlClient;

namespace ProjectPlanner.DatabaseUpdater.Services
{
    public class DatabaseService
    {
        private static SqlConnection _connection;
        public static SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(ArgumentService.Instance.ConnectionString);
                }

                return _connection;
            }
        }
    }
}
