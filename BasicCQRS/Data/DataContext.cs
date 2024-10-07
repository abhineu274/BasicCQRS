using BasicCQRS.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicCQRS.Data
{
    public class DataContext : DbContext // DataContext class inherits from DbContext
    {
        public DbSet<Employee> Employees { get; set; } // DbSet<Employee> is a collection of Employee entities

        public DataContext(DbContextOptions<DataContext> options) : base(options) //ritual
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) // this method is called when the model for a derived context has been initialized
        {
            base.OnModelCreating(modelBuilder);
            //we can also initialize the database with some data from here
            // e.g. modelBuilder.Entity<Employee>().HasData(
            // new Employee { Id = 1, Name = "John Doe", Address = "123 Main St", Email = " [email protected]", Phone = "123-456-7890", Username = " [email protected]", Password = "password" }
            // );
        }
    }
}
