using EmployeeManagement.Web.Data;
using EmployeeManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;

namespace EmployeeManagement.Web.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly EmployeeDbContext dbContext;

		public EmployeeController(EmployeeDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddEmployeeVIewModel viewModel)
		{

			if (!ModelState.IsValid)
			{
				// If the model state is invalid, return the view with the current model to show validation errors
				return View(viewModel);
			}

			byte[] profilePictureBytes = null;

			if (viewModel.ProfilePicture != null && viewModel.ProfilePicture.Length > 0)
			{
				// Define the target size
				int targetWidth = 100;
				int targetHeight = 100;

				// Convert the uploaded file into a byte array and resize
				using (var memoryStream = new MemoryStream())
				{
					// Copy the uploaded image into the memory stream
					await viewModel.ProfilePicture.CopyToAsync(memoryStream);

					// Reset the position of the stream to read from the beginning
					memoryStream.Position = 0;

					// Load the image using ImageSharp
					using (var image = Image.Load(memoryStream))
					{
						// Resize the image
						image.Mutate(x => x.Resize(targetWidth, targetHeight));

						// Save the resized image back into the memory stream as byte[]
						using (var outputMemoryStream = new MemoryStream())
						{
							image.SaveAsJpeg(outputMemoryStream); // Save as JPEG or other format if needed
							profilePictureBytes = outputMemoryStream.ToArray();
						}
					}
				}
			}

			var employee = new Employee
			{
				Name = viewModel.Name,
				Email = viewModel.Email,
				Phone = viewModel.Phone,
				DateOfBirth = viewModel.DateOfBirth,
				ProfilePicture = profilePictureBytes // Assign the byte array
			};

			await dbContext.Employees.AddAsync(employee);
			await dbContext.SaveChangesAsync();

			TempData["SuccessMessage"] = "Employee added successfully!";

			return View();
		}



		//[HttpGet]
		//public async Task<IActionResult> EmployeeList()
		//{
		//	var employees = await dbContext.Employees.ToListAsync();
		//	return View(employees);
		//}

		[HttpGet]
		public async Task<IActionResult> EmployeeList(string sortOrder, string searchString)
		{
			// Default sort order is ascending for each column
			ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
			ViewData["PhoneSortParm"] = sortOrder == "Phone" ? "phone_desc" : "Phone";
			ViewData["DateSortParm"] = sortOrder == "DateOfBirth" ? "date_desc" : "DateOfBirth";
			ViewData["CurrentFilter"] = searchString;

			// Fetch employees from the database
			var employees = from e in dbContext.Employees select e;

			// Apply sorting based on the sortOrder parameter
			switch (sortOrder)
			{
				case "name_desc":
					employees = employees.OrderByDescending(e => e.Name);
					break;
				case "Email":
					employees = employees.OrderBy(e => e.Email);
					break;
				case "email_desc":
					employees = employees.OrderByDescending(e => e.Email);
					break;
				case "Phone":
					employees = employees.OrderBy(e => e.Phone);
					break;
				case "phone_desc":
					employees = employees.OrderByDescending(e => e.Phone);
					break;
				case "DateOfBirth":
					employees = employees.OrderBy(e => e.DateOfBirth);
					break;
				case "date_desc":
					employees = employees.OrderByDescending(e => e.DateOfBirth);
					break;
				default:
					employees = employees.OrderBy(e => e.Name); // Default sort by Name
					break;
			}

			if (!String.IsNullOrEmpty(searchString))
			{
				employees = employees.Where(e => e.Name.Contains(searchString) || e.Email.Contains(searchString));
			}

			return View(await employees.ToListAsync());
		}

		[HttpGet]
		public async Task<IActionResult> GetProfilePicture(Guid id)
		{
			var employee = await dbContext.Employees.FindAsync(id);
			if (employee?.ProfilePicture == null)
			{
				return NotFound();
			}

			return File(employee.ProfilePicture, "image/png"); // Adjust MIME type if needed
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var employee = await dbContext.Employees.FindAsync(id);
			return View(employee);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Employee viewModel)
		{
			var employee = await dbContext.Employees.FindAsync(viewModel.Id);

			if (employee != null)
			{
				employee.Name = viewModel.Name;
				employee.Email = viewModel.Email;
				employee.Phone = viewModel.Phone;
				employee.DateOfBirth = viewModel.DateOfBirth;

				await dbContext.SaveChangesAsync();
				// Set success message using TempData
				TempData["SuccessMessage"] = "Employee details updated successfully!";

			}
				return RedirectToAction("EmployeeList", "Employee");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(Employee viewModel)
		{
			var employee = await dbContext.Employees.FindAsync(viewModel.Id);

			if (employee != null) { 
			    
				dbContext.Employees.Remove(employee);
				await dbContext.SaveChangesAsync();
			}

			return RedirectToAction("EmployeeList", "Employee");
		}
	}
}
