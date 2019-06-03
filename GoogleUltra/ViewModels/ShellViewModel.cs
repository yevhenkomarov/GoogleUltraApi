using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using GoogleMusicApi.Structure;
using GoogleUltra.GoogleMusic;
using GoogleUltra.GoogleMusic.Login;
using GoogleUltra.Models;
using GoogleUltra.Radio;

namespace GoogleUltra.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly IGoogleMusicClient _googleMusicClient;
        private readonly ITrackExtractor _trackExtractor;
        private readonly CurrentTrackModel _currentTrackModel;
        private ObservableCollection<Track> _tracksFound;
        private ObservableCollection<Playlist> _playlists;
        private string _currentTrackTitle;
        
        public ShellViewModel()
        {
            _trackExtractor = new RadioTrackInfoExtractor(new UltraFmRadioSettings());
            _googleMusicClient = new GoogleMusicClient {LoginData = new LoginData()};
            _googleMusicClient.InitializeGoogleMusicClient();
            _currentTrackModel = new CurrentTrackModel();
            _googleMusicClient.PlaylistsUpdated += OnPlaylistsUpdated;
        }

        private void OnPlaylistsUpdated()
        {
            AvailablePlayLists = _googleMusicClient.Playlists;
        }

        public string CurrentTrackTitle
        {
            get { return _currentTrackTitle; }
            set
            {
                _currentTrackTitle = value;
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

        public void GetCurrentTrack()
        {
            _currentTrackModel.TrackData = _trackExtractor.ExtractInfo();
            CurrentTrackTitle = _currentTrackModel.TrackData.metadata;
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
    }
}