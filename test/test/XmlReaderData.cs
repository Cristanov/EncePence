using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace EncePence
{
    class XmlReaderData : DataController
    {
        private string categoriesFilePath;
        private string wordsFilePath;
        public XmlReaderData()
        {
            categoriesFilePath = "..\\..\\Categories.xml";
            wordsFilePath = "..\\..\\Words.xml";
        }


        public Word[] GetAllWords()
        {
            throw new NotImplementedException();
        }

        public Word[] GetAllWordsByCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Word[] GetAllWordsByFirstChar(char firstChar)
        {
            throw new NotImplementedException();
        }

        public Category[] GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            using (XmlReader reader = XmlReader.Create(categoriesFilePath))
            {

                List<XElement> categoriesElements = GetElementsNamed(reader, "category").ToList();
                foreach (XElement item in categoriesElements)
                {
                    Category category = XElementToCategory(item);
                    categories.Add(category);
                }
            }
            return categories.ToArray();
        }

        private static Category XElementToCategory(XElement xElement)
        {
            int id = int.Parse(xElement.Descendants("id").FirstOrDefault().Value);
            string name = xElement.Descendants("name").FirstOrDefault().Value;
            string describtion = xElement.Descendants("describtion").FirstOrDefault().Value;
            Category category = new Category(id, name, describtion, CategoryVisibility.Instance.IsEnable(id));
            return category;
        }


        private IEnumerable<XElement> GetElementsNamed(XmlReader reader, string elementTagName)
        {
            List<XElement> elements = new List<XElement>();
            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == elementTagName)
                    {
                        XElement el = XNode.ReadFrom(reader) as XElement;
                        elements.Add(el);
                    }
                }
            }
            return elements;
        }

        public Word GetWord(int wordId)
        {
            throw new NotImplementedException();
        }

        public Word GetWord(string wordName)
        {
            throw new NotImplementedException();
        }

        public Word[] SearchWord(string searchString)
        {
            throw new NotImplementedException();
        }

        public Word[] GetFilteredWords(WordsFilter wordsFilter)
        {
            throw new NotImplementedException();
        }

        public Word GetRandomWord()
        {
            throw new NotImplementedException();
        }

        public Word GetRandomWord(int[] disabledId)
        {
            throw new NotImplementedException();
        }

        public Word GetRandomWord(int[] disabledWordIds, int[] idsWithMostShows, int[] disabledCategoryIds)
        {
            Word resultWord;
            using (XmlReader reader = XmlReader.Create(wordsFilePath))
            {
                reader.MoveToContent();
                List<XElement> words = new List<XElement>();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "word")
                    {
                        XElement el = XNode.ReadFrom(reader) as XElement;
                        int id = int.Parse(el.Descendants("id").FirstOrDefault().Value);
                        int categoryId = int.Parse(el.Descendants("categoryId").FirstOrDefault().Value);
                        if (!disabledWordIds.Contains(id) && !idsWithMostShows.Contains(id) && !disabledCategoryIds.Contains(categoryId))
                        {
                            words.Add(el);
                        }
                    }
                }
                int idx = new Random().Next(words.Count);
                resultWord = XElementToWord(words[idx]);
            }
            return resultWord;
        }

        public int GetWordsCount()
        {
            return GetWordsCount(new WordsFilter(new Category.AllCategories(), "", ""));
        }

        public int GetWordsCount(WordsFilter wordsFilter)
        {
            int count = 0;
            using (XmlReader reader = XmlReader.Create(wordsFilePath))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "word")
                    {
                        XElement el = XNode.ReadFrom(reader) as XElement;
                        if (isMatchToWordFilter(wordsFilter, el))
                        {
                            count++;
                        }

                    }
                }
            }
            return count;

        }

        private static bool isMatchToWordFilter(WordsFilter wordsFilter, XElement el)
        {
            if (wordsFilter.Category is Category.AllCategories)
            {
                if (el.Descendants("name").FirstOrDefault().Value.ToUpper().Contains(wordsFilter.SearchText.ToUpper()) &&
                    el.Descendants("name").FirstOrDefault().Value.ToUpper().StartsWith(wordsFilter.FirstLetter.ToUpper()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (int.Parse(el.Descendants("categoryId").FirstOrDefault().Value) == wordsFilter.Category.Id &&
                      el.Descendants("name").FirstOrDefault().Value.ToUpper().Contains(wordsFilter.SearchText.ToUpper()) &&
                      el.Descendants("name").FirstOrDefault().Value.ToUpper().StartsWith(wordsFilter.FirstLetter.ToUpper()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Word[] GetRange(int startIdx, int count, WordsFilter wordFilter)
        {
            List<Word> words = new List<Word>();
            using (XmlReader reader = XmlReader.Create(wordsFilePath))
            {
                List<XElement> wordElements = new List<XElement>();
                int i = 0;
                reader.MoveToContent();
                while (reader.Read() && i < count)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "word")
                    {
                        XElement el = XNode.ReadFrom(reader) as XElement;
                        if (isMatchToWordFilter(wordFilter, el))
                        {
                            i++;
                            wordElements.Add(el);
                        }
                    }
                }

                foreach (XElement item in wordElements)
                {
                    Word word = XElementToWord(item);
                    words.Add(word);
                }
            }
            return words.ToArray();
        }

        private Word XElementToWord(XElement xElement)
        {
            int id = int.Parse(xElement.Descendants("id").FirstOrDefault().Value);
            string name = xElement.Descendants("name").FirstOrDefault().Value;
            string describtion = xElement.Descendants("describtion").FirstOrDefault().Value;
            int categoryId = int.Parse(xElement.Descendants("categoryId").FirstOrDefault().Value);
            bool isEnabled = WordsVisibility.Instance.IsEnable(id);
            Word word = new Word(id, name, describtion, Category.GetCategoryById(categoryId, categoriesFilePath), isEnabled);
            return word;
        }


        public int GetNextCategoryId()
        {
            int maxCatId = GetAllCategories().OrderByDescending(x => x.Id).First().Id;
            return ++maxCatId;
        }

        public void AddCategory(int id, string name, string describtion)
        {
            XDocument doc = XDocument.Load(categoriesFilePath);
            doc.Root.Add(new XElement("category",
                new XElement("id", id),
                new XElement("name", name),
                new XElement("describtion", describtion)));

            doc.Save(categoriesFilePath);
        }


        public int GetNextWordId()
        {
            XDocument doc = XDocument.Load(wordsFilePath);
            int maxWordId = doc.Descendants("id").Max(x => int.Parse(x.Value));
            return ++maxWordId;
        }

        public void AddWord(int id, string name, string description, int categoryId)
        {
            XDocument doc = XDocument.Load(wordsFilePath);
            doc.Root.Add(new XElement("word",
                new XElement("id", id),
                new XElement("name", name),
                new XElement("describtion", description),
                new XElement("categoryId", categoryId)));

            doc.Save(wordsFilePath);
        }
    }
}
