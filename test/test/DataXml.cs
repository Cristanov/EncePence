using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;

namespace EncePence
{
    class DataXml : DataController
    {
        #region Data
        private XElement data, categories;
        #endregion

        #region Ctors
        public DataXml(string dataXmlPath, string categoriesXmlPath)
        {
            try
            {
                data = XElement.Load(dataXmlPath);
                categories = XElement.Load(categoriesXmlPath);
            }
            catch (Exception)
            {
                MessageBox.Show(String.Format("Błąd: Nie udało się otworzyć plików xml: {0}, {1}", dataXmlPath, categoriesXmlPath));
                throw;
            }
        }
        #endregion

        #region Methods

        public Word[] GetAllWords()
        {
            IEnumerable<XElement> words =
                from el in data.Elements("word")
                select el;

            List<Word> wordsList = new List<Word>();
            foreach (XElement item in words)
            {
                int id = Int32.Parse((from word in item.Descendants("id")
                                      select word.Value).First());
                string name = (from word in item.Descendants("name")
                               select word.Value).First();
                string describtion = (from word in item.Descendants("describtion")
                                      select word.Value).First();
                int categoryId = Int32.Parse((from word in item.Descendants("categoryId")
                                              select word.Value).First());
                Category category = Category.GetCategoryById(categoryId, categories);
                bool isEnabled = WordsVisibility.Instance.IsEnable(id);

                Word wordObject = new Word(id, name, describtion, category, isEnabled);
                wordsList.Add(wordObject);
            }

            return wordsList.ToArray();
        }


        public Word[] GetAllWordsByCategory(Category category)
        {
            List<Word> words = new List<Word>();
            words = (from el in data.Descendants("word")
                     where Int32.Parse(el.Element("categoryId").Value).Equals(category.Id)
                     select new Word(
                         (int)el.Element("id"),
                         (string)el.Element("name"),
                         (string)el.Element("describtion"),
                         Category.GetCategoryById((int)el.Element("categoryId"), categories),
                         CategoryVisibility.Instance.IsEnable((int)el.Element("id"))
                         )).ToList();

            return words.ToArray();
        }


        public Word[] GetAllWordsByFirstChar(char firstChar)
        {
            List<Word> words = new List<Word>();
            words = (from el in data.Descendants("word")
                     where el.Element("name").Value.ToString().StartsWith(firstChar.ToString(),
                         true, System.Globalization.CultureInfo.CurrentCulture)
                     select new Word(
                         (int)el.Element("id"),
                         (string)el.Element("name"),
                         (string)el.Element("describtion"),
                         Category.GetCategoryById((int)el.Element("categoryId"), categories),
                         CategoryVisibility.Instance.IsEnable((int)el.Element("id"))
                         )).ToList();

            return words.ToArray();
        }


