namespace GoogleUltra.GoogleMusic.Login
{
    public interface IGoogleMusicLoginData
    {
        string Login { get; set; }
        string Password { get; set; }
        string MasterToken { get; set; }
        bool RememberMe { get; set; }
    }
}