using SINeuronWPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SINeuronWPFApp
{
    /// <summary>
    /// Interaction logic for CreateNewPointDialog.xaml
    /// </summary>
    public partial class PointDialogWindow : Window
    {

        public PointDialogWindow(PointDialogViewModel _vm)
        {
            DataContext = vm = _vm;
            InitializeComponent();
        }
        
        private readonly PointDialogViewModel vm;
        
        private void cancel() => this.Close();

        private void CancelButton_Click(object sender, RoutedEventArgs e) => cancel();
        
        private void RB_1_Checked(object sender, RoutedEventArgs e)
        {
            vm.Point.Value = 1;
        }

        private void RB_minus_1_Checked(object sender, RoutedEventArgs e)
        {
            vm.Point.Value = -1;
        }

        private void submit()
        {
            vm.ChangesSubmitted = true;
            this.Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e) => submit();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Enter:
                    submit();
                    break;
                case Key.Escape:
                    cancel();
                    break;
            }
        }
    }
}
