using System;
using System.Windows;

namespace GoogleUltra
{
    public partial class MainWindow : Window
    {
        public event Action GetCurrentTrackBtnClicked;
        public event Action SearchOnGooglePlayBtnClicked;
        public event Action AddToPlayListBtnClicked;
        public event Action PlaylistSelectorClicked;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void GetCurrentTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentTrackBtnClicked?.Invoke();
        }

        private void SearchOnGooglePlayBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchOnGooglePlayBtnClicked?.Invoke();
        }

        private void AddToLocalListBtn_Click(object sender, RoutedEventArgs e)
        {
            AddToPlayListBtnClicked?.Invoke();
        }

        private void AvailablePlaylistsBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PlaylistSelectorClicked?.Invoke();
        }
    }
}
