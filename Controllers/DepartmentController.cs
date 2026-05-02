using EmployeeManagementApi.Data;
using EmployeeManagementApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DepartmentController : ControllerBase
{
  private readonly AppDbContext _context;

  public DepartmentController(AppDbContext context)
  {
    _context = context;
  }

  [HttpGet]
  public async Task<IActionResult> GetDepartments()
  {
    var departments = await _context.Departments.ToListAsync();
    return Ok(departments);
  }

  [Authorize(Roles = "Admin")]
  [HttpPost]
  public async Task<IActionResult> CreateDepartment(Department department)
  {
    _context.Departments.Add(department);
    await _context.SaveChangesAsync();

    return Ok(department);
  }
}