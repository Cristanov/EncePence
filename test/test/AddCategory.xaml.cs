using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EncePence
{
    /// <summary>
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window
    {
        DataController data;
        public AddCategory()
        {
            InitializeComponent();
            data = new XmlReaderData();
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            string name = tbCategoryName.Text;
            string describtion = tbDescription.Text;

            if (!string.IsNullOrWhiteSpace(name) && !IsCategoryExist(name))
            {
                int id = data.GetNextCategoryId();
                data.AddCategory(id, name, describtion);
                this.Close();
            }
            
        }

        private bool IsCategoryExist(string name)
        {
            return data.GetAllCategories().FirstOrDefault(x => x.Name == name) != null;
        }
    }
}
