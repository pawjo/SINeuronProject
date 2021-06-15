using SINeuronWPFApp.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace SINeuronWPFApp.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(SettingsViewModel _vm)
        {
            DataContext = vm = _vm;
            InitializeComponent();
        }

        private readonly SettingsViewModel vm;

        private void cancel() => this.Close();

        private void CancelButton_Click(object sender, RoutedEventArgs e) => cancel();

        private void submit()
        {
            vm.ChangesSubmitted = true;
            this.Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e) => submit();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    submit();
                    break;
                case Key.Escape:
                    cancel();
                    break;
            }
        }

        private void RB_errorTolerance_Checked(object sender, RoutedEventArgs e)
        {
            vm.StopConditionErrorTolerance = true;
        }

        private void RB_maxIteration_Checked(object sender, RoutedEventArgs e)
        {
            vm.StopConditionErrorTolerance = false;
        }
    }
}
