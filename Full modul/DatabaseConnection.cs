using Full_modul.Properties;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Full_modul
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private static readonly object _lock = new();
        private SqlConnection _connection;
        private DateTime _lastSuccessfulConnection;
        private int _failedAttempts;

        private DatabaseConnection()
        {
            LoadConnectionString();
        }

        public static bool IsInitialized { get; private set; }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            if (IsInitialized) return;

            try
            {
                IsInitialized = await TestConnectionAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                AppLogger.LogWarning("Database initialization was canceled");
                throw;
            }
            catch (Exception ex)
            {
                AppLogger.LogError($"Database initialization failed: {ex.Message}");
                throw;
            }
        }

        private void LoadConnectionString()
        {
            try
            {
                string connectionString = SecurityHelper.Decrypt(Settings.Default.ConnectionString);

                _connection = new SqlConnection(connectionString);
                AppLogger.LogDbInfo("Инициализировано новое подключение к БД");
            }
            catch (Exception ex)
            {
                _failedAttempts++;
                AppLogger.LogError($"Ошибка загрузки подключения (попытка {_failedAttempts}): {ex.Message}");
                if (_failedAttempts >= 3)
                {
                    MessageBox.Show($"Критическая ошибка подключения: {ex.Message}");
                    throw;
                }
                Thread.Sleep(1000);
                LoadConnectionString();
            }
        }

        private static string _lastConnectionString;
        private static DateTime _lastConnectionChangeTime = DateTime.MinValue;

        public static void UpdateConnection(string newRawConnectionString)
        {
            try
            {
                string encrypted = SecurityHelper.Encrypt(newRawConnectionString);

                if (Settings.Default.ConnectionString == encrypted &&
                    (DateTime.Now - _lastConnectionChangeTime).TotalMinutes < 5)
                {
                    return;
                }

                lock (_lock)
                {
                    Settings.Default.ConnectionString = encrypted;
                    Settings.Default.Save();
                    _lastConnectionString = newRawConnectionString;
                    _lastConnectionChangeTime = DateTime.Now;

                    _instance?.CloseConnection();
                    _instance = new DatabaseConnection();
                    AppLogger.LogDbInfo("Обновлены параметры подключения");
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError($"Ошибка обновления подключения: {ex.Message}");
                throw;
            }
        }

        private const int DefaultTimeout = 5;

        public static async Task<bool> TestConnectionAsync(CancellationToken cancellationToken)
        {
            try
            {
                await using var connection = new SqlConnection(GetDecryptedConnectionString());

                var openTask = connection.OpenAsync(cancellationToken);

                if (await Task.WhenAny(openTask, Task.Delay(2000, cancellationToken)) == openTask)
                {
                    return openTask.Status == TaskStatus.RanToCompletion;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static string GetDecryptedConnectionString()
        {
            try
            {
                return SecurityHelper.Decrypt(Settings.Default.ConnectionString);
            }
            catch
            {
                return string.Empty;
            }
        }

        public SqlDataReader ExecuteReader(string query, params SqlParameter[] parameters)
        {
            string connectionString = GetDecryptedConnectionString();
            var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(query, connection)
            {
                CommandTimeout = DefaultTimeout
            };
            command.Parameters.AddRange(parameters);
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public async Task<T> ExecuteScalarAsync<T>(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = new SqlConnection(GetDecryptedConnectionString()))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);
                    await connection.OpenAsync();
                    var result = await command.ExecuteScalarAsync();
                    return result == DBNull.Value ? default : (T)result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка выполнения асинхронного запроса: {ex.Message}", ex);
            }
        }

        public static async Task<bool> IsServerReachable(string host, int timeout)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(host, timeout);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        public static string GetServerNameFromConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(GetDecryptedConnectionString());
            return builder.DataSource.Split(',')[0];
        }

        public T ExecuteScalar<T>(string query, params SqlParameter[] parameters)
        {
            lock (_lock)
            {
                try
                {
                    using var cmd = new SqlCommand(query, Connection);
                    cmd.Parameters.AddRange(parameters);

                    var result = cmd.ExecuteScalar();
                    return result == DBNull.Value ? default : (T)result;
                }
                catch (SqlException sqlEx)
                {
                    string userMessage = sqlEx.Number switch
                    {
                        208 => "Ошибка: запрашиваемая таблица не существует",
                        547 => "Ошибка ограничения внешнего ключа",
                        2627 => "Нарушение уникальности данных",
                        _ => $"Ошибка базы данных: {sqlEx.Message}"
                    };

                    AppLogger.LogDbError($"SQL ошибка (ExecuteScalar): {sqlEx.Message}\nQuery: {query}");
                    ShowNotification(userMessage, Brushes.Red, 5);
                    return default;
                }
                catch (Exception ex)
                {
                    AppLogger.LogDbError($"Общая ошибка (ExecuteScalar): {ex.Message}\nQuery: {query}");
                    ShowNotification("Произошла непредвиденная ошибка при выполнении запроса", Brushes.Red, 5);
                    return default;
                }
            }
        }
        private static void ShowNotification(string message, Brush color, int durationSeconds = 3)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                try
                {
                    var notification = new NotificationWindow(message, color, durationSeconds);

                    notification.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    notification.Show();
                }
                catch (Exception ex)
                {
                    // Фолбэк в консоль если всё совсем сломалось
                    AppLogger.LogError($"Ошибка показа уведомления: {ex.Message}");
                }
            });
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
                catch (SqlException sqlEx)
                {
                    string userMessage = sqlEx.Number switch
                    {
                        208 => "Ошибка: запрашиваемая таблица не существует",
                        547 => "Ошибка: нарушение ограничений данных",
                        2627 => "Ошибка: дублирование уникальных данных",
                        _ => $"Ошибка базы данных: {sqlEx.Message}"
                    };

                    AppLogger.LogDbError($"SQL ошибка (ExecuteNonQuery): {sqlEx.Message}\nQuery: {query}");
                    ShowNotification(userMessage, Brushes.Red, 5);
                    return -1;
                }
                catch (Exception ex)
                {
                    AppLogger.LogDbError($"Общая ошибка (ExecuteNonQuery): {ex.Message}\nQuery: {query}");
                    ShowNotification("Произошла непредвиденная ошибка при выполнении команды", Brushes.Red, 5);
                    return -1;
                }
            }
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
    }
}