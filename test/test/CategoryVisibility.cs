using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EncePence
{
    sealed class CategoryVisibility
    {
        #region Data
        private static CategoryVisibility categoriesVisibilityInstance;
        private static object syncroot = new object();
        private XDocument categoriesVisibilityXDocument;
        string filename = Paths.GetCategoryVisibilityXmlFile();
        #endregion

        #region Ctors
        private CategoryVisibility()
        {
            categoriesVisibilityXDocument = CreatecategorysVisibilityXDocument();
        }
        #endregion

        #region Properties
        public static CategoryVisibility Instance
        {
            get
            {
                lock (syncroot)
                {
                    if (categoriesVisibilityInstance == null)
                    {
                        categoriesVisibilityInstance = new CategoryVisibility();
                    }
                }
                return categoriesVisibilityInstance;
            }
        }
        #endregion

        #region Methods

        private XDocument CreatecategorysVisibilityXDocument()
        {
            XDocument xDocument;
            if (!File.Exists(filename))
            {
                xDocument = new XDocument(new XElement("categoriesVisibility"));
                xDocument.Save(filename);
            }
            else
            {
                xDocument = XDocument.Load(filename);
            }

            return xDocument;
        }

        public void SetVisibility(int categoriesId, bool visible)
        {
            if (visible)
            {
                if (IsInVisibilityFile(categoriesId))
                {
                    XElement xelement = (from el in categoriesVisibilityXDocument.Descendants("category")
                                         where (int)el.Attribute("id") == categoriesId
                                         select el).FirstOrDefault();
                    xelement.Remove();
                    categoriesVisibilityXDocument.Save(filename);
                }
            }
            else
            {
                if (!IsInVisibilityFile(categoriesId))
                {
                    categoriesVisibilityXDocument.Element("categoriesVisibility").Add(
                                new XElement("category",
                                    new XAttribute("id", categoriesId),
                                    new XAttribute("visible", false))
                                );
                    categoriesVisibilityXDocument.Save(filename);
                }

            }
        }

        private bool IsInVisibilityFile(int categoryId)
        {
            IEnumerable<XElement> elements = from el in categoriesVisibilityXDocument.Descendants("category")
                                             where (int)el.Attribute("id") == categoryId
                                             select el;

            return elements.Count() == 0 ? false : true;
        }

        public bool IsEnable(int categoryId)
        {
            return !IsInVisibilityFile(categoryId);
        }

        public int[] GetDisabledCategoriesIds()
        {
            int[] disabledCategoriesIds = new int[] { };
            disabledCategoriesIds = (from el in categoriesVisibilityXDocument.Descendants("category")
                                    select (int)el.Attribute("id")).ToArray();
            return disabledCategoriesIds;
        }
        #endregion

    }
}
