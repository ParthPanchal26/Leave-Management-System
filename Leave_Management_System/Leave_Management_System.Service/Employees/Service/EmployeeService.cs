using Leave_Management_System.Models;
using Leave_Management_System.Models.Domain;
using Leave_Management_System.Models.DTO;
using Leave_Management_System.Repository.Employees.IRepositories;
using Leave_Management_System.Service.Employees.IService;
using Leave_Management_System.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        #region GET
        public async Task<ServiceResult<IEnumerable<EmployeeResponseDTO>>> GetEmployeeResponseDTOsAsync()
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

            return new ServiceResult<IEnumerable<EmployeeResponseDTO>>
            {
                Success = true,
                Statuscode = 200,
                ErrorMessage = null,
                Data = employeesDtos
            };

        }
        #endregion

        #region POST
        public async Task<ServiceResult<EmployeeResponseDTO>> CreateEmployeeAsync(EmployeeRequestDTO model)
        {

            //try
            //{
            var existingEmployee = await _employeeRepository.GetEmployeeByEmailAsync(model.Email);

            //if (existingEmployee != null) return null;

            if (existingEmployee != null)
            {
                return new ServiceResult<EmployeeResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = "Employee already exist",
                    Data = null
                };
            }

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

            return new ServiceResult<EmployeeResponseDTO>
            {
                Success = true,
                Statuscode = 200,
                ErrorMessage = null,
                Data = employeeResponseDTO
            };
            //}
            //catch (Exception ex)
            //{

            //    throw new Exception(ex.Message);
            //}

        }

        public async Task<ServiceResult<LoginResponseDTO>> LoginUser(LoginRequestDTO model)
        {
            //try
            //{
            var employee = await _employeeRepository.GetEmployeeByEmailAsync(model.Email);

            if (employee == null)
            {
                return new ServiceResult<LoginResponseDTO>
                {
                    Success = false,
                    Statuscode = 404,
                    ErrorMessage = "Employee not found",
                    Data = null
                };
            }

            if (employee.IsActive == false)
            {
                return new ServiceResult<LoginResponseDTO>
                {
                    Success = false,
                    Statuscode = 404,
                    ErrorMessage = "Employee not found",
                    Data = null
                };
            }

            var token = CreateToken(employee);

            return new ServiceResult<LoginResponseDTO>
            {
                Success = true,
                Statuscode = 200,
                ErrorMessage = null,
                Data = new LoginResponseDTO { Email = model.Email, Token = token }
            };
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
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
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<ServiceResult<EmployeeLeaveResponseDTO>> CreateLeaveRequestAsync(EmployeeLeaveRequestDTO model)
        {
            //try
            //{
            
            if (model.StartDate.Date < DateTime.Now.Date)
            {
                //throw new Exception("Start date can not be in past");
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = "Start date can not be in past",
                    Data = null
                };
            }

            if (model.StartDate.Date > model.EndDate.Date)
            {
                //throw new Exception("Invalid dates selected");
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = "Invalid dates selected",
                    Data = null
                };
            }

            if (model.EndDate.Date < DateTime.Now.Date)
            {
                //throw new Exception("End date can not be in past");
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = "End date can not be in past",
                    Data = null
                };
            }

            if (model.EmployeeId == Guid.Empty)
            {
                //throw new Exception("Invalid employee id found");
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = "Invalid employee id found",
                    Data = null
                };
            }

            if (model.LeaveTypeId < 1)
            {
                //throw new Exception("Invalid leave type found");
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = "Invalid leave type found",
                    Data = null
                };
            }

            // check for holiday
            var holiday = await _employeeRepository.CheckHolidayInDates(model.StartDate, model.EndDate);
            
            if(holiday != null)
            {
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = $"A holiday occurs at {holiday.HolidayDate}",
                    Data = null
                };
            }

            // check for existing leave
            var existingLeave = await _employeeRepository.CheckExistingLeave(model.EmployeeId, model.StartDate, model.EndDate);

            if (existingLeave != null)
            {
                //return null;
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = $"A leave already exist on {existingLeave.StartDate:dd/MM/yyyy}",
                    Data = null
                };
            }

            // create leave request
            var leaveRequest = new LeaveRequest
            {
                EmployeeId = model.EmployeeId,
                LeaveTypeId = model.LeaveTypeId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                LeaveReason = model.LeaveReason,
                LeaveStatus = LeaveStatusValues.LeavePending
            };

            var result = await _employeeRepository.CreateLeaveRequest(leaveRequest);

            var leaveResponse = new EmployeeLeaveResponseDTO
            {
                LeaveId = result.LeaveId,
                EmployeeId = model.EmployeeId,
                Employee = new LeaveIssuer
                {
                    EmployeeId = result.Employee?.EmployeeId,
                    EmployeeEmail = result.Employee?.Email,
                    EmployeeName = result.Employee?.FirstName + " " + result.Employee?.LastName,
                    EmployeePhoneNumber = result.Employee?.PhoneNumber
                },
                LeaveTypeId = result.LeaveTypeId,
                LeaveType = new IssuedLeaveType
                {
                    LeaveTypeId = result.LeaveType?.LeaveTypeId,
                    LeaveTypeName = result.LeaveType?.LeaveTypeName,
                    IsPaid = result.LeaveType?.IsPaid,
                },
                StartDate = result.StartDate,
                EndDate = result.EndDate,
                LeaveReason = result.LeaveReason,
                LeaveStatus = result.LeaveStatus,
                ApprovedBy = result.ApprovedBy,
                Approver = new Approver
                {
                    ApproverId = result.Approver?.EmployeeId,
                    ApproverEmail = result.Approver?.Email,
                    ApproverName = result.Approver?.FirstName + " " + result.Approver?.LastName,
                    ApproverPhoneNumber = result.Approver?.PhoneNumber
                },
                ApproveDate = result.ApproveDate,
                RejectReason = result.RejectReason,
                CreatedAt = result.CreatedAt,
            };

            //return leaveResponse;

            return new ServiceResult<EmployeeLeaveResponseDTO>
            {
                Success = true,
                Statuscode = 200,
                ErrorMessage = null,
                Data = leaveResponse
            };

            //}
            //catch (Exception ex)
            //{

            //    throw new Exception(ex.Message);
            //}
        }
        #endregion

        #region PUT
        public async Task<ServiceResult<EmployeeResponseDTO>> UpdateEmployeeAsync(Guid id, EmployeeUpdateDTO model)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                //return null;
                return new ServiceResult<EmployeeResponseDTO>
                {
                    Success = false,
                    Statuscode = 404,
                    ErrorMessage = "Employee not found",
                    Data = null
                };
            }

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

            //return employeeResponseDTO;
            return new ServiceResult<EmployeeResponseDTO>
            {
                Success = true,
                Statuscode = 200,
                ErrorMessage = null,
                Data = employeeResponseDTO
            };


        }
        #endregion

        #region DELETE

        public async Task<ServiceResult<bool?>> DeleteEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                //return null;
                return new ServiceResult<bool?>
                {
                    Success = false,
                    Statuscode = 404,
                    ErrorMessage = "Employee not found",
                    Data = null
                };
            }

            employee.IsActive = false;

            await _employeeRepository.DeleteEmployeeAsync(employee);

            //return true;
            return new ServiceResult<bool?>
            {
                Success = true,
                Statuscode = 200,
                ErrorMessage = null,
                Data = null
            };

        }

        #endregion
    }
}
