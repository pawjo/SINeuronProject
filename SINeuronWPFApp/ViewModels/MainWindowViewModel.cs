using SINeuronWPFApp.Models;
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

        public void AddPoint(ValuePoint p)
        {
            TrainingSet.Add(p);
            var uiPoint = createUIPoint(p);
            UIPoints.Add(uiPoint);
        }

        private UIPoint createUIPoint(ValuePoint point)
        {
            var border = new Border();
            double offset = Configuration.PointSize / 2;
            Canvas.SetLeft(border, point.X - offset);
            Canvas.SetBottom(border, point.Y - offset);
            border.Width = Configuration.PointSize;
            border.Height = Configuration.PointSize;
            border.BorderBrush = Configuration.PointBrush;
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


        protected bool isDragging;
        private Point clickPosition;


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var draggableControl = sender as UIElement;
            isDragging = true;
            clickPosition = e.GetPosition(SpaceCanvas);
            draggableControl.CaptureMouse();
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = sender as UIElement;
            draggable.ReleaseMouseCapture();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = sender as UIElement;
            if (isDragging && draggableControl != null)
            {
                Point currentPosition = e.GetPosition(SpaceCanvas);
                Canvas.SetLeft(draggableControl, currentPosition.X);
                Canvas.SetTop(draggableControl, currentPosition.Y);
            }
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

        public void Move(Ellipse e)
        {
        }
    }
}
