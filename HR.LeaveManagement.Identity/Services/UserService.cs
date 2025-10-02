using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HR.LeaveManagement.Identity.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirst("uid")?.Value;

    public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        this._userManager = userManager;
        this._httpContextAccessor = httpContextAccessor;
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