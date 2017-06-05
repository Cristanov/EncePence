using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EncePence
{
    public sealed class WordsVisibility
    {
        #region Data
        private static WordsVisibility wordsVisibilityInstance = null;
        private static object syncroot = new object();
        private XDocument wordsVisibilityXDocument;
        string filename = Paths.GetWordVisibilityXmlFile(); 
        #endregion

        #region Ctors
        private WordsVisibility()
        {
            wordsVisibilityXDocument = CreateWordsVisibilityXDocument();
        } 
        #endregion

        #region Method

        private XDocument CreateWordsVisibilityXDocument()
        {
            XDocument xDocument;
            if (!File.Exists(filename))
            {
                xDocument = new XDocument(new XElement("wordsVisibility"));
                xDocument.Save(filename);
            }
            else
            {
                xDocument = XDocument.Load(filename);
            }
            return xDocument;
        }

        public void SetVisibility(int wordsId, bool visible)
        {
            if (visible)
            {
                if (IsInVisibilityFile(wordsId))
                {
                    XElement xelement = (from el in wordsVisibilityXDocument.Descendants("word")
                                         where (int)el.Attribute("id") == wordsId
                                         select el).FirstOrDefault();
                    xelement.Remove();
                    wordsVisibilityXDocument.Save(filename);
                }
            }
            else
            {
                if (!IsInVisibilityFile(wordsId))
                {
                    wordsVisibilityXDocument.Element("wordsVisibility").Add(
                                new XElement("word",
                                    new XAttribute("id", wordsId),
                                    new XAttribute("visible", false))
                                );
                    wordsVisibilityXDocument.Save(filename);
                }
            }
        }

        private bool IsInVisibilityFile(int wordId)
        {
            IEnumerable<XElement> elements = from el in wordsVisibilityXDocument.Descendants("word")
                                             where (int)el.Attribute("id") == wordId
                                             select el;
            if (elements.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsEnable(int wordId)
        {
            return !IsInVisibilityFile(wordId);
        }

        public int[] GetDisabledIds()
        {
            int[] disabledIds = (from w in wordsVisibilityXDocument.Descendants("word")
                                 where (bool)w.Attribute("visible") == false
                                 select (int)w.Attribute("id")).ToArray();
            return disabledIds;
        }
        #endregion

        #region Properties

        public static WordsVisibility Instance
        {
            get
            {
                lock (syncroot)
                {
                    if (wordsVisibilityInstance == null)
                    {
                        wordsVisibilityInstance = new WordsVisibility();
                    }
                }
                return wordsVisibilityInstance;
            }
        } 
        #endregion

        
    }
}
