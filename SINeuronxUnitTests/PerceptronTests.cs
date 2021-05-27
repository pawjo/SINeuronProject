using SINeuronWPFApp.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace SINeuronxUnitTests
{
    public class PerceptronTests
    {
        public INeuron neuron;

        [Fact]
        public void NoWeightsModification()
        {
            List<Point> set = CreateExampleSet1();
            int expected = 0;
            int result = modifyWeightTestShema(3, 15, 23, set);
            Assert.Equal(expected, result);
        }



        [Fact]
        public void EpochLearningTestNoModification()
        {
            InitializeNeuronWithWeights(3, 15, 23, CreateExampleSet1());
            var w = new double[neuron.Weights.Length];
            setWeights(w);
            neuron.EpochLearning();
            bool expected = true;
            bool result = CheckWeightsEqual(w);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EpochLearningTestWithModification()
        {
            InitializeNeuronWithWeights(-100, 15, 23, CreateExampleSet1());
            var w = new double[neuron.Weights.Length];
            setWeights(w);
            neuron.EpochLearning();
            bool expected = false;
            bool result = CheckWeightsEqual(w);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AutoLearningTest1()
        {
            InitializeNeuronWithWeights(-100, 15, 23, CreateExampleSet1());
            neuron.AutoLearning();
            bool expected = true;
            bool result = neuron.CompletedLearning;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AutoLearningTest2()
        {
            InitializeNeuronWithWeights(304, 45, 23, CreateExampleSet1());
            neuron.AutoLearning();
            bool expected = true;
            bool result = neuron.CompletedLearning;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AutoLearningTest3()
        {
            InitializeNeuronWithWeights(-304, -40, 23, CreateExampleSet2());
            neuron.AutoLearning();
            bool expected = true;
            bool result = neuron.CompletedLearning;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AutoLearningTest4()
        {
            InitializeNeuronWithWeights(-304, -40, 200, CreateExampleSet2());
            neuron.AutoLearning();
            bool expected = true;
            bool result = neuron.CompletedLearning;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AutoLearningTest5_noSolution()
        {
            InitializeNeuronWithWeights(-304, -40, 200, CreateExampleSet3_noSolution());
            neuron.AutoLearning();
            bool expected = false;
            bool result = neuron.CompletedLearning;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MixedLearningTest1()
        {
            InitializeNeuronWithWeights(304, 45, 23, CreateExampleSet1());
            neuron.StepLearning();
            neuron.StepLearning();
            neuron.EpochLearning();
            neuron.AutoLearning();
            bool expected = true;
            bool result = neuron.CompletedLearning;
            Assert.Equal(expected, result);
        }

        private string NeuronWeightsToString()
        {
            return $"w0 = {neuron.Weights[0]}," +
                $" w1 = {neuron.Weights[1]}," +
                $" w2 = {neuron.Weights[2]}";
        }

        private static List<Point> CreateExampleSet1()
        {
            var set = new List<Point>();
            set.Add(new Point { X = 1, Y = 2, Value = 1 });
            set.Add(new Point { X = 5, Y = 6, Value = 1 });

            set.Add(new Point { X = -5, Y = 0, Value = -1 });
            return set;
        }

        private static List<Point> CreateExampleSet2()
        {
            var set = new List<Point>();
            set.Add(new Point { X = -8, Y = 3, Value = 1 });
            set.Add(new Point { X = -9, Y = 5, Value = 1 });
            set.Add(new Point { X = -5, Y = 0, Value = 1 });
            set.Add(new Point { X = -6, Y = 10, Value = 1 });
            set.Add(new Point { X = -4, Y = 6, Value = 1 });

            set.Add(new Point { X = 2, Y = 8, Value = -1 });
            set.Add(new Point { X = 5, Y = 12, Value = -1 });
            set.Add(new Point { X = 5, Y = 6, Value = -1 });
            set.Add(new Point { X = 1, Y = 2, Value = -1 });
            set.Add(new Point { X = 6, Y = 14, Value = -1 });
            return set;
        }

        private static List<Point> CreateExampleSet3_noSolution()
        {
            var set = new List<Point>();
            set.Add(new Point { X = -8, Y = 3, Value = 1 });
            set.Add(new Point { X = -9, Y = 5, Value = 1 });
            set.Add(new Point { X = -5, Y = 0, Value = 1 });
            set.Add(new Point { X = -6, Y = 10, Value = 1 });
            set.Add(new Point { X = -4, Y = 6, Value = 1 });

            set.Add(new Point { X = 2, Y = 8, Value = -1 });
            set.Add(new Point { X = 5, Y = 12, Value = -1 });
            set.Add(new Point { X = 5, Y = 6, Value = -1 });
            set.Add(new Point { X = 1, Y = 2, Value = -1 });
            set.Add(new Point { X = 6, Y = 14, Value = -1 });

            // punkt psujacy
            set.Add(new Point { X = 7, Y = 12, Value = 1 });


            return set;
        }

        private int modifyWeightTestShema(double w0, double w1, double w2, List<Point> set)
        {
            InitializeNeuronWithWeights(w0, w1, w2, set);
            var w = new double[neuron.Weights.Length];
            int result = 0;

            for (int i = 0; i < 3; i++)
            {
                setWeights(w);
                neuron.StepLearning();
                if (!CheckWeightsEqual(w))
                    result++;
            }
            return result;
        }

        private void InitializeNeuronWithWeights(double w0, double w1, double w2, List<Point> set)
        {
            CreatePerceptron();

            neuron.Initialize(set);
            neuron.Weights[0] = w0;
            neuron.Weights[1] = w1;
            neuron.Weights[2] = w2;
        }

        private void setWeights(double[] w)
        {
            for (int i = 0; i < neuron.Weights.Length; i++)
            {
                w[i] = neuron.Weights[i];
            }
        }
        private bool CheckWeightsEqual(double[] w)
        {
            for (int i = 0; i < neuron.Weights.Length; i++)
            {
                if (w[i] != neuron.Weights[i])
                    return false;
            }
            return true;
        }
        private void CreatePerceptron()
        {
            neuron = new Perceptron
            {
                ErrorTolerance = 0.5,
                IterationMax = 100,
                LearningRate = 0.5,
                StopConditionErrorTolerance = true,
            };
        }
    }
}