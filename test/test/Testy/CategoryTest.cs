using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Xml.Linq;

namespace EncePence.Testy
{
    [TestFixture]
    class CategoryTest
    {
        Category category, category2;
        [TestFixtureSetUp]
        public void SetUpMethod()
        {
            category = new Category();
            category2 = new Category(1, "Fizyka", "opis", true);

            //XElement categories = new XElement("categories",
            //    new XElement("category",
            //        new XElement("id", "1"),
            //        new XElement("name", "Fizyka"),
            //        new XElement("describtion", "opis fizyki")),

            //    new XElement("category",
            //        new XElement("id", "2"),
            //        new XElement("name", "Biologia"),
            //        new XElement("describtion", "opis biologii")));
            //categories.Save("categories.xml");
        }

        [Test]
        public void CtorTest()
        {
            Assert.IsNotNull(category);
            Assert.AreEqual(1, category2.Id);
            Assert.AreEqual("Fizyka", category2.Name);
            Assert.AreEqual(true, category2.IsEnabled);
            Assert.AreEqual("opis", category2.Describtion);
        }

        [Test]
        public void ProportiesTest()
        {
            category.Id = 1;
            Assert.AreEqual(1, category.Id);
            category.Name = "Biologia";
            Assert.AreEqual("Biologia", category.Name);
            category.IsEnabled = true;
            Assert.IsTrue(category.IsEnabled);
            category.Describtion = "jakiś opis";
            Assert.AreEqual("jakiś opis", category.Describtion);
        }

        [Test]
        public void GetCategoryByIdTest()
        {
            Category category = Category.GetCategoryById(1, "categories.xml");
            Assert.IsNotNull(category);
            Assert.AreEqual(1, category.Id);
            Assert.AreEqual("Fizyka", category.Name);
            Assert.AreEqual("opis fizyki", category.Describtion);
            Category category2 = Category.GetCategoryById(2, "categories.xml");
            Assert.IsNotNull(category2);
            Assert.AreEqual(2, category2.Id);
            Assert.AreEqual("Biologia", category2.Name);
            Assert.AreEqual("opis biologii", category2.Describtion);

            XElement elem = new XElement("categories",
                new XElement("category",
                    new XElement("id", 99),
                    new XElement("name", "Budownictwo"),
                    new XElement("describtion", "desc")),
                new XElement("category",
                    new XElement("id", 100),
                    new XElement("name", "Literatura"),
                    new XElement("describtion", "litDesc")));

            Category category3 = Category.GetCategoryById(99, elem);
            Assert.IsNotNull(category3);
            Assert.AreEqual(99, category3.Id);
            Assert.AreEqual("Budownictwo", category3.Name);
            Assert.AreEqual("desc", category3.Describtion);
        }

        [Test]
        public void GetCategoryByName()
        {
            XElement categories = XElement.Load("categories.xml");
            Category category = Category.GetCategoryByName("Fizyka", categories);
            Assert.IsNotNull(category);
            Assert.AreEqual("Fizyka", category.Name);
            Assert.AreEqual("opis fizyki", category.Describtion);
            Assert.IsTrue(category.IsEnabled);
            Assert.AreEqual(1, category.Id);

            Category category2 = Category.GetCategoryByName("Biologia", "categories.xml");
            Assert.IsNotNull(category);
            Assert.AreEqual("Biologia", category2.Name);
            Assert.AreEqual("opis biologii", category2.Describtion);
            Assert.IsTrue(category2.IsEnabled);
            Assert.AreEqual(2, category2.Id);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetCategoryByNameArgumentExceptionTest()
        {
            Category category = Category.GetCategoryByName("", "categories.xml");
        }

        [Test]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void GetCategoryFileNotFoundExceptionTest()
        {
            Category category = Category.GetCategoryById(2, "asdasd.xml");
        }

        [Test]
        public void CategoryToStringTest()
        {
            Category category = new Category();
            Assert.AreEqual(null, category.ToString());
            Category category2 = new Category(1, "kat", "desc", true);
            Assert.AreEqual("kat", category2.ToString());
        }

        [Test]
        public void SpecialAllCategoriesTest()
        {
            Category.AllCategories all = new Category.AllCategories();
            Assert.AreEqual("Wszystkie", all.Name);
        }

        [Test]
        public void CategoryIsEnableTest()
        {
            Category category = new Category(666, "name", "desc", true);
            category.IsEnabled = false;
            Assert.IsFalse(CategoryVisibility.Instance.IsEnable(category.Id));
        }
    }
}
