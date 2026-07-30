using Leave_Management_System.API.Controllers.Employees;
using Leave_Management_System.Models;
using Leave_Management_System.Models.DTO;
using Leave_Management_System.Service.Employees.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leave_Management_System.API.Controllers.LeaveRequest
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILogger<EmployeesController> _logger;

        public LeaveRequestsController(ILeaveRequestService leaveRequestService, ILogger<EmployeesController> logger)
        {
            _leaveRequestService = leaveRequestService;
            _logger = logger;
        }

        #region POST

        // Employee Leave Request
        [HttpPost("leave-request")]
        [Authorize(Roles = "HR,Employee,Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] EmployeeLeaveRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid input");

            var result = await _leaveRequestService.CreateLeaveRequestAsync(model);

            if (result.Success)
            {
                return StatusCode(result.Statuscode, result.Data);
            }
            else
            {
                _logger.LogError(result.ErrorMessage);
                return StatusCode(result.Statuscode, result.ErrorMessage);
            }
        }

        #endregion

        #region PUT
        // Employee Leave Request
        [HttpPut("{leaveId:guid}")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult<EmployeeLeaveResponseDTO>>> UpdateLeaveRequest([FromRoute] Guid leaveId, [FromBody] EmployeeLeaveRequestUpdateDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid inputs");

            var result = await _leaveRequestService.UpdateLeaveRequestAsync(leaveId, model);

            if (result.Success)
            {
                return StatusCode(result.Statuscode, result.Data);
            }
            else
            {
                _logger.LogError(result.ErrorMessage);
                return StatusCode(result.Statuscode, result.ErrorMessage);
            }


        }

        #endregion

    }
}
