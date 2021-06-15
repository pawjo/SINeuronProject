using Microsoft.Win32;
using SINeuronWPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SINeuronWPFApp.Views
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
            DataContext = vm = new MainWindowViewModel(SpaceCanvas);
            neuronButtons = new List<Button>();
            neuronButtons.Add(StepLearningButton);
            neuronButtons.Add(EpochLearningButton);
            neuronButtons.Add(AutoLearningButton);
            disableNeuronButtons();
        }



        private void CreateNewPoint_Click(object sender, RoutedEventArgs e)
        {
            vm.AddPoint();
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
            try
            {
                vm.Neuron.StepLearning();
            }
            catch (Exception exc)
            {
                new Notification(exc.Message).ShowDialog();
            }

            afterLearning();
            int afterCount = vm.Neuron.ErrorLog.Count;
            if (beforeCount != afterCount)
                vm.CreateChart();
        }

        private void EpochLearning_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vm.Neuron.EpochLearning();
            }
            catch (Exception exc)
            {
                new Notification(exc.Message).ShowDialog();
            }

            afterLearning();
            vm.CreateChart();
        }

        private void AutoLearning_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vm.Neuron.AutoLearning();
            }
            catch (Exception exc)
            {
                new Notification(exc.Message).ShowDialog();
            }

            afterLearning();
            vm.CreateChart();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            vm.Settings();
        }

        private void afterLearning()
        {
            if (vm.Neuron.CompletedLearning)
                LearningCompletedLabel.Visibility = Visibility.Visible;

            vm.IterationPropertyChanged();
        }

        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = "arff";
            dialog.Filter = fileDialogFilter;
            if (dialog.ShowDialog() == true)
                vm.SaveSet(dialog.FileName);
        }


        private void OpenData_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = "arff";
            dialog.Filter = fileDialogFilter;
            if (dialog.ShowDialog() == true)
                vm.OpenSet(dialog.FileName);
        }

        private string fileDialogFilter = "Zbiór danych Weka (*.arff)|*.arff|Wszystkie pliki (*.*)|*.*";
    }
}
