using Leave_Management_System.Models;
using Leave_Management_System.Models.Domain;
using Leave_Management_System.Models.DTO;
using Leave_Management_System.Models.Enum;
using Leave_Management_System.Repository.Employees.IRepositories;
using Leave_Management_System.Service.Employees.IService;
using Leave_Management_System.Utility;

namespace Leave_Management_System.Service.Employees.Service
{
    public class LeaveRequestService : ILeaveRequestService
    {

        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }

        #region POST

        public async Task<ServiceResult<EmployeeLeaveResponseDTO>> CreateLeaveRequestAsync(EmployeeLeaveRequestDTO model)
        {
            // chekc if startdate is in past
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

            // chekc if startdate appears after past date
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

            // chekc if enddate is in past
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

            // check if dates falls on weekend
            var weekdays = new[]
            {
                model.StartDate,
                model.EndDate
            }.Where(d => d.DayOfWeek == DayOfWeek.Sunday || d.DayOfWeek == DayOfWeek.Saturday).ToList();

            if (weekdays.Any())
            {
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = $"Leave can not be created on {weekdays[0].DayOfWeek} day - {weekdays[0]:dd/MM/yyyy}",
                    Data = null
                };
            }

            // check if weekend falls between dates
            var dates = UtilityMethods.GetDatesBetween(model.StartDate, model.EndDate);
            var weekDaysInBetween = new List<DateTime>(dates).Where(d => d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday).ToList();

