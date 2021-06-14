using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;

namespace SINeuronWPFApp.ViewModels
{
    public class ErrorChartViewModel : ViewModelBase
    {
        public SeriesCollection SeriesCollection { get; set; }

        public string[] Labels { get; set; }

        public Func<double, string> YFormatter { get; set; }

        public List<double> ErrorLog { get; set; }

        public string ErrorMessage { get; set; }

        public ErrorChartViewModel(List<double> errorLog)
        {
            ErrorLog = errorLog;

            if (ErrorLog != null)
            {
                SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Wykres błędu uczenia",
                        Values = new ChartValues<double>(ErrorLog)
                    }
                };

                int length = ErrorLog.Count;
                Labels = new string[length];
                for (int i = 0; i < length; i++)
                    Labels[i] = (i + 1).ToString();

                YFormatter = value => value.ToString();
            }
            //else
            //{
            //    MainGrid.Background = Brushes.Gray;
            //    ErrorMessage = "Brak danych.";
            //}
        }
    }
}
