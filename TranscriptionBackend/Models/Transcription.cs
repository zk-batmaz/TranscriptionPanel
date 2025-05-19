namespace TranscriptionBackend.Models
{
  public class Transcription
  {
    public int Id { get; set; }
    public string? FileName { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}
