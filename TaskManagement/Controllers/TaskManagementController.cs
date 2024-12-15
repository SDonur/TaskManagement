using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Dtos;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskManagementController : ControllerBase
    {
        private readonly WorkDbContext _context;

        public TaskManagementController(WorkDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            return Ok(await _context.Works.Select(x =>
                                                  new WorkDto 
                                                  { 
                                                      Id = x.Id, 
                                                      Title = x.Title,
                                                      IsCompleted = x.IsCompleted
                                                  }).AsNoTracking().ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveTask([FromBody] Work task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            task.CreatedDate = DateTime.Now;

            _context.Works.Add(task);

            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, Work work)
        {
            var getWork = await _context.Works.FindAsync(id);
            if (getWork == null)
            {
                return NotFound();
            }

            getWork.IsCompleted = work.IsCompleted;
            getWork.Title = work.Title;
            getWork.ModifiedDate = DateTime.Now;

            _context.Entry(getWork).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var work = await _context.Works.FindAsync(id);
            if (work == null)
            {
                return NotFound();
            }

            _context.Works.Remove(work);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}