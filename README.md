
# Employee List Management

This is my first project using **C# language and ASP.NET Core (MVC) framework**



## Tech Stack

**Client:** HTML, CSS, Bootstrap, JavaScript

**Server:** C#, ASP.NET Core (MVC)


## Features

- Perform full **CRUD** operations
- **Sort** by name, email, mobile, and date of birth (ascending/descending)
- Upload and **resize** employee **photos**, with backend **conversion to bytecode**
- **Search employees** by name or email with partial matches
- Edit employee details via the Edit button


## Project Overview with Screenshots

- **Employee List:** Displays data from the database, showing the employee list with profile pictures.

![1](https://github.com/user-attachments/assets/c1c145bb-3f05-42dc-aea7-b9a336160fed)


- **Add Employee:** Click on the Add Employee button to input and save new employee details.

![2](https://github.com/user-attachments/assets/69f36e68-2a76-4ddc-a112-efa8c745ca02)


- **Edit:** Modify employee details by clicking the Edit button in the employee list. The Edit page retrieves and displays saved data from the database.

![3](https://github.com/user-attachments/assets/5bb0d576-9160-4ec8-903a-fdb161c0281c)


- **Delete:** Click the Delete button in the employee list to remove an employee.

![5](https://github.com/user-attachments/assets/2467eee1-73cb-4536-9d83-b2141579a694)


- **Search Box:** Find employees by entering partial names or emails in the search box.

![6](https://github.com/user-attachments/assets/1360da01-06da-4f15-91ce-b7ba300dd156)


- **Sortable List:** Employees can be sorted in ascending or descending order. Click on the name, email, phone, or date of birth columns to sort the list accordingly.

![7](https://github.com/user-attachments/assets/3e49995a-a6e3-4673-9dfc-baf70420a368)




## Algorithms and Operations

- This code snippet shows how to handle an uploaded profile picture by resizing it to 100x100 pixels and converting it to a byte array using the ImageSharp library.

```csharp
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
```

- This code snippet demonstrates how to sort a list of employees based on various columns (Name, Email, Phone, DateOfBirth) and order (ascending/descending). It uses sorting parameters from the `sortOrder` variable and applies the appropriate sorting logic to the query.


```csharp
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
```