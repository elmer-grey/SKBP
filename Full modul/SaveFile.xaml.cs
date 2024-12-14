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
    /// Логика взаимодействия для SaveFile.xaml
    /// </summary>
    public partial class SaveFile : Window
    {
        public SaveFile()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            Data.Check0 = CheckBox0.IsChecked == true;
            Data.Check1 = CheckBox1.IsChecked == true;
            Data.Check2 = CheckBox2.IsChecked == true;
            Data.Check3 = CheckBox3.IsChecked == true;
            Close();
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            CheckBox0.IsChecked = false;
            CheckBox1.IsChecked = false;
            CheckBox2.IsChecked = false;
            CheckBox3.IsChecked = false;
        }
    }
}