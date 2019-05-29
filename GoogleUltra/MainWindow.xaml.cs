using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GoogleUltra
{
    public partial class MainWindow : Window
    {
        public event Action GetCurrentTrackBtnClicked;
        public event Action SearchOnGooglePlayBtnClicked;
        public event Action AddToPlayListBtnClicked;
        public event Action PlayTrackBtnClicked;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetCoverImage(BitmapImage image)
        {
            CoverImage.Source = image;
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

        private void PlayTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            PlayTrackBtnClicked?.Invoke();
        }
    }
}
