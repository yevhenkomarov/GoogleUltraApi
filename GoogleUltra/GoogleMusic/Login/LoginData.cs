namespace GoogleUltra.GoogleMusic.Login
{
    public class LoginData : IGoogleMusicLoginData
    {
        public string Login { get; set; } // = "yevhenkomarov@gmail.com";

        public string MasterToken { get; set; }

        public string Password { get; set; } //= "*******";

        public bool RememberMe { get; set; }
    }
}