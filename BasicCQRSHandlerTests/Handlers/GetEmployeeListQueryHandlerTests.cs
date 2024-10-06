using AutoMapper;
using BasicCQRS.Data.Query;
using BasicCQRS.DTO;
using BasicCQRS.Handlers.QueryHandlers;
using BasicCQRS.Models;
using BasicCQRS.Services;
using Moq;

namespace BasicCQRSHandlerTests.Handlers
{
    public class GetEmployeeListQueryHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetEmployeeListQueryHandler _handler;

        public GetEmployeeListQueryHandlerTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetEmployeeListQueryHandler(_employeeRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsListOfEmployeeDTO_WhenEmployeesExist()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Username = "john" },
                new Employee { Id = 2, Name = "Jane Doe", Username = "jane" }
            };

            var employeeDTOs = new List<EmployeeDTO>
            {
                new EmployeeDTO {  Name = "John Doe", Username = "john" },
                new EmployeeDTO {  Name = "Jane Doe", Username = "jane" }
            };

            _employeeRepositoryMock.Setup(repo => repo.GetEmployeesAsync())
                .ReturnsAsync(employees);
            _mapperMock.Setup(m => m.Map<List<EmployeeDTO>>(employees))
                .Returns(employeeDTOs);

            var query = new GetEmployeeListQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.IsType<List<EmployeeDTO>>(result);
            Assert.Equal("John Doe", result[0].Name);
            Assert.Equal("Jane Doe", result[1].Name);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoEmployeesExist()
        {
            // Arrange
            var employees = new List<Employee>(); // No employees
            var employeeDTOs = new List<EmployeeDTO>();

            _employeeRepositoryMock.Setup(repo => repo.GetEmployeesAsync())
                .ReturnsAsync(employees);
            _mapperMock.Setup(m => m.Map<List<EmployeeDTO>>(employees))
                .Returns(employeeDTOs);

            var query = new GetEmployeeListQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
