using TranscriptionBackend.Data;
using TranscriptionBackend.Models;

namespace TranscriptionBackend.Services
{
  public class UserService
  {
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
      _context = context;
    }

    public IEnumerable<User> GetAllUsers()
    {
      return _context.Users.ToList();
    }

    public User? GetUserById(int id)
    {
      return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public User? GetUserByUsername(string username)
    {
      return _context.Users.FirstOrDefault(u => u.Username == username);
    }

    public void AddUser(User user)
    {
      user.PasswordHash = Utils.PasswordHasher.ComputeSha256Hash(user.PasswordHash);
      _context.Users.Add(user);
      _context.SaveChanges();
    }

    public void UpdateUser(User user)
    {
      var existingUser = _context.Users.Find(user.Id);
      if (existingUser != null)
      {
        existingUser.Username = user.Username;
        existingUser.Role = user.Role;
        
        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
          existingUser.PasswordHash = Utils.PasswordHasher.ComputeSha256Hash(user.PasswordHash);
        }
        
        _context.SaveChanges();
      }
    }

    public void DeleteUser(int id)
    {
      var user = _context.Users.Find(id);
      if (user != null)
      {
        _context.Users.Remove(user);
        _context.SaveChanges();
      }
    }

    public bool ValidateUser(string username, string password)
    {
      var user = GetUserByUsername(username);
      if (user == null) return false;

      var hashedPassword = Utils.PasswordHasher.ComputeSha256Hash(password);
      return user.PasswordHash == hashedPassword;
    }
  }
}