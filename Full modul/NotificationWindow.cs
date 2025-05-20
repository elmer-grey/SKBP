using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Full_modul
{
    public class NotificationWindow : Window
    {
        private readonly DispatcherTimer _timer;

        public NotificationWindow(string message, Brush color, int durationSeconds = 3)
        {
            WindowStyle = WindowStyle.None;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            Width = 300;
            Height = 100;
            Topmost = true;
            Cursor = Cursors.Hand;
            MouseDown += (s, e) => CloseWithAnimation();

            var content = new Border
            {
                Background = color,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(10),
                Child = new TextBlock
                {
                    Text = message,
                    Foreground = Brushes.White,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };

            Content = content;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(durationSeconds) };
            _timer.Tick += (s, e) => CloseWindow();
            _timer.Start();
        }

        public void CloseWithAnimation()
        {
            _timer.Stop();
            var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            anim.Completed += (s, e) => Close();
            BeginAnimation(OpacityProperty, anim);
        }

        private void CloseWindow()
        {
            _timer.Stop();
            CloseWithAnimation();
        }

        protected override void OnClosed(EventArgs e)
        {
            _timer.Stop();
            base.OnClosed(e);
        }
    }
}
