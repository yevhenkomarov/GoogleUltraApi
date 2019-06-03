using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GoogleMusicApi.Common;
using GoogleMusicApi.Structure;
using GoogleMusicApi.Structure.Enums;
using GoogleUltra.GoogleMusic.Login;

namespace GoogleUltra.GoogleMusic
{
    public class GoogleMusicClient : IGoogleMusicClient
    {
        public event Action PlaylistsUpdated = delegate {  };
        private readonly MobileClient _mobileClient;

        public GoogleMusicClient()
        {
            _mobileClient = new MobileClient();
        }
        
        public bool IsLoggedIn { get; private set; }

        public IGoogleMusicLoginData LoginData { get; set; }

        public ObservableCollection<Playlist> Playlists { get; private set; }
        
        public async Task InitializeGoogleMusicClient()
        {
            if (string.IsNullOrWhiteSpace(LoginData.Login))
            {
                return;
            }

            IsLoggedIn = await _mobileClient.LoginAsync(LoginData.Login, LoginData.Password);
            if (IsLoggedIn)
            {
                Playlists = await InitPlayLists();
                PlaylistsUpdated.Invoke();
            }
        }

        public async Task<ObservableCollection<Track>> TryFind(string trackSearchData)
        {
            var trackSearchResult = new ObservableCollection<Track>();
            SearchResponse searchResult = await _mobileClient.SearchAsync(trackSearchData);

            foreach (SearchResult resultEntry in searchResult.Entries)
            {
                if (resultEntry.Track == null) continue;
                trackSearchResult.Add(resultEntry.Track);
            }

            return trackSearchResult;
        }

        public async Task<bool> AddTrackToPlaylist(Playlist playlist, Track track)
        {
            var response = await _mobileClient.AddSongToPlaylist(playlist, track);
            return response.ResponseMutation[0].ResponseCode == ResponseCode.Ok;
        }
        
        private async Task<ObservableCollection<Playlist>> InitPlayLists()
        {
            var playListsResult = await _mobileClient.ListPlaylistsAsync();
            var playlistsCollection = new ObservableCollection<Playlist>();
            foreach (var playlist in playListsResult.Data.Items)
            {
                playlistsCollection.Add(playlist);
            }

            return playlistsCollection;
        }

        public async Task<string> GetStreamAddress(Track selectedTrack)
        {
            var url = await _mobileClient.GetStreamUrlAsync(selectedTrack);
            return url.ToString();
        }
    }
}