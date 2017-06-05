using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncePence
{
    class WordsFilter
    {
        #region Ctors
        public WordsFilter(Category category, string searchText, string firstLetter)
        {
            this.Category = category;
            this.SearchText = searchText;
            this.FirstLetter = firstLetter;
        } 
        #endregion

        #region Properties
        public Category Category
        {
            get;
            set;
        }

        public string SearchText
        {
            get;
            set;
        }

        public string FirstLetter
        {
            get;
            set;
        } 
        #endregion
    }
}
