using MediatR;

namespace BasicCQRS.Data
{
    public class UpdateEmployeeCommand : IRequest<int>
    {
        public int Id { get; set; }  // Required to identify the employee
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public UpdateEmployeeCommand(int id, string name, string address, string email, string phone, string username, string password)
        {
            Id = id;
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
            Username = username;
            Password = password;    
        }
    }
}
