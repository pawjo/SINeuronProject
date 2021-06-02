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
        public MainWindowViewModel(List<ValuePoint> trainingSet, Canvas spaceCanvas)
        {
            UIPoints = new List<UIPoint>();
            SpaceCanvas = spaceCanvas;
            TrainingSet = trainingSet;
        }
        
        public bool IsSynchronized { get; set; }

        public List<UIPoint> UIPoints { get; set; }

        public List<ValuePoint> TrainingSet { get; set; }

        public Canvas SpaceCanvas { get; set; }



        //public void Refresh()
        //{
        //    if (Ellipses.Count != TrainingSet.Count)
        //    {
        //        if()
        //    }
        //}


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


        //public void AddPoint(ValuePoint p)
        //{
        //    TrainingSet.Add(p);
        //    var uiPoint = createUIPoint(p);
        //    UIPoints.Add(uiPoint);
        //}


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
                Border=border,
                TextBlock=textBlock
            };
        }


        private bool isDragging;
        private Point beforeMouseMove;


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                EditPoint(sender as Border);
            }
            else if(e.ClickCount == 1)
            {
                var draggableControl = sender as UIElement;
                isDragging = true;
                beforeMouseMove = e.GetPosition(SpaceCanvas);
                draggableControl.CaptureMouse();
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = sender as UIElement;
            draggable.ReleaseMouseCapture();
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
        //private void Border_MouseMove(object sender, MouseEventArgs e)
        //{
        //    var border = sender as Border;
        //    if (isDragging && border != null)
        //    {
        //        Point currentPosition = e.GetPosition(SpaceCanvas);
        //        SetBorderCanvasPosition(border, currentPosition.X, currentPosition.Y);
        //    }
        //}


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


        //private UIPoint createUIPoint(Point point)
        //{
        //    var canvas = new Canvas();
        //    double offset = Configuration.PointSize / 2;
        //    Canvas.SetLeft(canvas, point.X - offset);
        //    Canvas.SetBottom(canvas, point.Y + offset);
        //    SpaceCanvas.Children.Add(canvas);

        //    var ellipse = new Ellipse();
        //    ellipse.Height = Configuration.PointSize;
        //    ellipse.Width = Configuration.PointSize;
        //    ellipse.Stroke = Configuration.PointBrush;
        //    ellipse.StrokeThickness = 1;
        //    canvas.Children.Add(ellipse);

        //    var label = new Label();
        //    label.Content = point.Value.ToString();
        //    double labelLeft = (double)(Configuration.PointSize - label.Width) / 2;
        //    double labelbottom = (double)(Configuration.PointSize - label.Height) / 2;
        //    Canvas.SetLeft(label,  labelLeft);
        //    Canvas.SetBottom(label, labelbottom);
        //    canvas.Children.Add(label);

        //    return new UIPoint
        //    {
        //        Canvas = canvas,
        //        Ellipse = ellipse,
        //        Label = label
        //    };
        //}
    }
}
