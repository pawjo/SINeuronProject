using LiveCharts;
using LiveCharts.Wpf;
using SINeuronWPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SINeuronWPFApp.UserControls
{
    /// <summary>
    /// Interaction logic for ErrorChart.xaml
    /// </summary>
    public partial class ErrorChart : UserControl, INotifyPropertyChanged
    {
        public SeriesCollection SeriesCollection { get; set; }

        public string[] Labels { get; set; }

        public Func<double, string> YFormatter { get; set; }

        public List<double> ErrorLog 
        {
            get => GetValue(ErrorLogProperty) as List<double>;

            set
            {
                SetValue(ErrorLogProperty, value);
                CreateChart();
            }
        }

        public string ErrorMessage { get; set; }

        public static readonly DependencyProperty ErrorLogProperty = DependencyProperty
        .Register("ErrorLog", typeof(List<double>), typeof(ErrorChart),
        new FrameworkPropertyMetadata(null));

        public event PropertyChangedEventHandler PropertyChanged;

        public ErrorChart()
        {
            DataContext = this;
            InitializeComponent();

            CreateChart();
        }


        private void CreateChart()
        {
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
            onPropertyChanged(nameof(SeriesCollection));
            onPropertyChanged(nameof(Labels));
        }

        protected void onPropertyChanged(string v)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(v));
        }


        //public ErrorChart(ErrorChartViewModel _vm)
        //{
        //    DataContext = vm = _vm;
        //    InitializeComponent();
        //}

        //private readonly ErrorChartViewModel vm;







        //public void CreateErrorChart(Grid grid)
        //{
        //    var chart = new CartesianChart();

        //    chart.Series = new SeriesCollection
        //    {
        //        new LineSeries
        //        {
        //            Title = "Wykres błędu uczenia",
        //            Values = new ChartValues<double>(Neuron.ErrorLog)
        //        }
        //    };

        //    int length = Neuron.ErrorLog.Count;
        //    Labels = new string[length];
        //    for (int i = 0; i < length; i++)
        //        Labels[i] = (i + 1).ToString();

        //    chart.AxisX = new AxesCollection
        //    {
        //        new Axis
        //        {
        //            Title = "Epoka",
        //            Labels = Labels,
        //            Separator = new LiveCharts.Wpf.Separator
        //            {
        //                Step = 1.0,
        //                IsEnabled = true
        //            }
        //        }
        //    };

        //    YFormatter = value => value.ToString();
        //    chart.AxisY = new AxesCollection
        //    {
        //        new Axis
        //        {
        //            Title="Błąd",
        //            MinValue = 0,
        //            LabelFormatter = YFormatter
        //        }
        //    };

        //}
    }
}
