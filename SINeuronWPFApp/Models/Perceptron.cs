using System;
using System.Collections.Generic;

namespace SINeuronWPFApp.Models
{
    public class Perceptron : INeuron
    {
        public double CurrentError { get; set; }

        public int EpochSize { get; set; }
        
        public int EpochIterator { get; set; }

        public double ErrorTolerance { get; set; }
        
        public double LearningRate { get; set; }
        

        public double[] Weights { get; set; }

        public bool AutoLearning()
        {
            throw new NotImplementedException();
        }

        public int CalculateOutput(double x1, double x2)
        {
            double result = Weights[0] + Weights[1] * x1 + Weights[2] * x2;
            return result > 0 ? 1 : -1;
        }

        public bool CheckStopCondition()
        {
            throw new NotImplementedException();
        }

        public void EpochLearning(List<Point> epoch)
        {
            foreach (var point in epoch)
            {
                StepLearning(point);
            }
        }

        public void InitializeWeight()
        {
            var random = new Random();
            for (int i = 0; i < Weights.Length; i++)
                Weights[i] = random.Next() + random.NextDouble();
        }

        public void StepLearning(Point point)
        {
            int y = CalculateOutput(point.X, point.Y);

            if (y != point.Value)
                ModifyWeights(point);
        }

        private void ModifyWeights(Point point)
        {
            Weights[0] = Weights[0] + point.Value;
            Weights[1] = Weights[1] + point.X * point.Value;
            Weights[2] = Weights[2] + point.Y * point.Value;
        }
    }
}
