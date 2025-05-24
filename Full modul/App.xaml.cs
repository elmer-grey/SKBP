using Full_modul.Properties;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Windows;

namespace Full_modul
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string ReportsFolderName = "Отчёты";
        private const string CalculatorFolderName = "Калькулятор";
        private const string ConditionsFolderName = "Условия";
        private const string AppMutexName = "Global\\Full_modul_HR_Calculator_Mutex";

        private static Mutex _appMutex;
        public static DateTime StartTime { get; private set; }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            _appMutex = new Mutex(true, AppMutexName, out createdNew);

            if (!createdNew)
            {
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }

                Current.Shutdown();
                return;
            }

            StartTime = DateTime.Now;

            if (string.IsNullOrEmpty(Settings.Default.ConnectionString))
            {
                string rawConnection = BuildConnectionString(
                    server: "DESKTOP-TBLQV8A",
                    database: "calculator",
                    userId: "sqlserver",
                    password: "sqlserver");

                Settings.Default.ConnectionString = SecurityHelper.Encrypt(rawConnection);
                Settings.Default.Save();
            }

            string decryptedConnection = SecurityHelper.Decrypt(Settings.Default.ConnectionString);
            SqlConnectionStringBuilder builder = new(decryptedConnection)
            {
                ConnectTimeout = 3 
            };

            Settings.Default.ConnectionString = SecurityHelper.Encrypt(builder.ToString());
            Settings.Default.Save();

            DatabaseConnection.Instance.InitializeAsync();
            base.OnStartup(e);
            CreateReportFolders();

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Exception ex = (Exception)args.ExceptionObject;
                MessageBox.Show($"Необработанное исключение: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            DispatcherUnhandledException += (sender, args) =>
            {
                MessageBox.Show($"Необработанное исключение в UI: {args.Exception.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true;
            };
        }

        private string BuildConnectionString(string server, string database, string userId, string password)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                UserID = userId,
                Password = password,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            UserInfo.ClearTempData();
            DatabaseConnection.Instance.CloseConnection();
            if (_appMutex != null)
            {
                _appMutex.ReleaseMutex();
                _appMutex.Dispose();
            }

            KillAllProcesses();

            base.OnExit(e);
        }

        private void CreateReportFolders()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string reportsFolderPath = Path.Combine(appDirectory, ReportsFolderName);
            string calculatorFolderPath = Path.Combine(reportsFolderPath, CalculatorFolderName);
            string conditionsFolderPath = Path.Combine(reportsFolderPath, ConditionsFolderName);

            if (!Directory.Exists(reportsFolderPath))
            {
                Directory.CreateDirectory(reportsFolderPath);
                Directory.CreateDirectory(calculatorFolderPath);
                Directory.CreateDirectory(conditionsFolderPath);

                MessageBox.Show("При первом запуске программы созданы папки \"Отчёты\\Калькулятор\", \"Отчёты\\Условия\".", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void KillAllProcesses()
        {
            try
            {
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        process.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при завершении процессов: {ex.Message}");
            }
        }
    }
}
