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
        }



        private void CreateNewPoint_Click(object sender, RoutedEventArgs e)
        {
            vm.AddPoint();
        }

        private readonly MainWindowViewModel vm;

        private void DeletePoint_Click(object sender, RoutedEventArgs e)
        {
            if (vm.ActiveBorder != null)
                vm.DeletePoint(vm.ActiveBorder);
            else
                new Notification("Nie wybrano punktu do usunięcia.").ShowDialog();
        }
        private void EditPoint_Click(object sender, RoutedEventArgs e)
        {
            if (vm.ActiveBorder != null)
                vm.EditPoint(vm.ActiveBorder);
            else
                new Notification("Nie wybrano punktu do edycji.").ShowDialog();
        }
        private void InitializeLearning_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vm.InitializeNeuron();
            }
            catch (Exception exc)
            {
                new Notification(exc.Message).ShowDialog();
            }
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
                vm.CompletedLearningPropertyChanged();

            vm.IterationPropertyChanged();
        }

        private string fileDialogFilterArff = "Zbiór danych Weka (*.arff)|*.arff|Wszystkie pliki (*.*)|*.*";
        
        private string fileDialogFilterJson = "Plik json (*.json)|*.json|Wszystkie pliki (*.*)|*.*";
        
        private void Menu_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var image = sender as Image;
            var contextMenu = image.ContextMenu;
            contextMenu.PlacementTarget = image;
            contextMenu.IsOpen = true;
        }
        
        private void OpenData_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = "arff";
            dialog.Filter = fileDialogFilterArff;
            if (dialog.ShowDialog() == true)
                vm.OpenSet(dialog.FileName);
        }
        
        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = "arff";
            dialog.Filter = fileDialogFilterArff;
            if (dialog.ShowDialog() == true)
                vm.SaveSet(dialog.FileName);
        }

        private void OpenAppState_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = "arff";
            dialog.Filter = fileDialogFilterArff;
            if (dialog.ShowDialog() == true)
                vm.OpenAppStateWeka(dialog.FileName);
        }

        private void SaveAppState_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = "arff";
            dialog.Filter = fileDialogFilterArff;
            if (dialog.ShowDialog() == true)
                vm.SaveAppStateWeka(dialog.FileName);
        }
        


        //==============
        //JSON

        //private void OpenAppState_Click(object sender, RoutedEventArgs e)
        //{
        //    var dialog = new OpenFileDialog();
        //    dialog.DefaultExt = "json";
        //    dialog.Filter = fileDialogFilterJson;
        //    if (dialog.ShowDialog() == true)
        //        vm.OpenAppStateJson(dialog.FileName);
        //}

        //private void SaveAppState_Click(object sender, RoutedEventArgs e)
        //{
        //    var dialog = new SaveFileDialog();
        //    dialog.DefaultExt = "json";
        //    dialog.Filter = fileDialogFilterJson;
        //    if (dialog.ShowDialog() == true)
        //        vm.SaveAppStateJson(dialog.FileName);
        //}
    }
}
