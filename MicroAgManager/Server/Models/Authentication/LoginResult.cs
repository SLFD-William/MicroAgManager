namespace Server.Models.Authentication
{
    public class LoginResult
    {
        public string message { get; set; }
        public bool success { get; set; }
        public TokenModel? token { get; set; }
    }
}
