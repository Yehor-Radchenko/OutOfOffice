using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OutOfOffice.DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OutOfOffice.Common.Services.Jwt
{
    public class JwtService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly JwtOptions _options;

        public JwtService(UserManager<Employee> userManager, IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _options = jwtOptions.Value;
        }

        public async Task<string> GenerateToken(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Email, $"{employee.Email}"),
                new Claim(ClaimTypes.Name, $"{employee.FullName}"),
            };

            var roles = await _userManager.GetRolesAsync(employee);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_options.ExpiresHours)),
                signingCredentials: sign);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
