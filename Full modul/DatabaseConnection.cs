using Full_modul.Properties;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Full_modul
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private static readonly object _lock = new object();
        private SqlConnection _connection;

        private DatabaseConnection()
        {
            LoadConnectionString();
        }

        private void LoadConnectionString()
        {
            try
            {
                string encrypted = Settings.Default.ConnectionString;
                string connectionString;

                if (string.IsNullOrEmpty(encrypted))
                {
                    connectionString = "Data Source=DESKTOP-TBLQV8A;" +
                                     "Initial Catalog=calculator;" +
                                     "User ID=sqlserver;" +
                                     "Password=sqlserver;" +
                                     "TrustServerCertificate=True";
                }
                else
                {
                    connectionString = SecurityHelper.Decrypt(encrypted);
                }
                _connection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки подключения: {ex.Message}");
                throw;
            }
        }

        public static void UpdateConnection(string newRawConnectionString)
        {
            try
            {
                string encrypted = SecurityHelper.Encrypt(newRawConnectionString);
                Settings.Default.ConnectionString = encrypted;
                Settings.Default.Save();

                lock (_lock)
                {
                    _instance?.CloseConnection();
                    _instance = new DatabaseConnection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления подключения: {ex.Message}");
                throw;
            }
        }

        public SqlDataReader ExecuteReader(string query, params SqlParameter[] parameters)
        {
            string connectionString = SecurityHelper.Decrypt(Settings.Default.ConnectionString);
            var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddRange(parameters);
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static DatabaseConnection Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new DatabaseConnection();
                }
            }
        }

        public SqlConnection Connection
        {
            get
            {
                if (_connection.State == System.Data.ConnectionState.Closed)
                    _connection.Open();
                return _connection;
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
                _connection.Close();
        }

        public T ExecuteScalar<T>(string query, params SqlParameter[] parameters)
        {
            lock (_lock)
            {
                try
                {
                    using (var command = new SqlCommand(query, Connection))
                    {
                        command.Parameters.AddRange(parameters);
                        var result = command.ExecuteScalar();
                        return result == DBNull.Value ? default : (T)result;
                    }
                }
                catch (Exception ex)
                {
                    // Логирование ошибки
                    throw new Exception($"Ошибка выполнения запроса: {ex.Message}", ex);
                }
            }
        }

        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            lock (_lock)
            {
                try
                {
                    using (var command = new SqlCommand(query, Connection))
                    {
                        command.Parameters.AddRange(parameters);
                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка выполнения команды: {ex.Message}", ex);
                }
            }
        }
    }
}
