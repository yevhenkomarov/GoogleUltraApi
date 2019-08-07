using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GoogleMusicApi.Structure;
using GoogleUltra.GoogleMusic.Login;

namespace GoogleUltra.GoogleMusic
{
    public interface IGoogleMusicClient
    {
        event Action PlaylistsUpdated;
        bool IsLoggedIn { get; }
        IGoogleMusicLoginData LoginData { get; set; }
        void WriteLoginSessionDataToFile();
        void ClearLoginSessionData();
        ObservableCollection<Playlist> Playlists { get; }
        Task<bool> InitializeGoogleMusicClient();
        Task<ObservableCollection<Track>> TryFind(string trackSearchData);
        Task<bool> AddTrackToPlaylist(Playlist playlist, Track track);
        Task<string> GetStreamAddress(Track selectedTrack);
    }
}