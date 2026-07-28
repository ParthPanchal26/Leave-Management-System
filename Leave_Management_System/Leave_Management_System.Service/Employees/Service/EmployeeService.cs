using Leave_Management_System.Models.Domain;
using Leave_Management_System.Models.DTO;
using Leave_Management_System.Repository.Employees.IRepositories;
using Leave_Management_System.Service.Employees.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Leave_Management_System.Service.Employees.Service
{
    public class EmployeeService : IEmployeeService
    {


        private readonly IEmployeeRepository _employeeRepository;
        private readonly IConfiguration _configurations;

        public EmployeeService(IEmployeeRepository employeeRepository, IConfiguration configurations)
        {
            _employeeRepository = employeeRepository;
            _configurations = configurations;
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
                        Department = new EmployeeDepartment
                        {
                            DepartmentId = e.Department?.DepartmentId,
                            DepartmentName = e.Department?.DepartmentName,
                            Description = e.Department?.Description
                        },
                        Role = new EmployeeRole
                        {
                            RoleId = e.Role?.RoleId,
                            RoleName = e.Role?.RoleName,
                        },
                        Manager = new Manager
                        {
                            ManagerId = e.Manager?.EmployeeId,
                            Department = e.Manager?.Department?.DepartmentName,
                            ManagerName = e.Manager?.FirstName + " " + e.Manager?.LastName,
                            PhoneNumber = e.Manager?.PhoneNumber,
                            Role = e.Manager?.Role?.RoleName,
                        }
                    }
            ).ToList();

            return employeesDtos;

        }

        public async Task<EmployeeResponseDTO?> CreateEmployeeAsync(EmployeeRequestDTO model)
        {
            try
            {

                var existingEmployee = await _employeeRepository.GetEmployeeByEmailAsync(model.Email);

                if (existingEmployee != null) return null;

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
                    //ManagerId = model.ManagerId,
                    //RoleId = model.RoleId,
                    //DepartmentId = model.DepartmentId,
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
                    Department = new EmployeeDepartment
                    {
                        DepartmentId = result.Department?.DepartmentId,
                        DepartmentName = result.Department?.DepartmentName,
                        Description = result.Department?.Description
                    },
                    Role = new EmployeeRole
                    {
                        RoleId = result.Role?.RoleId,
                        RoleName = result.Role?.RoleName,
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<LoginResponseDTO?> LoginUser(LoginRequestDTO model)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByEmailAsync(model.Email);

                if (employee == null) return null;
                
                if (employee.IsActive == false) return null;

                var token = CreateToken(employee);

                return new LoginResponseDTO { Email = model.Email, Token = token };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string CreateToken(Employee employee)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, employee.Email),
                    new Claim(ClaimTypes.Role, employee?.Role?.RoleName),
                    new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
                };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configurations.GetValue<string>("JWT:JWT_Secret")!)
            );

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configurations.GetValue<string>("JWT:Issuer"),
                audience: _configurations.GetValue<string>("JWT:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<EmployeeResponseDTO?> UpdateEmployeeAsync(Guid id, EmployeeUpdateDTO model)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null) return null;

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Email = model.Email;
            employee.PhoneNumber = model.PhoneNumber;
            employee.DateOfBirth = model.DateOfBirth;
            employee.Salary = model.Salary;
            employee.ManagerId = model.ManagerId;
            employee.RoleId = model.RoleId;
            employee.DepartmentId = model.DepartmentId;

            var result = await _employeeRepository.UpdateEmployeeModelAsync(employee);

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
                Department = new EmployeeDepartment
                {
                    DepartmentId = result.Department?.DepartmentId,
                    DepartmentName = result.Department?.DepartmentName,
                    Description = result.Department?.Description
                },
                Role = new EmployeeRole
                {
                    RoleId = result.Role?.RoleId,
                    RoleName = result.Role?.RoleName,
                },
                Manager = new Manager
                {
                    ManagerId = result.Manager?.EmployeeId,
                    Department = result.Manager?.Department?.DepartmentName,
                    ManagerName = result.Manager?.FirstName + " " + result.Manager?.LastName,
                    PhoneNumber = result.Manager?.PhoneNumber,
                    Role = result.Manager?.Role?.RoleName,
                }
            };

            return employeeResponseDTO;

        }

        public async Task<bool?> DeleteEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null) return null;

            employee.IsActive = false;

            await _employeeRepository.DeleteEmployeeAsync(employee);

            return true;

        }

    }
}
