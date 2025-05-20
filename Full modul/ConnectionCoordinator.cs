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
        private static bool _isReconnecting;
        private static DateTime _lastAttemptTime = DateTime.MinValue;
        private static readonly TimeSpan MinRetryInterval = TimeSpan.FromSeconds(2);

        public static async Task<bool> SafeCheckConnectionAsync(Func<Task<bool>> checkAction)
        {
            if (!await _connectionLock.WaitAsync(0).ConfigureAwait(false))
                return DatabaseConnection.TestConnectionCachedState();

            try
            {
                var timeSinceLastAttempt = DateTime.Now - _lastAttemptTime;
                if (timeSinceLastAttempt < MinRetryInterval)
                {
                    await Task.Delay(MinRetryInterval - timeSinceLastAttempt);
                }

                _isReconnecting = true;
                _lastAttemptTime = DateTime.Now;

                return await checkAction().ConfigureAwait(false);
            }
            finally
            {
                _isReconnecting = false;
                _connectionLock.Release();
            }
        }
    }
}
