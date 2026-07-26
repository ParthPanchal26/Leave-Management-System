using Leave_Management_System.Models.DTO;
using Leave_Management_System.Service.Employees.IService;
using Microsoft.AspNetCore.Mvc;

namespace Leave_Management_System.API.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        #region GET

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDTO>>> GetAllEmployees()
        {
            var employees = await _employeeService.GetEmployeeResponseDTOsAsync();
            return Ok(employees);
        }

        #endregion

        #region POST

        [HttpPost]
        public async Task<ActionResult<EmployeeResponseDTO>> CreateEmployee([FromBody]EmployeeRequestDTO model)
        {
            return await _employeeService.CreateEmployeeAsync(model);
        }

        #endregion


    }
}
