using BasicCQRS.Data;
using BasicCQRS.Data.Query;
using BasicCQRS.DTO;
using BasicCQRS.Filters;
using BasicCQRS.Miscellaneous;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LoggingActionFilter))] // Apply the LoggingActionFilter to all actions in this controller
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/employees
        [HttpGet]
        public async Task<ActionResult<List<EmployeeDTO>>> Get()
        {
            var query = new GetEmployeeListQuery();
            var employees = await _mediator.Send(query);
            return Ok(employees);
        }

        // GET api/employees/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EmployeeDTO>> Get(int id)
        {
            var query = new GetEmployeeByIdQuery(id);
            var employee = await _mediator.Send(query);

            if (employee == null)
            {
                // Throw a custom exception so the global exception handler can catch it
                throw new NotFoundException($"Employee with ID {id} not found.");
            }
            return Ok(employee);
        }

        // POST api/employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> Post([FromBody] CreateEmployeeCommand command)
        {
            if (!ModelState.IsValid) //validation CreateEmployeeCommandValidator
            {
                return BadRequest(ModelState);
            }
            var createdEmployee = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), createdEmployee);
        }

        // PUT api/employees/5
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Put(int id, [FromBody] UpdateEmployeeCommand command)
        {
            if (!ModelState.IsValid) //validation - UpdateEmployeeCommandValidator
            {
                return BadRequest(ModelState);
            }
            command.Id = id; // Set the Id for the employee to update
            var result = await _mediator.Send(command);

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // DELETE api/employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var command = new DeleteEmployeeCommand(id);
            var result = await _mediator.Send(command);

            if (result == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
