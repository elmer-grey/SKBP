using Full_modul.Properties;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Full_modul
{
    /// <summary>
    /// Interaction logic for AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        private bool _isCheckingConnection = false;
        private DispatcherTimer _connectionCheckTimer;

        public AutorizationWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));
            TextBox_Login.GotFocus += TextBox_Login_GotFocus;
            TextBox_Login.LostFocus += TextBox_Login_LostFocus;
            LoadSavedCredentials();
            _connectionCheckTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5) // Проверка каждые 5 секунд
            };
            _connectionCheckTimer.Tick += async (s, e) => await UpdateConnectionStatus();

            Loaded += WindowLoaded;
        }
        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            await UpdateConnectionStatus();
            _connectionCheckTimer.Start();
        }
        protected override void OnClosed(EventArgs e)
        {
            _connectionCheckTimer.Stop();
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

        private async Task UpdateConnectionStatus()
        {
            if (_isCheckingConnection) return;

            _isCheckingConnection = true;
            ConnectionStatusText.Text = "Проверка подключения...";
            ConnectionIndicator.Fill = Brushes.Gray;

            bool isConnected = await DatabaseConnection.TestConnectionAsync();

            ConnectionIndicator.Fill = isConnected ? Brushes.Green : Brushes.Red;
            ConnectionStatusText.Text = isConnected ? "Подключено" : "Нет подключения";
            ConnectionIndicator.ToolTip = isConnected ?
            "Подключение к БД активно" :
            "Нет подключения к базе данных";
            _isCheckingConnection = false;
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

        private async void HandleLogin()
        {
            if (_isCheckingConnection)
            {
                //MessageBox.Show("Идет проверка подключения, подождите...");
                //return;
            }

            if (string.IsNullOrEmpty(TextBox_Login.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Вы не ввели логин или пароль!\nПожалуйста, заполните поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                // Секретная комбинация для админа
                if (TextBox_Login.Text == "superadmin" && PasswordBox.Password == "change")
                {
                    ShowConnectionStringDialog();
                    return;
                }

                // Показать Popup
                ConnectionCheckPopup.IsOpen = true;

                // Добавляем проверку перед запросом
                if (!await DatabaseConnection.TestConnectionAsync())
                {
                    var result = MessageBox.Show("Нет подключения к БД. Повторить попытку?",
                                              "Ошибка",
                                              MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        await UpdateConnectionStatus();
                        HandleLogin(); // Рекурсивный вызов
                    }
                    return;
                }

                // Сначала проверяем существует ли пользователь
                int userCount = DatabaseConnection.Instance.ExecuteScalar<int>(
                    "SELECT COUNT(1) FROM [calculator].[dbo].[hr] WHERE login_hr = @login AND pass_hr = @pass",
                    new SqlParameter("@login", TextBox_Login.Text),
                    new SqlParameter("@pass", PasswordBox.Password)
                );

                if (userCount > 0)
                {
                    // Получаем данные пользователя отдельным запросом
                    var userData = DatabaseConnection.Instance.ExecuteReader(
                        "SELECT [login_hr], [pass_hr] FROM [calculator].[dbo].[hr] WHERE login_hr = @login",
                        new SqlParameter("@login", TextBox_Login.Text)
                    );

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

                    MessageBox.Show("Вы вошли в систему!");
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
                    // Резервный вход для admin/admin
                    MessageBox.Show("Вы вошли в систему!");
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
            }
            catch (SqlException ex)
            {
                StringBuilder errorDetails = new StringBuilder();
                errorDetails.AppendLine($"Произошла ошибка SQL (Код: {ex.Number}):");

                // Перебираем все ошибки (некоторые запросы могут вызывать несколько ошибок)
                foreach (SqlError error in ex.Errors)
                {
                    errorDetails.AppendLine($"• Уровень: {error.Class}");
                    errorDetails.AppendLine($"• Сообщение: {error.Message}");
                    errorDetails.AppendLine($"• Процедура: {error.Procedure}");
                    errorDetails.AppendLine($"• Строка: {error.LineNumber}");
                    errorDetails.AppendLine("------");
                }

                MessageBox.Show(errorDetails.ToString(), "Детали ошибки БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Скрыть Popup
                ConnectionCheckPopup.IsOpen = false;
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

                    // Тестируем подключение
                    using (var testConnection = new SqlConnection(newConnectionString))
                    {
                        testConnection.Open();
                        testConnection.Close();
                    }

                    stopwatch.Stop();

                    // Обновляем подключение
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