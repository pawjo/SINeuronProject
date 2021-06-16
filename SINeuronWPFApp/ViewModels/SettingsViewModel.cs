namespace SINeuronWPFApp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public bool ChangesSubmitted { get; set; }

        public bool IsAdaline
        {
            get => !isPerceptron;
            set
            {
                isPerceptron = !value;
                onPropertyChanged(nameof(IsAdaline));
                onPropertyChanged(nameof(IsPerceptron));
            }
        }
        
        public bool IsPerceptron
        {
            get => isPerceptron;
            set
            {
                isPerceptron = value;
                onPropertyChanged(nameof(IsAdaline));
                onPropertyChanged(nameof(IsPerceptron));
            }
        }

        public double LearningRate
        {
            get => learningRate;
            set
            {
                learningRate = value;
                
                onPropertyChanged(nameof(LearningRate));
                onPropertyChanged(nameof(ModelIsValid));
            }
        }
        
        public bool ModelIsValid { get => modelIsValid; set => modelIsValid = value; }

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
            set
            {
                stopConditionErrorTolerance = !value;
                onPropertyChanged(nameof(StopConditionErrorTolerance));
                onPropertyChanged(nameof(StopConditionMaxIteration));
            }
        }

        public double ErrorTolerance { get; set; }

        public int IterationMax { get; set; }

        public int IterationWarning { get; set; }

        private bool isPerceptron;

        private double learningRate;

        private bool modelIsValid = true;

        private bool stopConditionErrorTolerance;
    }
}
