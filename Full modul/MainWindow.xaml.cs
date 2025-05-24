using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace Full_modul
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        private CalculatorWindow calculatorWindow;
        private OrganizAndLegalConditWindow organizAndLegalConditWindow;
        private Enterprise_card enterprise_Card;
        private SaveFile saveFile;
        private SaveFileWindowChoise saveFileWindowChoise;
        private DispatcherTimer _connectionTimer;
        private bool _isTimerInitialized;
        private List<WeakReference<BaseWindow>> _childWindows = new();

        public MainWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));
            this.Closing += MainWindow_Closing;

            UpdateUserStatus("Загрузка...");
            InitializeConnectionStatus();
        }
        private void InitializeConnectionStatus()
        {
            Dispatcher.Invoke(() =>
            {
                if (FindVisualChild<Ellipse>("ConnectionIndicator") is Ellipse indicator &&
                FindVisualChild<TextBlock>("ConnectionStatusText") is TextBlock statusText)
                {
                    statusText.Text = "Проверка подключения...";
                    indicator.Fill = Brushes.Gray;
                }
            });
        }

        private async Task InitializeAsync()
        {
            await InitializeConnectionElementsAsync();
            await LoadUserDataAsync();
        }

        public void InitializeWithConnectionState(bool initialState)
        {
            this.IsConnected = initialState;
            _connectionInitialized = true;
            Dispatcher.InvokeAsync(() => UpdateFullStatus());
        }

        private void InitializeConnectionTimer()
        {
            if (_connectionTimer == null)
            {
                _connectionTimer = new DispatcherTimer(DispatcherPriority.Background)
                {
                    Interval = TimeSpan.FromSeconds(5)
                };

                _connectionTimer.Tick += ConnectionTimer_Tick;
            }

            if (!_isTimerInitialized)
            {
                _connectionTimer.Start();
                _isTimerInitialized = true;
            }
        }

        private async void ConnectionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                _connectionTimer.Stop();

                bool newState = await ConnectionCoordinator.GetConnectionStateAsync()
                    .ConfigureAwait(false);

                if (newState != IsConnected)
                {
                    IsConnected = newState;
                    await Dispatcher.InvokeAsync(async () =>
                    {
                        await UpdateFullStatus();
                        NotifyAllChildren();
                    });
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError($"Ошибка в ConnectionTimer_Tick: {ex.Message}");
            }
            finally
            {
                if (_connectionTimer != null)
                {
                    bool isWindowLoaded = false;
                    Dispatcher.Invoke(() => isWindowLoaded = this.IsLoaded);

                    if (_isTimerInitialized && isWindowLoaded)
                    {
                        _connectionTimer.Start();
                    }
                }
            }
        }

        private void NotifyAllChildren()
        {
            foreach (var windowRef in _childWindows.ToList())
            {
                if (windowRef.TryGetTarget(out var window))
                {
                    window.SetConnectionState(IsConnected);
                }
            }
        }

        public void RegisterChildWindow(BaseWindow window)
        {
            _childWindows.Add(new WeakReference<BaseWindow>(window));

            window.SetConnectionState(IsConnected);
        }

        protected override async Task InitializeConnectionElementsAsync()
        {
            Dispatcher.Invoke(() =>
            {
                if (FindVisualChild<Ellipse>("ConnectionIndicator") is Ellipse indicator &&
                    FindVisualChild<TextBlock>("ConnectionStatusText") is TextBlock statusText)
                {
                    statusText.Text = "Загрузка...";
                    indicator.Fill = Brushes.Gray;
                }

                UpdateUserStatus(UserInfo.username == "admin"
                    ? "Администратор (проверка подключения...)"
                    : "Проверка подключения...");
            });

            if (!_connectionInitialized)
            {
                IsConnected = await ConnectionCoordinator.GetConnectionStateAsync();
                await UpdateFullStatus();
                _connectionInitialized = true;
            }
        }

        protected override async void OnConnectionStateChanged(bool isConnected)
        {
            try
            {
                base.OnConnectionStateChanged(isConnected);
            }
            catch (Exception ex)
            {
                AppLogger.LogError($"Ошибка в OnConnectionStateChanged: {ex.Message}");
            }
        }

        public async Task ForceConnectionCheck()
        {
            ConnectionCoordinator.ResetCache();
            await CheckConnectionAsync(true);
        }

        private void ConnectionStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                _ = ForceConnectionCheck();
            }
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e) //обрати внимание
        {
            var openDocuments = CheckForOpenDocuments(TempFileManager.TempFolder);
            if (openDocuments.Any())
            {
                string docList = string.Join("\n", openDocuments.Take(3));
                if (openDocuments.Count > 3) docList += "\n...";

                var result = MessageBox.Show(
                    $"Обнаружены открытые документы:\n{docList}\n\nЗакрыть их перед выходом?",
                    "Открытые документы",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

                if (result == MessageBoxResult.Yes)
                {
                    CloseAllDocuments(openDocuments);
                }
            }

            MessageBoxResult exitResult = MessageBox.Show("Вы завершили работу с программой? Если вы сейчас продолжите, то все несохраненные данные будут удалены!",
                                                       "Подтверждение выхода",
                                                       MessageBoxButton.YesNo,
                                                       MessageBoxImage.Warning);

            if (exitResult == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                if (_connectionTimer != null)
                {
                    _connectionTimer.Stop();
                    _connectionTimer.Tick -= ConnectionTimer_Tick;
                    _connectionTimer = null;
                }
                _isTimerInitialized = false;

                if (calculatorWindow != null) calculatorWindow.Close();
                if (organizAndLegalConditWindow != null) organizAndLegalConditWindow.Close();
                if (saveFile != null) saveFile.Close();
                if (saveFileWindowChoise != null) saveFileWindowChoise.Close();

                TempFileManager.CleanTempFiles();

                AutorizationWindow AutorizationWindow = new AutorizationWindow();
                AutorizationWindow.Show();

                base.OnClosed(e);
            }
        }
        private List<string> CheckForOpenDocuments(string tempFolder)
        {
            var openDocs = new List<string>();
            try
            {
                foreach (var file in Directory.GetFiles(tempFolder))
                {
                    try
                    {
                        using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            // Файл можно открыть - значит не используется
                        }
                    }
                    catch (IOException)
                    {
                        // Файл занят - значит открыт
                        openDocs.Add(Path.GetFileName(file));
                    }
                }
            }
            catch { /* Обработка ошибок доступа */ }
            return openDocs;
        }

        private void CloseAllDocuments(List<string> documents)
        {
            foreach (var doc in documents)
            {
                try
                {
                    var processes = Process.GetProcessesByName("AcroRd32")
                        .Where(p => !string.IsNullOrEmpty(p.MainWindowTitle)
                        && p.MainWindowTitle.Contains(Path.GetFileNameWithoutExtension(doc)));

                    foreach (var process in processes)
                    {
                        process.CloseMainWindow();
                        if (!process.WaitForExit(2000)) // Даем 2 секунды на закрытие
                        {
                            process.Kill();
                        }
                    }
                }
                catch { /* Игнорируем ошибки */ }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Данный функционал в процессе реализации!");
        }

        private void Button_Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вывод справки!");
        }

        private void Button_Reports_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Отчёты");

            if (Directory.Exists(folderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = folderPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            else
            {
                MessageBox.Show("Папка не найдена: " + folderPath, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_User_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Здесь Вы будете перенаправлены в Личный кабинет!");
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                var image = sender as Image;
                if (image != null)
                {
                    ContextMenu contextMenu = image.ContextMenu;
                    contextMenu.IsOpen = true;
                }
                e.Handled = true;
            }
        }

        private Dictionary<string, object> _childWindowStates = new Dictionary<string, object>();

        private void OpenCalculator_Click(object sender, RoutedEventArgs e)
        {

            if (calculatorWindow == null || !calculatorWindow.IsVisible)
            {
                if (!IsConnected && !EnsureDatabaseConnection(silent: false))
                {
                    return;
                }
                calculatorWindow = new CalculatorWindow();

                if (_childWindowStates.TryGetValue("CalculatorWindow", out var state))
                {
                    calculatorWindow.RestoreState(state);
                } else
                {
                    calculatorWindow._isFirstRun = true;
                }
                
                calculatorWindow.Closed += CalculatorWindow_Closed;
                RegisterChildWindow(calculatorWindow);
                calculatorWindow.Show();
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                if (calculatorWindow.WindowState == WindowState.Minimized)
                {
                    calculatorWindow.WindowState = WindowState.Normal;
                }
                else
                {
                    calculatorWindow.Activate();
                }
            }
        }

        private void CalculatorWindow_Closed(object sender, EventArgs e)
        {
            if (calculatorWindow != null)
            {
                _childWindowStates["CalculatorWindow"] = calculatorWindow.SaveState();
            }
            this.Activate();
            this.WindowState = WindowState.Normal;
            calculatorWindow = null;
        }

        private void OpenCondit_Click(object sender, RoutedEventArgs e)
        {
            if (organizAndLegalConditWindow == null || !organizAndLegalConditWindow.IsVisible)
            {
                if (!IsConnected && !EnsureDatabaseConnection(silent: false)) return;

                organizAndLegalConditWindow = new OrganizAndLegalConditWindow();

                RegisterChildWindow(organizAndLegalConditWindow);

                if (_childWindowStates.TryGetValue("OrganizAndLegalConditWindow", out var state))
                {
                    organizAndLegalConditWindow.RestoreState(state);
                }

                organizAndLegalConditWindow.Closed += ConditWindow_Closed;
                organizAndLegalConditWindow.Show();
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                if (organizAndLegalConditWindow.WindowState == WindowState.Minimized)
                    organizAndLegalConditWindow.WindowState = WindowState.Normal;
                else
                    organizAndLegalConditWindow.Activate();
            }
        }

        private void ConditWindow_Closed(object sender, EventArgs e)
        {
            if (organizAndLegalConditWindow != null)
            {
                _childWindowStates["OrganizAndLegalConditWindow"] = organizAndLegalConditWindow.SaveState();
            }
            this.Activate();
            this.WindowState = WindowState.Normal;
            organizAndLegalConditWindow = null;
        }

        private async void Button_Enterprise_Click(object sender, RoutedEventArgs e)
        {
            if (!IsConnected)
            {
                await EnqueueNotification(
                    "Невозможно открыть карточку предприятия без подключения к БД",
                    Brushes.Red,
                    true);
                return;
            }

            if (enterprise_Card == null || !enterprise_Card.IsVisible)
            {
                enterprise_Card = new Enterprise_card();
                enterprise_Card.Show();
                enterprise_Card.Closed += EnterWindow_Closed;
            }
            else
            {
                if (enterprise_Card.WindowState == WindowState.Minimized)
                {
                    enterprise_Card.WindowState = WindowState.Normal;
                }
                else
                {
                    enterprise_Card.Activate();
                }
            }
        }

        private void EnterWindow_Closed(object sender, EventArgs e)
        {
            this.Activate();
            enterprise_Card = null;
        }
    }
}