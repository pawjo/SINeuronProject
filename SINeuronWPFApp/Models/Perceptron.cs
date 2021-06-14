using System;
using System.Collections.Generic;

namespace SINeuronWPFApp.Models
{
    public class Perceptron : INeuron
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

        // Uczenie w zaleznosci od wybory uzytkownika,
        // trwa do momentu osiagniecia okreslonego bledu
        // lub do momentu przekroczenia maksymalnej liczby iteracji.
        // w pierwszym przypadku, w razie przekroczenia liczby
        // bezpieczenstwa iteracji, zostaje wyrzucony
        // wyjatek, dotyczacy zbyt dlugiego uczenia.
        public void AutoLearning()
        {
            while(!CompletedLearning)
            {
                EpochLearning();
            }
        }

        public int CalculateOutput(double x1, double x2)
        {
            double result = Weights[0] + Weights[1] * x1 + Weights[2] * x2;
            return result > 0 ? 1 : -1;
        }

        public bool CheckStopCondition()
        {
            if (StopConditionErrorTolerance)
                return CurrentError <= ErrorTolerance;
            else
                return IterationCount == IterationMax;
        }

        public void EpochLearning()
        {
            if (CompletedLearning)
                return;

            while(EpochIterator < EpochSize)
            {
                StepLearning();
            }
            FinalizeEpoch();
        }

        // Konczy uczenie epoki, jezeli warunek zatrzymania jest spełniony,
        // ustawia koniec uczenia.
        public void FinalizeEpoch()
        {
            EpochIterator = 0;
            CurrentError /= 2;
            if (CheckStopCondition())
                CompletedLearning = true;
            ErrorLog.Add(CurrentError);
            CurrentError = 0;
        }

        public void Initialize(List<ValuePoint> trainingSet)
        {
            CompletedLearning = false;
            CurrentError = 0;
            EpochIterator = 0;
            EpochSize = trainingSet.Count;
            IterationCount = 0;
            
            if (ErrorLog == null)
                ErrorLog = new List<double>();
            else
                ErrorLog.Clear();

            TrainingSet = trainingSet;
            var random = new Random();
            if (Weights == null)
                Weights = new double[3];

            for (int i = 0; i < Weights.Length; i++)
                Weights[i] = random.Next(-300, 300) + random.NextDouble();
        }

        // Wykonuje jeden krok uczenia dla jednego obiektu.
        public void StepLearning()
        {
            if (CompletedLearning)
                return;

            // Jeżeli warunkiem zatrzymania jest dopuszczalny blad,
            // sprawdzane jest czy liczba iteracji nie przekracza progu bezpieczenstwa,
            // przy ktorym uczenienie jest wstrzymywane i pokazywane jest powiadomienie.
            if (StopConditionErrorTolerance
                && IterationCount == IterationWarning)
                throw new Exception($"Przekroczono próg bezpieczeństwa" +
                    $" {IterationWarning} iteracji.");

            if(!StopConditionErrorTolerance
                && IterationCount == IterationMax)
                throw new Exception($"Przekroczono określony próg" +
                    $" {IterationMax} iteracji.");

            if (TrainingSet == null)
                throw new NullReferenceException("Zbior testowy jest nullem.");

            if (EpochIterator == EpochSize)
                FinalizeEpoch();
            
            var point = TrainingSet[EpochIterator++];

            int y = CalculateOutput(point.X, point.Y);

            if (y != point.Value)
                ModifyWeights(point);

            // Liczenie błędu
            CurrentError += Math.Pow(point.Value - y, 2);
            IterationCount++;
        }

        private void ModifyWeights(ValuePoint point)
        {
            Weights[0] = Weights[0] + point.Value;
            Weights[1] = Weights[1] + point.X * point.Value;
            Weights[2] = Weights[2] + point.Y * point.Value;
        }
    }
}
