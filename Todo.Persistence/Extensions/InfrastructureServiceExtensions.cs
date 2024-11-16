using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TodoApp.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interface;
using TodoApp.Application.Service;
using TodoApp.Infrastructure.Repository;

namespace TodoApp.Persistence.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));

            services.AddTransient<ITodoRepository, TodoRepository>();
            services.AddTransient<ITodoService, TodoService>();

            return services;
        }
    }
}
