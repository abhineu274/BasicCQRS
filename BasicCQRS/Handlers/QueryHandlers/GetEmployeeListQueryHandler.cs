using AutoMapper;
using BasicCQRS.Data.Query;
using BasicCQRS.DTO;
using BasicCQRS.Services;
using MediatR;

namespace BasicCQRS.Handlers.QueryHandlers
{
    public class GetEmployeeListQueryHandler : IRequestHandler<GetEmployeeListQuery, List<EmployeeDTO>>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetEmployeeListQueryHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDTO>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
        {
            var employees = await _repository.GetEmployeesAsync();
            return _mapper.Map<List<EmployeeDTO>>(employees);
        }
    }


}
