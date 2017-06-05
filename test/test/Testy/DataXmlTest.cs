using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Xml.Linq;
using System.Diagnostics;

namespace EncePence.Testy
{
    [TestFixture]
    class DataXmlTest
    {
        DataXml dataXml;
        XElement categories;

        [SetUp]
        public void SetUpMethod()
        {
            XElement words = new XElement("words",
                new XElement("word",
                    new XElement("id", 1),
                    new XElement("name", "Ogórek"),
                    new XElement("describtion", "takie zielone warzywo"),
                    new XElement("categoryId", "2")),
                new XElement("word",
                    new XElement("id", 2),
                    new XElement("name", "Wat"),
                    new XElement("describtion", "jednostka mocy"),
                    new XElement("categoryId", "1")),
                new XElement("word",
                    new XElement("id", 3),
                    new XElement("name", "Bit"),
                    new XElement("describtion", "Najmniejsza informacja oznaczająca wartość 1 lub 0"),
                    new XElement("categoryId", "4")),
                new XElement("word",
                    new XElement("id", 4),
                    new XElement("name", "Bajt"),
                    new XElement("describtion", "osiem bitów"),
                    new XElement("categoryId", "4"))
                    );
            words.Save("wordsTest.xml");

            categories = new XElement("categories",
                new XElement("category",
                    new XElement("id", 1),
                    new XElement("name", "Fizyka"),
                    new XElement("describtion", "")),
                new XElement("category",
                    new XElement("id", 2),
                    new XElement("name", "Biologia"),
                    new XElement("describtion", "")),
                new XElement("category",
                    new XElement("id", 3),
                    new XElement("name", "Historia"),
                    new XElement("describtion", "")),
                new XElement("category",
                    new XElement("id", 4),
                    new XElement("name", "Informatyka"),
                    new XElement("describtion", "")));
            categories.Save("categoriesTest.xml");

            dataXml = new DataXml("wordsTest.xml", "categoriesTest.xml");
        }

        [Test]
        public void CtorTest()
        {
            Assert.IsNotNull(dataXml);
        }

        [Test]
        public void GetAllWordsTest()
        {
            Word[] words = dataXml.GetAllWords();
            Assert.IsNotNull(words);
            Assert.AreEqual(4, words.Length);
            Assert.AreEqual(1, words[0].Id);
            Assert.AreEqual(2, words[1].Id);
            Assert.AreEqual(3, words[2].Id);
            Assert.AreEqual("Ogórek", words[0].Name);
            Assert.AreEqual("Wat", words[1].Name);
            Assert.AreEqual("Bit", words[2].Name);
            Assert.AreEqual("Biologia", words[0].Category.Name);
            Assert.AreEqual(2, words[0].Category.Id);
            Assert.AreEqual("Fizyka", words[1].Category.Name);
            Assert.AreEqual(1, words[1].Category.Id);
            Assert.AreEqual("Informatyka", words[2].Category.Name);
            Assert.AreEqual(4, words[2].Category.Id);
        }

        [Test]
        public void GetAllWordsByCategoryTest()
        {
            Category informatyka = Category.GetCategoryByName("Informatyka", "categoriesTest.xml");

            Word[] wordsInf = dataXml.GetAllWordsByCategory(informatyka);
            Assert.AreEqual(2, wordsInf.Length);
            Assert.AreEqual(3, wordsInf[0].Id);
            Assert.AreEqual(4, wordsInf[1].Id);
            Assert.AreEqual("Bit", wordsInf[0].Name);
            Assert.AreEqual("Bajt", wordsInf[1].Name);
        }

        [Test]
        public void GetAllWordsByFirstCharTest()
        {
            Word[] wordsStartsWithB = dataXml.GetAllWordsByFirstChar('b');
            Assert.AreEqual(2, wordsStartsWithB.Length);
            Assert.AreEqual("Bit", wordsStartsWithB[0].Name);
            Assert.AreEqual("Bajt", wordsStartsWithB[1].Name);
            Word[] wordsStartsWithW = dataXml.GetAllWordsByFirstChar('W');
            Assert.AreEqual(1, wordsStartsWithW.Length);
            Assert.AreEqual("Wat", wordsStartsWithW[0].Name);
            Word[] wordsStartsWithZ = dataXml.GetAllWordsByFirstChar('z');
            Assert.AreEqual(0, wordsStartsWithZ.Length);
        }

        [Test]
        public void GetAllCategoriesTest()
        {
            Category[] categories = dataXml.GetAllCategories();
            Assert.IsNotNull(categories);
            Assert.AreEqual(4, categories.Length);
            Assert.AreEqual("Fizyka", categories[0].Name);
            Assert.AreEqual("Biologia", categories[1].Name);
            Assert.AreEqual("Historia", categories[2].Name);
            Assert.AreEqual("Informatyka", categories[3].Name);
        }

        [Test]
        public void GetWordTest()
        {
            Word word = dataXml.GetWord(3);
            Assert.IsNotNull(word);
            Assert.AreEqual("Bit", word.Name);
            Assert.AreEqual(4, word.Category.Id);

            Word word2 = dataXml.GetWord("Bajt");
            Assert.IsNotNull(word2);
            Assert.AreEqual("Bajt", word2.Name);
            Assert.AreEqual(4, word.Category.Id);
        }

