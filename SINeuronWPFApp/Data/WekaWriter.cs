using ArffTools;
using SINeuronLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SINeuronWPFApp.Data
{
    public class WekaWriter
    {
        public void SaveSet(string path, List<ValuePoint> trainingSet)
        {
            using (var writer = new ArffWriter(path))
            {
                writer.WriteRelationName("Point");
                writer.WriteAttribute(new ArffAttribute(nameof(ValuePoint.X), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(ValuePoint.Y), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(ValuePoint.Value), ArffAttributeType.Nominal("-1", "1")));

                foreach (var item in trainingSet)
                {
                    int val = (item.Value == 1) ? 1 : 0;
                    writer.WriteInstance(new object[] { item.X, item.Y, val });
                }
            }
        }

        public void SaveAppState(string path, INeuron neuron)
        {
            if (neuron == null)
                return;

            string trainingSetPath = path.Insert(
                path.IndexOf(".arff", path.Length - 5), "_TrainingSet");

            var perceptron = neuron as Perceptron;
            string relationName = perceptron != null ? "Perceptron" : "Adaline";
            using (var writer = new ArffWriter(path))
            {
                writer.WriteRelationName(relationName);
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.CompletedLearning), boolArffAttribute()));
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.CurrentError), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.EpochSize), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.EpochIterator), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.ErrorLog), ArffAttributeType.String));
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.ErrorTolerance), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.IterationCount), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.IterationMax), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.IterationWarning), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.LearningRate), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.StopConditionErrorTolerance), boolArffAttribute()));
                writer.WriteAttribute(new ArffAttribute("TrainingSetPath", ArffAttributeType.String));
                writer.WriteAttribute(new ArffAttribute(nameof(INeuron.Weights), ArffAttributeType.String));

                writer.WriteInstance(new object[]
                {
                        neuron.CompletedLearning ? 1 : 0,
                        neuron.CurrentError,
                        (double)neuron.EpochSize,
                        (double)neuron.EpochIterator,
                        errorLogToString(neuron.ErrorLog),
                        neuron.ErrorTolerance,
                        (double)neuron.IterationCount,
                        (double)neuron.IterationMax,
                        (double)neuron.IterationWarning,
                        neuron.LearningRate,
                        neuron.StopConditionErrorTolerance ? 1 : 0,
                        trainingSetPath,
                        weightsToString(neuron.Weights)
                });
            }

            SaveSet(trainingSetPath, neuron.TrainingSet);
        }

        private string errorLogToString(List<double> errorLog)
        {
            string result = "[";
            if (errorLog != null && errorLog.Count > 0)
                result += errorLog[0];

            for (int i = 1; i < errorLog.Count; i++)
                result += "," + errorLog[i];

            result += "]";
            return result;
        }

        private string weightsToString(double[] weights)
        {
            if (weights == null)
                return "[0, 0, 0]";

            return $"[{weights[0]},{weights[1]},{weights[2]}]";
        }

        private ArffAttributeType boolArffAttribute() => ArffAttributeType.Nominal("0", "1");
    }
}
