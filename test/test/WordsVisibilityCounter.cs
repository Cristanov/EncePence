using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EncePence
{
    public sealed class WordsVisibilityCounter
    {
        #region Data
        private static WordsVisibilityCounter wordsVisCounterInstance = null;
        private static object syncroot = new object();
        private string fileName = Paths.GetWordVisibilityCounterXmlFile();
        private XDocument wordsVisibilityXdoc;   
        #endregion

        #region Ctors
        private WordsVisibilityCounter()
        {
            wordsVisibilityXdoc = CreateWordsVisXDoc();
        } 
        #endregion

        #region Methods

        private XDocument CreateWordsVisXDoc()
        {
            XDocument xDocument;
            if (!File.Exists(fileName))
            {
                xDocument = new XDocument(new XElement("wordsVisibilityCounter"));
                xDocument.Save(fileName);
            }
            else
            {
                xDocument = XDocument.Load(fileName);
            }
            return xDocument;
        }

        public void IncrementVisibilityCounter(int wordId)
        {
            IEnumerable<XElement> elements = from el in wordsVisibilityXdoc.Descendants("word")
                                             where (int)el.Attribute("id") == wordId
                                             select el;
            if (elements.Count() > 0)
            {
                XElement element = elements.FirstOrDefault();
                XAttribute showsAttr = element.Attribute("shows");
                int shows = (int)showsAttr;
                shows++;
                showsAttr.Value = shows.ToString();
            }
            else
            {
                XElement element = new XElement("word",
                    new XAttribute("id", wordId),
                    new XAttribute("shows", 1));
                wordsVisibilityXdoc.Element("wordsVisibilityCounter").Add(element);
            }
            wordsVisibilityXdoc.Save(fileName);
        }

        public int[] GetMostShowedIds(int countOfAllWords)
        {
            try
            {
                int maxShows = wordsVisibilityXdoc.Descendants("word").
                                   Max(x => (int)x.Attribute("shows"));

                int[] resultIds = (from w in wordsVisibilityXdoc.Descendants("word")
                                   where (int)w.Attribute("shows") == maxShows
                                   select (int)w.Attribute("id")).ToArray();
                if (resultIds.Length == countOfAllWords || resultIds.Length == 0)
                    return new int[] { };
                else
                    return resultIds;
            }
            catch (Exception)
            {
                return new int[] { 0 };
            }
        }
        #endregion

        #region Properties
        public static WordsVisibilityCounter Instance
        {
            get
            {
                lock (syncroot)
                {
                    if (wordsVisCounterInstance == null)
                    {
                        wordsVisCounterInstance = new WordsVisibilityCounter();
                    }
                }
                return wordsVisCounterInstance;
            }
        } 
        #endregion

        
    }
}
