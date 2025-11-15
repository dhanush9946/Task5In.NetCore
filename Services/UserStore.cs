using System.Collections.Concurrent;
using TaskManagement.Models;
namespace TaskManagement.Services
{
    public class UserStore
    {
        public List<User> Users { get; } = new();
        public List<TaskItem> Tasks { get; } = new();
    }
}
