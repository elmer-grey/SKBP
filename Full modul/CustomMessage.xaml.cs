using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Full_modul
{
    /// <summary>
    /// Логика взаимодействия для CustomMessage.xaml
    /// </summary>
    public partial class CustomMessage : Window
    {
        public string Result { get; private set; }

        public CustomMessage(string message, string title, IEnumerable<string> buttons)
        {
            InitializeComponent();;

            Title = title;
            MessageText.Text = message;

            ButtonsPanel.Children.Clear();

            foreach (var buttonText in buttons)
            {
                var button = new Button
                {
                    Content = buttonText,
                    Margin = new Thickness(5),
                    MinWidth = 70,
                    Tag = buttonText,
                    FontFamily = new FontFamily("Ubuntu"),
                    FontSize = 15,
                };

                button.Click += (s, e) =>
                {
                    Result = (string)((Button)s).Tag;
                    DialogResult = true;
                    Close();
                };

                ButtonsPanel.Children.Add(button);
            }

            if (ButtonsPanel.Children.Count > 0)
                ((Button)ButtonsPanel.Children[0]).Focus();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}
