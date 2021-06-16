using ArffTools;
using SINeuronLibrary;
using System;
using System.Collections.Generic;

namespace SINeuronWPFApp.Data
{
    public class WekaReader
    {
        public void OpenSet(string path, List<ValuePoint> trainingSet)
        {
            using (var reader = new ArffReader(path))
            {
                var header = reader.ReadHeader();
                object[] instance;
                while ((instance = reader.ReadInstance()) != null)
                {
                    trainingSet.Add(new ValuePoint
                    {
                        X = (double)instance[0],
                        Y = (double)instance[1],
                        Value = ((int)instance[2] == 1) ? 1 : -1
                    });
                }
            }
        }


        public INeuron OpenAppState(string path)
        {
            INeuron neuron;
            using (var reader = new ArffReader(path))
            {
                var header = reader.ReadHeader();
                //if (header.RelationName == "Perceptron")
                neuron = new Perceptron();

                object[] instance;
                instance = reader.ReadInstance();

                neuron.CompletedLearning = ((int)instance[0] == 1) ? true : false;
                neuron.CurrentError = (double)instance[1];
                neuron.EpochSize = (int)((double)instance[2]);
                neuron.EpochIterator = (int)((double)instance[3]);
                neuron.ErrorLog = errorLogFromString((string)instance[4]);
                neuron.ErrorTolerance = (double)instance[5];
                neuron.IterationCount = (int)((double)instance[6]);
                neuron.IterationMax = (int)((double)instance[7]);
                neuron.IterationWarning = (int)((double)instance[8]);
                neuron.LearningRate = (double)instance[9];
                neuron.StopConditionErrorTolerance = ((int)instance[10] == 1) ? true : false;
                string trainingSetPath = (string)instance[11];
                neuron.Weights = weightsFromString((string)instance[12]);

                neuron.TrainingSet = new List<ValuePoint>();
                OpenSet(trainingSetPath, neuron.TrainingSet);
            }
            return neuron;
        }

        private List<double> errorLogFromString(string str)
        {
            var log = new List<double>();
            var splitted = getSplitted(str);
            foreach (var item in splitted)
            {
                double.TryParse(item, out double result);
                log.Add(result);
            }
            return log;
        }

        private double[] weightsFromString(string str)
        {
            var weights = new double[3];
            var splitted = getSplitted(str);
            for (int i = 0; i < 3; i++)
            {
                double.TryParse(splitted[i], out double result);
                weights[i] = result;
            }
            return weights;
        }

        private string[] getSplitted(string str)
        {
            str = str.Remove(str.Length - 1, 1);
            str = str.Remove(0, 1);
            return str.Split(';');
        }
    }
}
