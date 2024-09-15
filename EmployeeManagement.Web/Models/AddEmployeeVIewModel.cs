using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Web.Models
{
    public class AddEmployeeVIewModel
    {

		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Phone number is required.")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Date of birth is required.")]
		public DateOnly DateOfBirth { get; set; }

		[Required(ErrorMessage = "Photo is required.")]
		public IFormFile ProfilePicture { get; set; }
    }
}
