namespace GoogleUltra.MusicPlayer
{
    public interface IMusicPlayer
    {
        void StartPlaying(string uri, MusicSource source);
        void StopPlaying();
    }
}