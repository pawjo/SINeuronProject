using System;
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

        public int IterationWarning
        {
            get => iterationWarning; 
            set
            {
                iterationWarning = value;
                iterationWarningActual = value;
            }
        }

        public double LearningRate { get; set; }
        
        public bool StopConditionErrorTolerance { get; set; }

        public List<ValuePoint> TrainingSet { get; set; }

        public double[] Weights { get; set; }

        public NeuronBase()
        {
            Weights = new double[3] { 0, 0, 0 };
        }

        // Uczenie w zaleznosci od wybory uzytkownika,
        // trwa do momentu osiagniecia okreslonego bledu
        // lub do momentu przekroczenia maksymalnej liczby iteracji.
        // w pierwszym przypadku, w razie przekroczenia liczby
        // bezpieczenstwa iteracji, zostaje wyrzucony
        // wyjatek, dotyczacy zbyt dlugiego uczenia.
        public void AutoLearning()
        {
            while (!CompletedLearning)
            {
                EpochLearning();
            }
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

            while (EpochIterator < EpochSize)
            {
                StepLearning();
            }
            FinalizeEpoch();
        }

        // Konczy uczenie epoki, jezeli warunek zatrzymania jest spełniony,
        // ustawia koniec uczenia.
        public abstract void FinalizeEpoch();

        public virtual void Initialize(List<ValuePoint> trainingSet)
        {
            Reset();
            EpochSize = trainingSet.Count;
            TrainingSet = trainingSet;
            var random = new Random();
            for (int i = 0; i < Weights.Length; i++)
                Weights[i] = random.Next(-300, 300) + random.NextDouble();
        }

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

        // Wykonuje jeden krok uczenia dla jednego obiektu.
        public abstract void StepLearning();


        protected int calculateOutput(double inputSum)
        {
            return inputSum > 0 ? 1 : -1;
        }

        protected ValuePoint stepLearningBase()
        {
            if (CompletedLearning)
                throw new Exception("Zakończono uczenie.");

            // Jeżeli warunkiem zatrzymania jest dopuszczalny blad,
            // sprawdzane jest czy liczba iteracji nie przekracza progu bezpieczenstwa,
            // przy ktorym uczenienie jest wstrzymywane i pokazywane jest powiadomienie.
            if (StopConditionErrorTolerance
                && IterationCount == iterationWarningActual)
            {
                iterationWarningActual += iterationWarning;
                throw new Exception($"Przekroczono próg bezpieczeństwa" +
                      $" {IterationWarning} iteracji.");
            }

            if (!StopConditionErrorTolerance
                && IterationCount == IterationMax)
                throw new Exception($"Przekroczono określony próg" +
                    $" {IterationMax} iteracji.");

            if (TrainingSet == null)
                throw new NullReferenceException("Zbior testowy jest nullem.");

            if (EpochIterator == EpochSize)
                FinalizeEpoch();

            IterationCount++;
            return TrainingSet[EpochIterator++];
        }

        protected double inputSum(ValuePoint point)
        {
            return Weights[0] + Weights[1] * point.X + Weights[2] * point.Y;
        }

        private int iterationWarning;

        private int iterationWarningActual;
    }
}
