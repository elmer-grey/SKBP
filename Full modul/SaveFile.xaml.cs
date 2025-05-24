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
            ResetCheckboxes();
        }

        private void ResetCheckboxes()
        {
            CheckBox0.IsChecked = CheckBox1.IsChecked =
            CheckBox2.IsChecked = CheckBox3.IsChecked = false;

            Data.Check0 = Data.Check1 = Data.Check2 = Data.Check3 = false;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (!(CheckBox0.IsChecked == true || CheckBox1.IsChecked == true ||
                  CheckBox2.IsChecked == true || CheckBox3.IsChecked == true))
            {
                var errorDialog = new CustomMessage(
                    "Не выбрано ни одного вычисления для сохранения. Пожалуйста, сделайте выбор.",
                    "Ошибка",
                    new List<string> { "OK" });
                errorDialog.Owner = this;
                errorDialog.ShowDialog();
                return;
            }

            Data.Check0 = CheckBox0.IsChecked == true;
            Data.Check1 = CheckBox1.IsChecked == true;
            Data.Check2 = CheckBox2.IsChecked == true;
            Data.Check3 = CheckBox3.IsChecked == true;

            this.DialogResult = true;
            this.Close();
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            var infoDialog = new CustomMessage(
                "Сохранение файла было отменено.",
                "Информация",
                new List<string> { "OK" });
            infoDialog.Owner = this;
            infoDialog.ShowDialog();

            this.DialogResult = false;
            this.Close();
        }
    }
}