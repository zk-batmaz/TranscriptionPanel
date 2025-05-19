using TranscriptionBackend.Data;
using TranscriptionBackend.Models;

namespace TranscriptionBackend.Services
{
  public class TranscriptionService
  {
    private readonly AppDbContext _context;

    public TranscriptionService(AppDbContext context)
    {
      _context = context;
    }

    public IEnumerable<TranscriptionDto> GetAll()
    {
      return _context.Transcriptions
          .Select(t => new TranscriptionDto
          {
            Id = t.Id,
            Text = t.Text,
            FileName = t.FileName
          })
          .ToList();
    }


    public Transcription? GetById(int id)
    {
      return _context.Transcriptions.FirstOrDefault(t => t.Id == id);
    }

    public void Add(Transcription transcription)
    {
      transcription.CreatedAt = DateTime.UtcNow;
      _context.Transcriptions.Add(transcription);
      _context.SaveChanges();
    }

    public void Update(Transcription updated)
    {
      var existing = _context.Transcriptions.FirstOrDefault(t => t.Id == updated.Id);
      if (existing != null)
      {
        existing.Text = updated.Text;
        _context.SaveChanges();
      }
    }

    public Transcription TranscribeAudio(string fileName)
    {
      var fakeText = "Bu bir test transkripsiyondur";

      var transcription = new Transcription
      {
        FileName = fileName,
        Text = fakeText,
        CreatedAt = DateTime.UtcNow
      };

      _context.Transcriptions.Add(transcription);
      _context.SaveChanges();

      return transcription;
    }

    public void LogActivity(int userId, string message, string action = "", string fileName = "")
    {
      var activity = new Activity
      {
        UserId = userId,
        ActivityMessage = message,
        Action = action,
        FileName = fileName,
        Timestamp = DateTime.UtcNow
      };

      _context.Activities.Add(activity);
      _context.SaveChanges();
    }
    public TranscriptionWithActivitiesDto GetWithActivities(int transcriptionId, int userId)
    {
      var transcription = _context.Transcriptions.Find(transcriptionId);
      if (transcription == null)
        return null;

      var activities = _context.Activities
          .Where(a => a.UserId == userId && a.ActivityMessage.Contains(transcription.FileName))
          .OrderByDescending(a => a.Timestamp)
          .Select(a => new ActivityDto
          {
            ActivityMessage = a.ActivityMessage,
            Timestamp = a.Timestamp
          })
          .ToList();

      return new TranscriptionWithActivitiesDto
      {
        Id = transcription.Id,
        FileName = transcription.FileName,
        Text = transcription.Text,
        CreatedAt = transcription.CreatedAt,
        Activities = activities
      };
    }
  }
}
