using System;
using System.Collections.Generic;
using TranscriptionBackend.Models;

namespace TranscriptionBackend.Models
{
    public class TranscriptionWithActivitiesDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<ActivityDto> Activities { get; set; } = new();
    }
}
