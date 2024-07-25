using Microsoft.EntityFrameworkCore;
using OutOfOffice.BLL.Services.IService;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.AbsenceReason;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.BLL.Services
{
    public class AbsenceReasonService : IAbsenceReasonService<AbsenceReasonDto, AbsenceReasonViewModel>
    {
        private readonly OutOfOfficeDbContext _context;

        public AbsenceReasonService(OutOfOfficeDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(AbsenceReasonDto dto)
        {
            var reason = new AbsenceReason
            {
                ReasonTitle = dto.ReasonTitle
            };

            await _context.AbsenceReasons.AddAsync(reason);
            await _context.SaveChangesAsync();
            return reason.Id;
        }

        public async Task<IEnumerable<AbsenceReasonViewModel>> GetAllAsync()
        {
            var reason = await _context.AbsenceReasons.ToListAsync();
            var subdivisionViewModels = reason.Select(sub => new AbsenceReasonViewModel
            {
                Id = sub.Id,
                ReasonTitle = sub.ReasonTitle
            });

            return subdivisionViewModels;
        }

        public async Task<bool> UpdateAsync(int id, AbsenceReasonDto dto)
        {
            var reason = await _context.AbsenceReasons.FindAsync(id);
            if (reason == null)
            {
                return false;
            }

            reason.ReasonTitle = dto.ReasonTitle;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
