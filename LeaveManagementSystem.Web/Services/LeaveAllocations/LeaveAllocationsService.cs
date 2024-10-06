using AutoMapper;
using LeaveManagementSystem.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context, 
        IHttpContextAccessor _httpContextAccessor, 
        UserManager<ApplicationUser> _userManager, 
        IMapper _mapper) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            // get all the leave types
            var leaveTypes = await _context.LeaveTypes
                //.Where(q => !q.AllocateLeave.Any(x => x.EmployeeId == employeeId))
                .ToListAsync();

            // get the current period based on the year
            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var monthsRemaining = period.EndDate.Month - currentDate.Month;

            // foreach leave type, create an allocation entry
            foreach (var leaveType in leaveTypes)
            {
                // Works, but not best practice
                //var allocationExists = await AllocationExists(employeeId, period.Id, leaveType.Id);
                //if (allocationExists)
                //{
                //    continue;
                //}
                var accuralRate = decimal.Divide(leaveType.NumberOfDays, 12);
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = period.Id,
                    Days = (int)Math.Ceiling(accuralRate * monthsRemaining)
                };

                _context.Add(leaveAllocation);
            }

            await _context.SaveChangesAsync();
        }

    
    }
}
