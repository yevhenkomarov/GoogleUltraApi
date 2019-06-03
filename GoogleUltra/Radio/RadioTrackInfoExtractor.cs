using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace GoogleUltra.Radio
{
    public class RadioTrackInfoExtractor : ITrackExtractor
    {
        private readonly IRadioSettings _radioSettings;
        
        public RadioTrackInfoExtractor(IRadioSettings radioSettings)
        {
            _radioSettings = radioSettings;
        }

        public string GetStreamAddress()
        {
            return _radioSettings.StreamAddress.ToString();
        }

        public CurrentTrackDto ExtractInfo()
        {
            CurrentTrackDto currentTrackInfo;
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(_radioSettings.RadioInfoUri);
            var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            using (var stream = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
            {
                currentTrackInfo = JsonConvert.DeserializeObject<CurrentTrackDto>(stream.ReadToEnd());
            }

            return currentTrackInfo;
        }

        public BitmapImage GetCoverImage(string uri)
        {
            return new BitmapImage(new Uri($"https://fmgid.com/stations/ultra/{uri}"));
        }
    }
}