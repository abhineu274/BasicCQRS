using BasicCQRS.Models;
using MediatR;

namespace BasicCQRS.Data.Command
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
