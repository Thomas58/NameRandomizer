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
    class Rising
    {
        private Canvas canvas;
        private Timer timer = new Timer();
        private Shape element;

        public Rising(Canvas canvas)
        {
            this.canvas = canvas;
            
            element = new Star() { Width = 100, Height = 80, Fill = Brushes.Gold, Stroke = Brushes.DarkGray, StrokeThickness = 3 };

            Fade = new Fade(canvas, element, timer);

            timer.Interval = 10;
            timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
        }

        public void Run()
        {
            canvas.Children.Add(element);
            Canvas.SetTop(element, (canvas.ActualHeight / 2) - (element.Height / 2));
            Canvas.SetLeft(element, (canvas.ActualWidth / 2) - (element.Width / 2));

            timer.Start();
        }
        
        private const double heightChange = 0.5;
        private const double widthChange = 0.5;
        private Fade Fade;
        private int iterations = 0;

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                canvas.Dispatcher.Invoke(() =>
                {
                    element.Height = element.Height + heightChange;
                    element.Width = element.Width + widthChange;
                    Canvas.SetTop(element, (canvas.ActualHeight / 2) - (element.Height / 2));
                    Canvas.SetLeft(element, (canvas.ActualWidth / 2) - (element.Width / 2));

                    iterations++;
                    if (100 < iterations && !Fade.Startet)
                    {
                        Fade.Run();
                    }
                });
            }
            catch (TaskCanceledException ex) { Console.WriteLine(ex); }
        }
    }
}
