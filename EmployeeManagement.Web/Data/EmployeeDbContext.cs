using EmployeeManagement.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Web.Data
{
    public class EmployeeDbContext: DbContext //employeeDbContext is the bridge between our SQL and application
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext>options) : base(options)
        {
            
        }
        public DbSet<Employee> Employees {  get; set; }
    }

}
