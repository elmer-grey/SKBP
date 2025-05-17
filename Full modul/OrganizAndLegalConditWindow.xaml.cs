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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace Full_modul
{
    /// <summary>
    /// Логика взаимодействия для OrganizAndLegalConditWindow.xaml
    /// </summary>
    public class DocumentItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool HasDocument { get; set; }
        public string DocumentPath { get; set; }
        public bool IsChecked { get; set; }
        public bool AffectsOtherContainer { get; set; }
        public List<int> AffectedItems { get; set; } = new List<int>();
    }

    public class OrganizationalMeasure
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public bool IsEnabled { get; set; }
        public bool RequiresDocument { get; set; }
        public int RequiredDocumentId { get; set; }
        public bool IsHighlighted { get; set; }
    }

    public partial class OrganizAndLegalConditWindow : Window
    {
        private List<DocumentItem> _documentItems = new List<DocumentItem>();
        private Dictionary<int, Image> _documentIcons = new Dictionary<int, Image>();
        private List<OrganizationalMeasure> _organizationalMeasures = new List<OrganizationalMeasure>();
        private Dictionary<int, RadioButton> _documentYesButtons = new Dictionary<int, RadioButton>();
        private Dictionary<int, RadioButton> _documentNoButtons = new Dictionary<int, RadioButton>();
        private Dictionary<int, RadioButton> _measureYesButtons = new Dictionary<int, RadioButton>();
        private Dictionary<int, RadioButton> _measureNoButtons = new Dictionary<int, RadioButton>();
        private Queue<string> _notificationQueue;
        private bool _isNotificationShowing = false;
        public OrganizAndLegalConditWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/HR.ico"));

            LoadUserData();
            InitializeData();
            CreateDocumentControls();
            CreateMeasureControls();
            _notificationQueue = new Queue<string>();
        }

        private void LoadUserData()
        {
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

        private void InitializeData()
        {
            _documentItems = new List<DocumentItem>
    {
        new DocumentItem
        {
            Id = 0,
            Text = "Устав организации",
            AffectsOtherContainer = true,
            AffectedItems = new List<int> { 0 } // Влияет на меру с Id=0
        },
        new DocumentItem
        {
            Id = 1,
            Text = "Трудовой договор",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 2,
            Text = "Коллективный договор",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 3,
            Text = "Кадровая политика",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 4,
            Text = "Правила внутреннего распорядка",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 5,
            Text = "Штатное расписание",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 6,
            Text = "Табель учёта рабочего времени",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 7,
            Text = "График отпусков",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 8,
            Text = "Должностные инструкции",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 9,
            Text = "Инструкция по технике безопасности",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 10,
            Text = "Положение о персонале",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 11,
            Text = "Положение об аттестации сотрудников",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 12,
            Text = "Положение о стимулировании и мотивировании",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 13,
            Text = "Положение о подразделениях организации",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 14,
            Text = "Положение о командировках",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 15,
            Text = "Положение об обучении и развитие персонала",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 16,
            Text = "Положение о защите персональных данных сотрудников",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 17,
            Text = "Положение об оценке персонала",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 18,
            Text = "Положение о наставничестве",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 19,
            Text = "Положение о системе документооборота",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 20,
            Text = "Положение о порядке приема, перевода и увольнения работников",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 21,
            Text = "Положение о порядке обучения и повышения квалификации персонала",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 22,
            Text = "Положение о корпоративной культуре",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 23,
            Text = "Положение о коммерческой тайне",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 24,
            Text = "Положение о государственной тайне",
            AffectsOtherContainer = false
        },
        new DocumentItem
        {
            Id = 25,
            Text = "Политика о разграничении доступа в помещение",
            AffectsOtherContainer = true,
            AffectedItems = new List<int> { 1 } // Влияет на меру с Id=1
        }
    };

            _organizationalMeasures = new List<OrganizationalMeasure>
    {
        new OrganizationalMeasure
        {
            Id = 0,
            Text = "Назначение ответственного за охрану труда",
            RequiresDocument = true,
            RequiredDocumentId = 0, // Требует документ с Id=0
            IsEnabled = true
        },
        new OrganizationalMeasure
        {
            Id = 1,
            Text = "Проведение обучения по охране труда",
            RequiresDocument = true,
            RequiredDocumentId = 25, // Требует документ с Id=25
            IsEnabled = true
        },
        new OrganizationalMeasure
        {
            Id = 2,
            Text = "Обеспечение СИЗ",
            RequiresDocument = false,
            IsEnabled = true
        },
        new OrganizationalMeasure
        {
            Id = 3,
            Text = "Обеспечение СИЗ",
            RequiresDocument = false,
            IsEnabled = true
        },
        new OrganizationalMeasure
        {
            Id = 4,
            Text = "Обеспечение СИЗ",
            RequiresDocument = false,
            IsEnabled = true
        },
            };
        }

        private void CreateDocumentControls()
        {
            var documentsPanel = new StackPanel();

            foreach (var item in _documentItems)
            {
                var grid = new Grid
                {
                    Name = $"DocGrid{item.Id}",
                    Margin = new Thickness(5),
                    Tag = item.Id
                };

                ContainerLegal.RegisterName(grid.Name, grid);

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(400) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                // Текст документа
                var textBlock = new TextBlock
                {
                    Text = item.Text,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5),
                    FontFamily = new FontFamily("Ubuntu"),
                    FontWeight = FontWeights.Normal,
                    FontSize = 15,
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 425
                };

                if (item.AffectsOtherContainer)
                {
                    var toolTip = new ToolTip();
                    var stackPanel = new StackPanel();
                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = "Этот параметр влияет на организационные меры",
                        FontWeight = FontWeights.Bold
                    });
                    toolTip.Content = stackPanel;
                    ToolTipService.SetToolTip(textBlock, toolTip);
                }

                Grid.SetColumn(textBlock, 0);
                grid.Children.Add(textBlock);

                // Иконка документа
                var image = new Image
                {
                    Name = $"DocIcon{item.Id}",
                    Height = 30,
                    Margin = new Thickness(5, 0, 0, 0),
                    Source = new BitmapImage(new Uri("pack://application:,,,/Images/Reports.png")),
                    Visibility = Visibility.Collapsed,
                    Tag = item.Id,
                    Cursor = Cursors.Hand,
                    ToolTip = $"Открыть документ: {item.Text}"
                };
                _documentIcons[item.Id] = image;
                image.MouseLeftButtonDown += OpenDocument_Click;
                Grid.SetColumn(image, 1);
                grid.Children.Add(image);

                var stackPanelRb = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Height = 30
                };

                var yesRb = new RadioButton
                {
                    Name = $"DocRb{item.Id}_Yes",
                    GroupName = $"DocGroup{item.Id}",
                    Margin = new Thickness(5, 0, 5, 0),
                    Style = (Style)FindResource("CustomRadioButton"),
                    Content = "Есть",
                    Tag = item.Id
                };
                yesRb.Checked += DocumentRadioButton_Checked;

                var noRb = new RadioButton
                {
                    Name = $"DocRb{item.Id}_No",
                    GroupName = $"DocGroup{item.Id}",
                    Margin = new Thickness(5, 0, 5, 0),
                    Style = (Style)FindResource("CustomRadioButton"),
                    Content = "Нет",
                    Tag = item.Id
                };
                noRb.Checked += DocumentRadioButton_Checked;

                _documentYesButtons[item.Id] = yesRb;
                _documentNoButtons[item.Id] = noRb;

                stackPanelRb.Children.Add(yesRb);
                stackPanelRb.Children.Add(noRb);
                Grid.SetColumn(stackPanelRb, 2);
                grid.Children.Add(stackPanelRb);

                documentsPanel.Children.Add(grid);
            }

            var scrollViewer = ContainerLegal.FindName("DocumentsScrollViewer") as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.Content = documentsPanel;
            }
        }

        private void CreateMeasureControls()
        {
            var measuresPanel = new StackPanel();

            foreach (var measure in _organizationalMeasures)
            {
                var grid = new Grid
                {
                    Name = $"MeasureGrid{measure.Id}",
                    Margin = new Thickness(5),
                    Visibility = Visibility.Visible,
                    Tag = measure.Id
                };

                ContainerOrgan.RegisterName(grid.Name, grid);

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                // Текст меры
                var textBlock = new TextBlock
                {
                    Text = measure.Text,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5),
                    FontFamily = new FontFamily("Ubuntu"),
                    FontWeight = FontWeights.Normal,
                    FontSize = 15,
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 425
                };
                Grid.SetColumn(textBlock, 0);
                grid.Children.Add(textBlock);

                // RadioButtons
                var stackPanelRb = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Height = 30
                };

                var yesRb = new RadioButton
                {
                    Name = $"MeasureRb{measure.Id}_Yes",
                    GroupName = $"MeasureGroup{measure.Id}",
                    Margin = new Thickness(5, 0, 5, 0),
                    Style = (Style)FindResource("CustomRadioButton"),
                    Content = "Есть",
                    Tag = measure.Id,
                    IsEnabled = measure.IsEnabled
                };
                yesRb.Checked += MeasureRadioButton_Checked;

                var noRb = new RadioButton
                {
                    Name = $"MeasureRb{measure.Id}_No",
                    GroupName = $"MeasureGroup{measure.Id}",
                    Margin = new Thickness(5, 0, 5, 0),
                    Style = (Style)FindResource("CustomRadioButton"),
                    Content = "Нет",
                    Tag = measure.Id,
                    IsEnabled = measure.IsEnabled
                };
                noRb.Checked += MeasureRadioButton_Checked;

                _measureYesButtons[measure.Id] = yesRb;
                _measureNoButtons[measure.Id] = noRb;

                stackPanelRb.Children.Add(yesRb);
                stackPanelRb.Children.Add(noRb);
                Grid.SetColumn(stackPanelRb, 1);
                grid.Children.Add(stackPanelRb);

                measuresPanel.Children.Add(grid);
            }

            var scrollViewer = ContainerOrgan.FindName("MeasuresScrollViewer") as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.Content = measuresPanel;
            }
        }

        private async void DocumentRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.IsChecked == true)
            {
                int id = (int)radioButton.Tag;
                bool isYes = radioButton.Content.ToString() == "Есть";

                var document = _documentItems.FirstOrDefault(d => d.Id == id);
                if (document != null)
                {
                    document.IsChecked = true;

                    if (_documentIcons.TryGetValue(id, out Image docIcon))
                    {
                        if (isYes)
                        {
                            docIcon.Opacity = 0.5;
                            docIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Images/loading.gif"));
                            docIcon.Visibility = Visibility.Visible;

                            bool fileExists = await CheckDocumentExistsOnServer(document.Text);

                            if (fileExists)
                            {
                                docIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Reports.png"));
                                docIcon.Opacity = 1;
                                document.HasDocument = true;
                            }
                            else
                            {
                                docIcon.Visibility = Visibility.Collapsed;
                                document.HasDocument = false;

                                ShowDocumentNotFoundNotification(document.Text);
                            }
                        }
                        else
                        {
                            docIcon.Visibility = Visibility.Collapsed;
                        }
                    }

                    if (document.AffectsOtherContainer)
                    {
                        foreach (var affectedId in document.AffectedItems)
                        {
                            var measure = _organizationalMeasures.FirstOrDefault(m => m.Id == affectedId);
                            if (measure != null && ContainerOrgan.FindName($"MeasureGrid{measure.Id}") is Grid grid)
                            {
                                bool wasVisibleBefore = grid.Visibility == Visibility.Visible;
                                grid.Visibility = isYes ? Visibility.Visible : Visibility.Collapsed;

                                if (!wasVisibleBefore && isYes)
                                {
                                    grid.Background = new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));
                                    measure.IsHighlighted = true;
                                }
                                else if (!isYes)
                                {
                                    _measureYesButtons[measure.Id].IsChecked = false;
                                    _measureNoButtons[measure.Id].IsChecked = false;
                                    measure.IsSelected = false;
                                    measure.IsHighlighted = false;
                                    grid.Background = Brushes.Transparent;
                                    CalculateSecondContainerResult();
                                }
                            }
                        }
                    }
                }
                CalculateFirstContainerResult();
            }
        }
        private async Task<bool> CheckDocumentExistsOnServer(string documentName)
        {
            try
            {
                string query = "SELECT COUNT(1) FROM Documents WHERE DocumentName = @DocumentName";
                int count = await DatabaseConnection.Instance.ExecuteScalarAsync<int>(
                    query,
                    new SqlParameter("@DocumentName", documentName));

                return count > 0;
            }
            catch
            {
                return false;
            }
        }

        private async void ShowDocumentNotFoundNotification(string documentName)
        {
            _notificationQueue.Enqueue(documentName);
            await ShowNextNotification();
        }

        private async Task ShowNextNotification()
        {
            if (_isNotificationShowing || !_notificationQueue.Any())
                return;

            _isNotificationShowing = true;

            string documentName = _notificationQueue.Dequeue();

            NotificationText.Text = $"Документ \"{documentName}\" не найден на сервере";

            var opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            var scaleAnimation = new DoubleAnimation
            {
                From = 0.8,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new ElasticEase { Oscillations = 1, Springiness = 4 }
            };

            var border = (Border)NotificationPopup.Child;
            border.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            PopupTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            PopupTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);

            NotificationPopup.IsOpen = true;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += async (s, e) =>
            {
                opacityAnimation.To = 0;
                opacityAnimation.From = 1;
                opacityAnimation.Duration = TimeSpan.FromSeconds(0.2);

                border.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);

                var closeTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.2) };
                closeTimer.Tick += (s2, e2) =>
                {
                    NotificationPopup.IsOpen = false;
                    closeTimer.Stop();
                    _isNotificationShowing = false;
                    _ = ShowNextNotification();
                };
                closeTimer.Start();

                timer.Stop();
            };
            timer.Start();
        }

        private void MeasureRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.IsChecked == true)
            {
                int id = (int)radioButton.Tag;
                bool isYes = radioButton.Content.ToString() == "Есть";

                var measure = _organizationalMeasures.FirstOrDefault(m => m.Id == id);
                if (measure != null && ContainerOrgan.FindName($"MeasureGrid{measure.Id}") is Grid grid)
                {
                    measure.IsSelected = true;

                    if (measure.IsHighlighted)
                    {
                        grid.Background = Brushes.Transparent;
                        measure.IsHighlighted = false;
                    }
                }
                CalculateSecondContainerResult();
            }
        }

        private void CalculateFirstContainerResult()
        {
            int sum = 0;
            int count = 0;
            bool allSelected = true;

            foreach (var item in _documentItems)
            {
                if (_documentYesButtons[item.Id].IsChecked == true)
                {
                    sum += 1;
                    count++;
                }
                else if (_documentNoButtons[item.Id].IsChecked == true)
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
                result0.Text = average.ToString("0.#####");

                if (ClickableImage2 != null && !string.IsNullOrEmpty(result0.Text))
                {
                    ClickableImage2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Arrow_Blue0.png"));
                }
            }
            else
            {
                result0.Text = string.Empty;
            }
        }

        private void CalculateSecondContainerResult()
        {
            int sum = 0;
            int count = 0;
            bool allSelected = true;

            foreach (var measure in _organizationalMeasures)
            {
                if (ContainerOrgan.FindName($"MeasureGrid{measure.Id}") is Grid grid
                    && grid.Visibility == Visibility.Visible)
                {
                    if (_measureYesButtons.TryGetValue(measure.Id, out var yesBtn) && yesBtn.IsChecked == true)
                    {
                        sum += 1;
                        count++;
                    }
                    else if (_measureNoButtons.TryGetValue(measure.Id, out var noBtn) && noBtn.IsChecked == true)
                    {
                        count++;
                    }
                    else
                    {
                        allSelected = false;
                        break;
                    }
                }
            }

            if (allSelected && count > 0)
            {
                double average = (double)sum / count;
                result1.Text = average.ToString("0.#####");
                Save0.IsEnabled = true;
            }
            else
            {
                result1.Text = string.Empty;
                Save0.IsEnabled = false;
            }
        }

        // Обработчики для ContainerLegal
        private void LegalResetButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in _documentItems)
            {
                _documentYesButtons[item.Id].IsChecked = false;
                _documentNoButtons[item.Id].IsChecked = false;
                item.IsChecked = false;
            }
            result0.Text = string.Empty;
            ClickableImage2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Arrow_Gray.png"));
        }

        private void LegalYesButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in _documentItems)
            {
                _documentYesButtons[item.Id].IsChecked = true;
                _documentNoButtons[item.Id].IsChecked = false;
                item.IsChecked = true;
            }
            CalculateFirstContainerResult();
        }

        private void LegalNoButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in _documentItems)
            {
                _documentYesButtons[item.Id].IsChecked = false;
                _documentNoButtons[item.Id].IsChecked = true;
                item.IsChecked = true;
            }
            CalculateFirstContainerResult();
        }

        // Обработчики для ContainerOrgan
        private void OrganResetButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var measure in _organizationalMeasures)
            {
                if (ContainerOrgan.FindName($"MeasureGrid{measure.Id}") is Grid grid
                    && grid.Visibility == Visibility.Visible)
                {
                    _measureYesButtons[measure.Id].IsChecked = false;
                    _measureNoButtons[measure.Id].IsChecked = false;
                    measure.IsSelected = false;
                    measure.IsHighlighted = false;
                    grid.Background = Brushes.Transparent;
                }
            }
            CalculateSecondContainerResult();
        }

        private void OrganYesButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var measure in _organizationalMeasures)
            {
                if (ContainerOrgan.FindName($"MeasureGrid{measure.Id}") is Grid grid
                    && grid.Visibility == Visibility.Visible)
                {
                    _measureYesButtons[measure.Id].IsChecked = true;
                    _measureNoButtons[measure.Id].IsChecked = false;
                    measure.IsSelected = true;
                    measure.IsHighlighted = false;
                    grid.Background = Brushes.Transparent;
                }
            }
            CalculateSecondContainerResult();
        }

        private void OrganNoButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var measure in _organizationalMeasures)
            {
                if (ContainerOrgan.FindName($"MeasureGrid{measure.Id}") is Grid grid
                    && grid.Visibility == Visibility.Visible)
                {
                    _measureYesButtons[measure.Id].IsChecked = false;
                    _measureNoButtons[measure.Id].IsChecked = true;
                    measure.IsSelected = true;
                    measure.IsHighlighted = false;
                    grid.Background = Brushes.Transparent;
                }
            }
            CalculateSecondContainerResult();
        }

        private async void OpenDocument_Click(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Image image)) return;

            image.IsEnabled = false;
            Cursor = Cursors.Wait;

            using (var loadingIndicator = new LoadingIndicator())
            {
                try
                {
                    int documentId = (int)image.Tag;
                    var document = _documentItems.FirstOrDefault(d => d.Id == documentId);

                    if (document == null || !document.HasDocument) return;

                    string documentPath = await GetDocumentPathFromDatabase(document.Text);

                    if (string.IsNullOrEmpty(documentPath))
                    {
                        MessageBox.Show("Документ не найден на сервере", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (!File.Exists(documentPath))
                    {
                        MessageBox.Show("Файл документа не найден", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    OpenDocumentInAdobeReader(documentPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии документа: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    image.IsEnabled = true;
                    Cursor = Cursors.Arrow;
                }
            }
        }

        private async Task<string> GetDocumentPathFromDatabase(string documentName)
        {
            string query = "SELECT ServerFilePath FROM Documents WHERE DocumentName = @DocumentName";

            try
            {
                string serverFilePath = await DatabaseConnection.Instance.ExecuteScalarAsync<string>(
                    query,
                    new SqlParameter("@DocumentName", documentName));

                if (string.IsNullOrEmpty(serverFilePath))
                {
                    throw new FileNotFoundException("Путь к документу не найден в базе данных");
                }

                if (!File.Exists(serverFilePath))
                {
                    throw new FileNotFoundException($"Файл не найден по указанному пути: {serverFilePath}");
                }
                return await DownloadFileFromServer(serverFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения документа: {ex.Message}", ex);
            }
        }

        private async Task<string> DownloadFileFromServer(string serverFilePath)
        {
            try
            {
                var tempFilePath = TempFileManager.GetTempFilePath(Path.GetFileName(serverFilePath));

                using (FileStream sourceStream = new FileStream(serverFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (FileStream destinationStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await sourceStream.CopyToAsync(destinationStream);
                }

                return tempFilePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка копирования файла с сервера: {ex.Message}", ex);
            }
        }

        private void OpenDocumentInAdobeReader(string filePath)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "AcroRd32.exe",
                    Arguments = $"\"{filePath}\"",
                    UseShellExecute = true
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                catch
                {
                    throw new Exception($"Не удалось открыть документ. Убедитесь, что Adobe Reader установлен.\n{ex.Message}");
                }
            }
        }

        public class LoadingIndicator : IDisposable
        {
            private readonly Window _overlayWindow;

            public LoadingIndicator()
            {
                _overlayWindow = new Window
                {
                    WindowStyle = WindowStyle.None,
                    AllowsTransparency = true,
                    Background = Brushes.Transparent,
                    Topmost = true,
                    ShowInTaskbar = false,
                    SizeToContent = SizeToContent.WidthAndHeight
                };

                var grid = new Grid { Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)) };
                var progress = new ProgressBar
                {
                    IsIndeterminate = true,
                    Width = 200,
                    Height = 20,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                grid.Children.Add(progress);
                _overlayWindow.Content = grid;

                var mainWindow = Application.Current.MainWindow;
                _overlayWindow.Owner = mainWindow;
                _overlayWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                _overlayWindow.Show();
            }

            public void Dispose()
            {
                _overlayWindow.Close();
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
                    CalculateSecondContainerResult();
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
