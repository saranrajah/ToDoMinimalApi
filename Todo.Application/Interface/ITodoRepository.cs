using TodoApp.Application.DTO;

namespace TodoApp.Application.Interface
{
    public interface ITodoRepository
    {
        Task<List<TodoItemDTO>> GetAllTodos();

        Task<List<TodoItemDTO>> GetCompleteTodos();

        Task<TodoItemDTO?> GetTodo(int id);

        Task<TodoItemDTO> CreateTodo(TodoItemDTO todo);

        Task UpdateTodo(int id, TodoItemDTO todo);

        Task DeleteTodo(int id);
    }
}
