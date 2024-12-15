using Full_modul.Properties;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Full_modul
{
    class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private static readonly object _lock = new object();
        private SqlConnection _connection;

        private DatabaseConnection()
        {
            string connectionString = Settings.Default.ConnectionString;
            _connection = new SqlConnection(connectionString);
        }

        public static DatabaseConnection Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseConnection();
                    }
                    return _instance;
                }
            }
        }

        public SqlConnection Connection
        {
            get
            {
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
