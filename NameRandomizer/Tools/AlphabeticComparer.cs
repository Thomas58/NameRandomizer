using NameRandomizer.Model;
using System.Collections.Generic;

namespace NameRandomizer.Tools
{
    class AlphabeticComparer : IComparer<Entry>
    {
        public int Compare(Entry x, Entry y) => Compare(x.EntryString, y.EntryString);
        public int Compare(string x, string y) => x.CompareTo(y);
    }
}
