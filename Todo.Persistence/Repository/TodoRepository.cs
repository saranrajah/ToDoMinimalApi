using Microsoft.EntityFrameworkCore;
using TodoApp.Application.DTO;
using TodoApp.Application.Interface;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Context;

namespace TodoApp.Infrastructure.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDb _todoDb;

        public TodoRepository(TodoDb todoDb)
        {
            _todoDb = todoDb;
        }

        public async Task<TodoItemDTO> CreateTodo(TodoItemDTO todo)
        {
            var todoItem = new Todo
            {
                IsComplete = todo.IsComplete,
                Name = todo.Name
            };

            _todoDb.Todos.Add(todoItem);
            await _todoDb.SaveChangesAsync();

            return todo = new TodoItemDTO(todoItem);
        }

        public async Task DeleteTodo(int id)
        {
            if (await _todoDb.Todos.FindAsync(id) is Todo todo)
            {
                _todoDb.Todos.Remove(todo);
                await _todoDb.SaveChangesAsync();
            }
        }

        public async Task<List<TodoItemDTO>> GetAllTodos()
        {
            return await _todoDb.Todos.Select(x => new TodoItemDTO(x)).ToListAsync();
        }

        public async Task<List<TodoItemDTO>> GetCompleteTodos()
        {
            return await _todoDb.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDTO(x)).ToListAsync();
        }

        public async Task<TodoItemDTO?> GetTodo(int id)
        {
            return await _todoDb.Todos.FindAsync(id) is Todo todo ? new TodoItemDTO(todo) : null;
        }

        public async Task UpdateTodo(int id, TodoItemDTO todoItemDTO)
        {
            var todo = await _todoDb.Todos.FindAsync(id);

            if (todo is null) throw new ApplicationException("Not Found");

            todo.Name = todoItemDTO.Name;
            todo.IsComplete = todoItemDTO.IsComplete;

            await _todoDb.SaveChangesAsync();
        }
    }
}
