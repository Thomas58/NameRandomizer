using GalaSoft.MvvmLight.CommandWpf;
using NameRandomizer.Commands;
using NameRandomizer.Model;
using NameRandomizer.Tools;
using NameRandomizer.View;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NameRandomizer.ViewModel
{
    class MainVM : ViewModel
    {
        private EntryList entrylist;

        public static Random Random = new Random();
        public static AlphabeticComparer Alphabetic = new AlphabeticComparer();

        private RollNameCommand rollNameCommand;
        public RelayCommand RollANameCommand => new RelayCommand(() => rollNameCommand.Execute(null), () => rollNameCommand.CanExecute(null));
        public RelayCommand<Window> ShowListCommand => new RelayCommand<Window>(ShowList);
        public RelayCommand<Window> ShowExtraListCommand => new RelayCommand<Window>(ShowExtraList);

        public ListWindow ListWindow;
        public ExtraListWindow ExtraListWindow;
        public void ShowList(Window owner)
        {
            if (ListWindow == null || !ListWindow.IsVisible)
            {
                ListWindow = new ListWindow() { DataContext = new ListVM(entrylist), Owner = owner };
                ListWindow.Show();
            }
        }
        public void ShowExtraList(Window owner)
        {
            if (ExtraListWindow == null || !ExtraListWindow.IsVisible)
            {
                ExtraListWindow = new ExtraListWindow() { DataContext = new ExtraListVM(entrylist), Owner = owner };
                ExtraListWindow.Show();
            }
        }

        public MainVM(Canvas canvas, Label nameLabel, Label extraLabel)
        {
            var temp = FileService.LoadFile();
            if (temp != null && temp.list != null && temp.extraslist != null)
            {
                temp.Sort(Alphabetic);
                this.entrylist = temp;
            }
            else
            {
                this.entrylist = new EntryList();
                entrylist.list = new List<Entry>();
                entrylist.extraslist = new List<Entry>();
            }
            rollNameCommand = new RollNameCommand(canvas, nameLabel, extraLabel, entrylist);
            hotkey = new HotKey(Key.N, KeyModifier.Shift | KeyModifier.Ctrl, OnHotKeyHandler);
        }

        HotKey hotkey;

        private void OnHotKeyHandler(HotKey hotKey)
        {
            if (rollNameCommand.CanExecute(null))
                rollNameCommand.Execute(null);
        }
    }
}
