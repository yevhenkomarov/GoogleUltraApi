using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using GoogleMusicApi.Structure;
using GoogleUltra.GoogleMusic;
using GoogleUltra.GoogleMusic.Login;
using GoogleUltra.Models;
using GoogleUltra.Radio;
using ListBox = System.Windows.Forms.ListBox;
using Screen = Caliburn.Micro.Screen;

namespace GoogleUltra.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly IGoogleMusicClient _googleMusicClient;
        private readonly ITrackExtractor _trackExtractor;
        private readonly CurrentTrackModel _currentTrackModel;
        private string _currentTrackTitle;
        private bool _isSomethingFound;

        public ShellViewModel()
        {
            _trackExtractor = new RadioTrackInfoExtractor(new UltraFmRadioSettings());
            _googleMusicClient = new GoogleMusicClient {LoginData = new LoginData()};
            _googleMusicClient.InitializeGoogleMusicClient();
            _currentTrackModel = new CurrentTrackModel();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged!=null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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

        private ObservableCollection<Track> _tracksFound;

        public ObservableCollection<Track> TracksFound
        {
            get { return _tracksFound; }
            set { _tracksFound = value; }
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
                _isSomethingFound = await _googleMusicClient.TryFind(_currentTrackModel.TrackData.metadata);
            }

            UpdateSearchResults();
        }

        private void UpdateSearchResults()
        {
            if (!_isSomethingFound) return;
            var listBox = new ListBox();
            foreach (Track track in _googleMusicClient.SearchResult)
            {
                listBox.Items.Add(new ListBoxItem {Content = track});
            }
            SearchResultListBox = listBox;
        }
        
        private ListBox _searchResultListBox;

        public ListBox SearchResultListBox
        {
            get { return _searchResultListBox; }
            set
            {
                _searchResultListBox = value;
                NotifyOfPropertyChange(() => SearchResultListBox);
            }
        }
    }
}