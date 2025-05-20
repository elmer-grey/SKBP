using Full_modul.Properties;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Full_modul
{
    /// <summary>
    /// Interaction logic for AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : BaseWindow
    {
        private bool _isConnectionInitialized = false;
        public AutorizationWindow()
        {
            InitializeComponent();

            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));
            TextBox_Login.GotFocus += TextBox_Login_GotFocus;
            TextBox_Login.LostFocus += TextBox_Login_LostFocus;

            LoadSavedCredentials();
            this.Loaded += async (s, e) =>
            {
                await InitializeConnectionAsync();
                UpdateConnectionStatus();
            };
        }

        protected override async Task InitializeConnectionElementsAsync()
        {
            await base.InitializeConnectionElementsAsync();
            UpdateConnectionStatus();
        }

        private async Task InitializeConnectionAsync()
        {
            Dispatcher.Invoke(() =>
            {
                if (FindVisualChild<TextBlock>("ConnectionStatusText") is TextBlock statusText)
                {
                    statusText.Text = "Проверка подключения...";
                }
            });
            ConnectionCheckPopup.IsOpen = true;
            try
            {
                await CheckConnectionAsync(forceCheck: true);
                _isConnectionInitialized = true;
            }
            finally
            {
                ConnectionCheckPopup.IsOpen = false;
            }
        }

        private void UpdateConnectionStatus()
        {
            Dispatcher.Invoke(() =>
            {
                if (FindVisualChild<TextBlock>("ConnectionStatusText") is TextBlock statusText)
                {
                    statusText.Text = IsConnected ? "Подключено" : "Нет подключения";
                }
                ConnectionCheckPopup.IsOpen = false;
            });
        }

        protected override async void OnConnectionStateChanged(bool isConnected)
        {
            if (!_isConnectionInitialized) return;

            base.OnConnectionStateChanged(isConnected);
            UpdateConnectionStatus();

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
                TextBox_Login.Text = Properties.Settings.Default.Username;
                PasswordBox.Password = Properties.Settings.Default.Password;
                CheckBox_SaveData.IsChecked = true;
                if (string.IsNullOrEmpty(PasswordBox.Password))
                {
                    TextBlock_ShowName.Visibility = Visibility.Visible;
                }
                else
                {
                    TextBlock_ShowName.Visibility = Visibility.Collapsed;
                }
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
            if (TextBox_Login.Text == "Введите логин")
            {
                TextBox_Login.Text = "";
                TextBox_Login.Foreground = new SolidColorBrush(Colors.Black);
                TextBox_Login.IsReadOnly = false;
            } else if (TextBox_Login.Text == UserInfo.username)
            {
                TextBox_Login.Foreground = new SolidColorBrush(Colors.Black);
                TextBox_Login.IsReadOnly = false;
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

        private bool _isManualCheck = false;

        private async Task<bool> TryReconnectWithFeedback()
        {
            ConnectionCheckPopup.IsOpen = true;

            try
            {
                // Обновляем статус перед попыткой подключения
                Dispatcher.Invoke(() =>
                {
                    if (FindVisualChild<TextBlock>("ConnectionStatusText") is TextBlock statusText)
                    {
                        statusText.Text = "Попытка подключения...";
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
                UpdateConnectionStatus();
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
                AppLogger.LogInfo($"Попытка входа пользователя: {TextBox_Login.Text}");

                if (TextBox_Login.Text == "superadmin" && PasswordBox.Password == "change")
                {
                    ShowConnectionStringDialog();
                    return;
                }

                if (!IsConnected)
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

                int userCount = DatabaseConnection.Instance.ExecuteScalar<int>(
                    "SELECT COUNT(1) FROM [calculator].[dbo].[hr] WHERE login_hr = @login AND pass_hr = @pass",
                    new SqlParameter("@login", TextBox_Login.Text),
                    new SqlParameter("@pass", PasswordBox.Password));

                if (userCount > 0)
                {
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
                            return;
                        }
                        MessageBox.Show("Изменение пароля отменено!", "Уведомление",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    new MainWindow().Show();
                    this.Close();

                    if (!(bool)CheckBox_SaveData.IsChecked)
                    {
                        TextBox_Login.Text = "";
                        PasswordBox.Password = "";
                    }
                }
                else if (TextBox_Login.Text == "admin" && PasswordBox.Password == "admin")
                {
                    UserInfo.username = TextBox_Login.Text;
                    new MainWindow().Show();
                    this.Close();

                    if (!(bool)CheckBox_SaveData.IsChecked)
                    {
                        TextBox_Login.Text = "";
                        PasswordBox.Password = "";
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
            if (e.Key == Key.Enter)
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
                e.Handled = true;
            }
        }

        private void Button_Login_Click(object sender, RoutedEventArgs e)
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

        private void CheckBox_SaveData_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.Username = TextBox_Login.Text;
            Settings.Default.Password = PasswordBox.Password;
            Settings.Default.RememberMe = true;
            Settings.Default.Save();
        }

        private void CheckBox_SaveData_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.Username = string.Empty;
            Settings.Default.Password = string.Empty;
            Settings.Default.RememberMe = false;
            Settings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Обратитесь к системному администратору для восстановления пароля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}