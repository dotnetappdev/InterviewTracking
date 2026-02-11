using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InterviewTracking.Api.Services;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InterviewsController : ControllerBase
{
    private readonly IInterviewService _interviewService;

    public InterviewsController(IInterviewService interviewService)
    {
        _interviewService = interviewService;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Interview>>> GetInterviews()
    {
        var userId = GetUserId();
        var interviews = await _interviewService.GetInterviewsAsync(userId);
        return Ok(interviews);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Interview>> GetInterview(Guid id)
    {
        var userId = GetUserId();
        var interview = await _interviewService.GetInterviewByIdAsync(id, userId);
        
        if (interview == null)
            return NotFound();

        return Ok(interview);
    }

    [HttpPost]
    public async Task<ActionResult<Interview>> CreateInterview([FromBody] Interview interview)
    {
        var userId = GetUserId();
        interview.UserId = userId;
        
        var createdInterview = await _interviewService.CreateInterviewAsync(interview);
        return CreatedAtAction(nameof(GetInterview), new { id = createdInterview.Id }, createdInterview);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Interview>> UpdateInterview(Guid id, [FromBody] Interview interview)
    {
        if (id != interview.Id)
            return BadRequest("ID mismatch");

        var userId = GetUserId();
        interview.UserId = userId;
        
        var updatedInterview = await _interviewService.UpdateInterviewAsync(interview, userId);
        
        if (updatedInterview == null)
            return NotFound();

        return Ok(updatedInterview);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteInterview(Guid id)
    {
        var userId = GetUserId();
        var deleted = await _interviewService.DeleteInterviewAsync(id, userId);
        
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
