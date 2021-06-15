using System.Windows;

namespace SINeuronWPFApp.Views
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Window
    {
        public Notification(string message)
        {
            Message = message;
            DataContext = this;
            InitializeComponent();
        }

        public string Message { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
