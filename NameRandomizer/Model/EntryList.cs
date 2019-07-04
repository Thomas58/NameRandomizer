using System.Collections.Generic;

namespace NameRandomizer.Model
{
    public class EntryList
    {
        public List<Entry> list = new List<Entry>();
        public List<Entry> extraslist = new List<Entry>();
        public bool listUsed = true;
        public bool extraUsed = true;

        internal void Sort(IComparer<Entry> comparer)
        {
            list.Sort(comparer);
            extraslist.Sort(comparer);
        }
    }
}
