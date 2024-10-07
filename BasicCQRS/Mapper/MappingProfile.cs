using AutoMapper;
using BasicCQRS.DTO;
using BasicCQRS.Models;

namespace BasicCQRS.Mapper
{
    public class MappingProfile : Profile // Automapper Profile inherits from Profile
    {
        //To map model and DTO we need to create this MappingProfile which will create a map between model and DTO and vice versa.
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>(); // Mapping Employee to EmployeeDTO
            CreateMap<EmployeeDTO, Employee>(); // Mapping EmployeeDTO to Employee
        }
    }
}
