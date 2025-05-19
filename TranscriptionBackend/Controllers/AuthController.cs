using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TranscriptionBackend.Data;
using TranscriptionBackend.Models;
using TranscriptionBackend.Utils;

namespace TranscriptionBackend.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
      if (user == null) return Unauthorized("Kullanıcı bulunamadı.");

      // Şifre kontrolü
      if (!VerifyPassword(request.Password, user.PasswordHash))
        return Unauthorized("Şifre yanlış.");

      var token = GenerateJwtToken(user);
      return Ok(new { token });
    }

    private bool VerifyPassword(string password, string storedHash)
    {
      var hashedInput = PasswordHasher.ComputeSha256Hash(password);
      return hashedInput == storedHash;
    }


    private string GenerateJwtToken(User user)
    {
      var jwtSettings = _configuration.GetSection("JwtSettings");
      var secretKey = jwtSettings["SecretKey"];
      var issuer = jwtSettings["Issuer"];
      var audience = jwtSettings["Audience"];

      var claims = new[]
      {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer,
          audience,
          claims,
          expires: DateTime.UtcNow.AddHours(1),
          signingCredentials: creds);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }

  public class LoginRequest
  {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
  }
}
