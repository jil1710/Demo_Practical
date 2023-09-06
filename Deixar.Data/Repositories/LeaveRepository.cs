using Deixar.Data.Contexts;
using Deixar.Domain.Entities;
using Deixar.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Deixar.Data.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDBContext _db;

        public LeaveRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Leave>> GetAllUserLeaveRequestsByIdAsync(int userId)
        {
            return await _db.Leaves.Where(l => l.CreatedBy == userId).ToListAsync();
        }

        public async Task<bool> CreateLeaveRequestAsync(Leave leave)
        {
            try
            {
                await _db.Leaves.AddAsync(leave);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Leave> GetUserLeaveRequestByIdAsync(Guid leaveId)
        {
            return await _db.Leaves.FirstOrDefaultAsync(l => l.Id == leaveId);
        }

        public async Task<bool> UpdateLeaveRequestAsync(Guid requestId, Leave leave)
        {
            try
            {
                var req = await _db.Leaves.SingleAsync(e => e.Id == requestId);
                if (req is null) return false;
                req.LeaveDate = leave.LeaveDate;
                req.IsPaidLeave = leave.IsPaidLeave;
                req.Notes = leave.Notes;
                _db.Leaves.Update(req);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> DeleteLeaveRequestAsync(Guid requestId)
        {
            try
            {
                var request = await _db.Leaves.SingleOrDefaultAsync(l => l.Id == requestId);
                if (request is null) return false;
                _db.Leaves.Remove(request);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ApproveLeaveRequestAsync(Guid requestId, int approvedBy)
        {
            try
            {
                var req = await _db.Leaves.SingleOrDefaultAsync(l => l.Id == requestId);
                if (req is null) return false;
                req.IsApproved = true;
                req.StatusUpdatedBy = approvedBy;
                _db.Leaves.Update(req);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RejectLeaveRequest(Guid requestId, int rejectedBy, string notes)
        {
            try
            {
                var req = await _db.Leaves.SingleOrDefaultAsync(l => l.Id == requestId);
                if (req is null) return false;
                req.IsApproved = false;
                req.StatusUpdatedBy = rejectedBy;
                req.Notes = notes;
                _db.Leaves.Update(req);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
