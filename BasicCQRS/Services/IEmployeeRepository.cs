using BasicCQRS.Models;

namespace BasicCQRS.Services
{
    public interface IEmployeeRepository
    {
        // This interface is used to define the methods that will be used to interact with the database.
        public Task<IEnumerable<Employee>> GetEmployeesAsync();
        public Task<Employee> GetEmployeeByIdAsync(int id);
        public Task<Employee> AddEmployeeAsync(Employee employee);
        public Task<int> UpdateEmployeeAsync(Employee employee);
        public Task<int> DeleteEmployeeAsync(int id);
        Task<Employee> GetEmployeeByUsernameAsync(string username);
    }

}
