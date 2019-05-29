using System.IO;
using System.Net;
using System.Text;
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
    }
}