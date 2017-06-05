using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncePence
{
    class Word
    {
        #region Ctors
        public Word()
        {
        }

        public Word(int id, string name, string description, Category category, bool isEnabled)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Category = category;
            this.IsEnabled = isEnabled;
        }

        #endregion

        #region Properties
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public Category Category
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get
            {
                return WordsVisibility.Instance.IsEnable(this.Id);
            }
            set
            {
                WordsVisibility.Instance.SetVisibility(this.Id, value);
            }
        } 
        #endregion
    }
}
