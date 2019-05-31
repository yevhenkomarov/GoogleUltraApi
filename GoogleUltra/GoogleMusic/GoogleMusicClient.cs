using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleMusicApi.Common;
using GoogleMusicApi.Structure;
using GoogleMusicApi.Structure.Enums;
using GoogleUltra.GoogleMusic.Login;

namespace GoogleUltra.GoogleMusic
{
    public class GoogleMusicClient : IGoogleMusicClient
    {
        private readonly MobileClient _mobileClient;
        private readonly List<Track> _trackSearchResult;

        public GoogleMusicClient()
        {
            _mobileClient = new MobileClient();
            _trackSearchResult = new List<Track>();
        }

        public bool IsLoggedIn { get; private set; }

        public IGoogleMusicLoginData LoginData { get; set; }

        public List<Playlist> Playlists { get; private set; }

        public List<Track> SearchResult => _trackSearchResult;

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
            }
        }

        public async Task<bool> TryFind(string trackSearchData)
        {
            _trackSearchResult.Clear();
            var searchResult = await _mobileClient.SearchAsync(trackSearchData);

            foreach (var resultEntry in searchResult.Entries)
            {
                if (resultEntry.Track == null) continue;
                _trackSearchResult.Add(resultEntry.Track);
            }

            return _trackSearchResult.Count > 0;
        }

        public async Task<bool> AddTrackToPlaylist(Playlist playlist, Track track)
        {
            var response = await _mobileClient.AddSongToPlaylist(playlist, track);
            return response.ResponseMutation[0].ResponseCode == ResponseCode.Ok;
        }
        
        private async Task<List<Playlist>> InitPlayLists()
        {
            var playListsResult = await _mobileClient.ListPlaylistsAsync();
            return playListsResult.Data.Items;
        }
    }
}