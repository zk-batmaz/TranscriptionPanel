using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TranscriptionBackend.Services;
using TranscriptionBackend.Data;
using TranscriptionBackend.Models;

namespace TranscriptionBackend.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize(Roles = "admin")]
  public class ActivityController : ControllerBase
  {
    private readonly ActivityService _activityService;
    private readonly AppDbContext _context;

    public ActivityController(ActivityService activityService, AppDbContext context)
    {
      _activityService = activityService;
      _context = context;
    }

    [HttpGet]
    public IActionResult GetAllActivities()
    {
      return Ok(_activityService.GetAllActivities());
    }

    [HttpGet("{userId}")]
    public IActionResult GetActivitiesByUser(int userId)
    {
      return Ok(_activityService.GetActivitiesByUser(userId));
    }
    [HttpGet("with-details")]
    public IActionResult GetAllWithDetails()
    {
      var activities = _activityService.GetAllActivityDtos();
      return Ok(activities);
    }
    [HttpPost]
    public IActionResult AddActivity([FromBody] ActivityDto dto)
    {
      var activity = new Activity
      {
        UserId = dto.UserId,
        ActivityMessage = dto.ActivityMessage,
        FileName = dto.FileName,
        Action = dto.Action,
        Timestamp = DateTime.UtcNow
      };

      _context.Activities.Add(activity);
      _context.SaveChanges();

      return Ok();
    }
  }
}
