using Microsoft.Data.SqlClient;

namespace RentalMobil.Data
{
    public class DatabaseConnection // for Database Connection
    {
        private readonly string _db;

        public DatabaseConnection(string db)
        {
            _db = db;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_db);
        }
    }
}
