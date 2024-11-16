using ToDoApi.Endpoints;
using TodoApp.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddInfrastructureServices();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddTransient<ITodoRepository, TodoRepository>();
//builder.Services.AddTransient<ITodoService, TodoService>();

var app = builder.Build();

app.RegisterTodoItemsEndpoints();

app.Run();