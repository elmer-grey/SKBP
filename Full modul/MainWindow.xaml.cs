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

        public MainWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));
            this.Closing += MainWindow_Closing;
            LoadUserDataAsync();
            // Устанавливаем начальный статус
            UpdateUserStatus(GetOfflineStatus());
            InitializeConnectionStatus();
        }

        private string GetOfflineStatus()
        {
            return UserInfo.username == "admin"
                ? "Администратор (оффлайн режим)"
                : "Оффлайн режим";
        }

        private async Task LoadUserDataAsync()
        {
            string query = @"SELECT REPLACE(LTRIM(RTRIM(COALESCE(lastname_hr, '') + ' ' + 
            COALESCE(name_hr, '') + ' ' + COALESCE(midname_hr, ''))), '  ', ' ') 
            AS FullName FROM [calculator].[dbo].[hr] WHERE login_hr = @login";

            try
            {
                if (!IsConnected)
                {
                    UpdateUserStatus(GetOfflineStatus());
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
                if (IsConnected)
                {
                    UpdateUserStatus("Не удалось загрузить данные");
                }
            }
        }

        protected override async Task InitializeConnectionElementsAsync()
        {
            await base.InitializeConnectionElementsAsync();
            if (IsConnected)
            {
                await LoadUserDataAsync();
            }
        }

        private void InitializeConnectionStatus()
        {
            Dispatcher.Invoke(() =>
            {
                if (FindVisualChild<TextBlock>("ConnectionStatusText") is TextBlock statusText)
                {
                    statusText.Text = "Проверка подключения...";
                }
            });
        }

        private void UpdateUserStatus(string status)
        {
            Dispatcher.Invoke(() =>
            {
                user.Text = status;
            });
        }

        protected override async void OnConnectionStateChanged(bool isConnected)
        {
            base.OnConnectionStateChanged(isConnected);

            if (isConnected)
            {
                await LoadUserDataAsync();
            }
            else
            {
                UpdateUserStatus(GetOfflineStatus());
            }
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы завершили работу с программой? Если вы сейчас продолжите, то все несохраненные данные будут удалены!",
                                                       "Подтверждение выхода",
                                                       MessageBoxButton.YesNo,
                                                       MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                if (calculatorWindow != null)
                {
                    calculatorWindow.Close();
                }
                if (organizAndLegalConditWindow != null)
                {
                    organizAndLegalConditWindow.Close();
                }
                if (saveFile != null)
                {
                    saveFile.Close();
                }
                if (saveFileWindowChoise != null)
                {
                    saveFileWindowChoise.Close();
                }
                AutorizationWindow AutorizationWindow = new AutorizationWindow();
                AutorizationWindow.Show();
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

        private void OpenCalculator_Click(object sender, RoutedEventArgs e)
        {

            if (calculatorWindow == null || !calculatorWindow.IsVisible)
            {
                if (!IsConnected && !EnsureDatabaseConnection(silent: false))
                {
                    return;
                }
                calculatorWindow = new CalculatorWindow();
                calculatorWindow.Closed += CalculatorWindow_Closed;
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
            this.Activate();
            this.WindowState = WindowState.Normal;
            calculatorWindow = null;
        }

        private void OpenCondit_Click(object sender, RoutedEventArgs e)
        {
            if (organizAndLegalConditWindow == null || !organizAndLegalConditWindow.IsVisible)
            {
                if (!IsConnected && !EnsureDatabaseConnection(silent: false))
                {
                    return;
                }
                organizAndLegalConditWindow = new OrganizAndLegalConditWindow();
                organizAndLegalConditWindow.Closed += ConditWindow_Closed;
                organizAndLegalConditWindow.Show();
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                if (organizAndLegalConditWindow.WindowState == WindowState.Minimized)
                {
                    organizAndLegalConditWindow.WindowState = WindowState.Normal;
                }
                else
                {
                    organizAndLegalConditWindow.Activate();
                }
            }
        }

        private void ConditWindow_Closed(object sender, EventArgs e)
        {
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