using System;
using System.Linq;
using System.Xml.Linq;

namespace EncePence
{
    class Category
    {
        #region Ctors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Category()
        {}

        /// <summary>
        /// Initializes a new instance of the Category class using necessary parameters.
        /// </summary>
        /// <param name="id">Category id</param>
        /// <param name="name">Category name</param>
        /// <param name="describtion">Category describtion</param>
        /// <param name="isEnabled">Specifies whether a words in this category are enabled to display</param>
        public Category(int id, string name, string describtion, bool isEnabled)
        {
            this.Id = id;
            this.Name = name;
            this.Describtion = describtion;
            this.IsEnabled = isEnabled;
        }
        #endregion

        #region Properties
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Describtion
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get
            {
                return CategoryVisibility.Instance.IsEnable(this.Id);
            }
            set
            {
                CategoryVisibility.Instance.SetVisibility(this.Id, value);
            }
        } 
        #endregion

        #region Methods
        public static Category GetCategoryById(int id, string xmlFilePath)
        {
            return GetCategoryById(id, XElement.Load(xmlFilePath));
        }

        public static Category GetCategoryById(int id, XElement xElement)
        {
            Category category =
                (from el in xElement.Descendants("category")
                 where el.Element("id").Value.Equals(id.ToString())
                 select new Category(
                         (int)el.Element("id"),
                         (string)el.Element("name"),
                         (string)el.Element("describtion"),
                         CategoryVisibility.Instance.IsEnable((int)el.Element("id"))
                     )).First();

            return category;
        }

        public static Category GetCategoryByName(string categoryName, XElement xElement)
        {
            if (categoryName.Equals(String.Empty))
            {
                throw new ArgumentException("argument 'categoryName' jest niepoprawny");
            }
            Category category = (from el in xElement.Descendants("category")
                                 where el.Element("name").Value.Equals(categoryName)
                                 select new Category(
                                     (int)el.Element("id"),
                                     (string)el.Element("name"),
                                     (string)el.Element("describtion"),
                                     true)).First();

            return category;
        }

        public static Category GetCategoryByName(string categoryName, string categoryXmlPath)
        {
            XElement xElement = XElement.Load(categoryXmlPath);
            return GetCategoryByName(categoryName, xElement);
        }

        public override string ToString()
        {
            return this.Name;
        } 
        #endregion

        #region InternalClasses
        public class AllCategories : Category
        {
            public AllCategories()
                : base()
            {
                this.Name = "Wszystkie";
            }
        } 
        #endregion
    }
}

