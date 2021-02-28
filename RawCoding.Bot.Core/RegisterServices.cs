using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MoistBot.Models
{
    public static class RegisterServices
    {
        public static IServiceCollection AddRawCodingBot(
            this IServiceCollection services,
            params Type[] assemblyAnchors)
        {
            var types = assemblyAnchors
                .Select(x => x.Assembly)
                .SelectMany(x => x.DefinedTypes)
                .Select(x => x.GetTypeInfo());

            foreach (var type in types)
            {
                services.TryRegisterSource(type);
                services.TryRegisterHandler(type);
                services.TryRegisterAction(type);
            }

            return services;
        }

        private static void TryRegisterSource(this IServiceCollection services, TypeInfo type)
        {
            if (type.IsAbstract || type.IsInterface)
                return;

            var eventSourceType = typeof(IMessageSource);
            if (eventSourceType.IsAssignableFrom(type))
                services.AddSingleton(eventSourceType, type);
        }

        private static void TryRegisterHandler(this IServiceCollection services, TypeInfo type)
        {
            if (type.IsAbstract || type.IsInterface)
                return;

            var handlerType = typeof(MessageHandler<>);
            var handlerInterface = type.GetInterfaces().FirstOrDefault(x => x.GetGenericTypeDefinition() == handlerType);
            if (handlerInterface != null)
                services.AddTransient(handlerInterface, type);
        }


        private static void TryRegisterAction(this IServiceCollection services, TypeInfo type)
        {
            if (type.IsAbstract || type.IsInterface)
                return;

            if (typeof(ITarget).IsAssignableFrom(type))
            {
                foreach (var i in type.GetInterfaces())
                {
                    services.AddTransient(i, type);
                }
            }
        }
    }
}