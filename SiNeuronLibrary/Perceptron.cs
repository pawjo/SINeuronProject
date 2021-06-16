using System;
using System.Collections.Generic;

namespace SINeuronLibrary
{
    public class Perceptron : NeuronBase
    {
        public Perceptron() : base()
        {
        }

        public override void FinalizeEpoch()
        {
            EpochIterator = 0;
            CurrentError /= 2;
            if (CheckStopCondition())
                CompletedLearning = true;
            ErrorLog.Add(CurrentError);
            CurrentError = 0;
        }

        public override void StepLearning()
        {
            var point = stepLearningBase();

            double sum = inputSum(point);
            int y = calculateOutput(sum);

            if (y != point.Value)
                modifyWeights(point);

            // Liczenie błędu
            CurrentError += Math.Pow(point.Value - y, 2);
        }

        private void modifyWeights(ValuePoint point)
        {
            Weights[0] = Weights[0] + point.Value;
            Weights[1] = Weights[1] + point.X * point.Value;
            Weights[2] = Weights[2] + point.Y * point.Value;
        }
    }
}
