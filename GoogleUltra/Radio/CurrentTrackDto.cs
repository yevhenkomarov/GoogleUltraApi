namespace GoogleUltra.Radio
{
    public class CurrentTrackDto
    {
        public string title;
        public string artist;
        public string album;
        public string uniqueid;
        public string metadata;
        public string google_url;
        public string cover;
        public CurrentTrackDto[] prev_tracks;
    }
}