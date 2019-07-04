using NameRandomizer.CanvasDrawings.Removers;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NameRandomizer.CanvasDrawings
{
    class CirclingEllipse
    {
        private Canvas canvas;
        private Timer timer = new Timer();
        private Ellipse circle = new Ellipse() { Width = 10, Height = 10, Fill = Brushes.Black };

        public CirclingEllipse(Canvas canvas)
        {
            this.canvas = canvas;
            Fade = new Fade(canvas, circle, timer);

            timer.Interval = 10;
            timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
        }

        public void Run()
        {
            canvasYcenter = (canvas.ActualHeight / 2) - (circle.ActualHeight / 2);
            canvasXcenter = (canvas.ActualWidth / 2) - (circle.ActualWidth / 2);
            radius = canvas.ActualHeight / 4;

            canvas.Children.Add(circle);
            Canvas.SetTop(circle, canvasYcenter);
            Canvas.SetLeft(circle, canvasXcenter);

            timer.Start();
        }

        private double canvasXcenter;
        private double canvasYcenter;
        private double radius;
        private double radiusChange = 0.1;
        private double xCurrent = 0;
        private double yCurrent = 0;
        private const double xChange = 0.05;
        private const double yChange = 0.05;
        private Fade Fade;
        private int iterations = 0;

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                canvas.Dispatcher.Invoke(() =>
                {
                    yCurrent += yChange;
                    xCurrent += xChange;
                    radius += radiusChange;
                    Canvas.SetTop(circle, canvasYcenter + radius * Math.Sin(yCurrent));
                    Canvas.SetLeft(circle, canvasXcenter + radius * Math.Cos(xCurrent));

                    iterations++;
                    if (100 < iterations && !Fade.Startet)
                    {
                        Fade.Run();
                    }
                });
            }
            catch (TaskCanceledException ex) { Console.WriteLine(ex); }
        }

        private bool IsVisible(Ellipse circle)
        {
            return
                -circle.Height < Canvas.GetTop(circle) &&
                Canvas.GetTop(circle) < canvas.ActualHeight &&
                -circle.Width < Canvas.GetLeft(circle) &&
                Canvas.GetLeft(circle) < canvas.ActualWidth;
        }
    }
}
