using BasicCQRS.DTO;
using MediatR;

namespace BasicCQRS.Data.Query
{
    public class GetEmployeeListQuery : IRequest<List<EmployeeDTO>>
    {

    }
}
