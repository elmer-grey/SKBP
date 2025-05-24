using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Full_modul.AppLogger;

namespace Full_modul
{
    public class BaseWindow : Window
    {
        private readonly DispatcherTimer _connectionCheckTimer;
        private Queue<(string, Brush, bool)> _notificationQueue = new();
        private bool _isNotificationActive;
        private bool _isCheckingConnection;
        public bool _wasDisconnected;
        private bool _isFirstConnection = true;
        private DateTime _lastDisconnectTime;
        private int _disconnectCount;
        protected bool _connectionInitialized = false;
        protected CancellationTokenSource _connectionCheckCts;
        private bool _isWindowEnabled = true;
        public bool IsConnected { get; set; }

        public BaseWindow()
        {
            if (!DatabaseConnection.IsInitialized)
            {
                DatabaseConnection.Instance.InitializeAsync();
            }
            _connectionCheckTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _connectionCheckTimer.Tick += async (s, e) => await CheckConnectionAsync();

            this.Loaded += OnWindowLoaded;
            this.Closed += (s, e) => _connectionCheckTimer.Stop();
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            await InitializeConnectionElementsAsync();
            _connectionCheckTimer.Start();
        }

        protected virtual async Task InitializeConnectionElementsAsync()
        {
            await CheckConnectionAsync();
        }

        public virtual void SetConnectionState(bool isConnected)
        {
            if (IsConnected != isConnected)
            {
                IsConnected = isConnected;
                Dispatcher.InvokeAsync(async () =>
                {
                    await UpdateFullStatus();
                    OnConnectionStateChanged(isConnected);
                });
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _connectionCheckTimer.Stop();
            _connectionCheckCts?.Cancel();
            base.OnClosed(e);
        }

        private DateTime _lastConnectionCheckTime = DateTime.MinValue;
        private const int ConnectionCheckInterval = 20; // секунд между проверками

        protected async Task<bool> CheckConnectionAsync(bool forceCheck = false)
        {
            if (!forceCheck && (DateTime.Now - _lastConnectionCheckTime).TotalSeconds < ConnectionCheckInterval)
            {
                return IsConnected;
            }

            if (_isCheckingConnection)
                return IsConnected;

            _isCheckingConnection = true;
            _connectionCheckCts?.Cancel();
            _connectionCheckCts = new CancellationTokenSource();

            try
            {
                _lastConnectionCheckTime = DateTime.Now;

                var isConnected = await Task.Run(async () =>
                {
                    try
                    {
                        if (forceCheck) ConnectionCoordinator.ResetCache();

                        var server = DatabaseConnection.GetServerNameFromConnectionString();
                        if (!await DatabaseConnection.IsServerReachable(server, 500))
                            return false;

                        return await DatabaseConnection.TestConnectionAsync(_connectionCheckCts.Token);
                    }
                    catch
                    {
                        return false;
                    }
                }, _connectionCheckCts.Token).ConfigureAwait(false);

                if (IsConnected != isConnected || forceCheck)
                {
                    if (forceCheck)
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            if (FindVisualChild<Ellipse>("ConnectionIndicator") is Ellipse indicator &&
                                FindVisualChild<TextBlock>("ConnectionStatusText") is TextBlock statusText)
                            {
                                statusText.Text = "Проверка подключения...";
                                indicator.Fill = Brushes.Gray;
                            }
                        });
                        await Task.Delay(3000);
                    }
                    IsConnected = isConnected;
                    await UpdateConnectionUI(
                        isConnected ? "Подключено" : "Нет подключения",
                        isConnected ? Brushes.LimeGreen : Brushes.Red);

                    OnConnectionStateChanged(isConnected);
                }

