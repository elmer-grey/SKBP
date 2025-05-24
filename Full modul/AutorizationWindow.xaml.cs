using Full_modul.Properties;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Full_modul
{
    /// <summary>
    /// Interaction logic for AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : BaseWindow
    {
        private bool _lastKnownConnectionState;
        private bool _isInitialConnectionCheckComplete;
        public AutorizationWindow()
        {
            InitializeComponent();
            InitializeAsync();
            TextBox_ShowPassword.ContextMenu = null;
        }
        private async void InitializeAsync()
        {
            LoginButton.IsEnabled = true;
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));
            TextBox_Login.GotFocus += TextBox_Login_GotFocus;
            TextBox_Login.LostFocus += TextBox_Login_LostFocus;
            LoadSavedCredentials();

            _ = Task.Run(() => InitializeConnectionSafeAsync());
        }

        protected override async Task InitializeConnectionElementsAsync()
        {
            await base.InitializeConnectionElementsAsync();
        }

        private async Task InitializeConnectionSafeAsync()
        {
            try
            {
                SetConnectionUI("Проверка подключения...", Brushes.Gray, false);

                bool isConnected = await ConnectionCoordinator.GetConnectionStateAsync();
                _lastKnownConnectionState = isConnected;
                _connectionInitialized = true;

                await ProcessConnectionResult(isConnected);
            }
            finally
            {
                ConnectionCheckPopup.IsOpen = false;
            }
        }

        private async Task ProcessConnectionResult(bool isConnected)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                ConnectionStatusText.Text = isConnected ? "Подключено" : "Нет подключения";
                ConnectionIndicator.Fill = isConnected ? Brushes.LimeGreen : Brushes.Red;
                LoginButton.IsEnabled = isConnected ? true : false;
            });

            if (isConnected)
            {
                AppLogger.LogInfo("Успешное подключение к БД");
            }
            else
            {
                _wasDisconnected = true;
                AppLogger.LogWarning("Отсутствует подключение к БД");
                await ShowNotification("Нет подключения к БД", Brushes.Red, true);
            }
        }

        private void SetConnectionUI(string status, Brush color, bool isEnabled)
        {
            Dispatcher.Invoke(() =>
            {
                ConnectionStatusText.Text = status;
                ConnectionIndicator.Fill = color;
                LoginButton.IsEnabled = isEnabled;
            });
        }

        protected override async void OnConnectionStateChanged(bool isConnected)
        {
            if (!_connectionInitialized)
                return;

            base.OnConnectionStateChanged(isConnected);

            await Dispatcher.InvokeAsync(() =>
            {
                if (LoginButton.IsEnabled != isConnected)
                {
                    LoginButton.IsEnabled = isConnected;
                }
            }, DispatcherPriority.Background);

            if (!isConnected)
            {
                await ShowNotification("Нет подключения к БД", Brushes.Red, true);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            TempFileManager.CleanTempFiles();
            base.OnClosed(e);
        }

        private void LoadSavedCredentials()
        {
            if (Properties.Settings.Default.RememberMe)
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Username))
                {
                    TextBox_Login.Text = Properties.Settings.Default.Username;
                    TextBox_Login.Foreground = Brushes.Black;
                    TextBox_Login.IsReadOnly = false;
                }

                PasswordBox.Password = Properties.Settings.Default.Password;
                CheckBox_SaveData.IsChecked = true;

                TextBlock_ShowName.Visibility = string.IsNullOrEmpty(PasswordBox.Password)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            else
            {
                UserInfo.username = "";
                UserInfo.password = "";
            }
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

        private void TextBox_Login_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox_Login.IsReadOnly = false;

            if (TextBox_Login.Text == "Введите логин")
            {
                TextBox_Login.Text = "";
                TextBox_Login.Foreground = Brushes.Black;
            }
        }

        private void TextBox_Login_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox_Login.Text))
            {
                TextBox_Login.Text = "Введите логин";
                TextBox_Login.Foreground = new SolidColorBrush(Color.FromArgb(192, 10, 10, 10));
                TextBox_Login.IsReadOnly = true;
            }
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

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                TextBlock_ShowName.Visibility = Visibility.Collapsed;
            }
            TextBox_ShowPassword.Visibility = Visibility.Collapsed;

            if (Keyboard.FocusedElement == PasswordBox)
            {
                PasswordBox.SelectAll();
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                TextBlock_ShowName.Visibility = Visibility.Visible;
                TextBlock_ShowName.Foreground = new SolidColorBrush(Color.FromArgb(192, 10, 10, 10));
                PasswordBox.Password = "";
            }
        }
        
        private void TextBox_ShowPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Apps)
            {
                e.Handled = true;
            }
        }

        private void TextBox_ShowPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                e.Handled = true;
            }
        }

        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_ShowPassword.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(PasswordBox.Password))
                {
                    TextBlock_ShowName.Visibility = Visibility.Visible;
                }
                else
                {
                    TextBlock_ShowName.Visibility = Visibility.Collapsed;
                }
                PasswordBox.Password = TextBox_ShowPassword.Text;
                TextBox_ShowPassword.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
                ShowPasswordButton.Content = "👁";
            }
            else
            {
                TextBox_ShowPassword.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
                TextBox_ShowPassword.Text = PasswordBox.Password;
                ShowPasswordButton.Content = "🙈";
            }
            Keyboard.ClearFocus();
        }

        private void TextBox_ShowPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBox.Password = TextBox_ShowPassword.Text;
        }

        private void Button_Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вывод справки!");
        }

        private async Task<bool> TryReconnectWithFeedback()
        {
            ConnectionCheckPopup.IsOpen = true;

            try
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

                bool isConnected = await CheckConnectionAsync();

                if (!isConnected)
                {
                    await ShowNotification("Не удалось подключиться к БД", Brushes.Red, true);
                }

                return isConnected;
            }
            finally
            {
                ConnectionCheckPopup.IsOpen = false;
                //UpdateConnectionStatus();
                UpdateConnectionUI();
            }
        }

        private async void HandleLogin()
        {
            if (string.IsNullOrEmpty(TextBox_Login.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Вы не ввели логин или пароль!\nПожалуйста, заполните поля!",
                              "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                if (TextBox_Login.Text == "superadmin" && PasswordBox.Password == "change")
                {
                    AppLogger.LogInfo($"Начало изменения строки подключения суперадминистратором");
                    TextBox_Login.Text = "";
                    PasswordBox.Password = "";
                    TextBlock_ShowName.Visibility = Visibility.Visible;
                    ShowConnectionStringDialog();
                    return;
                }

                AppLogger.LogInfo($"Попытка входа пользователя: {TextBox_Login.Text}");
                bool isConnected = false;
                for (int i = 0; i < 3; i++)
                {
                    isConnected = await ConnectionCoordinator.GetConnectionStateAsync();
                    if (isConnected) break;
                    await Task.Delay(1000);
                }

                if (!isConnected)
                {
                    var result = MessageBox.Show("Нет подключения к БД. Повторить попытку?",
                                              "Ошибка", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        if (await TryReconnectWithFeedback())
                        {
                            HandleLogin();
                        }
                        return;
                    }
                    return;
                }
                Keyboard.ClearFocus();
                int userCount = DatabaseConnection.Instance.ExecuteScalar<int>(
                    "SELECT COUNT(1) FROM [calculator].[dbo].[hr] WHERE login_hr = @login AND pass_hr = @pass",
                    new SqlParameter("@login", TextBox_Login.Text),
                    new SqlParameter("@pass", PasswordBox.Password));

                if (userCount > 0)
                {
                    bool isBlocked = DatabaseConnection.Instance.ExecuteScalar<int>(
        "SELECT COUNT(1) FROM [calculator].[dbo].[hr] WHERE login_hr = @login AND endwork_hr IS NOT NULL",
        new SqlParameter("@login", TextBox_Login.Text)) > 0;

                    if (isBlocked)
                    {
                        MessageBox.Show("Учетная запись заблокирована!\nОбратитесь к администратору.",
                                        "Доступ запрещен",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                        return;
                    }

                    var userData = DatabaseConnection.Instance.ExecuteReader(
                        "SELECT [login_hr], [pass_hr] FROM [calculator].[dbo].[hr] WHERE login_hr = @login",
                        new SqlParameter("@login", TextBox_Login.Text));

                    using (userData)
                    {
                        if (userData.Read())
                        {
                            UserInfo.username = userData["login_hr"].ToString().Trim();
                            UserInfo.password = userData["pass_hr"].ToString().Trim();
                        }
                    }

                    if (PasswordBox.Password == "KIBEVS1902")
                    {
                        var changePasswordWindow = new ChangePassWindow();
                        if (changePasswordWindow.ShowDialog() == true)
                        {
                            TextBox_ShowPassword.Text = "";
                            UserInfo.username = TextBox_Login.Text;
                            PasswordBox.Password = "";
                            TextBlock_ShowName.Visibility = Visibility.Visible;
                            return;
                        }
                        MessageBox.Show("Изменение пароля отменено!", "Уведомление",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var mainWindow = new MainWindow();
                    mainWindow.InitializeWithConnectionState(_lastKnownConnectionState);
                    mainWindow.Show();
                    this.Close();

                    if (!(bool)CheckBox_SaveData.IsChecked)
                    {
                        TextBox_Login.Text = "";
                        PasswordBox.Password = "";
                        TextBox_ShowPassword.Text = "";
                    }
                }
                else if (TextBox_Login.Text == "admin" && PasswordBox.Password == "admin")
                {
                    UserInfo.username = TextBox_Login.Text;

                    var mainWindow = new MainWindow();
                    mainWindow.InitializeWithConnectionState(_lastKnownConnectionState);
                    mainWindow.Show();
                    this.Close();

                    if (!(bool)CheckBox_SaveData.IsChecked)
                    {
                        TextBox_Login.Text = "";
                        PasswordBox.Password = "";
                        TextBox_ShowPassword.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Вы ввели неверный логин или пароль!",
                                  "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                AppLogger.LogInfo($"Успешный вход: {UserInfo.username}");
            }
            catch (SqlException ex)
            {
                StringBuilder errorDetails = new StringBuilder();
                errorDetails.AppendLine($"Произошла ошибка SQL (Код: {ex.Number}):");

                foreach (SqlError error in ex.Errors)
                {
                    errorDetails.AppendLine($"• Уровень: {error.Class}");
                    errorDetails.AppendLine($"• Сообщение: {error.Message}");
                    errorDetails.AppendLine($"• Процедура: {error.Procedure}");
                    errorDetails.AppendLine($"• Строка: {error.LineNumber}");
                    errorDetails.AppendLine("------");
                }

                MessageBox.Show(errorDetails.ToString(), "Детали ошибки БД", MessageBoxButton.OK, MessageBoxImage.Error);
                AppLogger.LogError($"Ошибка авторизации: {ex.Message}");
            }
        }

        private void ShowConnectionStringDialog()
        {
            string currentConnection = SecurityHelper.Decrypt(Settings.Default.ConnectionString);

            string newConnectionString = Microsoft.VisualBasic.Interaction.InputBox
            ($"Обновление строки подключения", "Введите новую строку подключения:", currentConnection);
            
            if (!string.IsNullOrEmpty(newConnectionString))
            {
                try
                {
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                    using (var testConnection = new SqlConnection(newConnectionString))
                    {
                        testConnection.Open();
                        testConnection.Close();
                    }

                    stopwatch.Stop();

                    DatabaseConnection.UpdateConnection(newConnectionString);

                    MessageBox.Show($"Подключение успешно обновлено!\n" +
                                  $"Время подключения: {stopwatch.ElapsedMilliseconds} мс",
                                  "Успех",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения: {ex.Message}\n" +
                                   "Проверьте параметры подключения",
                                   "Ошибка",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Error);
                }
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && IsConnected)
            {
                ProcessLogin();
                e.Handled = true;
            }
        }

        private async void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            ProcessLogin();
        }

        private void ProcessLogin()
        {
            try
            {
                HandleLogin();

                if ((bool)CheckBox_SaveData.IsChecked)
                {
                    CheckBox_SaveData_Checked(CheckBox_SaveData, new RoutedEventArgs());
                }
                else
                {
                    CheckBox_SaveData_Unchecked(CheckBox_SaveData, new RoutedEventArgs());
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError($"Ошибка входа: {ex.Message}");
                Dispatcher.InvokeAsync(() =>
                    MessageBox.Show("Ошибка при входе в систему"),
                    DispatcherPriority.Background);
            }

        }

        private void CheckBox_SaveData_Checked(object sender, RoutedEventArgs e)
        {
            if (TextBox_Login.Text != "Введите логин")
            {
                Settings.Default.Username = TextBox_Login.Text;
            }
            else
            {
                Settings.Default.Username = "";
            }

            Settings.Default.Password = PasswordBox.Password;
            Settings.Default.RememberMe = true;
            Settings.Default.Save();
        }

        private void CheckBox_SaveData_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.RememberMe = false;
            Settings.Default.Save();

            if (TextBox_Login.Text == "Введите логин" || string.IsNullOrWhiteSpace(TextBox_Login.Text))
            {
                TextBox_Login.Text = "Введите логин";
                TextBox_Login.Foreground = new SolidColorBrush(Color.FromArgb(192, 10, 10, 10));
                TextBox_Login.IsReadOnly = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Обратитесь к системному администратору для восстановления пароля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}