using BasicCQRS.Data.Command;
using BasicCQRS.Models;
using BasicCQRS.Services;
using BasicCQRS.Services.AuthService;
using MediatR;

namespace BasicCQRS.Handlers.CommandHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly ITokenService _tokenService; // Service for generating tokens
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPasswordService _passwordService; // Service for hashing and verifying passwords

        public LoginCommandHandler(ITokenService tokenService, IEmployeeRepository employeeRepository, IPasswordService passwordService)
        {
            _tokenService = tokenService;
            _employeeRepository = employeeRepository;
            _passwordService = passwordService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the employee from the repository
            var employee = await _employeeRepository.GetEmployeeByUsernameAsync(request.Username);

            // Validate username and password
            if (employee == null || !await _passwordService.VerifyPasswordAsync(request.Password, employee.Password)) // Consider hashing passwords in production
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            // Generate token if credentials are valid
            var token = _tokenService.GenerateToken(request.Username);

            return new LoginResponse
            {
                Token = token
            };
        }
    }
}
