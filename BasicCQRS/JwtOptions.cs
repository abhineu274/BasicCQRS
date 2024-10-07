namespace BasicCQRS
{
    public class JwtOptions
    { 
        // JwtOptions is a class that contains properties for JWT token generation
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
        public string Secret { get; set; } = "thisIsTheAJ@123456789thisIsTheAJ@123456789thisIsTheAJ@123456789";

    }
}
