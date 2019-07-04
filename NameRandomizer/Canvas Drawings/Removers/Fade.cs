using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Linq;
using System;

namespace NameRandomizer.CanvasDrawings.Removers
{
    public class Fade
    {
        private Canvas canvas;
        private Timer timer = new Timer();
        private Timer effectTimer;
        private Shape[] elements;

        public Fade(Canvas canvas, Shape element, Timer effectTimer = null)
        {
            this.canvas = canvas;
            this.elements = new Shape[1];
            elements[0] = element;
            this.effectTimer = effectTimer;
            Setup();
        }
        public Fade(Canvas canvas, Shape[] elements, Timer effectTimer = null)
        {
            this.canvas = canvas;
            this.elements = elements;
            this.effectTimer = effectTimer;
            Setup();
        }
        private void Setup()
        {
            timer.Interval = 100;
            timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
        }

        public void Run()
        {
            Startet = true;
            timer.Start();
        }

        public bool Startet { get; private set; } = false;
        public bool Faded { get; private set; } = false;
        
        private const double change = 0.1;

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                canvas.Dispatcher.Invoke(() =>
                {
                    foreach (Shape element in elements)
                        element.Opacity -= change;

                    if (elements.All((o) => o.Opacity < 0))
                    {
                        foreach (Shape element in elements)
                            canvas.Children.Remove(element);
                        Faded = true;
                        if (effectTimer != null)
                        {
                            effectTimer.Stop();
                            effectTimer.Dispose();
                        }
                        timer.Stop();
                        timer.Dispose();
                    }
                });
            }
            catch (TaskCanceledException ex) { Console.WriteLine(ex); }
        }
    }
}
