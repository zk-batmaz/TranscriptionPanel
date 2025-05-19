namespace TranscriptionBackend.Models
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string ActivityMessage { get; set; } = string.Empty;

    }
}
