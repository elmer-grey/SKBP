using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace Full_modul
{
    /// <summary>
    /// Логика взаимодействия для SaveFileWindowChoise.xaml
    /// </summary>
    public partial class SaveFileWindowChoise : Window
    {
        private string _previousWindow;

        public SaveFileWindowChoise(string previousWindow)
        {
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));
            InitializeComponent();
            _previousWindow = previousWindow;

            if (_previousWindow == "CalculatorWindow")
            {
                MessageTextBlock.Text = "Вы запустили процедуру сохранения отчёта! \nВведите в окно ниже название файла. \nВнимание! Он будет сохранен в соответствующую папку, где находится данное ПО!";
            }
            else if (_previousWindow == "Условия")
            {
                MessageTextBlock.Text = "Вы запустили процедуру сохранения документа с результатами! \nВведите в окно ниже название файла. \nВнимание! Он будет сохранен в соответствующую папку, где находится данное ПО!";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = FileNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show("Пожалуйста, введите название файла.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string folderPath = _previousWindow == "CalculatorWindow"
                ? Path.Combine(Directory.GetCurrentDirectory(), "отчёты")
                : Path.Combine(Directory.GetCurrentDirectory(), "результаты");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName + ".txt");

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine("Данные сохранены: " + fileName);

            }

            MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
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
    }
}
