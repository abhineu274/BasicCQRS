using BasicCQRS.Data;
using BasicCQRS.Models;
using BasicCQRS.Services;
using MediatR;

namespace BasicCQRS.Handlers
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, int>
    {
        private readonly IEmployeeRepository _repository;

        public UpdateEmployeeCommandHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new Employee
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address,
                Email = request.Email,
                Phone = request.Phone,
                Username = request.Username,
                Password = request.Password,
            };

            return await _repository.UpdateEmployeeAsync(employee);
        }
    }

}
