using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public List<EntryModel> Entries { get; set; }
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private string username;
        private readonly WykopApi _api = new WykopApi("1N2KUcbg8q");
        private readonly BackgroundWorker synchrozizeNoWorker = new BackgroundWorker();
        private List<HashtagModel> selectedHashtags = new List<HashtagModel>();



        public MainWindow()
        {
            synchrozizeNoWorker.DoWork += synchronizeNow_DoWork;
            synchrozizeNoWorker.RunWorkerCompleted += synchronizeNow_WorkCompletd;
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
            _backgroundWorker.RunWorkerAsync();
            LoadingGrid.Visibility = Visibility.Visible;
            LoadingGridHashtags.Visibility = Visibility.Visible;

        }

        private void synchronizeNow_WorkCompletd(object sender, RunWorkerCompletedEventArgs e)
        {
            SynchronizeNowStatus.Text = "Synchronizacja zakonczończona!";
        }

        private void synchronizeNow_DoWork(object sender, DoWorkEventArgs e)
        {
            SynchronizeNowStatus.Dispatcher.Invoke(
                DispatcherPriority.Normal, new Action(() => { SynchronizeNowStatus.Visibility = Visibility.Visible; }));
            foreach (EntryModel entry in Entries)
            {
                foreach (HashtagModel hashtag in entry.HashTags)
                {
                    if (selectedHashtags.Contains(hashtag))
                    {
                        string selectedDir = string.Empty;
                        Directory.Dispatcher.Invoke(DispatcherPriority.Normal,
                                new Action(() => { selectedDir = this.Directory.Text; }));
                        DirectoryInfo dir = new DirectoryInfo(selectedDir);
                        selectedDir = selectedDir + "\\" + hashtag.Directory;
                        try
                        {
                            // Determine whether the directory exists. 
                            if (!System.IO.Directory.Exists(selectedDir))
                            {
                                dir = System.IO.Directory.CreateDirectory(selectedDir);
                            }

                            string url = entry.Embed[0].Url;
                            WebClient webClient = new WebClient();
                            string filename = url.Substring(url.LastIndexOf("/"));
                            webClient.DownloadFile(url, selectedDir+"//"+filename);
                            break;
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine("The process failed: {0}", exception.ToString());
                        } 
                        
                    }
                }

            }
            
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            string res = _api.DoRequest("profile/votedentries/" + username + "/appkey,tI2TqhiVLa,format,json", null);
            JsonSerializer serializer = new JsonSerializer();
            Entries = serializer.DeserializeEntries(res);

        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            EntriesListBox.ItemsSource = Entries.Where(model => model.Embed.Count > 0 && model.Embed[0].Type.Equals("image"));
            HashtagList.ItemsSource = EntryModel.hashtagsExtractor.Hashtags.ToObservableCollection();
            LoadingGrid.Visibility = Visibility.Collapsed;
            LoadingGridHashtags.Visibility = Visibility.Collapsed;



        }



        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            LoadingGrid.Visibility = Visibility.Visible;
            if (!_backgroundWorker.IsBusy)
            {
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
                        selectedHashtags.Add(context);
                    }
                    else
                    {
                        element.Visibility = Visibility.Hidden;
                        selectedHashtags.Remove(context);
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

            EntryModel.hashtagsExtractor.Hashtags.Clear();
            LoadingGrid.Visibility = Visibility.Visible;
            LoadingGridHashtags.Visibility = Visibility.Visible;


            var usernameTextBox = (TextBox)sender;
            username = usernameTextBox.Text;
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
            synchrozizeNoWorker.RunWorkerAsync();
        }
    }
}
