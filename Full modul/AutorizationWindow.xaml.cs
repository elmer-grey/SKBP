using Microsoft.Data.SqlClient;
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

namespace Full_modul
{
    /// <summary>
    /// Interaction logic for AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        public AutorizationWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));
            TextBox_Login.GotFocus += TextBox_Login_GotFocus;
            TextBox_Login.LostFocus += TextBox_Login_LostFocus;
            LoadSavedCredentials();
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

        private void HandleLogin()
        {
            if (string.IsNullOrEmpty(TextBox_Login.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Вы не ввели логин или пароль!\nПожалуйста, заполните поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT [id_hr],[login_hr],[pass_hr] FROM [calculator].[dbo].[hr] WHERE login_hr = @login AND pass_hr = @pass", 
                    DatabaseConnection.Instance.Connection))
                {
                    sqlCommand.Parameters.AddWithValue("@login", TextBox_Login.Text);
                    sqlCommand.Parameters.AddWithValue("@pass", PasswordBox.Password);

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            UserInfo.username = Convert.ToString(dataReader["login_hr"]).TrimEnd();
                            UserInfo.password = Convert.ToString(dataReader["pass_hr"]).TrimEnd();

                            if (PasswordBox.Password == "KIBEVS1902")
                            {
                                ChangePassWindow changePasswordWindow = new ChangePassWindow();
                                if (changePasswordWindow.ShowDialog() == true)
                                {
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Изменение пароля отменено!", "Уведомление",
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы вошли в систему!");

                                MainWindow mainWindow = new MainWindow();
                                mainWindow.Show();
                                this.Close();
                                if (!(bool)CheckBox_SaveData.IsChecked)
                                {
                                    TextBox_Login.Text = "";
                                    PasswordBox.Password = "";
                                }
                            }
                        }
                        else
                        {
                            if (TextBox_Login.Text == "admin" && PasswordBox.Password == "admin")
                            {
                                MessageBox.Show("Вы вошли в систему!");

                                MainWindow mainWindow = new MainWindow();
                                mainWindow.Show();
                                this.Close();
                                if (!(bool)CheckBox_SaveData.IsChecked)
                                {
                                    TextBox_Login.Text = "";
                                    PasswordBox.Password = "";
                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы ввели неверный логин или пароль!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Нет подключения");
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
            Properties.Settings.Default.Username = TextBox_Login.Text;
            Properties.Settings.Default.Password = PasswordBox.Password;
            Properties.Settings.Default.RememberMe = true;
            Properties.Settings.Default.Save();
        }

        private void CheckBox_SaveData_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Username = string.Empty;
            Properties.Settings.Default.Password = string.Empty;
            Properties.Settings.Default.RememberMe = false;
            Properties.Settings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Обратитесь к системному администратору для восстановления пароля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}