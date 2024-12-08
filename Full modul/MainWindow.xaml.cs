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
        public MainWindow()
        {

            InitializeComponent();
            this.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/HR1.ico"));
        }        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Картинка и текст были нажаты!");
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Позволяет перетаскивать окно
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
            this.Close(); // Закрыть окно
        }
    }
}