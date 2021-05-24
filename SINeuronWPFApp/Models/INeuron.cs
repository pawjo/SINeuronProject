using System.Collections.Generic;

namespace SINeuronWPFApp.Models
{
    public interface INeuron
    {
        public double CurrentError { get; set; }

        public int EpochSize { get; set; }

        public int EpochIterator { get; set; }

        public double ErrorTolerance { get; set; }

        public double LearningRate { get; set; }

        public double[] Weights { get; set; }

        public bool AutoLearning();
        
        public int CalculateOutput(double x1, double x2);
        
        public bool CheckStopCondition();

        public void EpochLearning(List<Point> epoch);

        public void InitializeWeight();

        public void StepLearning(Point point);
    }
}
