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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Calendar = System.Windows.Controls.Calendar;
using Path = System.IO.Path;

namespace Full_modul
{
    /// <summary>
    /// Логика взаимодействия для CalculatorWindow.xaml
    /// </summary>    
    public partial class CalculatorWindow : BaseWindow
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

            LoadUserDataAsync();

            // Инициализация массивов
            dateComboBoxAgo =
            [
        dateComboBoxAgo0, dateComboBoxAgo1, dateComboBoxAgo2, dateComboBoxAgo3
            ];

            dateComboBoxLater =
            [
        dateComboBoxLater0, dateComboBoxLater1, dateComboBoxLater2, dateComboBoxLater3
            ];

            dateTextBoxAgo =
            [
        dateTextBoxAgo0, dateTextBoxAgo1, dateTextBoxAgo2, dateTextBoxAgo3
            ];

            dateTextBoxLater =
            [
        dateTextBoxLater0, dateTextBoxLater1, dateTextBoxLater2, dateTextBoxLater3
            ];

            this.Loaded += OnWindowLoaded;
            Loaded += (s, e) =>
            {
                RefreshData();
            };
        }

        protected void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (Owner is MainWindow mainWindow)
            {
                mainWindow.RegisterChildWindow(this);
            }
            this.Loaded -= OnWindowLoaded;
        }

        protected override async Task InitializeConnectionElementsAsync()
        {
            IsConnected = await ConnectionCoordinator.GetConnectionStateAsync();
            await base.InitializeConnectionElementsAsync();

            if (IsConnected)
            {
                await LoadUserDataAsync();
            }
            else
            {
                UpdateUserStatus(GetOfflineStatus());
            }
        }

        protected override async Task RefreshData()
        {
            if (AreDatesFilled(_currentKoefIndex))
            {
                SChR_calculation();
                CalculateCurrentCoefficient();
                CheckBoxVisibility();
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
            _currentKoefIndex = comboBoxFormules.SelectedIndex;
            if (!AreDatesFilled(_currentKoefIndex))
            {
                isDateLaterSelected = false;
                isDateAgoSelected = false;
            } else
            {
                isDateLaterSelected = true;
                isDateAgoSelected = true;
            }

            RefreshData();

            TextBlock_ShowName.Visibility = Visibility.Collapsed;

            foreach (var group in new[] { koefgroup0, koefgroup1, koefgroup2, koefgroup3 })
            {
                group.Visibility = Visibility.Collapsed;
            }

            switch (_currentKoefIndex)
            {
                case 0: koefgroup0.Visibility = Visibility.Visible; break;
                case 1: koefgroup1.Visibility = Visibility.Visible; break;
                case 2: koefgroup2.Visibility = Visibility.Visible; break;
                case 3: koefgroup3.Visibility = Visibility.Visible; break;
            }

            UpdateDatesSelectedState();
            CheckBoxVisibility();
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

        private DateTime defaultDateLater = new DateTime(2025, 1, 1);
        private DateTime defaultDateAgo = new DateTime(2024, 1, 1);

        private bool isDateLaterSelected;
        private bool isDateAgoSelected;

        private DateTime?[] selectedDatesAgo = new DateTime?[4];
        private DateTime?[] selectedDatesLater = new DateTime?[4];

        private ComboBox[] dateComboBoxAgo;
        private ComboBox[] dateComboBoxLater;
        private TextBox[] dateTextBoxAgo;
        private TextBox[] dateTextBoxLater;

        private bool isHandlingComboBoxSelection = false;
        private bool _isGlobalPeriod = false;
        private int _currentKoefIndex = 0;

        private async void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isHandlingComboBoxSelection) return;
            isHandlingComboBoxSelection = true;

            if (sender is Calendar calendar && calendar.Tag is string textBoxName)
            {
                DateTime? selectedDate = calendar.SelectedDate;
                TextBox dateTextBox = FindName(textBoxName) as TextBox;

                if (dateTextBox != null && selectedDate.HasValue)
                {
                    dateTextBox.Text = selectedDate.Value.ToShortDateString();
                    UpdateDateStorage(textBoxName, selectedDate.Value);
                    UpdateDatesSelectedState();

                    if (isDateAgoSelected && isDateLaterSelected)
                    {
                        if (!IsConnected)
                        {
                            MessageBox.Show("Нет подключения к БД. Расчет невозможен.",
                                          "Ошибка",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                            isHandlingComboBoxSelection = false;
                            return;
                        }

                        if (_isGlobalPeriod)
                        {
                            ApplyGlobalDates();
                        }
                        else
                        {
                            SChR_calculation();
                            CalculateCurrentCoefficient();
                        }
                    }

                    CheckBoxVisibility();
                }

                if (FindVisualChild<Popup>(calendar) is Popup popup)
                {
                    popup.IsOpen = false;
                }
            }

            isHandlingComboBoxSelection = false;
        }

        private void UpdateDatesSelectedState()
        {
            isDateAgoSelected = selectedDatesAgo[_currentKoefIndex].HasValue;
            isDateLaterSelected = selectedDatesLater[_currentKoefIndex].HasValue;
        }

        private void CalculateCurrentCoefficient()
        {
            switch (comboBoxFormules.SelectedIndex)
            {
                case 0: koef0(); break;
                case 1: koef1(); break;
                case 2: koef2(); break;
                case 3: koef3(); break;
            }
        }

        private void CalculateAllCoefficients()
        {
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0: koef0(); break;
                    case 1: koef1(); break;
                    case 2: koef2(); break;
                    case 3: koef3(); break;
                }
            }
        }

        private void UpdateDateStorage(string textBoxName, DateTime date)
        {
            switch (textBoxName)
            {
                case "dateTextBoxAgo0": selectedDatesAgo[0] = date; break;
                case "dateTextBoxLater0": selectedDatesLater[0] = date; break;
                case "dateTextBoxAgo1": selectedDatesAgo[1] = date; break;
                case "dateTextBoxLater1": selectedDatesLater[1] = date; break;
                case "dateTextBoxAgo2": selectedDatesAgo[2] = date; break;
                case "dateTextBoxLater2": selectedDatesLater[2] = date; break;
                case "dateTextBoxAgo3": selectedDatesAgo[3] = date; break;
                case "dateTextBoxLater3": selectedDatesLater[3] = date; break;
            }
        }

        private void ApplyGlobalDates()
        {
            if (!selectedDatesAgo[_currentKoefIndex].HasValue ||
                !selectedDatesLater[_currentKoefIndex].HasValue)
                return;

            for (int i = 0; i < 4; i++)
            {
                if (i != _currentKoefIndex)
                {
                    selectedDatesAgo[i] = selectedDatesAgo[_currentKoefIndex];
                    selectedDatesLater[i] = selectedDatesLater[_currentKoefIndex];

                    dateTextBoxAgo[i].Text = selectedDatesAgo[i]?.ToShortDateString() ?? "";
                    dateTextBoxLater[i].Text = selectedDatesLater[i]?.ToShortDateString() ?? "";
                }
            }

            isDateAgoSelected = true;
            isDateLaterSelected = true;

            SChR_calculation();
            CalculateAllCoefficients();
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

        private bool AreDatesFilled(int koefIndex)
        {
            return selectedDatesAgo[koefIndex].HasValue &&
                   selectedDatesLater[koefIndex].HasValue;
        }

        private bool OtherCoefficientsHaveData()
        {
            DateTime? currentAgo = selectedDatesAgo[_currentKoefIndex];
            DateTime? currentLater = selectedDatesLater[_currentKoefIndex];

            for (int i = 0; i < 4; i++)
            {
                if (i != _currentKoefIndex && AreDatesFilled(i))
                {
                    DateTime? otherAgo = selectedDatesAgo[i];
                    DateTime? otherLater = selectedDatesLater[i];

                    if (otherAgo != currentAgo || otherLater != currentLater)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void CheckBox_Date_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsConnected)
            {
                MessageBox.Show("Нет подключения к БД. Глобальный расчёт невозможен.",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                CheckBox_Date.IsChecked = false;
                return;
            }

            if (!AreDatesFilled(_currentKoefIndex))
            {
                CheckBox_Date.IsChecked = false;
                return;
            }

            if (OtherCoefficientsHaveData())
            {
                var dialog = new CustomMessage(
                    "При включении единого периода все даты будут заменены на текущие. Продолжить?",
                    "Подтверждение",
                    new List<string> { "Да", "Нет" });

                dialog.ShowDialog();

                if (dialog.Result != "Да")
                {
                    CheckBox_Date.IsChecked = false;
                    return;
                }
            }

            _isGlobalPeriod = true;
            ApplyGlobalDates();
        }

        private void CheckBox_Date_Unchecked(object sender, RoutedEventArgs e)
        {
            _isGlobalPeriod = false;
        }

        public bool _isFirstRun = true;

        private void CheckBoxVisibility()
        {
            if (_isFirstRun)
            {
                CheckBox_Date.Visibility = Visibility.Collapsed;
                CheckBox_Date.IsChecked = false;
                _isGlobalPeriod = false;
                _isFirstRun = false;
                return;
            }

            bool shouldBeVisible = AreDatesFilled(_currentKoefIndex);

            if (shouldBeVisible)
            {
                if (CheckBox_Date.Visibility != Visibility.Visible)
                {
                    CheckBox_Date.Visibility = Visibility.Visible;
                    var fadeIn = new DoubleAnimation(1, TimeSpan.FromMilliseconds(200));
                    CheckBox_Date.BeginAnimation(OpacityProperty, fadeIn);
                }
                CheckBox_Date.IsChecked = _isGlobalPeriod;
            }
            else
            {
                if (CheckBox_Date.Visibility == Visibility.Visible)
                {
                    var fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
                    fadeOut.Completed += (s, _) =>
                    {
                        CheckBox_Date.Visibility = Visibility.Collapsed;
                        CheckBox_Date.IsChecked = false;
                        _isGlobalPeriod = false;
                    };
                    CheckBox_Date.BeginAnimation(OpacityProperty, fadeOut);
                }
            }
        }

        public void ExportButton_Click(object sender, EventArgs e)
        {            
            ExportData();
        }

        public void ExportData()
        {
            var dialog = new CustomMessage(
                "Выберите тип сохранения:",
                "Процедура сохранения",
                new List<string> { "Отменить", "Текущий", "Несколько" });

            dialog.ShowDialog();

            switch (dialog.Result)
            {
                case "Текущий":
                    Data.BoxIndex = comboBoxFormules.SelectedIndex;
                    Data.SaveFile = 1;
                    Data.Check0 = Data.Check1 = Data.Check2 = Data.Check3 = false;

                    switch (Data.BoxIndex)
                    {
                        case 0: Data.Check0 = true; break;
                        case 1: Data.Check1 = true; break;
                        case 2: Data.Check2 = true; break;
                        case 3: Data.Check3 = true; break;
                    }

                    var saveFileWindowCurrent = new SaveFileWindowChoise(this, null, "CalculatorWindow");
                    saveFileWindowCurrent.ShowDialog();
                    break;

                case "Несколько":
                    var saveFileDialog = new SaveFile();
                    var dialogResult = saveFileDialog.ShowDialog();

                    if (dialogResult == true)
                    {
                        if (Data.Check0 || Data.Check1 || Data.Check2 || Data.Check3)
                        {
                            Data.SaveFile = 0;
                            var saveFileWindowMultiple = new SaveFileWindowChoise(this, null, "CalculatorWindow");
                            saveFileWindowMultiple.ShowDialog();
                        }
                    }
                    break;

                case "Отменить":
                default:
                    MessageBox.Show("Сохранение файла было отменено!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }
        }

        public bool[] _warningsShown = new bool[4];

        public string SaveData(int selected)
        {
            string formulaName = GetFormulaName(selected);
            string countText = GetCountText(selected);
            string SCHR = GetSCHR(selected);
            string result = GetResultText(selected);
            _warningsShown[selected] = false;

            var dateAgoTextBox = GetDateAgoTextBox(selected);
            var dateLaterTextBox = GetDateLaterTextBox(selected);

            // Парсим даты
            DateTime startDate, endDate;
            DateTime.TryParse(dateAgoTextBox?.Text, out startDate);
            DateTime.TryParse(dateLaterTextBox?.Text, out endDate);
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
                    return $"{formulaName}: -\nНачало периода подсчёта: -\nКонец периода подсчёта: -\n" +
                           $"Количество: -\nСЧР: -\n================\n";
                }
            }
            else
            {
                _warningsShown[selected] = false;
                return $"{formulaName}: {result}\nНачало периода подсчёта: {startDate.ToShortDateString()}\n" +
                       $"Конец периода подсчёта: {endDate.ToShortDateString()}\n" +
                       $"Количество: {countText}\n" +
                       $"СЧР: {SCHR}\n================\n";
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
        public static double SCHR = 0;
        public static string startDate, endDate;
        public static string command = "";

        private bool TryGetDates(out DateTime dateAgo, out DateTime dateLater, TextBox agoTextBox, TextBox laterTextBox)
        {
            dateAgo = defaultDateAgo;
            dateLater = defaultDateLater;

            if (!DateTime.TryParse(agoTextBox.Text, out dateAgo) || !isDateAgoSelected)
            {
                dateAgo = defaultDateAgo;
            }

            if (!DateTime.TryParse(laterTextBox.Text, out dateLater) || !isDateLaterSelected)
            {
                dateLater = defaultDateLater;
            }

            return isDateLaterSelected && isDateAgoSelected;
        }

        public void koef3()
        {
            if (TryGetDates(out var dateAgo, out var dateLater, dateTextBoxAgo0, dateTextBoxLater0))
            {
                id = dateLater.Date > dateAgo.Date ? 0 : 1;
                if (id == 0)
                {
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker] " +
                        $"WHERE start_date < @DateAgo AND(end_date > @DateLater OR end_date IS NULL)";// + $"AND level_worker = @LevelWorker"
                }
                else if (id == 1)
                {                    
                    command = $"SELECT COUNT(*) FROM[calculator].[dbo].[worker] " +
                        $"WHERE start_date < @DateLater AND(end_date > @DateAgo OR end_date IS NULL)";
                }

                using (var sqlCommand = new SqlCommand(command, DatabaseConnection.Instance.Connection))
                {
                    sqlCommand.Parameters.AddWithValue("@DateAgo", dateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", dateLater);
                    //sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

                    int count = (int)sqlCommand.ExecuteScalar();
                    count3.Text = count.ToString();
                    result();
                }
            }
        }

        public void koef2()
        {
            if (TryGetDates(out var dateAgo, out var dateLater, dateTextBoxAgo0, dateTextBoxLater0))
            {
                id = dateLater.Date > dateAgo.Date ? 0 : 1;
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
                    sqlCommand.Parameters.AddWithValue("@DateAgo", dateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", dateLater);
                    //sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

                    int count = (int)sqlCommand.ExecuteScalar();
                    count2.Text = count.ToString();
                    result();
                }
            }
        }

        public void koef1() //Коэффициент оборота по выбытию
        {
            if (TryGetDates(out var dateAgo, out var dateLater, dateTextBoxAgo0, dateTextBoxLater0))
            {
                id = dateLater.Date > dateAgo.Date ? 0 : 1;
                if (id == 0)
                {
                    command = $"SELECT COUNT(*) FROM [calculator].[dbo].[worker] " +
                        $"WHERE end_date BETWEEN @DateAgo AND @DateLater " +
                        $"AND id_dismissal_reason IN (2, 3, 5)";// AND level_worker = @LevelWorker
                }
                else if (id == 1)
                {
                    command = $"SELECT COUNT(*) FROM [calculator].[dbo].[worker] " +
                        $"WHERE end_date BETWEEN @DateLater AND @DateAgo " +
                        $"AND id_dismissal_reason IN (2, 3, 5)";
                }

                using (var sqlCommand = new SqlCommand(command, DatabaseConnection.Instance.Connection))
                {
                    sqlCommand.Parameters.AddWithValue("@DateAgo", dateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", dateLater);
                    //sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

                    int count = (int)sqlCommand.ExecuteScalar();
                    count1.Text = count.ToString();
                    result();
                }
            }
        }

        public void koef0() // Коэффициент оборота по приему
        {
            if (TryGetDates(out var dateAgo, out var dateLater, dateTextBoxAgo0, dateTextBoxLater0))
            {
                id = dateLater.Date > dateAgo.Date ? 0 : 1;
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
                    sqlCommand.Parameters.AddWithValue("@DateAgo", dateAgo);
                    sqlCommand.Parameters.AddWithValue("@DateLater", dateLater);
                    //sqlCommand.Parameters.AddWithValue("@LevelWorker", level);

                    int count = (int)sqlCommand.ExecuteScalar();
                    count0.Text = count.ToString();
                    result();
                }
            }
        }

        private TextBox? GetCountTextBox(int index) => index switch
        {
            0 => count0,
            1 => count1,
            2 => count2,
            3 => count3,
            _ => null
        };
        private TextBox? GetResultTextBox(int index) => index switch
        {
            0 => result0,
            1 => result1,
            2 => result2,
            3 => result3,
            _ => null
        };
        private Button? GetSaveButton(int index) => index switch
        {
            0 => Save0,
            1 => Save1,
            2 => Save2,
            3 => Save3,
            _ => null
        };
        private TextBox? GetSchrTextBox(int index) => index switch
        {
            0 => SCHR0,
            1 => SCHR1,
            2 => SCHR2,
            3 => SCHR3,
            _ => null
        };
        private TextBox? GetDateAgoTextBox(int index) => index switch
        {
            0 => dateTextBoxAgo0,
            1 => dateTextBoxAgo1,
            2 => dateTextBoxAgo2,
            3 => dateTextBoxAgo3,
            _ => null
        };
        private TextBox? GetDateLaterTextBox(int index) => index switch
        {
            0 => dateTextBoxLater0,
            1 => dateTextBoxLater1,
            2 => dateTextBoxLater2,
            3 => dateTextBoxLater3,
            _ => null
        };

        public void SChR_calculation()
        {
            for (int i = 0; i < 4; i++)
            {
                if (_isGlobalPeriod || AreDatesFilled(i))
                {
                    CalculateSCHRForCoefficient(i);
                }
                else
                {
                    GetSchrTextBox(i).Text = "";
                }
            }
        }

        private void CalculateSCHRForCoefficient(int coeffIndex)
        {
            DateTime dateAgo, dateLater;

            switch (coeffIndex)
            {
                case 0:
                    DateTime.TryParse(dateTextBoxAgo0.Text, out dateAgo);
                    DateTime.TryParse(dateTextBoxLater0.Text, out dateLater);
                    break;
                case 1:
                    DateTime.TryParse(dateTextBoxAgo1.Text, out dateAgo);
                    DateTime.TryParse(dateTextBoxLater1.Text, out dateLater);
                    break;
                case 2:
                    DateTime.TryParse(dateTextBoxAgo2.Text, out dateAgo);
                    DateTime.TryParse(dateTextBoxLater2.Text, out dateLater);
                    break;
                case 3:
                    DateTime.TryParse(dateTextBoxAgo3.Text, out dateAgo);
                    DateTime.TryParse(dateTextBoxLater3.Text, out dateLater);
                    break;
                default:
                    return;
            }

            // Определяем порядок дат
            bool isNormalOrder = dateLater > dateAgo;
            string startDate = isNormalOrder ? dateAgo.ToString("yyyy-MM-dd") : dateLater.ToString("yyyy-MM-dd");
            string endDate = isNormalOrder ? dateLater.ToString("yyyy-MM-dd") : dateAgo.ToString("yyyy-MM-dd");

            string query = $@"
    DECLARE @StartDate DATE = '{startDate}'; 
    DECLARE @EndDate DATE = '{endDate}';
    
    SELECT 
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
                        TextBox schrTextBox = GetSchrTextBox(coeffIndex);

                        if (reader.Read())
                        {
                            if (reader.IsDBNull(reader.GetOrdinal("AverageHeadcount")))
                            {
                                schrTextBox.Text = "";
                            }
                            else
                            {
                                double schr = Math.Round(Convert.ToDouble(reader["AverageHeadcount"]), 8);
                                schrTextBox.Text = schr.ToString();
                            }
                        }
                        else
                        {
                            schrTextBox.Text = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при расчете СЧР для коэффициента {coeffIndex}: {ex.Message}");
                    GetSchrTextBox(coeffIndex).Text = "";
                }
            }
        }

        public void result()
        {
            for (int i = 0; i < 4; i++)
            {
                UpdateResultForCoefficient(i);
            }
        }

        private void UpdateResultForCoefficient(int coeffIndex)
        {
            var countTextBox = GetCountTextBox(coeffIndex);
            var resultTextBox = GetResultTextBox(coeffIndex);
            var saveButton = GetSaveButton(coeffIndex);
            var schrTextBox = GetSchrTextBox(coeffIndex);
            var dateAgoTextBox = GetDateAgoTextBox(coeffIndex);
            var dateLaterTextBox = GetDateLaterTextBox(coeffIndex);

            bool shouldCalculate = _isGlobalPeriod || AreDatesFilled(coeffIndex);

            if (!shouldCalculate)
            {
                schrTextBox.Text = "";
                resultTextBox.Text = "";
                saveButton.IsEnabled = false;
                return;
            }

            if (!double.TryParse(schrTextBox.Text, out double schrValue) || schrValue == 0 ||
                !double.TryParse(countTextBox.Text, out double countValue))
            {
                resultTextBox.Text = "";
                saveButton.IsEnabled = false;
                return;
            }

            double resultValue = countValue / schrValue;
            resultTextBox.Text = Math.Round(resultValue, 8).ToString();
            saveButton.IsEnabled = !string.IsNullOrWhiteSpace(resultTextBox.Text);
        }

        public object SaveState()
        {
            return new
            {
                CurrentKoefIndex = _currentKoefIndex,
                SelectedDatesAgo = selectedDatesAgo.Select(d => d?.ToString("yyyy-MM-dd")).ToArray(),
                SelectedDatesLater = selectedDatesLater.Select(d => d?.ToString("yyyy-MM-dd")).ToArray(),
                IsGlobalPeriod = _isGlobalPeriod,
                CheckBoxVisible = CheckBox_Date.Visibility == Visibility.Visible,
                DateTextBoxValues = new[]
                {
            dateTextBoxAgo0.Text,
            dateTextBoxAgo1.Text,
            dateTextBoxAgo2.Text,
            dateTextBoxAgo3.Text,
            dateTextBoxLater0.Text,
            dateTextBoxLater1.Text,
            dateTextBoxLater2.Text,
            dateTextBoxLater3.Text
        }
            };
        }

        public void RestoreState(object state)
        {
            if (state == null) return;

            try
            {
                dynamic data = state;

                _currentKoefIndex = (int)data.CurrentKoefIndex;
                _isGlobalPeriod = (bool)data.IsGlobalPeriod;

                var datesAgo = (string[])data.SelectedDatesAgo;
                var datesLater = (string[])data.SelectedDatesLater;

                for (int i = 0; i < 4; i++)
                {
                    selectedDatesAgo[i] = !string.IsNullOrEmpty(datesAgo[i]) ?
                        DateTime.Parse(datesAgo[i]) : (DateTime?)null;
                    selectedDatesLater[i] = !string.IsNullOrEmpty(datesLater[i]) ?
                        DateTime.Parse(datesLater[i]) : (DateTime?)null;
                }

                var dateTextBoxValues = (string[])data.DateTextBoxValues;
                dateTextBoxAgo0.Text = dateTextBoxValues[0];
                dateTextBoxAgo1.Text = dateTextBoxValues[1];
                dateTextBoxAgo2.Text = dateTextBoxValues[2];
                dateTextBoxAgo3.Text = dateTextBoxValues[3];
                dateTextBoxLater0.Text = dateTextBoxValues[4];
                dateTextBoxLater1.Text = dateTextBoxValues[5];
                dateTextBoxLater2.Text = dateTextBoxValues[6];
                dateTextBoxLater3.Text = dateTextBoxValues[7];

                _isFirstRun = false;
                comboBoxFormules.SelectedIndex = _currentKoefIndex;
                UpdateDatesSelectedState();

                CheckBoxVisibility();

                if (AreDatesFilled(_currentKoefIndex))
                {
                    SChR_calculation();
                    CalculateCurrentCoefficient();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка восстановления состояния: {ex.Message}");
            }
        }
    }
}
