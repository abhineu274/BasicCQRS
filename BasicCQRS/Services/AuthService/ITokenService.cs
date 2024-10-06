namespace BasicCQRS.Services.AuthService
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }

}
