using SINeuronLibrary;

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
            set
            {
                if (value)
                    Point.Value = 1;
                onPropertyChanged(nameof(IsValue1));
                onPropertyChanged(nameof(IsValueMinus1));
            }
        }

        public bool IsValueMinus1
        {
            get => Point.Value == -1;
            set
            {
                if (value)
                    Point.Value = -1;
                onPropertyChanged(nameof(IsValue1));
                onPropertyChanged(nameof(IsValueMinus1));
            }
        }
    }
}
