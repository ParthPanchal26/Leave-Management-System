using Leave_Management_System.Models.DTO;
using Leave_Management_System.Service.Employees.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Leave_Management_System.API.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        #region GET

        [Authorize(Roles = "HR,Admin")]
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDTO>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetEmployeeResponseDTOsAsync();

                _logger.LogInformation($"Employees fetched: ${JsonSerializer.Serialize(employees)}");

                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Employees not found: ${ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to fetch employees");
            }
        }

        #endregion

        #region POST
        [Authorize(Roles = "HR,Admin")]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeeResponseDTO>> CreateEmployee([FromBody] EmployeeRequestDTO model)
        {
            try
            {

                if (!ModelState.IsValid) BadRequest("Invalid inputs");

                var result = await _employeeService.CreateEmployeeAsync(model);

                if (result == null)
                {
                    _logger.LogError($"Employee not Created and exists! Employee: ${JsonSerializer.Serialize(model)}");
                    return BadRequest("User already exist");
                }

                _logger.LogInformation($"Employee Created: ${JsonSerializer.Serialize(result)}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Employees not Created: ${ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create employee");
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponseDTO>> LoginEmployee([FromBody] LoginRequestDTO model)
        {
            try
            {

                if (!ModelState.IsValid) BadRequest("Invalid inputs");

                var result = await _employeeService.LoginUser(model);

                if (result == null)
                {
                    _logger.LogError($"Employee not found! Employee: ${JsonSerializer.Serialize(model)} ");
                    return NotFound("User not found");
                }

                _logger.LogInformation($"Employee Logged In! Employee Email: ${model.Email}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Employees not Sign-In: ${ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to login employee");
            }
        }

        #endregion

        #region PUT
        [Authorize(Roles = "HR,Admin")]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeeResponseDTO>> UpdateEmployee([FromRoute] Guid id, [FromBody] EmployeeUpdateDTO model)
        {
            try
            {
                if (id == Guid.Empty) BadRequest("Invalid inputs");

                if (!ModelState.IsValid) BadRequest("Invalid inputs");

                var result = await _employeeService.UpdateEmployeeAsync(id, model);

                if (result == null)
                {
                    _logger.LogError($"Employee not found! Employee: ${JsonSerializer.Serialize(model)}");
                    return NotFound("Employee not found");
                }

                _logger.LogInformation($"Employee Updated! Employee: ${JsonSerializer.Serialize(result)}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Employees not Updated: ${ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update employees");
            }
        }

        #endregion

        #region DELETE
        [Authorize(Roles = "HR,Admin")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            try
            {

                if (id == Guid.Empty) BadRequest("Invalid inputs");

                var result = await _employeeService.DeleteEmployeeByIdAsync(id);

                if (result == null)
                {
                    _logger.LogError($"Employee Not deleted and not exist! Employee id: ${id}");
                    return NotFound("Employee does not exist");
                }

                _logger.LogInformation($"Employee Deleted! Employee Id: ${id}");
                return Ok("Employee deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Employees not Deleted: ${ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete employee");
            }
        }

        #endregion

    }
}
