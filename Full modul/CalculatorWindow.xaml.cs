using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
            InitializeDateTextBoxes();
            DataContext = new ViewModel();
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

        // Вспомогательный метод для поиска дочернего элемента
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Закрываем выпадающие списки, если кликнули вне календаря
            foreach (var comboBox in FindVisualChildren<ComboBox>(this))
            {
                if (!IsMouseOverCalendar(e, comboBox) && comboBox.IsDropDownOpen)
                {
                    comboBox.IsDropDownOpen = false;
                }
            }
        }

        private bool IsMouseOverCalendar(MouseButtonEventArgs e, ComboBox comboBox)
        {
            // Получаем позицию курсора относительно окна
            var mousePosition = e.GetPosition(this);
            var popup = FindVisualChild<Popup>(comboBox);
            var calendar = FindVisualChild<Calendar>(popup);

            // Проверяем, находится ли курсор над календарем
            if (calendar != null)
            {
                var calendarPosition = calendar.PointToScreen(new Point(0, 0));
                return mousePosition.X >= calendarPosition.X && mousePosition.X <= calendarPosition.X + calendar.ActualWidth &&
                       mousePosition.Y >= calendarPosition.Y && mousePosition.Y <= calendarPosition.Y + calendar.ActualHeight;
            }

            return false;
        }

        // Метод для поиска всех визуальных элементов определенного типа
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
    }
}
