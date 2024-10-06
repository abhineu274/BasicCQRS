using BasicCQRS.DTO;
using BasicCQRS.Models;
using MediatR;

namespace BasicCQRS.Data
{
    public class CreateEmployeeCommand : IRequest<EmployeeDTO>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public CreateEmployeeCommand(string name, string address, string email, string phone, string username, string password)
        {
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
            Username = username;
            Password = password;
        }
    }
}
