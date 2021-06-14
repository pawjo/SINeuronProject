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
        private List<Button> neuronButtons { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = vm = new MainWindowViewModel(new List<ValuePoint>(), SpaceCanvas);
            DataContext = vm = new MainWindowViewModel(SpaceCanvas);
            neuronButtons = new List<Button>();
            //neuronButtons.Add(InitializeLearningButton);
            neuronButtons.Add(StepLearningButton);
            neuronButtons.Add(EpochLearningButton);
            neuronButtons.Add(AutoLearningButton);
            disableNeuronButtons();
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
        private void disableNeuronButtons()
        {
            foreach (var item in neuronButtons)
            {
                item.IsEnabled = false;
            }
        }
        
        private void EditPoint_Click(object sender, RoutedEventArgs e)
        {
            if (vm.ActiveBorder != null)
                vm.EditPoint(vm.ActiveBorder);
        }

        private void enableNeuronButtons()
        {
            foreach (var item in neuronButtons)
            {
                item.IsEnabled = true;
            }
        }

        private void InitializeLearning_Click(object sender, RoutedEventArgs e)
        {
            LearningCompletedLabel.Visibility = Visibility.Hidden;
            if (vm.InitializeNeuron())
                enableNeuronButtons();
        }

        private void StepLearning_Click(object sender, RoutedEventArgs e)
        {
            int beforeCount = vm.Neuron.ErrorLog.Count;
            vm.Neuron.StepLearning();
            int afterCount = vm.Neuron.ErrorLog.Count;
            if (beforeCount != afterCount)
                vm.CreateChart();

            vm.IterationPropertyChanged();
        }

        private void EpochLearning_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Neuron.EpochLearning() && vm.Neuron.CompletedLearning)
            {
                LearningCompletedLabel.Visibility = Visibility.Visible;
            }
            vm.CreateChart();
            vm.IterationPropertyChanged();
        }

        private void AutoLearning_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Neuron.AutoLearning())
            {
                LearningCompletedLabel.Visibility = Visibility.Visible;
            }
            vm.CreateChart();
            vm.IterationPropertyChanged();
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
