﻿using Microsoft.EntityFrameworkCore;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.Employee;
using OutOfOffice.Common.ViewModels.Project;
using OutOfOffice.Common.Enums;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.Common.Services;

public class ProjectService
{
    private readonly OutOfOfficeDbContext _context;

    public ProjectService(OutOfOfficeDbContext context)
    {
        _context = context;
    }

    public async Task<List<TableProjectViewModel>> GetAllProjectsAsync()
    {
        var projects = await _context.Projects
            .Select(p => new TableProjectViewModel
            {
                Id = p.Id,
                ProjectType = p.ProjectType.ToString(),
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ProjectManagerId = p.ProjectManagerId,
                Comment = p.Comment,
                Status = p.Status.ToString()
            })
            .ToListAsync();

        return projects;
    }

    public async Task<FullProjectViewModel?> GetFullProjectByIdAsync(int id)
    {
        var project = await _context.Projects
            .Include(p => p.ProjectManager)
            .Include(p => p.Employees)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
            return null;

        var viewModel = new FullProjectViewModel
        {
            Id = project.Id,
            ProjectType = project.ProjectType.ToString(),
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Comment = project.Comment,
            Status = project.Status.ToString(),
            ProjectManager = new BriefEmployeeViewModel
            {
                Id = project.ProjectManager.Id,
                FullName = project.ProjectManager.FullName
            },
            InvolvedEmployees = project.Employees.Select(e => new BriefEmployeeViewModel
            {
                Id = e.Id,
                FullName = e.FullName
            })
        };

        return viewModel;
    }

    public async Task<Project> AddProjectAsync(ProjectDto dto)
    {
        var project = new Project
        {
            ProjectType = dto.ProjectType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            ProjectManagerId = dto.ProjectManagerId,
            Comment = dto.Comment,
            Status = dto.Status
        };

        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        return project;
    }

    public async Task<bool> UpdateProjectAsync(int id, ProjectDto dto)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null)
            throw new KeyNotFoundException($"Project with ID {id} not found.");

        project.ProjectType = dto.ProjectType;
        project.StartDate = dto.StartDate;
        project.EndDate = dto.EndDate;
        project.ProjectManagerId = dto.ProjectManagerId;
        project.Comment = dto.Comment;

        _context.Projects.Update(project);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeactivateProjectAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null)
            throw new KeyNotFoundException($"Project with ID {id} not found.");

        project.Status = ProjectStatus.Deactivated;
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddEmployeeToProjectAsync(int projectId, int employeeId)
    {
        var project = await _context.Projects
            .Include(p => p.Employees)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");

        var employee = await _context.Employees.FindAsync(employeeId);

        if (employee == null)
            throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

        if (project.Employees.Contains(employee))
            return false;

        project.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveEmployeeFromProjectAsync(int projectId, int employeeId)
    {
        var project = await _context.Projects
            .Include(p => p.Employees)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");

        var employee = project.Employees.FirstOrDefault(e => e.Id == employeeId);

        if (employee == null)
            throw new KeyNotFoundException($"Employee with ID {employeeId} not found in the project.");

        project.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return true;
    }
}
