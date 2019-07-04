using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using NameRandomizer.Model;
using System.Timers;
using NameRandomizer.Tools;
using NameRandomizer.ViewModel;
using System.Windows.Controls;
using NameRandomizer.CanvasDrawings;

namespace NameRandomizer.Commands
{
    class RollNameCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private static int[] iterations = { 100, 50, 10 };
        private static int[] iterationInterval = { 10, 50, 100 };

        private Canvas Canvas;
        private Label NameLabel;
        private Label ExtraLabel;
        private Timer RollTimer = new Timer();
        private int iteration = 0;
        private int counter = iterations[0];
        private int nameIndex;
        private int extraIndex;
        private EntryList entrylist;

        public RollNameCommand(Canvas canvas, Label nameLabel, Label extraLabel, EntryList list)
        {
            this.Canvas = canvas;
            this.NameLabel = nameLabel;
            this.ExtraLabel = extraLabel;
            this.entrylist = list;
            RollTimer.Elapsed += new ElapsedEventHandler(Roll_Tick);
        }

        public bool CanExecute(object o) => 0 < entrylist.list.Count && 0 < entrylist.extraslist.Count && !RollTimer.Enabled;

        public void Execute(object o)
        {
            // Roll a name and save the result to the file.
            var NameRollNumber = Roll(entrylist.list, entrylist.listUsed);
            var ExtraRollNumber = Roll(entrylist.extraslist, entrylist.extraUsed);
            FileService.SaveFile(entrylist);
            
            // Calculate starting index for roll animation.
            nameIndex = Mod(NameRollNumber - iterations.Sum(), entrylist.list.Count);
            extraIndex = Mod(ExtraRollNumber - iterations.Sum(), entrylist.extraslist.Count);

            // Start the animation.
            iteration = 0;
            counter = iterations[0];
            RollTimer.Interval = iterationInterval[0];
            RollTimer.Start();
        }

        private int Roll(List<Entry> list, bool use)
        {
            int rollNumber = MainVM.Random.Next(list.Count);
            while (list[rollNumber].Used)
            {
                rollNumber++;
                if (list.Count <= rollNumber)
                    rollNumber = 0;
            }
            list[rollNumber].Used = use;
            if (list.TrueForAll((o) => o.Used))
                foreach (Entry e in list)
                    e.Used = false;
            return rollNumber;
        }

        private void Roll_Tick(object sender, ElapsedEventArgs e) => Canvas.Dispatcher.Invoke(() =>
        {
            NameLabel.Content = entrylist.list[nameIndex].EntryString;
            ExtraLabel.Content = entrylist.extraslist[extraIndex].EntryString;
            if (counter <= 0)
            {
                iteration++;
                if (iterations.Length <= iteration)
                {
                    NameLabel.Content = entrylist.list[nameIndex].EntryString;
                    ExtraLabel.Content = entrylist.extraslist[extraIndex].EntryString;
                    RollTimer.Stop();
                    CommandManager.InvalidateRequerySuggested();
                    new SpreadingCircles(Canvas).Run();
                }
                else
                {
                    counter = iterations[iteration];
                    RollTimer.Interval = iterationInterval[iteration];
                }
            }
            counter--;
            nameIndex++;
            if (entrylist.list.Count <= nameIndex)
                nameIndex = 0;
            extraIndex++;
            if (entrylist.extraslist.Count <= extraIndex)
                extraIndex = 0;
        });

        private int Mod(int x, int m)
        {
            if (m == 0) return 0;
            return (x % m + m) % m;
        }
    }
}
