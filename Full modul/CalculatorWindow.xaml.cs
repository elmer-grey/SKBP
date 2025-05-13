using Full_modul;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Calendar = System.Windows.Controls.Calendar;
using Path = System.IO.Path;

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
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));
            koefgroup0.Visibility = Visibility.Collapsed;
            koefgroup1.Visibility = Visibility.Collapsed;
            koefgroup2.Visibility = Visibility.Collapsed;
            koefgroup3.Visibility = Visibility.Collapsed;
            this.Style = (Style)Application.Current.Resources["ComboBoxStyleCalendar"];
            this.Loaded += CalendarComboBox_Loaded;

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
            MessageBox.Show("Вывод справки!");
        }

        private void Button_Reports_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Отчёты", "Калькулятор");

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

        private void comboBoxFormules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isDateLaterSelected = false;
            isDateAgoSelected = false;
            //isLevelWorkerSelected = false;
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
                    if (AreValuesEnteredInKoefGroup(0))
                    {
                        isDateLaterSelected = true;
                        isDateAgoSelected = true;
                        //isLevelWorkerSelected = true;
                    }
                    break;
                case 1:
                    koefgroup1.Visibility = Visibility.Visible;
                    if (AreValuesEnteredInKoefGroup(1))
                    {
                        isDateLaterSelected = true;
                        isDateAgoSelected = true;
                        //isLevelWorkerSelected = true;
                    }
                    break;
                case 2:
                    koefgroup2.Visibility = Visibility.Visible;
                    if (AreValuesEnteredInKoefGroup(2))
                    {
                        isDateLaterSelected = true;
                        isDateAgoSelected = true;
                        //isLevelWorkerSelected = true;
                    }
                    break;
                case 3:
                    koefgroup3.Visibility = Visibility.Visible;
                    if (AreValuesEnteredInKoefGroup(3))
                    {
                        isDateLaterSelected = true;
                        isDateAgoSelected = true;
                        //isLevelWorkerSelected = true;
                    }
                    break;
                default:
                    break;
            }
        }

        private void ComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                Popup popup = FindVisualChild<Popup>(comboBox);
                if (popup != null)
                {
                    popup.IsOpen = !popup.IsOpen;
                }
            }
        }

        private DateTime defaultDateLater = new DateTime(2023, 1, 1);
        private DateTime defaultDateAgo = new DateTime(2022, 1, 1);

        private bool isDateLaterSelected = false;
        private bool isDateAgoSelected = false;

        //private bool isLevelWorkerSelected = false;

        private bool isHandlingComboBoxSelection = false;

        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isHandlingComboBoxSelection) return;
            isHandlingComboBoxSelection = true;

            Calendar calendar = sender as Calendar;
            if (calendar != null)
            {
                DateTime? selectedDate = calendar.SelectedDate;

                if (calendar.Tag != null)
                {
                    string textBoxName = calendar.Tag.ToString();

                    TextBox dateTextBox = FindName(textBoxName) as TextBox;

                    if (dateTextBox != null && selectedDate.HasValue)
                    {
                        dateTextBox.Text = selectedDate.Value.ToShortDateString();

                        if (textBoxName == "dateTextBoxLater0")
                        {
                            isDateLaterSelected = true;
                        }
                        else if (textBoxName == "dateTextBoxAgo0")
                        {
                            isDateAgoSelected = true;
                        }
                        else if (textBoxName == "dateTextBoxLater1")
                        {
                            isDateLaterSelected = true;
                        }
                        else if (textBoxName == "dateTextBoxAgo1")
                        {
                            isDateAgoSelected = true;
                        }
                        else if (textBoxName == "dateTextBoxLater2")
                        {
                            isDateLaterSelected = true;
                        }
                        else if (textBoxName == "dateTextBoxAgo2")
                        {
                            isDateAgoSelected = true;
                        }
                        else if (textBoxName == "dateTextBoxLater3")
                        {
                            isDateLaterSelected = true;
                        }
                        else if (textBoxName == "dateTextBoxAgo3")
                        {
                            isDateAgoSelected = true;
                        }
                    }
                }

                Popup popup = FindVisualChild<Popup>(calendar);
                if (popup != null)
                {
                    popup.IsOpen = false;
                }
            }

            //if (isDateLaterSelected && isDateAgoSelected && isLevelWorkerSelected)
            if (isDateLaterSelected && isDateAgoSelected)
            {
                switch (comboBoxFormules.SelectedIndex)
                {
                    case 0:
                        koef0();
                        break;
                    case 1:
                        koef1();
                        break;
                    case 2:
                        koef2();
                        break;
                    case 3:
                        koef3();
                        break;
                    default:
                        break;
                }
                SChR_calculation();
            }
            //ComboBox_SelectionChanged(sender, e);

            isHandlingComboBoxSelection = false;
        }

        private void CalendarComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var calendar = FindVisualChild<Calendar>(sender as ComboBox);
            if (calendar != null)
            {
                calendar.SelectedDatesChanged += Calendar_SelectedDateChanged;
            }
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            T child = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var childType = VisualTreeHelper.GetChild(parent, i);
                if (childType is T childOfType)
                {
                    child = childOfType;
                    break;
                }
                else
                {
                    child = FindVisualChild<T>(childType);
                    if (child != null) break;
                }
            }
            return child;
        }

        private void Calendar_MouseDown(object sender, MouseButtonEventArgs e)
        {
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

        //private void combobox_selectionchanged(object sender, selectionchangedeventargs e)
        //{
        //    if (ishandlingcomboboxselection) return;
        //    ishandlingcomboboxselection = true;

        //    combobox levelworker = sender as combobox;
        //    comboboxitem selecteditem = levelworker.selecteditem as comboboxitem;

        //    if (selecteditem != null)
        //    {
        //        string? selectedvalue = selecteditem.content?.tostring();
        //        levelworker.text = selectedvalue;

        //        islevelworkerselected = true;
        //    }

        //    int selectedgroupindex = comboboxformules.selectedindex;
        //    if (arevaluesenteredinkoefgroup(selectedgroupindex))
        //    {
        //        isdatelaterselected = true;
        //        isdateagoselected = true;
        //    }

        //    switch (comboboxformules.selectedindex)
        //    {
        //        case 0:
        //            level = levelworker0.selectedindex switch
        //            {
        //                0 => 1,
        //                1 => 2,
        //                2 => 3,
        //                _ => 0
        //            };

        //            koef0();
        //            schr_calculation();
        //            break;
        //        case 1:
        //            level = levelworker1.selectedindex switch
        //            {
        //                0 => 1,
        //                1 => 2,
        //                2 => 3,
        //                _ => 0
        //            };

        //            koef1();
        //            schr_calculation();
        //            break;
        //        case 2:
        //            level = levelworker2.selectedindex switch
        //            {
        //                0 => 1,
        //                1 => 2,
        //                2 => 3,
        //                _ => 0
        //            };

        //            koef2();
        //            schr_calculation();
        //            break;
        //        case 3:
        //            level = levelworker3.selectedindex switch
        //            {
        //                0 => 1,
        //                1 => 2,
        //                2 => 3,
        //                _ => 0
        //            };

        //            koef3();
        //            schr_calculation();
        //            break;
        //        default:
        //            throw new argumentexception("invalid formula index");
        //    }
        //    calendar_selecteddatechanged(sender, e);

        //    ishandlingcomboboxselection = false;
        //}

        private bool AreValuesEnteredInKoefGroup(int groupIndex)
        {
            switch (groupIndex)
            {
                case 0:
                    return !string.IsNullOrEmpty(count0.Text) && !string.IsNullOrEmpty(dateTextBoxLater0.Text) && !string.IsNullOrEmpty(dateTextBoxAgo0.Text);
                case 1:
                    return !string.IsNullOrEmpty(count1.Text) && !string.IsNullOrEmpty(dateTextBoxLater1.Text) && !string.IsNullOrEmpty(dateTextBoxAgo1.Text);
                case 2:
                    return !string.IsNullOrEmpty(count2.Text) && !string.IsNullOrEmpty(dateTextBoxLater2.Text) && !string.IsNullOrEmpty(dateTextBoxAgo2.Text);
                case 3:
                    return !string.IsNullOrEmpty(count3.Text) && !string.IsNullOrEmpty(dateTextBoxLater3.Text) && !string.IsNullOrEmpty(dateTextBoxAgo3.Text);
                default:
                    return false;
            }
        }

        public void ExportButton_Click(object sender, EventArgs e)
        {            
            ExportData();
        }
        
        public void ExportData()
        {           
            var result = MessageBox.Show("Вы запустили процедуру сохранения данных.\nЕсли Вы хотите сохранить только данные вычисления, то нажмите \"Да\"\n" +
                "Если хотите сохранить несколько вычислений, то нажмите \"Нет\"", "Процедура сохранения", MessageBoxButton.YesNo, MessageBoxImage.Question);
            SaveFileWindowChoise saveFileWindow = new SaveFileWindowChoise(this, null, "CalculatorWindow");
            if (result == MessageBoxResult.No)
            {
                SaveFile SF = new SaveFile();
                SF.ShowDialog();
                if (Data.Check0 == false && Data.Check1 == false && Data.Check2 == false && Data.Check3 == false)
                {
                    MessageBox.Show("Сохранение файла было отменено пользователем!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                } else
                {
                    Data.SaveFile = 0;
                    saveFileWindow.ShowDialog();
                }
            }
            else if (result == MessageBoxResult.Yes)
            {
                Data.BoxIndex = comboBoxFormules.SelectedIndex;
                saveFileWindow.ShowDialog();
            }
        }

        public bool[] _warningsShown = new bool[4];

        public string SaveData(int selected)
        {
            string formulaName = GetFormulaName(selected);
            //string levelText = GetLevelText(selected);
            string countText = GetCountText(selected);
            string SCHR = GetSCHR(selected);
            string result = GetResultText(selected);
            _warningsShown[selected] = false;

            if (string.IsNullOrEmpty(result))
            {
                if (!_warningsShown[selected])
                {
                    var warningMessage = MessageBox.Show($"Вы не заполнили формулу '{formulaName}'! Продолжить?",
                        "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    _warningsShown[selected] = true;

                    if (warningMessage == MessageBoxResult.Cancel)
                    {
                        return string.Empty;
                    }
                    return $"{formulaName}\nНачало периода подсчёта: -\nКонец периода подсчёта: -\n" +
                           $"Количество: -\nСЧР: -\nРезультат: -\n================\n";
                }
            }
            else
            {
                _warningsShown[selected] = false;
                return $"{formulaName}\nНачало периода подсчёта: {startDate}\n" +
                       $"Конец периода подсчёта: {endDate}\n" +
                       $"Количество: {countText}\n" +
                       $"СЧР: {SCHR}\nРезультат: {result}\n================\n";
            }
            return string.Empty;
        }

        private string GetFormulaName(int selected)
        {
            switch (selected)
            {
                case 0: return "Коэффициент оборота по приему";
                case 1: return "Коэффициент оборота по выбытию";
                case 2: return "Коэффициент текучести кадров";
                case 3: return "Коэффициент постоянства состава персонала предприятия";
                default: return "Неизвестная формула";
            }
        }

        //private string GetLevelText(int selected)
        //{
        //    switch (selected)
        //    {
        //        case 0: return levelworker0.Text;
        //        case 1: return levelworker1.Text;
        //        case 2: return levelworker2.Text;
        //        case 3: return levelworker3.Text;
        //        default: return "";
        //    }
        //}

        private string GetCountText(int selected)
        {
            switch (selected)
            {
                case 0: return count0.Text;
                case 1: return count1.Text;
                case 2: return count2.Text;
                case 3: return count3.Text;
                default: return "";
            }
        }

        private string GetSCHR(int selected)
        {
            switch (selected)
            {
                case 0: return SCHR0.Text;
                case 1: return SCHR1.Text;
                case 2: return SCHR2.Text;
                case 3: return SCHR3.Text;
                default: return "";
            }
        }

        private string GetResultText(int selected)
        {
            switch (selected)
            {
                case 0: return result0.Text;
                case 1: return result1.Text;
                case 2: return result2.Text;
                case 3: return result3.Text;
                default: return "";
            }
        }

        public static int id = -1;
        //public static int level = -1;
        public static double SCHR = 0;
        public static string startDate, endDate;
        public static string command = ""; 

        public void koef3()
        {
            DateTime DateLater;
            DateTime DateAgo;

            if (!DateTime.TryParse(dateTextBoxLater3.Text, out DateLater) || !isDateLaterSelected)
            {
                DateLater = defaultDateLater;
            }

            if (!DateTime.TryParse(dateTextBoxAgo3.Text, out DateAgo) || !isDateAgoSelected)
            {
                DateAgo = defaultDateAgo;
            }

            if (isDateLaterSelected && isDateAgoSelected)
            {
                id = DateLater.Date > DateAgo.Date ? 0 : 1;
                if (id == 0)
                {
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker]" +
                        $"WHERE start_date < @DateAgo AND(end_date > @DateLater OR end_date IS NULL)";// + $"AND level_worker = @LevelWorker"
                }
                else if (id == 1)
                {                    
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker]" +
                        $"WHERE start_date < @DateLater AND(end_date > @DateAgo OR end_date IS NULL)";
                }

                using (var sqlCommand = new SqlCommand(command, DatabaseConnection.Instance.Connection))
                {
                    sqlCommand.Parameters.AddWithValue("@DateAgo", DateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", DateLater);
                    //sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

                    int count = (int)sqlCommand.ExecuteScalar();
                    count3.Text = count.ToString();
                    result();
                }
            }
        }

        public void koef2()
        {
            DateTime DateLater;
            DateTime DateAgo;

            if (!DateTime.TryParse(dateTextBoxLater2.Text, out DateLater) || !isDateLaterSelected)
            {
                DateLater = defaultDateLater;
            }

            if (!DateTime.TryParse(dateTextBoxAgo2.Text, out DateAgo) || !isDateAgoSelected)
            {
                DateAgo = defaultDateAgo;
            }

            if (isDateLaterSelected && isDateAgoSelected)
            {
                id = DateLater.Date > DateAgo.Date ? 0 : 1;
                if (id == 0)
                {
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker] " +
                        $"WHERE end_date BETWEEN @DateAgo AND @DateLater " +
                        $"AND id_dismissal_reason IN(1, 4)";// AND level_worker = @LevelWorker
                }
                else if (id == 1)
                {
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker] " +
                        $"WHERE end_date BETWEEN @DateLater AND @DateAgo " +
                        $"AND id_dismissal_reason IN(1, 4)";
                }

                using (var sqlCommand = new SqlCommand(command, DatabaseConnection.Instance.Connection))
                {
                    sqlCommand.Parameters.AddWithValue("@DateAgo", DateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", DateLater);
                    //sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

                    int count = (int)sqlCommand.ExecuteScalar();
                    count2.Text = count.ToString();
                    result();
                }
            }
        }

        public void koef1()
        {
            DateTime DateLater;
            DateTime DateAgo;

            if (!DateTime.TryParse(dateTextBoxLater1.Text, out DateLater) || !isDateLaterSelected)
            {
                DateLater = defaultDateLater;
            }

            if (!DateTime.TryParse(dateTextBoxAgo1.Text, out DateAgo) || !isDateAgoSelected)
            {
                DateAgo = defaultDateAgo;
            }

            if (isDateLaterSelected && isDateAgoSelected)
            {
                id = DateLater.Date > DateAgo.Date ? 0 : 1;
                if (id == 0)
                {
                    command = $"SELECT COUNT(*) FROM [calculator].[dbo].[worker]" +
                        $"WHERE end_date BETWEEN @DateAgo AND @DateLater" +
                        $"AND id_dismissal_reason IN (2, 3, 5)";// AND level_worker = @LevelWorker
                }
                else if (id == 1)
                {
                    command = $"SELECT COUNT(*) FROM [calculator].[dbo].[worker]" +
                        $"WHERE end_date BETWEEN @DateLater AND @DateAgo" +
                        $"AND id_dismissal_reason IN (2, 3, 5)";
                }

                using (var sqlCommand = new SqlCommand(command, DatabaseConnection.Instance.Connection))
                {
                    sqlCommand.Parameters.AddWithValue("@DateAgo", DateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", DateLater);
                    //sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

                    int count = (int)sqlCommand.ExecuteScalar();
                    count1.Text = count.ToString();
                    result();
                }
            }
        }

        public void koef0()
        {
            DateTime DateLater;
            DateTime DateAgo;

            if (!DateTime.TryParse(dateTextBoxLater0.Text, out DateLater) || !isDateLaterSelected)
            {
                DateLater = defaultDateLater;
            }

            if (!DateTime.TryParse(dateTextBoxAgo0.Text, out DateAgo) || !isDateAgoSelected)
            {
                DateAgo = defaultDateAgo;
            }

            if (isDateLaterSelected && isDateAgoSelected)
            {
                id = DateLater.Date > DateAgo.Date ? 0 : 1;
                if (id == 0)
                {
                    command = $"SELECT COUNT(*) FROM [calculator].[dbo].[worker] WHERE start_date BETWEEN @DateAgo AND @DateLater";//+ $" AND level_worker = @LevelWorker"
                }
                else if (id == 1)
                {
                    command = $"SELECT COUNT(*) FROM [calculator].[dbo].[worker] WHERE start_date BETWEEN @DateLater AND @DateAgo";
                }

                using (var sqlCommand = new SqlCommand(command, DatabaseConnection.Instance.Connection))
                {
                    sqlCommand.Parameters.AddWithValue("@DateAgo", DateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", DateLater);
                    //sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

                    int count = (int)sqlCommand.ExecuteScalar();
                    count0.Text = count.ToString();
                    result();
                }
            }
        }

        private void BuildQuery(int id, ref string query, int selectedIndex)
        {
            DateTime dateLater, dateAgo;

            switch (selectedIndex)
            {
                case 0:
                    DateTime.TryParse(dateTextBoxLater0.Text, out dateLater);
                    DateTime.TryParse(dateTextBoxAgo0.Text, out dateAgo);
                    break;
                case 1:
                    DateTime.TryParse(dateTextBoxLater1.Text, out dateLater);
                    DateTime.TryParse(dateTextBoxAgo1.Text, out dateAgo);
                    break;
                case 2:
                    DateTime.TryParse(dateTextBoxLater2.Text, out dateLater);
                    DateTime.TryParse(dateTextBoxAgo2.Text, out dateAgo);
                    break;
                case 3:
                    DateTime.TryParse(dateTextBoxLater3.Text, out dateLater);
                    DateTime.TryParse(dateTextBoxAgo3.Text, out dateAgo);
                    break;
                default:
                    throw new ArgumentException("Invalid formula index");
            }

            startDate = id == 0 ? dateAgo.ToString("yyyy-MM-dd") : dateLater.ToString("yyyy-MM-dd");
            endDate = id == 0 ? dateLater.ToString("yyyy-MM-dd") : dateAgo.ToString("yyyy-MM-dd");

            query += $"DECLARE @StartDate DATE = '{startDate}'; " +
                      $"DECLARE @EndDate DATE = '{endDate}';";
        }

        public void SChR_calculation()
        {
            if (isDateLaterSelected && isDateAgoSelected) //if (isDateLaterSelected && isDateAgoSelected && isLevelWorkerSelected)
            {                
                string query = "";
                BuildQuery(id, ref query, comboBoxFormules.SelectedIndex);

                query += $@"SELECT 
        SUM(
            DATEDIFF(DAY,
                CASE 
                    WHEN start_date < @StartDate THEN @StartDate 
                    ELSE start_date 
                END,
                CASE 
                    WHEN end_date IS NULL THEN @EndDate
                    WHEN end_date > @EndDate THEN @EndDate
                    ELSE end_date
                END
            ) + 1
        ) / 
        CAST(DATEDIFF(DAY, @StartDate, @EndDate) + 1 AS FLOAT) AS AverageHeadcount
    FROM 
        [calculator].[dbo].[worker]
    WHERE
        start_date <= @EndDate AND 
        (end_date >= @StartDate OR end_date IS NULL);";

                using (var command = new SqlCommand(query, DatabaseConnection.Instance.Connection))
                {
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader.IsDBNull(reader.GetOrdinal("AverageHeadcount")))
                                {
                                    SCHR = 0;
                                }
                                else
                                {
                                    // Получаем уже рассчитанное среднее значение из запроса
                                    SCHR = Math.Round(Convert.ToDouble(reader["AverageHeadcount"]), 8);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
                        SCHR = 0;
                    }
                }

                switch (comboBoxFormules.SelectedIndex)
                {
                    case 0:
                        SCHR0.Text = SCHR.ToString();
                        break;
                    case 1:
                        SCHR1.Text = SCHR.ToString();
                        break;
                    case 2:
                        SCHR2.Text = SCHR.ToString();
                        break;
                    case 3:
                        SCHR3.Text = SCHR.ToString();
                        break;
                    default:
                        break;
                }
                result();
            }
        }

        public void result()
        {
            if (SCHR == 0)
            {
                switch (comboBoxFormules.SelectedIndex)
                {
                    case 0:
                        SetResultButtonAndStyle(0, result0, Save0);
                        break;
                    case 1:
                        SetResultButtonAndStyle(1, result1, Save1);
                        break;
                    case 2:
                        SetResultButtonAndStyle(2, result2, Save2);
                        break;
                    case 3:
                        SetResultButtonAndStyle(3, result3, Save3);
                        break;
                    default:
                        break;
                }                
                return;
            }

            switch (comboBoxFormules.SelectedIndex)
            {
                case 0:
                    //CalculateResult(count0, result0, levelworker0, Save0);
                    CalculateResult(count0, result0, Save0);
                    break;
                case 1:
                    CalculateResult(count1, result1, Save1);
                    break;
                case 2:
                    CalculateResult(count2, result2, Save2);
                    break;
                case 3:
                    CalculateResult(count3, result3, Save3);
                    break;
                default:
                    break;
            }
        }

        private void SetResultButtonAndStyle(int caseIndex, TextBox resultTextBox, Button button)
        {
            resultTextBox.Text = "0";
            UpdateTextBoxButtonStyle(resultTextBox, button);
        }

        //private void CalculateResult(TextBox countTextBox, TextBox resultTextBox, ComboBox levelWorkerComboBox, Button button)
        private void CalculateResult(TextBox countTextBox, TextBox resultTextBox, Button button)
        {
            double factor = 0;

            //switch (levelWorkerComboBox.SelectedIndex)
            //{
            //    case 0:
            //        factor = 0.2;
            //        break;
            //    case 1:
            //    case 2:
            //        factor = 0.8;
            //        break;
            //    default:
            //        return;
            //}

            resultTextBox.Text = (Double.Parse(countTextBox.Text) / SCHR * 1).ToString();
            resultTextBox.Text = Math.Round(Convert.ToDouble(resultTextBox.Text), 8).ToString();
            UpdateTextBoxButtonStyle(resultTextBox, button);
        }

        private void UpdateTextBoxButtonStyle(TextBox resultTextBox, Button button)
        {
            if (!string.IsNullOrWhiteSpace(resultTextBox.Text))
            {
                button.IsEnabled = true;
            }
            else
            {
                button.IsEnabled = false;
            }
        }
    }
}
