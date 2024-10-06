using BasicCQRS.Models;
using Microsoft.AspNetCore.Identity;

namespace BasicCQRS.Services.AuthService
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordHasher<Employee> _passwordHasher;

        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<Employee>();
        }

        public string HashPassword(string password)
        {
            var employee = new Employee(); // Create a dummy employee to hash the password
            return _passwordHasher.HashPassword(employee, password);
        }

        public async Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
        {
            var employee = new Employee(); // Create a dummy employee to verify the password
            var result = _passwordHasher.VerifyHashedPassword(employee, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
