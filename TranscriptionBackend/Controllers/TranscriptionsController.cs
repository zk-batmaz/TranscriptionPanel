using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TranscriptionBackend.Models;
using TranscriptionBackend.Services;

namespace TranscriptionBackend.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize(Roles = "admin,editor")]
  public class TranscriptionsController : ControllerBase
  {
    private readonly TranscriptionService _service;

    public TranscriptionsController(TranscriptionService service)
    {
      _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
      var data = _service.GetAll();
      return Ok(data);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromForm] IFormFile file, [FromForm] string text)
    {
      if (file == null || string.IsNullOrWhiteSpace(text))
        return BadRequest("Missing information.");

      var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
      if (!Directory.Exists(uploadFolder))
      {
        Directory.CreateDirectory(uploadFolder);
      }

      var filePath = Path.Combine(uploadFolder, file.FileName);
      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      var transcription = new Transcription
      {
        FileName = file.FileName,
        Text = text
      };

      _service.Add(transcription);

      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
      _service.LogActivity(userId, $"New tranxcription added: {file.FileName}");

      return Ok(transcription);
    }


    [HttpPost("upload")]
    public async Task<IActionResult> UploadAudio([FromForm] IFormFile audioFile)
    {
      if (audioFile == null || audioFile.Length == 0)
        return BadRequest("File not found or empty.");

      var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
      if (!Directory.Exists(uploadFolder))
      {
        Directory.CreateDirectory(uploadFolder);
      }

      var filePath = Path.Combine(uploadFolder, audioFile.FileName);

      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await audioFile.CopyToAsync(stream);
      }

      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
      _service.LogActivity(userId, $"The audio file was uploaded successfully.: {audioFile.FileName}");

      return Ok(new { message = "The file was uploaded successfully.", fileName = audioFile.FileName });
    }

    [HttpPost("upload-and-transcribe")]
    public async Task<IActionResult> UploadAndTranscribe([FromForm] IFormFile audioFile)
    {
      if (audioFile == null || audioFile.Length == 0)
        return BadRequest("File not found or empty.");

      var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
      if (!Directory.Exists(uploadFolder))
      {
        Directory.CreateDirectory(uploadFolder);
      }

      var filePath = Path.Combine(uploadFolder, audioFile.FileName);

      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await audioFile.CopyToAsync(stream);
      }

      var transcription = _service.TranscribeAudio(audioFile.FileName);

      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
      _service.LogActivity(userId, $"Audio file uploaded and transcription created: {audioFile.FileName}");

      return Ok(transcription);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

      _service.LogActivity(userId, $"Transcription displayed: ID {id}");

      var result = _service.GetWithActivities(id, userId);
      if (result == null)
        return NotFound();

      return Ok(result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Transcription updatedTranscription)
    {
      var existing = _service.GetById(id);
      if (existing == null)
        return NotFound();

      updatedTranscription.Id = id;
      _service.Update(updatedTranscription);

      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
      _service.LogActivity(userId, $"Transcription updated: {updatedTranscription.FileName}");

      return Ok(updatedTranscription);
    }
  }
}