        [Test]
        [ExpectedException(typeof(EncePence.DataXml.NotFoundWordException))]
        public void NotFoundExceptionTest()
        {
            Word word = dataXml.GetWord(100);
        }

        [Test]
        [ExpectedException(typeof(EncePence.DataXml.NotFoundWordException))]
        public void NotFoundExceptionTest2()
        {
            Word word = dataXml.GetWord("słowo którego nie ma");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetWordTestArgumentExceptionTest()
        {
            Word word = dataXml.GetWord("");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetWordTestArgumentNullExceptionTest()
        {
            Word word = dataXml.GetWord(null);
        }

        [Test]
        public void SearchWordTest()
        {
            Word[] words = dataXml.SearchWord("ba");
            Assert.AreEqual(1, words.Length);
            Assert.AreEqual("Bajt", words[0].Name);

            words = dataXml.SearchWord("coś czego nie ma");
            Assert.AreEqual(0, words.Length);

            words = dataXml.SearchWord("a");
            Assert.AreEqual(2, words.Length);
            Assert.AreEqual("Wat", words[0].Name);
            Assert.AreEqual("Bajt", words[1].Name);
        }


        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SearchWordTestArgumentNullExceptionTest()
        {
            Word[] word = dataXml.SearchWord(null);
        }

        [Test]
        public void GetFilteredWordsTest()
        {
            Category category = Category.GetCategoryById(4, categories);

            WordsFilter wf = new WordsFilter(category, "", "");
            Word[] words = dataXml.GetFilteredWords(wf);
            Assert.AreEqual(2, words.Length);
            Assert.AreEqual("Bit", words[0].Name);
            Assert.AreEqual("Bajt", words[1].Name);

            wf = new WordsFilter(new Category.AllCategories(), "", "");
            words = dataXml.GetFilteredWords(wf);
            Assert.AreEqual(4, words.Length);
            Assert.AreEqual("Ogórek", words[0].Name);
            Assert.AreEqual("Wat", words[1].Name);
            Assert.AreEqual("Bit", words[2].Name);
            Assert.AreEqual("Bajt", words[3].Name);

            wf = new WordsFilter(category, "Ba", "");
            words = dataXml.GetFilteredWords(wf);
            Assert.AreEqual(1, words.Length);
            Assert.AreEqual("Bajt", words[0].Name);

            wf = new WordsFilter(category, "Bi", "b");
            words = dataXml.GetFilteredWords(wf);
            Assert.AreEqual(1, words.Length);
            Assert.AreEqual("Bit", words[0].Name);

            wf = new WordsFilter(new Category.AllCategories(), "", "b");
            words = dataXml.GetFilteredWords(wf);
            Assert.AreEqual(2, words.Length);
            Assert.AreEqual("Bit", words[0].Name);
            Assert.AreEqual("Bajt", words[1].Name);
        }

        [Test]
        public void GetRandomWordTest()
        {
            Word randomWord = dataXml.GetRandomWord();
            Assert.IsNotNull(randomWord);
        }

        [Test]
        public void GetRandowWordEnableTest()
        {
            int[] disabledIds = new int[] { 1, 2, 3 };
            Word randowWord = dataXml.GetRandomWord(disabledIds);
            Assert.IsNotNull(randowWord);
            Assert.AreEqual(4, randowWord.Id);
            Assert.AreEqual("Bajt", randowWord.Name);
        }

        [Test]
        public void GetRandomEnabledWordWithNoShows()
        {
            int[] disabledIds = new int[] { 1, 2 };
            int[] idsWithMostShows = new int[] { 1, 4 };
            Word randomWord = dataXml.GetRandomWord(disabledIds, idsWithMostShows, new int[] { });
            Assert.IsNotNull(randomWord);
            Assert.AreEqual(3, randomWord.Id);
        }

        [Test]
        public void GetRandomEnabledWordWithNoShows2()
        {
            int[] disabledIds = new int[] { 1, 2 };
            int[] idsWithMostShows = new int[] { };
            Word randomWord = dataXml.GetRandomWord(disabledIds, idsWithMostShows, new int[] { });
            Assert.IsNotNull(randomWord);
        }

        [Test]
        public void GetCountTest()
        {
            Assert.AreEqual(4, dataXml.GetWordsCount());
        }

        [Test]
        public void GetCountWordFilterTest()
        {
            Category cat = new Category(4, "a", "as", true);
            WordsFilter wordFilter = new WordsFilter(cat, "", "");
            int count = dataXml.GetWordsCount(wordFilter);
            Assert.AreEqual(2, count);
        }

        [Test]
        public void GetRangeTest()
        {
            int startIdx = 1;
            int count = 2;
            WordsFilter wordFilter = new WordsFilter(new Category.AllCategories(), "", "");
            Word[] words = dataXml.GetRange(startIdx, count, wordFilter);
            Assert.AreEqual(2, words.Length);

            WordsFilter wordFilter2 = new WordsFilter(new Category(4, "", "", true), "", "");
            Word[] words2 = dataXml.GetRange(1, 1, wordFilter2);
            Assert.AreEqual(1, words2.Length);
            Assert.IsTrue(words2[0].Category.Id == 4);
            Assert.AreEqual("Bit", words2[0].Name);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetRangestartArgumentOutOfRangeTest()
        {
            Word[] words = dataXml.GetRange(-1, 1, new WordsFilter(new Category.AllCategories(), "", ""));
        }

    }
}
