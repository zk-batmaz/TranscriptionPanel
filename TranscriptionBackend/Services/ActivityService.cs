using TranscriptionBackend.Data;
using TranscriptionBackend.Models;

namespace TranscriptionBackend.Services
{
  public class ActivityService
  {
    private readonly AppDbContext _context;

    public ActivityService(AppDbContext context)
    {
      _context = context;
    }

    public void LogActivity(int userId, string action, string? fileName = null)
    {
      var activity = new Activity
      {
        UserId = userId,
        Action = action,
        FileName = fileName ?? string.Empty,
        Timestamp = DateTime.UtcNow
      };

      _context.Activities.Add(activity);
      _context.SaveChanges();
    }

    public IEnumerable<Activity> GetAllActivities()
    {
      return _context.Activities
          .OrderByDescending(a => a.Timestamp)
          .ToList();
    }

    public IEnumerable<Activity> GetActivitiesByUser(int userId)
    {
      return _context.Activities
          .Where(a => a.UserId == userId)
          .OrderByDescending(a => a.Timestamp)
          .ToList();
    }

    // ✅ DTO'lu versiyon
    public IEnumerable<ActivityDto> GetAllActivityDtos()
    {
      return _context.Activities
          .Join(_context.Users,
              activity => activity.UserId,
              user => user.Id,
              (activity, user) => new ActivityDto
              {
                Id = activity.Id,
                UserId = user.Id,
                UserName = user.Username,
                Action = activity.Action,
                FileName = activity.FileName,
                Timestamp = activity.Timestamp
              })
          .OrderByDescending(a => a.Timestamp)
          .ToList();
    }
  }
}
