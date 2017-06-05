using DataVirtualization;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace EncePence
{
    public partial class MainWindow
    {
        private void CreateApplicationFolder()
        {
            string myAppDataDirPath = Paths.GetMyAppDataPath();
            if (!Directory.Exists(myAppDataDirPath))
            {
                Directory.CreateDirectory(myAppDataDirPath);
                CreateApplicationData();
            }
        }

        private void CreateApplicationData()
        {
            Settings.CreateSettingsXmlFile(Paths.GetMySettingsXmlFilePath());
        }

        private void InitializeBackGroundWorker()
        {
            bgw = new BackgroundWorker();
            bgw.DoWork += bgw_DoWork;
            bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
            bgw.WorkerSupportsCancellation = true;
        }

        private void InitializeDispatcherTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, (int)settings.DisplayValue, 0);
            dispatcherTimer.Tick += ShowRandomWord;
            dispatcherTimer.Start();
        }

        private void DisableUIForLoadData()
        {
            SetStatusLabelTextToLoading();
            SetMouseToWait();
            cbCategories.IsEnabled = false;
            DisableAlphabet();
        }
        private void SetStatusLabelTextToLoading()
        {
            lStatusBar.Content = "Ładuję bazę...";
        }

        private static void SetMouseToWait()
        {
            Mouse.OverrideCursor = Cursors.Wait;
        }

        private void DisableAlphabet()
        {
            foreach (var item in spAlphabet.Children)
            {
                Label letter = item as Label;
                letter.MouseEnter -= label_MouseEnter;
                letter.MouseLeave -= label_MouseLeave;
            }
        }

        private void EnableUIAfterLoadData()
        {
            lStatusBar.Content = "Załadowano: ";
            SetMouseToArrow();
            cbCategories.IsEnabled = true;
            tbSearch.IsEnabled = true;
            EnableAlphabet();
        }
        private void EnableAlphabet()
        {
            foreach (var item in spAlphabet.Children)
            {
                Label letter = item as Label;
                letter.MouseEnter += label_MouseEnter;
                letter.MouseLeave += label_MouseLeave;
            }
        }

        private static void SetMouseToArrow()
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void LoadAllCategories()
        {
            Category[] categories = data.GetAllCategories();
            dgCategories.ItemsSource = categories;
        }

        private void AddCategoriesToComboBox()
        {
            cbCategories.Items.Add(new Category.AllCategories());
            Category[] categories = data.GetAllCategories();
            foreach (Category item in categories)
            {
                cbCategories.Items.Add(item);
            }
        }

        private void InitializeAlphabet()
        {
            alphabet = new string[] {"wszystkie", "a", "b", "c", "d", "e", "f", "g", "h"
            , "i", "j", "k", "l", "m", "n", "o", "p","q", "r", "s", "t","u","v", "w","x", "y", "z"};

            foreach (string item in alphabet)
            {
                Label label = new Label();
                label.Content = item.ToUpper(); ;
                label.MouseDown += label_MouseDown;
                label.MouseEnter += label_MouseEnter;
                label.MouseLeave += label_MouseLeave;
                spAlphabet.Children.Add(label);
            }
        }

        private void RunBgwIfNotBusy()
        {
            if (bgw.IsBusy)
            {
                bgw.CancelAsync();

            }
            else
            {
                SetStatusLabelTextToLoading();
                SetMouseToWait();
                bgw.RunWorkerAsync();
            }
        }

        private void RefreshDataGrid()
        {
            Dispatcher.BeginInvoke(new ThreadStart(() => dgWords.ItemsSource = null));
            Binding b = new Binding("DataContext");
            b.Source = dgWords;
            b.Mode = BindingMode.Default;
            Dispatcher.BeginInvoke(new ThreadStart(() => dgWords.SetBinding(DataGrid.ItemsSourceProperty, b)));
            LoadWordsToDataGrid();
        }

        public void LoadWordsToDataGrid()
        {
            wordProvider = new WordProvider(data, wordFilter);
            Dispatcher.BeginInvoke(new ThreadStart(() => DataContext = new VirtualizingCollection<Word>(wordProvider, 100)));
        }

        private void DeselectAlphabetLetters()
        {
            foreach (Label item in spAlphabet.Children)
            {
                item.FontWeight = FontWeights.Normal;
            }
        }

        private void InitializeSettingsControls()
        {
            settings = new Settings(Paths.GetMySettingsXmlFilePath());
            cpHeaderBackground.SelectedColor = settings.HeaderBackground;
            cpHeaderForeground.SelectedColor = settings.HeaderForeground;
            cpContentBackground.SelectedColor = settings.ContentBackground;
            cpContentForeground.SelectedColor = settings.ContentForeground;
            cbEffect.SelectedItem = settings.WindowAnimation;
            cbWindowPosition.SelectedItem = settings.Position;
            tbIntervalValue.Text = settings.DisplayValue.ToString();
            cbStartWithSystem.IsChecked = settings.IsStartWithSystem;
            cbCloseToTray.IsChecked = settings.IsCloseToTray;
        }

        public static void RunOnWindowsStart(Boolean turnOn)
        {
            string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutName = "EncePence.lnk";
            if (turnOn)
            {
                CreateShortCut(startupFolderPath, shortcutName);
            }
            else
            {
                try
                {
                    DeleteShortCut(startupFolderPath, shortcutName);
                }
                catch (Exception)
                {}
            }
        }

        private static void CreateShortCut(string startupFolderPath, string shortcutName)
        {
            Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
            dynamic shell = Activator.CreateInstance(t);
            try
            {
                var lnk = shell.CreateShortcut(startupFolderPath + "\\" + shortcutName);
                try
                {
                    string currentDirectory = Directory.GetCurrentDirectory();
                    string targetPath = Path.Combine(currentDirectory, Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().ProcessName));
                    lnk.TargetPath = @targetPath;
                    lnk.WorkingDirectory = currentDirectory;
                    lnk.IconLocation = currentDirectory + "\\EncePence.exe, 0";
                    lnk.Save();
                }
                finally
                {
                    Marshal.FinalReleaseComObject(lnk);
                }
            }
            catch
            {
                MessageBox.Show("Nie udało się utworzyć skrótu.");
            }
            finally
            {
                Marshal.FinalReleaseComObject(shell);
            }
        }

        private static void DeleteShortCut(string startupFolderPath, string shortcutName)
        {
            string[] files = Directory.GetFiles(startupFolderPath, shortcutName);
            foreach (string item in files)
            {
                File.Delete(item);
            }
        }

        private void CreateShortCutIfNecessary()
        {
            if (settings.IsStartWithSystem)
            {
                RunOnWindowsStart(true);
            }
        }


    }
}
