using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private bool _wasDisconnected;
        private bool _isFirstConnection = true;
        private DateTime _lastDisconnectTime;
        private int _disconnectCount;
        protected bool _connectionInitialized = false;
        protected bool IsConnected { get; private set; }

        public BaseWindow()
        {
            if (!DatabaseConnection.IsInitialized)
            {
                DatabaseConnection.Instance.Initialize();
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
            await CheckConnectionAsync(true);
        }

        protected async Task<bool> CheckConnectionAsync(bool forceCheck = false)
        {
            if (_isCheckingConnection)
                return IsConnected;

            _isCheckingConnection = true;
            try
            {

                bool newState = await ConnectionCoordinator.SafeCheckConnectionAsync(async () =>
                {
                    try
                    {
                        bool result = await DatabaseConnection.TestConnectionAsync();
                        _connectionInitialized = true;
                        return result;
                    }
                    catch (TimeoutException)
                    {
                        AppLogger.LogDbWarning("Проверка подключения превысила таймаут");
                        return false;
                    }
                });

                if (newState != IsConnected || forceCheck)
                {
                    IsConnected = newState;
                    await UpdateConnectionUI();
                    if (_connectionInitialized)
                    {
                        OnConnectionStateChanged(IsConnected);
                    }
                }
                return IsConnected;
            }
            finally
            {
                _isCheckingConnection = false;
            }
        }

        private bool _disconnectNotificationShown;
        private static readonly object _notificationLock = new();

        protected virtual void OnConnectionStateChanged(bool isConnected)
        {
            if (DateTime.Now - App.StartTime < TimeSpan.FromSeconds(2))
                return;
            lock (_notificationLock)
            {
                if (isConnected)
                {
                    _disconnectNotificationShown = false;
                    if (_wasDisconnected)
                    {
                        var downtime = DateTime.Now - _lastDisconnectTime;
                        AppLogger.LogDbConnectionEvent(
                            $"Подключение восстановлено после {downtime.TotalSeconds:F1} сек",
                            ConnectionEventType.Reconnected);

                        if (!_isFirstConnection)
                        {
                            ShowGlobalNotification("Подключение восстановлено", Brushes.Green);
                        }
                        _wasDisconnected = false;
                    }
                }
                else if (!_wasDisconnected && !_disconnectNotificationShown)
                {
                    _disconnectNotificationShown = true;
                    _lastDisconnectTime = DateTime.Now;
                    _disconnectCount++;
                    AppLogger.LogDbWarning($"Потеряно подключение (#{_disconnectCount})");
                    _wasDisconnected = true;

                    ShowGlobalNotification("Нет подключения к БД", Brushes.Red);
                }
            }
        }

        private static bool _globalNotificationShown;
        private static void ShowGlobalNotification(string message, Brush color)
        {
            if (_globalNotificationShown) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    var mainWindow = Application.Current.MainWindow;
                    var notification = new NotificationWindow(message, color, 5)
                    {
                        Owner = mainWindow,
                        WindowStartupLocation = mainWindow != null
                            ? WindowStartupLocation.CenterOwner
                            : WindowStartupLocation.CenterScreen
                    };

                    notification.Closed += (s, e) => _globalNotificationShown = false;
                    _globalNotificationShown = true;
                    notification.Show();
                }
                catch (Exception ex)
                {
                    AppLogger.LogError($"Ошибка показа глобального уведомления: {ex.Message}");
                }
            });
        }

        protected async Task ShowNotification(string message, Brush color, bool isCritical = false)
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
            // Ограничиваем частоту обновления UI
            if ((DateTime.Now - _lastUiUpdateTime).TotalMilliseconds < 500)
                return;

            await Dispatcher.InvokeAsync(() =>
            {
                if (FindVisualChild<Ellipse>("ConnectionIndicator") is Ellipse indicator &&
                    FindVisualChild<TextBlock>("ConnectionStatusText") is TextBlock statusText)
                {
                    statusText.Text = IsConnected ? "Подключено" : "Нет подключения";
                    indicator.Fill = IsConnected ? Brushes.Green : Brushes.Red;
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
            return FindName(name) as T;
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