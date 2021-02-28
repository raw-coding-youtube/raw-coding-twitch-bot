using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MoistBot.Models
{
    public static class RegisterServices
    {
        public static IServiceCollection AddRawCodingBot(
            this IServiceCollection services,
            Assembly assembly)
        {
            foreach (var type in assembly.DefinedTypes.Select(x => x.GetTypeInfo()))
            {
                services.TryRegisterSource(type);
                services.TryRegisterHandler(type);
            }

            return services;
        }

        private static void TryRegisterSource(this IServiceCollection services, TypeInfo type)
        {
            if (type.IsAbstract || type.IsInterface)
                return;

            var eventSourceType = typeof(IEventSource);
            if (eventSourceType.IsAssignableFrom(type))
            {
                var lifetime = type.GetCustomAttribute<LifetimeAttribute>();
                if (lifetime == null)
                {
                    services.AddTransient(eventSourceType, type);
                }
                else if (lifetime.LifeTime == ServiceLifeTime.Singleton)
                {
                    services.AddSingleton(eventSourceType, type);
                }
            }
        }

        private static void TryRegisterHandler(this IServiceCollection services, TypeInfo type)
        {
            if (type.IsAbstract || type.IsInterface)
                return;

            var handlerType = typeof(IEventHandler<>);
            var handlerInterface = type.GetInterfaces().FirstOrDefault(x => x.GetGenericTypeDefinition() == handlerType);
            if (handlerInterface != null)
            {
                var lifetime = type.GetCustomAttribute<LifetimeAttribute>();
                if (lifetime == null)
                {
                    services.AddTransient(handlerInterface, type);
                }
                else if (lifetime.LifeTime == ServiceLifeTime.Singleton)
                {
                    var exists = services.Any(x => x.ServiceType == type);
                    if (exists)
                    {
                        services.AddSingleton(handlerInterface, sp => sp.GetService(type));
                        return;
                    }

                    services.AddSingleton(handlerInterface, type);
                }
            }
        }
    }
}