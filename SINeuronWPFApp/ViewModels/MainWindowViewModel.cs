using ArffTools;
using LiveCharts;
using LiveCharts.Wpf;
using SINeuronLibrary;
using SINeuronWPFApp.Data;
using SINeuronWPFApp.Models;
using SINeuronWPFApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SINeuronWPFApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(Canvas spaceCanvas)
        {
            createPerceptron();
            SpaceCanvas = spaceCanvas;
            SpaceCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(SpaceCanvas_MouseDown);
            TrainingSet = new List<ValuePoint>();
            UIPoints = new List<UIPoint>();
            createAxesLabels();
            CreateChart();
        }

        public Border ActiveBorder { get; set; }

        public Visibility VisibilityCompletedLearningMessage
        {
            get
            {
                if (Neuron != null && Neuron.CompletedLearning)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
        }

        public string CompletedLearningText
        {
            get
            {
                if (Neuron != null)
                {
                    if (Neuron.StopConditionErrorTolerance)
                        return "Osiągnięto maksymalny błąd docelowy.";
                    else
                        return "Osiągnięto maksymalną liczbę iteracji.";
                }
                else
                    return null;
            }
        }

        public double Error
        {
            get
            {
                if (Neuron.ErrorLog != null && Neuron.ErrorLog.Count > 0)
                    return Neuron.ErrorLog.Last();
                else
                    return 0;
            }
        }

        public bool IsPerceptron
        {
            get => true;
        }

        public bool IsSynchronized
        {
            get => isSynchronized;
            set
            {
                isSynchronized = value;
                onPropertyChanged(nameof(IsSynchronized));
            }
        }

        private bool isSynchronized;

        public int Iteration
        {
            get
            {
                if (Neuron != null)
                    return Neuron.IterationCount;
                else
                    return 0;
            }
        }

        public double LineAngle
        {
            get
            {
                if (Neuron.Weights != null)
                {
                    double result = Math.Atan(Neuron.Weights[1] / Neuron.Weights[2]) * (180 / Math.PI);
                    return result;
                }
                else
                    return 0;
            }
        }

        public double LineY
        {
            get
            {
                double result = Configuration.SpaceCanvasYOffset;
                if (Neuron.Weights != null)
                    result -= Neuron.Weights[0] / Neuron.Weights[2];
                return result;
            }
        }

        public bool NeuronButtonsEnabled
        {
            get
            {
                ;
                if (IsSynchronized && !Neuron.CompletedLearning)
                    return true;
                else
                    return false;
            }
        }

        public NeuronBase Neuron { get; set; }

        public SeriesCollection SeriesCollection { get; set; }

        public Canvas SpaceCanvas { get; set; }

        public List<ValuePoint> TrainingSet { get; set; }

        public List<UIPoint> UIPoints { get; set; }

        public double Weight0 { get => getWeight(0); }

        public double Weight1 { get => getWeight(1); }

        public double Weight2 { get => getWeight(2); }


        //=================================================
        // Obsluga plikow
        public void OpenSet(string path)
        {
            clearPoints();
            Reset();

            try
            {
                var reader = new WekaReader();
                reader.OpenSet(path, TrainingSet);
            }
            catch (Exception exc)
            {
                new Notification(exc.Message).ShowDialog();
            }
            SynchronizeFromTrainingSet();
        }

        public void SaveSet(string path)
        {
            Synchronize();

            try
            {
                var writer = new WekaWriter();
                writer.SaveSet(path, TrainingSet);
            }
            catch (Exception exc)
            {
                new Notification(exc.Message).ShowDialog();
            }
        }

        public void OpenAppStateWeka(string path)
        {
            try
            {
                var reader = new WekaReader();
                Neuron = reader.OpenAppState(path);
                TrainingSet = Neuron.TrainingSet;
                SynchronizeFromTrainingSet();
                IterationPropertyChanged();
                CompletedLearningPropertyChanged();
                CreateChart();
            }
            catch (Exception exc)
            {
                new Notification(exc.Message).ShowDialog();
            }
        }

        public void SaveAppStateWeka(string path)
        {
            Synchronize();

            try
            {
                var writer = new WekaWriter();
                writer.SaveAppState(path, Neuron);
            }
            catch (Exception exc)
            {
                new Notification(exc.Message).ShowDialog();
            }
        }

        //public void SaveSet(string path)
        //{
        //    Synchronize();

        //    try
        //    {
        //        using( var writer = new ArffWriter(path))
        //        {
        //            writer.WriteRelationName("Point");
        //            writer.WriteAttribute(new ArffAttribute(nameof(ValuePoint.X), ArffAttributeType.Numeric));
        //            writer.WriteAttribute(new ArffAttribute(nameof(ValuePoint.Y), ArffAttributeType.Numeric));
        //            writer.WriteAttribute(new ArffAttribute(nameof(ValuePoint.Value), ArffAttributeType.Nominal("-1","1")));

        //            foreach (var item in TrainingSet)
        //            {
        //                int val = (item.Value == 1) ? 1 : 0;
        //                writer.WriteInstance(new object[] { item.X, item.Y, val });
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        new Notification(exc.Message).ShowDialog();
        //    }
        //}

        //public void SaveAppStateWeka(string path)
        //{
        //    Synchronize();

        //    string trainingSetPath = path.Insert(
        //        path.IndexOf(".arff", path.Length - 5), "_TrainingSet");
        //    ;
        //    try
        //    {
        //        using (var writer = new ArffWriter(path))
        //        {
        //            writer.WriteRelationName(IsPerceptron ? "Perceptron" : "Adaline");
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.CompletedLearning), boolArffAttribute()));
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.CurrentError), ArffAttributeType.Numeric));
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.EpochSize), ArffAttributeType.Numeric));
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.EpochIterator), ArffAttributeType.Numeric));
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.ErrorLog), ArffAttributeType.String));
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.ErrorTolerance), ArffAttributeType.Numeric));
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.IterationCount), ArffAttributeType.Numeric));
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.IterationMax), ArffAttributeType.Numeric));
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.IterationWarning), ArffAttributeType.Numeric));
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.LearningRate), ArffAttributeType.Numeric));
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.StopConditionErrorTolerance), boolArffAttribute()));
        //            writer.WriteAttribute(new ArffAttribute("TrainingSetPath", ArffAttributeType.String));
        //            writer.WriteAttribute(new ArffAttribute(nameof(INeuron.Weights), ArffAttributeType.String));

        //            string errorLogString = "[";
        //            foreach (var item in Neuron.ErrorLog)
        //            {

        //            }

        //            writer.WriteInstance(new object[]
        //            {
        //                Neuron.CompletedLearning ? 1 : 0,
        //                Neuron.CurrentError,
        //                (double)Neuron.EpochSize,
        //                (double)Neuron.EpochIterator,
        //                Neuron.ErrorLog.ToString(),
        //                Neuron.ErrorTolerance,
        //                (double)Neuron.IterationCount,
        //                (double)Neuron.IterationMax,
        //                (double)Neuron.IterationWarning,
        //                Neuron.LearningRate,
        //                Neuron.StopConditionErrorTolerance ? 1 : 0,
        //                trainingSetPath,
        //                Neuron.Weights.ToString()
        //            });
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        new Notification(exc.Message).ShowDialog();
        //    }

        //    SaveSet(trainingSetPath);
        //}
        //private ArffAttributeType boolArffAttribute() => ArffAttributeType.Nominal("0", "1");

        public void OpenAppStateJson(string path)
        {
            string json = File.ReadAllText(path);
            Neuron = JsonSerializer.Deserialize<Perceptron>(json);
            IterationPropertyChanged();
            if (TrainingSet.Count == UIPoints.Count)
            {
                CreateChart();
                IsSynchronized = true;
            }
        }

        public void SaveAppStateJson(string path)
        {
            string json = JsonSerializer.Serialize(Neuron);
            File.WriteAllText(path, json);
        }
        //=================================================


        //=================================================
        // Do wykresu
        public string[] Labels { get; set; }

        public Func<double, string> YFormatter { get; set; }

        public double MaxValueY { get; set; }

        public void CreateChart()
        {
            List<double> values = Neuron.ErrorLog;
            if (values == null)
                values = new List<double>();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Wykres błędu uczenia",
                    Values = new ChartValues<double>(values)
                }
            };

            int length = values.Count;
            Labels = new string[length];
            for (int i = 0; i < length; i++)
                Labels[i] = (i + 1).ToString();

            YFormatter = value => value.ToString();

            onPropertyChanged(nameof(SeriesCollection));
            onPropertyChanged(nameof(Labels));
        }

        private void createAxesLabels()
        {
            var zeroLabel = new Label
            {
                Content = 0
            };
            SpaceCanvas.Children.Add(zeroLabel);
            Canvas.SetLeft(zeroLabel, SpaceCanvas.Width / 2 - 15);
            Canvas.SetTop(zeroLabel, SpaceCanvas.Height / 2 + 12);

            for (int i = -(int)(SpaceCanvas.Width / 2); i < SpaceCanvas.Width; i += 50)
                createXLabel(i);

            for (int i = -(int)(SpaceCanvas.Height / 2); i < SpaceCanvas.Height; i += 50)
                createYLabel(i);
        }

        private void createXLabel(int val)
        {
            if (val != 0)
            {
                string stringVal = val.ToString();

                var sp = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };

                var rect = new Rectangle
                {
                    Fill = Configuration.AxesBrush,
                    Width = 2,
                    Height = 20
                };
                sp.Children.Add(rect);

                var label = new Label
                {
                    Content = stringVal
                };
                sp.Children.Add(label);

                SpaceCanvas.Children.Add(sp);
                Canvas.SetLeft(sp, SpaceCanvas.Width / 2 - 3.5 - 3.5 * stringVal.Length + val);
                Canvas.SetTop(sp, SpaceCanvas.Height / 2 - 8);
            }
        }

        private void createYLabel(int val)
        {
            if (val != 0)
            {
                string stringVal = val.ToString();

                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                var label = new Label
                {
                    Content = stringVal
                };
                sp.Children.Add(label);

                var rect = new Rectangle
                {
                    Fill = Configuration.AxesBrush,
                    Width = 20,
                    Height = 2
                };
                sp.Children.Add(rect);

                SpaceCanvas.Children.Add(sp);
                Canvas.SetLeft(sp, Configuration.SpaceCanvasXOffset - 20 - 6 * stringVal.Length);
                Canvas.SetTop(sp, Configuration.SpaceCanvasYOffset - 12 - val);
            }
        }
        //==================================================




        public void AddPoint()
        {
            var point = openPointDialogWindow("Dodaj nowy punkt.");
            if (point != null)
            {
                TrainingSet.Add(point);
                var uiPoint = createUIPoint(point);
                UIPoints.Add(uiPoint);
                Reset();
            }
        }

        public void CompletedLearningPropertyChanged()
        {
            onPropertyChanged(nameof(VisibilityCompletedLearningMessage));
            onPropertyChanged(nameof(CompletedLearningText));
            onPropertyChanged(nameof(NeuronButtonsEnabled));
        }

        public void DeletePoint(Border border)
        {
            for (int i = 0; i < UIPoints.Count; i++)
            {
                if (UIPoints[i].Border == border)
                {
                    SpaceCanvas.Children.Remove(border);
                    UIPoints.RemoveAt(i);
                    TrainingSet.RemoveAt(i);
                    return;
                }
                IsSynchronized = false;
                Reset();
            }
        }

        public void EditPoint(Border border)
        {
            var oldPoint = createValuePoint(border);
            if (oldPoint == null)
                return;
            var point = openPointDialogWindow("Edytuj punkt.", oldPoint);
            if (point != null)
            {
                if (oldPoint.X != point.X || oldPoint.Y != point.Y)
                    setBorderCanvasPosition(border, point.X, point.Y);
                if (oldPoint.Value != point.Value)
                {
                    var textBlock = border.Child as TextBlock;
                    if (textBlock != null)
                        textBlock.Text = point.Value.ToString();
                }
                IsSynchronized = false;
                Reset();
            }
        }

        public bool InitializeNeuron()
        {
            if (UIPoints.Count < 2)
                return false;

            Synchronize();

            Neuron.Initialize(TrainingSet);
            //Neuron.Weights[0] = -304;
            //Neuron.Weights[1] = 40;
            //Neuron.Weights[2] = 23;
            CompletedLearningPropertyChanged();
            CreateChart();
            IterationPropertyChanged();
            return true;
        }

        public void Reset()
        {
            Neuron.Reset();
            CreateChart();
            IterationPropertyChanged();
            onPropertyChanged(nameof(NeuronButtonsEnabled));
        }

        public void IterationPropertyChanged()
        {
            onPropertyChanged(nameof(Error));
            onPropertyChanged(nameof(Iteration));
            onPropertyChanged(nameof(LineAngle));
            onPropertyChanged(nameof(LineY));
            onPropertyChanged(nameof(Weight0));
            onPropertyChanged(nameof(Weight1));
            onPropertyChanged(nameof(Weight2));
        }

        public void MoveElement(UIElement element, double dx, double dy)
        {
            Canvas.SetLeft(element, Canvas.GetLeft(element) + dx);
            Canvas.SetTop(element, Canvas.GetTop(element) + dy);
        }

        public void Settings()
        {
            var settingsVm = new SettingsViewModel
            {
                ErrorTolerance = Neuron.ErrorTolerance,
                IterationMax = Neuron.IterationMax,
                IterationWarning = Neuron.IterationWarning,
                StopConditionErrorTolerance = Neuron.StopConditionErrorTolerance
            };

            var settingsWindow = new SettingsWindow(settingsVm);
            settingsWindow.ShowDialog();
            if (settingsVm.ChangesSubmitted)
            {
                Neuron.ErrorTolerance = settingsVm.ErrorTolerance;
                Neuron.IterationMax = settingsVm.IterationMax;
                Neuron.IterationWarning = settingsVm.IterationWarning;
                Neuron.StopConditionErrorTolerance = settingsVm.StopConditionErrorTolerance;
                Reset();
            }
        }

        public void Synchronize()
        {
            if (!IsSynchronized)
            {
                TrainingSet.Clear();
                foreach (var item in UIPoints)
                {
                    TrainingSet.Add(createValuePoint(item.Border));
                }
                IsSynchronized = true;
            }
        }

        public void SynchronizeFromTrainingSet()
        {
            if (!IsSynchronized)
            {
                foreach (var item in TrainingSet)
                    UIPoints.Add(createUIPoint(item));
                IsSynchronized = true;
            }
        }



        //=================================================
        //Przeciąganie elementow
        private bool isDragging;
        private Point beforeMouseMove;

        private void activateBorder(Border border)
        {
            if (ActiveBorder != null)
                deactivateBorder();
            ActiveBorder = border;
            border.BorderBrush = Configuration.ActivePointBorderBrush;
            border.Background = Configuration.ActivePointBackgroundBrush;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                EditPoint(sender as Border);
            }
            else if (e.ClickCount == 1)
            {
                var border = sender as Border;
                activateBorder(border);
                isDragging = true;
                beforeMouseMove = e.GetPosition(SpaceCanvas);
                border.CaptureMouse();
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                var border = sender as Border;
                border.ReleaseMouseCapture();
                Reset();
            }
        }


        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            if (isDragging && border != null)
            {
                IsSynchronized = false;
                Point currentPosition = e.GetPosition(SpaceCanvas);
                double dx = currentPosition.X - beforeMouseMove.X;
                double dy = currentPosition.Y - beforeMouseMove.Y;
                MoveElement(border, dx, dy);
                beforeMouseMove = currentPosition;
            }
        }
        //=================================================


        private void clearPoints()
        {
            foreach (var item in UIPoints)
                SpaceCanvas.Children.Remove(item.Border);
            UIPoints.Clear();
            TrainingSet.Clear();
        }

        private void createPerceptron()
        {
            Neuron = new Perceptron
            {
                ErrorTolerance = 0.5,
                IterationMax = 20,
                IterationWarning = 1000,
                LearningRate = 0.5,
                StopConditionErrorTolerance = true,
            };
        }

        private void createPoint(ValuePoint point)
        {
            TrainingSet.Add(point);
            var uiPoint = createUIPoint(point);
            UIPoints.Add(uiPoint);
        }

        private UIPoint createUIPoint(ValuePoint point)
        {
            var border = new Border();
            setBorderCanvasPosition(border, point.X, point.Y);
            border.Width = Configuration.PointSize;
            border.Height = Configuration.PointSize;
            border.BorderBrush = Configuration.PointBorderBrush;
            border.Background = Configuration.PointBackgroundBrush;
            border.BorderThickness = new Thickness(1);
            border.CornerRadius = new CornerRadius(60);
            border.MouseLeftButtonDown += new MouseButtonEventHandler(Border_MouseLeftButtonDown);
            border.MouseLeftButtonUp += new MouseButtonEventHandler(Border_MouseLeftButtonUp);
            border.MouseMove += new MouseEventHandler(Border_MouseMove);
            SpaceCanvas.Children.Add(border);

            var textBlock = new TextBlock();
            textBlock.Text = point.Value.ToString();
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            border.Child = textBlock;

            return new UIPoint
            {
                Border = border,
                TextBlock = textBlock
            };
        }

        private ValuePoint createValuePoint(Border border)
        {
            double x = Canvas.GetLeft(border) - SpaceCanvas.Width / 2 + Configuration.PointOffset;
            double y = SpaceCanvas.Height / 2 - Canvas.GetTop(border) - Configuration.PointOffset;

            var textBlock = border.Child as TextBlock;
            if (textBlock != null)
                return new ValuePoint
                {
                    X = x,
                    Y = y,
                    Value = textBlock.Text == "1" ? 1 : -1
                };
            else
                return null;
        }

        private void deactivateBorder()
        {
            if (ActiveBorder != null)
            {
                ActiveBorder.BorderBrush = Configuration.PointBorderBrush;
                ActiveBorder.Background = Configuration.PointBackgroundBrush;
                ActiveBorder = null;
            }
        }

        private double getWeight(int i)
        {
            if (Neuron.Weights != null)
                return Neuron.Weights[i];
            else return 0;
        }

        private ValuePoint openPointDialogWindow(string text, ValuePoint point = null)
        {
            var pointVm = new PointDialogViewModel(text, point);
            var window = new PointDialogWindow(pointVm);
            window.ShowDialog();
            if (pointVm.ChangesSubmitted)
                return pointVm.Point;
            else
                return null;
        }

        private void setBorderCanvasPosition(Border border, double left, double bottom)
        {
            double offset = Configuration.PointOffset;

            Canvas.SetLeft(border, left - offset + SpaceCanvas.Width / 2);
            Canvas.SetTop(border, SpaceCanvas.Height / 2 - bottom - offset);
        }

        private void SpaceCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ActiveBorder != null && !ActiveBorder.IsMouseOver)
                deactivateBorder();
        }
    }
}
