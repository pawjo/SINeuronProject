using System.Collections.Generic;

namespace SINeuronLibrary
{
    public interface INeuron
    {
        public bool CompletedLearning { get; set; }

        public double CurrentError { get; set; }

        public int EpochSize { get; set; }

        public int EpochIterator { get; set; }

        public List<double> ErrorLog { get; set; }

        public double ErrorTolerance { get; set; }

        public int IterationCount { get; set; }

        public int IterationMax { get; set; }

        public int IterationWarning { get; set; }

        public double LearningRate { get; set; }
        
        public bool StopConditionErrorTolerance { get; set; }

        public List<ValuePoint> TrainingSet { get; set; }

        public double[] Weights { get; set; }

        public void AutoLearning();
        
        public int CalculateOutput(double x1, double x2);
        
        public bool CheckStopCondition();

        public void EpochLearning();

        public void FinalizeEpoch();

        public void Initialize(List<ValuePoint> trainingSet);

        public void Reset();

        public void StepLearning();
    }
}
