﻿using System.Collections.Generic;

namespace SINeuronWPFApp.Models
{
    public interface INeuron
    {
        public bool CompletedLearning { get; set; }

        public double CurrentError { get; set; }

        public int EpochSize { get; set; }

        public int EpochIterator { get; set; }

        public double ErrorTolerance { get; set; }

        public int IterationCount { get; set; }

        public int IterationMax { get; set; }

        public double LearningRate { get; set; }
        
        public bool StopConditionErrorTolerance { get; set; }

        public List<Point> TrainingSet { get; set; }

        public double[] Weights { get; set; }

        public bool AutoLearning();
        
        public int CalculateOutput(double x1, double x2);
        
        public bool CheckStopCondition();

        public bool EpochLearning();

        public void FinalizeEpoch();

        public void Initialize(List<Point> trainingSet);

        public void StepLearning();
    }
}
