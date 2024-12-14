﻿using Microsoft.Data.SqlClient;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CalculatorWindow calculatorWindow;

        public MainWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));            
            this.Closing += MainWindow_Closing;

            string query = "SELECT REPLACE(LTRIM(RTRIM(COALESCE(lastname_hr, '') + ' ' + COALESCE(name_hr, '') + ' ' + COALESCE(midname_hr, ''))), '  ', ' ') AS FullName FROM [calculator].[dbo].[hr] WHERE login_hr = @login";

            using (SqlConnection connection = new SqlConnection(Constants.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@login", UserInfo.username);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        user.Text = result.ToString().Trim();
                    }
                    else
                    {
                        user.Text = "Царь и Бог";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // Отображаем диалоговое сообщение для подтверждения закрытия
            MessageBoxResult result = MessageBox.Show("Вы завершили работу с программой? Если вы сейчас продолжите, то все несохраненные данные будут удалены!",
                                                       "Подтверждение выхода",
                                                       MessageBoxButton.YesNo,
                                                       MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                // Отменяем закрытие окна
                e.Cancel = true;
            }
            else
            {
                if (calculatorWindow != null)
                {
                    calculatorWindow.Close();
                }
                AutorizationWindow AutorizationWindow = new AutorizationWindow();
                AutorizationWindow.Show();

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Картинка и текст были нажаты!");
        }

        private void Button_Reports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Здесь Вы будете перенаправлены в папку с отчётами!");
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
            this.WindowState = WindowState.Minimized; // Свернуть окно
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
                    contextMenu.IsOpen = true; // Открыть контекстное меню
                }
                e.Handled = true; // Отметить событие как обработанное
            }
        }
        private void OpenCalculator_Click(object sender, RoutedEventArgs e)
        {
            if (calculatorWindow == null || !calculatorWindow.IsVisible)
            {
                // Создаем новое окно калькулятора, если его еще нет или оно закрыто
                calculatorWindow = new CalculatorWindow();
                calculatorWindow.Closed += CalculatorWindow_Closed; // Подписываемся на событие закрытия
                calculatorWindow.Show();

                // Сворачиваем основное окно
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                if (calculatorWindow.WindowState == WindowState.Minimized)
                {
                    // Если калькулятор свернут, восстанавливаем его
                    calculatorWindow.WindowState = WindowState.Normal;
                }
                else
                {
                    // Если калькулятор открыт, просто активируем его
                    calculatorWindow.Activate();
                }
            }
        }

        private void CalculatorWindow_Closed(object sender, EventArgs e)
        {
            // Активируем главное окно при закрытии калькулятора
            this.Activate();
            this.WindowState = WindowState.Normal; // Восстанавливаем состояние окна
            calculatorWindow = null; // Удаляем ссылку на закрытое окно
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                // Здесь можно добавить логику, если нужно что-то сделать при сворачивании
            }
        }
    }
}