        public Category[] GetAllCategories()
        {
            Category[] allCategories;
            try
            {
                allCategories = (from el in categories.Descendants("category")
                                 select new Category(
                                         (int)el.Element("id"),
                                         (string)el.Element("name"),
                                         (string)el.Element("describtion"),
                                         CategoryVisibility.Instance.IsEnable((int)el.Element("id"))
                                     )).ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            return allCategories;
        }


        public Word GetWord(int wordId)
        {
            try
            {
                Word word = (from el in data.Descendants("word")
                             where Int32.Parse(el.Element("id").Value) == wordId
                             select new Word(
                                 (int)el.Element("id"),
                                 (string)el.Element("name"),
                                 (string)el.Element("describtion"),
                                 Category.GetCategoryById(Int32.Parse(el.Element(
                                     "categoryId").Value), categories),
                                 true
                                 )).First();
                return word;
            }
            catch (Exception)
            {
                throw new NotFoundWordException("Nie znaleziono słowa o podanym id");
            }
        }


        public Word GetWord(string wordName)
        {
            if (wordName == null)
            {
                throw new ArgumentNullException("argument 'wordName' jest null");
            }
            if (wordName == String.Empty)
            {
                throw new ArgumentException("argument 'wordName' jest pusty");
            }
            try
            {
                Word word = (from el in data.Descendants("word")
                             where el.Element("name").Value.ToString().Equals(wordName)
                             select new Word(
                                 (int)el.Element("id"),
                                 (string)el.Element("name"),
                                 (string)el.Element("describtion"),
                                 Category.GetCategoryById(Int32.Parse(el.Element("categoryId").Value), categories),
                                 true
                                 )).First();
                return word;
            }
            catch (Exception)
            {
                throw new NotFoundWordException("Nie znaleziono słowa o podanej nazwie");
            }
        }

        public class NotFoundWordException : Exception
        {
            public NotFoundWordException()
            { }

            public NotFoundWordException(string message)
                : base(message)
            { }
        }


        public Word[] SearchWord(string searchString)
        {
            Word[] words = (from el in data.Descendants("word")
                            where Regex.Match(el.Element("name").Value.ToString(),
                            searchString, RegexOptions.IgnoreCase).Success
                            select new Word(
                                (int)el.Element("id"),
                                (string)el.Element("name"),
                                (string)el.Element("describtion"),
                                Category.GetCategoryById(Int32.Parse(
                                     el.Element("categoryId").Value), categories),
                                true
                                )).ToArray();

            return words;
        }

        public Word[] GetFilteredWords(WordsFilter wordFilter)
        {
            Word[] words = new Word[] { };
            Word[] wordsWithCategory = new Word[] { };
            if (wordFilter.Category is Category.AllCategories)
            {
                wordsWithCategory = GetAllWords();
                words = wordsWithCategory;
            }
            else
            {
                wordsWithCategory = GetAllWordsByCategory(wordFilter.Category);
                words = wordsWithCategory;
            }

            Word[] wordsWithSearchText = new Word[] { };
            wordsWithSearchText = words;
            if (wordFilter.SearchText != null && wordFilter.SearchText != String.Empty)
            {
                wordsWithSearchText = (from w in wordsWithCategory
                                       where Regex.Match(w.Name, wordFilter.SearchText, RegexOptions.IgnoreCase).Success
                                       select w).ToArray();
                words = wordsWithSearchText;
            }

            if (wordFilter.FirstLetter != null && wordFilter.FirstLetter != String.Empty)
            {
                Word[] wordWithFirstLetter = (from w in wordsWithSearchText
                                              where w.Name.StartsWith(wordFilter.FirstLetter,
                                                true, System.Globalization.CultureInfo.CurrentCulture)
                                              select w).ToArray();
                words = wordWithFirstLetter;
            }

            return words;
        }


        public Word GetRandomWord()
        {
            Word[] allWords = this.GetAllWords();
            Random r = new Random();
            int randomId = r.Next(allWords.Length);
            Word resultWord = allWords[randomId];
            return resultWord;
        }


        public Word GetRandomWord(int[] disabledId)
        {
            Word[] allWords = this.GetAllWords();
            allWords = (from w in allWords
                        where !disabledId.Contains(w.Id)
                        select w).ToArray();
            Random r = new Random();
            int randomIdx = r.Next(allWords.Length);
            Word resultWord = allWords[randomIdx];
            return resultWord;
        }


        public Word GetRandomWord(int[] disabledWordIds, int[] idsWithMostShows, int[] disabledCategoryIds)
        {
            Word[] words = (from w in data.Descendants("word")
                              where !disabledCategoryIds.Contains((int)w.Element("categoryId"))
                              && !disabledWordIds.Contains((int)w.Element("id"))
                              && !idsWithMostShows.Contains((int)w.Element("id"))
                              select new Word
                              {
                                  Id = (int)w.Element("id"),
                                  Name = (string)w.Element("name"),
                                  Description = (string)w.Element("describtion"),
                                  IsEnabled = true,
                                  Category = Category.GetCategoryById((int)w.Element("categoryId"),categories)
                              }).ToArray();
            int randomIdx = new Random().Next(words.Length);

            return words[randomIdx];
        }


        public int GetWordsCount()
        {
            return this.GetAllWords().Count();
        }

        public int GetWordsCount(WordsFilter wordsFilter)
        {
            return this.GetFilteredWords(wordsFilter).Count();
        }

        public Word[] GetRange(int startIdx, int count, WordsFilter wordFilter)
        {
            if (startIdx < 0)
            {
                throw new ArgumentOutOfRangeException("startIdx", "can't beeee less than zero");
            }
            Word[] words = this.GetAllWords();
            Word[] wordsResult = new Word[] { };
            if (wordFilter.Category is Category.AllCategories)
            {
                wordsResult = (from w in words
                               where w.Name.Contains(wordFilter.SearchText)
                               && w.Name.StartsWith(wordFilter.FirstLetter)
                               orderby w.Name
                               select w).Skip(startIdx).Take(count).ToArray();
            }
            else
            {
                wordsResult = (from w in words
                               where w.Category.Id == wordFilter.Category.Id
                               && w.Name.Contains(wordFilter.SearchText)
                               && w.Name.StartsWith(wordFilter.FirstLetter)
                               orderby w.Name
                               select w).Skip(startIdx).Take(count).ToArray();
            }
            return wordsResult;
        }
        #endregion



        public int GetNextCategoryId()
        {
            throw new NotImplementedException();
        }

        public void AddCategory(int id, string name, string describtion)
        {
            throw new NotImplementedException();
        }


        public int GetNextWordId()
        {
            throw new NotImplementedException();
        }

        public void AddWord(int id, string name, string description, int p)
        {
            throw new NotImplementedException();
        }
    }
}
