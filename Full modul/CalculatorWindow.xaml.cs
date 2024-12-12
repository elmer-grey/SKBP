using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Calendar = System.Windows.Controls.Calendar;

namespace Full_modul
{
    /// <summary>
    /// Логика взаимодействия для CalculatorWindow.xaml
    /// </summary>
    

    public partial class CalculatorWindow : Window
    {
        public CalculatorWindow()
        {
            InitializeComponent();
            //InitializeDateTextBoxes();
            //DataContext = new ViewModel();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));
            koefgroup0.Visibility = Visibility.Collapsed;
            koefgroup1.Visibility = Visibility.Collapsed;
            koefgroup2.Visibility = Visibility.Collapsed;
            koefgroup3.Visibility = Visibility.Collapsed;
        }
    
        private void InitializeDateTextBoxes()
        {
            // Установка текущей даты для всех TextBox
            foreach (var textBox in FindVisualChildren<TextBox>(this))
            {
                textBox.Text = DateTime.Now.ToShortDateString();
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

        private void comboBoxFormules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlock_ShowName.Visibility = Visibility.Collapsed;
            koefgroup0.Visibility = Visibility.Collapsed;
            koefgroup1.Visibility = Visibility.Collapsed;
            koefgroup2.Visibility = Visibility.Collapsed;
            koefgroup3.Visibility = Visibility.Collapsed;

            int selectedIndex = comboBoxFormules.SelectedIndex;

            switch (selectedIndex)
            {
                case 0:
                    koefgroup0.Visibility = Visibility.Visible;
                    break;
                case 1:
                    koefgroup1.Visibility = Visibility.Visible;
                    break;
                case 2:
                    koefgroup2.Visibility = Visibility.Visible;
                    break;
                case 3:
                    koefgroup3.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void ComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Получаем ссылку на ComboBox
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                // Проверяем, есть ли у ComboBox открытый Popup
                Popup popup = FindVisualChild<Popup>(comboBox);
                if (popup != null)
                {
                    popup.IsOpen = !popup.IsOpen; // Переключаем видимость Popup
                }
            }
        }

        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calendar = sender as Calendar;
            if (calendar != null)
            {
                DateTime? selectedDate = calendar.SelectedDate;

                // Проверяем, установлен ли Tag
                if (calendar.Tag != null)
                {
                    // Получаем имя TextBox из Tag
                    string textBoxName = calendar.Tag.ToString();

                    // Находим соответствующий TextBox по имени
                    TextBox dateTextBox = FindName(textBoxName) as TextBox;

                    if (dateTextBox != null && selectedDate.HasValue)
                    {
                        dateTextBox.Text = selectedDate.Value.ToShortDateString();
                    }
                }

                // Закрываем Popup после выбора даты
                Popup popup = FindVisualChild<Popup>(calendar);
                if (popup != null)
                {
                    popup.IsOpen = false;
                }
            }
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

        private void Calendar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Убираем фокус с календаря
            Keyboard.ClearFocus();
        }

        private IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                {
                    yield return typedChild;
                }

                foreach (var childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем выбранный элемент
            ComboBox levelworker0 = sender as ComboBox;
            ComboBoxItem selectedItem = levelworker0.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                // Получаем текст выбранного элемента
                string? selectedValue = selectedItem.Content?.ToString();

                levelworker0.Text = selectedValue;
            }
        }

