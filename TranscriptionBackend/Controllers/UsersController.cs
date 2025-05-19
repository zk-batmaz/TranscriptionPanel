using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TranscriptionBackend.Services;
using TranscriptionBackend.Models;

namespace TranscriptionBackend.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UsersController : ControllerBase
  {
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
      _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public IActionResult GetAllUsers()
    {
      try
      {
        var users = _userService.GetAllUsers();
        return Ok(users);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error occurred while retrieving users", error = ex.Message });
      }
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
      try
      {
        if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
          return BadRequest(new { message = "Username and password required" });
        }

        var user = new User
        {
          Username = request.Username,
          PasswordHash = request.Password, // UserService'de hashlenecek
          Role = request.Role
        };

        _userService.AddUser(user);
        

        var userResponse = new
        {
          Id = user.Id,
          Username = user.Username,
          Role = user.Role
        };
        
        return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, userResponse);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error occurred while creating user", error = ex.Message });
      }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteUser(int id)
    {
      try
      {
        var user = _userService.GetUserById(id);
        if (user == null)
          return NotFound(new { message = "User not found" });

        _userService.DeleteUser(id);
        return Ok(new { message = "User deleted successfully" });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error occurred while deleting user", error = ex.Message });
      }
    }
  }

  // DTO sınıfları
  public class CreateUserRequest
  {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "editor";
  }
}