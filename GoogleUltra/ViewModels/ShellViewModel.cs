using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using GoogleMusicApi.Structure;
using GoogleUltra.GoogleMusic;
using GoogleUltra.GoogleMusic.Login;
using GoogleUltra.Models;
using GoogleUltra.MusicPlayer;
using GoogleUltra.Radio;

namespace GoogleUltra.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly IGoogleMusicClient _googleMusicClient;
        private readonly ITrackExtractor _trackExtractor;
        private readonly IMusicPlayer _musicPlayer; 
        private readonly CurrentTrackModel _currentTrackModel;
        private ObservableCollection<Track> _tracksFound;
        private ObservableCollection<Playlist> _playlists;
        
        public ShellViewModel()
        {
            _trackExtractor = new RadioTrackInfoExtractor(new UltraFmRadioSettings());
            _googleMusicClient = new GoogleMusicClient {LoginData = new LoginData()};
            _googleMusicClient.InitializeGoogleMusicClient();
            _currentTrackModel = new CurrentTrackModel();
            _googleMusicClient.PlaylistsUpdated += OnPlaylistsUpdated;
            _musicPlayer = new BasicMusicPlayer();
        }

        private void OnPlaylistsUpdated()
        {
            AvailablePlayLists = _googleMusicClient.Playlists;
        }

        public string CurrentTrackTitle
        {
            get { return ShowCurrentTrackName(); }
            set
            {
                _currentTrackModel.TrackData.metadata = value;
                NotifyOfPropertyChange(() => CurrentTrackTitle);
            }
        }
        
        public ObservableCollection<Track> TracksFound
        {
            get { return _tracksFound; }
            set
            {
                _tracksFound = value;
                NotifyOfPropertyChange(() => TracksFound);
            }
        }

        public ObservableCollection<Playlist> AvailablePlayLists
        {
            get { return _playlists; }
            set
            {
                _playlists = value;
                NotifyOfPropertyChange(() => AvailablePlayLists);
            }
        }

        public Track HighlightedTrack
        {
            get { return _currentTrackModel.HighlightedTrack; }
            set
            {
                _currentTrackModel.HighlightedTrack = value;
                NotifyOfPropertyChange(() => HighlightedTrack);
            }
        }

        public Playlist HighlightedPlaylist
        {
            get { return _currentTrackModel.HighlightedPlaylist; }
            set
            {
                _currentTrackModel.HighlightedPlaylist = value;
                NotifyOfPropertyChange(() => HighlightedPlaylist);
            }
        }

        public void GetCurrentTrackData()
        {
            _currentTrackModel.TrackData = _trackExtractor.ExtractInfo();
            CurrentTrackTitle = _currentTrackModel.TrackData.metadata;
            _currentTrackModel.CoverImage = 
                _trackExtractor.GetCoverImage(_currentTrackModel.TrackData.cover);
        }

        public async Task SearchOnGooglePlayBtn()
        {
            if (_googleMusicClient.IsLoggedIn)
            {
                TracksFound = await _googleMusicClient.TryFind(_currentTrackModel.TrackData.metadata);
            }
        }

        public void AddToPlayList()
        {
            _googleMusicClient.AddTrackToPlaylist(_currentTrackModel.HighlightedPlaylist,
                _currentTrackModel.HighlightedTrack);
        }

        public async Task PlayGoogleMusicTrack()
        {
            var uri = await _googleMusicClient.GetStreamAddress(HighlightedTrack);
            _musicPlayer.StartPlaying(uri, MusicSource.GoogleMusic);
        }

        public void StopGoogleMusicTrack()
        {
            _musicPlayer.StopPlaying();
        }

        public void PlayRadio()
        {
            _musicPlayer.StartPlaying(_trackExtractor.GetStreamAddress(),
                MusicSource.Radio);
        }

        private string ShowCurrentTrackName()
        {
            return _currentTrackModel.TrackData == null ? "Current track name" :
                _currentTrackModel.TrackData.metadata;
        }
    }
}