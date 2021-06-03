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

        public double ErrorTolerance { get; set; }
        
        public int IterationCount { get; set; }

        public int IterationMax { get; set; }

        public double LearningRate { get; set; }

        public bool StopConditionErrorTolerance { get; set; }

        public List<ValuePoint> TrainingSet { get; set; }

        public double[] Weights { get; set; }

        // Metoda zwraca true w przypadku pomyslnego zakonczenia
        // uczenia w zależnosci od wyboru użytkownika, czyli
        // zostanie osiągniety blad docelowy lub zostanie osiągnieta
        // maksymalna liczba iteracji.
        // Jezeli maksymalna liczba iteracji zostanie przekroczona,
        // a użytkownik określił warunek zatrzymania jako blad docelowy,
        // zwracana jest wartosc false.
        public bool AutoLearning()
        {
            IterationCount = 0;

            while(!CompletedLearning)
            {
                if (!EpochLearning())
                    return false;
            }
            return true;
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
                return IterationCount > IterationMax;
        }

        // Jezeli maksymalna liczba iteracji zostanie przekroczona zanim
        // zakonczy sie uczenie dla calej epoki, zwracana jest wartosc false.
        // W innym wypadku zwracana jest wartosc true.
        public bool EpochLearning()
        {
            while(EpochIterator < EpochSize)
            {
                if (IterationCount > IterationMax)
                    return false;
                StepLearning();
            }
            FinalizeEpoch();
            return true;
        }

        // Konczy uczenie epoki, jezeli warunek zatrzymania jest spełniony,
        // ustawia koniec uczenia.
        public void FinalizeEpoch()
        {
            EpochIterator = 0;
            CurrentError /= 2;
            if (CheckStopCondition())
                CompletedLearning = true;
            CurrentError = 0;
        }

        public void Initialize(List<ValuePoint> trainingSet)
        {
            CompletedLearning = false;
            CurrentError = 0;
            EpochIterator = 0;
            EpochSize = trainingSet.Count;
            IterationCount = 0;
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
