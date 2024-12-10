using System;
using System.Collections.Generic;
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
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Картинка и текст были нажаты!");
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

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (PasswordBox1.Password != "" && PasswordBox2.Password != "")
                {
                    MessageBox.Show("Вы нажали Enter!");
                    e.Handled = true;
                }
                else
                {
                    MessageBox.Show("Вы не ввели пароль!");
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
                PasswordBox1.Password = TextBox_ShowPassword1.Text;
                TextBox_ShowPassword1.Visibility = Visibility.Collapsed;
                PasswordBox1.Visibility = Visibility.Visible;
                ShowPasswordButton1.Content = "👁";
            }
            else
            {
                TextBox_ShowPassword1.Visibility = Visibility.Visible;
                PasswordBox1.Visibility = Visibility.Collapsed;
                TextBox_ShowPassword1.Text = PasswordBox1.Password;
                ShowPasswordButton1.Content = "🙈";
            }
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
                PasswordBox2.Password = TextBox_ShowPassword1.Text;
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
        }
    }
}
