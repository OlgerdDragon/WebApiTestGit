using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApiTest.Data;
using WebApiTest.Models;
using WebApiTest.Services.ToDoService.Dto;

namespace WebApiTest.Services.ToDoService
{
    public class ToDoService : IToDoService
    {
        private readonly TodoContext _context;

        public ToDoService(TodoContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItemDto>> GetToDoItemsAsync()
        {
            return await _context.TodoItems.Select(i => new TodoItemDto
            {
                Id = i.Id,
                Name = i.Name,
                IsComplete = i.IsComplete
            }).ToListAsync();
        }
        public async Task<TodoItem> FindAsync(long id)
        {
            return await _context.TodoItems.FindAsync(id);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Add(TodoItem todoItem)
        {
             _context.TodoItems.Add(todoItem);
                    }
        public void Remove(TodoItem todoItem)
        {
            _context.TodoItems.Remove(todoItem);
        }
        public bool TodoItemExists(long id) =>
             _context.TodoItems.Any(e => e.Id == id);
    }
}