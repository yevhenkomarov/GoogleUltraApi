using Caliburn.Micro;
using GoogleUltra.Radio;

namespace GoogleUltra.ViewModels
{
    public class ShellViewModel : Screen
    {
        private string _currentTrack;
        private readonly ITrackExtractor _trackExtractor;

        public ShellViewModel()
        {
            _trackExtractor = new RadioTrackInfoExtractor(new UltraFmRadioSettings());
        }

        public string CurrentTrack
        {
            get { return _currentTrack; }
            set
            {
                _currentTrack = value;
                NotifyOfPropertyChange(() => CurrentTrack);
            }
        }

        public void GetCurrentTrack()
        {
            CurrentTrack = _trackExtractor.ExtractInfo().metadata;
        }
    }
}