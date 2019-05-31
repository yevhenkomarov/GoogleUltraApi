using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleMusicApi.Structure;
using GoogleUltra.GoogleMusic.Login;

namespace GoogleUltra.GoogleMusic
{
    public interface IGoogleMusicClient
    {
        bool IsLoggedIn { get; }
        IGoogleMusicLoginData LoginData { get; set; }
        List<Playlist> Playlists { get; }
        List<Track> SearchResult { get; }
        Task InitializeGoogleMusicClient();
        Task<bool> TryFind(string trackSearchData);
        Task<bool> AddTrackToPlaylist(Playlist playlist, Track track);
    }
}