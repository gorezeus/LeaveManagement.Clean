using HR.LeaveManagement.Application.Identity;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace HR.LeaveManagement.Identity.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        this._userManager = userManager;
    }

    public async Task<List<Employee>> GetEmployees()
    {
        var employees = await _userManager.GetUsersInRoleAsync("Employee");
        return employees.Select(e => new Employee
        {
            Email = e.Email,
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName
        }).ToList();
    }

    public async Task<Employee> GetEmployee(string userId)
    {
        var employee = await _userManager.FindByIdAsync(userId);
        return new Employee
        {
            Email = employee.Email,
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName
        };
    }
}