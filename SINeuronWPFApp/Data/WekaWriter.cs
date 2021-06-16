using ArffTools;
using SINeuronLibrary;
using System;
using System.Collections.Generic;
using System.IO;
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

        public void SaveAppState(string path, NeuronBase neuron)
        {
            if (neuron == null)
                return;

            string trainingSetPath = Path.GetFileNameWithoutExtension(path);
            trainingSetPath += "_TrainingSet.arff";

            var perceptron = neuron as Perceptron;
            string relationName = perceptron != null ? "Perceptron" : "Adaline";
            using (var writer = new ArffWriter(path))
            {
                writer.WriteRelationName(relationName);
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.CompletedLearning), boolArffAttribute()));
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.CurrentError), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.EpochSize), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.EpochIterator), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.ErrorLog), ArffAttributeType.String));
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.ErrorTolerance), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.IterationCount), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.IterationMax), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.IterationWarning), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.LearningRate), ArffAttributeType.Numeric));
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.StopConditionErrorTolerance), boolArffAttribute()));
                writer.WriteAttribute(new ArffAttribute("TrainingSetFileName", ArffAttributeType.String));
                writer.WriteAttribute(new ArffAttribute(nameof(NeuronBase.Weights), ArffAttributeType.String));

                double currentError = neuron.CurrentError;
                if (neuron.EpochIterator == 0)
                    currentError = neuron.ErrorLog[neuron.ErrorLog.Count - 1];

                writer.WriteInstance(new object[]
                {
                        neuron.CompletedLearning ? 1 : 0,
                        currentError,
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
                result += $"{separator}{errorLog[i]}";

            result += "]";
            return result;
        }

        private string weightsToString(double[] weights)
        {
            if (weights == null)
                return $"[0{separator}0{separator}0]";

            return $"[{weights[0]}{separator}{weights[1]}{separator}{weights[2]}]";
        }

        private ArffAttributeType boolArffAttribute() => ArffAttributeType.Nominal("0", "1");

        private char separator = ';';
    }
}
