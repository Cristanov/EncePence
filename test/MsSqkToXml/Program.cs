using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MsSqkToXml
{
    class Program
    {
        private static string categoryPath = @"C:\Users\Crist_000\Documents\Visual Studio 2013\Projects\encepence\trunk\test\test\Categories.xml";
        private static string wordPath = @"C:\Users\Crist_000\Documents\Visual Studio 2013\Projects\encepence\trunk\test\test\Words.xml";
        static void Main(string[] args)
        {
            //CreateXml();
            XDocument doc = XDocument.Load(@"C:\Users\Crist_000\Documents\Visual Studio 2013\Projects\encepence\trunk\test\test\Words.xml");
            XDocument newDoc = new XDocument();

            List<XElement> elems = (from el in doc.Descendants("word")
                                    select el).ToList();
            foreach (XElement item in elems)
            {
                string name = item.Descendants("name").FirstOrDefault().Value;
                if (name.StartsWith(@"“") || name.StartsWith(@"„") || name.StartsWith(@"‘") || name.StartsWith(@"«"))
                {
                    name = name.Substring(1, name.Length - 1);
                    item.Descendants("name").FirstOrDefault().Value = name;
                }
                if (name.EndsWith(@"“") || name.EndsWith(@"„") || name.EndsWith(@"‘") || name.EndsWith(@"«") || name.EndsWith(@"”"))
                {
                    name = name.Substring(0, name.Length - 1);
                    item.Descendants("name").FirstOrDefault().Value = name;
                }
            }
            elems = (from el in doc.Descendants("word")
                     orderby el.Descendants("name").FirstOrDefault().Value
                     select el).ToList();
            XElement root = new XElement("words");
            foreach (XElement item in elems)
            {
                root.Add(item);
            }
            newDoc.Add(root);
            newDoc.Save("SortedWords.xml");
            Console.WriteLine("END");
            Console.ReadKey();
        }

        private static void CreateXml()
        {
            using (var ctx = new EncePenceEntities2())
            {
                var words = ctx.Words;
                int i = 0;
                foreach (Word item in words)
                {
                    AddWordToXml(wordPath, item);
                    Console.WriteLine(i++.ToString() + ". Dodano: " + item.Name);

                }
            }
        }

        private static void AddWordToXml(string wordPath, Word item)
        {
            XDocument doc = XDocument.Load(wordPath);
            XElement words = doc.Descendants("words").FirstOrDefault();
            XElement word = new XElement("word",
                new XElement("id", item.WordId),
                new XElement("name", item.Name),
                new XElement("describtion", item.Describtion),
                new XElement("categoryId", item.CategoryId));
            words.Add(word);
            doc.Save(wordPath);
            
        }
       

        private static void AddToXml(string path, Category item)
        {
            XDocument doc = XDocument.Load(path);
            XElement categories = doc.Descendants("categories").FirstOrDefault();
            XElement categoryX = new XElement(
                new XElement("category",
                    new XElement("id", item.CategoryId),
                    new XElement("name", item.Name),
                    new XElement("describtion", item.Describtion)));
            categories.Add(categoryX);
            categories.Save(path);
            
            Console.WriteLine("zapisano: " + item.Name);
        }
    }
}
