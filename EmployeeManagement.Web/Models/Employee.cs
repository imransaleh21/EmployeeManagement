namespace EmployeeManagement.Web.Models
{
    public class Employee
    {
        public Guid Id { get; set; } 
        
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
