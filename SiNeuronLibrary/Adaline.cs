using System;
using System.Collections.Generic;

namespace SINeuronLibrary
{
    public class Adaline : NeuronBase
    {
        public Adaline() : base()
        {
        }

        public override void FinalizeEpoch()
        {
            EpochIterator = 0;
            CurrentError /= EpochSize;
            if (CheckStopCondition())
                CompletedLearning = true;
            ErrorLog.Add(CurrentError);
            CurrentError = 0;
        }

        public override void Initialize(List<ValuePoint> trainingSet)
        {
            Reset();
            EpochSize = trainingSet.Count;
            TrainingSet = trainingSet;
            var random = new Random();
            for (int i = 0; i < Weights.Length; i++)
                Weights[i] = random.NextDouble();
        }

        public override void StepLearning()
        {
            var point = stepLearningBase();

            double sum = inputSum(point);
            double delta = point.Value - sum;

            modifyWeights(point, delta);

            // Liczenie błędu
            CurrentError += Math.Pow(delta, 2);
        }

        private void modifyWeights(ValuePoint point, double delta)
        {
            double factor = LearningRate * delta;
            Weights[0] = Weights[0] + factor;
            Weights[1] = Weights[1] + factor * point.X;
            Weights[2] = Weights[2] + factor * point.Y;
        }
    }
}
