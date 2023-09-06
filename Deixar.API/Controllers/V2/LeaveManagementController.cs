using Castle.Components.DictionaryAdapter.Xml;
using Deixar.API.Commons;
using Deixar.Data.Contexts;
using Deixar.Domain.DTOs;
using Deixar.Domain.Entities;
using Deixar.Domain.Interfaces;
using Deixar.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deixar.API.Controllers.V2
{
    [LogMethod]
    [ApiController]
    [ApiVersion("2.0")]
    [EndpointGroupName("v2")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LeaveManagementController : ControllerBase
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly ApplicationDBContext applicationDBContext;
        private readonly IEmailUtility emailUtility;

        public LeaveManagementController(ILeaveRepository leaveRepository, ApplicationDBContext applicationDBContext, IEmailUtility emailUtility)
        {
            _leaveRepository = leaveRepository;
            this.applicationDBContext = applicationDBContext;
            this.emailUtility = emailUtility;
        }

        /// <summary>
        /// Returns all leave request of current user (Login required)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllLeaveRequest()
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id")!.Value);
            IEnumerable<Leave> leaves = await _leaveRepository.GetAllUserLeaveRequestsByIdAsync(userId);
            return Ok(new { Message = $"Leave request of UserId {userId} retrived successfully.", Leaves = leaves });
        }

        /// <summary>
        /// Get single leave request given by Leave Id
        /// </summary>
        /// <param name="leaveRequestId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLeaveRequest(Guid leaveRequestId)
        {
            Leave leave = await _leaveRepository.GetUserLeaveRequestByIdAsync(leaveRequestId);
            if (leave == null) return NotFound("Leave request not found!");
            return Ok(new { Message = $"Leave request retrived successfully.", Leave = leave });
        }

        /// <summary>
        /// Add new leave request
        /// </summary>
        /// <param name="leaveRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddLeaveRequestAsync(LeaveRequestModel leaveRequest)
        {
            if (!ModelState.IsValid) return BadRequest(new { Error = "Please enter valid details" });

            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id")!.Value);
            Leave leave = new()
            {
                CreatedBy = userId,
                IsPaidLeave = leaveRequest.IsPaidLeave,
                Reason = leaveRequest.Reason,
                LeaveDate = leaveRequest.LeaveDate,
                IsApproved = false
            };

            bool isCreated = await _leaveRepository.CreateLeaveRequestAsync(leave);
            if (!isCreated) return BadRequest(new { Error = "Something went wrong while creating leave request!" });

            return Ok(new { Message = $"Leave request created successfully. (ID: {leave.Id})" });
        }

        /// <summary>
        /// Update existing leave request
        /// </summary>
        /// <param name="leaveRequestId"> leave</param>
        /// <param name="leaveRequest"> leave</param>
        /// <returns></returns>``
        [HttpPut]
        public async Task<IActionResult> UpdateLeaveRequestAsync(Guid leaveRequestId, LeaveRequestModel leaveRequest)
        {
            if (!ModelState.IsValid || String.IsNullOrEmpty(leaveRequestId.ToString())) return BadRequest(new { Error = "Please enter valid details" });

            Leave leave = new()
            {
                IsPaidLeave = leaveRequest.IsPaidLeave,
                Reason = leaveRequest.Reason,
                LeaveDate = leaveRequest.LeaveDate,
            };

            bool isUpdated = await _leaveRepository.UpdateLeaveRequestAsync(leaveRequestId, leave);
            if (!isUpdated) return BadRequest(new { Error = "Something went wrong while updating leave request!" });
            return Ok(new { Message = "Leave request updated successfully" });
        }

        /// <summary>
        /// Remove leave request 
        /// </summary>
        /// <param name="leaveRequestId">Leave request id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteLeaveRequest(Guid leaveRequestId)
        {
            bool isDeleted = await _leaveRepository.DeleteLeaveRequestAsync(leaveRequestId);
            if (!isDeleted) return BadRequest(new { Error = "Something went wrong while deleting leave request!" });
            return Ok(new { Message = "Leave request deleted successfully." });
        }

        /// <summary>
        /// Approve leave request (HR access only)
        /// </summary>
        /// <param name="leaveRequestId">Leave request id</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> ApproveLeaveRequest(Guid leaveRequestId)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id")!.Value);
            var leave = await applicationDBContext.Leaves.FindAsync(leaveRequestId);
            var user = await applicationDBContext.Users.FindAsync(leave.CreatedBy);
            bool isApproved = await _leaveRepository.ApproveLeaveRequestAsync(leaveRequestId, userId);
            if (!isApproved) return BadRequest(new { Error = "Something went wrong while approving leave request!" });
            var msg = new Message(new[] { user!.EmailAddress },"Leave Request Approval", "Your Leave request is approved!");
            emailUtility.SendMail(msg);
            return Ok(new { Message = "Leave request approved successfully." });
        }

        /// <summary>
        /// Reject leave request (HR access only)
        /// </summary>
        /// <param name="leaveRequestId">Leave request id</param>
        /// <param name="notes">Notes</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> RejectLeaveRequest(Guid leaveRequestId, string notes)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id")!.Value);
            var leave = await applicationDBContext.Leaves.FindAsync(leaveRequestId);

            var user = await applicationDBContext.Users.FindAsync(leave.CreatedBy);
            bool isRejected = await _leaveRepository.RejectLeaveRequest(leaveRequestId, userId, notes);
            if (!isRejected) return BadRequest(new { Error = "Something went wrong while rejecting leave request!" });
            var msg = new Message(new[] { user!.EmailAddress }, "Leave Request Rejection", $"Your Leave request is Rejected due to {notes}!");
            emailUtility.SendMail(msg);
            return Ok(new { Message = "Leave request rejected." });
        }
    }
}
