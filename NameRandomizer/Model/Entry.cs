using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace NameRandomizer.Model
{
    [Serializable]
    public class Entry : IComparable<Entry>, IComparable<string>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string entryString;
        public string EntryString { get { return entryString; } set { entryString = value; OnPropertyChanged(); } }
        private bool used = false;
        public bool Used { get { return used; } set { used = value; OnPropertyChanged(); } }

        public Entry() { }
        public Entry(string entry) { this.EntryString = entry; }

        public int CompareTo(Entry other) => EntryString.CompareTo(other.EntryString);
        public int CompareTo(string other) => EntryString.CompareTo(other);
    }
}
