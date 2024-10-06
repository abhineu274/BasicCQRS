namespace BasicCQRS.Services.AuthService
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        Task<bool> VerifyPasswordAsync(string password, string hashedPassword);
    }
}
