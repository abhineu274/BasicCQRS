using BasicCQRS.Data;
using BasicCQRS.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicCQRS.Services
{
    public class EmployeeRepository : IEmployeeRepository //implements IEmployeeRepository interface
    {
        private readonly DataContext _context; // Database context

        public EmployeeRepository(DataContext context)
        {
            _context = context; // Inject database context based on the configuration in Program.cs
        }

        // Get all employees
        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        // Get an employee by id
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        // Add a new employee
        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            var result = _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        // Update an existing employee
        public async Task<int> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.Id);

            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.Address = employee.Address;
                existingEmployee.Email = employee.Email;
                existingEmployee.Phone = employee.Phone;

                _context.Employees.Update(existingEmployee);
                return await _context.SaveChangesAsync();
            }

            return 0; // No record found
        }

        // Delete an employee by id
        public async Task<int> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                return await _context.SaveChangesAsync();
            }

            return 0; // No record found
        }

        public async Task<Employee> GetEmployeeByUsernameAsync(string username)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);
        }
    }

}
