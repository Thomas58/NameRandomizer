using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NameRandomizer.CanvasDrawings.Removers;
using NameRandomizer.Canvas_Drawings.Custom_Shapes;

namespace NameRandomizer.CanvasDrawings
{
    class SpreadingCircles
    {
        private Canvas canvas;
        private Timer timer = new Timer();
        private Shape[] circles = new Star[50];

        public SpreadingCircles(Canvas canvas)
        {
            this.canvas = canvas;

            for (int i = 0; i < circles.Length; i++)
                circles[i] = new Star() { Width = 10, Height = 10, Fill = Brushes.Gold, Stroke = Brushes.DarkGray };

            circleChange = Math.PI * 2 / circles.Length;
            Fade = new Fade(canvas, circles, timer);

            timer.Interval = 10;
            timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
        }

        public void Run()
        {
            canvasYcenter = (canvas.ActualHeight / 2) - (circles[0].ActualHeight / 2);
            canvasXcenter = (canvas.ActualWidth / 2) - (circles[0].ActualWidth / 2);
            radius = 0.0;
            radiusChangeStage = new double[] { canvas.ActualHeight / 3, Double.PositiveInfinity };
            
            for (int i = 0; i < circles.Length; i++)
            {
                canvas.Children.Add(circles[i]);
                Canvas.SetTop(circles[i], canvasYcenter);
                Canvas.SetLeft(circles[i], canvasXcenter);
            }

            timer.Start();
        }

        private double canvasXcenter;
        private double canvasYcenter;
        private double radius;
        private double[] radiusChange = { 2.0, 0.2 };
        private double[] radiusChangeStage;
        private int stage = 0;
        private double circleChange;
        private Fade Fade;
        private int iterations = 0;
        private int iterationBreak = 200;

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                canvas.Dispatcher.Invoke(() =>
                {

                    radius += radiusChange[stage];
                    if (radiusChangeStage[stage] <= radius)
                        stage++;
                    double circle = 0.0;
                    for (int i = 0; i < circles.Length; i++)
                    {
                        Canvas.SetTop(circles[i], canvasYcenter + radius * Math.Sin(circle));
                        Canvas.SetLeft(circles[i], canvasXcenter + radius * Math.Cos(circle));
                        circle += circleChange;
                    }

                    iterations++;
                    if (iterationBreak < iterations && !Fade.Startet)
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
