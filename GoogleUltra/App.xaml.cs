using GoogleMusicApi.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GoogleMusicApi.Structure;
using System.Windows.Media;

namespace GoogleUltra
{
    public partial class App : Application
    {
        private MobileClient _mobileClient;
        private Regex _regex;
        private MainWindow _mainWindow;
        private List<Track> _trackSearchResult;
        private List<Playlist> _playlists;
        private Playlist _currentPlaylist;
        private bool _isLoggedIn;
        private string _currentTrack;

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
            _regex = new Regex(@"<title>([A-Z,a-z]+.+[-].+)<\/title>");
            SubscribeToButtons();
        }

        private async void InitializeGoogleMusicClient()
        {
            _mobileClient = new MobileClient();
            _isLoggedIn = await _mobileClient.LoginAsync(LoginData.Login, LoginData.Password);

            if (_isLoggedIn)
            {
                await InitPlaylists();
                _mainWindow.SearchOnGooglePlayBtn.IsEnabled = true;
            }
        }

        private async Task InitPlaylists()
        {
            var playlistsResult = await _mobileClient.ListPlaylistsAsync();
            _playlists = playlistsResult.Data.Items;

            foreach (var playlist in _playlists)
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
        }

        private void OnPlaylistSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentPlaylist = (Playlist)e.AddedItems[0];

            if(_currentPlaylist != null)
            {
                _mainWindow.AddToPlayListBtn.IsEnabled = true;
            }
        }

        private void AddTrackToPlayList()
        {
            var selectedItem = (ListBoxItem)_mainWindow.SearchResultListBox.SelectedValue;
            if(selectedItem != null)
            {
                var track = (Track)selectedItem.Content;
                _mobileClient.AddSongToPlaylist(_currentPlaylist, track);
            }
        }

        private void GetSongSearchData()
        {
            Uri uri = new Uri("http://nashe2.hostingradio.ru/ultra-128.mp3.xspf");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            MatchCollection matchResult;
            using (StreamReader stream = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
            {
                matchResult = _regex.Matches(stream.ReadToEnd());
            }
            _currentTrack = matchResult[0].Groups[1].Value;
            _mainWindow.CurrentTrackTxt.Text = _currentTrack;
        }

        private async void TryFind()
        {
            SearchResponse searchResult = await _mobileClient.SearchAsync(_currentTrack);
            _trackSearchResult.Clear();
            _mainWindow.SearchResultListBox.Items.Clear();

            foreach (SearchResult resultEntry in searchResult.Entries)
            {
                if (resultEntry.Track == null) continue;
                _trackSearchResult.Add(resultEntry.Track);
                var item = new ListBoxItem(){ Content = resultEntry.Track };
                _mainWindow.SearchResultListBox.Items.Add(item);
            }
        }
    }
}
