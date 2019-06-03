
using System.Windows.Media.Imaging;

namespace GoogleUltra.Radio
{
    public interface ITrackExtractor
    {
        string GetStreamAddress();
        CurrentTrackDto ExtractInfo();
        BitmapImage GetCoverImage(string uri);
    }
}