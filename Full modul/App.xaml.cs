using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Full_modul.Properties;
using Microsoft.Data.SqlClient;

namespace Full_modul
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string ReportsFolderName = "Отчёты1";
        private const string CalculatorFolderName = "Калькулятор";
        private const string ConditionsFolderName = "Условия";

        protected override void OnStartup(StartupEventArgs e)
        {
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

            base.OnStartup(e);
            CreateReportFolders();
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
            Settings.Default.Username = string.Empty;
            Settings.Default.Password = string.Empty;
            Settings.Default.RememberMe = false;
            Settings.Default.Save();
            DatabaseConnection.Instance.CloseConnection();

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

                MessageBox.Show("Папки 'Отчёты', 'Калькулятор' и 'Условия' были успешно созданы.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
