using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using NameRandomizer.Model;
using NameRandomizer.Tools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static NameRandomizer.Model.EntryList;

namespace NameRandomizer.ViewModel
{
    class ExtraListVM : ViewModel
    {
        private EntryList entrylist;
        private ObservableCollection<Entry> entries;
        public ObservableCollection<Entry> Entries { get { return entries; } set { entries = value; OnPropertyChanged(); } }
        private string textfield = "";
        public string TextField { get { return textfield; } set { textfield = value; OnPropertyChanged(); } }
        private Entry selectedItem;
        public Entry SelectedItem { get { return selectedItem; } set { selectedItem = value; OnPropertyChanged(); } }
        public string Title { get { return "Extra Name List (" + entrylist.extraslist.Count + ")"; } }
        public bool Used { get { return entrylist.extraUsed; } set { entrylist.extraUsed = value; OnPropertyChanged(); } }

        public RelayCommand AddEntryCommand => new RelayCommand(AddEntry);
        public RelayCommand DeleteEntryCommand => new RelayCommand(DeleteEntry, () => selectedItem != null);
        public RelayCommand TextFileCommand => new RelayCommand(TextFile);

        public ExtraListVM(EntryList entrylist)
        {
            this.entrylist = entrylist;
            
            Entries = new ObservableCollection<Entry>(entrylist.extraslist);
        }

        public void AddEntry()
        {
            if (entrylist.extraslist.Any((e) => e.EntryString.Equals(TextField)))
            {
                MessageBox.Show("Entry already exists");
            }
            else
            {
                entrylist.extraslist.Add(new Entry(TextField));
                entrylist.extraslist.Sort(MainVM.Alphabetic);
                FileService.SaveFile(entrylist);
                Entries = new ObservableCollection<Entry>(entrylist.extraslist);
                TextField = "";
                OnPropertyChanged("Title");
            }
        }

        public void DeleteEntry()
        {
            entrylist.extraslist.Remove(SelectedItem);
            Entries = new ObservableCollection<Entry>(entrylist.extraslist);
            FileService.SaveFile(entrylist);
            OnPropertyChanged("Title");
        }

        public void TextFile()
        {
            OpenFileDialog dialog = new OpenFileDialog() { CheckFileExists = true, CheckPathExists = true, Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*", Multiselect = false };
            var success = dialog.ShowDialog();
            if (success.HasValue && success.Value)
            {
                FileInfo info = new FileInfo(dialog.FileName);
                using (StreamReader reader = info.OpenText())
                {
                    while (!reader.EndOfStream)
                    {
                        string name = reader.ReadLine();
                        if (!entrylist.extraslist.Any((e) => e.EntryString.Equals(name)))
                            entrylist.extraslist.Add(new Entry(name));
                    }
                }
                entrylist.extraslist.Sort();
                Entries = new ObservableCollection<Entry>(entrylist.extraslist);
                FileService.SaveFile(entrylist);
                OnPropertyChanged("Title");
            }
        }
    }
}
