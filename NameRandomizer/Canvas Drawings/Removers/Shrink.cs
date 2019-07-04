using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace NameRandomizer.CanvasDrawings.Removers
{
    class Shrink
    {
        private Canvas canvas;
        private Timer timer = new Timer();
        private Timer effectTimer;
        private Shape[] elements;

        public Shrink(Canvas canvas, Shape element, Timer effectTimer = null)
        {
            this.canvas = canvas;
            this.elements = new Shape[1];
            elements[0] = element;
            this.effectTimer = effectTimer;
            Setup();
        }
        public Shrink(Canvas canvas, Shape[] elements, Timer effectTimer = null)
        {
            this.canvas = canvas;
            this.elements = elements;
            this.effectTimer = effectTimer;
            Setup();
        }
        private void Setup()
        {
            percentageHeightChange = new double[elements.Length];
            percentageWidthChange = new double[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                percentageHeightChange[i] = elements[i].Height * change;
                percentageWidthChange[i] = elements[i].Width * change;
            }

            timer.Interval = 100;
            timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
        }

        public void Run()
        {
            if (!Startet)
            {
                Startet = true;
                timer.Start();
            }
        }

        public bool Startet { get; private set; } = false;
        public bool Faded { get; private set; } = false;

        private const double change = 0.1;
        private double[] percentageHeightChange;
        private double[] percentageWidthChange;

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                canvas.Dispatcher.Invoke(() =>
                {
                    for (int i = 0; i < elements.Length; i++)
                    {
                        elements[i].Height = (elements[i].Height - percentageHeightChange[i] <= 0.0) ? 0.0 : elements[i].Height - percentageHeightChange[i];
                        elements[i].Width = (elements[i].Width - percentageWidthChange[i] <= 0.0) ? 0.0 : elements[i].Width - percentageWidthChange[i];
                    }

                    if (elements.All((o) => o.Height == 0.0 || o.Width == 0.0))
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
