using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using EncePence;

namespace DataVirtualization
{
    class WordProvider : IItemsProvider<Word>
    {
        private int count = 0;
        private readonly int fetchDelay = 1000;
        private DataController data;
        private WordsFilter wordFilter;

        public WordProvider(DataController dataController, WordsFilter wordFilter)
        {
            this.data = dataController;
            this.wordFilter = wordFilter;
            this.count = data.GetWordsCount(wordFilter);
        }

        public int FetchCount()
        {
            Thread.Sleep(fetchDelay);
            return count;
        }

        public IList<Word> FetchRange(int startIdx, int count)
        {
            Trace.WriteLine("FetchRange: " + startIdx + "," + count);
            return data.GetRange(startIdx, count, wordFilter).ToList();
        }
    }
}
