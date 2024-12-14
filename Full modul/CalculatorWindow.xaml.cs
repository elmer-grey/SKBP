using Full_modul;
using Microsoft.Data.SqlClient;
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

            using (SqlConnection connection = new SqlConnection(Constants.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@login", UserInfo.username);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        user.Text = result.ToString().Trim();
                    }
                    else
                    {
                        user.Text = "Царь и Бог";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }
    
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Картинка и текст были нажаты!");
        }

        private void Button_Reports_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Отчёты", "Калькулятор");
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
            isLevelWorkerSelected = false;
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
                        isLevelWorkerSelected = true;
                    }
                    break;
                case 1:
                    koefgroup1.Visibility = Visibility.Visible;
                    if (AreValuesEnteredInKoefGroup(1))
                    {
                        isDateLaterSelected = true;
                        isDateAgoSelected = true;
                        isLevelWorkerSelected = true;
                    }
                    break;
                case 2:
                    koefgroup2.Visibility = Visibility.Visible;
                    if (AreValuesEnteredInKoefGroup(2))
                    {
                        isDateLaterSelected = true;
                        isDateAgoSelected = true;
                        isLevelWorkerSelected = true;
                    }
                    break;
                case 3:
                    koefgroup3.Visibility = Visibility.Visible;
                    if (AreValuesEnteredInKoefGroup(3))
                    {
                        isDateLaterSelected = true;
                        isDateAgoSelected = true;
                        isLevelWorkerSelected = true;
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

        private bool isLevelWorkerSelected = false;

        private bool isHandlingComboBoxSelection = false;
        private bool isHandlingCalendarSelection = false;

        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isHandlingComboBoxSelection) return; // Предотвращаем повторный вызов
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

            if (isDateLaterSelected && isDateAgoSelected && isLevelWorkerSelected)
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
            ComboBox_SelectionChanged(sender, e);

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
            if (isHandlingComboBoxSelection) return; 
            isHandlingComboBoxSelection = true;

            ComboBox levelworker = sender as ComboBox;
            ComboBoxItem selectedItem = levelworker.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                string? selectedValue = selectedItem.Content?.ToString();
                levelworker.Text = selectedValue;

                isLevelWorkerSelected = true;
            }

            int selectedGroupIndex = comboBoxFormules.SelectedIndex;
            if (AreValuesEnteredInKoefGroup(selectedGroupIndex))
            {
                isDateLaterSelected = true;
                isDateAgoSelected = true;
            }

            switch (comboBoxFormules.SelectedIndex)
            {
                case 0:
                    level = levelworker0.SelectedIndex switch
                    {
                        0 => 1,
                        1 => 2,
                        2 => 3,
                        _ => 0
                    };

                    koef0();
                    SChR_calculation();
                    break;
                case 1:
                    level = levelworker1.SelectedIndex switch
                    {
                        0 => 1,
                        1 => 2,
                        2 => 3,
                        _ => 0
                    };

                    koef1();
                    SChR_calculation();
                    break;
                case 2:
                    level = levelworker2.SelectedIndex switch
                    {
                        0 => 1,
                        1 => 2,
                        2 => 3,
                        _ => 0
                    };

                    koef2();
                    SChR_calculation();
                    break;
                case 3:
                    level = levelworker3.SelectedIndex switch
                    {
                        0 => 1,
                        1 => 2,
                        2 => 3,
                        _ => 0
                    };

                    koef3();
                    SChR_calculation();
                    break;
                default:
                    throw new ArgumentException("Invalid formula index");
            }
            Calendar_SelectedDateChanged(sender, e);

            isHandlingComboBoxSelection = false;
        }

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
            var result = MessageBox.Show("Вы запустили процедуру сохранения данных.\nЕсли Вы хотите сохранить только данные вычиления, то нажмите \"Да\"\n" +
                "Если хотите сохранить несколько вычислений, то нажмите \"Нет\"", "Процедура сохранения", MessageBoxButton.YesNo, MessageBoxImage.Question);
            SaveFileWindowChoise saveFileWindow = new SaveFileWindowChoise(this, "CalculatorWindow");
            if (result == MessageBoxResult.No)
            {
                SaveFile SF = new SaveFile();
            }
            else if (result == MessageBoxResult.Yes)
            {
                Data.BoxIndex = comboBoxFormules.SelectedIndex;
                saveFileWindow.ShowDialog();
            }
            // или "Условия"

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

        public string SaveData(int selected)
        {
            string formulaName = GetFormulaName(selected);
            string levelText = GetLevelText(selected);
            string countText = GetCountText(selected);
            string SCHR = GetSCHR(selected);
            string result = GetResultText(selected);

            return $"{formulaName}\nНачало периода: {startDate}\n" +
                   $"Конец периода: {endDate}\n" +
                   $"Должность: {levelText}\nКоличество: {countText}\n" +
                   $"СЧР: {SCHR}\nРезультат: {result}\n================\n";
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

        private string GetLevelText(int selected)
        {
            switch (selected)
            {
                case 0: return levelworker0.Text;
                case 1: return levelworker1.Text;
                case 2: return levelworker2.Text;
                case 3: return levelworker3.Text;
                default: return "";
            }
        }

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
        public static int level = -1;
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
                        $"WHERE startwork_worker < @DateAgo AND(endwork_worker > @DateLater OR endwork_worker = '1900-01-01')" +
                        $"AND level_worker = @LevelWorker";
                }
                else if (id == 1)
                {                    
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker]" +
                        $"WHERE startwork_worker < @DateAgo AND(endwork_worker > @DateLater OR endwork_worker = '1900-01-01')" +
                        $"AND level_worker = @LevelWorker";
                }

                using (var sqlCommand = new SqlCommand(command, DatabaseConnection.Instance.Connection))
                {
                    sqlCommand.Parameters.AddWithValue("@DateAgo", DateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", DateLater);
                    sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

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
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker] WHERE (startwork_worker < @DateAgo OR startwork_worker > @DateAgo) " +
                        $"AND endwork_worker < @DateLater AND fire_worker = 'По собственному желанию' AND level_worker = @LevelWorker";
                }
                else if (id == 1)
                {
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker] WHERE (startwork_worker < @DateLater OR startwork_worker > @DateLater) " +
                        $"AND endwork_worker < @DateAgo AND fire_worker = 'По собственному желанию' AND level_worker = @LevelWorker";
                }

                using (var sqlCommand = new SqlCommand(command, DatabaseConnection.Instance.Connection))
                {
                    sqlCommand.Parameters.AddWithValue("@DateAgo", DateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", DateLater);
                    sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

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
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker] " +
                        $"WHERE startwork_worker < @DateAgo AND (endwork_worker > @DateLater OR endwork_worker = '1900-01-01') " +
                        $"AND (fire_worker = 'Увольнение' OR fire_worker = 'Выход на пенсию') AND level_worker = @LevelWorker";
                }
                else if (id == 1)
                {
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker] " +
                        $"WHERE startwork_worker < @DateLater AND (endwork_worker > @DateAgo OR endwork_worker = '1900-01-01') " +
                        $"AND (fire_worker = 'Увольнение' OR fire_worker = 'Выход на пенсию') AND level_worker = @LevelWorker";
                }

                using (var sqlCommand = new SqlCommand(command, DatabaseConnection.Instance.Connection))
                {
                    sqlCommand.Parameters.AddWithValue("@DateAgo", DateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", DateLater);
                    sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

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
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker] WHERE startwork_worker > @DateAgo AND startwork_worker < @DateLater" +
                    $" AND level_worker = @LevelWorker";
                }
                else if (id == 1)
                {
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker] WHERE startwork_worker > @DateLater AND startwork_worker < @DateAgo" +
                    $" AND level_worker = @LevelWorker";
                }

                using (var sqlCommand = new SqlCommand(command, DatabaseConnection.Instance.Connection))
                {
                    sqlCommand.Parameters.AddWithValue("@DateAgo", DateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", DateLater);
                    sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

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
            if (isDateLaterSelected && isDateAgoSelected && isLevelWorkerSelected)
            {                
                string query = "";
                BuildQuery(id, ref query, comboBoxFormules.SelectedIndex);

                query += $@"; WITH DateRangeCTE AS(SELECT @StartDate AS DateValue            
UNION ALL            
SELECT DATEADD(day, 1, DateValue)
FROM DateRangeCTE            
WHERE DateValue < @EndDate),
Employees AS(
    SELECT id_worker, position_worker, startwork_worker, 
    CASE WHEN endwork_worker = '1900-01-01' THEN @EndDate ELSE endwork_worker END AS ActualEndDate
    FROM [calculator].[dbo].[worker]
    WHERE startwork_worker <= @EndDate AND level_worker = '{level}'
),
DailyCounts AS(
    SELECT DateValue, position_worker, COUNT(id_worker) AS DailyCount
    FROM DateRangeCTE
    JOIN Employees ON DateValue BETWEEN Employees.startwork_worker AND Employees.ActualEndDate
    GROUP BY DateValue, position_worker
)
SELECT SUM(DailyCount) AS TotalCount
FROM DailyCounts
OPTION(MAXRECURSION 0);";

                using (var command = new SqlCommand(query, DatabaseConnection.Instance.Connection))
                {
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader.IsDBNull(reader.GetOrdinal("TotalCount")))
                                {
                                    SCHR = 0;
                                }
                                else
                                {                                    
                                    SCHR = Math.Round(Convert.ToDouble(reader["TotalCount"]) / 365, 8);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
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
                    CalculateResult(count0, result0, levelworker0, Save0);
                    break;
                case 1:
                    CalculateResult(count1, result1, levelworker1, Save1);
                    break;
                case 2:
                    CalculateResult(count2, result2, levelworker2, Save2);
                    break;
                case 3:
                    CalculateResult(count3, result3, levelworker3, Save3);
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

        private void CalculateResult(TextBox countTextBox, TextBox resultTextBox, ComboBox levelWorkerComboBox, Button button)
        {
            double factor = 0;

            switch (levelWorkerComboBox.SelectedIndex)
            {
                case 0:
                    factor = 0.2;
                    break;
                case 1:
                case 2:
                    factor = 0.8;
                    break;
                default:
                    return;
            }

            resultTextBox.Text = (Double.Parse(countTextBox.Text) / SCHR * factor).ToString();
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
