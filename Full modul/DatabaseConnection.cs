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

        public void Initialize()
        {
            if (IsInitialized) return;

            LoadConnectionString();
            _ = TestConnectionAsync().ConfigureAwait(false);
            IsInitialized = true;

            AppLogger.LogDbInfo("Подключение к БД инициализировано");
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
        private static bool _lastConnectionState;
        private static DateTime _lastCheckTime = DateTime.MinValue;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(10);
        private static readonly SemaphoreSlim _connectionCheckSemaphore = new(1, 1);
        private static DateTime _lastPingTime = DateTime.MinValue;
        private static bool _lastPingResult;
        private static DateTime _lastErrorLogTime = DateTime.MinValue;
        private const int ErrorLogInterval = 30; // секунд между одинаковыми ошибками
        private static readonly SemaphoreSlim _stateCacheLock = new(1, 1);

        public static bool TestConnectionCachedState()
        {
            _stateCacheLock.Wait();
            try
            {
                return (DateTime.Now - _lastCheckTime).TotalSeconds < 5
                    ? _lastConnectionState
                    : false;
            }
            finally
            {
                _stateCacheLock.Release();
            }
        }

        public static async Task<bool> TestConnectionAsync()
        {
            await _connectionCheckSemaphore.WaitAsync();
            try
            {
                if (DateTime.Now - _lastCheckTime < CacheDuration)
                    return _lastConnectionState;

                bool currentState = false;
                try
                {
                    // Быстрая проверка доступности сервера
                    if (!await IsServerReachable(GetServerNameFromConnectionString(), 500))
                    {
                        _lastConnectionState = false;
                        return false;
                    }

                    // Проверка подключения к SQL с таймаутом
                    using var cts = new CancellationTokenSource(1500); // Уменьшен таймаут
                    await using var connection = new SqlConnection(GetDecryptedConnectionString());
                    await connection.OpenAsync(cts.Token);
                    currentState = true;
                }
                catch (OperationCanceledException)
                {
                    AppLogger.LogDbWarning("Проверка подключения отменена по таймауту");
                    currentState = false;
                }
                catch (Exception ex)
                {
                    AppLogger.LogDbError($"Ошибка подключения: {ex.Message}");
                    currentState = false;
                }

                _lastConnectionState = currentState;
                _lastCheckTime = DateTime.Now;
                return currentState;
            }
            finally
            {
                _connectionCheckSemaphore.Release();
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

        private static readonly ConcurrentDictionary<string, object> _cache = new();

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

        private static async Task<bool> IsServerReachable(string host, int timeout)
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

        private static string GetServerNameFromConnectionString()
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
                catch (Exception ex)
                {
                    AppLogger.LogDbError($"SQL ошибка (ExecuteScalar): {ex.Message}\nQuery: {query}");
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