using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementApi.DTOs;

public class EmployeeCreateDto
{
  [Required(ErrorMessage = "Name is required")]
  [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
  public string Name { get; set; } = string.Empty;

  [Required(ErrorMessage = "Email is required")]
  [EmailAddress(ErrorMessage = "Invalid email format")]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "Salary must be greater than 0")]
  public decimal Salary { get; set; }

  [Required(ErrorMessage = "Department is required")]
  public int DepartmentId { get; set; }
}