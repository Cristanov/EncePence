using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace EncePence.Testy
{
    [TestFixture]
    class AdapterCategoryTest
    {
        [Test]
        public void GetCategoryTest()
        {
            Adapters.Category categoryEntity = new Adapters.Category()
            {
                CategoryId = 100,
                Name="cat",
                Describtion = "desc"
            };

            EncePence.Category category = Adapters.CategoryAdapter.GetCategory(categoryEntity);

            Assert.AreEqual(100, category.Id);
            Assert.AreEqual("cat", category.Name);
            Assert.AreEqual("desc", category.Describtion);
        }

        [Test]
        public void GetCategoryEntityTest()
        {
            EncePence.Category category = new Category(40, "category", "describtion", true);

            EncePence.Adapters.Category categoryEntity = 
                Adapters.CategoryAdapter.GetCategoryEntity(category);

            //Assert.AreEqual(40, categoryEntity.CategoryId);
            Assert.AreEqual("category", categoryEntity.Name);
            Assert.AreEqual("describtion", categoryEntity.Describtion);
        }
    }
}
