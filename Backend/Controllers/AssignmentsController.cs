using System.Security.Claims;
using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _service;

        public AssignmentsController(IAssignmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<GetAssignmentDto>> GetAll() => await _service.GetAllAsync();

        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetById(int id)
        // {
        //     var dto = await _service.GetByIdAsync(id);
        //     return dto == null ? NotFound() : Ok(dto);
        // }

        // POST: api/assignments
        [HttpPost]
        [Authorize(Roles = "Manager")] // hanya Instructor yang bisa lihat
        public async Task<ActionResult<int>> CreateAsync(CreateAssignmentDto dto)
        {
            var id = await _service.CreateAssignmentWithQuestionsAsync(dto);
            return Ok("oke");
        }

        [HttpGet("{id}", Name = "GetByIdAsync")]
        public async Task<ActionResult<GetAssignmentDto>> GetByIdAsync(int id)
        {
            var assignment = await _service.GetByIdAsync(id);
            if (assignment == null) return NotFound();
            return Ok(assignment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")] // hanya Instructor yang bisa lihat
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/with-questions")]
        [Authorize]
        public async Task<IActionResult> GetByIdWithQuestions(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isInLearnerRole = User.IsInRole("Learner");
            Console.WriteLine(User);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID tidak ditemukan dalam token.");

            if (isInLearnerRole)
            {
                var dto = await _service.GetByIdWithQuestionsAsync(id, userId, "Learner");
                return dto == null ? NotFound() : Ok(dto);
            }
            else
            {
                var dtoDefault = await _service.GetByIdWithQuestionsAsync(id);
                return dtoDefault == null ? NotFound() : Ok(dtoDefault);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, UpdateAssignmentDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpGet("{assignmentId}/submissions")]
        [Authorize(Roles = "Manager")] // hanya Instructor yang bisa lihat
        public async Task<IActionResult> GetSubmissionsForAssignment(int assignmentId)
        {
            var submissions = await _service.GetSubmissionsByAssignmentIdAsync(assignmentId);
            return Ok(submissions);
        }
    }
}