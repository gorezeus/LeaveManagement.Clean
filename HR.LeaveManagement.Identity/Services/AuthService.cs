using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HR.LeaveManagement.Identity.Services;

public class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null) throw new NotFoundException($"User with {request.Email} not found.", request.Email);

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            throw new BadRequestException($"Credential for '{request.Email} aren't valid'");

        var jwtSecurityToken = await GenerateTokenAsync(user);

        return new AuthResponse
        {
            Id = user.Id,
            Email = request.Email,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Username = user.UserName
        };
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest request)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Username,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Employee");
            return new RegistrationResponse { UserId = user.Id };
        }

        var str = new StringBuilder();
        foreach (var err in result.Errors) str.AppendFormat("{0}\n", err.Description);
        throw new BadRequestException($"{str}");
    }

    private async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        // convert roles into claims
        var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

        // merge all claim into one array
        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signinCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signinCredentials
        );
    }
}