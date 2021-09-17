using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Services.ToDoService.Dto;

namespace WebApiTest.Services.ToDoService
{
    public interface IToDoService
    {
        Task<List<TodoItemDto>> GetToDoItemsAsync();
        public  Task<TodoItem> FindAsync(long id);
        public Task<int> SaveChangesAsync();
        public void Add(TodoItem todoItem);
        public void Remove(TodoItem todoItem);
        public bool TodoItemExists(long id);
    }
}