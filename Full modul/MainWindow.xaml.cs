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