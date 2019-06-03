namespace GoogleUltra.MusicPlayer
{
    public interface IMusicPlayer
    {
        bool IsPlayingNow { get; set; }
        void StartPlaying();
        void StopPlaying();
    }
}