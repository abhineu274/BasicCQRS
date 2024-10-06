using AutoMapper;
using BasicCQRS.DTO;
using BasicCQRS.Models;

namespace BasicCQRS.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeDTO, Employee>();
        }
    }
}