            if (weekDaysInBetween.Any())
            {
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = $"{weekDaysInBetween[0].DayOfWeek} appears on {weekDaysInBetween[0]:dd/MM/yyyy}",
                    Data = null
                };
            }

            // check if employee id is wrong
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

            // check if leave type is wrong
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

            // check if holiday appears between dates
            var holiday = await _leaveRequestRepository.CheckHolidayInDates(model.StartDate, model.EndDate);

            if (holiday != null)
            {
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = $"A holiday of {holiday.HolidayName} occurs on {holiday.HolidayDate.DayOfWeek} {holiday.HolidayDate:dd/MM/yyyy}",
                    Data = null
                };
            }

            // check existing leave appears between dates
            var existingLeave = await _leaveRequestRepository.CheckExistingLeave(model.EmployeeId, model.StartDate, model.EndDate);

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

            // fetch leaveType info
            var leaveDetails = await _leaveRequestRepository.GetLeaveTypeInfo(model.LeaveTypeId);

            if (leaveDetails == null)
            {
                //return null;
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 404,
                    ErrorMessage = $"Leave type not found",
                    Data = null
                };
            }

            var requestedLeaveDays = (model.EndDate.Date - model.StartDate.Date).Days + 1;


            if (leaveDetails.MaxDaysPerYear < requestedLeaveDays)
            {
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = $"You can not have more than {leaveDetails.MaxDaysPerYear} leaves for {leaveDetails.LeaveTypeName}",
                    Data = null
                };
            }

            // fetch all approved-pending leaveRequests using employeeId & leaveTypeId
            var allLeaves = await _leaveRequestRepository.GetLeavesByEmployeeIdLeaveTypeId(model.EmployeeId, model.LeaveTypeId);

            // calculate total leave days
            var totalLeaveDays = 0;
            foreach (var leave in allLeaves)
            {
                totalLeaveDays += (leave.EndDate.Date - leave.StartDate.Date).Days + 1;
            }

            // calculate remaining days for leaveType
            var totalRemainingDays = leaveDetails.MaxDaysPerYear - totalLeaveDays;
            if (totalRemainingDays <= 0)
            {
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = $"You have exceeded total {leaveDetails.MaxDaysPerYear} leaves for {leaveDetails.LeaveTypeName}",
                    Data = null
                };
            }

            if (requestedLeaveDays > totalRemainingDays)
            {
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 400,
                    ErrorMessage = $"You only have {totalRemainingDays} {leaveDetails.LeaveTypeName} day(s) remaining.",
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
                LeaveStatus = LeaveStatus.PENDING,
            };

            var result = await _leaveRequestRepository.CreateLeaveRequest(leaveRequest);

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
                LeaveStatus = result.LeaveStatus.ToString(),
                ReviewedBy = result.ReviewedBy,
                Reviewer = new Reviewer
                {
                    ReviewerId = result.Reviewer?.EmployeeId,
                    ReviewerEmail = result.Reviewer?.Email,
                    ReviewerName = result.Reviewer?.FirstName + " " + result.Reviewer?.LastName,
                    ReviewerPhoneNumber = result.Reviewer?.PhoneNumber
                },
                ApproveDate = result.ApproveDate,
                RejectReason = result.RejectReason,
                CreatedAt = result.CreatedAt,
            };

            return new ServiceResult<EmployeeLeaveResponseDTO>
            {
                Success = true,
                Statuscode = 200,
                ErrorMessage = null,
                Data = leaveResponse
            };

        }

        #endregion

        #region PUT

        public async Task<ServiceResult<EmployeeLeaveResponseDTO>> UpdateLeaveRequestAsync(Guid leaveId, EmployeeLeaveRequestUpdateDTO model)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestById(leaveId);

            if (leaveRequest == null)
            {
                return new ServiceResult<EmployeeLeaveResponseDTO>
                {
                    Success = false,
                    Statuscode = 404,
                    ErrorMessage = "Leave not found",
                    Data = null
                };
            }

            leaveRequest.LeaveStatus = model.LeaveStatus;
            leaveRequest.ReviewedBy = model.ReviewedBy;

            if (leaveRequest.LeaveStatus == LeaveStatus.APPROVED)
            {
                leaveRequest.ApproveDate = DateTime.Now;
            }

            if (leaveRequest.LeaveStatus == LeaveStatus.REJECTED)
            {
                leaveRequest.ApproveDate = null;
                leaveRequest.RejectReason = model.RejectReason;
            }

            var result = await _leaveRequestRepository.UpdateLeaveRequest(leaveRequest);

            var leaveResponse = new EmployeeLeaveResponseDTO
            {
                LeaveId = result.LeaveId,
                EmployeeId = result.EmployeeId,
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
                LeaveStatus = result.LeaveStatus.ToString(),
                ReviewedBy = result.ReviewedBy,
                Reviewer = new Reviewer
                {
                    ReviewerId = result.Reviewer?.EmployeeId,
                    ReviewerEmail = result.Reviewer?.Email,
                    ReviewerName = result.Reviewer?.FirstName + " " + result.Reviewer?.LastName,
                    ReviewerPhoneNumber = result.Reviewer?.PhoneNumber
                },
                ApproveDate = result.ApproveDate,
                RejectReason = result.RejectReason,
                CreatedAt = result.CreatedAt,
            };

            return new ServiceResult<EmployeeLeaveResponseDTO>
            {
                Success = true,
                Statuscode = 200,
                ErrorMessage = null,
                Data = leaveResponse
            };

        }

        //private async Task UpdateLeaveApprovedBalance(LeaveRequest leaveRequest)
        //{

        //    var leaveDetails = await _leaveRequestRepository.GetLeaveTypeInfo(leaveRequest.LeaveTypeId);

        //    var lastLeave = await _leaveRequestRepository.GetLeaveBalance(leaveRequest.EmployeeId, leaveRequest.LeaveTypeId);

        //    var usedDays = 1;

        //    if(lastLeave != null)
        //    {
        //        usedDays = lastLeave.UsedDays + 1;
        //    }

        //    var leaveBalance = new LeaveBalance
        //    {
        //        EmployeeId = leaveRequest.EmployeeId,
        //        LeaveTypeId = leaveRequest.LeaveTypeId,
        //        LeaveYear = leaveRequest.CreatedAt.Year,
        //        AllocatedDays = leaveDetails.MaxDaysPerYear,
        //        UsedDays = usedDays,
        //    };

        //    await _leaveRequestRepository.CreateLeaveBalance(leaveBalance);
        //}

        #endregion

    }
}
