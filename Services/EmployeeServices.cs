using EmployeeManagementApi.DTOs;
using EmployeeManagementApi.Repositories;

namespace EmployeeManagementApi.Services;

public class EmployeeServices : IEmployeeServices
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeServices(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<EmployeeResponseDto>> GetAllEmployeesAsync(PaginationParams paginationParams)
    {
        return await _employeeRepository.GetAllEmployeesAsync(paginationParams);
    }

    public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id)
    {
        return await _employeeRepository.GetEmployeeByIdAsync(id);
    }

    public async Task<EmployeeResponseDto> CreateEmployeeAsync(EmployeeCreateDto dto)
    {
        return await _employeeRepository.CreateEmployeeAsync(dto);
    }

    public async Task<EmployeeResponseDto?> UpdateEmployeeAsync(int id, EmployeeCreateDto dto)
    {
        return await _employeeRepository.UpdateEmployeeAsync(id, dto);
    }

    public async Task<EmployeeResponseDto?> DeleteEmployeeAsync(int id)
    {
        
        return await _employeeRepository.DeleteEmployeeAsync(id);
    }
}