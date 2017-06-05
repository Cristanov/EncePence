using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace EncePence.Testy
{
    [TestFixture]
    class WordFilterTest
    {
        WordsFilter wf;
        DataController data;

        [SetUp]
        public void setupMethod()
        {
            data = new DataXml(@"..\..\Words.xml", @"..\..\Categories.xml");
        }

        [Test]
        public void WordFilterProportiesTest()
        {
            Category category = new Category(1, "Fizyka", "", true);
            wf = new WordsFilter(category, "text", "f");
            Assert.AreEqual("Fizyka", wf.Category.Name);
            Assert.AreEqual("text", wf.SearchText);
            Assert.AreEqual("f", wf.FirstLetter);
        }
    }
}
