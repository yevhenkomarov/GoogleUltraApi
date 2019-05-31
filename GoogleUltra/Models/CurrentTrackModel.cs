using Caliburn.Micro;
using GoogleUltra.Radio;

namespace GoogleUltra.Models
{
    public class CurrentTrackModel : PropertyChangedBase 
    {
        public CurrentTrackDto TrackData { get; set; }
    }
}