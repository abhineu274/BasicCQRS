namespace BasicCQRS
{
    public class JwtOptions
    {
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
        public string Secret { get; set; } = "thisIsTheAJ@123456789thisIsTheAJ@123456789thisIsTheAJ@123456789";

    }
}
