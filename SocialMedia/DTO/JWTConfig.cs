namespace SocialMedia.DTO
{
    public class JWTConfig
    {
        public string Issuer { get; set; }  
        public string Audience { get; set; }    
        public string key { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
