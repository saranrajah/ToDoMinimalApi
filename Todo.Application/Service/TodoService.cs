using TodoApp.Application.DTO;
using TodoApp.Application.Interface;

namespace TodoApp.Application.Service
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<TodoItemDTO> CreateTodo(TodoItemDTO todo)
        {
            return await _todoRepository.CreateTodo(todo);
        }

        public async Task DeleteTodo(int id)
        {
            await _todoRepository.DeleteTodo(id);
        }

        public async Task<List<TodoItemDTO>> GetAllTodos()
        {
            return await _todoRepository.GetAllTodos();
        }

        public async Task<List<TodoItemDTO>> GetCompleteTodos()
        {
            return await _todoRepository.GetCompleteTodos();
        }

        public async Task<TodoItemDTO?> GetTodo(int id)
        {
            return await _todoRepository.GetTodo(id);
        }

        public async Task UpdateTodo(int id, TodoItemDTO todoItemDTO)
        {
            await _todoRepository.UpdateTodo(id, todoItemDTO);
        }
    }
}
