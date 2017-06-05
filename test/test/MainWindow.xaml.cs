using MWlodarz.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.ComponentModel;
using DataVirtualization;
using System.Windows.Data;
using System.IO;

namespace EncePence
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Settings settings;
        string[] alphabet;
        DataController data;
        WordsFilter wordFilter;
        DispatcherTimer dispatcherTimer;
        BackgroundWorker bgw;
        WordProvider wordProvider;

        public MainWindow()
        {
            InitializeComponent();
            CreateApplicationFolder();
            InitializeSettingsControls();
            CreateShortCutIfNecessary();
            //data = new DataXml(@"..\..\Words.xml", @"..\..\Categories.xml");
            data = new XmlReaderData();
            //data = new MsSqlData();
            wordFilter = new WordsFilter(new Category.AllCategories(), "", "");
            InitializeDispatcherTimer();
            InitializeAlphabet();
            AddCategoriesToComboBox();
            LoadAllCategories();
            InitializeBackGroundWorker();
            DisableUIForLoadData();
            bgw.RunWorkerAsync();
            SetVersionText();
        }

        private void SetVersionText()
        {
            string text = tbAbout.Text;
            string version;
            using (StreamReader reader = new StreamReader("..\\..\\version.txt"))
            {
                version = reader.ReadLine();
            }
            text = text.Insert(19, version);
            tbAbout.Text = text;
        }

        public void RefreshWords()
        {
            dgWords.ItemsSource = null;
            Binding b = new Binding("DataContext");
            b.Source = dgWords;
            b.Mode = BindingMode.Default;
            dgWords.SetBinding(DataGrid.ItemsSourceProperty, b);
            LoadWordsToDataGrid();
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            RefreshDataGrid();
        }
        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableUIAfterLoadData();
        }

        void ShowRandomWord(object sender, EventArgs e)
        {
            int[] disabledIds = WordsVisibility.Instance.GetDisabledIds();
            int[] mostShowedWordsId = WordsVisibilityCounter.Instance.GetMostShowedIds(data.GetWordsCount());
            int[] disabledCategories = CategoryVisibility.Instance.GetDisabledCategoriesIds();
            Word randomWord = data.GetRandomWord(disabledIds, mostShowedWordsId, disabledCategories);
            WordsVisibilityCounter.Instance.IncrementVisibilityCounter(randomWord.Id);
            myPopup.HeadText = String.Format("{0}: ({1})",randomWord.Name,randomWord.Category.Name);
            myPopup.ContentText = randomWord.Description;
            myPopup.IsOpen = true;
        }


        void label_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        void label_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DisableUIForLoadData();
            DeselectAlphabetLetters();
            Label label = sender as Label;
            label.FontWeight = FontWeights.Bold;
            string labelText = label.Content.ToString();
            if (labelText.Equals("WSZYSTKIE"))
            {
                wordFilter.FirstLetter = "";
            }
            else
            {
                wordFilter.FirstLetter = labelText;
            }
            RunBgwIfNotBusy();
        }



        private void cbPreviev_Checked(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = true;
        }

        private void cbPreviev_Unchecked(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = false;
        }

        private void cbWindowPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.Position = (MPopup.Position)cbWindowPosition.SelectedItem;
            myPopup.PopupPosition = (MPopup.Position)cbWindowPosition.SelectedItem;
        }

        private void cbEffect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            myPopup.Popup.PopupAnimation = (System.Windows.Controls.Primitives.PopupAnimation)cbEffect.SelectedItem;
            try
            {
                settings.WindowAnimation = (System.Windows.Controls.Primitives.PopupAnimation)cbEffect.SelectedItem;
            }
            catch (Exception)
            {
            }
        }

        private void cpHeaderBackground_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Color color = cpHeaderBackground.SelectedColor;
            myPopup.tbHead.Background = new SolidColorBrush(color);
            settings.HeaderBackground = color;
        }

        private void cpHeaderForeground_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Color color = cpHeaderForeground.SelectedColor;
            myPopup.tbHead.Foreground = new SolidColorBrush(color);
            settings.HeaderForeground = color;
        }

        private void cpContentBackground_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Color color = cpContentBackground.SelectedColor;
            myPopup.tbContent.Background = new SolidColorBrush(color);
            settings.ContentBackground = color;
        }

        private void cpContentForeground_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Color color = cpContentForeground.SelectedColor;
            myPopup.tbContent.Foreground = new SolidColorBrush(color);
            settings.ContentForeground = color;
        }

        private void cbWindowPosition_Loaded(object sender, RoutedEventArgs e)
        {
            List<MPopup.Position> positions =
                Enum.GetValues(typeof(MPopup.Position)).Cast<MPopup.Position>().ToList();
            cbWindowPosition.ItemsSource = positions;
        }

        private void tbIntervalValue_LostFocus(object sender, RoutedEventArgs e)
        {

            if (tbIntervalValue.Text != String.Empty)
            {
                settings.DisplayValue = Double.Parse(tbIntervalValue.Text);
                dispatcherTimer.Interval = new TimeSpan(0, Convert.ToInt32(settings.DisplayValue), 0);
            }
            else
            {
                tbIntervalValue.Text = settings.DisplayValue.ToString();
            }
        }

        private void tb_previewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = !(((int)e.Key <= 43 && (int)e.Key >= 34) ||
                ((int)e.Key >= 74 && (int)e.Key <= 83) || (int)e.Key == 2 || (int)e.Key == 32);
            Console.WriteLine((int)e.Key);
        }

        private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text.Equals("Szukaj..."))
            {
                tbSearch.Text = "";
                tbSearch.Foreground = Brushes.Black;
            }
        }

        private void tbSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text == String.Empty)
            {
                tbSearch.Text = "Szukaj...";
                tbSearch.Foreground = Brushes.Silver;
            }
        }

        private void cbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = cbCategories.SelectedItem as Category;
            wordFilter.Category = category;
            ComboBox cb = sender as ComboBox;
            if (cb.IsLoaded)
            {
                RunBgwIfNotBusy();
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbSearch.Text != "Szukaj...")
            {
                wordFilter.SearchText = tbSearch.Text.Trim();
                DisableUIForLoadData();
                RunBgwIfNotBusy();
            }
        }

        private void dgWords_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            int y = e.Row.GetIndex();
            CheckBox cb = (CheckBox)e.EditingElement;
            Word editingWord = dg.Items[y] as Word;
            editingWord.IsEnabled = (bool)cb.IsChecked;
        }

        private void dgCategories_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            int rowIdx = e.Row.GetIndex();
            CheckBox cb = e.EditingElement as CheckBox;
            Category editingCategory = dg.Items[rowIdx] as Category;
            editingCategory.IsEnabled = (bool)cb.IsChecked;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dgWords.ItemsSource = null;
            Binding b = new Binding("DataContext");
            b.Source = dgWords;
            b.Mode = BindingMode.Default;
            dgWords.SetBinding(DataGrid.ItemsSourceProperty, b);
            RefreshWords();
        }

        private void cbStartWithSystem_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cbStartWithSystem = sender as CheckBox;
            if (cbStartWithSystem.IsLoaded)
            {
                RunOnWindowsStart(true);
                settings.IsStartWithSystem = true;
            }
        }

        private void cbStartWithSystem_Unchecked(object sender, RoutedEventArgs e)
        {
            RunOnWindowsStart(false);
            settings.IsStartWithSystem = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CreateShortCutIfNecessary();
        }

        private void dgWords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgWords.SelectedIndex != -1)
            {
                Word selectedWord = (Word)dgWords.SelectedItem;
                myPopup.HeadText = selectedWord.Name;
                myPopup.ContentText = selectedWord.Description;
                myPopup.IsOpen = true;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (settings.IsCloseToTray)
            {
                e.Cancel = true;
                Window window = (Window)sender;
                window.WindowState = System.Windows.WindowState.Minimized;
            }
        }



        private void cbCloseToTray_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cbIsCloseToTray = sender as CheckBox;
            if (cbIsCloseToTray.IsLoaded)
            {
                settings.IsCloseToTray = true;
            }
        }

        private void cbCloseToTray_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.IsCloseToTray = false;
        }

        private void AddCategory_ButtonClick(object sender, RoutedEventArgs e)
        {
            AddCategory addCategoryWindow = new AddCategory();
            addCategoryWindow.Show();
        }

        private void AddWord_ButtonClick(object sender, RoutedEventArgs e)
        {
            AddWord addWordWindow = new AddWord();
            addWordWindow.Show();
        }

    }
}
