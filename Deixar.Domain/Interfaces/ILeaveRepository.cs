using Deixar.Domain.Entities;

namespace Deixar.Domain.Interfaces
{
    public interface ILeaveRepository
    {
        /// <summary>
        /// Returs all leave request of given user id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        Task<IEnumerable<Leave>> GetAllUserLeaveRequestsByIdAsync(int userId);

        /// <summary>
        /// Returs leave request by leave request id
        /// </summary>
        /// <param name="leaveId">Leave request Id</param>
        /// <returns></returns>
        Task<Leave> GetUserLeaveRequestByIdAsync(Guid leaveId);

        /// <summary>
        /// Create new leave request
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        Task<bool> CreateLeaveRequestAsync(Leave leave);

        /// <summary>
        /// Update leave request
        /// </summary>
        /// <param name="requestId">Leave request Id</param>
        /// <param name="leave">Updated object</param>
        /// <returns></returns>
        Task<bool> UpdateLeaveRequestAsync(Guid requestId, Leave leave);

        /// <summary>
        /// Delete leave request
        /// </summary>
        /// <param name="requestId">Leave request Id</param>
        /// <returns></returns>
        Task<bool> DeleteLeaveRequestAsync(Guid requestId);

        /// <summary>
        /// Approve leave request 
        /// </summary>
        /// <param name="requestId">Leave request Id</param>
        /// <param name="approvedBy">User id</param>
        /// <returns></returns>
        Task<bool> ApproveLeaveRequestAsync(Guid requestId, int approvedBy);

        /// <summary>
        /// Reject leave request 
        /// </summary>
        /// <param name="requestId">Leave request Id</param>
        /// <param name="rejectedBy">User id</param>
        /// <param name="message">Reason to reject</param>
        /// <returns></returns>
        Task<bool> RejectLeaveRequest(Guid requestId, int rejectedBy, string notes);
    }
}
