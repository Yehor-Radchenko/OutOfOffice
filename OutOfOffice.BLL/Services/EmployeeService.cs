using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.BLL.Dto;
using OutOfOffice.BLL.Exceptions;
using OutOfOffice.BLL.Services.IService;
using OutOfOffice.BLL.ViewModels.Employee;
using OutOfOffice.BLL.ViewModels.LeaveRequest;
using OutOfOffice.Common.Enums;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly OutOfOfficeDbContext _context;

        public EmployeeService(OutOfOfficeDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(EmployeeDto dto)
        {
            ArgumentNullException.ThrowIfNull(nameof(dto));

            int photoId = 0;
            if (dto.Photo is not null && dto.Photo.Length > 0)
            {
                photoId = await UploadPhotoAsync(dto.Photo);
            }

            Employee employeeModel = new Employee
            {
                FullName = dto.FullName,
                Subdivision = dto.Subdivision,
                PositionId = dto.PositionId,
                Status = dto.Status,
                EmployeePartnerId = dto.EmployeePartnerId != 0 ? dto.EmployeePartnerId : null,
                OutOfOfficeBalance = dto.OutOfOfficeBalance,
                PhotoId = photoId != 0 ? photoId : null,
            };

            await _context.Employees.AddAsync(employeeModel);
            await _context.SaveChangesAsync();
            return employeeModel.Id;
        }

        public async Task<int> UploadPhotoAsync(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                byte[] photoBytes = memoryStream.ToArray();

                Photo photoModel = new Photo { Base64Data = photoBytes };
                await _context.Photos.AddAsync(photoModel);
                await _context.SaveChangesAsync();

                return photoModel.Id;
            }
        }

        public async Task<bool> ChangeStatusAsync(int id, EmployeeStatus expectedEmployeeStatus)
        {
            var getEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (getEmployee is null) 
            {
                throw new EntityNotFoundException($"Employee with ID {id} not found.");
            }

            getEmployee.Status = expectedEmployeeStatus;
            _context.Update(getEmployee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BriefEmployeeViewModel>> GetBriefListAsync(string? search = null)
        {
            var result = new List<BriefEmployeeViewModel>();

            if (search != null)
            {
                result = await _context.Employees
                    .Where(e => EF.Functions.Like(e.FullName, $"%{search}%"))
                    .Select(e => new BriefEmployeeViewModel
                    {
                        Id = e.Id,
                        FullName = e.FullName,
                    }).ToListAsync();

                return result;
            }

            result = await _context.Employees
                    .Select(e => new BriefEmployeeViewModel
                    {
                        Id = e.Id,
                        FullName = e.FullName,
                    }).ToListAsync();

            return result;
        }

        public async Task<FullEmployeeViewModel> GetFullInfoByIdAsync(int id)
        {
            var employeeInfo = await _context.Employees
                .Include(e => e.EmployeePartner)
                .Include(e => e.Projects)
                .Include(e => e.LeaveRequests)
                    .ThenInclude(lr => lr.ApprovalRequest)
                .Include(e => e.Photo)
                .Include(e => e.Position)
                .Where(e => e.Id == id)
                .Select(e => new FullEmployeeViewModel
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    Subdivision = e.Subdivision,
                    Status = e.Status.ToString(),
                    OutOfOfficeBalance = e.OutOfOfficeBalance,
                    Photo = e.Photo != null ? e.Photo.Base64Data : null,

                    EmployeePartnerInfo = e.EmployeePartner != null ? new BriefEmployeeViewModel
                    {
                        Id = e.EmployeePartner.Id,
                        FullName = e.EmployeePartner.FullName
                    } : null,

                    Position = e.Position.Name,

                    SubordinateEmployees = _context.Employees
                        .Where(subEmp => subEmp.EmployeePartnerId == e.Id)
                        .Select(subEmp => new BriefEmployeeViewModel
                        {
                            Id = subEmp.Id,
                            FullName = subEmp.FullName
                        }).ToList(),

                    LeaveRequests = e.LeaveRequests
                        .Select(lr => new EmployeeLeaveRequestViewModel
                        {
                            Id = lr.Id,
                            AbsenceReason = lr.AbsenceReason.ToString(),
                            StartDate = lr.StartDate,
                            EndDate = lr.EndDate,
                            Comment = lr.Comment,
                            Status = lr.Status.ToString(),
                            ApproveStatus = lr.ApprovalRequest != null ? lr.ApprovalRequest.Status.ToString() : "No ApprovalRequest info",
                        }).ToList(),

                    ProjectIds = e.Projects.Select(p => p.Id).ToList()
                })
                .FirstOrDefaultAsync();

            if (employeeInfo == null)
            {
                throw new EntityNotFoundException($"Employee with ID {id} not found.");
            }

            return employeeInfo;
        }

        public async Task<List<TableEmployeeViewModel>> GetTableDataAsync(string? searchValue = null)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(e => e.FullName.Contains(searchValue) || e.Subdivision.Contains(searchValue));
            }

            var employees = await query.ToListAsync();

            var result = employees.Select(e => new TableEmployeeViewModel
            {
                Id = e.Id,
                FullName = e.FullName,
                Subdivision = e.Subdivision,
                PositionId = e.PositionId,
                Status = e.Status.ToString(),
                EmployeePartnerId = e.EmployeePartnerId,
                OutOfOfficeBalance = e.OutOfOfficeBalance,
                PhotoId = e.PhotoId
            }).ToList();

            return result;
        }

        public async Task<bool> UpdateAsync(int id, EmployeeDto expectedEntityValues)
        {
            var employee = await _context.Employees
                .Include(e => e.Photo)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return false;
            }

            employee.FullName = expectedEntityValues.FullName;
            employee.Subdivision = expectedEntityValues.Subdivision;
            employee.PositionId = expectedEntityValues.PositionId;
            employee.Status = expectedEntityValues.Status;
            employee.EmployeePartnerId = expectedEntityValues.EmployeePartnerId;
            employee.OutOfOfficeBalance = expectedEntityValues.OutOfOfficeBalance;

            if (expectedEntityValues.Photo != null)
            {
                var newPhotoId = await UploadPhotoAsync(expectedEntityValues.Photo);
                employee.PhotoId = newPhotoId;
            }

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
