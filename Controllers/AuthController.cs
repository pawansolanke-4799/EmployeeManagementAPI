using EmployeeManagementApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EmployeeManagementApi.Entities;
using System.Text;
using EmployeeManagementApi.Data;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;

namespace EmployeeManagementApi.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly IConfiguration _configuration;

  private readonly AppDbContext _context;

  public AuthController(IConfiguration configuration, AppDbContext context)
  {
    _configuration = configuration;
    _context = context;
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login(LoginDto loginDto)
  {
    var user = await _context.Users
    .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

    if (user == null)
    {
      return Unauthorized("Invalid credentials");
    }

    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
    loginDto.Password,
    user.PasswordHash);

    if (!isPasswordValid)
    {
      return Unauthorized("Invalid credentials");
    }

    var claims = new[]
    {
      new Claim(ClaimTypes.Email, user.Email),
      new Claim(ClaimTypes.Role, user.Role)
    };

    var key = new SymmetricSecurityKey(
      System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      issuer: _configuration["Jwt:Issuer"],
      audience: _configuration["Jwt:Audience"],
      claims: claims,
      expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
      signingCredentials: creds);


    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

    var refreshToken = GenerateRefreshToken();

    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

    await _context.SaveChangesAsync();

    return Ok(new
    {
      accessToken = jwt,
      refreshToken = refreshToken
    });
  }

  [HttpPost("refresh-token")]
  public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
  {
    var principal = GetPrincipalFromExpiredToken(request.AccessToken);

    if (principal == null)
    {
      return BadRequest("Invalid access token");
    }

    var email = principal.FindFirst(ClaimTypes.Email)?.Value;

    if (email == null)
    {
      return BadRequest("Invalid token claims");
    }

    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    if (user == null ||
        user.RefreshToken != request.RefreshToken ||
        user.RefreshTokenExpiryTime <= DateTime.Now)
    {
      return Unauthorized("Invalid refresh token");
    }

    var claims = new[]
    {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
    );

    var creds = new SigningCredentials(
        key,
        SecurityAlgorithms.HmacSha256
    );

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(
            Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])
        ),
        signingCredentials: creds
    );

    var newAccessToken = new JwtSecurityTokenHandler().WriteToken(token);

    var newRefreshToken = GenerateRefreshToken();

    user.RefreshToken = newRefreshToken;
    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

    await _context.SaveChangesAsync();

    return Ok(new
    {
      accessToken = newAccessToken,
      refreshToken = newRefreshToken
    });
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register(RegisterDto registerDto)
  {
    var existingUser = await _context.Users
        .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

    if (existingUser != null)
    {
      return BadRequest("User already exists");
    }

    var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

    var user = new User
    {
      FullName = registerDto.FullName,
      Email = registerDto.Email,
      PasswordHash = passwordHash,
      Role = registerDto.Role
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return Ok("User registered successfully");
  }

  private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
  {
    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidateAudience = true,
      ValidateIssuer = true,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        ),
      ValidateLifetime = false,
      ValidIssuer = _configuration["Jwt:Issuer"],
      ValidAudience = _configuration["Jwt:Audience"]
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    var principal = tokenHandler.ValidateToken(
        token,
        tokenValidationParameters,
        out SecurityToken securityToken
    );

    if (securityToken is not JwtSecurityToken jwtSecurityToken ||
        !jwtSecurityToken.Header.Alg.Equals(
            SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
    {
      return null;
    }

    return principal;
  }

  private string GenerateRefreshToken()

  {

    return Convert.ToBase64String(Guid.NewGuid().ToByteArray())

        + Convert.ToBase64String(Guid.NewGuid().ToByteArray());

  }
}