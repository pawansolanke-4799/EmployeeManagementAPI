using EmployeeManagementApi.Data;
using EmployeeManagementApi.DTOs;
using EmployeeManagementApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementApi.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
  private readonly AppDbContext _context;

  public EmployeeRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<List<EmployeeResponseDto>> GetAllEmployeesAsync(PaginationParams paginationParams)
  {
    var query = _context.Employees
               .Include(e => e.Department)
               .AsQueryable();

    if (!string.IsNullOrWhiteSpace(paginationParams.Search))
    {
      query = query.Where(e =>
      e.Name.Contains(paginationParams.Search) ||
      e.Email.Contains(paginationParams.Search));
    }

    if (!string.IsNullOrWhiteSpace(paginationParams.Department))

    {
      query = query.Where(e =>
          e.Department != null &&
          e.Department.Name.Contains(paginationParams.Department));
    }

    query = paginationParams.SortBy?.ToLower() switch
    {
      "name" => paginationParams.SortOrder?.ToLower() == "desc"
          ? query.OrderByDescending(e => e.Name)
          : query.OrderBy(e => e.Name),
      "salary" => paginationParams.SortOrder?.ToLower() == "desc"
          ? query.OrderByDescending(e => e.Salary)
          : query.OrderBy(e => e.Salary),
      _ => paginationParams.SortOrder?.ToLower() == "desc"
          ? query.OrderByDescending(e => e.Id)
          : query.OrderBy(e => e.Id)
    };

    return await query
        .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
        .Take(paginationParams.PageSize)
        .Select(e => new EmployeeResponseDto
        {
          Id = e.Id,
          Name = e.Name,
          Email = e.Email,
          Salary = e.Salary,
          DepartmentId = e.DepartmentId,
          DepartmentName = e.Department != null
                ? e.Department.Name
                : string.Empty
        })
        .ToListAsync();
  }

  public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id)
  {
    return await _context.Employees
        .Include(e => e.Department)
        .Where(e => e.Id == id)
        .Select(e => new EmployeeResponseDto
        {
          Id = e.Id,
          Name = e.Name,
          Email = e.Email,
          Salary = e.Salary,
          DepartmentId = e.DepartmentId,
          DepartmentName = e.Department != null
                ? e.Department.Name
                : string.Empty
        })
        .FirstOrDefaultAsync();
  }

  public async Task<EmployeeResponseDto> CreateEmployeeAsync(EmployeeCreateDto dto)
  {
    var employee = new Employee
    {
      Name = dto.Name,
      Email = dto.Email,
      Salary = dto.Salary,
      DepartmentId = dto.DepartmentId
    };

    _context.Employees.Add(employee);

    await _context.SaveChangesAsync();

    var department = await _context.Departments
        .FindAsync(dto.DepartmentId);

    return new EmployeeResponseDto
    {
      Id = employee.Id,
      Name = employee.Name,
      Email = employee.Email,
      Salary = employee.Salary,
      DepartmentId = employee.DepartmentId,
      DepartmentName = department?.Name ?? string.Empty
    };
  }

  public async Task<EmployeeResponseDto?> UpdateEmployeeAsync(int id, EmployeeCreateDto dto)
  {
    var employee = await _context.Employees.FindAsync(id);

    if (employee == null)
    {
      throw new Exception($"Employee with id {id} not found.");
    }

    employee.Name = dto.Name;
    employee.Email = dto.Email;
    employee.Salary = dto.Salary;
    employee.DepartmentId = dto.DepartmentId;

    await _context.SaveChangesAsync();

    var department = await _context.Departments
        .FindAsync(dto.DepartmentId);

    return new EmployeeResponseDto
    {
      Id = employee.Id,
      Name = employee.Name,
      Email = employee.Email,
      Salary = employee.Salary,
      DepartmentId = employee.DepartmentId,
      DepartmentName = department?.Name ?? string.Empty
    };
  }

  public async Task<EmployeeResponseDto?> DeleteEmployeeAsync(int id)
  {
    var employee = await _context.Employees.FindAsync(id);

    if (employee == null)
    {
      return null;
    }

    _context.Employees.Remove(employee);
    await _context.SaveChangesAsync();

    var department = await _context.Departments
        .FindAsync(employee.DepartmentId);

    return new EmployeeResponseDto
    {
      Id = employee.Id,
      Name = employee.Name,
      Email = employee.Email,
      Salary = employee.Salary,
      DepartmentId = employee.DepartmentId,
      DepartmentName = department?.Name ?? string.Empty
    };
  }
}