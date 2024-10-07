using BasicCQRS.Controllers;
using BasicCQRS.Data;
using BasicCQRS.Data.Query;
using BasicCQRS.DTO;
using BasicCQRS.Miscellaneous;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BasicCQRSControllerTests.Controller
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock; // Mock IMediator
        private readonly EmployeesController _controller; // Controller to test

        public EmployeesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>(); // Create a new mock of IMediator
            _controller = new EmployeesController(_mediatorMock.Object); // Create a new instance of the controller with the mock object
        }

        private string GenerateJwtToken()
        {
            // Mock token generation for testing
            return "mocked.jwt.token.here"; // Replace with logic to create a valid token if necessary
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithEmployeeList() // Test the Get method of the controller
        {
            // Arrange
            var employeeList = new List<EmployeeDTO>
            {
            new EmployeeDTO {  Name = "John Doe" },
            new EmployeeDTO {  Name = "Jane Doe" }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetEmployeeListQuery>(), default))
                .ReturnsAsync(employeeList);
            // Mock the return value for a list of employees

            // Act
            var result = await _controller.Get();
            //Controller calls the Get method but as we are already mocking the mediator, it will return the mocked value

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<EmployeeDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithEmployee()
        {
            // Arrange
            var employee = new EmployeeDTO { Name = "John Doe", Username = "aj" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetEmployeeByIdQuery>(), default))
                .ReturnsAsync(employee); // Mock the return value for a valid employee

            var token = GenerateJwtToken(); // Generate a valid token

            // Set up the HttpContext and add the token to the header
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<EmployeeDTO>(okResult.Value);
            Assert.Equal("John Doe", returnValue.Name);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenEmployeeNotFound()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetEmployeeByIdQuery>(), default))
                .ReturnsAsync((EmployeeDTO)null); // Simulate that no employee was found

            var token = GenerateJwtToken(); // Generate a valid token

            // Set up the HttpContext and add the token to the header
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _controller.Get(1));

            // Optionally check the exception message
            Assert.Equal("Employee with ID 1 not found.", exception.Message);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtAction_WithEmployee()
        {
            // Arrange
            var newEmployee = new CreateEmployeeCommand("aj", "bhaggaon", "aj@gmail.com", "1231213122", "aj", "aj@123");
            var createdEmployee = new EmployeeDTO { Name = "John Doe" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateEmployeeCommand>(), default))
                .ReturnsAsync(createdEmployee);

            // Act
            var result = await _controller.Post(newEmployee);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<EmployeeDTO>(createdAtActionResult.Value);
            Assert.Equal("John Doe", returnValue.Name);
        }
    }
}
