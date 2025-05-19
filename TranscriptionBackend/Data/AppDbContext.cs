using Microsoft.EntityFrameworkCore;
using TranscriptionBackend.Models;

namespace TranscriptionBackend.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Transcription> Transcriptions { get; set; }
    public DbSet<Activity> Activities { get; set; }
  }
}
