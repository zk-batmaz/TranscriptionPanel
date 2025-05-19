using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranscriptionBackend.Models
{
  public class Activity
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ActivityMessage { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
  }
}
