using JobApplicationTracker.API.Exceptions;
using JobApplicationTracker.API.Models;
using JobApplicationTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationService _jobApplicationService;
        public JobApplicationsController(IJobApplicationService jobApplicationService)
        {
            _jobApplicationService = jobApplicationService;
        }

        [HttpGet]

        public async Task<ActionResult<PaginatedResult<JobApplication>>> GetApplications(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var paginatedResult = await _jobApplicationService.GetPaginatedApplicationsAsync(pageNumber, pageSize);

            return Ok(paginatedResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var application = await _jobApplicationService.GetByIdAsync(id);
            return application == null ? NotFound() : Ok(application);

        }

        [HttpPost]
        public async Task<IActionResult> Create(JobApplication jobApplication)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = 400, error = "invalid fields in job application data.", details = ModelState });

            var created = await _jobApplicationService.CreateAsync(jobApplication);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, JobApplication application)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = 400, error = "invalid fields in job application data.", details = ModelState });

            if (id != application.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var updated = await _jobApplicationService.UpdateAsync(id, application);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _jobApplicationService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}