using TranscriptionBackend.Models;
using TranscriptionBackend.Utils;
using TranscriptionBackend.Data;

namespace TranscriptionBackend.Extensions
{
  public static class DbSeeder
  {
    public static void SeedUsers(AppDbContext context)
    {
      if (!context.Users.Any())
      {
        var admin = new User
        {
          Username = "admin",
          PasswordHash = PasswordHasher.ComputeSha256Hash("admin123"),
          Role = "admin"
        };

        var editor = new User
        {
          Username = "editor",
          PasswordHash = PasswordHasher.ComputeSha256Hash("editor123"),
          Role = "editor"
        };

        context.Users.AddRange(admin, editor);
        context.SaveChanges();
      }
    }
  }
}
