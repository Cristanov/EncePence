using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace EncePence.Testy
{
    [TestFixture]
    class WordAdapterTest
    {
        [Test]
        public void GetWordTest()
        {
            Adapters.Category categoryEntity = new Adapters.Category()
            {
                CategoryId = 1,
                Name="name",
                Describtion="descCat"
            };
            Adapters.Word wordEntity = new Adapters.Word()
            {
                WordId = 666,
                Name = "word",
                Describtion = "desc",
                Category = categoryEntity
            };
            EncePence.Word word = Adapters.WordAdapter.GetWord(wordEntity);

            Assert.IsNotNull(word);
            Assert.AreEqual(666, word.Id);
            Assert.AreEqual("word", word.Name);
            Assert.AreEqual("desc", word.Description);
            Assert.AreEqual(1, word.Category.Id);
            Assert.AreEqual("name", word.Category.Name);
            Assert.AreEqual("descCat", word.Category.Describtion);
        }

        [Test]
        public void GetWordEntity()
        {
            EncePence.Category category = new Category(444, "categ", "desc", true);
            EncePence.Word word = new Word(555, "word", "descWord", category, true);
            Adapters.Word wordEntity = Adapters.WordAdapter.GetWordEntity(word);

            Assert.IsNotNull(wordEntity);
            Assert.AreEqual(555, wordEntity.WordId);
            Assert.AreEqual("word", wordEntity.Name);
            Assert.AreEqual("descWord", wordEntity.Describtion);
            Assert.AreEqual(444, wordEntity.Category.CategoryId);
            Assert.AreEqual("categ", wordEntity.Category.Name);
            Assert.AreEqual("desc", wordEntity.Category.Describtion);
        }
    }
}
