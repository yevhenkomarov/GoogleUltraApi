using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using GoogleMusicApi.Common;
using GoogleMusicApi.Structure;
using GoogleMusicApi.Structure.Enums;
using GoogleUltra.GoogleMusic.Login;
using Newtonsoft.Json;

namespace GoogleUltra.GoogleMusic
{
    public class GoogleMusicClient : IGoogleMusicClient
    {
        private const string SavedLoginDataFilePath = "UserData.json";
        public event Action PlaylistsUpdated = delegate { };
        private readonly MobileClient _mobileClient;

        public GoogleMusicClient()
        {
            _mobileClient = new MobileClient();
            LoginData = new LoginData();
            CheckForSavedLoginData();
        }

        private async void CheckForSavedLoginData()
        {
            if (IsLoginDataFileExists())
            {
                var streamReader = new StreamReader(File.Open(SavedLoginDataFilePath, FileMode.Open));
                var savedLoginData = JsonConvert.DeserializeAnonymousType(streamReader.ReadToEnd(),
                    new { Login = string.Empty, MasterToken = string.Empty, RememberMe  = false});
                LoginData.Login = savedLoginData.Login;
                LoginData.MasterToken = savedLoginData.MasterToken;
                LoginData.RememberMe = savedLoginData.RememberMe;
                await InitializeGoogleMusicClient();
            }
        }

        public bool IsLoggedIn { get; private set; }

        public IGoogleMusicLoginData LoginData { get; set; }

        private static bool IsLoginDataFileExists()
        {
            return File.Exists(SavedLoginDataFilePath);
        }

        public void ClearLoginSessionData()
        {
            if (IsLoginDataFileExists())
            {
                File.Delete(SavedLoginDataFilePath);
            }
        }

        public ObservableCollection<Playlist> Playlists { get; private set; }

        public async Task<bool> InitializeGoogleMusicClient()
        {
            if (!string.IsNullOrEmpty(LoginData.MasterToken))
            {
                IsLoggedIn = await _mobileClient.LoginWithToken(LoginData.Login, LoginData.MasterToken);
            }
            else
            {
                IsLoggedIn = await _mobileClient.LoginAsync(LoginData.Login, LoginData.Password);
            }
            
            if (IsLoggedIn)
            {
                Playlists = await InitPlayLists();
                PlaylistsUpdated.Invoke();
                LoginData.MasterToken = _mobileClient.Session.MasterToken;
            }

            return IsLoggedIn;
        }

        public async Task<string> GetStreamAddress(Track selectedTrack)
        {
            var url = await _mobileClient.GetStreamUrlAsync(selectedTrack);
            return url.ToString();
        }

        public void WriteLoginSessionDataToFile()
        {
            using (var streamWriter = new StreamWriter(SavedLoginDataFilePath))
            {
                JsonConvert.SerializeObject(LoginData);
                streamWriter.WriteLine(JsonConvert.SerializeObject(LoginData));
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
    }
}