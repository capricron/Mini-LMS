// Controllers/LearnerController.cs
using System.Security.Claims;
using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LearnerController : ControllerBase
    {
        private readonly ILearnerService _service;

        public LearnerController(ILearnerService service) => _service = service;

        [HttpPost("submit")]
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> SubmitAnswers([FromBody] SubmitAnswerRequestDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                var result = await _service.SubmitAnswersAsync(dto, userId); // Kirim userId
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("result/{assignmentId}")]
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> GetSubmissionResult(int assignmentId)
        {
            var learnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Console.WriteLine(learnerId);
            if (string.IsNullOrEmpty(learnerId)) return Unauthorized();
            var result = await _service.GetSubmissionResultAsync(learnerId, assignmentId);
            return result == null ? NotFound("Belum ada jawaban.") : Ok(result);
        }
    }
}