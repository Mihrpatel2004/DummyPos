using Microsoft.Data.SqlClient;

namespace DummyPos.Data
{
    public class DbHelper
    {
        private readonly string _connectionString;

        public DbHelper(IConfiguration c)
        {
            _connectionString = c.GetConnectionString("DbConnection");
        }

        public SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            return connection;
        }
    }

    
}
