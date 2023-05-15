namespace BackEnd.Authentication
{
    public class TokenModel
    {
        public string jwtBearer { get; set; }
        public string refreshToken { get; set; }
        public DateTime expiration { get; set; }
    }
}
