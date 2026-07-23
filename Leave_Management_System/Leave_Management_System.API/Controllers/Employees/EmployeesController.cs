using Leave_Management_System.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Leave_Management_System.API.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        #region GET

        public async Task<IActionResult> GetAllEmployees()
        {
            return Ok();
        }

        #endregion

    }
}
