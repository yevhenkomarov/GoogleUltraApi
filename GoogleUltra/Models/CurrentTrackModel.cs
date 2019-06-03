using System.Windows.Media.Imaging;
using Caliburn.Micro;
using GoogleMusicApi.Structure;
using GoogleUltra.Radio;

namespace GoogleUltra.Models
{
    public class CurrentTrackModel : PropertyChangedBase 
    {
        public CurrentTrackDto TrackData { get; set; }
        public Track HighlightedTrack { get; set; }
        public Playlist HighlightedPlaylist { get; set; }
        public BitmapImage CoverImage { get; set; }
    }
}