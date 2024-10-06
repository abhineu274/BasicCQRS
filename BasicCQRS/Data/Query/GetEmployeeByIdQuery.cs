using BasicCQRS.DTO;
using BasicCQRS.Models;
using MediatR;

namespace BasicCQRS.Data
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeDTO>
    {
        public int Id { get; set; }

        public GetEmployeeByIdQuery(int id)
        {
            Id = id;
        }
    }

}
