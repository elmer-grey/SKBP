using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Full_modul
{
    /// <summary>
    /// Логика взаимодействия для ChangePassWindow.xaml
    /// </summary>
    public partial class ChangePassWindow : Window
    {
        public ChangePassWindow()
        {
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));
            InitializeComponent();
            this.Focusable = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Password();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (PasswordBox1.Password != "" && PasswordBox2.Password != "")
                {
                    Password();
                    e.Handled = true;
                }
            }
        }

        private void PasswordBox_GotFocus1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox1.Password))
            {
                TextBlock_ShowName1.Visibility = Visibility.Collapsed;
            }
            TextBox_ShowPassword1.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_LostFocus1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox1.Password))
            {
                TextBlock_ShowName1.Visibility = Visibility.Visible;
                TextBlock_ShowName1.Foreground = new SolidColorBrush(Color.FromArgb(192, 10, 10, 10));
                PasswordBox1.Password = "";
            }
        }

        private void ShowPasswordButton_Click1(object sender, RoutedEventArgs e)
        {
            if (TextBox_ShowPassword1.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(PasswordBox1.Password))
                {
                    TextBlock_ShowName1.Visibility = Visibility.Visible;
                } else
                {
                    TextBlock_ShowName1.Visibility = Visibility.Collapsed;
                }
                TextBox_ShowPassword1.Visibility = Visibility.Collapsed;
                PasswordBox1.Visibility = Visibility.Visible;
                ShowPasswordButton1.Content = "👁";
            }
            else
            {
                TextBox_ShowPassword1.Text = PasswordBox1.Password;
                TextBox_ShowPassword1.Visibility = Visibility.Visible;
                PasswordBox1.Visibility = Visibility.Collapsed;
                ShowPasswordButton1.Content = "🙈";
            }
            Keyboard.ClearFocus();
        }

        private void PasswordBox_GotFocus2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox2.Password))
            {
                TextBlock_ShowName2.Visibility = Visibility.Collapsed;
            }
            TextBox_ShowPassword2.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_LostFocus2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox2.Password))
            {
                TextBlock_ShowName2.Visibility = Visibility.Visible;
                TextBlock_ShowName2.Foreground = new SolidColorBrush(Color.FromArgb(192, 10, 10, 10));
                PasswordBox2.Password = "";
            }
        }

        private void ShowPasswordButton_Click2(object sender, RoutedEventArgs e)
        {
            if (TextBox_ShowPassword2.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(PasswordBox1.Password))
                {
                    TextBlock_ShowName2.Visibility = Visibility.Visible;
                }
                else
                {
                    TextBlock_ShowName2.Visibility = Visibility.Collapsed;
                }
                TextBox_ShowPassword2.Visibility = Visibility.Collapsed;
                PasswordBox2.Visibility = Visibility.Visible;
                ShowPasswordButton2.Content = "👁"; 
            }
            else
            {
                TextBox_ShowPassword2.Visibility = Visibility.Visible;
                PasswordBox2.Visibility = Visibility.Collapsed;
                TextBox_ShowPassword2.Text = PasswordBox2.Password;
                ShowPasswordButton2.Content = "🙈";
            }
            Keyboard.ClearFocus();
        }

        private void TextBox_ShowPassword1_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBox1.Password = TextBox_ShowPassword1.Text;
        }

        private void TextBox_ShowPassword2_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBox2.Password = TextBox_ShowPassword2.Text;
        }

        public void Password()
        {
            if (string.IsNullOrWhiteSpace(PasswordBox1.Password) || string.IsNullOrWhiteSpace(PasswordBox2.Password))
            {
                MessageBox.Show("Одно или несколько полей заполнены неверно!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PasswordBox1.Password == PasswordBox2.Password)
            {
                if (!ValidatePassword(PasswordBox2.Password))
                {
                    MessageBox.Show("Пароль не удовлетворяет условиям безопасности!\n - Специальные символы\n - Заглавные буквы\n" +
                        " - Буквы в нижнем регистре\n - Цифры\n - Минимальная длина 8 символов", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(Constants.ConnectionString))
                {
                    conn.Open();

                    string sqlUpdate = "UPDATE [calculator].[dbo].[hr] SET [pass_hr] = @pass_hr WHERE login_hr = @login_hr AND pass_hr = @old_pass_hr";
                    SqlCommand sqlCom = new SqlCommand(sqlUpdate, conn);
                    sqlCom.Parameters.AddWithValue("@pass_hr", PasswordBox2.Password);
                    sqlCom.Parameters.AddWithValue("@login_hr", UserInfo.username);
                    sqlCom.Parameters.AddWithValue("@old_pass_hr", UserInfo.password);

                    try
                    {
                        sqlCom.ExecuteNonQuery();
                        UserInfo.password = PasswordBox2.Password;
                        MessageBox.Show("Пароль успешно изменён!\nВход теперь доступен только с новым паролем.", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                        this.Close();
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Ошибка выполнения запроса:\n" + err.Message,
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidatePassword(string password)
        {
            var hasLetter = false;
            var hasDigit = false;
            var hasUpper = false;
            var hasPunctuation = false;

            foreach (char pass in password)
            {
                if (char.IsUpper(pass) && !hasUpper)
                    hasUpper = true;
                if (char.IsDigit(pass) && !hasDigit)
                    hasDigit = true;
                if (char.IsLetter(pass) && !hasLetter)
                    hasLetter = true;
                if (char.IsPunctuation(pass) && !hasPunctuation)
                    hasPunctuation = true;
            }

            return hasLetter && hasDigit && hasUpper && hasPunctuation && password.Length >= 8;
        }
    }    
}
