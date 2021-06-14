namespace SINeuronWPFApp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public bool ChangesSubmitted { get; set; }

        public bool StopConditionErrorTolerance 
        {
            get => stopConditionErrorTolerance; 
            set
            {
                stopConditionErrorTolerance = value;
                onPropertyChanged(nameof(StopConditionErrorTolerance));
                onPropertyChanged(nameof(StopConditionMaxIteration));
            }
        }
        
        public bool StopConditionMaxIteration
        {
            get => !stopConditionErrorTolerance; 
        }

        public double ErrorTolerance { get; set; }

        public int IterationMax { get; set; }

        public int IterationWarning { get; set; }

        private bool stopConditionErrorTolerance;
    }
}
