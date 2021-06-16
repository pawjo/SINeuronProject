using System.Collections.Generic;

namespace SINeuronLibrary
{
    public abstract class NeuronBase
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

        public abstract void AutoLearning();
        
        public abstract int CalculateOutput(double x1, double x2);

        public bool CheckStopCondition()
        {
            if (StopConditionErrorTolerance)
                return CurrentError <= ErrorTolerance;
            else
                return IterationCount == IterationMax;
        }

        public abstract void EpochLearning();

        public abstract void FinalizeEpoch();

        public abstract void Initialize(List<ValuePoint> trainingSet);

        public void Reset()
        {
            CompletedLearning = false;
            CurrentError = 0;
            EpochIterator = 0;
            IterationCount = 0;

            if (ErrorLog == null)
                ErrorLog = new List<double>();
            else
                ErrorLog.Clear();

            if (Weights == null)
                Weights = new double[3];
            for (int i = 0; i < 3; i++)
                Weights[i] = 0;
        }

        public abstract void StepLearning();
    }
}