                return isConnected;
            }
            finally
            {
                _isCheckingConnection = false;
            }
        }

        private async Task UpdateConnectionUI(string status, Brush indicatorColor)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                if (FindVisualChild<TextBlock>("ConnectionStatusText") is TextBlock statusText &&
                    FindVisualChild<Ellipse>("ConnectionIndicator") is Ellipse indicator)
                {
                    statusText.Text = status;
                    indicator.Fill = indicatorColor;
                }
            }, DispatcherPriority.Background);
        }

        protected TextBlock UserStatusTextBlock => FindVisualChild<TextBlock>("user");

        protected virtual string GetOfflineStatus()
        {
            return UserInfo.username == "admin"
                ? "Администратор (оффлайн режим)"
                : "Оффлайн режим";
        }

        protected virtual async Task LoadUserDataAsync()
        {
            string query = @"SELECT REPLACE(LTRIM(RTRIM(COALESCE(lastname_hr, '') + ' ' + 
        COALESCE(name_hr, '') + ' ' + COALESCE(midname_hr, ''))), '  ', ' ') 
        AS FullName FROM [calculator].[dbo].[hr] WHERE login_hr = @login";

            try
            {
                if (!IsConnected)
                {
                    await UpdateFullStatus();
                    return;
                }

                string fullName = await Task.Run(() =>
                    DatabaseConnection.Instance.ExecuteScalar<string>(
                        query,
                        new SqlParameter("@login", UserInfo.username)));

                UpdateUserStatus(!string.IsNullOrEmpty(fullName) ? fullName.Trim() : "Администратор");
            }
            catch
            {
                UpdateUserStatus("Не удалось загрузить данные");
            }
        }

        protected void UpdateUserStatus(string status)
        {
            if (Dispatcher.CheckAccess())
            {
                if (UserStatusTextBlock != null)
                {
                    UserStatusTextBlock.Text = status;
                }
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(() => UpdateUserStatus(status)));
            }
        }

        protected async Task UpdateFullStatus()
        {
            await Dispatcher.InvokeAsync(async () =>
            {

                await UpdateConnectionUI(
                        IsConnected ? "Подключено" : "Нет подключения",
                        IsConnected ? Brushes.LimeGreen : Brushes.Red);
                if (IsConnected)
                {
                    await LoadUserDataAsync();
                }
                else
                {
                    UpdateUserStatus(GetOfflineStatus());
                }
            });
        }

        private string _lastDisconnectReason;
        private static readonly SemaphoreSlim _notificationLock = new SemaphoreSlim(1, 1);

        private class ReleaseWrapper : IDisposable
        {
            private readonly SemaphoreSlim _semaphore;
            public ReleaseWrapper(SemaphoreSlim semaphore) => _semaphore = semaphore;
            public void Dispose() => _semaphore.Release();
        }

        protected virtual async void OnConnectionStateChanged(bool isConnected)
        {
            if (DateTime.Now - App.StartTime < TimeSpan.FromSeconds(2))
                return;

            await Dispatcher.InvokeAsync(async () =>
            {
                await UpdateFullStatus();

                if (isConnected)
                {
                    if (_wasDisconnected)
                    {
                        var downtime = DateTime.Now - _lastDisconnectTime;
                        string disconnectReason = _lastDisconnectReason ?? "причина неизвестна";
                        AppLogger.LogDbConnectionEvent(
                            $"Подключение восстановлено после {downtime.TotalSeconds:F1} сек. Причина: {disconnectReason}",
                            ConnectionEventType.Reconnected);

                        await ShowNotification("Подключение восстановлено", Brushes.Green);
                        _wasDisconnected = false;
                        _lastDisconnectReason = null;
                        await RefreshData();
                    }
                }
                else if (!isConnected && !_wasDisconnected)
                {
                    await ShowNotification("Нет подключения к БД", Brushes.Red, true);
                    _lastDisconnectTime = DateTime.Now;
                    _disconnectCount++;
                    _lastDisconnectReason = await GetConnectionFailureReason().ConfigureAwait(false);
                    AppLogger.LogDbWarning($"Потеряно подключение (#{_disconnectCount}). Причина: {_lastDisconnectReason}");
                    _wasDisconnected = true;
                }
            });
        }
                
        protected virtual Task RefreshData()
        {
            return Task.CompletedTask;
        }

        private async Task<string> GetConnectionFailureReason()
        {
            try
            {
                string server = DatabaseConnection.GetServerNameFromConnectionString();

                bool isServerReachable = await DatabaseConnection.IsServerReachable(server, 500)
                    .ConfigureAwait(false);

                if (!isServerReachable)
                {
                    return "сервер не отвечает";
                }

                bool testResult = await DatabaseConnection.TestConnectionAsync(_connectionCheckCts.Token)
                    .ConfigureAwait(false);

                if (testResult)
                {
                    return "временный сбой (соединение восстановилось)";
                }

                return "ошибка аутентификации или доступа к БД";
            }
            catch (SqlException sqlEx)
            {
                return sqlEx.Number switch
                {
                    -2 => "таймаут подключения",
                    53 => "сервер SQL недоступен",
                    18456 => "ошибка аутентификации",
                    4060 => "неверное имя базы данных",
                    _ => $"ошибка SQL (код {sqlEx.Number})"
                };
            }
            catch (OperationCanceledException)
            {
                return "проверка подключения отменена";
            }
            catch (Exception ex)
            {
                return $"ошибка: {ex.GetType().Name}";
            }
        }

        public async Task ShowNotification(string message, Brush color, bool isCritical = false)
        {
            await (isCritical ? Application.Current.Dispatcher : Dispatcher).InvokeAsync(() =>
            {
                try
                {
                    NotificationWindow notification;

                    if (isCritical)
                    {
                        var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        notification = new NotificationWindow(message, color, isCritical ? 5 : 3)
                        {
                            WindowStartupLocation = mainWindow != null ?
                                WindowStartupLocation.CenterOwner :
                                WindowStartupLocation.CenterScreen,
                            Owner = mainWindow
                        };
                        mainWindow?.Activate();
                    }
                    else if (!IsLoaded || !IsVisible || IsClosing())
                    {
                        notification = new NotificationWindow(message, color, isCritical ? 5 : 3);
                    }
                    else
                    {
                        notification = new NotificationWindow(message, color, isCritical ? 5 : 3)
                        {
                            Owner = this
                        };
                    }

                    notification.PreviewKeyDown += (s, e) =>
                    {
                        if (e.Key == Key.Escape)
                        {
                            e.Handled = true;
                            var window = s as NotificationWindow;
                            window?.CloseWithAnimation();
                        }
                    };

                    notification.Show();
                }
                catch (Exception ex)
                {
                    var fallbackNotification = new NotificationWindow(message, color, isCritical ? 5 : 3);
                    fallbackNotification.Show();
                    AppLogger.LogError($"Ошибка показа уведомления: {ex.Message}");
                }
            });
        }

        protected async Task EnqueueNotification(string message, Brush color, bool isCritical = false)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                _notificationQueue.Enqueue((message, color, isCritical));
                ProcessNotificationQueue();
            });
        }

        private async void ProcessNotificationQueue()
        {
            if (_isNotificationActive || _notificationQueue.Count == 0)
                return;

            _isNotificationActive = true;
            var (message, color, isCritical) = _notificationQueue.Dequeue();

            await ShowNotification(message, color, isCritical);
            await Task.Delay(isCritical ? 5000 : 3000);

            _isNotificationActive = false;
            ProcessNotificationQueue();
        }

        private DateTime _lastUiUpdateTime = DateTime.MinValue;
        protected async Task UpdateConnectionUI()
        {
            if ((DateTime.Now - _lastUiUpdateTime).TotalMilliseconds < 500)
                return;

            await Dispatcher.InvokeAsync(() =>
            {
                if (FindVisualChild<Ellipse>("ConnectionIndicator") is Ellipse indicator &&
                    FindVisualChild<TextBlock>("ConnectionStatusText") is TextBlock statusText)
                {
                    statusText.Text = IsConnected ? "Подключено" : "Нет подключения";
                    indicator.Fill = IsConnected ? Brushes.LimeGreen : Brushes.Red;
                    _lastUiUpdateTime = DateTime.Now;
                }
            });
        }

        protected bool EnsureDatabaseConnection(bool silent = false)
        {
            if (IsConnected) return true;
            if (silent) return false;

            var result = MessageBox.Show(
                "Нет подключения к БД. Продолжить выполнение с ограниченным функционалом?",
                "Ошибка подключения",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                AppLogger.LogWarning("Пользователь продолжил работу без подключения к БД");
                return true;
            }
            return false;
        }

        protected T FindVisualChild<T>(string name) where T : DependencyObject
        {
            if (string.IsNullOrEmpty(name) || this == null)
                return null;

            if (this is T rootElement && this is FrameworkElement rootFe && rootFe.Name == name)
                return rootElement;

            return Application.Current.Dispatcher.Invoke(() => FindVisualChildRecursive<T>(this, name));
        }


        private static T FindVisualChildRecursive<T>(DependencyObject parent, string name)
            where T : DependencyObject
        {
            if (parent == null)
                return null;

            try
            {
                if (!(parent is Visual || parent is Visual3D))
                    return null;

                int childrenCount = Application.Current.Dispatcher.Invoke(() => VisualTreeHelper.GetChildrenCount(parent));
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = Application.Current.Dispatcher.Invoke(() => VisualTreeHelper.GetChild(parent, i));
                    if (child == null)
                        continue;

                    if (child is T result &&
                        child is FrameworkElement fe &&
                        fe.Name == name)
                    {
                        return result;
                    }

                    var childResult = FindVisualChildRecursive<T>(child, name);
                    if (childResult != null)
                        return childResult;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка поиска визуального элемента: {ex.Message}");
            }

            return null;
        }

        private bool IsClosing()
        {
            try
            {
                var fields = typeof(Window).GetField("_isClosing",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);
                return fields?.GetValue(this) as bool? ?? false;
            }
            catch
            {
                return false;
            }
        }
    }
}