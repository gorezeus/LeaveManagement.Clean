using HR.LeaveManagement.Application.Models.Identity;

namespace HR.LeaveManagement.Application.Identity;
public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task<RegistrationResponse> Register(RegistrationRequest request);
}
