using BasicCQRS.Data;
using BasicCQRS.Services;
using MediatR;

namespace BasicCQRS.Handlers
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, int>
    {
        private readonly IEmployeeRepository _repository;

        public DeleteEmployeeCommandHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteEmployeeAsync(request.Id);
        }
    }

}
