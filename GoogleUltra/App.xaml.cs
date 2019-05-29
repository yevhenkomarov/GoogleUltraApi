using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GoogleMusicApi.Common;
using GoogleMusicApi.Structure;
using NAudio.Wave;
using Newtonsoft.Json;

namespace GoogleUltra
{
    public partial class App : Application
    {
        private MobileClient _mobileClient;
        private MainWindow _mainWindow;
        private List<Track> _trackSearchResult;
        private List<Playlist> _playLists;
        private Playlist _currentPlaylist;
        private bool _isLoggedIn;
        private CurrentTrackDto _currentTrackInfo;
        private readonly Uri _radioPlaylistUri = new Uri("https://fmgid.com/stations/ultra/current.json");
        private Track _selectedTrack;
        private bool _isPlayingNow;
        private readonly WaveOutEvent _waveOutEvent = new WaveOutEvent();

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _mainWindow = new MainWindow();
            _mainWindow.Show();
            Initialize();
        }

        private void Initialize()
        {
            InitializeGoogleMusicClient();
            _trackSearchResult = new List<Track>();
            SubscribeToButtons();
        }

        private async void InitializeGoogleMusicClient()
        {
            _mobileClient = new MobileClient();
            _isLoggedIn = await _mobileClient.LoginAsync(LoginData.Login, LoginData.Password);

            if (_isLoggedIn)
            {
                await InitPlayLists();
                _mainWindow.SearchOnGooglePlayBtn.IsEnabled = true;
            }
        }

        private async Task InitPlayLists()
        {
            var playListsResult = await _mobileClient.ListPlaylistsAsync();
            _playLists = playListsResult.Data.Items;

            foreach (var playlist in _playLists)
            {
                _mainWindow.AvailablePlaylistsBox.Items.Add(playlist);
            }
        }

        private void SubscribeToButtons()
        {
            _mainWindow.GetCurrentTrackBtnClicked += GetSongSearchData;
            _mainWindow.SearchOnGooglePlayBtnClicked += TryFind;
            _mainWindow.AddToPlayListBtnClicked += AddTrackToPlayList;
            _mainWindow.AvailablePlaylistsBox.SelectionChanged += OnPlaylistSelectionChanged;
            _mainWindow.SearchResultListBox.SelectionChanged += OnSearchListSelectionChanged;
            _mainWindow.PlayTrackBtnClicked += OnPlayTrackBtnClicked;
        }

        private void OnSearchListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var result = new ListBoxItem();
            if (e.AddedItems.Count <= 0)
            {
                return;
            }
            result = (ListBoxItem) e.AddedItems[0];
            _selectedTrack = (Track)result.Content;
        }

        private async void OnPlayTrackBtnClicked()
        {
            if (!_isPlayingNow)
            {
                var url = await _mobileClient.GetStreamUrlAsync(_selectedTrack);
            
                var mediaFoundationReader = 
                    new MediaFoundationReader(url.ToString());
                _waveOutEvent.Init(mediaFoundationReader);
                _waveOutEvent.Play();
                _isPlayingNow = true;
                _mainWindow.PlayTrackBtn.Content = "IsPlaying";
            }
            else
            {
                _waveOutEvent.Stop();
                _mainWindow.PlayTrackBtn.Content = "Stopped";
                _isPlayingNow = false;
            }
        }

        private void OnPlaylistSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentPlaylist = (Playlist) e.AddedItems[0];

            if (_currentPlaylist != null)
            {
                _mainWindow.AddToPlayListBtn.IsEnabled = true;
            }
        }

        private async void AddTrackToPlayList()
        {
            var selectedItem = (ListBoxItem) _mainWindow.SearchResultListBox.SelectedValue;
            if (selectedItem == null) return;
            _selectedTrack = (Track) selectedItem.Content;
            await _mobileClient.AddSongToPlaylist(_currentPlaylist, _selectedTrack);
        }

        private void GetSongSearchData()
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(_radioPlaylistUri);
            HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            using (StreamReader stream = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
            {
                _currentTrackInfo = JsonConvert.DeserializeObject<CurrentTrackDto>(stream.ReadToEnd());
            }

            _mainWindow.CurrentTrackTxt.Text = _currentTrackInfo.metadata;

            var coverImage = new BitmapImage(new Uri($"https://fmgid.com/stations/ultra/{_currentTrackInfo.cover}"));
            _mainWindow.SetCoverImage(coverImage);  
        }

        private async void TryFind()
        {
            SearchResponse searchResult = await _mobileClient.SearchAsync(_currentTrackInfo.metadata);
            _trackSearchResult.Clear();
            _mainWindow.SearchResultListBox.Items.Clear();

            foreach (SearchResult resultEntry in searchResult.Entries)
            {
                if (resultEntry.Track == null) continue;
                _trackSearchResult.Add(resultEntry.Track);
                var item = new ListBoxItem() {Content = resultEntry.Track};
                _mainWindow.SearchResultListBox.Items.Add(item);
            }
        }
    }
}