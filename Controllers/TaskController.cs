using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController:ControllerBase
    {
        private readonly UserStore store;

        public TaskController(UserStore _store)
        {
            store = _store;
        }

        [HttpGet]
        public IActionResult GetTasks() => Ok(store.Tasks);
        [HttpPost]
        public IActionResult CreateTask(CreateTaskDto dto)
        {
            var username = User.Identity!.Name!;

            var task = new TaskItem
            {
                Id = store.Tasks.Count + 1,
                Title = dto.Title,
                Description = dto.Description,
                CreatedBy = username
            };

            store.Tasks.Add(task);
            return Ok(task);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateTask(int id, UpdateTaskDto dto)
        {
            var task = store.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            task.Title = dto.Title ?? task.Title;
            task.Description = dto.Description ?? task.Description;

            return Ok(task);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public IActionResult DeleteTask(int id)
        {
            var task = store.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            store.Tasks.Remove(task);
            return Ok("Deleted");
        }
    }
    public record CreateTaskDto(string Title,string? Description);
    public record UpdateTaskDto(string? Title,string? Description);
}

