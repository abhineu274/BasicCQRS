using AutoMapper;
using BasicCQRS.Data;
using BasicCQRS.DTO;
using BasicCQRS.Models;
using BasicCQRS.Services;
using BasicCQRS.Services.AuthService;
using MediatR;

namespace BasicCQRS.Handlers
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDTO> // IRequestHandler<TRequest, TResponse> is a MediatR interface that defines the request and response types
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public CreateEmployeeCommandHandler(IEmployeeRepository repository, IMapper mapper, IPasswordService passwordService)
        {
            // Constructor to inject the repository, mapper, and password service configured in Program.cs
            _repository = repository;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<EmployeeDTO> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken) // Handle method to create an employee. Return type needs to match with the IRequestHandler return type
        {
            var password = _passwordService.HashPassword(request.Password); // Hash the password before storing it in the database
            var employee = new Employee
            {
                Name = request.Name,
                Address = request.Address,
                Email = request.Email,
                Phone = request.Phone,
                Username = request.Username,
                Password = password,
            };

            var res = await _repository.AddEmployeeAsync(employee); // Add the employee to the database
            return _mapper.Map<EmployeeDTO>(res); // Map the response to EmployeeDTO
        }
    }


}
