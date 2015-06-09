using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using wypokDownloader.Helpers;
using wypokDownloader.Model;
using wypokDownloader.wykop;
using CheckBox = System.Windows.Controls.CheckBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace wypokDownloader.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public List<EntryModel> Entries { get; set; }
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private string _username;
        private readonly WykopApi _api = new WykopApi("1N2KUcbg8q");
        private readonly BackgroundWorker _synchrozizeNoWorker = new BackgroundWorker();
        private List<HashtagModel> _selectedHashtags = new List<HashtagModel>();



        public MainWindow()
        {
            _synchrozizeNoWorker.DoWork += synchronizeNow_DoWork;
            _synchrozizeNoWorker.RunWorkerCompleted += synchronizeNow_WorkCompletd;
            InitializeComponent();
        }


        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _backgroundWorker.RunWorkerCompleted += worker_RunWorkerCompleted;
            _backgroundWorker.DoWork += worker_DoWork;
            LoadingGrid.Visibility = Visibility.Collapsed;
            LoadingGridHashtags.Visibility = Visibility.Collapsed;

        }

        private void synchronizeNow_WorkCompletd(object sender, RunWorkerCompletedEventArgs e)
        {
            if (SynchronizeNowStatus.Text != "Wybierz folder!" && SynchronizeNowStatus.Text != "Nie wpisów do przetworzenia.")
                SynchronizeNowStatus.Text = "Synchronizacja zakonczończona!";
        }

        private void synchronizeNow_DoWork(object sender, DoWorkEventArgs e)
        {
            SetStatusText("");
            string directory = "";
            Directory.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(
                        () => directory = Directory.Text));
            Directory.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(
                        () => Directory.BorderBrush = new SolidColorBrush(Colors.Black)));
            if (directory == "")
            {
                SynchronizeNowStatus.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(
                        () =>
                            SynchronizeNowStatus.Text = "Wybierz folder!"));
                Directory.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(
                        () => Directory.BorderBrush = new SolidColorBrush(Colors.Red)));
                return;
            }
            if (Entries != null && Entries.Count > 0)
            {
                SynchronizeNowStatus.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    new Action(() => { SynchronizeNowStatus.Visibility = Visibility.Visible; }));
                foreach (EntryModel entry in Entries)
                {
                    foreach (HashtagModel hashtag in entry.HashTags)
                    {
                        if (_selectedHashtags.Contains(hashtag))
                        {
                            string selectedDir = string.Empty;
                            Directory.Dispatcher.Invoke(DispatcherPriority.Normal,
                                new Action(() => { selectedDir = Directory.Text; }));
                            selectedDir = selectedDir + "\\" + hashtag.Directory;
                            try
                            {
                                // Determine whether the directory exists. 
                                if (!System.IO.Directory.Exists(selectedDir))
                                {
                                    System.IO.Directory.CreateDirectory(selectedDir);
                                }

                                string url = entry.Embed[0].Url;
                                WebClient webClient = new WebClient();
                                string filename = url.Substring(url.LastIndexOf("/", StringComparison.Ordinal));
                                webClient.DownloadFile(url, selectedDir + "//" + filename);
                                break;
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(@"The process failed: {0}", exception);
                            }

                        }
                    }
                }

            }
            else
            {
                SetStatusText("Nie wpisów do przetworzenia.");
            }

        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => LoadingGrid.Visibility = Visibility.Visible));
            Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => LoadingGridHashtags.Visibility = Visibility.Visible));
            SetStatusText("");
            string res = _api.DoRequest("profile/votedentries/" + _username + "/appkey,tI2TqhiVLa,format,json", null);
            SynchronizeNowStatus.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(
                        () => UsernameTxtBox.BorderBrush = new SolidColorBrush(Colors.Black)));
            SetStatusText("");

            if (res.StartsWith("{\"error\""))
            {
                SetStatusText("Nie znaleziono użytkownika lub wystąpiły problemy z połączeniem");
                SynchronizeNowStatus.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(
                        () => UsernameTxtBox.BorderBrush = new SolidColorBrush(Colors.Red)));
                return;
            }

            JsonSerializer serializer = new JsonSerializer();
            Entries = serializer.DeserializeEntries(res);

        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Entries != null)
            {
                EntriesListBox.ItemsSource = Entries.Where(model => model.Embed.Count > 0 && model.Embed[0].Type.Equals("image"));
                HashtagList.ItemsSource = EntryModel.HashtagsExtractor.Hashtags.ToObservableCollection();
            }
            LoadingGrid.Visibility = Visibility.Collapsed;
            LoadingGridHashtags.Visibility = Visibility.Collapsed;

        }



        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            UsernameTxtBox.BorderBrush = new SolidColorBrush(Colors.Black);
            LoadingGrid.Visibility = Visibility.Visible;
            if (!_backgroundWorker.IsBusy)
            {
                _username = UsernameTxtBox.Text;
                _backgroundWorker.RunWorkerAsync();
            }

        }

        private void BtnOpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Directory.Text = openFileDialog.SelectedPath;
        }


        private void Checkbox_OnClick(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            var context = checkbox.DataContext as HashtagModel;
            if (context != null)
            {
                foreach (var element in FindVisualChildren<TextBox>(checkbox.Parent))
                {
                    if (element.Name == "directory" && checkbox.IsChecked == true)
                    {
                        element.Visibility = Visibility.Visible;
                        _selectedHashtags.Add(context);
                    }
                    else
                    {
                        element.Visibility = Visibility.Hidden;
                        _selectedHashtags.Remove(context);
                    }
                }
            }

        }



        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }


        private void UsernameTxtBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            EntryModel.HashtagsExtractor.Hashtags.Clear();
            LoadingGrid.Visibility = Visibility.Visible;
            LoadingGridHashtags.Visibility = Visibility.Visible;


            var usernameTextBox = (TextBox)sender;
            _username = usernameTextBox.Text;
            if (!_backgroundWorker.IsBusy)
            {
                _backgroundWorker.RunWorkerAsync();

            }
        }

        private void ButtonBaseExit_OnClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void SynchronizeNow_OnClick(object sender, RoutedEventArgs e)
        {
            _synchrozizeNoWorker.RunWorkerAsync();
        }


        private void SetStatusText(string text)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                   new Action(
                       () =>
                           SynchronizeNowStatus.Text = text));
        }
    }
}
