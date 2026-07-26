using Leave_Management_System.Models.Domain;
using Leave_Management_System.Models.DTO;
using Leave_Management_System.Repository.Employees.IRepositories;
using Leave_Management_System.Service.Employees.IService;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace Leave_Management_System.Service.Employees.Service
{
    public class EmployeeService : IEmployeeService
    {


        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<EmployeeResponseDTO>> GetEmployeeResponseDTOsAsync()
        {


            var employees = await _employeeRepository.GetEmployeesAsync();

            var employeesDtos = employees.Select(e =>
                    new EmployeeResponseDTO
                    {
                        EmployeeId = e.EmployeeId,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Email = e.Email,
                        PhoneNumber = e.PhoneNumber,
                        DateOfBirth = e.DateOfBirth,
                        HireDate = e.HireDate,
                        Salary = e.Salary,
                        CreatedAt = e.CreatedAt,
                        UpdatedAt = e.UpdatedAt,
                        Department =
                        {
                            DepartmentId = e.Department.DepartmentId,
                            DepartmentName = e.Department.DepartmentName,
                            Description = e.Department.Description
                        },
                        Role =
                        {
                            RoleId = e.Role.RoleId,
                            RoleName = e.Role.RoleName,
                        },
                        Manager =
                        {
                            ManagerId = e.Manager?.ManagerId,
                            Department = e.Manager.Department.DepartmentName,
                            ManagerName = e.Manager.FirstName + " " + e.Manager.LastName,
                            PhoneNumber = e.Manager.PhoneNumber,
                            Role = e.Manager.Role.RoleName,
                        }
                    }
            ).ToList();

            return employeesDtos;

        }

        public async Task<EmployeeResponseDTO?> CreateEmployeeAsync(EmployeeRequestDTO model)
        {

            var passwordHash = new PasswordHasher<EmployeeRequestDTO>()
                .HashPassword(model, model.Password);

            var employeeModel = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                HireDate = model.HireDate,
                Salary = model.Salary,
                PasswordHash = passwordHash,
                ManagerId = model.ManagerId,
                RoleId = model.RoleId,
                DepartmentId = model.DepartmentId,
            };

            var result = await _employeeRepository.CreateEmployeeModelAsync(employeeModel);

            var employeeResponseDTO = new EmployeeResponseDTO
            {
                EmployeeId = result.EmployeeId,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                PhoneNumber = result.PhoneNumber,
                DateOfBirth = result.DateOfBirth,
                HireDate = result.HireDate,
                Salary = result.Salary,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt,
                Department = new Department
                        {
                            DepartmentId = result.Department.DepartmentId,
                            DepartmentName = result.Department.DepartmentName,
                            Description = result.Department.Description
                        },
                Role = new Role
                        {
                            RoleId = result.Role.RoleId,
                            RoleName = result.Role.RoleName,
                        },
                Manager = new Manager
                        {
                            ManagerId = result.Manager?.EmployeeId,
                            Department = result.Manager?.Department.DepartmentName,
                            ManagerName = result.Manager?.FirstName + " " + result.Manager?.LastName,
                            PhoneNumber = result.Manager?.PhoneNumber,
                            Role = result.Manager?.Role.RoleName,
                        }
            };

            return employeeResponseDTO;

        }
    }
}
