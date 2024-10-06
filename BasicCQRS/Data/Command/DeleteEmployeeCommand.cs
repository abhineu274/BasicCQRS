using BasicCQRS.Models;
using MediatR;

namespace BasicCQRS.Data
{
    public class DeleteEmployeeCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DeleteEmployeeCommand(int id)
        {
            Id = id;
        }
    }


}
