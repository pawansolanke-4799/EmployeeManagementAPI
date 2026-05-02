using EmployeeManagementApi.DTOs;

namespace EmployeeManagementApi.Repositories; 

public interface IEmployeeRepository
{
  Task<List<EmployeeResponseDto>> GetAllEmployeesAsync(PaginationParams paginationParams);

  Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id);

  Task<EmployeeResponseDto> CreateEmployeeAsync(EmployeeCreateDto dto);

  Task<EmployeeResponseDto?> UpdateEmployeeAsync(int id , EmployeeCreateDto dto);

  Task<EmployeeResponseDto?> DeleteEmployeeAsync(int id);
}