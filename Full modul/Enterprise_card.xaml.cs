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
using System.Windows.Threading;

namespace Full_modul
{
    /// <summary>
    /// Логика взаимодействия для Enterprise_card.xaml
    /// </summary>
    public partial class Enterprise_card : Window
    {
        public Enterprise_card()
        {
            InitializeComponent();
            LoadCompanyData();
            this.KeyDown += Enterprise_card_KeyDown;
        }

        private void Enterprise_card_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CloseButton_Click(sender, e);
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

        private void LoadCompanyData()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            var stats = dbHelper.GetCompanyStatistics();

            CompanyNameText.Text = "\"Архимед\" Рекламная компания";
            TotalEmployeesText.Text = stats.TotalEmployees.ToString();

            var ageGenderList = new List<DatabaseHelper.AgeGenderGroup>
            {
                new DatabaseHelper.AgeGenderGroup { AgeGroup = "18-30", MaleCount = stats.AgeGroups[0], FemaleCount = stats.AgeGroups[1] },
                new DatabaseHelper.AgeGenderGroup { AgeGroup = "31-40", MaleCount = stats.AgeGroups[2], FemaleCount = stats.AgeGroups[3] },
                new DatabaseHelper.AgeGenderGroup { AgeGroup = "41-50", MaleCount = stats.AgeGroups[4], FemaleCount = stats.AgeGroups[5] },
                new DatabaseHelper.AgeGenderGroup { AgeGroup = "51-60", MaleCount = stats.AgeGroups[6], FemaleCount = stats.AgeGroups[7] },
                new DatabaseHelper.AgeGenderGroup { AgeGroup = "61-65", MaleCount = stats.AgeGroups[8], FemaleCount = stats.AgeGroups[9] },
                new DatabaseHelper.AgeGenderGroup { AgeGroup = "66+", MaleCount = stats.AgeGroups[10], FemaleCount = stats.AgeGroups[11] }
            };

            AgeGenderBreakdown.ItemsSource = ageGenderList;

            GenderCompositionText.Text = $"Мужчины: {stats.GenderCounts[0]}\nЖенщины: {stats.GenderCounts[1]}";

            var educationList = new Dictionary<string, int>
            {
                ["Среднее:"] = stats.EducationCounts[0],
                ["Специальное:"] = stats.EducationCounts[1],
                ["Неполное высшее:"] = stats.EducationCounts[2],
                ["Высшее:"] = stats.EducationCounts[3]
            };
            EducationBreakdown.ItemsSource = educationList;

            var experienceList = new Dictionary<string, int>
            {
                ["Менее 5 лет:"] = stats.ExperienceCounts[0],
                ["6-10 лет:"] = stats.ExperienceCounts[1],
                ["11-15 лет:"] = stats.ExperienceCounts[2],
                ["16 и более лет:"] = stats.ExperienceCounts[3]
            };
            ExperienceBreakdown.ItemsSource = experienceList;

            RetentionRateText.Text = $"{stats.RetentionRate:P2}";
        }
    }
}
