using NAudio.Wave;

namespace GoogleUltra.MusicPlayer
{
    public class BasicMusicPlayer : IMusicPlayer
    {
        private readonly WaveOutEvent _waveOutEvent = new WaveOutEvent();
        private MusicSource _currentMusicSource;

        public void StartPlaying(string uri, MusicSource source)
        {
            if (_currentMusicSource != source)
            {
                _waveOutEvent.Stop();
            }
            _currentMusicSource = source;
            var mediaFoundationReader = 
                new MediaFoundationReader(uri);
            _waveOutEvent.Init(mediaFoundationReader);
            _waveOutEvent.Play();
        }

        public void StopPlaying()
        {
            _waveOutEvent.Stop();
        }
    }
}