using System.ComponentModel.DataAnnotations;
namespace EmployeeManagementApi.Entities;

public class Employee
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set;} = string.Empty;
    public decimal Salary { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
}