        public void ExportButton_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        public void ExportData()
        {
            SaveFileWindowChoise saveFileWindow = new SaveFileWindowChoise("CalculatorWindow"); // или "Условия"
            saveFileWindow.ShowDialog();
            /*
            SaveFile sfa = new SaveFile();
            sfa.ShowDialog();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Текстовый файл|*.txt";
            saveFileDialog1.Title = "Сохранить данные ...";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();
                fs.Close();
                DateTime dt = DateTime.Now;
                StreamWriter sw = new StreamWriter(fs.Name, true, Encoding.UTF8);
                Thread thread = new Thread(() => Clipboard.SetText(sw.NewLine));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
                sw.WriteLine("Пользователь {0}. Время {1}\n", UserInfo.username, DateTime.Now);
                if (Data.Check0 == true)
                {
                    if (result0.Text == "")
                    {
                        var result = MessageBox.Show("Вы не заполнили формулу ''Коэффициент оборота по приему''! Продолжить?",
                            "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            sw.WriteLine($"Коэффициент оборота по приему\nСЧР равен: -" +
                                 $"\nНачало периода подсчёта: -\nКонец периода подсчёта: -\n" +
                                 $"Рассматриваемая должность: -\nКоличество: -\n" +
                    $"Результат равен: -\n" + "================\n");
                        }
                    }
                    else
                    {
                        if (YearAgo0.Checked == true)
                            sw.WriteLine($"Коэффициент оборота по приему\nСЧР равен: {SChRText0.Text}" +
                                 $"\nНачало периода подсчёта: {YearAgoText0.Text}\nКонец периода подсчёта: {dateTimePicker0.Value.ToLongDateString()}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox0.Text}\nКоличество: {amountKoef0.Text}\n" +
                $"Результат равен: {resultKoef0.Text}\n" + "================\n");
                        else
                            sw.WriteLine($"Коэффициент оборота по приему\nСЧР равен: {SChRText0.Text}" +
                             $"\nНачало периода подсчёта: {dateTimePicker0.Value.ToLongDateString()}\nКонец периода подсчёта: {YearForwardText0.Text}\n" +
                             $"Рассматриваемая должность: {LevelComboBox0.Text}\nКоличество: {amountKoef0.Text}\n" +
                $"Результат равен: {resultKoef0.Text}\n" + "================\n");
                    }
                }
                if (Data.Check1 == true)
                {
                    if (amountKoef1.Text == "")
                    {
                        var result = MessageBox.Show("Вы не заполнили формулу ''Коэффициент оборота по выбытию''! Продолжить?",
                            "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            sw.WriteLine($"Коэффициент оборота по выбытию\nСЧР равен: -" +
                                 $"\nНачало периода подсчёта: -\nКонец периода подсчёта: -\n" +
                                 $"Рассматриваемая должность: -\nКоличество: -\n" +
                    $"Результат равен: -\n" + "================\n");
                        }
                    }
                    else
                    {
                        if (YearAgo1.Checked == true)
                            sw.WriteLine($"Коэффициент оборота по выбытию\nСЧР равен: {SChRText1.Text}" +
                                 $"\nНачало периода подсчёта: {YearAgoText1.Text}\nКонец периода подсчёта: {dateTimePicker1.Value.ToLongDateString()}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox1.Text}\nКоличество: {amountKoef1.Text}\n" +
                $"Результат равен: {resultKoef1.Text}\n" + "================\n");
                        else
                            sw.WriteLine($"Коэффициент оборота по выбытию\nСЧР равен: {SChRText1.Text}" +
                             $"\nНачало периода подсчёта: {dateTimePicker1.Value.ToLongDateString()}\nКонец периода подсчёта: {YearForwardText1.Text}\n" +
                             $"Рассматриваемая должность: {LevelComboBox1.Text}\nКоличество: {amountKoef1.Text}\n" +
                $"Результат равен: {resultKoef1.Text}\n" + "================\n");
                    }
                }
                if (Data.Check2 == true)
                {
                    if (amountKoef2.Text == "")
                    {
                        var result = MessageBox.Show("Вы не заполнили формулу ''Коэффициент текучести кадров''! Продолжить?",
                            "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            sw.WriteLine($"Коэффициент текучести кадров\nСЧР равен: -" +
                                 $"\nНачало периода подсчёта: -\nКонец периода подсчёта: -\n" +
                                 $"Рассматриваемая должность: -\nКоличество: -\n" +
                    $"Результат равен: -\n" + "================\n");
                        }
                    }
                    else
                    {
                        if (YearAgo2.Checked == true)
                            sw.WriteLine($"Коэффициент текучести кадров\nСЧР равен: {SChRText2.Text}" +
                                 $"\nНачало периода подсчёта: {YearAgoText2.Text}\nКонец периода подсчёта: {dateTimePicker2.Value.ToLongDateString()}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox2.Text}\nКоличество: {amountKoef2.Text}\n" +
                $"Результат равен: {resultKoef2.Text}\n" + "================\n");
                        else
                            sw.WriteLine($"Коэффициент текучести кадров\nСЧР равен: {SChRText2.Text}" +
                             $"\nНачало периода подсчёта: {dateTimePicker2.Value.ToLongDateString()}\nКонец периода подсчёта: {YearForwardText2.Text}\n" +
                             $"Рассматриваемая должность: {LevelComboBox2.Text}\nКоличество: {amountKoef2.Text}\n" +
                $"Результат равен: {resultKoef2.Text}\n" + "================\n");
                    }
                }
                if (Data.Check3 == true)
                {
                    if (amountKoef3.Text == "")
                    {
                        var result = MessageBox.Show("Вы не заполнили формулу ''Коэффициент постоянства состава персонала предприятия''! Продолжить?",
                            "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            sw.WriteLine($"Коэффициент постоянства состава персонала предприятия\nСЧР равен: -" +
                                 $"\nНачало периода подсчёта: -\nКонец периода подсчёта: -\n" +
                                 $"Рассматриваемая должность: -\nКоличество: -\n" +
                    $"Результат равен: -\n" + "================\n");
                        }
                    }
                    else
                    {
                        if (YearAgo3.Checked == true)
                            sw.WriteLine($"Коэффициент постоянства состава персонала предприятия\nСЧР равен: {SChRText3.Text}" +
                                 $"\nНачало периода подсчёта: {YearAgoText3.Text}\nКонец периода подсчёта: {dateTimePicker3.Value.ToLongDateString()}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox3.Text}\nКоличество: {amountKoef3.Text}\n" +
                $"Результат равен: {resultKoef3.Text}\n" + "================\n");
                        else
                            sw.WriteLine($"Коэффициент постоянства состава персонала предприятия\nСЧР равен: {SChRText3.Text}" +
                             $"\nНачало периода подсчёта: {dateTimePicker3.Value.ToLongDateString()}\nКонец периода подсчёта: {YearForwardText3.Text}\n" +
                             $"Рассматриваемая должность: {LevelComboBox3.Text}\nКоличество: {amountKoef3.Text}\n" +
                $"Результат равен: {resultKoef3.Text}\n" + "================\n");
                    }
                }
                sw.Close();
            }
            MessageBox.Show("Данные успешно сохранены в указанный файл!", "Уведомление",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);*/
        }
    }
}
