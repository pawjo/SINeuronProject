using SINeuronWPFApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfModelTetris.ViewModels;

namespace SINeuronWPFApp.ViewModels
{
    public class PointDialogViewModel : ViewModelBase
    {
        public bool ChangesSubmitted { get; set; }

        public ValuePoint Point { get; set; }
        
        public string Text { get; set; }

        public PointDialogViewModel(string text, ValuePoint point = null)
        {
            ChangesSubmitted = false;
            if (point != null)
                Point = new ValuePoint
                {
                    X = point.X,
                    Y = point.Y,
                    Value = point.Value
                };
            else
                Point = new ValuePoint
                {
                    Value = 1
                };
            Text = text;
        }

        public bool IsValue1
        {
            get => Point.Value == 1;
        }

        public bool IsValueMinus1
        {
            get => Point.Value == -1;
        }
    }
}
