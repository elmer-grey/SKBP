using System.Configuration;
using System.Data;
using System.Windows;
using Full_modul.Properties;

namespace Full_modul
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Очищаем сохраненные данные перед выходом из приложения
            Settings.Default.Username = string.Empty;
            Settings.Default.Password = string.Empty;
            Settings.Default.RememberMe = false;
            Settings.Default.Save();

            base.OnExit(e);
        }
    }
}
