using Microsoft.EntityFrameworkCore;
using OutOfOffice.BLL.Services.IService;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.Subdivision;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.BLL.Services
{
    public class SubdivisionService : ISubdivisionService<SubdivisionDto, SubdivisionViewModel>
    {
        private readonly OutOfOfficeDbContext _context;

        public SubdivisionService(OutOfOfficeDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(SubdivisionDto dto)
        {
            var subdivision = new Subdivision
            {
                Name = dto.Name
            };

            await _context.Subdivisions.AddAsync(subdivision);
            await _context.SaveChangesAsync();
            return subdivision.Id;
        }

        public async Task<IEnumerable<SubdivisionViewModel>> GelAllAsync()
        {
            var subdivisions = await _context.Subdivisions.ToListAsync();
            var subdivisionViewModels = subdivisions.Select(sub => new SubdivisionViewModel
            {
                Id = sub.Id,
                Name = sub.Name
            });

            return subdivisionViewModels;
        }
       
        public async Task<bool> UpdateAsync(int id, SubdivisionDto dto)
        {
            var subdivision = await _context.Subdivisions.FindAsync(id);
            if (subdivision == null)
            {
                return false;
            }

            subdivision.Name = dto.Name;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
