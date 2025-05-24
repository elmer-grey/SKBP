using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Full_modul
{
    public static class ConnectionCoordinator
    {
        private static readonly SemaphoreSlim _connectionLock = new(1, 1);
        private static DateTime _lastAttemptTime = DateTime.MinValue;
        private static readonly TimeSpan MinRetryInterval = TimeSpan.FromSeconds(2);
        private static bool _lastConnectionState;
        private static DateTime _lastCheckTime;
        private static DateTime _lastErrorLogTime = DateTime.MinValue;
        private const int ErrorLogInterval = 20;

        public static async Task<bool> GetConnectionStateAsync(CancellationToken cancellationToken = default)
        {
            await _connectionLock.WaitAsync();
            try
            {
                var timeSinceLastAttempt = DateTime.Now - _lastAttemptTime;
                if (timeSinceLastAttempt < MinRetryInterval)
                {
                    await Task.Delay(MinRetryInterval - timeSinceLastAttempt, cancellationToken);
                }

                _lastAttemptTime = DateTime.Now;
                try
                {
                    if ((DateTime.Now - _lastCheckTime).TotalSeconds < 5)
                        return _lastConnectionState;

                    _lastConnectionState = await DatabaseConnection.TestConnectionAsync(cancellationToken);
                    _lastCheckTime = DateTime.Now;
                    return _lastConnectionState;
                }
                catch (OperationCanceledException)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (DateTime.Now - _lastErrorLogTime > TimeSpan.FromSeconds(ErrorLogInterval))
                {
                    AppLogger.LogDbError($"Ошибка подключения: {ex.Message}");
                    _lastErrorLogTime = DateTime.Now;
                }
                return false;
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        public static void SetConnectionState(bool state, bool force = false)
        {
            if (force || (DateTime.Now - _lastCheckTime).TotalSeconds >= 5)
            {
                _lastConnectionState = state;
                _lastCheckTime = DateTime.Now;
            }
        }

        public static void ResetCache()
        {
            _lastConnectionState = false;
            _lastCheckTime = DateTime.MinValue;
            _lastAttemptTime = DateTime.MinValue;
        }
    }
}
