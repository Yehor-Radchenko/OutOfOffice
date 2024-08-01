using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.Exceptions;
using OutOfOffice.Common.ViewModels.Employee;
using OutOfOffice.Common.ViewModels.LeaveRequest;
using OutOfOffice.Common.Enums;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;
using OutOfOffice.BLL.Services.IService;
using OutOfOffice.Common.ViewModels.AbsenceReason;
using OutOfOffice.Common.ViewModels.Project;
using OutOfOffice.BLL.Exceptions;

namespace OutOfOffice.BLL.Services;

public class EmployeeService : IEmployeeService
{
    private readonly UserManager<Employee> _userManager;
    private readonly OutOfOfficeDbContext _context;

    public EmployeeService(UserManager<Employee> userManager, OutOfOfficeDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<int> RegisterAsync(EmployeeDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var existingEmployee = await _userManager.FindByEmailAsync(dto.Email);
        if (existingEmployee != null)
        {
            throw new ConflictException($"Employee with email '{dto.Email}' already exists.");
        }

        int photoId = 0;
        if (dto.PhotoBase64 is not null && dto.PhotoBase64.Length > 0)
        {
            photoId = await UploadPhotoAsync(dto.PhotoBase64);
        }

        var employeeModel = new Employee
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            SubdivisionId = dto.SubdivisionId,
            PositionId = dto.PositionId,
            Status = EmployeeStatus.Active,
            EmployeePartnerId = dto.EmployeePartnerId != 0 ? dto.EmployeePartnerId : null,
            OutOfOfficeBalance = dto.OutOfOfficeBalance,
            PhotoId = photoId != 0 ? photoId : null
        };

        var result = await _userManager.CreateAsync(employeeModel, dto.Password);
        if (!result.Succeeded)
        {
            throw new ApplicationException($"Failed to create user: {result.Errors.FirstOrDefault()?.Description}");
        }

        employeeModel = await _userManager.FindByEmailAsync(dto.Email);
        if (employeeModel == null)
        {
            throw new ApplicationException("Failed to retrieve the created user.");
        }

        var userManagerResult = await _userManager.AddToRoleAsync(employeeModel, "Employee");
        if (!userManagerResult.Succeeded)
        {
            throw new ApplicationException($"Failed to assign role: {userManagerResult.Errors.FirstOrDefault()?.Description}");
        }

        return employeeModel.Id;
    }


    public async Task<int> UploadPhotoAsync(string base64)
    {
        Photo photoModel = new Photo { Base64Data = base64 };
        await _context.Photos.AddAsync(photoModel);
        await _context.SaveChangesAsync();
        
        return photoModel.Id;
    }

    public async Task<bool> UploadPhotoToEmployeeAsync(int employeeId, string base64)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with Id {employeeId} not found.");
        }

        Photo photoModel = new Photo { Base64Data = base64 };
        await _context.Photos.AddAsync(photoModel);
        await _context.SaveChangesAsync();

        employee.PhotoId = photoModel.Id;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ChangeStatusAsync(int id, EmployeeStatus expectedEmployeeStatus)
    {
        var getEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

        if (getEmployee is null) 
        {
            throw new KeyNotFoundException($"Employee with Id {id} not found.");
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
                .ThenInclude(pr => pr.ProjectManager)
            .Include(e => e.LeaveRequests)
                .ThenInclude(lr => lr.ApprovalRequest)
            .Include(e => e.LeaveRequests)
                .ThenInclude(lr => lr.AbsenceReason)
            .Include(e => e.Photo)
            .Include(e => e.Position)
            .Include(e => e.Subdivision)
            .Where(e => e.Id == id)
            .Select(e => new FullEmployeeViewModel
            {
                Id = e.Id,
                FullName = e.FullName,
                Subdivision = e.Subdivision.Name,
                Status = e.Status.ToString(),
                OutOfOfficeBalance = e.OutOfOfficeBalance,
                PhotoBase64 = e.Photo != null ? e.Photo.Base64Data : null,

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
                        AbsenceReason = new AbsenceReasonViewModel()
                        {
                            Id = lr.AbsenceReason.Id,
                            ReasonTitle = lr.AbsenceReason.ReasonTitle,
                        },
                        StartDate = lr.StartDate,
                        EndDate = lr.EndDate,
                        Comment = lr.Comment,
                        Status = lr.Status.ToString(),
                        ApproveStatus = lr.ApprovalRequest != null ? lr.ApprovalRequest.Status.ToString() : "No ApprovalRequest info",
                    }).ToList(),

                Projects = e.Projects.Select(pr => new BriefProjectViewModel()
                {
                    Id = pr.Id,
                    ProjectManager = new BriefEmployeeViewModel
                    {
                        Id = pr.ProjectManager.Id,
                        FullName = pr.ProjectManager.FullName,
                    },
                    Comment = pr.Comment,
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (employeeInfo == null)
        {
            throw new KeyNotFoundException($"Employee with Id {id} not found.");
        }

        return employeeInfo;
    }

    public async Task<List<TableEmployeeViewModel>> GetTableDataAsync(string? searchValue = null)
    {
        var query = _context.Employees
            .Include(e => e.Subdivision)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(e => e.FullName.Contains(searchValue) || e.Subdivision.Name.Contains(searchValue));
        }

        var employees = await query.ToListAsync();

        var result = employees.Select(e => new TableEmployeeViewModel
        {
            Id = e.Id,
            FullName = e.FullName,
            Subdivision = e.Subdivision.Name,
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
        employee.SubdivisionId = expectedEntityValues.SubdivisionId;
        employee.PositionId = expectedEntityValues.PositionId;
        employee.EmployeePartnerId = expectedEntityValues.EmployeePartnerId;
        employee.OutOfOfficeBalance = expectedEntityValues.OutOfOfficeBalance;

        if (expectedEntityValues.PhotoBase64 != null)
        {
            var newPhotoId = await UploadPhotoAsync(expectedEntityValues.PhotoBase64);
            employee.PhotoId = newPhotoId;
        }

        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemovePhotoAsync(int employeeId)
    {
        var employee = await _context.Employees
            .Include(e => e.Photo)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        ArgumentNullException.ThrowIfNull(nameof(employee));

        if (employee.Photo is null)
        {
            throw new PhotoNotFoundException("There is no photo found to delete.");
        }

        _context.Photos.Remove(employee.Photo);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string> GetPhotoByIdAsync(int userId)
    {
        var photo = await _context.Employees
            .Include (e => e.Photo)
            .Where(e => e.Id == userId)
            .Select(e => e.Photo.Base64Data)
            .FirstOrDefaultAsync();

        return photo ?? string.Empty;
    }
}
