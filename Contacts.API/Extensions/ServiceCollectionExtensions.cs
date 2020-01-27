using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static ICollection<Type> _exportedTypeCache;

        private static ICollection<Type> ExportedTypeCache => _exportedTypeCache ??= GetExportedTypes();

        private static ICollection<Type> GetExportedTypes()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.StartsWith(nameof(Contacts)))
                .SelectMany(a => a.GetExportedTypes())
                .ToList();
        }

        /// <summary>
        /// Registers generic types with all its subtypes.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="interfaceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        /// <param name="registerBaseTypes"></param>
        public static void AddDerived(this IServiceCollection collection, Type interfaceType, Type implementationType, ServiceLifetime lifetime, bool registerBaseTypes = true)
        {
            if (registerBaseTypes) collection.Add(new ServiceDescriptor(interfaceType, implementationType, lifetime));

            var derivedImplementations = ExportedTypeCache
                .Where(s => s.BaseType != typeof(object) && s.BaseType != null
                                                         && (s.BaseType.IsGenericType ? s.BaseType.GetGenericTypeDefinition() : s.BaseType) == implementationType);

            var derivedInterfaces = ExportedTypeCache
                .Where(t => t.IsInterface
                            && t.GetInterfaces().Any(i => (i.IsGenericType ? i.GetGenericTypeDefinition() : i) == interfaceType))
                .ToArray();

            foreach (var derivedImplementationsType in derivedImplementations)
            {
                var derivedInterface = derivedInterfaces.FirstOrDefault(i => i.IsAssignableFrom(derivedImplementationsType));

                // Doesn't work for implementations that are generic and extended by other generic implementations and interfaces.
                // Basically if class<T> implements an interface and derives from implementationtype, then IsAssignableFrom fails here
                // even though types match.
                if (derivedInterface == null)
                {
                    //throw new ArgumentException($"Failed to find interface for type {derivedImplementationsType.FullName}");
                    continue;
                }

                collection.Add(new ServiceDescriptor(derivedInterface, derivedImplementationsType, lifetime));
            }
        }
    }
}
