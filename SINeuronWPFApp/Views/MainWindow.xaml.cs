using SINeuronWPFApp.Models;
using SINeuronWPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SINeuronWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm = new MainWindowViewModel(new List<Models.ValuePoint>(), SpaceCanvas);
        }

        private void CreateNewPoint_Click(object sender, RoutedEventArgs e)
        {
            vm.AddPoint();
            //var pointVm = new PointDialogViewModel("Stwórz nowy punkt.");
            //var window = new PointDialogWindow(pointVm);
            //window.ShowDialog();
            //if (pointVm.ChangesSubmitted)
            //{
            //    vm.AddPoint(pointVm.Point);
            //}
        }

        private readonly MainWindowViewModel vm;

        private void Test_click(object sender, RoutedEventArgs e)
        {
            vm.Synchronize();
            ;
        }

        private void DeletePoint_Click(object sender, RoutedEventArgs e)
        {
            if (vm.ActiveBorder != null)
                vm.DeletePoint(vm.ActiveBorder);
        }



        //private void Window_KeyDown(object sender, KeyEventArgs e)
        //{
        //    var element = vm.UIPoints.Last().Border;
        //    switch (e.Key)
        //    {
        //        case Key.Left:
        //            vm.MoveElement(element, -20, 0);
        //            break;
        //        case Key.Right:
        //            vm.MoveElement(element, 20, 0);
        //            break;
        //        case Key.Up:
        //            vm.MoveElement(element, 0, 20);
        //            break;
        //        case Key.Down:
        //            vm.MoveElement(element, 0, -20);
        //            break;
        //    }
        //}

        //protected bool isDragging;
        //private Point clickPosition;
        //private TranslateTransform originTT;

        //private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var draggableControl = sender as UIElement;
        //    isDragging = true;
        //    clickPosition = e.GetPosition(SpaceCanvas);
        //    draggableControl.CaptureMouse();
        //}

        //private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    isDragging = false;
        //    var draggable = sender as UIElement;
        //    draggable.ReleaseMouseCapture();
        //}

        //private void Canvas_MouseMove(object sender, MouseEventArgs e)
        //{
        //    var draggableControl = sender as UIElement;
        //    if (isDragging && draggableControl != null)
        //    {
        //        Point currentPosition = e.GetPosition(SpaceCanvas);
        //        Canvas.SetLeft(draggableControl, currentPosition.X);
        //        Canvas.SetTop(draggableControl, currentPosition.Y);
        //    }
        //}

    }
}
