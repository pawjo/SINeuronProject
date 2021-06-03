using SINeuronWPFApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SINeuronWPFApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Border ActiveBorder { get; set; }

        public bool IsSynchronized { get; set; }

        public Rectangle Line { get; set; }

        public INeuron Neuron { get; set; }

        public List<UIPoint> UIPoints { get; set; }

        public List<ValuePoint> TrainingSet { get; set; }

        public Canvas SpaceCanvas { get; set; }

        public double Weight0 { get => Neuron.Weights[0]; }

        public double Weight1 { get => Neuron.Weights[1]; }

        public double Weight2 { get => Neuron.Weights[2]; }

        public MainWindowViewModel(List<ValuePoint> trainingSet, Canvas spaceCanvas)
        {
            createPerceptron();
            SpaceCanvas = spaceCanvas;
            SpaceCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(SpaceCanvas_MouseDown);
            TrainingSet = trainingSet;
            UIPoints = new List<UIPoint>();
        }


        public void AddPoint()
        {
            var point = OpenPointDialogWindow("Dodaj nowy punkt.");
            if (point != null)
            {
                TrainingSet.Add(point);
                var uiPoint = createUIPoint(point);
                UIPoints.Add(uiPoint);
            }
        }

        public void DeletePoint(Border border)
        {
            for (int i = 0; i < UIPoints.Count; i++)
            {
                if(UIPoints[i].Border==border)
                {
                    SpaceCanvas.Children.Remove(border);
                    UIPoints.RemoveAt(i);
                    TrainingSet.RemoveAt(i);
                    return;
                }
            }
        }

        public void EditPoint(Border border)
        {
            var oldPoint = createValuePoint(border);
            if (oldPoint == null)
                return;
            var point = OpenPointDialogWindow("Edytuj punkt.", oldPoint);
            if (point != null)
            {
                if (oldPoint.X != point.X || oldPoint.Y != point.Y)
                    SetBorderCanvasPosition(border, point.X, SpaceCanvas.Height - point.Y);
                if (oldPoint.Value != point.Value)
                {
                    var textBlock = border.Child as TextBlock;
                    if (textBlock != null)
                        textBlock.Text = point.Value.ToString();
                }
            }
        }

        public bool InitializeNeuron()
        {
            if (UIPoints.Count < 2)
                return false;
            if (!IsSynchronized)
                Synchronize();

            Neuron.Initialize(TrainingSet);
            onPropertyChanged(nameof(Weight0));
            return true;
        }

        public void MoveElement(UIElement element, double dx, double dy)
        {
            Canvas.SetLeft(element, Canvas.GetLeft(element) + dx);
            Canvas.SetTop(element, Canvas.GetTop(element) + dy);
        }

        public void Synchronize()
        {
            TrainingSet.Clear();
            foreach (var item in UIPoints)
            {
                TrainingSet.Add(createValuePoint(item.Border));
            }
            IsSynchronized = true;
        }

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
            isDragging = false;
            var border = sender as Border;
            border.ReleaseMouseCapture();
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

        private void createPerceptron()
        {
            Neuron = new Perceptron
            {
                ErrorTolerance = 0.5,
                IterationMax = 100,
                LearningRate = 0.5,
                StopConditionErrorTolerance = true,
            };
        }

        private UIPoint createUIPoint(ValuePoint point)
        {
            var border = new Border();
            SetBorderCanvasPosition(border, point.X, SpaceCanvas.Height - point.Y);
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
            double x = Canvas.GetLeft(border) + Configuration.PointOffset;
            double y = SpaceCanvas.Height - Canvas.GetTop(border) - Configuration.PointOffset;
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

        private ValuePoint OpenPointDialogWindow(string text, ValuePoint point = null)
        {
            var pointVm = new PointDialogViewModel(text, point);
            var window = new PointDialogWindow(pointVm);
            window.ShowDialog();
            if (pointVm.ChangesSubmitted)
                return pointVm.Point;
            else
                return null;
        }

        private void SetBorderCanvasPosition(Border border, double x, double y)
        {
            double offset = Configuration.PointOffset;
            Canvas.SetLeft(border, x - offset);
            Canvas.SetTop(border, y - offset);
        }

        private void SpaceCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ActiveBorder != null && !ActiveBorder.IsMouseOver)
                deactivateBorder();
        }
    }
}
