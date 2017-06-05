using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace EncePence.Testy
{
    [TestFixture]
    class WordTest
    {
        Word word, word2;

        [Test]
        public void CtorTest()
        {
            word = new Word();
            Assert.IsNotNull(word);
            word2 = new Word(1, "Słowo", "Opis słowa", new Category(), true);
            Assert.AreEqual(1, word2.Id);
            Assert.AreEqual("Słowo", word2.Name);
            Assert.AreEqual("Opis słowa", word2.Description);
            Assert.IsNotNull(word2.Category);
            Assert.IsTrue(word2.IsEnabled);
        }

        [Test]
        public void ProportiesTest()
        {
            word.Id = 1;
            Assert.AreEqual(1, word.Id);
            word.Name = "coś";
            Assert.AreEqual("coś", word.Name);
            word.Description = "opis coś";
            Assert.AreEqual("opis coś", word.Description);
            word.Category = new Category();
            Assert.IsNotNull(word.Category);
            word.IsEnabled = false;
            Assert.IsFalse(word.IsEnabled);
            word.IsEnabled = true;
        }

        [Test]
        public void VisibilityPropertyTest()
        {
            Word word = new Word(100, "name", "opis", new Category(), true);
            word.IsEnabled = false;
            Assert.IsFalse(word.IsEnabled);
            Assert.IsFalse(WordsVisibility.Instance.IsEnable(word.Id));
            word.IsEnabled = true;
        }
    }
}
