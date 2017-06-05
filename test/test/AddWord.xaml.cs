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
    /// Interaction logic for AddWord.xaml
    /// </summary>
    public partial class AddWord : Window
    {
        DataController data;
        public AddWord()
        {
            InitializeComponent();
            data = new XmlReaderData();
            Category[] categories = data.GetAllCategories();
            cbCategory.ItemsSource = categories;
        }

        private void AddWord_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text;
            string description = tbDescription.Text;
            Category category = cbCategory.SelectedItem as Category;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(description) && cbCategory.SelectedItem != null)
            {
                int id = data.GetNextWordId();
                data.AddWord(id, name, description, category.Id);
                this.Close();
            }
        }
    }
}
