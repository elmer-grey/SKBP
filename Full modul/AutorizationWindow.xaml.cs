﻿using System.Text;
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
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR1.ico"));
            TextBox_Login.GotFocus += TextBox_Login_GotFocus;
            TextBox_Login.LostFocus += TextBox_Login_LostFocus;
            //TextBox_Pass.GotFocus += TextBox_Pass_GotFocus;
            //TextBox_Pass.LostFocus += TextBox_Pass_LostFocus;
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

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TextBox_Login.Text != "" && PasswordBox.Password != "")
                {
                    MessageBox.Show("Вы нажали Enter!");
                    e.Handled = true;
                }
                else
                {
                    MessageBox.Show("Вы не ввели логин и/или пароль!");
                    e.Handled = true;
                }
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
        }
    }
}