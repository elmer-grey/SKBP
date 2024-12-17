using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
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
        private CalculatorWindow _calculatorWindow;
        private OrganizAndLegalConditWindow _organizAndLegalConditWindow;
        public int SelectedIndex { get; private set; }

        public SaveFileWindowChoise(CalculatorWindow calculatorWindow, OrganizAndLegalConditWindow organizAndLegalConditWindow, string previousWindow)
        {
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));
            InitializeComponent();
            _previousWindow = previousWindow;
            _calculatorWindow = calculatorWindow;
            _organizAndLegalConditWindow = organizAndLegalConditWindow;

            if (_previousWindow == "CalculatorWindow")
            {
                MessageTextBlock.Text = "Вы запустили процедуру сохранения отчёта! \nВведите в окно ниже название файла. \nВнимание! Он будет сохранен в соответствующую папку, где находится данное ПО!";
            }
            else if (_previousWindow == "OrganizAndLegalConditWindow")
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
                ? Path.Combine(Directory.GetCurrentDirectory(), "Отчёты", "Калькулятор")
                : Path.Combine(Directory.GetCurrentDirectory(), "Отчёты", "Условия");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName + ".txt");

            try
            {
                if (_previousWindow == "CalculatorWindow")
                {
                    bool hasDataToSave = false;
                    List<string> dataToSave = new List<string>();

                    if (Data.SaveFile == 0)
                    {
                        if (Data.Check0 == true)
                        {
                            string data = _calculatorWindow.SaveData(0);
                            if (!string.IsNullOrEmpty(data))
                            {
                                dataToSave.Add(data);
                                hasDataToSave = true;
                            }
                        }
                        if (Data.Check1 == true)
                        {
                            string data = _calculatorWindow.SaveData(1);
                            if (!string.IsNullOrEmpty(data))
                            {
                                dataToSave.Add(data);
                                hasDataToSave = true;
                            }
                        }
                        if (Data.Check2 == true)
                        {
                            string data = _calculatorWindow.SaveData(2);
                            if (!string.IsNullOrEmpty(data))
                            {
                                dataToSave.Add(data);
                                hasDataToSave = true;
                            }
                        }
                        if (Data.Check3 == true)
                        {
                            string data = _calculatorWindow.SaveData(3);
                            if (!string.IsNullOrEmpty(data))
                            {
                                dataToSave.Add(data);
                                hasDataToSave = true;
                            }
                        }

                        if (!hasDataToSave)
                        {
                            MessageBox.Show("Сохранение файла было отменено пользователем!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            this.Close();
                            return;
                        }

                        Data.SaveFile = 1;

                        using (StreamWriter sw = new StreamWriter(filePath, true))
                        {
                            sw.WriteLine("Пользователь {0}. Время {1}\n", UserInfo.username, DateTime.Now);

                            foreach (var data in dataToSave)
                            {
                                sw.WriteLine(data);
                            }
                        }
                    }
                    else
                    {
                        string data = string.Empty;
                        switch (Data.BoxIndex)
                        {
                            case 0:
                                data = _calculatorWindow.SaveData(0);
                                break;
                            case 1:
                                data = _calculatorWindow.SaveData(1);
                                break;
                            case 2:
                                data = _calculatorWindow.SaveData(2);
                                break;
                            case 3:
                                data = _calculatorWindow.SaveData(3);
                                break;
                            default:
                                break;
                        }

                        if (string.IsNullOrEmpty(data))
                        {
                            MessageBox.Show("Сохранение файла было отменено пользователем!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            this.Close();
                            return;
                        }
                        using (StreamWriter sw = new StreamWriter(filePath, true))
                        {
                            sw.WriteLine("Пользователь {0}. Время {1}\n", UserInfo.username, DateTime.Now);
                            sw.WriteLine(data);
                        }
                    }
                } else
                {
                    string data = _organizAndLegalConditWindow.SaveData();
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine("Пользователь {0}. Время {1}\n", UserInfo.username, DateTime.Now);
                        sw.WriteLine(data);
                    }
                }
                MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.Close();
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SaveButton_Click(sender, null);
                e.Handled = true;
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
