using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для OrganizAndLegalConditWindow.xaml
    /// </summary>
    public partial class OrganizAndLegalConditWindow : Window
    {
        public OrganizAndLegalConditWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));

            string query = "SELECT REPLACE(LTRIM(RTRIM(COALESCE(lastname_hr, '') + ' ' + COALESCE(name_hr, '') + ' ' + COALESCE(midname_hr, ''))), '  ', ' ') AS FullName FROM [calculator].[dbo].[hr] WHERE login_hr = @login";

            try
            {
                string fullName = DatabaseConnection.Instance.ExecuteScalar<string>(
                    query,
                    new SqlParameter("@login", UserInfo.username)
                );

                user.Text = !string.IsNullOrEmpty(fullName) ? fullName.Trim() : "Администратор";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных пользователя: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void Button_Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Здесь будет справка!");
        }

        private void Button_Reports_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Отчёты", "Условия");

            if (Directory.Exists(folderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = folderPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            else
            {
                MessageBox.Show("Папка не найдена: " + folderPath, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.IsChecked == true)
            {
                int index = int.Parse(radioButton.Tag.ToString());
                HandleFirstContainer(index, radioButton.Content.ToString());

                // Обновляем результаты
                var radioGroups1 = new[] {
            (Rb0_Yes, Rb0_No),
            (Rb1_Yes, Rb1_No),
            // ... все остальные пары RadioButton
        };
                CalculateAverage(radioGroups1, result0, ClickableImage2);

                UpdateSecondContainerResults();
            }
        }

        private void HandleFirstContainer(int index, string selectedValue)
        {
            if (index == 1) // Для первого RadioButton, который влияет на Grid10
            {
                Grid10.Visibility = selectedValue == "Нет" ? Visibility.Collapsed : Visibility.Visible;
            }

            if (index == 4) // Для четвертого RadioButton, который влияет на Grid13
            {
                Grid13.Visibility = selectedValue == "Нет" ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void CalculateAverage((RadioButton yes, RadioButton no)[] radioGroups, TextBox resultText, Image clickableImage)
        {
            int sum = 0;
            int count = 0;
            bool allSelected = true;

            foreach (var (yesRb, noRb) in radioGroups)
            {
                if (yesRb.IsChecked == true)
                {
                    sum += 1;
                    count++;
                }
                else if (noRb.IsChecked == true)
                {
                    count++;
                }
                else
                {
                    allSelected = false;
                    break;
                }
            }

            if (allSelected && count > 0)
            {
                double average = (double)sum / count;
                resultText.Text = average.ToString("0.#####");

                if (clickableImage != null && resultText.Text != string.Empty)
                {
                    clickableImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Arrow_Blue0.png"));
                }
            }
            else
            {
                resultText.Text = string.Empty;
            }
        }

        private void UpdateSecondContainerResults()
        {
            int sum = 0;
            int count = 0;
            int visibleCount = 0;
            bool isAnyGridVisible = false;

            for (int i = 9; i <= 15; i++)
            {
                var grid = (Grid)this.FindName($"Grid{i}");
                var yesRb = (RadioButton)this.FindName($"Rb{i}_Yes");
                var noRb = (RadioButton)this.FindName($"Rb{i}_No");

                if (grid.Visibility == Visibility.Visible)
                {
                    isAnyGridVisible = true;
                    visibleCount++;

                    if (yesRb.IsChecked == true)
                    {
                        sum += 1;
                        count++;
                    }
                    else if (noRb.IsChecked == true)
                    {
                        count++;
                    }
                }
            }

            if (isAnyGridVisible && count == visibleCount)
            {
                double average = (double)sum / visibleCount;
                result1.Text = average.ToString("0.######");
                Save0.IsEnabled = true;
            }
            else
            {
                result1.Text = string.Empty;
                Save0.IsEnabled = false;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= 25; i++)
            {
                var yesRb = (RadioButton)this.FindName($"Rb{i}_Yes");
                var noRb = (RadioButton)this.FindName($"Rb{i}_No");
                if (yesRb != null) yesRb.IsChecked = false;
                if (noRb != null) noRb.IsChecked = false;
            }
        }
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= 25; i++)
            {
                var yesRb = (RadioButton)this.FindName($"Rb{i}_Yes");
                var noRb = (RadioButton)this.FindName($"Rb{i}_No");
                if (yesRb != null) yesRb.IsChecked = true;
                if (noRb != null) noRb.IsChecked = false;
            }
        }
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= 25; i++)
            {
                var yesRb = (RadioButton)this.FindName($"Rb{i}_Yes");
                var noRb = (RadioButton)this.FindName($"Rb{i}_No");
                if (yesRb != null) yesRb.IsChecked = false;
                if (noRb != null) noRb.IsChecked = true;
            }
        }
        //private void UpdatePlaceholderVisibility(ComboBox comboBox)
        //{
        //    var grid = (Grid)comboBox.Parent;
        //    var placeholderText = (TextBlock)grid.Children[1];

        //    if (comboBox.SelectedItem is ComboBoxItem selectedItem)
        //    {
        //        placeholderText.Visibility = selectedItem.Content.ToString() != "-" ? Visibility.Collapsed : Visibility.Visible;
        //    }
        //}

        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (sender is ComboBox comboBox)
        //    {
        //        UpdatePlaceholderVisibility(comboBox);
        //        HandleFirstContainer(comboBox);

        //        var comboBoxes1 = new[] { Box0, Box1, Box2, Box3, Box4, Box5, Box6 };
        //        CalculateAverage(comboBoxes1, result0, ClickableImage2);

        //        UpdateSecondContainerResults();
        //    }
        //}

        //private void HandleFirstContainer(ComboBox comboBox)
        //{
        //    if (comboBox.SelectedItem is ComboBoxItem selectedItem)
        //    {
        //        string selectedValue = selectedItem.Content.ToString();

        //        if (comboBox == Box1)
        //        {
        //            Grid10.Visibility = selectedValue == "Нет" ? Visibility.Collapsed : Visibility.Visible;
        //        }

        //        if (comboBox == Box4)
        //        {
        //            Grid13.Visibility = selectedValue == "Нет" ? Visibility.Collapsed : Visibility.Visible;
        //        }
        //    }
        //}

        //private void UpdateSecondContainerResults()
        //{
        //    int sum = 0;
        //    int count = 0;
        //    int visibleCount = 0;
        //    bool isAnyGridVisible = false;

        //    for (int i = 9; i <= 15; i++)
        //    {
        //        var comboBox = (ComboBox)this.FindName($"Box{i}");
        //        var Grid = (Grid)this.FindName($"Grid{i}");

        //        if (Grid.Visibility == Visibility.Visible)
        //        {
        //            isAnyGridVisible = true;
        //            visibleCount++;

        //            if (comboBox.SelectedItem is ComboBoxItem selectedItem)
        //            {
        //                if (selectedItem.Content.ToString() == "Да")
        //                {
        //                    sum += 1;
        //                }
        //                count++;
        //            }
        //        }
        //    }

        //    if (isAnyGridVisible && count == visibleCount)
        //    {
        //        double average = (double)sum / visibleCount; 
        //        result1.Text = average.ToString("0.######");
        //        Save0.IsEnabled = true;
        //    }
        //    else
        //    {
        //        result1.Text = string.Empty;
        //        Save0.IsEnabled = false;
        //    }
        //}

        //private void CalculateAverage(ComboBox[] comboBoxes, TextBox resultText, Image clickableImage)
        //{
        //    int sum = 0;
        //    int count = 0;
        //    bool allSelected = true;

        //    foreach (var comboBox in comboBoxes)
        //    {
        //        if (comboBox.SelectedItem is ComboBoxItem selectedItem)
        //        {
        //            if (selectedItem.Content.ToString() == "Да")
        //            {
        //                sum += 1;
        //            }
        //            count++;
        //        }
        //        else
        //        {
        //            allSelected = false;
        //            break;
        //        }
        //    }

        //    if (allSelected && count > 0)
        //    {
        //        double average = (double)sum / count;
        //        resultText.Text = average.ToString("0.#####");

        //        if (clickableImage != null && resultText.Text != string.Empty)
        //        {
        //            clickableImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Arrow_Blue0.png"));
        //        }
        //    }
        //    else
        //    {
        //        resultText.Text = string.Empty;
        //    }
        //}

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContainerLegal.Visibility == Visibility.Visible)
            {
                if (result0.Text != string.Empty)
                {
                    ContainerLegal.Visibility = Visibility.Collapsed;
                    ContainerOrgan.Visibility = Visibility.Visible;
                    UpdateSecondContainerResults();
                }
            }
            else
            {
                ContainerLegal.Visibility = Visibility.Visible;
                ContainerOrgan.Visibility = Visibility.Collapsed;
            }
        }

        private void ClickableImage_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(result0.Text))
            {
                Mouse.OverrideCursor = Cursors.Hand;
                ClickableImage2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Arrow_Blue0_Light.png"));
                ClickableImage3.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Arrow_Blue0_Light.png"));
            }
        }

        private void ClickableImage_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(result0.Text))
            {
                Mouse.OverrideCursor = null;
                ClickableImage2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Arrow_Blue0.png"));
                ClickableImage3.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Arrow_Blue0.png"));
            }
        }

        public void ExportButton_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        public void ExportData()
        {
            SaveFileWindowChoise saveFileWindow = new SaveFileWindowChoise(null, this, "OrganizAndLegalConditWindow");
            saveFileWindow.ShowDialog();
        }

        public string SaveData()
        {
            if (!string.IsNullOrEmpty(result0.Text) && !string.IsNullOrEmpty(result1.Text))
            {
                return $"Правовые условия\nПолученный коэффициент: {result0.Text}\n\n" +
                    $"Организационные условия\nПолученный коэффициент: {result1.Text}\n================\n";
            }
            return string.Empty;
        }
    }
}
