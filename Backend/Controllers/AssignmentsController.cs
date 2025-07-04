using Backend.DTOs;
using Backend.Services;
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
        public async Task<IEnumerable<GetAssignmentDto>> GetAll() => await _service.GetAllAsync();

        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetById(int id)
        // {
        //     var dto = await _service.GetByIdAsync(id);
        //     return dto == null ? NotFound() : Ok(dto);
        // }

        // POST: api/assignments
        [HttpPost]
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


        // [HttpPut("{id}")]
        // public async Task<IActionResult> Update(int id, UpdateAssignmentDto dto)
        // {
        //     await _service.UpdateAsync(id, dto);
        //     return NoContent();
        // }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/with-questions")]
        public async Task<IActionResult> GetByIdWithQuestions(int id)
        {
            var dto = await _service.GetByIdWithQuestionsAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, UpdateAssignmentDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }
    }
}