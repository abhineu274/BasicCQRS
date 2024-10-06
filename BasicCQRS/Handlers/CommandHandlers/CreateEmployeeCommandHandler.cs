using AutoMapper;
using BasicCQRS.Data;
using BasicCQRS.DTO;
using BasicCQRS.Models;
using BasicCQRS.Services;
using BasicCQRS.Services.AuthService;
using MediatR;

namespace BasicCQRS.Handlers
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDTO>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public CreateEmployeeCommandHandler(IEmployeeRepository repository, IMapper mapper, IPasswordService passwordService)
        {
            _repository = repository;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<EmployeeDTO> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var password = _passwordService.HashPassword(request.Password);
            var employee = new Employee
            {
                Name = request.Name,
                Address = request.Address,
                Email = request.Email,
                Phone = request.Phone,
                Username = request.Username,
                Password = password,
            };

            var res = await _repository.AddEmployeeAsync(employee);
            return _mapper.Map<EmployeeDTO>(res); 
        }
    }


}
