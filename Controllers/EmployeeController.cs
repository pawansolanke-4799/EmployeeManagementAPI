using EmployeeManagementApi.Data;
using EmployeeManagementApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementApi.DTOs;
using EmployeeManagementApi.Repositories;
using EmployeeManagementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
//[Authorize]
public class EmployeeController : ControllerBase
{
  private readonly IEmployeeServices _employeeServices;

  public EmployeeController(IEmployeeServices employeeServices)
  {
    _employeeServices = employeeServices;
  }

  [HttpGet]
  [ResponseCache(Duration = 60)]
  public async Task<IActionResult> GetEmployees([FromQuery] PaginationParams paginationParams)
  {
    var employees = await _employeeServices.GetAllEmployeesAsync(paginationParams);
    return Ok(employees);
  }

  [HttpGet("{id}")]
  [ResponseCache(Duration = 60)]
  public async Task<IActionResult> GetEmployee(int id)
  {
    var employee = await _employeeServices.GetEmployeeByIdAsync(id);

    if (employee == null)
    {
      return NotFound("Employee not found");
    }

    return Ok(employee);
  }

  [HttpPost]
  public async Task<IActionResult> CreateEmployee(EmployeeCreateDto employeeDto)
  {
    var employee = await _employeeServices.CreateEmployeeAsync(employeeDto);
    return Ok(employee);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateEmployee(int id, EmployeeCreateDto employeeDto)
  {
    var employee = await _employeeServices.UpdateEmployeeAsync(id, employeeDto);
    return Ok(employee);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteEmployee(int id)
  {
    var employee = await _employeeServices.DeleteEmployeeAsync(id);

    if (employee == null)
    {
      return NotFound("Employee not found");
    }

    return Ok(employee);
  }
}

