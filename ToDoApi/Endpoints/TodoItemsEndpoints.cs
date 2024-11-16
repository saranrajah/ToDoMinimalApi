using Microsoft.EntityFrameworkCore;
using TodoApp.Application.DTO;
using TodoApp.Application.Interface;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Context;

namespace ToDoApi.Endpoints
{
    public static class TodoItemsEndpoints
    {
        public static void RegisterTodoItemsEndpoints(this WebApplication app)
        {
            RouteGroupBuilder todoItemsV1 = app.MapGroup("/todoitems/v1");

            todoItemsV1.MapGet("/", GetAllTodos);
            todoItemsV1.MapGet("/complete", GetCompleteTodos);
            todoItemsV1.MapGet("/{id}", GetTodo);
            todoItemsV1.MapPost("/", CreateTodo);
            todoItemsV1.MapPut("/{id}", UpdateTodo);
            todoItemsV1.MapDelete("/{id}", DeleteTodo);

            RouteGroupBuilder todoItemsV2 = app.MapGroup("/todoitems/v2");

            todoItemsV2.MapGet("/", GetAllTodosV2);
            todoItemsV2.MapGet("/complete", GetCompleteTodosV2);
            todoItemsV2.MapGet("/{id}", GetTodoV2);
            todoItemsV2.MapPost("/", CreateTodoV2);
            todoItemsV2.MapPut("/{id}", UpdateTodoV2);
            todoItemsV2.MapDelete("/{id}", DeleteTodoV2);
        }

        #region<V1>

        static async Task<IResult> GetAllTodos(TodoDb db)
        {
            return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync());
        }

        static async Task<IResult> GetCompleteTodos(TodoDb db)
        {
            return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDTO(x)).ToListAsync());
        }

        static async Task<IResult> GetTodo(int id, TodoDb db)
        {
            return await db.Todos.FindAsync(id)
                is Todo todo
                    ? TypedResults.Ok(new TodoItemDTO(todo))
                    : TypedResults.NotFound();
        }

        static async Task<IResult> CreateTodo(TodoItemDTO todoItemDTO, TodoDb db)
        {
            var todoItem = new Todo
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };

            db.Todos.Add(todoItem);
            await db.SaveChangesAsync();

            todoItemDTO = new TodoItemDTO(todoItem);

            return TypedResults.Created($"/todoitems/{todoItem.Id}", todoItemDTO);
        }

        static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDTO, TodoDb db)
        {
            var todo = await db.Todos.FindAsync(id);

            if (todo is null) return TypedResults.NotFound();

            todo.Name = todoItemDTO.Name;
            todo.IsComplete = todoItemDTO.IsComplete;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<IResult> DeleteTodo(int id, TodoDb db)
        {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        #endregion

        #region<V2>

        static async Task<IResult> GetAllTodosV2(ITodoService todoService)
        {
            return TypedResults.Ok(await todoService.GetAllTodos());
        }

        static async Task<IResult> GetCompleteTodosV2(ITodoService todoService)
        {
            return TypedResults.Ok(await todoService.GetCompleteTodos());
        }

        static async Task<IResult> GetTodoV2(int id, ITodoService todoService)
        {
            return await todoService.GetTodo(id) is TodoItemDTO todo
                    ? TypedResults.Ok(todo)
                    : TypedResults.NotFound();
        }

        static async Task<IResult> CreateTodoV2(TodoItemDTO todoItemDTO, ITodoService todoService)
        {
            todoItemDTO = await todoService.CreateTodo(todoItemDTO);

            return TypedResults.Created($"/todoitems/{todoItemDTO.Id}", todoItemDTO);
        }

        static async Task<IResult> UpdateTodoV2(int id, TodoItemDTO todoItemDTO, ITodoService todoService)
        {
            await todoService.UpdateTodo(id, todoItemDTO);

            return TypedResults.NoContent();
        }

        static async Task<IResult> DeleteTodoV2(int id, ITodoService todoService)
        {
            await todoService.DeleteTodo(id);

            return TypedResults.NotFound();
        }

        #endregion
    }
}
