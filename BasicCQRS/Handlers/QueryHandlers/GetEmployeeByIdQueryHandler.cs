using AutoMapper;
using BasicCQRS.Data;
using BasicCQRS.DTO;
using BasicCQRS.Services;
using MediatR;

namespace BasicCQRS.Handlers
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDTO>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EmployeeDTO> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetEmployeeByIdAsync(request.Id);
            return _mapper.Map<EmployeeDTO>(result);
        }
    }

}
