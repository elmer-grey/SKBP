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
        private static NotificationWindow _currentWindow;
        private static readonly object _lock = new();

        public NotificationWindow(string message, Brush color, int durationSeconds = 3)
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            MaxWidth = 350;
            Height = 100;
            ShowInTaskbar = false;
            Topmost = false;
            Focusable = true;
            SizeToContent = SizeToContent.WidthAndHeight;
            Cursor = Cursors.Arrow;

            var content = new Border
            {
                Background = color,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(10),
                Child = new TextBlock
                {
                    FontSize = 16,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = message,
                    Foreground = Brushes.White,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center
                }
            };

            content.MouseEnter += (s, e) => Cursor = Cursors.Hand;
            content.MouseLeave += (s, e) => Cursor = Cursors.Arrow;

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) CloseWithAnimation(); };
            MouseDown += (s, e) => CloseWithAnimation();

            Content = content;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _timer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromSeconds(durationSeconds)
            };
            _timer.Tick += (s, e) => CloseWithAnimation();

            _timer.Start();
        }

        public void CloseWithAnimation()
        {
            if (!IsLoaded) return;

            _timer.Stop();
            var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            anim.Completed += (s, e) => Close();
            BeginAnimation(OpacityProperty, anim);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_currentWindow == this)
            {
                _currentWindow = null;
            }
            base.OnClosed(e);
        }
    }
}
