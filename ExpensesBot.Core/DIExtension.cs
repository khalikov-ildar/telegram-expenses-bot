using System.Reflection;
using ExpensesBot.Core.Interfaces;
using ExpensesBot.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesBot.Core;

public static class DiExtension
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly(); 
        
        foreach (var type in assembly.GetTypes()
                     .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                         .Any(i => i.IsGenericType && 
                                   i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))))
        {
            var interfaceType = type.GetInterfaces()
                .First(i => i.IsGenericType && 
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));
            
            services.AddScoped(interfaceType, type);
        }
        return services;
    }
}